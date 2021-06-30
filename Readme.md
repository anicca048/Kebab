# Kebab (https://github.com/anicca048/Kebab)
Kebab is a graphical "connection oriented" packet sniffer for Win32.
This project's goal is to provide a connection tracking utility for system
	administration and real time traffic analysis / debugging. While other tools
	use the kernel's connection tracking system, Kebab reads packets straight
	off the wire. This eliminates blind spots caused by the kernel's definition
	of an "existing" connection, and the lack of tracking of packets being
	routed to other destinations.
Written in C#, Kebab is published for free under the terms of the MIT
	open source license.
Kebab does not require any special permissions to run, so please do not run
	Kebab in admin-mode. If a vulnerability is found in the way that Kebab or
	Npcap parses packet information, or in the parsing of external data, such as
	update checking and or loading configuration file data, and you are
	running Kebab in admin-mode, it could lead to privilege escalation. There is
    currently no signature checking mechanism in place either, so a malicious
    program or user could also easily gain privilege escalation through
    modifying or replacing the main executable, or the DLL files used. In short,
    **DO NOT** run Kebab as admin, thanks.

# Npcap
WinPcap has been out of date for quite some time, and is lacking in
	functionality and security features.
Kebab uses Npcap (an alternative to WinPcap written by the awesome Nmap team).
	Npcap is up to date, and has many additional features and security
	benifits that WinPcap just doesn't.
You can find Npcap on the Nmap team's site here: https://nmap.org/npcap/
Npcap is published under several different licenses, including a free license
	that you can use to install on a few non commercial systems. You can read it
	here: https://github.com/nmap/npcap/blob/master/LICENSE
You can find a download link to the most recent version under the "Downloading
	and Installing Npcap Free Edition".
It is recommended that when you install Npcap, you check the "Restrict Npcap
    driver's access to Administrators only" option, so that access to the
	system level driver is restricted to privileged users only. Otherwise any
    tool can use the system driver to have unrestricted (and largely untracked)
	communications through your firewall. :(

# MaxMind GeoLite2 Database and API
This product includes GeoLite2 data created by MaxMind, available from
	https://www.maxmind.com
Kebab uses the MaxMind GeoLite2 City and ASN databases and accompanying C# API.
	The API is published in two parts, the MaxMind-DB-Reader-dotnet project,
	which parses MaxMind GeoLite2 databases, and GeoIP2-dotnet which handles
	both the use of the web API, and the static lookups through the
	MaxMind-DB-Reader-dotnet library.
Both parts of the API are published for free under the OpenSource Apache
	License, Version 2.0, while the database is published for free under custom
	licensing terms.
You can read more about their products and projects on the MaxMind site here:
	https://www.maxmind.com . You can also find the API source code and
	documentation on the various project Github repos:
	https://github.com/maxmind/MaxMind-DB-Reader-dotnet and 
	https://github.com/maxmind/GeoIP2-dotnet .
The API License is distributed with both the source and release of Kebab in the
	file "docs/MaxMind_API_license.txt" and the GeoLite2 copyright and
	attribution statement can be found in the file
	"docs/MaxMind_GeoLite2_copyright.txt".

# Instructions (RTFM edition)
Capture Tab - The controls on this tab are used to setup the packet capture.
	
	Capture Options Group - This group allows the user to select the libpcap
		interface to perform a packet capture on. It also provides controls to
		start and stop a capture, and a method to rescan the libpcap interface
		list, which could be useful in the event of a network card
		change / insertion.
	
	Capture Filter Group - This group allows the user to set the libpcap
		filter, which will be compiled and used to filter out packets from the
		capture session. There are several displayed filtering options, such as
		Destination and Source IP. And a "Filter String" box, where a libpcap
		filter string can be entered, for the more advanced users. Options from
		this filter group are much more efficient than any display filter,
		however, packets not matching the libpcap filter exactly will not show
		up in the Connections tab, skewing the results. Use these pre-capture
		filtering options carefully.

Connections Tab - Pretty basic, simply groups captured packets into connections.
	
	Display Filter Group - This group is used to filter and clear connections in
		the Connections Tab. The "Filter String" box uses a custom display
        filter syntax, that is very simular to libpcap capture filter syntax.
        It allows "tcp" and "udp" keywords, and "host" and "port" keywords
        (with "src" and "dst" modifyers, and allows ranges using '-' delimiter).
        The display filter string only uses "and" logic, so connetions must
        match all keywords, modifyers, and values expressed. An example would
        be: <tcp src host 10.0.0.0-10.255.255.255 dst port 22-23>
        The timeout checkbox will remove connections that have had
		no activity based on the user set timeout in seconds (default is 10).
		The clear connections button, you guessed it, clears all the connections
		from the list. Use these destructive options with care.
	
	Connections Data Grid View - This is used to display the simplified
		components of the connection in a list like fashion. All connections
		are made up of several core components, and a few extra components
		which are subject to change. The core components are as follows:
			
			Number (#), this is the number of the connection in order of first
			observed packet.
			
			Protocol (Type), this is the L4 protocol (TCP or UDP in our case) of
			the connection.
			
			Local Address, this is the assumed local address (if there is one)
			in the connection, or just the source address of the first observed
			packet if not.
			
			Local Port (Port), the matching port for the Local Address.
			
			Transmission Direction (RXTX), the observed one or two way state of
			the communication between the endpoints.
			
			Remote Address, the assumed remote address (if there is one) in the
			connection, or just the destination address of the first observed
			packet if not.
			
			Remote Port (Port), the matching port for the Remote Address.
			
			Number of Packets (Packets Sent), the observed total number of
            packets sent between hosts.
			
			Payload Data Transmitted (Bytes Sent), the total size of observed
			packet protocol payload data, in bytes, sent between hosts.
			
			GeoIP info (ISO), this is the country and State / Region
			ISO code for the RemoteAddress field. If the geo data is not found
			for a given IP than there will be two dashes "--" instead.
			
			ASN Organization name, this is the organization that was registered
			as owning the given IP block that the RemoteAddress is part of.

# Known Issues / Bugs / Errata
Connection Sorting: Currently if you sort the connection list in any way
(except by property "Number" in ascending order), under heavy load the UI loop
can get bogged down by the constant resorting of the list. This may cause
delayed responses to user input and manipulation of the window / controls.
Optimizations are in place to help combat this issue. Further optimizations,
and mitigations, are being considered to better deal with the issue.

GeoIP data: sometimes geoip data will be out of date, or missing entirely for a
given IP address. Currently the dbases provided is a recent release of
MaxMind GeoLite2, as this doesn't require any accounts or API keys. Eventually
an option to enter a personal MaxMind account web API key, for more accurate and
up-to-date results, will be added (should happen in the near future).
