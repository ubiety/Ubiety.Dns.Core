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
using System.Threading.Tasks;

using Ubiety.Dns.Core.Common;
using Ubiety.Dns.Core.Common.Extensions;
using Ubiety.Logging.Core;

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     DNS resolver runs queries against a server.
    /// </summary>
    public partial class Resolver
    {
        private readonly IUbietyLogger _logger = UbietyLogger.Get<Resolver>();
        private readonly Dictionary<Question, Response> _responseCache;
        private readonly List<IPEndPoint> _dnsServers;

        private readonly bool _useCache;

        /// <summary> Initializes a new instance of the <see cref="Resolver" /> class. </summary>
        /// <remarks> Dieter (coder2000) Lunn, 2020-04-01. </remarks>
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
        ///     Gets the current version of the library.
        /// </summary>
        public static string Version => Assembly.GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion;

        /// <summary>
        ///     Gets the resolution timeout in milliseconds.
        /// </summary>
        public int Timeout { get; init; }

        /// <summary>
        ///     Gets the number of retries before giving up.
        /// </summary>
        public int Retries { get; init; }

        /// <summary>
        ///     Gets a value indicating whether recursion is enabled for queries.
        /// </summary>
        public bool Recursion { get; init; }

        /// <summary>
        ///     Gets or sets protocol to use.
        /// </summary>
        public TransportType TransportType { get; set; }

        /// <summary>
        ///     Gets a value indicating whether to use the cache.
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
        ///     Translates the IPV4 or IPV6 address into an arpa address.
        /// </summary>
        /// <param name="ip">IP address to get the arpa address for.</param>
        /// <returns>The 'mirrored' IPV4 or IPV6 arpa address.</returns>
        public static string GetArpaFromIp(IPAddress ip)
        {
            ip = ip.ThrowIfNull(nameof(ip));

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
        ///     Get ARPA address from enumerator.
        /// </summary>
        /// <param name="enumerator">Enumerator for the address.</param>
        /// <returns>String of the ARPA address.</returns>
        public static string GetArpaFromEnumerator(string enumerator)
        {
            var sb = new StringBuilder();
#if NET7_0_OR_GREATER
            var number = Number().Replace(enumerator, string.Empty);
#else
            var number = Regex.Replace(enumerator, "[^0-9]", string.Empty);
#endif
            sb.Append("e164.arpa.");
            foreach (var c in number)
            {
                sb.Insert(0, $"{c}.");
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Clear the resolver cache.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public void ClearCache()
        {
            lock (_responseCache)
            {
                _responseCache.Clear();
            }
        }

        /// <summary> Execute a query on a DNS server. </summary>
        /// <param name="domainName">    Domain name to look up. </param>
        /// <param name="questionType">  Question type of the query. </param>
        /// <param name="questionClass"> Class type of the query. </param>
        /// <returns> DNS response for request. </returns>
        public Response Query(string domainName, QuestionType questionType, QuestionClass questionClass)
        {
            if (_dnsServers.Count <= 0)
            {
                _logger.Error("No DNS servers to query.");
                return null;
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

        /// <summary> Execute a query on a DNS server. </summary>
        /// <param name="domainName">    Domain name to look up. </param>
        /// <param name="questionType">  Question type of the query. </param>
        /// <returns> DNS response for request. </returns>
        public Response Query(string domainName, QuestionType questionType)
        {
            return Query(domainName, questionType, QuestionClass.IN);
        }

#if NET7_0_OR_GREATER
        [GeneratedRegex("[^0-9]")]
        private static partial Regex Number();
#endif

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
                TransportType.Tcp => TcpRequest(request).Result,
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
                _logger.Debug($"Attempt {attempts} of {Retries}...");
                foreach (var server in _dnsServers)
                {
                    _logger.Debug($"Connecting to server {server.Address}...");
                    using var client = new UdpClient(AddressFamily.InterNetworkV6) { Client = { DualMode = true } };

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

        private async Task<Response> TcpRequest(Request request)
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
                            Client = { DualMode = true },
                        }
                        : new TcpClient(AddressFamily.InterNetwork)
                        {
                            ReceiveTimeout = Timeout,
                        };

                        await client.ConnectAsync(server.Address, server.Port).ConfigureAwait(false);

                        if (!client.Connected)
                        {
                            client.Close();
                            _logger.Error($"Connection to nameserver {server.Address} failed.");
                            continue;
                        }

#if NETSTANDARD2_0
                        using var stream = new BufferedStream(client.GetStream());
#else
                        await using var stream = new BufferedStream(client.GetStream());
#endif

                        _logger.Debug("Sending request to server...");
                        WriteRequest(stream, request);

                        return ReceiveResponse(stream, server);
                    }
                    catch (SocketException e)
                    {
                        _logger.Error(e, "Socket exception occurred during request.");
                        throw;
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
                var length = (stream.ReadByte() << 8) | stream.ReadByte();
                if (length <= 0)
                {
                    _logger.Error($"Connection to nameserver {server.Address} failed");
                    throw new SocketException();
                }

                messageSize += length;

                var data = new byte[length];
                _ = stream.Read(data, 0, length);

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
}
