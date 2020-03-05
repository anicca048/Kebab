
using System;
using System.Collections.Generic;

using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;

namespace Kebab
{
    // Custom exception class for CaptureSession errors.
    [Serializable()]
    public class CaptureSessionException : System.Exception
    {
        public CaptureSessionException() : base() { }
        public CaptureSessionException(string message) : base(message) { }
        public CaptureSessionException(string message, System.Exception inner) : base(message, inner) { }

        protected CaptureSessionException(System.Runtime.Serialization.SerializationInfo info,
                  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    // Connection filter object (based on gui filter options) gets turned into a BerkleyPacketFilter.
    public class SessionFilter
    {
        // Layer 4 Protocols to use for filtering.
        public bool UDP { get; set; }
        public bool TCP { get; set; }

        // IPV4Addresses.
        // Getter and setter for source address (setter auto checks address validity).
        private IpV4Address _sourceIP;
        public string SourceIP
        {
            get
            {
                if (_sourceIP == default(IpV4Address))
                    return default(string);
                else
                    return _sourceIP.ToString();
            }
            set
            {
                // Make sure ipstring is valid, by attempting to convert it to an IPV4Address.
                if (!IpV4Address.TryParse(value, out _sourceIP))
                {
                    _sourceIP = default(IpV4Address);
                    throw new CaptureSessionException("Error: source IP string is invalid!");
                }
            }
        }

        // Getter and setter for destination address (setter auto checks address validity).
        private IpV4Address _destinationIP;
        public string DestinationIP
        {
            get
            {
                if (_destinationIP == default(IpV4Address))
                    return default(string);
                else
                    return _destinationIP.ToString();
            }
            set
            {
                // Make sure ipstring is valid, by attempting to convert it to an IPV4Address.
                if (!IpV4Address.TryParse(value, out _destinationIP))
                {
                    _sourceIP = default(IpV4Address);
                    throw new CaptureSessionException("Error: destination IP string is invalid!");
                }
            }
        }

        // Protocol port numbers.
        // Getter and setter for source port (setter auto checks port validity).
        public ushort _sourcePort;
        public string SourcePort
        {
            get
            {
                if (_sourcePort == default(ushort))
                    return default(string);
                else
                    return _sourcePort.ToString();
            }
            set
            {
                // Make sure port is valid, by attempting to convert it to an ushort.
                if (!ushort.TryParse(value, out _sourcePort) || _sourcePort == 0)
                {
                    _sourcePort = default(ushort);
                    throw new CaptureSessionException("Error: source Port string is invalid!");
                }
            }
        }

        // Getter and setter for destination port (setter auto checks port validity).
        public ushort _destinationPort;
        public string DestinationPort
        {
            get
            {
                if (_destinationPort == default(ushort))
                    return default(string);
                else
                    return _destinationPort.ToString();
            }
            set
            {
                // Make sure port is valid, by attempting to convert it to an ushort.
                if (!ushort.TryParse(value, out _destinationPort) || _destinationPort == 0)
                {
                    _sourcePort = default(ushort);
                    throw new CaptureSessionException("Error: destination Port string is invalid!");
                }
            }
        }

        // Check if the filter is the program default (returns false if user specified custom options).
        public bool IsDefaultState()
        {
            if ((TCP && UDP) && (_sourceIP == default(IpV4Address)) && (_destinationIP == default(IpV4Address)) &&
                (_sourcePort == default(ushort)) && (_destinationPort == default(ushort)))
                return true;
            else
                return false;
        }
    }

    public class CaptureSession
    {
        // Device and communicator for opening pcap live interface.
        private PacketDevice _packetDevice;
        private PacketCommunicator _packetCommunicator;

        // Capture filter object and compiled pcap filter var.
        private BerkeleyPacketFilter _packetFilter;

        // Pcap devices for use with PcapDotNet.
        private IList<LivePacketDevice> _deviceList;

        // Pcap devices for gui display.
        private List<string> _deviceDisplayList;
        public List<string> DeviceDisplayList
        {
            get { return _deviceDisplayList; }
        }

        public CaptureSession()
        {
           // Obtain host information?
        }
        ~CaptureSession()
        {
            // Check if user initialized object.
            if (_packetCommunicator != default(PacketCommunicator))
                _packetCommunicator.Dispose();

            // Check if user initialized object.
            if (_packetFilter != default(BerkeleyPacketFilter))
                _packetFilter.Dispose();
        }

        public void SetupDeviceList()
        {
            // Retrieve the device list from the local machine.
            try
            {
                // Make sure we reset list incase this is called from the refresh interfaces button.
                if (_deviceList != default(IList<LivePacketDevice>))
                    _deviceList = default(IList<LivePacketDevice>);

                // Get list of interfaces for this machine.
                _deviceList = LivePacketDevice.AllLocalMachine;
            }
            catch (System.InvalidOperationException ex)
            {
                throw new CaptureSessionException("Error: failed retrieving device list!\n"
                                                  + "Make sure WinPcap / Npcap is properly installed!\n"
                                                  + "(and that you have the necessary permissions if using Npcap)\n\n"
                                                  + ex.Message);
            }

            // Ensure that we have at least one pcap interface.
            if (_deviceList.Count == 0)
            {
                throw new CaptureSessionException("Error: no interfaces found! Make sure WinPcap / Npcap is properly installed!");
            }

            // Make sure list is empty.
            _deviceDisplayList = new List<string>();

            // Convert device names and descriptions for gui use.
            for (int i = 0; i != _deviceList.Count; ++i)
            {
                // Description looks like: "Network Adapter 'Loopback Adapter' on local host".
                if (_deviceList[i].Description != null)
                    _deviceDisplayList.Add((i + 1).ToString() + ".) " + _deviceList[i].Description.ToString().Split('\'')[1] +
                                            " [ " + GetDeviceIPV4Address(_deviceList[i]) + " ]");
                else
                    _deviceDisplayList.Add((i + 1).ToString() + ".) " + "No Device Description");
            }
        }

        // Find first IPV4 address of interface (if there is one).
        private string GetDeviceIPV4Address(IPacketDevice device)
        {
            foreach (DeviceAddress address in device.Addresses)
            {
                // Check if address is vaild IPV4 address.
                if (address.Address.Family.ToString() == "Internet")
                {
                    if (address.Address != null)
                    {
                        // Address looks like: "Internet 0.0.0.0".
                        string ip = address.Address.ToString().Split(' ')[1];

                        // Dont return a BIND_ANY address.
                        if (ip != "0.0.0.0")
                        {
                            return ip;
                        }
                    }
                }
            }

            // Return empty string if no IP was found.
            return "";
        }

        // Captures a packet, returns -1 if invalid packet was recieved.
        public int CapturePacket(out L4Packet pkt)
        {
            try
            {
                // Connection object that will be either a tcp or udp packet.
                pkt = new L4Packet();
                // Packet used to build connection object.
                Packet pcapPacket;

                // Check if we have recieved a usefull packet.
                PacketCommunicatorReceiveResult result = _packetCommunicator.ReceivePacket(out pcapPacket);

                switch (result)
                {
                    case PacketCommunicatorReceiveResult.Timeout:
                    {
                        // Timeout elapsed.
                        return -1;
                    }
                    case PacketCommunicatorReceiveResult.Ok:
                    {
                        // Process packet info.
                        break;
                    }
                    default:
                    {
                        // This should never happen, but the PcapDotNet docs said one should have it just in case, so here it is.
                        throw new CaptureSessionException("Error: reached default case in packet processing!");
                    }
                }

                // Convert packet into an IPv4 packet or die.
                IpV4Datagram IPV4Packet;

                if (_packetCommunicator.DataLink.Kind == DataLinkKind.Ethernet)
                    IPV4Packet = pcapPacket.Ethernet.IpV4;
                else if (_packetCommunicator.DataLink.Kind == DataLinkKind.IpV4)
                    IPV4Packet = pcapPacket.IpV4;
                else
                    return -1;

                // Add IPv4 packet info to connection.
                pkt.Source = IPV4Packet.Source;
                pkt.Destination = IPV4Packet.Destination;

                // Set protocol / make sure it's a valid one.
                if (IPV4Packet.Protocol.ToString().ToUpper() == "TCP")
                    pkt.Protocol = L4Protocol.TCP;
                else if (IPV4Packet.Protocol.ToString().ToUpper() == "UDP")
                    pkt.Protocol = L4Protocol.UDP;
                else
                    return -1;

                // Add protocol packet info to connection.
                if (pkt.Protocol == L4Protocol.TCP)
                {
                    // Proccess TCP packet and add it to new connection object.
                    TcpDatagram TCPPacket = IPV4Packet.Tcp;

                    pkt.SrcPort = TCPPacket.SourcePort;
                    pkt.DstPort = TCPPacket.DestinationPort;
                    pkt.PayloadSize = ((uint)(TCPPacket.Payload.Length));
                }
                else
                {
                    // Process UDP packet and add it to new connection object.
                    UdpDatagram UDPPacket = IPV4Packet.Udp;

                    pkt.SrcPort = UDPPacket.SourcePort;
                    pkt.DstPort = UDPPacket.DestinationPort;
                    pkt.PayloadSize = ((uint)(UDPPacket.Payload.Length));
                }

                pkt.Direction = TransmissionDirection.ONE_WAY;

                return 0;
            }
            // PcapDotNet uses InvalidOperationException.
            catch (InvalidOperationException ex)
            {
                throw new CaptureSessionException("Error: packet capturing loop failed!\n\n" + ex.Message);
            }
        }

        // Packet capture loop initializer.
        public void SetupCapture(int deviceIndex, SessionFilter simpleFilter, string complexFilterStr)
        {
            try
            {
                // Pcap Live Session cleanup.
                if (_packetCommunicator != default(PacketCommunicator))
                {
                    _packetCommunicator.Dispose();
                    _packetCommunicator = default(PacketCommunicator);
                }

                // Compiled filter cleanup.
                if (_packetFilter != default(BerkeleyPacketFilter))
                {
                    _packetFilter.Dispose();
                    _packetFilter = default(BerkeleyPacketFilter);
                }

                // Pcap interface cleanup.
                if (_packetDevice != default(PacketDevice))
                {
                    _packetDevice = default(PacketDevice);
                }

                // Select a packet device from list based on index.
                _packetDevice = _deviceList[deviceIndex];
                // Crate packet comunicator (Pcap Live Session) for processing packets.
                _packetCommunicator = _packetDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000);

                // Make sure it is a supported device type.
                if (_packetCommunicator.DataLink.Kind != DataLinkKind.Ethernet &&
                    _packetCommunicator.DataLink.Kind != DataLinkKind.IpV4)
                    throw new CaptureSessionException(("Error: device: " + _packetDevice.Description + " is not an Ethernet or raw IP device!"));

                // Create string that will become the libpcap packet filter.
                string packetFilterStr = String.Empty;

                // Set compiled packet filter.
                if (simpleFilter.IsDefaultState())
                {
                    // Create packet filter with default options.
                    packetFilterStr = "(tcp or udp)";
                }
                else
                {
                    // Set all or one protocol.
                    if (simpleFilter.TCP && simpleFilter.UDP)
                        packetFilterStr += "(tcp or udp) and ";
                    else if (simpleFilter.TCP)
                        packetFilterStr += "tcp and ";
                    else if (simpleFilter.UDP)
                        packetFilterStr += "udp and ";

                    // Set src ip and or src port and or dst ip and or dst port.
                    if (simpleFilter.SourceIP != default(string))
                        packetFilterStr += ("src host " + simpleFilter.SourceIP + " and ");
                    if (simpleFilter.SourcePort != default(string))
                        packetFilterStr += ("src port " + simpleFilter.SourcePort + " and ");
                    if (simpleFilter.DestinationIP != default(string))
                        packetFilterStr += ("dst host " + simpleFilter.DestinationIP + " and ");
                    if (simpleFilter.DestinationPort != default(string))
                        packetFilterStr += ("dst port " + simpleFilter.DestinationPort + " and ");

                    // Strip trailing (and ) from string.
                    packetFilterStr = packetFilterStr.Remove(packetFilterStr.Length - 5);
                }

                // Add complex filter to the end if it was used.
                if (complexFilterStr != String.Empty)
                    packetFilterStr += " and " + complexFilterStr;

                // Create packet filter from user options.
                _packetFilter = _packetCommunicator.CreateFilter(packetFilterStr);

                _packetCommunicator.SetFilter(_packetFilter);
            }
            // PcapDotNet uses InvalidOperationException.
            catch (InvalidOperationException ex)
            {
                throw new CaptureSessionException("Error: failed seting up capture session!\n\n" + ex.Message);
            }
            // Trigers for things like BSD loopback device.
            catch (System.NotSupportedException)
            {
                throw new CaptureSessionException(("Error: device: " + _deviceDisplayList[deviceIndex + 1] + " is not an Ethernet or raw IP device!"));
            }
            // Thrown on failed filter compilation.
            catch (System.ArgumentException ex)
            {
                throw new CaptureSessionException("Error: failed seting capture filter!\n\n" + ex.Message);
            }
        }
    }
}
