
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using PcapDotNet.Packets.IpV4;

namespace Kebab
{
    // Describe layer 4 protocol of connection.
    public enum L4Protocol
    {
        TCP,
        UDP
    }

    // Describe direction of transmission.
    public enum TransmissionDirection
    {
        ONE_WAY,
        REV_ONE_WAY,
        TWO_WAY
    }

    // Describe type of connection packet match.
    public enum Match
    {
        NO_MATCH,
        MATCH,
        REV_MATCH
    }

    // Wrapperclass to implement IComparable for IPV4Address from pacapdotnet.
    public class ConnectionAddress : IComparable
    {
        public IpV4Address Address { get; set; }

        public ConnectionAddress(IpV4Address addr) { this.Address = addr; }

        public override string ToString()
        {
            return Address.ToString();
        }

        public int CompareTo(object obj)
        {
            // Make sure we are comparing to a valid object.
            if ((obj == null) || !(obj is ConnectionAddress))
                throw new NotSupportedException("Error: compared object is not a IP4Address type!");

            // Cast objest to usable current type.
            ConnectionAddress addr = (obj as ConnectionAddress);

            // Compare numerical ip value.
            if (this.Address.ToValue() > addr.Address.ToValue())
                return 1;
            else if (this.Address.ToValue() < addr.Address.ToValue())
                return -1;
            else
                return 0;
        }

        public override bool Equals(Object obj)
        {
            // Check for null and compare run-time types.
            if ((obj != null) && (obj is ConnectionAddress))
            {
                // Cast objest to usable current type.
                ConnectionAddress addr = (obj as ConnectionAddress);

                if (Equals(this.Address.ToValue(), addr.Address.ToValue()))
                    return true;
            }

            return false;
        }

        // Get hash code override (cause compiler warnings got to me!).
        public override int GetHashCode()
        {
            // Just return the raw value of ip as int hash.
            return ((int)(this.Address.ToValue()));
        }

        public bool AddressIsLocal()
        {
            // Class A (10.0.0.0/8).
            const UInt32 CLASS_A_ADDRESS = 0x0A000000;
            const UInt32 CLASS_A_NETMASK = 0xFF000000;
            // Class B (172.16.0.0/12).
            const UInt32 CLASS_B_MINADDR = 0xAC100000;
            const UInt32 CLASS_B_MAXADDR = 0xAC1F0000;
            const UInt32 CLASS_B_NETMASK = 0xFFF00000;
            // Class C (192.168.0.0/16).
            const UInt32 CLASS_C_ADDRESS = 0xC0A80000;
            const UInt32 CLASS_C_NETMASK = 0xFFFF0000;
            //Local / Loopback (127.0.0.0/8)
            const UInt32 LOOPBAK_ADDRESS = 0x7F000000;
            const UInt32 LOOPBAK_NETMASK = 0xFF000000;

            // Convert IP to 4byte segment.
            UInt32 bytes = ((UInt32)(Address.ToValue()));

            // Compares bytes to local ip ranges using netmask anding.
            if ((bytes & CLASS_A_NETMASK) == CLASS_A_ADDRESS)
                return true;
            else if (((bytes & CLASS_B_NETMASK) >= CLASS_B_MINADDR) &&
                     ((bytes & CLASS_B_NETMASK) <= CLASS_B_MAXADDR))
                return true;
            else if ((bytes & CLASS_C_NETMASK) == CLASS_C_ADDRESS)
                return true;
            else if ((bytes & LOOPBAK_NETMASK) == LOOPBAK_ADDRESS)
                return true;
            else
                return false;
        }
    }

    // Class for holding packets before they are converted to connections (lightweight).
    public class L4Packet
    {
        // IP protcol (tcp / udp).
        public L4Protocol Protocol { get; set; }
        // Transmission direction.
        public TransmissionDirection Direction { get; set; }
        // Local IP address (sender).
        public IpV4Address Source { get; set; }
        // Protocol local port (sender port).
        public ushort SrcPort { get; set; }
        // Remote IP address (destination).
        public IpV4Address Destination { get; set; }
        // Protocol remote port (destination port).
        public ushort DstPort { get; set; }
        // Data size (in bytes) of protocol payload segments (change forces list view update).
        public uint PayloadSize { get; set; }
    }

    // Class to wrap protocol emum (for conversion to string / printing).
    public class ConnectionType : IComparable
    {
        public L4Protocol Protocol { get; set; }

        public ConnectionType(L4Protocol protocol) { this.Protocol = protocol; }

        public override string ToString()
        {
            if (Equals(this.Protocol, L4Protocol.TCP))
                return "TCP";
            else
                return "UDP";
        }

        public int CompareTo(object obj)
        {
            // Make sure we are comparing to a valid object.
            if ((obj == null) || !(obj is ConnectionType))
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
                return "->";
            else if (Equals(this.Direction, TransmissionDirection.REV_ONE_WAY))
                return "<-";
            else
                return "<>";
        }

