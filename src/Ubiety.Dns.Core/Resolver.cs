/*
 * Copyright 2020 Dieter Lunn
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

using Ubiety.Dns.Core.Common;
using Ubiety.Logging.Core;

namespace Ubiety.Dns.Core;

/// <summary>
/// Represents a DNS resolver that performs DNS queries and manages response caching
/// with support for customizable transport protocols and query configurations.
/// </summary>
public partial class Resolver
{
    private readonly IUbietyLogger _logger = UbietyLogger.Get<Resolver>();
    private readonly Dictionary<Question, Response> _responseCache;
    private readonly List<IPEndPoint> _dnsServers;

    private readonly bool _useCache;

    /// <summary> Initializes a new instance of the <see cref="Resolver" /> class. </summary>
    /// <param name="dnsServers"> Set of DNS servers to use for resolution. </param>
    internal Resolver(IEnumerable<IPEndPoint> dnsServers)
    {
#pragma warning disable SA1010 // Opening square brackets should be spaced correctly
        _responseCache = [];
        _dnsServers = [.. dnsServers];
#pragma warning restore SA1010 // Opening square brackets should be spaced correctly

        TransportType = TransportType.Tcp;
    }

    /// <summary>
    /// Gets the version information for the current assembly.
    /// </summary>
    public static string Version => Assembly.GetExecutingAssembly()
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
        ?.InformationalVersion;

    /// <summary>
    /// Gets or initializes the timeout duration, in milliseconds, for DNS queries.
    /// This value determines how long the resolver waits for a response before timing out.
    /// </summary>
    public int Timeout { get; init; }

    /// <summary>
    /// Gets or initializes the number of retry attempts for DNS queries in case of failure.
    /// </summary>
    public int Retries { get; init; }

    /// <summary>
    /// Gets a value indicating whether DNS recursion is enabled for the resolver.
    /// When enabled, recursive DNS queries are performed, allowing the resolver to fetch complete DNS responses.
    /// </summary>
    public bool Recursion { get; init; }

    /// <summary>
    /// Gets or sets the transport protocol used for DNS queries.
    /// Determines whether queries are sent over UDP or TCP.
    /// </summary>
    public TransportType TransportType { get; set; }

    /// <summary>
    /// Gets a value indicating whether the DNS resolver should use caching for responses.
    /// When set to <c>false</c>, the existing cache is cleared.
    /// </summary>
    public bool UseCache
    {
        get => _useCache;

        init
        {
            _useCache = value;
            if (_useCache)
            {
                return;
            }

            ClearCache();
        }
    }

    /// <summary>
    /// Converts the given IP address into its corresponding reverse DNS ARPA address.
    /// </summary>
    /// <param name="ip">The IP address to be converted into an ARPA address.</param>
    /// <returns>A string representing the reverse DNS ARPA address for the provided IP address. If the address family is unsupported, returns "?".</returns>
    public static string GetArpaFromIp(IPAddress ip)
    {
        ArgumentNullException.ThrowIfNull(ip);

        switch (ip.AddressFamily)
        {
            case AddressFamily.InterNetwork:
            {
                var sb = new StringBuilder();
                sb.Append("in-addr.arpa.");
                foreach (var b in ip.GetAddressBytes())
                {
                    sb.Insert(0, $"{b}.");
                }

                return sb.ToString();
            }

            case AddressFamily.InterNetworkV6:
            {
                var sb = new StringBuilder();
                sb.Append("ip6.arpa.");
                foreach (var b in ip.GetAddressBytes())
                {
                    sb.Insert(0, $"{(b >> 4) & 0xf:x}.");
                    sb.Insert(0, $"{b & 0xf:x}.");
                }

                return sb.ToString();
            }

            default:
                return "?";
        }
    }

    /// <summary>
    /// Converts an enumerator string to its corresponding ARPA address.
    /// </summary>
    /// <param name="enumerator">The enumerator representing a numerical address to convert.</param>
    /// <returns>The resulting ARPA address as a string.</returns>
    public static string GetArpaFromEnumerator(string enumerator)
    {
        var sb = new StringBuilder();
        var number = Number().Replace(enumerator, string.Empty);
        sb.Append("e164.arpa.");
        foreach (var c in number)
        {
            sb.Insert(0, $"{c}.");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Clears all entries in the DNS response cache.
    /// </summary>
    public void ClearCache()
    {
        lock (_responseCache)
        {
            _responseCache.Clear();
        }
    }

    /// <summary> Sends a DNS query for the specified domain name, question type, and question class. </summary>
    /// <param name="domainName"> The domain name to resolve. </param>
    /// <param name="questionType"> The type of DNS query (e.g., A, AAAA, MX). </param>
    /// <param name="questionClass"> The class of DNS query (e.g., IN for Internet). </param>
    /// <returns> A <see cref="Response"/> containing the result of the DNS query. </returns>
    /// <exception cref="InvalidOperationException">The resolver has no DNS servers configured.</exception>
    public Response Query(string domainName, QuestionType questionType, QuestionClass questionClass = QuestionClass.IN)
    {
        if (_dnsServers.Count <= 0)
        {
            _logger.Error("No DNS servers to query.");
            throw new InvalidOperationException("The resolver has no DNS servers configured.");
        }

        _logger.Debug($"Received {questionType} query for {domainName}");

        var question = new Question(domainName, questionType, questionClass);
        var response = SearchInCache(question);
        if (response != null)
        {
            _logger.Debug("Returning cached response...");
            return response;
        }

        _logger.Debug("Sending request to server...");
        var request = new Request();
        request.AddQuestion(question);
        return GetResponse(request);
    }

    [GeneratedRegex("[^0-9]")]
    private static partial Regex Number();

    private static void WriteRequest(BufferedStream stream, Request request)
    {
        var data = request.GetBytes();
        stream.WriteByte((byte)((data.Length >> 8) & 0xFF));
        stream.WriteByte((byte)(data.Length & 0xFF));
        stream.Write(data, 0, data.Length);
        stream.Flush();
    }

    private static ushort GetUniqueId()
    {
        using var rng = RandomNumberGenerator.Create();
        var rand = new byte[16];
        rng.GetBytes(rand);
        var id = BitConverter.ToUInt16(rand, 0);

        return id;
    }

    private Response GetResponse(Request request)
    {
        request.Header.Id = GetUniqueId();
        request.Header.Recursion = Recursion;

        return TransportType switch
        {
            TransportType.Udp => UdpRequest(request),
            TransportType.Tcp => TcpRequest(request),
            _ => throw new InvalidOperationException(),
        };
    }

    private Response SearchInCache(Question question)
    {
        _logger.Debug("Searching cache for question...");
        if (!_useCache)
        {
            return null;
        }

        Response response;

        lock (_responseCache)
        {
            if (!_responseCache.TryGetValue(question, out Response value))
            {
                _logger.Debug("Question does not exist in cache.");
                return null;
            }

            response = value;
        }

        _logger.Debug("Found question in cache...");
        return response.ResourceRecords.Any(rr => rr.IsExpired(response.TimeStamp)) ? null : response;
    }

    private void AddToCache(Response response)
    {
        if (!_useCache)
        {
            return;
        }

        // No question, no caching
        if (response.Questions.Count == 0)
        {
            return;
        }

        // Only cached non-error responses
        if (response.Header.ResponseCode != ResponseCode.NoError)
        {
            return;
        }

        var question = response.Questions[0];

        lock (_responseCache)
        {
            _responseCache.Remove(question);

            _responseCache.Add(question, response);
        }
    }

    private Response UdpRequest(Request request)
    {
        _logger.Debug("Starting UDP request...");
        for (var attempts = 0; attempts < Retries; attempts++)
        {
            _logger.Debug($"Attempt {attempts + 1} of {Retries}...");
            foreach (var server in _dnsServers)
            {
                _logger.Debug($"Connecting to server {server.Address}...");
                using var client = new UdpClient(AddressFamily.InterNetworkV6);
                client.Client.DualMode = true;

                // Without this Receive blocks indefinitely and Timeout applies to TCP only.
                client.Client.ReceiveTimeout = Timeout;
                client.Client.SendTimeout = Timeout;

                try
                {
                    var sendBytes = request.GetBytes();
                    client.Send(sendBytes, sendBytes.Length, server);
                    var remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    var data = client.Receive(ref remoteEndPoint);

                    var response = new Response(server, data);
                    AddToCache(response);

                    client.Close();
                    return response;
                }
                catch (SocketException exception)
                {
                    _logger.Error(exception, $"Connection to nameserver {server.Address} failed");
                }
            }
        }

        var responseTimeout = new Response(true);
        return responseTimeout;
    }

    private Response TcpRequest(Request request)
    {
        _logger.Debug("Starting TCP request...");
        for (var attempts = 0; attempts < Retries; attempts++)
        {
            _logger.Debug($"Attempt {attempts + 1} of {Retries}...");
            foreach (var server in _dnsServers)
            {
                _logger.Debug($"Connecting to {server.Address}...");

                try
                {
                    using var client = Socket.OSSupportsIPv6 ? new TcpClient(AddressFamily.InterNetworkV6)
                        {
                            ReceiveTimeout = Timeout,
                            SendTimeout = Timeout,
                            Client = { DualMode = true },
                        }
                        : new TcpClient(AddressFamily.InterNetwork)
                        {
                            ReceiveTimeout = Timeout,
                            SendTimeout = Timeout,
                        };

                    client.Connect(server.Address, server.Port);

                    if (!client.Connected)
                    {
                        client.Close();
                        _logger.Error($"Connection to nameserver {server.Address} failed.");
                        continue;
                    }

                    using var stream = new BufferedStream(client.GetStream());

                    _logger.Debug("Sending request to server...");
                    WriteRequest(stream, request);

                    return ReceiveResponse(stream, server);
                }
                catch (Exception e) when (e is SocketException or IOException)
                {
                    // A read timeout surfaces as an IOException wrapping a SocketException, and a
                    // truncated response as an EndOfStreamException. Neither should abort the run:
                    // fall through and give the remaining servers and attempts a chance.
                    _logger.Error(e, $"Request to nameserver {server.Address} failed");
                }
            }
        }

        _logger.Debug("Connection timed out");
        var responseTimeout = new Response(true);
        return responseTimeout;
    }

    private Response ReceiveResponse(Stream stream, IPEndPoint server)
    {
        var transferResponse = new Response();
        var soa = 0;
        var messageSize = 0;

        while (true)
        {
            var lengthHigh = stream.ReadByte();
            var lengthLow = stream.ReadByte();
            if (lengthHigh < 0 || lengthLow < 0)
            {
                _logger.Error($"Connection to nameserver {server.Address} closed before sending a length prefix");
                throw new SocketException();
            }

            var length = (lengthHigh << 8) | lengthLow;
            if (length <= 0)
            {
                _logger.Error($"Connection to nameserver {server.Address} failed");
                throw new SocketException();
            }

            messageSize += length;

            // ReadExactly loops until the full message arrives; a plain Read can return a
            // partial buffer and would leave the remainder to be parsed as zeroed bytes.
            var data = new byte[length];
            stream.ReadExactly(data);

            _logger.Debug("Building response...");
            var response = new Response(server, data);

            if (response.Header.ResponseCode != ResponseCode.NoError)
            {
                _logger.Debug($"Error from server - {response.Header.ResponseCode}");
                return response;
            }

            if (response.Questions[0].QuestionType != QuestionType.AXFR)
            {
                AddToCache(response);
                return response;
            }

            if (transferResponse.Questions.Count == 0)
            {
                transferResponse.Questions.AddRange(response.Questions);
            }

            transferResponse.Answers.AddRange(response.Answers);
            transferResponse.Authorities.AddRange(response.Authorities);
            transferResponse.Additional.AddRange(response.Additional);

            if (response.Answers[0].Type == RecordType.SOA)
            {
                soa++;
            }

            if (soa != 2)
            {
                continue;
            }

            transferResponse.Header.QuestionCount = (ushort)transferResponse.Questions.Count;
            transferResponse.Header.AnswerCount = (ushort)transferResponse.Answers.Count;
            transferResponse.Header.NameserverCount = (ushort)transferResponse.Authorities.Count;
            transferResponse.Header.AdditionalRecordsCount = (ushort)transferResponse.Additional.Count;
            transferResponse.MessageSize = messageSize;

            return transferResponse;
        }
    }
} // class
