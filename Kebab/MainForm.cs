
using System;
using System.Linq;
using System.Reflection;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Principal;

using ShimDotNet;

using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Responses;
using MaxMind.Db;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Kebab
{
    public partial class MainForm : Form
    {
        // Breif description of the program.
        private const string aboutPage = Program.Name + " version " + Program.Version
                                         + "\n"
                                         + "Copyright © 2018 anicca048\n"
                                         + "\n"
                                         + "Written in C#, " + Program.Name + " is published for free under the terms of the MIT opensource license.\n"
                                         + "\n"
                                         + Program.Name + " uses Npcap, and the MaxMind GeoLite2 City and ASN databases and accompanying C# API.\n"
                                         + "\n"
                                         + "All third party API's / libraries / dll's used by " + Program.Name + " are opensource.\n"
                                         + "\n"
                                         + "Program and legal documentation are included in the *Readme* and *License* / *Copyright* files respectively.\n"
                                         + "\n"
                                         + "Further documentation and source code can be found on the project's Github repo.\n"
                                         + "\n"
                                         + Program.Name + " Github repo: https://github.com/anicca048/Kebab\n"
                                         + "Npcap Github repo: https://github.com/nmap/npcap\n"
                                         + "MaxMind C# API Github repo: https://github.com/maxmind/MaxMind-DB-Reader-dotnet\n"
                                         + "and https://github.com/maxmind/GeoIP2-dotnet";

        // Header to match connections for saving list.
        private const string connListHdr = "#    Type    LocalAddress:Port  RXTX    RemoteAddress:Port  PacketsSent  DataSent(B)     ISO    ASNOrg\n";

        // Configuration object.
        private Config programConfig;

        // Interface drop down list data source.
        private BindingList<string> deviceList;

        // Grid view data source (connection list).
        private BindingSource connectionSource;
        private ConnectionList connectionList;

        // Connection packet queue and lock for background workers (so as not to overload the UI loop.)
        private Queue<IPV4_PACKET> pendingPackets = new Queue<IPV4_PACKET>();
        private readonly object pendingPacketsLock = new object();

        // Packet capture engine (main class from ShimDotNet).
        private CaptureEngine captureEngine;

        // Background worker for processing packets.
        private BackgroundWorker _captureWorker;

        // Timers for updating the connection stats.
        System.Windows.Forms.Timer packetTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer timeoutTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer displayFilterTimer = new System.Windows.Forms.Timer();

        // Minimal wait time between packet batch processing in milliseconds.
        private const int packetBatchInterval = 10;
        // Batch packet number limit.
        private const int packetBatchSize = 500;
        // Minimal wait time between inactive entry removal operations in milliseconds.
        private const int timeoutInterval = 500;
        // Inactive entry time limit in seconds.
        private int timeoutInactivityLimit = 30;
        // How often to apply display filter checks in milliseconds.
        private const int displayFilterInterval = 500;

        // File stream for using save option without reprompting dialog.
        private string saveFileName;
        // Filter string to use for file dialog window.
        private const string saveFileFilter = "text file (*.txt)|*.txt|All files (*.*)|*.*";

        // Used to evaluate textfields that should only contian numeric values such as ports.
        private const string nonNumericRegex = @"[^0-9]";
        // Used to evaluate textfields that contain numeric vaules and periods such as ip addrs.
        private const string nonNumericDotRegex = @"[^0-9\.]";

        // GeoLite2 dbase filenames.
        private const string IPCityLookupDB = "db/GeoLite2-City.mmdb";
        private const string IPASNLookupDB = "db/GeoLite2-ASN.mmdb";
        // MM GeoLite2 dbase mapper / reader object.
        private DatabaseReader CityReader;
        private DatabaseReader ASNReader;

        // Indicates that the form closing event has been called and that the worker completion method needs to close the form.
        private bool formClosePending = false;

        // 
        // Main form directly related methods.
        // 

        // Entry function for the form.
        public MainForm()
        {
            // Combat form flickering and resize drawing glitching.
            this.DoubleBuffered = true;

            // Sets up form components / gui elements.
            InitializeComponent();

            // Add timer event handlers.
            packetTimer.Tick += new EventHandler(UpdateConnList);
            timeoutTimer.Tick += new EventHandler(TimeoutConnList);
            displayFilterTimer.Tick += new EventHandler(FilterConnList);

            // Set timer intervals.
            packetTimer.Interval = packetBatchInterval;
            timeoutTimer.Interval = timeoutInterval;
            displayFilterTimer.Interval = displayFilterInterval;
        }

        // User defined init (runs after MainForm constructor).
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Double buffered property is not transfering to the DGV, so we'll force it through reflection.
            typeof(Control).InvokeMember("DoubleBuffered",
                                         BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                                         null, ConnectionGridView, new object[] { true });

            // Spawn mainform on top (but don't force always on top).
            this.TopMost = true;
            this.TopMost = false;

            // Check if program is running as admin, and inform user of danger if so.
            if ((new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator))
            {
                MessageBox.Show("Warning: it is not recommended to run " + Program.Name + " as Administrator!"
                                + "\nRunning this program with Administrator privileges could put your computer at risk!", Program.Name);
            }

            // Init cengine.
            try
            {
                captureEngine = new CaptureEngine();
            }
            // Incase npcap is not installed (thrown if ShimDotNet can't load .../Npcap/wpcap.dll).
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(("Error: failed to initilize capture engine:\n" + ex.Message
                                 + "\n\nMake sure that Npcap is installed properly."), Program.Name);
                System.Environment.Exit(1);
            }

            // Get device list (serves as basic pcap init / test).
            if (captureEngine.genDeviceList() == -1)
            {
                MessageBox.Show(("Error: failed to generate device list:\n" + captureEngine.getEngineError()), Program.Name);
                System.Environment.Exit(1);
            }

            // Create new device / interface list.
            deviceList = new BindingList<string>();

            // Generate displayed device list.
            BuildDeviceList();

            // BInd device list to drop down menu.
            InterfaceDropDownList.DataSource = deviceList;

            // Create GeoIP reader (maps dbase to memory and allows quick lookups).
            try
            {
                CityReader = new DatabaseReader(IPCityLookupDB);
                ASNReader = new DatabaseReader(IPASNLookupDB);
            }
            catch (FileNotFoundException ex)
            {
                // Warn user of missing database file and exit.
                MessageBox.Show(("Error: could not find a database file: " + ex.Message + "\n\nDownload a fresh copy of "
                                 + Program.Name + " to ensure that you have all the required files."), Program.Name);
                System.Environment.Exit(1);
            }
            catch (InvalidDatabaseException ex)
            {
                // Warn user of missing database file and exit.
                MessageBox.Show(("Error: invalid or corrupt database file: " + ex.Message + "\n\nDownload a fresh copy of "
                                 + Program.Name + " to ensure that you have all the required files."), Program.Name);
                System.Environment.Exit(1);
            }

            // Create new program configuration state variable.
            programConfig = new Config(Program.ConfigFileName);

            // Attempt to load configuration from file, or use default config.
            if (!programConfig.LoadConfig())
            {
                MessageBox.Show(("Error: failed to load configuration file!\nThe file may be invalid or corrupt!"
                                 + "\n\n" + Program.Name + " will now generate a new config file."
                                 + "\nAny preexisting config file will be overwritten."), Program.Name);
            }

            // Set default and or loaded config.
            ApplyConfig();

            // Attempt to overwrite config file with default and or loaded values.
            if (!programConfig.SaveConfig())
            {
                MessageBox.Show(("Error: failed to save configuration file!" + "\n\n" + Program.Name +
                                 " will run in non-persistent config mode, user preferences will not be saved!"),
                                Program.Name);

                //nonPersistentConfig = true;
            }

            // Check if auto update functionality is enabled.
            if (programConfig.Vars.update_check == "true")
            {
                // Retrieve current version.
                string tagName = GetCurrentTagName();

                // Application is not the latest version.
                if (!Program.GithubAPI_ReleaseTagElementValue.Equals(tagName))
                {
                    // Inform user of available update, and ask if they would like to visit web page.
                    if (MessageBox.Show("An update is available!\n\nCurrent version: \t" + Program.GithubAPI_ReleaseTagElementValue + "\nLatest version: \t"
                                        + tagName + "\n\nWould you like to vist the latest release web page?",
                                        Program.Name, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        // Load URL in default web browser.
                        System.Diagnostics.Process.Start(Program.GithubWEB_LatestReleaseURL);
                    }
                }
            }

            // Enable form after init is done.
            this.Enabled = true;
        }

        // Fetch update information.
        private string GetCurrentTagName()
        {
            // Create HTTP WEB request using API URL string.
            HttpWebRequest APIRequest = (HttpWebRequest)WebRequest.Create(Program.GithubAPI_LatestReleaseURL);
            // Github API requires a user agent to be set.
            APIRequest.UserAgent = Program.GithubAPI_HTTPUserAgent;
            // Create storage location for response data.
            HttpWebResponse APIresponse;

            // Send GET and attempt to store response.
            try
            {
                APIresponse = (HttpWebResponse)APIRequest.GetResponse();
            }
            // Catch any failures such as 400 series web responses.
            catch (System.Net.WebException ex)
            {
                MessageBox.Show(("Error: failed to check for update:\n" + ex.Message
                                 + "\n\nMake sure that you have a valid internet connection."), Program.Name);
                return "";
            }

            // Get payload data from response.
            Stream responseDataStream = APIresponse.GetResponseStream();
            StreamReader responseReader = new StreamReader(responseDataStream);
            // Save data as string.
            string JSONString = responseReader.ReadToEnd();

            // Cleanup response data parsing opjects.
            responseReader.Close();
            responseDataStream.Close();
            APIresponse.Close();

            // Create json object to parse json data.
            JObject JSONObj;

            // Attempt to parse json data.
            try
            {
                JSONObj = JObject.Parse(JSONString);
            }
            catch (JsonReaderException)
            {
                MessageBox.Show("Error: failed to check for update:\nAPI data is invalid or corrupt.", Program.Name);
                return "";
            }

            // tag_name value string.
            string tagName;

            // Attempt to get tag_name element for comparison.
            try
            {
                tagName = JSONObj[Program.GithubAPI_ReleaseTagElementName].Value<string>();
            }
            catch (System.ArgumentNullException)
            {
                MessageBox.Show("Error: failed to check for update:\nAPI data is invalid.", Program.Name);
                return "";
            }

            return tagName;
        }

        // Apply loaded configuration settings.
        private void ApplyConfig()
        {
            // Apply banner message string to mainform title.
            this.Text += (" - " + programConfig.Vars.banner_message);
        }

        // Reusable grouping for cleanup tasks when form needs to close.
        private void MainFormCleanup()
        {
            // Stop timers.
            packetTimer.Stop();
            timeoutTimer.Stop();
            displayFilterTimer.Stop();

            // Dispose geolite constructs.
            CityReader.Dispose();
            ASNReader.Dispose();
        }

        // Overide form close to do background worker cleanup.
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Set this before testing if workers are busy to double ensure that a timing gab is impossible.
            formClosePending = true;

            // If background worker is running then force cleanup and let it handle form close.
            if (_captureWorker != default(BackgroundWorker) && _captureWorker.IsBusy)
            {
                _captureWorker.CancelAsync();
                e.Cancel = true;
                this.Enabled = false;
                return;
            }

            // Cleanup.
            MainFormCleanup();

            // Otherwise let the form close naturally.
            base.OnFormClosing(e);
        }

        private void BuildDeviceList()
        {
            // Generate inteface list with capture engine and check for issues.
            if (captureEngine.genDeviceList() == -1)
            {
                // Inform user of critical error.
                MessageBox.Show(("Error: failed to generate device list:\n" + captureEngine.getEngineError()), Program.Name);
                System.Environment.Exit(1);
            }

            // Generate .Net interface list for fetching MS "friendly names" for interfaces.
            List<NetworkInterface> interfaces = new List<NetworkInterface>(NetworkInterface.GetAllNetworkInterfaces());

            // Clear interface list.
            deviceList.Clear();

            // Add an invalid entry ontop to force user to select an interface instead of just going with the first one.
            deviceList.Add("Select Interface");

            // Get device count.
            int listSize = captureEngine.getDeviceCount();

            // Add device descriptions to list.
            for (int i = 0; i < listSize; i++)
            {
                // Name lookup flag.
                bool friendlyNameFound = false;

                // Try to match pcap interface uuid to .net interface uuid.
                foreach (NetworkInterface iface in interfaces)
                {
                    // Storage var for interface uuid.
                    string iface_uuid;

                    // Pull uuid by string splitting (\Device\NPF_{XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX}).
                    try
                    {
                        iface_uuid = Regex.Split(captureEngine.getDeviceName(i), @"\\Device\\NPF_")[1];
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                        continue;
                    }

                    // Compare uuid to iface in list.
                    if (iface_uuid.Equals(iface.Id))
                    {
                        // Get interface name.
                        string ifaceName = iface.Name;

                        // Check if interface has ipv4 address, and if so add it to name string.
                        foreach (UnicastIPAddressInformation ip in iface.GetIPProperties().UnicastAddresses)
                        {
                            // If we have a valid IPv4 address, add it.
                            if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork
                                && ip.Address.ToString() != "0.0.0.0")
                            {
                                ifaceName += (" [ " + ip.Address.ToString() + " ]");
                                break;
                            }
                        }

                        // If found add friendly name to list and trip flag.
                        deviceList.Add(ifaceName);
                        friendlyNameFound = true;
                        break;
                    }
                }

                // Add best name for interface.
                if (friendlyNameFound)
                    continue;
                else if (captureEngine.getDeviceDescription(i).Length > 0)
                    deviceList.Add(captureEngine.getDeviceDescription(i));
                else
                    deviceList.Add(captureEngine.getDeviceName(i));
            }
        }

        // 
        // Background worker related methods
        // 

        // Last background worker does form cleanup.
        private void DoBWCleanup(object sender, RunWorkerCompletedEventArgs e)
        {
            // Do ShimDotNet cleanup.
            captureEngine.stopCapture();

            // Check if user is waiting for form to close.
            if (formClosePending)
            {
                // Cleanup.
                MainFormCleanup();

                // Close form.
                this.Close();
            }

            // Re enable start button now that a new capture can be started.
            ChangeOnStopCapture(sender, e);
        }

        // Setup all background workers.
        private void DoBWSetup()
        {
            // Check if backgroundworker exists and Dispose it.
            if (_captureWorker != default(BackgroundWorker))
                _captureWorker.Dispose();

            // Initialize new background worker object.
            _captureWorker = new BackgroundWorker();

            // Start packet capture background worker.
            _captureWorker.WorkerSupportsCancellation = true;
            _captureWorker.DoWork += CaptureWorker_DoWork;
            _captureWorker.RunWorkerCompleted += DoBWCleanup;
            _captureWorker.RunWorkerAsync();
        }

        // Threaded pcap packet processing loop.
        private void CaptureWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Build a reference worker so we can check cancellationPending() from ProcessPackets().
            BackgroundWorker thisWorker = sender as BackgroundWorker;

            // Start packet capture loop.
            while (!thisWorker.CancellationPending)
            {
                IPV4_PACKET pkt = new IPV4_PACKET();

                // Process a packet / connection, and add it to the queue.
                if (captureEngine.getNextPacket(ref pkt) != -1)
                {
                    // Obtain lock on queue and then add connection / packet.
                    lock (pendingPacketsLock)
                    {
                        pendingPackets.Enqueue(pkt);
                    }
                }
            }
        }

        // 
        // Display filter related methods.
        // 

        // Checks if display filters match a connection.
        private bool CheckDisplayFilter(object DataSource)
        {
            // Convert dataGridViewRow data source to actual type.
            Connection connection = (Connection)DataSource;

            // Ceck if we have a display filter to use.
            bool IPFilter = (IPDisplayFilter.Text.Trim().Length > 0);
            bool PortFilter = (PortDisplayFilter.Text.Trim().Length > 0);

            if (IPFilter && PortFilter)
            {
                if ((connection.Source.ToString() == IPDisplayFilter.Text.Trim()
                     || connection.Destination.ToString() == IPDisplayFilter.Text.Trim())
                    && (connection.SrcPort.ToString() == PortDisplayFilter.Text.Trim()
                     || connection.DstPort.ToString() == PortDisplayFilter.Text.Trim()))
                    return true;
                else
                    return false;
            }
            else if (IPFilter)
            {
                if (connection.Source.ToString() == IPDisplayFilter.Text.Trim()
                    || connection.Destination.ToString() == IPDisplayFilter.Text.Trim())
                    return true;
                else
                    return false;
            }
            else if (PortFilter)
            {
                if (connection.SrcPort.ToString() == PortDisplayFilter.Text.Trim()
                    || connection.DstPort.ToString() == PortDisplayFilter.Text.Trim())
                    return true;
                else
                    return false;
            }
            else
            {
                return true;
            }
        }

        // Make sure we don't paint rows that don't match the display filter.
        private bool _displayFilterSet = false;
        private void ApplyDisplayFilter()
        {
            // Loop through connectionList and hide any connections that don't match filter (and unhide ones that do).
            foreach (DataGridViewRow row in ConnectionGridView.Rows)
            {
                if (!CheckDisplayFilter(row.DataBoundItem))
                {
                    row.DefaultCellStyle.ForeColor = SystemColors.GrayText;
                    row.DefaultCellStyle.Font = ConnectionGridView.DefaultCellStyle.Font;
                }
                else
                {
                    row.DefaultCellStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#04cc84");
                    row.DefaultCellStyle.Font = new Font(ConnectionGridView.DefaultCellStyle.Font.Name,
                                                         ConnectionGridView.DefaultCellStyle.Font.Size,
                                                         FontStyle.Bold);
                }
            }

            // Mark displayfilter flag true.
            if (!_displayFilterSet)
                _displayFilterSet = true;

            // Show displayfilter changes.
            ConnectionGridView.Update();
        }

        // Clears text from display filter and removes filter from connections list.
        private void ClearDisplayFilter()
        {
            // Clear display filter text.
            IPDisplayFilter.Clear();
            PortDisplayFilter.Clear();
        }

        // Removes filter from connection list.
        private void RemoveDisplayFilter()
        {
            // Remove displayfilter font changes.
            foreach (DataGridViewRow row in ConnectionGridView.Rows)
            {
                row.DefaultCellStyle.ForeColor = ConnectionGridView.DefaultCellStyle.ForeColor;
                row.DefaultCellStyle.Font = ConnectionGridView.DefaultCellStyle.Font;
            }

            // Mark displayfilter flag false.
            if (_displayFilterSet)
                _displayFilterSet = false;

            // Show displayfilter changes.
            ConnectionGridView.Update();
        }

        // Force repaint for display filter adhearence.
        private void IPDisplayFilter_TextChanged(object sender, EventArgs e)
        {
            if (!(IPDisplayFilter.Text.Trim().Length > 0 || PortDisplayFilter.Text.Trim().Length > 0))
            {
                if (_displayFilterSet)
                    RemoveDisplayFilter();

                displayFilterTimer.Stop();

                return;
            }

            ApplyDisplayFilter();
            displayFilterTimer.Start();
        }

        // Force repaint for display filter adhearence.
        private void PortDisplayFilter_TextChanged(object sender, EventArgs e)
        {
            if (!(PortDisplayFilter.Text.Trim().Length > 0 || IPDisplayFilter.Text.Trim().Length > 0))
            {
                if (_displayFilterSet)
                    RemoveDisplayFilter();

                displayFilterTimer.Stop();

                return;
            }

            ApplyDisplayFilter();
            displayFilterTimer.Start();
        }

        // Force repaint for display filter adhearence when rows are added.
        private void ConnectionGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (_displayFilterSet)
            {
                ApplyDisplayFilter();
            }
        }
        // Force repaint for display filter adhearence when the list is sorted by a property.
        private void ConnectionGridView_Sorted(object sender, EventArgs e)
        {
            if (_displayFilterSet)
            {
                ApplyDisplayFilter();
            }
        }

        // 
        // ConnectionGridView modifiers.
        // 

        // Update connection stats with packets remaining in the queue.
        private void UpdateConnList(object sender, EventArgs e)
        {
            // We will only process the number of connections in the queue when this function is invoked.
            int packetsToProcess = 0;

            // Obtain lock on pending connections queue so that we can size the queue.
            lock (pendingPacketsLock)
            {
                packetsToProcess = pendingPackets.Count;
            }

            // Need to limit how much we process per run or we will bottleneck the UI loop.
            if (packetsToProcess > packetBatchSize)
                packetsToProcess = packetBatchSize;

            while (packetsToProcess > 0)
            {
                // Create new connection object.
                IPV4_PACKET pkt;

                // Grab first connection in the queue for processing.
                lock (pendingPacketsLock)
                {
                    pkt = pendingPackets.Dequeue();
                }

                // Set direction for packet.
                TransmissionDirection direction = TransmissionDirection.ONE_WAY;

                // Make sure we don't add a new connection when a matching one already exists and was updated.
                bool packetMatched = false;

                // Loop through connections list and see if we have a new connection.
                foreach (Connection tmpConn in connectionList)
                {
                    // Find matching connections(regardless of source and dest orientation).
                    if (tmpConn.PacketMatch(pkt) != MatchType.NO_MATCH)
                    {
                        tmpConn.PacketCount++;
                        tmpConn.ByteCount += pkt.payload_size;
                        tmpConn.TimeStamp = DateTime.Now;

                        // Check if direction needs to be updated to both ways.
                        if (tmpConn.State.Direction != TransmissionDirection.TWO_WAY)
                        {
                            if (tmpConn.PacketMatch(pkt) == MatchType.REVERSE_MATCH
                                && tmpConn.State.Direction == direction)
                                tmpConn.State.Direction = TransmissionDirection.TWO_WAY;
                            // Deals with case where both endpoints are local so no setting of REV_ONE_WAY occurs.
                            else if (tmpConn.State.Direction != direction)
                                tmpConn.State.Direction = TransmissionDirection.TWO_WAY;
                        }

                        // Match made.
                        packetMatched = true;

                        // One less to process.
                        packetsToProcess--;

                        break;
                    }
                }

                // If a match was not found, create a new connection and add it to the list.
                if (!packetMatched)
                {
                    // Create new connection.
                    Connection newConn = new Connection(pkt, direction);

                    // Don't create local connections if not wanted by user.
                    if (newConn.Source.AddressIsLocal() && newConn.Destination.AddressIsLocal() && RemoveLocalConnectionsCheckBox.Checked)
                    {
                        // Mark packet as processed.
                        packetsToProcess--;
                        // Move on to next packet.
                        continue;
                    }
                    // Need to do a check to make sure that local and loobpack connections get priority for the Source address.
                    else if (newConn.Destination.AddressIsLocal() && !newConn.Source.AddressIsLocal())
                    {
                        // Swap IP addrs.
                        IPAddress tmpIP = newConn.Destination.Address;
                        newConn.Destination.Address = newConn.Source.Address;
                        newConn.Source.Address = tmpIP;

                        // Swap ports.
                        UInt16 tmpUint16 = newConn.DstPort;
                        newConn.DstPort = newConn.SrcPort;
                        newConn.SrcPort = tmpUint16;

                        // Flip direction.
                        newConn.State.Direction = TransmissionDirection.REV_ONE_WAY;
                    }

                    // Add geographical info.
                    if (CityReader.TryCity(newConn.Destination.ToString(), out CityResponse cityResp))
                    {
                        // Add info from response object, or "--" marker for empty components of the response.
                        if ((newConn.DstGeo.Country = cityResp.Country.Name) == default(string))
                            newConn.DstGeo.Country = "--";

                        if ((newConn.DstGeo.CountryISO = cityResp.Country.IsoCode) == default(string))
                            newConn.DstGeo.CountryISO = "--";

                        if ((newConn.DstGeo.State = cityResp.MostSpecificSubdivision.Name) == default(string))
                            newConn.DstGeo.State = "--";

                        if ((newConn.DstGeo.StateISO = cityResp.MostSpecificSubdivision.IsoCode) == default(string))
                            newConn.DstGeo.StateISO = "--";

                        if ((newConn.DstGeo.City = cityResp.City.Name) == default(string))
                            newConn.DstGeo.City = "--";
                    }
                    // GeoLookup failed.
                    else
                    {
                        // If city dbase lookup failed, set vals to empty marker.
                        newConn.DstGeo.Country = "--";
                        newConn.DstGeo.CountryISO = "--";
                        newConn.DstGeo.State = "--";
                        newConn.DstGeo.StateISO = "--";
                        newConn.DstGeo.City = "--";
                    }

                    // Add ASN info.
                    if (ASNReader.TryAsn(newConn.Destination.ToString(), out AsnResponse asnResp))
                    {
                        newConn.DstASN = asnResp.AutonomousSystemNumber;

                        if ((newConn.DstASNOrg = asnResp.AutonomousSystemOrganization) == default(string))
                            newConn.DstASNOrg = "--";
                    }
                    else
                    {
                        newConn.DstASN = null;
                        newConn.DstASNOrg = "--";
                    }

                    // Asing the connection a number.
                    newConn.Number = ((uint)connectionList.Count + 1);
                    // And add it to the list.
                    connectionList.Add(newConn);

                    // Finaly, we have one less to process.
                    packetsToProcess--;
                }
            }
        }

        // Reorders connection numbers to remove gaps in the order and avoid large number to small conn count ratios.
        private void ReorderOnRemoval()
        {
            // Create list of connection numbers.
            List<uint> connNums = new List<uint>();

            // Add all existing connection numbers to list.
            foreach (Connection conn in connectionList)
                connNums.Add(conn.Number);

            // Sort list of connection numbers in ascending order.
            connNums.Sort();

            // Apply reordering.
            for (int i = 0; i < connNums.Count; i++)
            {
                // Match current number to connection.
                foreach (Connection conn in connectionList)
                {
                    // Change number of matching connection to new value.
                    if (conn.Number.Equals(connNums[i]))
                    {
                        conn.Number = (uint)(i + 1);
                        break;
                    }
                }
            }
        }

        // Runs connection timeout removal operations on Connection List.
        private void TimeoutConnList(object sender, EventArgs e)
        {
            // Flag to check if reordering needs to occur.
            bool reorderConnections = false;

            // Loop through connections list and see if we have any inactive connections to remove.
            while (true)
            {
                // Flag to see if we removed any connections.
                bool removedConnection = false;

                // Loop through connectionList and remove an inactive entry if we find one.
                foreach (Connection loopConn in connectionList)
                {
                    // Find Inactive connetion based on timestamp.
                    if ((DateTime.Now - loopConn.TimeStamp).TotalSeconds >= timeoutInactivityLimit)
                    {
                        // Remove inactive entry and mark flag so that we will do another inactive connection check.
                        connectionList.Remove(loopConn);
                        removedConnection = true;

                        if (!reorderConnections)
                            reorderConnections = true;

                        // Must break beause loop limit was changed.
                        break;
                    }
                }

                // Stop checking for inactive connections if all have been checked.
                if (!removedConnection)
                {
                    // Handle connection list modifiction necesities.
                    if (reorderConnections)
                    {
                        // Cleanup connection numbers in the wake of created gaps.
                        ReorderOnRemoval();

                        // Refresh DataGridView values to avoid errors with pending UI events on removed connections.
                        ConnectionGridView.Update();
                    }

                    break;
                }
            }
        }

        // Apply displayfilter if enabled.
        private void FilterConnList(object sender, EventArgs e)
        {
            if (_displayFilterSet)
            {
                ApplyDisplayFilter();
            }
        }

        // Allows thread safe clearing of binded data list from background worker.
        private void ClearConnList()
        {
            // Clear the queue first.
            lock (pendingPacketsLock)
            {
                pendingPackets.Clear();
            }

            // Clear the binded list of entries.
            ConnectionGridView.ClearSelection();
            connectionList.Clear();
            
            // Make control redraw.
            ConnectionGridView.Update();
        }

        //
        // Copy and save functionality members.
        //

        // Return selected row list (fixes reverse order bug).
        private List<DataGridViewRow> getSelectedRows()
        {
            List<DataGridViewRow> rows =
            (from DataGridViewRow row in ConnectionGridView.SelectedRows
             where !row.IsNewRow
             orderby row.Index
             select row).ToList<DataGridViewRow>();

            return rows;
        }

        // Turns specified rows from connListView into formated conn lines as a string for copy and save functions.
        private string getConnsFromRows(List<DataGridViewRow> rows)
        {
            string outputStr = "";

            foreach (DataGridViewRow row in rows)
            {
                outputStr += (row.Cells[0].Value.ToString().PadRight(4) + " "
                              + row.Cells[1].Value.ToString().PadRight(4) + " "
                              + row.Cells[2].Value.ToString().PadLeft(15) + ":" + row.Cells[3].Value.ToString().PadRight(5) + " "
                              + row.Cells[4].Value.ToString().PadRight(5) + " "
                              + row.Cells[5].Value.ToString().PadLeft(15) + ":" + row.Cells[6].Value.ToString().PadRight(5) + " "
                              + row.Cells[7].Value.ToString().PadRight(12) + " "
                              + row.Cells[8].Value.ToString().PadRight(15) + " "
                              + row.Cells[9].Value.ToString().PadRight(6) + " "
                              + row.Cells[10].Value.ToString()
                              + "\n");
            }

            return outputStr;
        }

        // Copy all selected rows with right click.
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure there is something to copy.
            if (connectionList == null || connectionList.Count == 0
                || ConnectionGridView == null || ConnectionGridView.SelectedRows.Count == 0)
                return;

            string copyStr = getConnsFromRows(getSelectedRows());

            if (copyStr != "")
                Clipboard.SetText(copyStr);
        }

        // Copy local address of connection for all selected connections.
        private void localAddressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure there is something to copy.
            if (connectionList == null || connectionList.Count == 0
                || ConnectionGridView == null || ConnectionGridView.SelectedRows.Count == 0)
                return;

            string copyString = "";

            foreach (DataGridViewRow row in getSelectedRows())
                copyString += (row.Cells[2].Value.ToString() + "\n");

            if (copyString != "")
                Clipboard.SetText(copyString);
        }

        // Copy local port of connection for all selected connections.
        private void localPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure there is something to copy.
            if (connectionList == null || connectionList.Count == 0
                || ConnectionGridView == null || ConnectionGridView.SelectedRows.Count == 0)
                return;

            string copyString = "";

            foreach (DataGridViewRow row in getSelectedRows())
                copyString += (row.Cells[3].Value.ToString() + "\n");

            if (copyString != "")
                Clipboard.SetText(copyString);
        }

        // Copy local address and port pair of connection for all selected connections.
        private void localAddressPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure there is something to copy.
            if (connectionList == null || connectionList.Count == 0
                || ConnectionGridView == null || ConnectionGridView.SelectedRows.Count == 0)
                return;

            string copyString = "";

            foreach (DataGridViewRow row in getSelectedRows())
                copyString += (row.Cells[2].Value.ToString() + ":" + row.Cells[3].Value.ToString() + "\n");

            if (copyString != "")
                Clipboard.SetText(copyString);
        }

        // Copy remote address of connection for all selected connections.
        private void remoteAddressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure there is something to copy.
            if (connectionList == null || connectionList.Count == 0
                || ConnectionGridView == null || ConnectionGridView.SelectedRows.Count == 0)
                return;

            string copyString = "";

            foreach (DataGridViewRow row in getSelectedRows())
                copyString += (row.Cells[5].Value.ToString() + "\n");

            if (copyString != "")
                Clipboard.SetText(copyString);
        }

        // Copy remote pair of connection for all selected connections.
        private void remotePortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure there is something to copy.
            if (connectionList == null || connectionList.Count == 0
                || ConnectionGridView == null || ConnectionGridView.SelectedRows.Count == 0)
                    return;

            string copyString = "";

            foreach (DataGridViewRow row in getSelectedRows())
                copyString += (row.Cells[6].Value.ToString() + "\n");

            if (copyString != "")
                Clipboard.SetText(copyString);
        }

        // Copy remote address and port pair of connection for all selected connections.
        private void remoteAddressPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure there is something to copy.
            if (connectionList == null || connectionList.Count == 0
                || ConnectionGridView == null || ConnectionGridView.SelectedRows.Count == 0)
                return;

            string copyString = "";

            foreach (DataGridViewRow row in getSelectedRows())
                copyString += (row.Cells[5].Value.ToString() + ":" + row.Cells[6].Value.ToString() + "\n");

            if (copyString != "")
                Clipboard.SetText(copyString);
        }

        // Copy ISO Country and Subregion code string for all selected connections.
        private void iSOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure there is something to copy.
            if (connectionList == null || connectionList.Count == 0
                || ConnectionGridView == null || ConnectionGridView.SelectedRows.Count == 0)
                return;

            string copyString = "";

            foreach (DataGridViewRow row in getSelectedRows())
                copyString += (row.Cells[9].Value.ToString() + "\n");

            if (copyString != "")
                Clipboard.SetText(copyString);
        }

        // Copy ASN Organization name for all selected connections.
        private void aSNOrganizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure there is something to copy.
            if (connectionList == null || connectionList.Count == 0
                || ConnectionGridView == null || ConnectionGridView.SelectedRows.Count == 0)
                return;

            string copyString = "";

            foreach (DataGridViewRow row in getSelectedRows())
                copyString += (row.Cells[10].Value.ToString() + "\n");

            if (copyString != "")
                Clipboard.SetText(copyString);
        }

        // Copy remote address, port, and related meta info (such as ASN org and GEO info).
        private void allRemoteHostInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure there is something to copy.
            if (connectionList == null || connectionList.Count == 0
                || ConnectionGridView == null || ConnectionGridView.SelectedRows.Count == 0)
                return;

            string copyString = "";

            foreach (DataGridViewRow row in getSelectedRows())
                copyString += ((row.Cells[5].Value.ToString() + ":" + row.Cells[6].Value.ToString()).PadRight(21)
                               + " " + row.Cells[9].Value.ToString().PadRight(6) + " " + row.Cells[10].Value.ToString()
                               + "\n");

            if (copyString != "")
                Clipboard.SetText(copyString);
        }

        // Saves connectionlist to file either with dialog or manually if a dialog was already used.
        private void saveConnListToFile(bool useSaveDialog)
        {
            // Create output stream for file writing.
            Stream saveFileStream;

            // Use save file dialog if requested.
            if (useSaveDialog)
            {
                // Create and setup save file dialog.
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title =
                saveFileDialog.Filter = saveFileFilter;
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;

                // If a file was already saved to before, select that as the assumed save file.
                if (saveFileName != default(string))
                    saveFileDialog.FileName = Path.GetFileName(saveFileName);

                // Show file save dialog.
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                // Check if file opened correctly (shouldn't happen as dialog checks for potential failures).
                if ((saveFileStream = saveFileDialog.OpenFile()) == null)
                {
                    MessageBox.Show("Error: failed to open save file for writing!", Program.Name);

                    return;
                }

                // Save new file name for later operations.
                saveFileName = saveFileDialog.FileName;
            }
            // Open file directly if dialog wasn't requested.
            else
            {
                // If file no longer exists, then prompt the user for new file to save.
                if (!File.Exists(saveFileName))
                {
                    // Call member recursivley to get dialog.
                    saveConnListToFile(true);

                    return;
                }

                // Open file stream for writing.
                try
                {
                    saveFileStream = File.OpenWrite(saveFileName);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Error: failed to open save file for writing:\nPermision denied!", Program.Name);

                    return;
                }

                // Shouldn't happen, checked for file path existence and permissions.
                if (saveFileStream == null)
                {
                    MessageBox.Show("Error: failed to open save file for writing!", Program.Name);

                    return;
                }
            }

            // Make sure file is empty before writing list.
            if (saveFileStream.Length > 0)
            {
                saveFileStream.SetLength(0);
                saveFileStream.Flush();
            }

            // Create sream writer for pushing strings to file.
            StreamWriter saveFileWriter = new StreamWriter(saveFileStream);

            // Write header to file first.
            saveFileWriter.Write(connListHdr);

            // Write all connections to file.
            saveFileWriter.Write(getConnsFromRows(ConnectionGridView.Rows.Cast<DataGridViewRow>().ToList()));

            // Write all data to file before close.
            saveFileWriter.Flush(); 
            // Close file.
            saveFileStream.Close();

            if (!saveToolStripMenuItem.Enabled)
                saveToolStripMenuItem.Enabled = true;
        }

        // Save all connections in list to selected file.
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure there is something to save.
            if (connectionList == null || connectionList.Count == 0
                || ConnectionGridView == null || ConnectionGridView.SelectedRows.Count == 0)
            {
                // Inform user of failure to save and exit.
                MessageBox.Show("Error: cannot save an empty list!", Program.Name);

                return;
            }

            if (saveFileName != default(string))
                saveConnListToFile(false);
            else
                saveConnListToFile(true);
        }

        // Save all connections to new file.
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure there is something to save.
            if (connectionList == null || connectionList.Count == 0
                || ConnectionGridView == null || ConnectionGridView.SelectedRows.Count == 0)
            {
                // Inform user of failure to save and exit.
                MessageBox.Show("Error: list is empty, nothing to save!", Program.Name);

                return;
            }

            saveConnListToFile(true);
        }

        // 
        // Other UI and control event related members.
        // 

        // Disables certian bulk UI elements on when a capture is started.
        private void ChangeOnStartCapture(object sender, EventArgs e)
        {
            // Disable Init Page elements.
            RefreshInterfacesButton.Enabled = false;
            CaptureStartButton.Enabled = false;
            InterfaceLabel.Enabled = false;
            InterfaceDropDownList.Enabled = false;
            clearConnsOnStartCheckBox.Enabled = false;
            CaptureFilterGroupBox.Enabled = false;

            // Enable Connection page elements.
            DisplayFilterGroupBox.Enabled = true;

            // Allow user to stop background worker.
            CaptureStopButton.Enabled = true;
        }

        // Enables certian bulk UI elements on when a capture is stoped.
        private void ChangeOnStopCapture(object sender, EventArgs e)
        {
            // Disable Connection page elements.
            DisplayFilterGroupBox.Enabled = false;

            // Enable Init Page elements.
            RefreshInterfacesButton.Enabled = true;
            CaptureStartButton.Enabled = true;
            InterfaceLabel.Enabled = true;
            InterfaceDropDownList.Enabled = true;
            clearConnsOnStartCheckBox.Enabled = true;
            CaptureFilterGroupBox.Enabled = true;
        }

        // Determine if the user can start a capture or not based on valid drop down selection.
        private void InterfaceDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if interface drop down is on a valid option.
            if (InterfaceDropDownList.SelectedIndex > 0)
            {
                CaptureStartButton.Enabled = true;
                CaptureFilterGroupBox.Enabled = true;
            }
            // Shouldn't be possible after first selection but just to be safe here it is.
            else
            {
                CaptureStartButton.Enabled = false;
                CaptureFilterGroupBox.Enabled = false;
            }
        }

        // Build capture filter from user selected controls for use with shim.
        private int getCaptureFilterStr(out string captureFilterStr)
        {
            // Init string.
            captureFilterStr = String.Empty;

            // Ensure at least one protocol (tcp or udp) is selected in Filter Options.
            if (!TCPCheckBox.Checked && !UDPCheckBox.Checked)
            {
                MessageBox.Show("Error: you must select at least one protocol!", Program.Name);
                return -1;
            }

            // Add protocol to filter.
            if (TCPCheckBox.Checked && UDPCheckBox.Checked)
                captureFilterStr = "( tcp or udp ) and ";
            else if (!TCPCheckBox.Checked && UDPCheckBox.Checked)
                captureFilterStr = "udp and ";
            else
                captureFilterStr = "tcp and ";

            // Check for user entered ip.
            if (SourceIPFilter.Text.Trim().Length > 0)
            {
                // Ensure ip is valid.
                if (!IPAddress.TryParse(SourceIPFilter.Text.Trim(), out IPAddress _))
                {
                    MessageBox.Show("Error: invalid source ip address!", Program.Name);
                    return -1;
                }

                // Add ip to filter string.
                captureFilterStr += ("src host " + SourceIPFilter.Text.Trim() + " and ");
            }

            // Check for user entered port.
            if (SourcePortFilter.Text.Trim().Length > 0)
            {
                // Ensure port number is valid.
                if (!UInt16.TryParse(SourcePortFilter.Text.Trim(), out _))
                {
                    MessageBox.Show("Error: invalid source port number!", Program.Name);
                    return -1;
                }

                // Add port to filter string.
                captureFilterStr += ("src port " + SourcePortFilter.Text.Trim() + " and ");
            }

            // Check for user entered ip.
            if (DestinationIPFilter.Text.Trim().Length > 0)
            {
                // Ensure ip is valid.
                if (!IPAddress.TryParse(DestinationIPFilter.Text.Trim(), out IPAddress _))
                {
                    MessageBox.Show("Error: invalid destination ip address!", Program.Name);
                    return -1;
                }

                // Add ip to filter string.
                captureFilterStr += ("dst host " + DestinationIPFilter.Text.Trim() + " and ");
            }

            // Check for user entered port.
            if (DestinationPortFilter.Text.Trim().Length > 0)
            {
                // Ensure port number is valid.
                if (!UInt16.TryParse(DestinationPortFilter.Text.Trim(), out _))
                {
                    MessageBox.Show("Error: invalid destination port number!", Program.Name);
                    return -1;
                }

                // Add port to filter string.
                captureFilterStr += ("dst port " + DestinationPortFilter.Text.Trim() + " and ");
            }

            // Remove trailing " and ".
            captureFilterStr = captureFilterStr.Remove(captureFilterStr.Length - 5);

            // Check and see if user opted to use complexFilter (directly using libpcap filter string).
            if (complexFilter.Text.Trim().Length > 0)
                captureFilterStr += " and ( " + complexFilter.Text.Trim() + " )";

            return 0;
        }

        // Starts the whole pcap process after user presses button.
        private void CaptureStartButton_Click(object sender, EventArgs e)
        {
            if (InterfaceDropDownList.SelectedIndex > 0)
            {
                // Get libpcap syntax filter string to pass to ShimDotNet.
                string captureFilterStr;

                if (getCaptureFilterStr(out captureFilterStr) == -1)
                    return;

                bool assumeInterfaceIsRaw = assumeRawCheckBox.Checked;

                // Open pcap live session (offset index by -1 because of invalid first entry in drop down list).
                if (captureEngine.startCapture((InterfaceDropDownList.SelectedIndex - 1), captureFilterStr, assumeInterfaceIsRaw) == -1)
                {
                    // If an error happens here we want the user to be able to try another interface.
                    MessageBox.Show(("Error: failed starting capture:\n" + captureEngine.getEngineError()), Program.Name);
                    // Cleanup.
                    captureEngine.stopCapture();
                    return;
                }

                // Check if the binded list has been setup on a previous run.
                if (connectionSource == default(BindingSource))
                {
                    // Setup BindList for binding to BindSource.
                    connectionList = new ConnectionList();
                    connectionList.AllowNew = true;
                    connectionList.AllowEdit = true;
                    connectionList.AllowRemove = true;
                    connectionList.RaiseListChangedEvents = true;
                    
                    // Setup BindSoure for binding to DataGridView.
                    connectionSource = new BindingSource {DataSource = connectionList};

                    // Setup DataGridView and bind BindSource.
                    ConnectionGridView.AutoGenerateColumns = false;
                    ConnectionGridView.ColumnHeadersVisible = true;
                    ConnectionGridView.DataSource = connectionSource;
                }

                // Clear connections list if settings is set.
                if (clearConnsOnStartCheckBox.Checked)
                    ClearConnList();

                // Sets up all background workers.
                DoBWSetup();

                // Start packet batching timer.
                packetTimer.Start();

                // Start displayfilterTimer if display filter is set by user.
                if (_displayFilterSet)
                    displayFilterTimer.Start();

                // Start timeoutTimer if timout is set by user.
                if (TimeoutCheckBox.Checked)
                    timeoutTimer.Start();

                // Change UI elements to reflect new capture state.
                ChangeOnStartCapture(sender, e);

                // Switch over to connections veiw after capture start.
                TabControl.SelectTab(ConnectionPage);
            }
            else
            {
                MessageBox.Show("Error: you must select an interface!", Program.Name);
            }
        }

        // Kill background thread doing pcap processing when user presses button.
        private void CaptureStopButton_Click(object sender, EventArgs e)
        {
            // Don't allow user to stop background worker untill a new one has been started.
            CaptureStopButton.Enabled = false;

            // Stop Capture.
            if (_captureWorker != default(BackgroundWorker) && _captureWorker.IsBusy)
                _captureWorker.CancelAsync();

            // Stop timers.
            packetTimer.Stop();
            timeoutTimer.Stop();
            displayFilterTimer.Stop();
        }

        // Refreshes interface list / info.
        private void RefreshInterfacesButton_Click(object sender, EventArgs e)
        {
            BuildDeviceList();
        }

        // Removes any user filter settings.
        private void ClearFiltersButton_Click(object sender, EventArgs e)
        {
            // Reset protocol check boxes to default.
            TCPCheckBox.Checked = true;
            UDPCheckBox.Checked = true;
            // Reset remove local connections checkbox.
            RemoveLocalConnectionsCheckBox.Checked = true;

            // Remove any text in filter text boxes.
            SourceIPFilter.Clear();
            SourcePortFilter.Clear();
            DestinationIPFilter.Clear();
            DestinationPortFilter.Clear();
            complexFilter.Clear();
        }

        // Clear binded connection list if user presses button.
        private void ClearConnectionsButton_Click(object sender, EventArgs e)
        {
            // Clear connections list.
            ClearConnList();
        }

        // Removes connections tab display filters.
        private void ClearDisplayFiltersButton_Click(object sender, EventArgs e)
        {
            ClearDisplayFilter();
            RemoveDisplayFilter();
        }

        // Allow user to quite without using window quit button (Shortcut ^Q).
        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            // Set this before testing if workers are busy to double ensure that a timing gab is impossible.
            formClosePending = true;

            // If background worker is running then force cleanup and let it handle form close.
            if(_captureWorker != default(BackgroundWorker) && _captureWorker.IsBusy)
            {
                _captureWorker.CancelAsync();
                this.Enabled = false;
                return;
            }

            // Otherwise just close form.
            this.Close();
        }

        // Show about page (well really a msgbox becuase I don't want to waste a form on an about page).
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(aboutPage, Program.Name);
        }

        // Decide if filter timer needs to be started or stoped.
        private void TimeoutCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.TimeoutCheckBox.Checked)
                this.timeoutTimer.Start();
            else
                this.timeoutTimer.Stop();
        }
        // Set timeout limit var.
        private void timeLimit_TextChanged(object sender, EventArgs e)
        {
            // Make sure we have a valid timeout value.
            if (timeLimit.Text.Trim().Length > 0)
            {
                Match badChar = Regex.Match(timeLimit.Text.Trim(), nonNumericRegex);

                if (badChar.Length > 0)
                {
                    MessageBox.Show("Error: invalid timeout limit!", Program.Name);
                    timeLimit.Text = timeoutInactivityLimit.ToString();

                    return;
                }

                if (!Int32.TryParse(timeLimit.Text.Trim(), out timeoutInactivityLimit))
                    MessageBox.Show("Error: invalid timeout limit!", Program.Name);
            }
        }

        // Show dropdown menu on mouse hover.
        private void copyComponentToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            copyComponentToolStripMenuItem.ShowDropDown();
        }

        // Show libpcap and npcap version info.
        private void libpcapVersionInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(captureEngine.getLibVersion(), Program.Name);
        }

        // Check if there is a newer release by comparing embeded release tag to latest release tag.
        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Retrieve current version.
            string tagName = GetCurrentTagName();

            // Application is latest version.
            if (Program.GithubAPI_ReleaseTagElementValue.Equals(tagName))
            {
                // Inform user that there are no available updates.
                MessageBox.Show("No updates are available.", Program.Name);
            }
            // Application is not latest version (not going to do numeric comparision, this should be gud nuf).
            else
            {
                // Inform user of available update, and ask if they would like to visit web page.
                if (MessageBox.Show("An update is available!\n\nCurrent version: \t" + Program.GithubAPI_ReleaseTagElementValue + "\nLatest version: \t"
                                    + tagName + "\n\nWould you like to vist the latest release web page?",
                                    Program.Name, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    // Load URL in default web browser.
                    System.Diagnostics.Process.Start(Program.GithubWEB_LatestReleaseURL);
                }
            }
        }
    }
}
