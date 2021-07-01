
using System;
using System.Net;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using ShimDotNet;

namespace Kebab
{
    // Describe direction of transmission.
    public enum TransmissionDirection
    {
        ONE_WAY,
        REV_ONE_WAY,
        TWO_WAY
    }

    // Describe type of connection packet match.
    public enum MatchType
    {
        MATCH,
        REV_MATCH,
        NON_MATCH
    }

    // Wrapperclass to implement IComparable for IPV4Address from pacapdotnet.
    public class ConnectionAddress : IComparable
    {
        static public UInt32 IPV4ToUint32(IPAddress address)
        {
            // Make sure this is an IPV4 address.
            if (address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
                throw new ArgumentException("Error: IPAddress is not IPV4.");

            // Could not find another way to get direct access to address bytes in numeric format.
            // IPAddress.Equals() is woefully unequipped for many actions, so stop [FN] saying that it is MS!
            #pragma warning disable CS0618

            return (UInt32)IPAddress.NetworkToHostOrder((Int32)address.Address);

            #pragma warning restore CS0618
        }

        static public string NetMaskToPrefix(IPAddress address)
        {
            // Get numeric representation of IP.
            UInt32 n = IPV4ToUint32(address);

            // Bit shift left one bit at a time untill we have a 0 value.
            UInt16 i = 0;
            for (; n > 0; i++)
                n <<= 1;

            // Convert count of one bits to notation str.
            return ("/" + i.ToString());
        }

        public IPAddress Address { get; set; }

        public ConnectionAddress(IPAddress addr) { this.Address = addr; }

        public override string ToString()
        {
            return Address.ToString();
        }

        public int CompareTo(object obj)
        {
            // Make sure we are comparing to a valid object.
            if (obj == null || !(obj is ConnectionAddress))
                throw new NotSupportedException("Error: compared object is not a ConnectionAddress type!");

            // Convert addresses into numerical value for comparison.
            UInt32 left = this.AddressValue();
            UInt32 right = ((ConnectionAddress)obj).AddressValue();

            // Compare built in type.
            return left.CompareTo(right);
        }

        public override bool Equals(Object obj)
        {
            // Check for null and compare run-time types.
            if (obj != null && obj is ConnectionAddress)
            {
                // Cast objest to usable current type.
                ConnectionAddress addr = obj as ConnectionAddress;

                if (Equals(this.Address, addr.Address))
                    return true;
            }

            return false;
        }

        // Get hash code override (cause compiler warnings got to me!).
        public override int GetHashCode()
        {
            // Just return the raw value of ip as int hash.
            return (int)this.Address.GetHashCode();
        }

        // Converts ConnectionAddress data into numerical value.
        public UInt32 AddressValue()
        {
            return IPV4ToUint32(this.Address);
        }

        public bool AddressIsLocal()
        {
            // Class A Private (10.0.0.0/8).
            const UInt32 CLASS_A_ADDRESS = 0x0A000000;
            const UInt32 CLASS_A_NETMASK = 0xFF000000;
            // Class B Private (172.16.0.0/12).
            const UInt32 CLASS_B_ADDRESS = 0xAC100000;
            const UInt32 CLASS_B_NETMASK = 0xFFF00000;
            // Class C Private (192.168.0.0/16).
            const UInt32 CLASS_C_ADDRESS = 0xC0A80000;
            const UInt32 CLASS_C_NETMASK = 0xFFFF0000;
            // Local / Loopback (127.0.0.0/8)
            const UInt32 LOOPBAK_ADDRESS = 0x7F000000;
            const UInt32 LOOPBAK_NETMASK = 0xFF000000;
            // Link Local (169.254.0.0/16)
            const UInt32 LINKLOC_ADDRESS = 0xA9FE0000;
            const UInt32 LINKLOC_NETMASK = 0xFFFF0000;
            // Multicast (224.0.0.0/4)
            const UInt32 MULTCST_ADDRESS = 0xE0000000;
            const UInt32 MULTCST_NETMASK = 0xF0000000;
            // Local Broadcast (255.255.255.255/32)
            const UInt32 BRODCST_ADDRESS = 0xFFFFFFFF;

            // Convert IP to 4 Byte segment (NetToHo doesn't like long for somereason).
            UInt32 IPValue = this.AddressValue();

            // Compares bytes to local ip ranges using netmask anding.
            if ((IPValue & CLASS_A_NETMASK) == CLASS_A_ADDRESS)
                return true;
            else if ((IPValue & CLASS_B_NETMASK) == CLASS_B_ADDRESS)
                return true;
            else if ((IPValue & CLASS_C_NETMASK) == CLASS_C_ADDRESS)
                return true;
            else if ((IPValue & LOOPBAK_NETMASK) == LOOPBAK_ADDRESS)
                return true;
            else if ((IPValue & LINKLOC_NETMASK) == LINKLOC_ADDRESS)
                return true;
            else if ((IPValue & MULTCST_NETMASK) == MULTCST_ADDRESS)
                return true;
            else if (IPValue == BRODCST_ADDRESS)
                return true;

            return false;
        }
    }

