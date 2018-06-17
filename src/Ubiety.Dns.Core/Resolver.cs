using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Ubiety.Dns.Core.Common;

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     DNS resolver runs querys against a server
    /// </summary>
    public class Resolver
    {
        /// <summary>
        ///     Default DNS port
        /// </summary>
        public const Int32 DefaultPort = 53;

        private readonly List<IPEndPoint> dnsServers;
        private readonly IPEndPoint[] defaultDnsServers =
            {
                new IPEndPoint(IPAddress.Parse("208.67.222.222"), DefaultPort),
                new IPEndPoint(IPAddress.Parse("208.67.220.220"), DefaultPort)
            };

        private readonly Dictionary<string, Response> responseCache;

        private UInt16 unique;
        private Boolean useCache;
        private Int32 retries;
        private Int32 timeout;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Resolver" /> class
        /// </summary>
        /// <param name="dnsServers">Set of DNS servers</param>
        public Resolver(IPEndPoint[] dnsServers)
        {
            this.responseCache = new Dictionary<String, Response>();
            this.dnsServers = new List<IPEndPoint>();
            this.dnsServers.AddRange(dnsServers);

            this.unique = (UInt16)new Random().Next();
            this.retries = 3;
            this.timeout = 1;
            this.Recursion = true;
            this.useCache = true;
            this.TransportType = TransportType.Udp;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Resolver" /> class
        /// </summary>
        /// <param name="dnsServer">DNS server to use</param>
        public Resolver(IPEndPoint dnsServer)
            : this(new IPEndPoint[] { dnsServer })
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Resolver" /> class
        /// </summary>
        /// <param name="serverIpAddress">DNS server to use</param>
        /// <param name="serverPortNumber">DNS port to use</param>
        public Resolver(IPAddress serverIpAddress, Int32 serverPortNumber)
            : this(new IPEndPoint(serverIpAddress, serverPortNumber))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Resolver" /> class
        /// </summary>
        /// <param name="serverIpAddress">DNS server address to use</param>
        /// <param name="serverPortNumber">DNS port to use</param>
        public Resolver(String serverIpAddress, Int32 serverPortNumber)
            : this(IPAddress.Parse(serverIpAddress), serverPortNumber)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Resolver" /> class
        /// </summary>
        /// <param name="serverIpAddress">DNS server address to use</param>
        public Resolver(String serverIpAddress)
            : this(IPAddress.Parse(serverIpAddress), DefaultPort)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Resolver" /> class
        /// </summary>
        public Resolver()
            : this(GetDnsServers())
        {
        }

        /// <summary>
        ///     Verbose event handler
        /// </summary>
        /// <param name="sender">Object sending the event</param>
        /// <param name="e">Event arguments</param>
        public delegate void VerboseEventHandler(Object sender, VerboseEventArgs e);

        /// <summary>
        /// Verbose messages from internal operations
        /// </summary>
        public event VerboseEventHandler OnVerbose;

        /// <summary>
        ///     Gets the current version of the library
        /// </summary>
        public static string Version
        {
            get => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        ///     Gets the default OpenDNS servers
        /// </summary>
        public IPEndPoint[] DefaultDnsServers
        {
            get => this.defaultDnsServers;
        }

        /// <summary>
        ///     Gets or sets timeout in milliseconds
        /// </summary>
        public int Timeout
        {
            get => this.timeout;

            set
            {
                this.timeout = value * 1000;
            }
        }

        /// <summary>
        ///     Gets or sets the number of retries before giving up
        /// </summary>
        public int Retries
        {
            get => this.retries;

            set
            {
                if (value >= 1)
                {
                    this.retries = value;
                }
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether recursion is enabled for doing queries
        /// </summary>
        public bool Recursion { get; set; }

        /// <summary>
        ///     Gets or sets protocol to use
        /// </summary>
        public TransportType TransportType { get; set; }

        /// <summary>
        ///     Gets or sets list of DNS servers to use
        /// </summary>
        public IPEndPoint[] DnsServers
        {
            get
            {
                return this.dnsServers.ToArray();
            }

            set
            {
                this.dnsServers.Clear();
                this.dnsServers.AddRange(value);
            }
        }

        /// <summary>
        ///     Gets or sets the first DNS server address or sets single DNS server to use
        /// </summary>
        public string DnsServer
        {
            get
            {
                return this.dnsServers[0].Address.ToString();
            }

            set
            {
                IPAddress ip;
                if (IPAddress.TryParse(value, out ip))
                {
                    this.dnsServers.Clear();
                    this.dnsServers.Add(new IPEndPoint(ip, DefaultPort));
                    return;
                }

                Response response = this.Query(value, QuestionType.A);
                if (response.RecordA.Count > 0)
                {
                    this.dnsServers.Clear();
                    this.dnsServers.Add(new IPEndPoint(response.RecordA[0].Address, DefaultPort));
                }
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether to use the cache
        /// </summary>
        public bool UseCache
        {
            get
            {
                return this.useCache;
            }

            set
            {
                this.useCache = value;
                if (!this.useCache)
                {
                    this.responseCache.Clear();
                }
            }
        }

        /// <summary>
        ///     Gets a list of default DNS servers used on the Windows machine.
        /// </summary>
        /// <returns>Array of DNS servers</returns>
        public static IPEndPoint[] GetDnsServers()
        {
            List<IPEndPoint> list = new List<IPEndPoint>();

            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface n in adapters)
            {
                if (n.OperationalStatus == OperationalStatus.Up)
                {
                    IPInterfaceProperties ipProps = n.GetIPProperties();

                    // thanks to Jon Webster on May 20, 2008
                    foreach (IPAddress ipAddr in ipProps.DnsAddresses)
                    {
                        IPEndPoint entry = new IPEndPoint(ipAddr, DefaultPort);
                        if (!list.Contains(entry))
                        {
                            list.Add(entry);
                        }
                    }
                }
            }

            return list.ToArray();
        }

        /// <summary>
        ///     Translates the IPV4 or IPV6 address into an arpa address
        /// </summary>
        /// <param name="ip">IP address to get the arpa address form</param>
        /// <returns>The 'mirrored' IPV4 or IPV6 arpa address</returns>
        public static String GetArpaFromIp(IPAddress ip)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("in-addr.arpa.");
                foreach (Byte b in ip.GetAddressBytes())
                {
                    sb.Insert(0, $"{b}.");
                }

                return sb.ToString();
            }

            if (ip.AddressFamily == AddressFamily.InterNetworkV6)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("ip6.arpa.");
                foreach (byte b in ip.GetAddressBytes())
                {
                    sb.Insert(0, $"{(b >> 4) & 0xf:x}.");
                    sb.Insert(0, $"{(b >> 0) & 0xf:x}.");
                }

                return sb.ToString();
            }

            return "?";
        }

        /// <summary>
        ///     Get ARPA address from enum
        /// </summary>
        /// <param name="enumerator">Enum for the address</param>
        /// <returns>String of the ARPA address</returns>
        public static String GetArpaFromEnum(String enumerator)
        {
            StringBuilder sb = new StringBuilder();
            String number = System.Text.RegularExpressions.Regex.Replace(enumerator, "[^0-9]", String.Empty);
            sb.Append("e164.arpa.");
            foreach (Char c in number)
            {
                sb.Insert(0, $"{c}.");
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Clear the resolver cache
        /// </summary>
        public void ClearCache()
        {
            this.responseCache.Clear();
        }

        /// <summary>
        ///     Do Query on specified DNS servers
        /// </summary>
        /// <param name="name">Name to query</param>
        /// <param name="qtype">Question type</param>
        /// <param name="qclass">Class type</param>
        /// <returns>Response of the query</returns>
        public Response Query(String name, QuestionType qtype, QuestionClass qclass)
        {
            Question question = new Question(name, qtype, qclass);
            Response response = this.SearchInCache(question);
            if (response != null)
            {
                return response;
            }

            Request request = new Request();
            request.AddQuestion(question);
            return this.GetResponse(request);
        }

        /// <summary>
        ///     Do an QClass=IN Query on specified DNS servers
        /// </summary>
        /// <param name="name">Name to query</param>
        /// <param name="qtype">Question type</param>
        /// <returns>Response of the query</returns>
        public Response Query(string name, QuestionType qtype)
        {
            Question question = new Question(name, qtype, QuestionClass.IN);
            Response response = this.SearchInCache(question);
            if (response != null)
            {
                return response;
            }

            Request request = new Request();
            request.AddQuestion(question);
            return this.GetResponse(request);
        }

        private Response GetResponse(Request request)
        {
            request.Header.Id = this.unique;
            request.Header.Recursion = this.Recursion;

            if (this.TransportType == TransportType.Udp)
            {
                return this.UdpRequest(request);
            }

            if (this.TransportType == TransportType.Tcp)
            {
                return this.TcpRequest(request).Result;
            }

            Response response = new Response();
            response.Error = "Unknown TransportType";
            return response;
        }

        private void Verbose(string format, params object[] args)
        {
            if (this.OnVerbose != null)
            {
                this.OnVerbose(this, new VerboseEventArgs(string.Format(CultureInfo.CurrentCulture, format, args)));
            }
        }

        private Response SearchInCache(Question question)
        {
            if (!this.useCache)
            {
                return null;
            }

            string strKey = question.QuestionClass + "-" + question.QuestionType + "-" + question.QuestionName;

            Response response = null;

            lock (this.responseCache)
            {
                if (!this.responseCache.ContainsKey(strKey))
                {
                    return null;
                }

                response = this.responseCache[strKey];
            }

            int timeLived = (int)((DateTime.Now.Ticks - response.TimeStamp.Ticks) / TimeSpan.TicksPerSecond);
            foreach (ResourceRecord rr in response.ResourceRecords)
            {
                rr.TimeLived = timeLived;

                // The TTL property calculates its actual time to live
                if (rr.TTL == 0)
                {
                    return null; // out of date
                }
            }

            return response;
        }

        private void AddToCache(Response response)
        {
            if (!this.useCache)
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

            Question question = response.Questions[0];

            string strKey = question.QuestionClass + "-" + question.QuestionType + "-" + question.QuestionName;

            lock (this.responseCache)
            {
                if (this.responseCache.ContainsKey(strKey))
                {
                    this.responseCache.Remove(strKey);
                }

                this.responseCache.Add(strKey, response);
            }
        }

        private Response UdpRequest(Request request)
        {
            // RFC1035 max. size of a UDP datagram is 512 bytes
            byte[] responseMessage = new byte[512];

            for (int intAttempts = 0; intAttempts < this.retries; intAttempts++)
            {
                for (int intDnsServer = 0; intDnsServer < this.dnsServers.Count; intDnsServer++)
                {
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, this.Timeout);

                    try
                    {
                        socket.SendTo(request.GetData(), this.dnsServers[intDnsServer]);
                        int intReceived = socket.Receive(responseMessage);
                        byte[] data = new byte[intReceived];
                        Array.Copy(responseMessage, data, intReceived);
                        Response response = new Response(this.dnsServers[intDnsServer], data);
                        this.AddToCache(response);
                        return response;
                    }
                    catch (SocketException)
                    {
                        this.Verbose($";; Connection to nameserver {intDnsServer + 1} failed");
                        continue; // next try
                    }
                    finally
                    {
                        this.unique++;

                        // close the socket
                        socket.Close();
                    }
                }
            }

            Response responseTimeout = new Response();
            responseTimeout.Error = "Timeout Error";
            return responseTimeout;
        }

        private async Task<Response> TcpRequest(Request request)
        {
            for (int intAttempts = 0; intAttempts < this.retries; intAttempts++)
            {
                foreach (var server in this.dnsServers)
                {
                    TcpClient client = new TcpClient();
                    client.ReceiveTimeout = this.timeout;

                    try
                    {
                        await client.ConnectAsync(server.Address, server.Port).ConfigureAwait(false);

                        if (!client.Connected)
                        {
                            client.Close();
                            this.Verbose($";; Connection to nameserver {server.Address} failed");
                            continue;
                        }

                        var stream = new BufferedStream(client.GetStream());

                        WriteRequest(stream, request);

                        return ReceiveResponse(stream, server);
                    }
                    catch(SocketException)
                    {
                        continue;
                    }
                    finally
                    {
                        this.unique++;
                        client.Close();
                    }
                }
            }

            Response responseTimeout = new Response();
            responseTimeout.Error = "Timeout Error";
            return responseTimeout;
        }

        private static void WriteRequest(BufferedStream stream, Request request)
        {
            Byte[] data = request.GetData();
            stream.WriteByte((Byte)((data.Length >> 8) & 0xFF));
            stream.WriteByte((Byte)(data.Length & 0xFF));
            stream.Write(data, 0, data.Length);
            stream.Flush();
        }

        private Response ReceiveResponse(BufferedStream stream,  IPEndPoint server)
        {
            Response transferResponse = new Response();
            Int32 soa = 0;
            Int32 messageSize = 0;

            while (true)
            {
                Int32 length = stream.ReadByte() << 8 | stream.ReadByte();
                if (length <= 0)
                {
                    this.Verbose($"Connection to nameserver {server.Address} failed");
                    throw new SocketException();
                }

                messageSize += length;

                Byte[] data = new Byte[length];
                stream.Read(data, 0, length);

                Response response = new Response(server, data);

                if (response.Header.ResponseCode != ResponseCode.NoError)
                {
                    return response;
                }

                if (response.Questions[0].QuestionType != QuestionType.AXFR)
                {
                    this.AddToCache(response);
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

                if (soa == 2)
                {
                    transferResponse.Header.QuestionCount = (UInt16)transferResponse.Questions.Count;
                    transferResponse.Header.AnswerCount = (UInt16)transferResponse.Answers.Count;
                    transferResponse.Header.NameserverCount = (UInt16)transferResponse.Authorities.Count;
                    transferResponse.Header.AdditionalRecordsCount = (UInt16)transferResponse.Additionals.Count;
                    transferResponse.MessageSize = messageSize;

                    return transferResponse;
                }
            }
        }
    } // class
}