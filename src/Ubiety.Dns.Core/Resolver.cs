using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using Heijden.DNS;
using Ubiety.Dns.Core.Common;

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     DNS resolver runs querys against a server
    /// </summary>
    public class Resolver
    {
        /// <summary>
        ///     Gets the current version of the library
        /// </summary>
        public string Version
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        /// <summary>
        ///     Default DNS port
        /// </summary>
        public const int defaultPort = 53;

        /// <summary>
        ///     Default OpenDNS server addresses
        /// </summary>
        public static readonly IPEndPoint[] defaultDnsServers = 
            { 
                new IPEndPoint(IPAddress.Parse("208.67.222.222"), defaultPort), 
                new IPEndPoint(IPAddress.Parse("208.67.220.220"), defaultPort) 
            };

        private ushort unique;
        private bool useCache;
        private bool recursion;
        private int retries;
        private int timeout;
        private TransportType transportType;

        private List<IPEndPoint> dnsServers;

        private Dictionary<string,Response> responseCache;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Resolver" /> class
        /// </summary>
        /// <param name="dnsServers">Set of DNS servers</param>
        public Resolver(IPEndPoint[] dnsServers)
        {
            this.responseCache = new Dictionary<string, Response>();
            this.dnsServers = new List<IPEndPoint>();
            this.dnsServers.AddRange(dnsServers);

            this.unique = (ushort)(new Random()).Next();
            this.retries = 3;
            this.timeout = 1;
            this.recursion = true;
            this.useCache = true;
            this.transportType = TransportType.Udp;
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
        public Resolver(IPAddress serverIpAddress, int serverPortNumber)
            : this(new IPEndPoint(serverIpAddress,serverPortNumber))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Resolver" /> class
        /// </summary>
        /// <param name="serverIpAddress">DNS server address to use</param>
        /// <param name="serverPortNumber">DNS port to use</param>
        public Resolver(string serverIpAddress, int serverPortNumber)
            : this(IPAddress.Parse(serverIpAddress), serverPortNumber)
        {
        }
        
        /// <summary>
        ///     Initializes a new instance of the <see cref="Resolver" /> class
        /// </summary>
        /// <param name="serverIpAddress">DNS server address to use</param>
        public Resolver(string serverIpAddress)
            : this(IPAddress.Parse(serverIpAddress), defaultPort)
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
        ///     Event args for verbose output
        /// </summary>
        public class VerboseOutputEventArgs : EventArgs
        {
            /// <summary>
            ///     Gets the string message
            /// </summary>
            public string Message;

            /// <summary>
            ///     Initializes a new instance of the <see cref="VerboseOutputEventArgs" /> class
            /// </summary>
            /// <param name="message">Message to output</param>
            public VerboseOutputEventArgs(string message)
            {
                this.Message = message;
            }
        }

        private void Verbose(string format, params object[] args)
        {
            if (OnVerbose != null)
            {
                OnVerbose(this, new VerboseEventArgs(string.Format(format, args)));
            }
        }

        /// <summary>
        /// Verbose messages from internal operations
        /// </summary>
        public event VerboseEventHandler OnVerbose;

        /// <summary>
        ///     Verbose event handler
        /// </summary>
        public delegate void VerboseEventHandler(object sender, VerboseEventArgs e);

        /// <summary>
        ///     Verbose event args
        /// </summary>
        public class VerboseEventArgs : EventArgs
        {
            /// <summary>
            ///     Gets the message to output
            /// </summary>
            public string Message;

            /// <summary>
            ///     Initializes a new instance of the <see cref="VerboseEventArgs" /> class
            /// </summary>
            /// <param name="message">Verbose message</param>
            public VerboseEventArgs(string message)
            {
                this.Message = message;
            }
        }


        /// <summary>
        ///     Gets or sets timeout in milliseconds
        /// </summary>
        public int Timeout
        {
            get
            {
                return this.timeout;
            }
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
            get
            {
                return this.retries;
            }
            set
            {
                if(value >= 1)
                {
                    this.retries = value;
                }
            }
        }

        /// <summary>
        ///     Gets or set recursion for doing queries
        /// </summary>
        public bool Recursion
        {
            get
            {
                return this.recursion;
            }
            set
            {
                this.recursion = value;
            }
        }

        /// <summary>
        ///     Gets or sets protocol to use
        /// </summary>
        public TransportType TransportType
        {
            get
            {
                return this.transportType;
            }
            set
            {
                this.transportType = value;
            }
        }

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
        ///     Gets first DNS server address or sets single DNS server to use
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
                    this.dnsServers.Add(new IPEndPoint(ip, defaultPort));
                    return;
                }
                Response response = Query(value, QType.A);
                if (response.RecordsA.Length > 0)
                {
                    this.dnsServers.Clear();
                    this.dnsServers.Add(new IPEndPoint(response.RecordsA[0].Address, defaultPort));
                }
            }
        }

        /// <summary>
        ///     Gets or sets whether to use the cache
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
        ///     Clear the resolver cache
        /// </summary>
        public void ClearCache()
        {
            this.responseCache.Clear();
        }

        private Response SearchInCache(Question question)
        {
            if (!this.useCache)
            {
                return null;
            }

            string strKey = question.QClass + "-" + question.QType + "-" + question.QName;

            Response response = null;

            lock (this.responseCache)
            {
                if (!this.responseCache.ContainsKey(strKey))
                    return null;

                response = this.responseCache[strKey];
            }

            int TimeLived = (int)((DateTime.Now.Ticks - response.TimeStamp.Ticks) / TimeSpan.TicksPerSecond);
            foreach (RR rr in response.RecordsRR)
            {
                rr.TimeLived = TimeLived;
                // The TTL property calculates its actual time to live
                if (rr.TTL == 0)
                    return null; // out of date
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
            if (response.header.RCODE != RCode.NoError)
            {
                return;
            }

            Question question = response.Questions[0];

            string strKey = question.QClass + "-" + question.QType + "-" + question.QName;

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
                        socket.SendTo(request.Data, this.dnsServers[intDnsServer]);
                        int intReceived = socket.Receive(responseMessage);
                        byte[] data = new byte[intReceived];
                        Array.Copy(responseMessage, data, intReceived);
                        Response response = new Response(this.dnsServers[intDnsServer], data);
                        AddToCache(response);
                        return response;
                    }
                    catch (SocketException)
                    {
                        Verbose(string.Format(";; Connection to nameserver {0} failed", (intDnsServer + 1)));
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

        private Response TcpRequest(Request request)
        {
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            //sw.Start();

            byte[] responseMessage = new byte[512];

            for (int intAttempts = 0; intAttempts < this.retries; intAttempts++)
            {
                for (int intDnsServer = 0; intDnsServer < this.dnsServers.Count; intDnsServer++)
                {
                    TcpClient tcpClient = new TcpClient();
                    tcpClient.ReceiveTimeout = this.Timeout;

                    try
                    {
                        IAsyncResult result = tcpClient.BeginConnect(this.dnsServers[intDnsServer].Address, this.dnsServers[intDnsServer].Port, null, null);

                        bool success = result.AsyncWaitHandle.WaitOne(this.Timeout, true);

                        if (!success || !tcpClient.Connected)
                        {
                            tcpClient.Close();
                            this.Verbose(string.Format(";; Connection to nameserver {0} failed", (intDnsServer + 1)));
                            continue;
                        }

                        BufferedStream bs = new BufferedStream(tcpClient.GetStream());

                        byte[] data = request.Data;
                        bs.WriteByte((byte)((data.Length >> 8) & 0xff));
                        bs.WriteByte((byte)(data.Length & 0xff));
                        bs.Write(data, 0, data.Length);
                        bs.Flush();

                        Response TransferResponse = new Response();
                        int intSoa = 0;
                        int intMessageSize = 0;

                        //Debug.WriteLine("Sending "+ (request.Length+2) + " bytes in "+ sw.ElapsedMilliseconds+" mS");

                        while (true)
                        {
                            int intLength = bs.ReadByte() << 8 | bs.ReadByte();
                            if (intLength <= 0)
                            {
                                tcpClient.Close();
                                this.Verbose(string.Format(";; Connection to nameserver {0} failed", (intDnsServer + 1)));
                                throw new SocketException(); // next try
                            }

                            intMessageSize += intLength;

                            data = new byte[intLength];
                            bs.Read(data, 0, intLength);
                            Response response = new Response(this.dnsServers[intDnsServer], data);

                            //Debug.WriteLine("Received "+ (intLength+2)+" bytes in "+sw.ElapsedMilliseconds +" mS");

                            if (response.header.RCODE != RCode.NoError)
                            {
                                return response;
                            }

                            if (response.Questions[0].QType != QType.AXFR)
                            {
                                this.AddToCache(response);
                                return response;
                            }

                            // Zone transfer!!

                            if(TransferResponse.Questions.Count==0)
                            {
                                TransferResponse.Questions.AddRange(response.Questions);
                            }

                            TransferResponse.Answers.AddRange(response.Answers);
                            TransferResponse.Authorities.AddRange(response.Authorities);
                            TransferResponse.Additionals.AddRange(response.Additionals);

                            if (response.Answers[0].Type == RecordType.SOA)
                            {
                                    intSoa++;
                            }

                            if (intSoa == 2)
                            {
                                TransferResponse.header.QuestionCount = (ushort)TransferResponse.Questions.Count;
                                TransferResponse.header.AnswerCount = (ushort)TransferResponse.Answers.Count;
                                TransferResponse.header.NameserverCount = (ushort)TransferResponse.Authorities.Count;
                                TransferResponse.header.AdditionalRecordsCount = (ushort)TransferResponse.Additionals.Count;
                                TransferResponse.MessageSize = intMessageSize;
                                return TransferResponse;
                            }
                        }
                    } // try
                    catch (SocketException)
                    {
                        continue; // next try
                    }
                    finally
                    {
                        this.unique++;

                        // close the socket
                        tcpClient.Close();
                    }
                }
            }
            Response responseTimeout = new Response();
            responseTimeout.Error = "Timeout Error";
            return responseTimeout;
        }

        /// <summary>
        /// Do Query on specified DNS servers
        /// </summary>
        /// <param name="name">Name to query</param>
        /// <param name="qtype">Question type</param>
        /// <param name="qclass">Class type</param>
        /// <returns>Response of the query</returns>
        public Response Query(string name, QType qtype, QClass qclass)
        {
            Question question = new Question(name, qtype, qclass);
            Response response = SearchInCache(question);
            if (response != null)
            {
                return response;
            }

            Request request = new Request();
            request.AddQuestion(question);
            return GetResponse(request);
        }

        /// <summary>
        /// Do an QClass=IN Query on specified DNS servers
        /// </summary>
        /// <param name="name">Name to query</param>
        /// <param name="qtype">Question type</param>
        /// <returns>Response of the query</returns>
        public Response Query(string name, QType qtype)
        {
            Question question = new Question(name, qtype, QClass.IN);
            Response response = SearchInCache(question);
            if (response != null)
            {
                return response;
            }

            Request request = new Request();
            request.AddQuestion(question);
            return GetResponse(request);
        }

        private Response GetResponse(Request request)
        {
            request.header.Id = this.unique;
            request.header.RD = this.recursion;

            if (this.transportType == TransportType.Udp)
            {
                return UdpRequest(request);
            }

            if (this.transportType == TransportType.Tcp)
            {
                return TcpRequest(request);
            }

            Response response = new Response();
            response.Error = "Unknown TransportType";
            return response;
        }

        /// <summary>
        /// Gets a list of default DNS servers used on the Windows machine.
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
                        IPEndPoint entry = new IPEndPoint(ipAddr, defaultPort);
                        if (!list.Contains(entry))
                        {
                            list.Add(entry);
                        }
                    }

                }
            }
            return list.ToArray();
        } 

        private IPHostEntry MakeEntry(string hostname)
        {
            IPHostEntry entry = new IPHostEntry();

            entry.HostName = hostname;

            Response response = Query(hostname, QType.A, QClass.IN);

            // fill AddressList and aliases
            List<IPAddress> AddressList = new List<IPAddress>();
            List<string> Aliases = new List<string>();
            foreach (AnswerRR answerRR in response.Answers)
            {
                if (answerRR.Type == RecordType.A)
                {
                    // answerRR.RECORD.ToString() == (answerRR.RECORD as RecordA).Address
                    AddressList.Add(IPAddress.Parse((answerRR.RECORD.ToString())));
                    entry.HostName = answerRR.NAME;
                }
                else
                {
                    if (answerRR.Type == RecordType.CNAME)
                    {
                        Aliases.Add(answerRR.NAME);
                    }
                }
            }
            entry.AddressList = AddressList.ToArray();
            entry.Aliases = Aliases.ToArray();

            return entry;
        }

        /// <summary>
        /// Translates the IPV4 or IPV6 address into an arpa address
        /// </summary>
        /// <param name="ip">IP address to get the arpa address form</param>
        /// <returns>The 'mirrored' IPV4 or IPV6 arpa address</returns>
        public static string GetArpaFromIp(IPAddress ip)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("in-addr.arpa.");
                foreach (byte b in ip.GetAddressBytes())
                {
                    sb.Insert(0, string.Format("{0}.", b));
                }
                return sb.ToString();
            }
            if (ip.AddressFamily == AddressFamily.InterNetworkV6)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("ip6.arpa.");
                foreach (byte b in ip.GetAddressBytes())
                {
                    sb.Insert(0, string.Format("{0:x}.", (b >> 4) & 0xf));
                    sb.Insert(0, string.Format("{0:x}.", (b >> 0) & 0xf));
                }
                return sb.ToString();
            }
            return "?";
        }

        /// <summary>
        /// </summary>
        public static string GetArpaFromEnum(string strEnum)
        {
            StringBuilder sb = new StringBuilder();
            string Number = System.Text.RegularExpressions.Regex.Replace(strEnum, "[^0-9]", "");
            sb.Append("e164.arpa.");
            foreach (char c in Number)
            {
                sb.Insert(0, string.Format("{0}.", c));
            }
            return sb.ToString();
        }

        private enum RRRecordStatus
        {
            UNKNOWN,
            NAME,
            TTL,
            CLASS,
            TYPE,
            VALUE
        }

        /// <summary>
        /// </summary>
        public void LoadRootFile(string strPath)
        {
            StreamReader sr = new StreamReader(strPath);
            while (!sr.EndOfStream)
            {
                string strLine = sr.ReadLine();
                if (strLine == null)
                {
                    break;
                }

                int intI = strLine.IndexOf(';');
                if (intI >= 0)
                {
                    strLine = strLine.Substring(0, intI);
                }

                strLine = strLine.Trim();
                if (strLine.Length == 0)
                {
                    continue;
                }
                
                RRRecordStatus status = RRRecordStatus.NAME;
                string Name="";
                string Ttl="";
                string Class="";
                string Type="";
                string Value="";
                string strW = "";
                for (intI = 0; intI < strLine.Length; intI++)
                {
                    char C = strLine[intI];

                    if (C <= ' ' && strW!="")
                    {
                        switch (status)
                        {
                            case RRRecordStatus.NAME:
                                Name = strW;
                                status = RRRecordStatus.TTL;
                                break;
                            case RRRecordStatus.TTL:
                                Ttl = strW;
                                status = RRRecordStatus.CLASS;
                                break;
                            case RRRecordStatus.CLASS:
                                Class = strW;
                                status = RRRecordStatus.TYPE;
                                break;
                            case RRRecordStatus.TYPE:
                                Type = strW;
                                status = RRRecordStatus.VALUE;
                                break;
                            case RRRecordStatus.VALUE:
                                Value = strW;
                                status = RRRecordStatus.UNKNOWN;
                                break;
                            default:
                                break;
                        }
                        strW = "";
                    }
                    if (C > ' ')
                        strW += C;
                }

            }
            sr.Close();
        }
    } // class
}