    // Class to contain connection address geo data.
    public class GeoData : IComparable
    {
        // Geographical info.
        public string Country { get; set; }
        public string CountryISO { get; set; }
        public string State { get; set; }
        public string StateISO { get; set; }
        public string City { get; set; }

        public int CompareTo(object obj)
        {
            // Make sure we are comparing to a valid object.
            if (obj == null || !(obj is GeoData))
                throw new NotSupportedException("Error: compared object is not a GeoData type!");

            // Cast objest to usable current type.
            GeoData geo = (obj as GeoData);

            // Compare numerical value based on enum.
            if (this.CountryISO.CompareTo(geo.CountryISO) > 0)
                return 1;
            else if (this.CountryISO.CompareTo(geo.CountryISO) < 0)
                return -1;
            else
            {
                if (this.StateISO.CompareTo(geo.StateISO) > 0)
                    return 1;
                else if (this.StateISO.CompareTo(geo.StateISO) < 0)
                    return -1;
                else
                    return 0;
            }
        }

        // Return the country code to the UI.
        public override string ToString()
        {
            if (this.StateISO != "--")
                return (this.CountryISO + "-" + this.StateISO);
            else
                return (this.CountryISO);
        }
    }

    // Class to wrap protocol emum (for conversion to string / printing).
    public class ConnectionType : IComparable
    {
        public L4_PROTOCOL Protocol { get; set; }

        public ConnectionType(L4_PROTOCOL protocol) { this.Protocol = protocol; }

        public override string ToString()
        {
            if (Equals(this.Protocol, L4_PROTOCOL.TCP))
                return "TCP";
            else
                return "UDP";
        }

        public int CompareTo(object obj)
        {
            // Make sure we are comparing to a valid object.
            if (obj == null || !(obj is ConnectionType))
                throw new NotSupportedException("Error: compared object is not a ConnectionType type!");

            // Cast objest to usable current type.
            ConnectionType connType = (obj as ConnectionType);

            // Compare numerical value based on enum.
            if (this.Protocol > connType.Protocol)
                return 1;
            else if (this.Protocol < connType.Protocol)
                return -1;
            else
                return 0;
        }
    }

    // Class to wrap direction emum (for conversion to string / printing).
    public class ConnectionState : IComparable
    {
        public TransmissionDirection Direction { get; set; }

        public ConnectionState(TransmissionDirection direction) { this.Direction = direction; }

        public override string ToString()
        {
            if (Equals(this.Direction, TransmissionDirection.ONE_WAY))
                return " -> ";
            else if (Equals(this.Direction, TransmissionDirection.REV_ONE_WAY))
                return " <- ";
            else
                return " <> ";
        }

        public int CompareTo(object obj)
        {
            // Make sure we are comparing against a valid object.
            if (obj == null || !(obj is ConnectionState))
                throw new NotSupportedException("Error: compared object is not a ConnectionState type!");

            // Cast objest to usable current type.
            ConnectionState connState = (obj as ConnectionState);

            //Compare numerical value based on enum
            if (this.Direction > connState.Direction)
                return 1;
            else if (this.Direction < connState.Direction)
                return -1;
            else
                return 0;
        }
    }

