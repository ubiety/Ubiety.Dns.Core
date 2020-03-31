/*
 *      Copyright (C) 2020 Dieter (coder2000) Lunn
 *
 *      This program is free software: you can redistribute it and/or modify
 *      it under the terms of the GNU General Public License as published by
 *      the Free Software Foundation, either version 3 of the License, or
 *      (at your option) any later version.
 *
 *      This program is distributed in the hope that it will be useful,
 *      but WITHOUT ANY WARRANTY; without even the implied warranty of
 *      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *      GNU General Public License for more details.
 *
 *      You should have received a copy of the GNU General Public License
 *      along with this program.  If not, see <https://www.gnu.org/licenses/>.
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
#if NET452 || NET471
using TransportType = Ubiety.Dns.Core.Common.TransportType;
#endif

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     DNS resolver runs queries against a server.
    /// </summary>
    public class Resolver
    {
        private readonly IUbietyLogger _logger = UbietyLogger.Get<Resolver>();
        private readonly Dictionary<Question, Response> _responseCache;
        private readonly List<IPEndPoint> _dnsServers;

        private bool _useCache;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Resolver" /> class.
        /// </summary>
        /// <param name="dnsServers">Set of DNS servers to use for resolution.</param>
        internal Resolver(IEnumerable<IPEndPoint> dnsServers)
        {
            _responseCache = new Dictionary<Question, Response>();
            _dnsServers = new List<IPEndPoint>();
            _dnsServers.AddRange(dnsServers);

            TransportType = TransportType.Udp;
        }

        /// <summary>
        ///     Gets the current version of the library.
        /// </summary>
        public static string Version => Assembly.GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        /// <summary>
        ///     Gets or sets resolution timeout in milliseconds.
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        ///     Gets or sets the number of retries before giving up.
        /// </summary>
        public int Retries { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether recursion is enabled for queries.
        /// </summary>
        public bool Recursion { get; set; }

        /// <summary>
        ///     Gets or sets protocol to use.
        /// </summary>
        public TransportType TransportType { get; set; }

        /// <summary>
        ///     Gets the list of DNS servers in the resolver.
        /// </summary>
        [Obsolete("Is this needed? If you use it please open an issue.")]
        public List<IPEndPoint> DnsServers => _dnsServers;

        /// <summary>
        ///     Gets or sets a value indicating whether to use the cache.
        /// </summary>
        public bool UseCache
        {
            get => _useCache;

            set
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
        /// <param name="ip">IP address to get the arpa address form.</param>
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
            }

            return "?";
        }

        /// <summary>
        ///     Get ARPA address from enumerator.
        /// </summary>
        /// <param name="enumerator">Enumerator for the address.</param>
        /// <returns>String of the ARPA address.</returns>
        public static string GetArpaFromEnumerator(string enumerator)
        {
            var sb = new StringBuilder();
            var number = Regex.Replace(enumerator, "[^0-9]", string.Empty);
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

        /// <summary>
        ///     Execute a query on a DNS server.
        /// </summary>
        /// <param name="name">Domain name to look up.</param>
        /// <param name="questionType">Question type of the query.</param>
        /// <param name="questionClass">Class type of the query. Defaults to Internet.</param>
        /// <returns>DNS response for request.</returns>
        public Response Query(string name, QuestionType questionType, QuestionClass questionClass = QuestionClass.IN)
        {
            var question = new Question(name, questionType, questionClass);
            var response = SearchInCache(question);
            if (response != null)
            {
                return response;
            }

            var request = new Request();
            request.AddQuestion(question);
            return GetResponse(request);
        }

        private static void WriteRequest(Stream stream, Request request)
        {
            var data = request.GetData();
            stream.WriteByte((byte)((data.Length >> 8) & 0xFF));
            stream.WriteByte((byte)(data.Length & 0xFF));
            stream.Write(data, 0, data.Length);
            stream.Flush();
        }

        private static ushort GetUniqueId()
        {
            using var rng = new RNGCryptoServiceProvider();
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
            if (!_useCache)
            {
                return null;
            }

            Response response;

            lock (_responseCache)
            {
                if (!_responseCache.ContainsKey(question))
                {
                    return null;
                }

                response = _responseCache[question];
            }

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
                if (_responseCache.ContainsKey(question))
                {
                    _responseCache.Remove(question);
                }

                _responseCache.Add(question, response);
            }
        }

        private Response UdpRequest(Request request)
        {
            for (var attempts = 0; attempts < Retries; attempts++)
            {
                foreach (var server in _dnsServers)
                {
                    using var client = new UdpClient(AddressFamily.InterNetworkV6) { Client = { DualMode = true } };

                    try
                    {
                        var sendBytes = request.GetData();
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
            for (var attempts = 0; attempts < Retries; attempts++)
            {
                foreach (var server in _dnsServers)
                {
                    using var client = new TcpClient(AddressFamily.InterNetworkV6)
                    {
                        ReceiveTimeout = Timeout, Client = { DualMode = true },
                    };

                    using var stream = new BufferedStream(client.GetStream());

                    try
                    {
                        await client.ConnectAsync(server.Address, server.Port).ConfigureAwait(false);

                        if (!client.Connected)
                        {
                            client.Close();
                            _logger.Error($"Connection to nameserver {server.Address} failed");
                            continue;
                        }

                        WriteRequest(stream, request);

                        return ReceiveResponse(stream, server);
                    }
                    catch (SocketException e)
                    {
                        _logger.Error(e, "Socket exception occured during request.");
                        throw;
                    }
                }
            }

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
                stream.Read(data, 0, length);

                var response = new Response(server, data);

                if (response.Header.ResponseCode != ResponseCode.NoError)
                {
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
