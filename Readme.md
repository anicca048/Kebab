# Kebab
Kebab is a "connection oriented" graphical packet sniffer for MS Windows.
It simplifies packets captured on an interface into connections for real time
	traffic analysis and debugging.
Written in C#, Kebab is published for free under the terms of the MIT
	opensource license.
Kebab does not require any special permissions to run, so please do not run
	Kebab in admin-mode. If a vulnerabillity is found in the way that Kebab or
	PcapDotNet parses packet information and you are running Kebab in admin-mode
	it could lead to privledge escalation.

# PcapDotNet
Kebab uses PcapDotNet, a wrapper for WinPcap (Outdated windows port of libpcap).
PcapDotNet is opensource, and you can find both the source code and the releases
	on github here: https://github.com/PcapDotNet/Pcap.Net
PcapDotNet is published under a custom license which can be found
	here: https://github.com/PcapDotNet/Pcap.Net/blob/master/license.txt
The PcapDotNet license is distributed with both the source and release of
	Kebab in the file PcapDotNet_license.txt.

# Npcap
As mentioned earlier, while PcapDotNet is intended for use with WinPcap, WinPcap
	has been out of date for quite some time, and is lacking in functionality
	and security features.
It is recommended that you install Npcap instead (alternative to WinPcap written
	by the awesome Nmap team) Npcap is up to date, and has many additional
	features and security benifits that WinPcap just doesn't.
You can find Npcap on the Nmap team's site here: https://nmap.org/npcap/
Npcap is published under several different licenses, including a free license
	that you can use to install on a few non comercial systems. You can read it
	here: https://github.com/nmap/npcap/blob/master/LICENSE
You can find a download link to the most recent version under the "Downloading
	and Installing Npcap Free Edition".
It is recommended that if you install Npcap, you check the "Install Npcap in
	WinPcap API-compatible Mode" and "Restrict Npcap driver's access to
	Administrators only" options. But you will need the WinPcap compatability
	option if you want Kebab to work with Npcap.

# Instructions (RTFM edition)
Capture Tab - The controls on this tab are used to setup the packet capture.
	Capture Options Group - This group allows the user to select the libpcap
		interface to perform a packet capture on. It also provides controls to
		start and stop a capture, and a method to rescan the libpcap interface
		list, which could be usefull in the event of a network card
		change / insersion.
	Capture Filter Group - This group allows the user to set the libpcap
		filter, which will be compiled and used to filter out packets from the
		capture session. This filter is more efficeint than a display filter,
		however, packets not matching the libpcap filter exactly will not show
		up in the Connections tab, skewing the results. Use these pre-capture
		filtering options conservativley.
Connections Tab - Prety basic, simply groups captured packets into connections.
	Display Filter Group - This group is used to filter and clear connetions in
		the Connections Tab. Use the IP and Port filters to make matching
		connections easier to spot, without removing non matching connections.
		The display filter will match any connection with at least one host who
		matches all filters used. If you need more precise filtering, set the 
		pre-capture libpcap filter using the Capture Filter Group, on the
		Capture Tab. The timeout checkbox will remove connections that have had
		no activity in the last 10 seconds. The clear connections button, you
		guessed it, clears all the connections from the list. Use these
		destructive options with care.
	Connections Data Grid View - This is used to display the simplified
		components of the connection in a list like fashion. All connections
		are made up of several core components, and a few extra components
		which are subject to change. The core components are as follows:
		
		Number (#), this is the number of the connection in order of first
		observed packet.
		
		Protocol (Type), this is the level 4 protcol (TCP or UDP in our case)
		of the connection.
		
		Local Address, this is the assumed local address (if there is one) in
		the connection, or just the source address of the first observed packet.
		
		Local Port (Port), the matching port for the Local Address.
		
		Transmission Direction (State), the observed one or two way state of the
		connection.
		
		Remote Address, the assumed remote address (if there is one) in the
		connection, or just the destination address of the first observed
		packet.
		
		Remote Port (Port), the matching port for the Remote Address.
		
		Number of Packets (Packets), the observed total number of packets sent
		between local and remote host.
		
		Total Data Transmitted (Data Size), the total size of observed packet
		protocol payload data, in bytes.
		
# Known Issues / Bugs / Errata
Connection numbering: Currently if you use the connection timeout option,
deleted entries may cause gaps in the connection numbering. The current fix
prevents this, but can still lead to large numbering of a small list in the
right conditions. Once the list reaches 0 however, the numbering will be
restored to normalcy. Users will just have to deal with this until a proper
non-destructive connection number reodering algorithim is implemented.
(it's gonna be a while.)

Connection Sorting: Currently if you sort the connection list (by any property)
under heavy load it will bogg down the UI loop causing the delsyed responses
to user input and manipulation of the window / controls. The sort algorithim
has been optimized to fight this, but nothing more can be done untill a new
sort algorithim is implemnted. (should happen soon.)

