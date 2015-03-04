using Fluid.Web;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.Http;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Fluid
{
    public class FluidWire : IDisposable
    {
        private byte[][] m_PlayerIOAddresses;
        private bool m_Sniff = false;
        private bool m_WaitingForResponse = false;
        private DateTime m_RequestTimestamp;
        private double m_Lag;

        private Thread m_SniffThread;

        private string m_RelativeUriTarget;
        private NameValueCollection m_QuerySniffed;

        private bool m_Disposed;
        private FluidLog m_Log;
        private PacketCommunicator m_Communicator;

        /// <summary>
        /// Gets the playerio server lag
        /// </summary>
        public double PlayerIOServerLag { get { return m_Lag; } }

        /// <summary>
        /// Gets if currently sniffing requests
        /// </summary>
        public bool SniffingRequests
        {
            get
            {
                return m_Sniff;
            }
        }

        /// <summary>
        /// The event for when playerio request was sniffed
        /// </summary>
        public event PlayerIORequestSniffedHandler OnPlayerIORequestSniffed;

        /// <summary>
        /// Gets the address's bytes
        /// </summary>
        /// <param name="address">The IPV4 address</param>
        private byte[] GetAddressBytes(IpV4Address address)
        {
            uint addressValue = address.ToValue();
            return new byte[]
            {
                (byte)(addressValue >> 24 & 255L),
                (byte)(addressValue >> 16 & 255L),
                (byte)(addressValue >> 8 & 255L),
                (byte)(addressValue & 255L)
            };
        }

        /// <summary>
        /// Gets the player io api service's ip address
        /// </summary>
        /// <returns></returns>
        private byte[][] GetPlayerIOAddresses()
        {
            IPAddress[] addresses = Dns.GetHostAddresses("api.playerio.com"); //api.gameanalytics.com

            byte[][] addressesBytes = new byte[addresses.Length][];
            for (int i = 0; i < addresses.Length; i++)
            {
                addressesBytes[i] = addresses[i].GetAddressBytes();
            }

            return addressesBytes;
        }

        /// <summary>
        /// Chceks if the IPV4 Address is the player io api
        /// </summary>
        /// <param name="address">The IPV4 Address</param>
        private bool IsPlayerIOApi(IpV4Address address)
        {
            if (m_PlayerIOAddresses == null)
            {
                return false;
            }

            byte[] addressBytes = GetAddressBytes(address);
            for (int j = 0; j < m_PlayerIOAddresses.Length; j++)
            {
                for (int i = 0; i < addressBytes.Length; i++)
                {
                    if (addressBytes[i] != m_PlayerIOAddresses[j][i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Sniff's for a url query
        /// </summary>
        /// <param name="relativeUri">The relative uri</param>
        /// <param name="timeout">The timeout in milliseconds</param>
        /// <returns>The parsed query; otherwise null</returns>
        public NameValueCollection SniffQuery(string relativeUri, int timeout)
        {
            if (m_Disposed)
            {
                if (m_Log == null)
                {
                    return null;
                }

                m_Log.Add(FluidLogCategory.Fail, "The wire is already disposed.");
                return null;
            }


            if (m_Communicator == null)
            {
                if (m_Log != null)
                {
                    m_Log.Add(FluidLogCategory.Fail, "Must Open the wire before sniffing for packets.");
                }

                return null;
            }

            m_QuerySniffed = null;
            m_RelativeUriTarget = relativeUri;

            Stopwatch sniffStopwatch = Stopwatch.StartNew();
            while (sniffStopwatch.ElapsedMilliseconds <= timeout && m_QuerySniffed == null && !m_Disposed)
            {
                m_Communicator.ReceivePackets(1, PacketQueryHandler);
            }

            return m_QuerySniffed;
        }

        /// <summary>
        /// Start asyncronously sniffing playerio api requests 
        /// </summary>
        public void StartSniffingPlayerIORequests()
        {
            if (m_Disposed)
            {
                if (m_Log == null)
                {
                    return;
                }

                m_Log.Add(FluidLogCategory.Fail, "The wire is already disposed.");
                return;
            }

            if (m_Communicator == null)
            {
                if (m_Log == null)
                {
                    return;
                }

                if (m_Log != null)
                {
                    m_Log.Add(FluidLogCategory.Fail, "Must Open the wire before sniffing for packets.");
                }

                return;
            }

            if (m_SniffThread == null)
            {
                CreateAndStartThread();
            }
            else
            {
                if (m_SniffThread.ThreadState == System.Threading.ThreadState.Running)
                {
                    if (m_Log == null)
                    {
                        return;
                    }

                    m_Log.Add(FluidLogCategory.Suggestion, "Consider checking if the wire is already sniffing before starting to sniff requests.");
                    return;
                }

                CreateAndStartThread();
            }
        }

        /// <summary>
        /// Creates and starts a sniffing thread
        /// </summary>
        private void CreateAndStartThread()
        {
            m_Sniff = true;

            m_SniffThread = new Thread(SniffPlayerIORequests);
            m_SniffThread.Name = "FluidSniffer";
            m_SniffThread.Start();
        }

        /// <summary>
        /// Stops sniffing for player io requests
        /// </summary>
        public void StopSniffingPlayerIORequests()
        {
            if (m_Disposed)
            {
                if (m_Log == null)
                {
                    return;
                }

                m_Log.Add(FluidLogCategory.Fail, "The wire is already disposed.");
                return;
            }

            m_Sniff = false;

            if (m_SniffThread == null)
            {
                m_Log.Add(FluidLogCategory.Suggestion, "Consider checking if the wire is already sniffing before attempting to stop the sniffing.");
                return;
            }

            if (m_SniffThread.ThreadState == System.Threading.ThreadState.Running)
            {
                m_SniffThread.Abort();
            }
        }

        /// <summary>
        /// Sniffing thread
        /// </summary>
        private void SniffPlayerIORequests()
        {
            try
            {
                m_PlayerIOAddresses = GetPlayerIOAddresses();

                while (m_Sniff && !m_Disposed)
                {
                    m_Communicator.ReceivePackets(1, PlayerIOPacketHandler);
                }
            }
            catch (ThreadAbortException)
            {
                //Handled
            }
            catch (InvalidOperationException)
            {
                if (m_Log == null)
                {
                    return;
                }

                m_Log.Add(FluidLogCategory.Message, "The wire was disposed while sniffing. To stop the sniffing without disposing call StopSniffingPlayerIORequests()");
            }
            catch (Exception)
            {
                if (m_Log == null)
                {
                    return;
                }

                m_Log.Add(FluidLogCategory.Message, "Unhandled exception occurred in Fluid wire sniffing. Sniffing stopped.");
            }
        }

        /// <summary>
        /// Handles a player io request
        /// </summary>
        /// <param name="packet"></param>
        private void PlayerIOPacketHandler(Packet packet)
        {
            IpV4Datagram ip = packet.Ethernet.IpV4;

            TcpDatagram tcp = ip.Tcp;
            if (tcp == null)
            {
                return;
            }

            IReadOnlyCollection<HttpDatagram> httpParts = tcp.HttpCollection;
            if (httpParts != null)
            {
                using (IEnumerator<HttpDatagram> parts = httpParts.GetEnumerator())
                {
                    while (parts.MoveNext())
                    {
                        HttpDatagram cur = parts.Current;
                        if (tcp.Http.IsRequest)
                        {
                            if (IsPlayerIOApi(ip.Destination))
                            {
                                HttpRequestDatagram httpRequest = (HttpRequestDatagram)tcp.Http;
                                if (httpRequest.Uri != null)
                                {
                                    WebHeaderCollection headerCollection = new WebHeaderCollection();
                                    NameValueCollection headerValues = new NameValueCollection();
                                    using (IEnumerator<HttpField> headers = httpRequest.Header.GetEnumerator())
                                    {
                                        while (headers.MoveNext())
                                        {
                                            headerValues.Add(headers.Current.Name, headers.Current.ValueString);
                                        }
                                    }

                                    headerCollection.Add(headerValues);

                                    byte[] bodyBytes = null;
                                    using (MemoryStream bodyStream = httpRequest.Body.ToMemoryStream())
                                    {
                                        bodyBytes = bodyStream.ToArray();
                                    }

                                    string requestUriRaw = string.Format("http://api.playerio.com{0}", httpRequest.Uri);
                                    PlayerIORequest sniffed = new PlayerIORequest(httpRequest.Method.Method, new Uri(requestUriRaw), headerCollection, bodyBytes);

                                    if (!m_WaitingForResponse)
                                    {
                                        m_RequestTimestamp = DateTime.Now;
                                    }
                                    m_WaitingForResponse = true;

                                    if (OnPlayerIORequestSniffed != null)
                                    {
                                        OnPlayerIORequestSniffed(sniffed);
                                    }
                                }
                            }
                        }
                        else if (tcp.Http.IsResponse && m_WaitingForResponse)
                        {
                            m_WaitingForResponse = false;

                            TimeSpan responseSpan = DateTime.Now.Subtract(m_RequestTimestamp);
                            m_Lag = responseSpan.TotalMilliseconds;
                        }
                    }
                }
            }
            
        }

        /// <summary>
        /// Packet handler
        /// </summary>
        /// <param name="packet">The packet to handle</param>
        private void PacketQueryHandler(Packet packet)
        {
            IpV4Datagram ip = packet.Ethernet.IpV4;

            TcpDatagram tcp = ip.Tcp;
            if (tcp == null)
            {
                return;
            }

            IReadOnlyCollection<HttpDatagram> httpParts = tcp.HttpCollection;
            if (httpParts != null)
            {
                using (IEnumerator<HttpDatagram> parts = httpParts.GetEnumerator())
                {
                    while (parts.MoveNext())
                    {
                        HttpDatagram cur = parts.Current;
                        if (tcp.Http.IsRequest)
                        {
                            HttpRequestDatagram httpRequest = (HttpRequestDatagram)tcp.Http;
                            if (httpRequest.Uri != null)
                            {
                                string uriRequestEncoded = httpRequest.Uri;
                                string uriDecoded = HttpUtility.UrlDecode(uriRequestEncoded);
                                if (uriDecoded.StartsWith(m_RelativeUriTarget))
                                {
                                    m_QuerySniffed = HttpUtility.ParseQueryString(uriRequestEncoded);
                                    break;
                                }
                            }
                        }
                        else if (tcp.Http.IsResponse)
                        {

                        }
                    }
                }
            }
        }

        /// <summary>
        /// Opens the wire
        /// </summary>
        /// <returns>If the task was successful</returns>
        public bool Open()
        {
            if (m_Disposed)
            {
                if (m_Log == null)
                {
                    return false;
                }

                m_Log.Add(FluidLogCategory.Fail, "The wire is already disposed.");
                return false;
            }

            Console.WriteLine("Opening...");
            IList<LivePacketDevice> allDevices = null;
            try
            {
                allDevices = LivePacketDevice.AllLocalMachine;
            }
            catch
            {
                if (m_Log != null)
                {
                    m_Log.Add(FluidLogCategory.Message, "Failed to read packet devices from machine. IO error.");
                }

                Console.WriteLine("Failed.");
                return false;
            }

            Console.WriteLine("Found devives: {0}", allDevices.Count);
            if (allDevices.Count == 0)
            {
                return false;
            }

            LivePacketDevice adapter = allDevices[0];

            Console.WriteLine("Calling open()");
            m_Communicator = adapter.Open();

            Console.WriteLine("Opened!");
            return true;
        }

        /// <summary>
        /// Disposes the wire communicator
        /// </summary>
        public void Dispose()
        {
            if (m_Disposed)
            {
                return;
            }

            m_Disposed = true;
            if (SniffingRequests)
            {
                StopSniffingPlayerIORequests();
            }

            if (m_Communicator != null)
            {
                m_Communicator.Dispose();
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Creates a packet sniffer
        /// </summary>
        /// <param name="logger">The logger</param>
        public FluidWire(FluidLog logger)
        {
            this.m_Log = logger;
        }

        /// <summary>
        /// Destroys the wire
        /// </summary>
        ~FluidWire()
        {
            if (!m_Disposed)
            {
                this.Dispose();
            }
        }
    }
}