    // Connection object for connections list.
    public class Connection : INotifyPropertyChanged
    {
        // Connection number in connection list.
        private uint _number;
        public uint Number
        {
            get
            {
                return _number;
            }
            set
            {
                if (value != _number)
                {
                    _number = value;
                    NotifyPropertyChanged();
                }
            }
        }
        // IP protcol (tcp / udp).
        public ConnectionType Type { get; set; }
        // Direction of transmission of packets.
        private ConnectionState _state;
        public ConnectionState State
        {
            get
            {
                return _state;
            }
            set
            {
                if (value != _state)
                {
                    _state = value;
                    NotifyPropertyChanged();
                }
            }
        }
        // Local IP address (sender).
        public ConnectionAddress Source { get; set; }
        // Remote IP address (destination).
        public ConnectionAddress Destination { get; set; }
        // GeoIP data object for the Remote / Destination ip addr.
        public GeoData DstGeo { get; set; }
        // Autonomous system number for remote address.
        public long? DstASN { get; set; }
        // ASN registered organization name for remote address.
        public string DstASNOrg { get; set; }
        // Protocol local port (sender port).
        public UInt16 SrcPort { get; set; }
        // Protocol remote port (destination port).
        public UInt16 DstPort { get; set; }
        // Number of packets seen matching this connection (change forces list view update).
        private UInt64 _packetCount;
        public UInt64 PacketCount
        {
            get
            {
                return _packetCount;
            }
            set
            {
                if (value != _packetCount)
                {
                    _packetCount = value;
                    NotifyPropertyChanged();
                }
            }
        }
        // Data size (in bytes) of protocol payload segments (change forces list view update).
        private UInt64 _byteCount;
        public UInt64 ByteCount
        {
            get
            {
                return _byteCount;
            }
            set
            {
                if (value != _byteCount)
                {
                    _byteCount = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Timestamp for checking if this is an old / dead connection.
        public DateTime TimeStamp { get; set; }

        // Ctor allows conversion from L4Packet to Connection.
        public Connection(IPV4_PACKET pkt)
        {
            this.Type = new ConnectionType(pkt.protocol);
            this.State = new ConnectionState(TransmissionDirection.ONE_WAY);
            this.Source = new ConnectionAddress(pkt.source_address);
            this.Destination = new ConnectionAddress(pkt.destination_address);
            this.SrcPort = pkt.source_port;
            this.DstPort = pkt.destination_port;
            this.DstGeo = new GeoData();
            this.PacketCount = 1;
            this.ByteCount = pkt.payload_size;
            this.TimeStamp = DateTime.Now;
        }

        // Implement INotifyPropertyChanged (for auto bind list updating).
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Match packet to connection (-1 = no match, 0 = match, 1 = reverse match.
        public MatchType PacketMatch(IPV4_PACKET pkt)
        {
            // Check protocol first (biggest devider of connections).
            if (!Equals(this.Type.Protocol, pkt.protocol))
                return MatchType.NON_MATCH;

            // Check if IP's and ports (or inverse) match.
            if ((Equals(this.Source.Address, pkt.source_address) && Equals(this.SrcPort, pkt.source_port))
                && (Equals(this.Destination.Address, pkt.destination_address) && Equals(this.DstPort, pkt.destination_port)))
                return MatchType.MATCH;
            else if ((Equals(this.Source.Address, pkt.destination_address) && Equals(this.SrcPort, pkt.destination_port))
                     && (Equals(this.Destination.Address, pkt.source_address) && Equals(this.DstPort, pkt.source_port)))
                return MatchType.REV_MATCH;

            return MatchType.NON_MATCH;
        }

        // Match pre-existing connections (Equals() is check value, == is check reference).
        public override bool Equals(Object obj)
        {
            // Check for null and compare run-time types.
            if (obj != null && (obj is Connection))
            {
                // Cast objest to usable current type.
                Connection conn = (obj as Connection);

                // Check protocol first (biggest devider of connections).
                if (!Equals(this.Type.Protocol, conn.Type.Protocol))
                    return false;

                // Check if IP's and ports (or inverse) match.
                if ((Equals(this.Source, conn.Source) && Equals(this.SrcPort, conn.SrcPort))
                    && (Equals(this.Destination, conn.Destination) && Equals(this.DstPort, conn.DstPort)))
                    return true;

                return false;
            }

            return false;
        }
        // Get hash code override (cause compiler warnings got to me!).
        public override int GetHashCode()
        {
            // Just xor address port combos to get a fairly collision resistent hash.
            int result = (this.Source.Address.GetHashCode() + this.SrcPort.GetHashCode());
            result ^= (this.Destination.Address.GetHashCode() + this.DstPort.GetHashCode());

            return result;
        }
        // ToString override (cause I can).
        public override string ToString()
        {
            return (this.Source.ToString() + ":" + this.SrcPort.ToString() + " " + this.State.ToString()
                    + " " + this.Destination.ToString() + ":" + this.DstPort.ToString());
        }
    }

    // Simple class to extend BindingList to add Searching and Sorting.
    public class ConnectionList : BindingList<Connection>
    {
        // Check if Searching is Supported.
        protected override bool SupportsSearchingCore
        {
            get { return true; }
        }

        // Search Funtionality.
        protected override int FindCore(PropertyDescriptor prop, object key)
        {
            // Make sure key isn't empty / invalid.
            if (prop != null && key != null)
            {
                // Loop through the items to see if the key value matches the property value.
                foreach (Connection conn in (this.Items as List<Connection>))
                {
                    if (Equals(prop.GetValue(conn), key))
                        return IndexOf(conn);
                }
            }

            // Retrun -1 if the item could not be found.
            return -1;
        }

        // Search Accessor.
        public int Find(PropertyDescriptor prop, object key)
        {
            return FindCore(prop, key);
        }

        // Check if Sorting Is Supported.
        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        // Check if Sorting Has Occurred.
        private bool _isSortedValue = false;
        protected override bool IsSortedCore
        {
            get { return _isSortedValue; }
        }

        // Allow external functions to access what sort direction is bneing used.
        private ListSortDirection _sortDirectionValue;
        protected override ListSortDirection SortDirectionCore
        {
            get { return _sortDirectionValue; }
        }
        // Allow external functions to access what property is being used to sort the list.
        private PropertyDescriptor _sortPropertyValue;
        protected override PropertyDescriptor SortPropertyCore
        {
            get { return _sortPropertyValue; }
        }

        // Determine if two items in list need to be exchanged during insertion sort.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ExchangeTest(IList<Connection> connList, Connection key, int index,
                                         PropertyDescriptor prop, ListSortDirection direction)
        {
            if (Equals(direction, ListSortDirection.Ascending))
            {
                if (((IComparable)prop.GetValue(key)).CompareTo((IComparable)prop.GetValue(connList[index])) < 0)
                    return true;
            }
            else
            {
                if (((IComparable)prop.GetValue(key)).CompareTo((IComparable)prop.GetValue(connList[index])) > 0)
                    return true;
            }

            return false;
        }

        // Used to prevent excessive ascending Number sorting.
        private bool _alreadyInOrder = true;
        // Tracks stage of sorting.
        private bool _suppressSort = false;
        private bool _suppressNotification = false;

        // Sort Functionallity.
        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            // Set internal property and direction.
            _sortPropertyValue = prop;
            _sortDirectionValue = direction;

            // Don't allow sort to be called by OnListChanged() until the sort is over.
            _suppressSort = true;
            // Stop stack overflow and binded events.
            _suppressNotification = true;

            // Only do an ascending sort by "Number" once.
            if (prop.DisplayName == "Number" && direction == ListSortDirection.Ascending && _alreadyInOrder)
            {
                // Indicate that a sort has occured.
                if (!_isSortedValue)
                    _isSortedValue = true;

                // Stop suppressing notification events.
                _suppressNotification = false;

                // Stop suppressing sorting events.
                _suppressSort = false;

                // Don't sort the list.
                return;
            }

            // Inserstion sort.
            for (int i = 1; i < this.Count; i++)
            {
                // Setup insertion parms.
                Connection key = this[i];

                // Sorted section insertion loop.
                for (int j = (i - 1); j >= 0 && ConnectionList.ExchangeTest(this.Items, key, j, _sortPropertyValue,
                                                                            _sortDirectionValue); j--)
                {
                    this[j + 1] = this[j];
                    this[j] = key;
                }
            }

            // If an ascending sort by "Number" was performed, than we don't need to keep sorting on list changes.
            if (prop.DisplayName == "Number" && direction == ListSortDirection.Ascending)
                _alreadyInOrder = true;
            else
                _alreadyInOrder = false;

            // Indicate that a sort has occured.
            if (!_isSortedValue)
                _isSortedValue = true;

            // Stop suppressing notification events.
            _suppressNotification = false;

            // Stop suppressing sorting events.
            _suppressSort = false;
        }

        // Sort Accessor.
        public void ApplySort(PropertyDescriptor prop, ListSortDirection direction)
        {
            ApplySortCore(prop, direction);
        }

        // Sort Removing Functionallity (doesn't revert sort, just disables sorting functionallity).
        protected override void RemoveSortCore()
        {
            // Ensure the list has been sorted.
            if (_isSortedValue)
            {
                // Reset sorted vars.
                _isSortedValue = false;
                _sortDirectionValue = default;
                _sortPropertyValue = default;
            }
        }

        // Sort Removing Accessor.
        public void RemoveSort()
        {
            RemoveSortCore();
        }

        // Ensure sorting occurs when elements of the list are updated.
        protected override void OnListChanged(ListChangedEventArgs args)
        {
            // Don't allow any processing if flag is set.
            if (_suppressNotification)
                return;

            // Don't call sort from end of sort function, but allow sorting of list in general on item change.
            if (_isSortedValue == true && !_suppressSort)
                ApplySortCore(_sortPropertyValue, _sortDirectionValue);

            // Do base operations.
            base.OnListChanged(args);
        }
    }
}
