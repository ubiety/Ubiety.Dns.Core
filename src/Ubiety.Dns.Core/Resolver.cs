/*
 * Copyright © 2020-2026 Dieter (coder2000) Lunn
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
using System.Threading;
using System.Threading.Tasks;

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
        _responseCache = [];
        _dnsServers = [.. dnsServers];

        TransportType = TransportType.Tcp;
    }

    /// <summary>
    /// Gets the version information for the current assembly.
    /// </summary>
    public static string? Version => Assembly.GetExecutingAssembly()
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
    /// Gets the transport used for UDP queries.
    /// </summary>
    /// <remarks>
    /// Defaults to a real socket. Internal so tests can substitute one; not part of the public
    /// surface and not a supported extension point.
    /// </remarks>
    internal IUdpTransport UdpTransport { get; init; } = new UdpTransport();

    /// <summary>
    /// Gets the transport used for TCP queries.
    /// </summary>
    /// <remarks>
    /// Defaults to a real socket. Internal so tests can substitute one; not part of the public
    /// surface and not a supported extension point.
    /// </remarks>
    internal ITcpTransport TcpTransport { get; init; } = new TcpTransport();

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

    /// <summary> Sends a DNS query asynchronously. </summary>
    /// <param name="domainName"> The domain name to resolve. </param>
    /// <param name="questionType"> The type of DNS query (e.g., A, AAAA, MX). </param>
    /// <param name="questionClass"> The class of DNS query (e.g., IN for Internet). </param>
    /// <param name="cancellationToken"> Cancels the query. </param>
    /// <returns> A <see cref="Response"/> containing the result of the DNS query. </returns>
    /// <exception cref="InvalidOperationException">The resolver has no DNS servers configured.</exception>
    /// <exception cref="OperationCanceledException"><paramref name="cancellationToken"/> was cancelled.</exception>
    /// <remarks>
    /// A cached answer is returned without awaiting anything. Cancellation is distinct from the
    /// configured timeout: a timeout fails over to the next server, cancellation abandons the query.
    /// </remarks>
    public async Task<Response> QueryAsync(
        string domainName,
        QuestionType questionType,
        QuestionClass questionClass = QuestionClass.IN,
        CancellationToken cancellationToken = default)
    {
        if (_dnsServers.Count <= 0)
        {
            _logger.Error("No DNS servers to query.");
            throw new InvalidOperationException("The resolver has no DNS servers configured.");
        }

        _logger.Debug($"Received {questionType} query for {domainName}");

        var question = new Question(domainName, questionType, questionClass);
        var cached = SearchInCache(question);
        if (cached != null)
        {
            _logger.Debug("Returning cached response...");
            return cached;
        }

        _logger.Debug("Sending request to server...");
        var request = new Request();
        request.AddQuestion(question);
        return await GetResponseAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Reads one or more length-prefixed DNS messages from a connected stream.
    /// </summary>
    /// <param name="stream">The stream to read framed messages from.</param>
    /// <param name="server">The server the messages came from, recorded on the response.</param>
    /// <returns>
    /// The response, or for an AXFR query the accumulated transfer once the closing SOA arrives.
    /// </returns>
    /// <remarks>
    /// Internal rather than private so it can be tested over a <see cref="MemoryStream" /> without
    /// a socket; it is not part of the public surface.
    /// </remarks>
    internal Response ReceiveResponse(Stream stream, IPEndPoint server)
    {
        var transferResponse = new Response();
        var soa = 0;
        var messageSize = 0;
        var isTransfer = false;

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

            // RFC 5936 requires the question section on the first response of a zone transfer but
            // lets later messages omit it, so only re-read the intent when one is actually present.
            // Indexing unconditionally threw on any message without a question section.
            if (response.Questions.Count > 0)
            {
                isTransfer = response.Questions[0].QuestionType == QuestionType.AXFR;
            }

            if (!isTransfer)
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

            // A transfer message with no answers carries no SOA to count; malformed input must not
            // index past the end of the list.
            if (response.Answers.Count > 0 && response.Answers[0].Type == RecordType.SOA)
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

    /// <summary>
    /// Reads one or more length-prefixed DNS messages from a connected stream, asynchronously.
    /// </summary>
    /// <param name="stream">The stream to read framed messages from.</param>
    /// <param name="server">The server the messages came from, recorded on the response.</param>
    /// <param name="cancellationToken">Cancels the read.</param>
    /// <returns>
    /// The response, or for an AXFR query the accumulated transfer once the closing SOA arrives.
    /// </returns>
    /// <remarks>
    /// Deliberately a separate implementation rather than the synchronous one bridged onto a task.
    /// The two differ only in how bytes are pulled off the stream; C# has no way to express that
    /// once, and blocking on an asynchronous read is the deadlock this library moved away from.
    /// </remarks>
    internal async Task<Response> ReceiveResponseAsync(
        Stream stream, IPEndPoint server, CancellationToken cancellationToken)
    {
        var transferResponse = new Response();
        var soa = 0;
        var messageSize = 0;
        var isTransfer = false;
        var prefix = new byte[2];

        while (true)
        {
            var read = await stream
                .ReadAtLeastAsync(prefix, 2, throwOnEndOfStream: false, cancellationToken)
                .ConfigureAwait(false);
            if (read < 2)
            {
                _logger.Error($"Connection to nameserver {server.Address} closed before sending a length prefix");
                throw new SocketException();
            }

            var length = (prefix[0] << 8) | prefix[1];
            if (length <= 0)
            {
                _logger.Error($"Connection to nameserver {server.Address} failed");
                throw new SocketException();
            }

            messageSize += length;

            var data = new byte[length];
            await stream.ReadExactlyAsync(data, cancellationToken).ConfigureAwait(false);

            _logger.Debug("Building response...");
            var response = new Response(server, data);

            if (response.Header.ResponseCode != ResponseCode.NoError)
            {
                _logger.Debug($"Error from server - {response.Header.ResponseCode}");
                return response;
            }

            if (response.Questions.Count > 0)
            {
                isTransfer = response.Questions[0].QuestionType == QuestionType.AXFR;
            }

            if (!isTransfer)
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

            if (response.Answers.Count > 0 && response.Answers[0].Type == RecordType.SOA)
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

    [GeneratedRegex("[^0-9]")]
    private static partial Regex Number();

    private static void WriteRequest(Stream stream, Request request)
    {
        var data = request.GetBytes();
        stream.WriteByte((byte)((data.Length >> 8) & 0xFF));
        stream.WriteByte((byte)(data.Length & 0xFF));
        stream.Write(data, 0, data.Length);
        stream.Flush();
    }

    private static async Task WriteRequestAsync(
        Stream stream, Request request, CancellationToken cancellationToken)
    {
        var data = request.GetBytes();
        var framed = new byte[data.Length + 2];
        framed[0] = (byte)((data.Length >> 8) & 0xFF);
        framed[1] = (byte)(data.Length & 0xFF);
        data.CopyTo(framed, 2);

        await stream.WriteAsync(framed, cancellationToken).ConfigureAwait(false);
        await stream.FlushAsync(cancellationToken).ConfigureAwait(false);
    }

    private static ushort GetUniqueId()
    {
        using var rng = RandomNumberGenerator.Create();
        var rand = new byte[16];
        rng.GetBytes(rand);
        var id = BitConverter.ToUInt16(rand, 0);

        return id;
    }

    private async Task<Response> GetResponseAsync(Request request, CancellationToken cancellationToken)
    {
        request.Header.Id = GetUniqueId();
        request.Header.Recursion = Recursion;

        return TransportType switch
        {
            TransportType.Udp => await UdpRequestAsync(request, cancellationToken).ConfigureAwait(false),
            TransportType.Tcp => await TcpRequestAsync(request, cancellationToken).ConfigureAwait(false),
            _ => throw new InvalidOperationException(),
        };
    }

    private async Task<Response> UdpRequestAsync(Request request, CancellationToken cancellationToken)
    {
        _logger.Debug("Starting UDP request...");
        for (var attempts = 0; attempts < Retries; attempts++)
        {
            _logger.Debug($"Attempt {attempts + 1} of {Retries}...");
            foreach (var server in _dnsServers)
            {
                _logger.Debug($"Connecting to server {server.Address}...");

                try
                {
                    var data = await UdpTransport
                        .ExchangeAsync(request.GetBytes(), server, Timeout, cancellationToken)
                        .ConfigureAwait(false);

                    var response = new Response(server, data);
                    AddToCache(response);

                    return response;
                }
                catch (SocketException exception)
                {
                    _logger.Error(exception, $"Connection to nameserver {server.Address} failed");
                }
            }
        }

        return new Response(true);
    }

    private async Task<Response> TcpRequestAsync(Request request, CancellationToken cancellationToken)
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
                    using var connection = await TcpTransport
                        .ConnectAsync(server, Timeout, cancellationToken)
                        .ConfigureAwait(false);

                    _logger.Debug("Sending request to server...");
                    await WriteRequestAsync(connection.Stream, request, cancellationToken)
                        .ConfigureAwait(false);

                    return await ReceiveResponseAsync(connection.Stream, server, cancellationToken)
                        .ConfigureAwait(false);
                }
                catch (Exception e) when (e is SocketException or IOException)
                {
                    _logger.Error(e, $"Request to nameserver {server.Address} failed");
                }
            }
        }

        _logger.Debug("Connection timed out");
        return new Response(true);
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

    private Response? SearchInCache(Question question)
    {
        _logger.Debug("Searching cache for question...");
        if (!_useCache)
        {
            return null;
        }

        Response response;

        lock (_responseCache)
        {
            if (!_responseCache.TryGetValue(question, out Response? value))
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

                try
                {
                    var data = UdpTransport.Exchange(request.GetBytes(), server, Timeout);

                    var response = new Response(server, data);
                    AddToCache(response);

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
                    using var connection = TcpTransport.Connect(server, Timeout);

                    _logger.Debug("Sending request to server...");
                    WriteRequest(connection.Stream, request);

                    return ReceiveResponse(connection.Stream, server);
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
} // class
