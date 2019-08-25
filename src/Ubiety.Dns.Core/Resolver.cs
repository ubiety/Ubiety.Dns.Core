/*
 * Licensed under the MIT license
 * See the LICENSE file in the project root for more information
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ubiety.Dns.Core.Common;

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     DNS resolver runs querys against a server.
    /// </summary>
    public class Resolver
    {
        private readonly Dictionary<string, Response> _responseCache;
        private int _retries;
        private int _timeout;

        private ushort _unique;
        private bool _useCache;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Resolver" /> class.
        /// </summary>
        /// <param name="dnsServers">Set of DNS servers.</param>
        public Resolver(IEnumerable<IPEndPoint> dnsServers)
        {
            var rng = new RNGCryptoServiceProvider();
            var rand = new byte[16];
            rng.GetBytes(rand);
            _responseCache = new Dictionary<string, Response>();
            DnsServers = new List<IPEndPoint>();
            DnsServers.AddRange(dnsServers);

            _unique = BitConverter.ToUInt16(rand, 0);
            _retries = 3;
            _timeout = 1;
            Recursion = true;
            _useCache = true;
            TransportType = TransportType.Udp;

            rng.Dispose();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Resolver" /> class.
        /// </summary>
        /// <param name="dnsServer">DNS server to use.</param>
        public Resolver(IPEndPoint dnsServer)
            : this(new[] { dnsServer })
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Resolver" /> class.
        /// </summary>
        /// <param name="serverIpAddress">DNS server to use.</param>
        /// <param name="serverPortNumber">DNS port to use.</param>
        public Resolver(IPAddress serverIpAddress, int serverPortNumber)
            : this(new IPEndPoint(serverIpAddress, serverPortNumber))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Resolver" /> class.
        /// </summary>
        /// <param name="serverIpAddress">DNS server address to use.</param>
        /// <param name="serverPortNumber">DNS port to use.</param>
        public Resolver(string serverIpAddress, int serverPortNumber)
            : this(IPAddress.Parse(serverIpAddress), serverPortNumber)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Resolver" /> class.
        /// </summary>
        /// <param name="serverIpAddress">DNS server address to use.</param>
        public Resolver(string serverIpAddress)
            : this(IPAddress.Parse(serverIpAddress), DefaultPort)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Resolver" /> class.
        /// </summary>
        public Resolver()
            : this(GetSystemDnsServers())
        {
        }

        /// <summary>
        ///     Verbose event handler.
        /// </summary>
        /// <param name="sender">Object sending the event.</param>
        /// <param name="e">Event arguments.</param>
        public delegate void VerboseEventHandler(object sender, VerboseEventArgs e);

        /// <summary>
        ///     Verbose messages from internal operations
        /// </summary>
        public event VerboseEventHandler OnVerbose;

        /// <summary>
        ///     Gets the current version of the library.
        /// </summary>
        public static string Version => Assembly.GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        /// <summary>
        ///     Gets the default DNS servers for OpenDNS.
        /// </summary>
        public static List<IPEndPoint> DefaultDnsServers => new List<IPEndPoint>
        {
            new IPEndPoint(IPAddress.Parse("208.67.222.222"), DefaultPort),
            new IPEndPoint(IPAddress.Parse("208.67.220.220"), DefaultPort),
        };

        /// <summary>
        ///     Gets or sets the default DNS port.
        /// </summary>
        public static int DefaultPort { get; set; } = 53;

        /// <summary>
        ///     Gets or sets timeout in milliseconds.
        /// </summary>
        public int Timeout
        {
            get => _timeout;

            set => _timeout = value * 1000;
        }

        /// <summary>
        ///     Gets or sets the number of retries before giving up.
        /// </summary>
        public int Retries
        {
            get => _retries;

            set
            {
                if (value >= 1)
                {
                    _retries = value;
                }
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether recursion is enabled for doing queries.
        /// </summary>
        public bool Recursion { get; set; }

        /// <summary>
        ///     Gets or sets protocol to use.
        /// </summary>
        public TransportType TransportType { get; set; }

        /// <summary>
        ///     Gets a list of DNS servers to use.
        /// </summary>
        public List<IPEndPoint> DnsServers { get; }

        /// <summary>
        ///     Gets or sets the first DNS server address or sets single DNS server to use.
        /// </summary>
        public string DnsServer
        {
            get => DnsServers[0].Address.ToString();

            set
            {
                if (IPAddress.TryParse(value, out var ip))
                {
                    DnsServers.Clear();
                    DnsServers.Add(new IPEndPoint(ip, DefaultPort));
                    return;
                }

                var response = Query(value, QuestionType.A);
                if (response.RecordA.Count <= 0)
                {
                    return;
                }

                DnsServers.Clear();
                DnsServers.Add(new IPEndPoint(response.RecordA[0].Address, DefaultPort));
            }
        }

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

                lock (_responseCache)
                {
                    _responseCache.Clear();
                }
            }
        }

        /// <summary>
        ///     Gets a list of default DNS servers used on the Windows machine.
        /// </summary>
        /// <returns>Array of DNS servers.</returns>
        public static IEnumerable<IPEndPoint> GetSystemDnsServers()
        {
            var list = new List<IPEndPoint>();

            var adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var n in adapters)
            {
                if (n.OperationalStatus != OperationalStatus.Up)
                {
                    continue;
                }

                var ipProps = n.GetIPProperties();

                // thanks to Jon Webster on May 20, 2008
                foreach (var ipAddr in ipProps.DnsAddresses)
                {
                    var entry = new IPEndPoint(ipAddr, DefaultPort);
                    if (!list.Contains(entry))
                    {
                        list.Add(entry);
                    }
                }
            }

            return list.ToArray();
        }

        /// <summary>
        ///     Translates the IPV4 or IPV6 address into an arpa address.
        /// </summary>
        /// <param name="ip">IP address to get the arpa address form.</param>
        /// <returns>The 'mirrored' IPV4 or IPV6 arpa address.</returns>
        public static string GetArpaFromIp(IPAddress ip)
        {
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
        ///     Get ARPA address from enum.
        /// </summary>
        /// <param name="enumerator">Enum for the address.</param>
        /// <returns>String of the ARPA address.</returns>
        public static string GetArpaFromEnum(string enumerator)
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
        public void ClearCache()
        {
            lock (_responseCache)
            {
                _responseCache.Clear();
            }
        }

        /// <summary>
        ///     Do Query on specified DNS servers.
        /// </summary>
        /// <param name="name">Name to query.</param>
        /// <param name="qtype">Question type.</param>
        /// <param name="qclass">Class type.</param>
        /// <returns>Response of the query.</returns>
        public Response Query(string name, QuestionType qtype, QuestionClass qclass)
        {
            var question = new Question(name, qtype, qclass);
            var response = SearchInCache(question);
            if (response != null)
            {
                return response;
            }

            var request = new Request();
            request.AddQuestion(question);
            return GetResponse(request);
        }

        /// <summary>
        ///     Do an QClass=IN Query on specified DNS servers.
        /// </summary>
        /// <param name="name">Name to query.</param>
        /// <param name="qtype">Question type.</param>
        /// <returns>Response of the query.</returns>
        public Response Query(string name, QuestionType qtype)
        {
            var question = new Question(name, qtype, QuestionClass.IN);
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

        private Response GetResponse(Request request)
        {
            request.Header.Id = _unique;
            request.Header.Recursion = Recursion;

            switch (TransportType)
            {
                case TransportType.Udp:
                    return UdpRequest(request);
                case TransportType.Tcp:
                    return TcpRequest(request).Result;
            }

            var response = new Response { Error = "Unknown TransportType" };
            return response;
        }

        private void Verbose(string format, params object[] args)
        {
            OnVerbose?.Invoke(this, new VerboseEventArgs(string.Format(CultureInfo.CurrentCulture, format, args)));
        }

        private Response SearchInCache(Question question)
        {
            if (!_useCache)
            {
                return null;
            }

            var strKey = question.QuestionClass + "-" + question.QuestionType + "-" + question.QuestionName;

            Response response;

            lock (_responseCache)
            {
                if (!_responseCache.ContainsKey(strKey))
                {
                    return null;
                }

                response = _responseCache[strKey];
            }

            var timeLived = (int)((DateTime.Now.Ticks - response.TimeStamp.Ticks) / TimeSpan.TicksPerSecond);
            foreach (var rr in response.ResourceRecords)
            {
                rr.TimeLived = timeLived;

                // The TTL property calculates its actual time to live
                if (rr.Ttl == 0)
                {
                    return null; // out of date
                }
            }

            return response;
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

            var strKey = question.QuestionClass + "-" + question.QuestionType + "-" + question.QuestionName;

            lock (_responseCache)
            {
                if (_responseCache.ContainsKey(strKey))
                {
                    _responseCache.Remove(strKey);
                }

                _responseCache.Add(strKey, response);
            }
        }

        private Response UdpRequest(Request request)
        {
            // RFC1035 max. size of a UDP datagram is 512 bytes
            var responseMessage = new byte[512];

            for (var intAttempts = 0; intAttempts < _retries; intAttempts++)
            {
                for (var intDnsServer = 0; intDnsServer < DnsServers.Count; intDnsServer++)
                {
                    var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, Timeout);

                    try
                    {
                        socket.SendTo(request.GetData(), DnsServers[intDnsServer]);
                        var intReceived = socket.Receive(responseMessage);
                        var data = new byte[intReceived];
                        Array.Copy(responseMessage, data, intReceived);
                        var response = new Response(DnsServers[intDnsServer], data);
                        AddToCache(response);
                        return response;
                    }
                    catch (SocketException)
                    {
                        Verbose($";; Connection to nameserver {intDnsServer + 1} failed");
                    }
                    finally
                    {
                        _unique++;

                        // close the socket
                        socket.Close();
                    }
                }
            }

            var responseTimeout = new Response { Error = "Timeout Error" };
            return responseTimeout;
        }

        private async Task<Response> TcpRequest(Request request)
        {
            for (var intAttempts = 0; intAttempts < _retries; intAttempts++)
            {
                foreach (var server in DnsServers)
                {
                    var client = new TcpClient { ReceiveTimeout = _timeout };
                    var stream = new BufferedStream(client.GetStream());

                    try
                    {
                        await client.ConnectAsync(server.Address, server.Port).ConfigureAwait(false);

                        if (!client.Connected)
                        {
                            client.Close();
                            Verbose($";; Connection to nameserver {server.Address} failed");
                            continue;
                        }

                        WriteRequest(stream, request);

                        return ReceiveResponse(stream, server);
                    }
                    catch (SocketException e)
                    {
                        Verbose(e.Message);
                        throw;
                    }
                    finally
                    {
                        _unique++;
                        stream.Close();
                        stream.Dispose();

                        client.Close();
                        client.Dispose();
                    }
                }
            }

            var responseTimeout = new Response { Error = "Timeout Error" };
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
                    Verbose($"Connection to nameserver {server.Address} failed");
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
                transferResponse.Additionals.AddRange(response.Additionals);

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
                transferResponse.Header.AdditionalRecordsCount = (ushort)transferResponse.Additionals.Count;
                transferResponse.MessageSize = messageSize;

                return transferResponse;
            }
        }
    } // class
}