        public int CompareTo(object obj)
        {
            // Make sure we are comparing against a valid object.
            if ((obj == null) || !(obj is ConnectionState))
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
        // Protocol local port (sender port).
        public ushort SrcPort { get; set; }
        // Protocol remote port (destination port).
        public ushort DstPort { get; set; }
        // Number of packets seen matching this connection (change forces list view update).
        private uint _packetCount;
        public uint PacketCount
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
        private uint _dataSent;
        public uint DataSent
        {
            get
            {
                return _dataSent;
            }
            set
            {
                if (value != _dataSent)
                {
                    _dataSent = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Timestamp for checking if this is an old / dead connection.
        public DateTime TimeStamp { get; set; }

        // Default ctor so we don't have to use the one that takes L4Packets.
        //public Connection() { this.PacketCount = 1; this.DataSent = 0; }

        // Ctor allows conversion from L4Packet to Connection.
        public Connection(L4Packet pkt)
        {
            this.Type = new ConnectionType(pkt.Protocol);
            this.State = new ConnectionState(pkt.Direction);
            this.Source = new ConnectionAddress(pkt.Source);
            this.Destination = new ConnectionAddress(pkt.Destination);
            this.SrcPort = pkt.SrcPort;
            this.DstPort = pkt.DstPort;
            this.PacketCount = 1;
            this.DataSent = pkt.PayloadSize;
            this.TimeStamp = DateTime.Now;
        }

        // Implement INotifyPropertyChanged (for auto bind list updating).
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        // Match packet to connection (-1 = no match, 0 = match, 1 = reverse match.
        public Match PacketMatch(L4Packet pkt)
        {
            // Check protocol first (biggest devider of connections).
            if (!Equals(this.Type.Protocol, pkt.Protocol))
                return Match.NO_MATCH;

            // Check if IP's and ports (or inverse) match.
            if ((Equals(this.Source.Address, pkt.Source) && Equals(this.SrcPort, pkt.SrcPort)) &&
                (Equals(this.Destination.Address, pkt.Destination) && Equals(this.DstPort, pkt.DstPort)))
                return Match.MATCH;
            else if ((Equals(this.Source.Address, pkt.Destination) && Equals(this.SrcPort, pkt.DstPort)) &&
                     (Equals(this.Destination.Address, pkt.Source) && Equals(this.DstPort, pkt.SrcPort)))
                return Match.REV_MATCH;

            return Match.NO_MATCH;
        }

        // Match pre-existing connections (Equals() is check value, == is check reference).
        public override bool Equals(Object obj)
        {
            // Check for null and compare run-time types.
            if ((obj != null) && (obj is Connection))
            {
                // Cast objest to usable current type.
                Connection conn = (obj as Connection);

                // Check protocol first (biggest devider of connections).
                if (!Equals(this.Type.Protocol, conn.Type.Protocol))
                    return false;

                // Check if IP's and ports (or inverse) match.
                if ((Equals(this.Source, conn.Source) && Equals(this.SrcPort, conn.SrcPort)) &&
                    (Equals(this.Destination, conn.Destination) && Equals(this.DstPort, conn.DstPort)))
                    return true;

                return false;
            }

            return false;
        }
        // Get hash code override (cause compiler warnings got to me!).
        public override int GetHashCode()
        {
            // Just xor address port combos to get a fairly collision resistent hash.
            uint result = (this.Source.Address.ToValue() + this.SrcPort);
            result ^= (this.Destination.Address.ToValue() + this.DstPort);

            return ((int)(result));
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

        // Exchange two items in list during sort.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Exchange(IList<Connection> connList, int first, int second)
        {
            Connection tmpConn = connList[first];
            connList[first] = connList[second];
            connList[second] = tmpConn;
        }

        // Determine if two items in list need to be exchanged during sort.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ExchangeTest(IList<Connection> connList, int first, int second, PropertyDescriptor prop, ListSortDirection direction)
        {
            if (Equals(direction, ListSortDirection.Ascending))
            {
                if (((IComparable)(prop.GetValue(connList[first]))).CompareTo((IComparable)(prop.GetValue(connList[second]))) > 0)
                    return true;
            }
            else
            {
                if (((IComparable)(prop.GetValue(connList[first]))).CompareTo((IComparable)(prop.GetValue(connList[second]))) < 0)
                    return true;
            }

            return false;
        }

        // Sort Functionallity.
        private bool _suppressSort = false;
        private bool _suppressNotification = false;
        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            // Set internal property and direction.
            _sortPropertyValue = prop;
            _sortDirectionValue = direction;

            // Don't allow sort to be called by OnListChanged() until the sort is over.
            _suppressSort = true;
            // Stop stack overflow and binded events.
            _suppressNotification = true;

            // Sort loop limiter.
            int sortLength = this.Count;

            // Optimized bubblesort (Skip tail sort).
            while (sortLength > 1)
            {
                // Initialize default value before each run of Exchange loop.
                int exchLocation = 0;

                // Exchange loop.
                for (int i = 1; i < sortLength; i++)
                {
                    if (ConnectionList.ExchangeTest(this.Items, (i - 1), i, _sortPropertyValue, _sortDirectionValue))
                    {
                        // Exchange elements.
                        ConnectionList.Exchange(this.Items, (i - 1), i);

                        // Set location of last Exchange.
                        exchLocation = i;
                    }
                }

                // Set the loop limit based on the position of the last element sorted.
                sortLength = exchLocation;
            }

            // Indicate that a sort has occured.
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
                _sortDirectionValue = default(ListSortDirection);
                _sortPropertyValue = default(PropertyDescriptor);
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
            // Don't call sort from end of sort function, but allow sorting of list in general on item change.
            if (_isSortedValue == true && !_suppressSort)
                ApplySortCore(_sortPropertyValue, _sortDirectionValue);

            // Do base operations so long as a sort is not currently in effect.
            if (!_suppressNotification)
                base.OnListChanged(args);
        }

        // Using this for deriving the next connection number partially resolves issues with using connection timeouts. (Compiler inline suggestion).
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint GetNewConnNumber(IList<Connection> connList)
        {
            uint LargestConnNumber = 0;

            foreach (Connection conn in connList)
            {
                if (conn.Number > LargestConnNumber)
                    LargestConnNumber = conn.Number;
            }

            return (LargestConnNumber + 1);
        }
    }
}
