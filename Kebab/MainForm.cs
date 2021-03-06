﻿
using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;
using System.ComponentModel;
using System.Security.Principal;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

using ShimDotNet;

using MaxMind.Db;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Responses;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kebab
{
    public partial class MainForm : Form
    {
        // Header to match connections for saving list.
        static private readonly string connListHdr = "#    Type    LocalAddress:Port  RXTX    RemoteAddress:Port  PacketsSent  BytesSent       ISO    ASNOrg                                                                                          Note\n";

        // Error string for recognizing removed interface situations.
        static private readonly string devRemovedErrStr = "ERROR_DEVICE_REMOVED/STATUS_DEVICE_REMOVED";
        // Auto downed interface recovery loop config vars.
        static private readonly int autoRecoveryAttempts = 3;
        static private readonly int autoRecoveryInterval = 1000;

        // Indicate if auto recovery fail event needs to be handled after worker close.
        private bool autoRecoveryFail = false;

        // Configuration object.
        private Config programConfig;
        private readonly object programConfigLock = new object();

        // Interface drop down list data source.
        private BindingList<string> deviceList;

        // Grid view data source (connection list).
        private BindingSource connectionSource;
        private ConnectionList connectionList;

        private enum ConnListIndex
        {
            NOTE,
            NUMBER,
            TYPE,
            SRC_HOST,
            SRC_PORT,
            STATE,
            DST_HOST,
            DST_PORT,
            PKT_CNT,
            DATA_SIZE,
            ISO,
            ASN
        }

        // Connection packet queue and lock for capture background worker.
        private readonly Queue<IPV4_PACKET> pendingPackets = new Queue<IPV4_PACKET>();
        private readonly object pendingPacketsLock = new object();

        // Current release tag value and lock for update check background woker.
        private string latestReleaseTagName;
        private readonly object latestReleaseTagNameLock = new object();

        // Packet capture engine (main class from ShimDotNet).
        private CaptureEngine captureEngine;

        // Packet capture needs to happen on a separate thread.
        private readonly BackgroundWorker captureWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
        // Update checking needs to happen on a separate thread.
        private readonly BackgroundWorker updateWorker = new BackgroundWorker { WorkerSupportsCancellation = false };
        // Constantly check if there are pending changes in the config file.
        private readonly BackgroundWorker configWorker = new BackgroundWorker { WorkerSupportsCancellation = true };

        // Timers for updating the connection stats and view.
        private readonly System.Windows.Forms.Timer packetTimer = new System.Windows.Forms.Timer();
        private readonly System.Windows.Forms.Timer timeoutTimer = new System.Windows.Forms.Timer();
        private readonly System.Windows.Forms.Timer displayTimer = new System.Windows.Forms.Timer();
        // Timer for applying pending config changes.
        private readonly System.Windows.Forms.Timer checkConfigTimer = new System.Windows.Forms.Timer();

        // Minimal wait time between packet batch processing in milliseconds.
        private const int packetBatchInterval = 10;
        // Batch packet number limit.
        private const int packetBatchSize = 500;
        // Minimal wait time between inactive entry removal operations in milliseconds.
        private const int timeoutInterval = 500;
        // Inactive entry time limit in seconds.
        private int timeoutInactivityLimit = 30;
        // How often to apply display filter checks in milliseconds.
        private const int displayInterval = 500;
        // How many connections to lookup metadat for per run.
        private const int connMetaLookupBatchSize = 10;
        // How often to check config file in milliseconds.
        private const int checkConfigInterval = 1000;

        // File stream for using save option without reprompting dialog.
        private string saveFileName;
        // Filter string to use for file dialog window.
        static private readonly string saveFileFilter = "text file (*.txt)|*.txt|All files (*.*)|*.*";

        // GeoLite2 dbase filenames.
        static private readonly string IPCityLookupDB = "db/GeoLite2-City.mmdb";
        static private readonly string IPASNLookupDB = "db/GeoLite2-ASN.mmdb";
        // MM GeoLite2 dbase mapper / reader object.
        private DatabaseReader CityReader;
        private DatabaseReader ASNReader;

        // Font to be used for numeric value containers.
        private readonly Font DataFont = new Font("Consolas", 12, FontStyle.Regular);

        // 
        // Main form directly related methods.
        // 

        // Entry function for the form.
        public MainForm()
        {
            // Ensure that the form is double buffered to combat flickering and tearing.
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            // Sets up form components / gui elements.
            InitializeComponent();
        }

        // User defined init (runs after MainForm constructor).
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Force double buffered property on certain controls.
            typeof(Control).InvokeMember("DoubleBuffered",
                                         BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                                         null, ConnectionListView, new object[] { true });

            // Set DGV fonts and fore colors (becuase VS UI editor keeps reseting them).
            ConnectionListView.DefaultCellStyle.Font = DataFont;
            ConnectionListView.DefaultCellStyle.ForeColor = Color.White;
            ConnectionListView.ColumnHeadersDefaultCellStyle.Font = DataFont;
            ConnectionListView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            // Spawn mainform on top (but don't force always on top).
            this.TopMost = true;
            this.TopMost = false;

            // Add timer event handlers.
            packetTimer.Tick += new EventHandler(UpdateConnList);
            timeoutTimer.Tick += new EventHandler(TimeoutConnList);
            displayTimer.Tick += new EventHandler(DisplayConnList);
            checkConfigTimer.Tick += new EventHandler(CheckConfig);

            // Set timer intervals.
            packetTimer.Interval = packetBatchInterval;
            timeoutTimer.Interval = timeoutInterval;
            displayTimer.Interval = displayInterval;
            checkConfigTimer.Interval = checkConfigInterval;

            // Setup background workers.
            captureWorker.DoWork += CaptureWorker_DoWork;
            captureWorker.RunWorkerCompleted += CaptureWorker_RunWorkerCompleted;
            updateWorker.DoWork += UpdateWorker_DoWork;
            updateWorker.RunWorkerCompleted += UpdateWorker_RunWorkerCompleted;
            configWorker.DoWork += ConfigWorker_DoWork;

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
            // Incase npcap is not installed (could be thrown if ShimDotNet can't load wpcap.dll).
            catch (FileNotFoundException)
            {
                MessageBox.Show(("Error: failed to initilize capture engine!"
                                 + "\n\nMake sure that you have a recent version of " + Program.Name + "."
                                 + "\n\nAnd make sure that Npcap is installed properly: https://nmap.org/npcap/"), Program.Name);
                System.Environment.Exit(1);
            }

            // Run init routine to make sure that everything (mostly npcap related) is alright.
            if (!captureEngine.init())
            {
                MessageBox.Show(("Error: failed to initilize capture engine!"
                                 + "\n\nMake sure that you have a recent version of " + Program.Name + "."
                                 + "\n\nAnd make sure that Npcap is installed properly: https://nmap.org/npcap/"), Program.Name);
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
                MessageBox.Show(("Error: could not find a database file: " + ex.Message
                                 + "\n\nMake sure that you have a recent version of " + Program.Name + "."), Program.Name);
                System.Environment.Exit(1);
            }
            catch (InvalidDatabaseException ex)
            {
                // Warn user of missing database file and exit.
                MessageBox.Show(("Error: invalid or corrupt database file: " + ex.Message
                                 + "\n\nMake sure that you have a recent version of " + Program.Name + "."), Program.Name);
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
            programConfig.ClearPendingChanges();

            // Attempt to overwrite config file with default and or loaded values.
            if (!programConfig.SaveConfig())
            {
                MessageBox.Show(("Error: failed to save configuration file!" + "\n\n" + Program.Name +
                                 " will run in non-persistent config mode, user preferences will not be saved!"),
                                Program.Name);
            }
            else
                programConfig.SaveOnLoad = true;

            // Check if auto update functionality is enabled.
            if (programConfig.CVars.update_check == "true")
            {
                // Retrieve current version.
                latestReleaseTagName = GetCurrentTagName();

                // Check if application is the latest version.
                if (!Program.GithubAPI_ReleaseTagElementValue.Equals(latestReleaseTagName))
                {
                    // Inform user of available update, and ask if they would like to visit web page.
                    if (MessageBox.Show("An update is available!\n\nCurrent version: \t" + Program.GithubAPI_ReleaseTagElementValue + "\nLatest version: \t"
                                        + latestReleaseTagName + "\n\nWould you like to vist the latest release web page?",
                                        Program.Name, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        // Load URL in default web browser.
                        System.Diagnostics.Process.Start(Program.GithubWEB_LatestReleaseURL);
                    }
                }
            }

            // Enable auto config check mechanisms.
            ConfigWorker_Start();
            checkConfigTimer.Start();

            // Enable form after init is done.
            this.Enabled = true;
        }

        // Sends a message to the specified window.
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        // Releases the mouse capture from the window.
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        // Determine what part of the window was clicked on by the mouse.
        private const int WM_NCHITTEST = 0x84;
        // Left mouse button down.
        private const int WM_NCLBUTTONDOWN = 0xA1;

        // The client area.
        private const int HTCLIENT = 0x01;
        // Title bar area (just under the upper border).
        private const int HTCAPTION = 0x02;
        // Left border area.
        private const int HTLEFT = 0x0A;
        // Right border area.
        private const int HTRIGHT = 0x0B;
        // Upper border area.
        private const int HTTOP = 0x0C;
        // Upper left corner border area.
        private const int HTTOPLEFT = 0x0D;
        // Upper right corner border area.
        private const int HTTOPRIGHT = 0x0E;
        // Lower border area.
        private const int HTBOTTOM = 0x0F;
        // Lower left corner border area.
        private const int HTBOTTOMLEFT = 0x10;
        // Lower right corner border area.
        private const int HTBOTTOMRIGHT = 0x11;

        // Create resize handles on edges of client area for a borderless form.
        protected override void WndProc(ref Message m)
        {
            // Size of region that will result in a resize handle being drawn.
            const int RESIZE_REGION_SIZE = 3;

            // Run base functionality.
            base.WndProc(ref m);

            // Check window event type.
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    if ((int)m.Result == HTCLIENT)
                    {
                        // Get client relative coordinates.
                        Point screenPoint = new Point(m.LParam.ToInt32());
                        Point clientPoint = this.PointToClient(screenPoint);

                        // Handle being in the upper region.
                        if (clientPoint.Y <= RESIZE_REGION_SIZE)
                        {
                            if (clientPoint.X <= RESIZE_REGION_SIZE)
                                m.Result = (IntPtr)HTTOPLEFT;
                            else if (clientPoint.X < (Size.Width - RESIZE_REGION_SIZE))
                            {
                                // If maximized we want to be dragable from the very top.
                                if (this.WindowState != FormWindowState.Maximized)
                                    m.Result = (IntPtr)HTTOP;
                                else
                                    m.Result = (IntPtr)HTCAPTION;
                            }
                            else
                                m.Result = (IntPtr)HTTOPRIGHT;
                        }
                        // Handle being in the middle region (most of the form).
                        else if (clientPoint.Y <= (Size.Height - RESIZE_REGION_SIZE))
                        {
                            if (clientPoint.X <= RESIZE_REGION_SIZE)
                                m.Result = (IntPtr)HTLEFT;
                            else if (clientPoint.X < (Size.Width - RESIZE_REGION_SIZE))
                                m.Result = (IntPtr)HTCAPTION;
                            else
                                m.Result = (IntPtr)HTRIGHT;
                        }
                        // Handle being in the lower region.
                        else
                        {
                            if (clientPoint.X <= RESIZE_REGION_SIZE)
                                m.Result = (IntPtr)HTBOTTOMLEFT;
                            else if (clientPoint.X < (Size.Width - RESIZE_REGION_SIZE))
                                m.Result = (IntPtr)HTBOTTOM;
                            else
                                m.Result = (IntPtr)HTBOTTOMRIGHT;
                        }

                        // Don't show draggable handles if maximized.
                        if (this.WindowState == FormWindowState.Maximized
                            && (int)m.Result != HTCAPTION)
                            m.Result = (IntPtr)HTCLIENT;
                    }
                    
                    break;
                default:
                    break;
            }
        }

        // Ensure that form is treated as the first control in a group of controls.
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // Turn on WS_GROUP.
                cp.Style |= 0x20000;

                return cp;
            }
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
                return String.Empty;
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
                return String.Empty;
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
                return String.Empty;
            }

            return tagName;
        }

        // Apply loaded configuration settings.
        private void ApplyConfig()
        {
            // Apply banner message string to mainform title.
            if (programConfig.CVars.flavor_text.Length > 0)
                TitleBarLebel.Text = (Program.Name + " - " + programConfig.CVars.flavor_text);
            else
                TitleBarLebel.Text = Program.Name;

            // Apply connection tags.
            if (connectionList?.Count > 0)
            {
                foreach (Connection conn in connectionList)
                {
                    foreach (string[] tag in programConfig.CVars.tag_list)
                    {
                        if (tag[0] == conn.DstHost.Address.ToString())
                        {
                            conn.Note = tag[1];
                            break;
                        }
                    }
                }
            }
        }

        // Check if there are pending config file changes that need to be applied.
        private void CheckConfig(object sender, EventArgs e)
        {
            lock (programConfigLock)
            {
                if (programConfig.ChangesPending)
                {
                    // Set default and or loaded config.
                    ApplyConfig();
                    programConfig.ClearPendingChanges();
                }
            }
        }

        // Reusable grouping for cleanup tasks when form needs to close.
        private void MainFormCleanup()
        {
            // Stop timers.
            packetTimer.Stop();
            timeoutTimer.Stop();
            displayTimer.Stop();
            checkConfigTimer.Stop();

            // Dispose timers.
            packetTimer.Dispose();
            timeoutTimer.Dispose();
            displayTimer.Dispose();
            checkConfigTimer.Dispose();

            // Dispose backgorund workers.
            captureWorker.Dispose();
            updateWorker.Dispose();
            configWorker.Dispose();

            // Dispose geolite constructs.
            CityReader.Dispose();
            ASNReader.Dispose();
        }

        // Overide form close to do background worker cleanup.
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // If background worker is running then force cleanup and let it handle form close.
            if (captureWorker != default(BackgroundWorker) && captureWorker.IsBusy)
            {
                captureWorker.CancelAsync();
                _captureWorkerDone.WaitOne();
            }

            if (updateWorker != default(BackgroundWorker) && updateWorker.IsBusy)
            {
                _updateWorkerDone.WaitOne();
            }

            if (configWorker != default(BackgroundWorker) && configWorker.IsBusy)
            {
                configWorker.CancelAsync();
                _configWorkerDone.WaitOne();
            }

            // Do ShimDotNet cleanup.
            captureEngine.stopCapture();

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
                                ifaceName += (" [ " + ip.Address.ToString() + ConnectionAddress.NetMaskToPrefix(ip.IPv4Mask) + " ]");
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

        // End of thread flag.
        private readonly AutoResetEvent _captureWorkerDone = new AutoResetEvent(false);
        // Threaded pcap packet processing loop.
        private void CaptureWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Create a reference worker so we can check for cancel signal.
            BackgroundWorker thisWorker = sender as BackgroundWorker;

            // Start packet capture loop.
            while (!thisWorker.CancellationPending)
            {
                IPV4_PACKET pkt = new IPV4_PACKET();
                int returnCode = captureEngine.getNextPacket(ref pkt);

                // Process a packet / connection, and add it to the queue.
                if (returnCode == 0)
                {
                    // Obtain lock on queue and then add connection / packet.
                    lock (pendingPacketsLock)
                    {
                        pendingPackets.Enqueue(pkt);
                    }
                }
                // Deal with capture failures.
                else if (returnCode == -3)
                {
                    // Check for downed interface situation.
                    if (Regex.Match(captureEngine.getEngineError(), devRemovedErrStr).Length > 0)
                    {
                        // Mark recovery attempt as active.
                        bool recovered = false;

                        // Attempt to recover from downed interface.
                        for (int recoveryAttempts = 0; recoveryAttempts < autoRecoveryAttempts; recoveryAttempts++)
                        {
                            // If reload succeeds then we can stop the loop.
                            if (captureEngine.reloadCapture() != -1)
                            {
                                recovered = true;
                                break;
                            }

                            // Otherwise wait for specified interval.
                            Thread.Sleep(autoRecoveryInterval);
                        }

                        // If autorecovery failed we need to warn the user and stop the capture loop.
                        if (!recovered)
                        {
                            // Set flag for RunWorkerCompleted.
                            autoRecoveryFail = true;
                            // Break the capture loop.
                            break;
                        }
                    }
                }
            }

            // Signal end of thread work.
            _captureWorkerDone.Set();
        }

        // Handle auto recovery fail event.
        private void CaptureWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (autoRecoveryFail)
            {
                // Stop capture.
                StopCapture();
                // Refresh device list.
                BuildDeviceList();

                // Notify user.
                MessageBox.Show("Error: The active interface went down and auto recovery failed!"
                                + "\n\nStart a new capture when ready.", Program.Name);
                // Reset flag.
                autoRecoveryFail = false;
            }
        }

        // Initilize capture worker.
        private void CaptureWorker_Start()
        {
            // Don't start worker until it's done running.
            if (captureWorker.IsBusy)
                return;

            // Start packet capture background worker.
            captureWorker.RunWorkerAsync();
        }

        // End of thread flag.
        private readonly AutoResetEvent _updateWorkerDone = new AutoResetEvent(false);
        // Parses github json API to determine if an update is available.
        private void UpdateWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Retrieve current version.
            lock (latestReleaseTagNameLock)
            {
                latestReleaseTagName = GetCurrentTagName();
            }

            // Signal end of thread work.
            _updateWorkerDone.Set();
        }

        // Shows update information after update check has been performed.
        private void UpdateWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lock (latestReleaseTagNameLock)
            {
                // Application is the latest version.
                if (Program.GithubAPI_ReleaseTagElementValue.Equals(latestReleaseTagName))
                {
                    // Inform user that there are no available updates.
                    MessageBox.Show("No updates are available.", Program.Name);
                }
                // Application is not latest version (not going to do numeric comparision, this should be gud nuf).
                else
                {
                    // Inform user of available update, and ask if they would like to visit web page.
                    if (MessageBox.Show("An update is available!\n\nCurrent version: \t" + Program.GithubAPI_ReleaseTagElementValue + "\nLatest version: \t"
                                        + latestReleaseTagName + "\n\nWould you like to vist the latest release web page?",
                                        Program.Name, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        // Load URL in default web browser.
                        System.Diagnostics.Process.Start(Program.GithubWEB_LatestReleaseURL);
                    }
                }
            }
        }

        // Guards the starting of update check worker.
        private void UpdateWorker_Start()
        {
            // Don't continue if a background worker already exists and is still running.
            if (updateWorker.IsBusy)
                return;

            // Start packet capture background worker.
            updateWorker.RunWorkerAsync();
        }

        // End of thread flag.
        private readonly AutoResetEvent _configWorkerDone = new AutoResetEvent(false);
        // Checks for pending changes by continuously loading config file.
        private void ConfigWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Create a reference worker so we can check for cancel signal.
            BackgroundWorker thisWorker = sender as BackgroundWorker;

            // Start config file checking loop.
            while (!thisWorker.CancellationPending)
            {
                lock (programConfigLock)
                {
                    programConfig.LoadConfig();
                }

                Thread.Sleep(checkConfigInterval / 2);
            }

            // Signal end of thread work.
            _configWorkerDone.Set();
        }

        // Guards the starting of update check worker.
        private void ConfigWorker_Start()
        {
            // Don't continue if a background worker already exists and is still running.
            if (configWorker.IsBusy)
                return;

            // Start packet capture background worker.
            configWorker.RunWorkerAsync();
        }

        // 
        // Display filter related methods.
        // 

        // Colors matching connections differently from non-matching connections.
        private void ApplyDisplayFilter()
        {
            // Loop through connectionList and hide any connections that don't match filter (and unhide ones that do).
            foreach (DataGridViewRow row in ConnectionListView.Rows)
            {
                if (!_displayFilter.IsMatch((Connection)row.DataBoundItem))
                {
                    row.DefaultCellStyle.ForeColor = SystemColors.GrayText;
                    row.DefaultCellStyle.Font = ConnectionListView.DefaultCellStyle.Font;
                }
                else
                {
                    row.DefaultCellStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#08D088");
                    row.DefaultCellStyle.Font = new Font(ConnectionListView.DefaultCellStyle.Font.Name,
                                                         ConnectionListView.DefaultCellStyle.Font.Size,
                                                         FontStyle.Bold);
                }
            }

            // Show displayfilter changes.
            ConnectionListView.Invalidate();
        }

        // Clears text from display filter and removes filter from connections list.
        private void ClearDisplayFilter()
        {
            // Clear display filter text.
            DisplayFilterStr.Clear();
        }

        // Removes filter from connection list.
        private void RemoveDisplayFilter()
        {
            // Remove displayfilter font changes.
            foreach (DataGridViewRow row in ConnectionListView.Rows)
            {
                row.DefaultCellStyle.ForeColor = ConnectionListView.DefaultCellStyle.ForeColor;
                row.DefaultCellStyle.Font = ConnectionListView.DefaultCellStyle.Font;
            }

            // Show displayfilter changes.
            ConnectionListView.Invalidate();
        }

        private DisplayFilter _displayFilter;
        private bool _displayFilterActive = false;
        // Force repaint for display filter adhearence.
        private void DisplayFilter_TextChanged(object sender, EventArgs e)
        {
            // Check if we have a valid display filter string.
            if (DisplayFilterStr.Text.Trim().Length > 0 && DisplayFilter.TryParse(DisplayFilterStr.Text.Trim(), out _displayFilter))
            {
                // Set font to indicate a valid filter.
                if (DisplayFilterStr.ForeColor != SystemColors.WindowText)
                    DisplayFilterStr.ForeColor = SystemColors.WindowText;

                // Enable display filtering.
                _displayFilterActive = true;
                ApplyDisplayFilter();
                return;
            }

            // Set red font to indicate invalid / empty filter.
            if (DisplayFilterStr.ForeColor != Color.Red)
                DisplayFilterStr.ForeColor = Color.Red;

            // Otherwise if a filter is active we need to disable it.
            if (_displayFilterActive)
            {
                RemoveDisplayFilter();
                _displayFilterActive = false;
            }
        }

        // Force repaint for display filter adhearence when rows are added.
        private void ConnectionListView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (_displayFilterActive)
                ApplyDisplayFilter();
        }
        // Force repaint for display filter adhearence when the list is sorted by a property.
        private void ConnectionListView_Sorted(object sender, EventArgs e)
        {
            if (_displayFilterActive)
                ApplyDisplayFilter();
        }

        // Flag to determine if selection is currently allowed.
        private bool _selectionDisabled = true;
        // Disable automatically selecting rows.
        private void ConnectionListView_SelectionChanged(object sender, EventArgs e)
        {
            if (_selectionDisabled)
                ConnectionListView.ClearSelection();
        }

        // Disables row selection and ensures current row is not selected if applicable.
        private void DisableRowSelection()
        {
            if (!_selectionDisabled)
            {
                // Disable selection of current ruow.
                _selectionDisabled = true;

                // Deselect rows.
                ConnectionListView.ClearSelection();

                // Disable context meny items (for copying conn info).
                foreach (ToolStripItem item in ConnectionContextMenu.Items)
                    item.Enabled = false;
            }
        }

        // Enables row selection and ensures current row is selected if applicable.
        private void EnableRowSelection()
        {
            if (_selectionDisabled)
            {
                // Enable selection of current ruow.
                _selectionDisabled = false;

                // Select current row if applicaable.
                if (ConnectionListView.CurrentRow != null)
                    ConnectionListView.CurrentRow.Selected = true;

                // Enable context meny items (for copying conn info).
                foreach (ToolStripItem item in ConnectionContextMenu.Items)
                    item.Enabled = true;
            }
        }

        // Enable row selection on click.
        private void ConnectionListView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            EnableRowSelection();
        }

        // Clear row selection on header click.
        private void ConnectionListView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DisableRowSelection();
        }

        // Handle row selection events on certian key releases.
        private void ConnectionListView_KeyUp(object sender, KeyEventArgs e)
        {
            // Disable selection on escape.
            if (e.KeyCode == Keys.Escape)
            {
                DisableRowSelection();
            }
            // Select all on Ctrl+A.
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                EnableRowSelection();
                ConnectionListView.SelectAll();
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

                // Make sure we don't add a new connection when a matching one already exists and was updated.
                bool packetMatched = false;

                // Loop through connections list and see if we have a new connection.
                foreach (Connection conn in connectionList)
                {
                    // Get match type of packet.
                    MatchType packetMatch = conn.PacketMatch(pkt);

                    // Find matching connections(regardless of source and dest orientation).
                    if (packetMatch != MatchType.NON_MATCH)
                    {
                        conn.PacketCount++;
                        conn.DataSize += pkt.payload_size;
                        conn.TimeStamp = DateTime.Now;

                        // Check if direction needs to be updated to both ways.
                        if (conn.State.Direction != TransmissionDirection.TWO_WAY)
                        {
                            // Reverse match with both packets going from src to dest means we have a two way conn.
                            if (packetMatch == MatchType.REV_MATCH && conn.State.Direction == TransmissionDirection.ONE_WAY)
                                conn.State.Direction = TransmissionDirection.TWO_WAY;
                            // Matched packet but the original had src, dst, and direction flipped, means we have a two way conn.
                            else if (packetMatch == MatchType.MATCH && conn.State.Direction == TransmissionDirection.REV_ONE_WAY)
                                conn.State.Direction = TransmissionDirection.TWO_WAY;
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
                    Connection newConn = new Connection(pkt);

                    // Don't create local connections if not wanted by user.
                    if (newConn.SrcHost.AddressIsLocal() && newConn.DstHost.AddressIsLocal() && RemoveLocalConnectionsCheckBox.Checked)
                    {
                        // Mark packet as processed.
                        packetsToProcess--;
                        // Move on to next packet.
                        continue;
                    }
                    // Need to do a check to make sure that local and loobpack connections get priority for the Source address.
                    else if (newConn.DstHost.AddressIsLocal() && !newConn.SrcHost.AddressIsLocal())
                    {
                        // Swap IP addrs.
                        IPAddress tmpIP = newConn.DstHost.Address;
                        newConn.DstHost.Address = newConn.SrcHost.Address;
                        newConn.SrcHost.Address = tmpIP;

                        // Swap ports.
                        UInt16 tmpUint16 = newConn.DstPort;
                        newConn.DstPort = newConn.SrcPort;
                        newConn.SrcPort = tmpUint16;

                        // Flip direction.
                        newConn.State.Direction = TransmissionDirection.REV_ONE_WAY;
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
        private void ReorderOnInsertionDeletion()
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
                foreach (Connection conn in connectionList)
                {
                    // Find Inactive connetion based on timestamp.
                    if ((DateTime.Now - conn.TimeStamp).TotalSeconds >= timeoutInactivityLimit)
                    {
                        // Remove inactive entry and mark flag so that we will do another inactive connection check.
                        connectionList.Remove(conn);
                        removedConnection = true;
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
                        ReorderOnInsertionDeletion();

                        // Refresh DataGridView values to avoid errors with pending UI events on removed connections.
                        ConnectionListView.Invalidate();
                    }

                    break;
                }
            }
        }

        // Apply displayfilter if enabled.
        private void FilterConnList()
        {
            if (_displayFilterActive)
                ApplyDisplayFilter();
        }

        // Update connections with GEO / ISO and ASN info.
        private void AddConnListMeta()
        {
            // Make sure we process no more than the batch size.
            int connsToLookup = connectionList.Count;

            if (connsToLookup > connMetaLookupBatchSize)
                connsToLookup = connMetaLookupBatchSize;

            foreach (Connection conn in connectionList)
            {
                // Make sure we don't go ove limit.
                if (connsToLookup == 0)
                    break;

                // Make sure meta check hasn't been done before.
                if (conn.MetaDataAdded)
                    continue;

                // Add geographical info.
                if (CityReader.TryCity(conn.DstHost.ToString(), out CityResponse cityResp))
                {
                    // Add info from response object, or "--" marker for empty components of the response.
                    if (cityResp.Country.Name != default)
                        conn.DstGeo.Country = cityResp.Country.Name;

                    if (cityResp.Country.IsoCode != default)
                        conn.DstGeo.CountryISO = cityResp.Country.IsoCode;

                    if (cityResp.MostSpecificSubdivision.Name != default)
                        conn.DstGeo.State = cityResp.MostSpecificSubdivision.Name;

                    if (cityResp.MostSpecificSubdivision.IsoCode != default)
                        conn.DstGeo.StateISO = cityResp.MostSpecificSubdivision.IsoCode;

                    if (cityResp.City.Name != default)
                        conn.DstGeo.City = cityResp.City.Name;
                }

                // Add ASN info.
                if (ASNReader.TryAsn(conn.DstHost.ToString(), out AsnResponse asnResp))
                {
                    conn.DstASN = asnResp.AutonomousSystemNumber;

                    if (asnResp.AutonomousSystemOrganization != default)
                        conn.DstASNOrg = asnResp.AutonomousSystemOrganization;
                }

                // Mark metadata lookup done.
                conn.MetaDataAdded = true;
                // One down.
                connsToLookup--;
            }
        }

        // Applys connection meta data and filters.
        private void DisplayConnList(object sender, EventArgs e)
        {
            AddConnListMeta();
            FilterConnList();
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
            _selectionDisabled = true;
            ConnectionListView.ClearSelection();
            connectionList.Clear();
            
            // Make control redraw.
            ConnectionListView.Invalidate();
        }

        //
        // Copy and save functionality members.
        //

        // Return selected row list (fixes reverse order bug).
        private List<DataGridViewRow> GetSelectedRows()
        {
            List<DataGridViewRow> rows =
            (from DataGridViewRow row in ConnectionListView.SelectedRows
             where !row.IsNewRow
             orderby row.Index
             select row).ToList<DataGridViewRow>();

            return rows;
        }

        // Turns specified rows from connListView into formated conn lines as a string for copy and save functions.
        private string GetConnsFromRows(List<DataGridViewRow> rows)
        {
            string connList = String.Empty;

            foreach (DataGridViewRow row in rows)
            {
                connList += (row.Cells[(int)ConnListIndex.NUMBER].Value.ToString().PadRight(4) + " "
                             + row.Cells[(int)ConnListIndex.TYPE].Value.ToString().PadRight(4) + " "
                             + row.Cells[(int)ConnListIndex.SRC_HOST].Value.ToString().PadLeft(15) + ":" + row.Cells[(int)ConnListIndex.SRC_PORT].Value.ToString().PadRight(5) + " "
                             + row.Cells[(int)ConnListIndex.STATE].Value.ToString().PadRight(5) + " "
                             + row.Cells[(int)ConnListIndex.DST_HOST].Value.ToString().PadLeft(15) + ":" + row.Cells[(int)ConnListIndex.DST_PORT].Value.ToString().PadRight(5) + " "
                             + row.Cells[(int)ConnListIndex.PKT_CNT].Value.ToString().PadRight(12) + " "
                             + row.Cells[(int)ConnListIndex.DATA_SIZE].Value.ToString().PadRight(15) + " "
                             + row.Cells[(int)ConnListIndex.ISO].Value.ToString().PadRight(6) + " "
                             + row.Cells[(int)ConnListIndex.ASN].Value.ToString().PadRight(96) + " "
                             + row.Cells[(int)ConnListIndex.NOTE].Value.ToString()
                             + "\n");
            }

            return connList;
        }

        // Copy all selected rows with right click.
        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure there is something to copy.
            if (connectionList == null || connectionList.Count == 0
                || ConnectionListView == null || ConnectionListView.SelectedRows.Count == 0)
                return;

            string connList = GetConnsFromRows(GetSelectedRows());

            if (connList != String.Empty)
                Clipboard.SetText(connList);
        }

        // Copy local address of connection for all selected connections.
        private void LocalAddressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure there is something to copy.
            if (connectionList == null || connectionList.Count == 0
                || ConnectionListView == null || ConnectionListView.SelectedRows.Count == 0)
                return;

            string localAddrList = String.Empty;

            foreach (DataGridViewRow row in GetSelectedRows())
                localAddrList += (row.Cells[(int)ConnListIndex.SRC_HOST].Value.ToString() + "\n");

            if (localAddrList != String.Empty)
                Clipboard.SetText(localAddrList);
        }

        // Copy local port of connection for all selected connections.
        private void LocalPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure there is something to copy.
            if (connectionList == null || connectionList.Count == 0
                || ConnectionListView == null || ConnectionListView.SelectedRows.Count == 0)
                return;

            string localPortList = String.Empty;

            foreach (DataGridViewRow row in GetSelectedRows())
                localPortList += (row.Cells[(int)ConnListIndex.SRC_PORT].Value.ToString() + "\n");

            if (localPortList != String.Empty)
                Clipboard.SetText(localPortList);
        }

        // Copy local address and port pair of connection for all selected connections.
        private void LocalAddressPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure there is something to copy.
            if (connectionList == null || connectionList.Count == 0
                || ConnectionListView == null || ConnectionListView.SelectedRows.Count == 0)
                return;

            string localAddrPortList = String.Empty;

            foreach (DataGridViewRow row in GetSelectedRows())
                localAddrPortList += (row.Cells[(int)ConnListIndex.SRC_HOST].Value.ToString()
                                      + ":" + row.Cells[(int)ConnListIndex.SRC_PORT].Value.ToString() + "\n");

            if (localAddrPortList != String.Empty)
                Clipboard.SetText(localAddrPortList);
        }

        // Copy remote address of connection for all selected connections.
        private void RemoteAddressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure there is something to copy.
            if (connectionList == null || connectionList.Count == 0
                || ConnectionListView == null || ConnectionListView.SelectedRows.Count == 0)
                return;

            string remoteAddrList = String.Empty;

            foreach (DataGridViewRow row in GetSelectedRows())
                remoteAddrList += (row.Cells[(int)ConnListIndex.DST_HOST].Value.ToString() + "\n");

            if (remoteAddrList != String.Empty)
                Clipboard.SetText(remoteAddrList);
        }

        // Copy remote pair of connection for all selected connections.
        private void RemotePortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure there is something to copy.
            if (connectionList == null || connectionList.Count == 0
                || ConnectionListView == null || ConnectionListView.SelectedRows.Count == 0)
                    return;

            string remotePortList = String.Empty;

            foreach (DataGridViewRow row in GetSelectedRows())
                remotePortList += (row.Cells[(int)ConnListIndex.DST_PORT].Value.ToString() + "\n");

            if (remotePortList != String.Empty)
                Clipboard.SetText(remotePortList);
        }

        // Copy remote address and port pair of connection for all selected connections.
        private void RemoteAddressPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure there is something to copy.
            if (connectionList == null || connectionList.Count == 0
                || ConnectionListView == null || ConnectionListView.SelectedRows.Count == 0)
                return;

            string remoteAddrPortList = String.Empty;

            foreach (DataGridViewRow row in GetSelectedRows())
                remoteAddrPortList += (row.Cells[(int)ConnListIndex.DST_HOST].Value.ToString()
                                       + ":" + row.Cells[(int)ConnListIndex.DST_PORT].Value.ToString() + "\n");

            if (remoteAddrPortList != String.Empty)
                Clipboard.SetText(remoteAddrPortList);
        }

        // Copy ISO Country and Subregion code string for all selected connections.
        private void ISOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure there is something to copy.
            if (connectionList == null || connectionList.Count == 0
                || ConnectionListView == null || ConnectionListView.SelectedRows.Count == 0)
                return;

            string ISOList = String.Empty;

            foreach (DataGridViewRow row in GetSelectedRows())
                ISOList += (row.Cells[(int)ConnListIndex.ISO].Value.ToString() + "\n");

            if (ISOList != String.Empty)
                Clipboard.SetText(ISOList);
        }

        // Copy ASN Organization name for all selected connections.
        private void ASNOrganizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure there is something to copy.
            if (connectionList == null || connectionList.Count == 0
                || ConnectionListView == null || ConnectionListView.SelectedRows.Count == 0)
                return;

            string ASNOrgList = String.Empty;

            foreach (DataGridViewRow row in GetSelectedRows())
                ASNOrgList += (row.Cells[(int)ConnListIndex.ASN].Value.ToString() + "\n");

            if (ASNOrgList != String.Empty)
                Clipboard.SetText(ASNOrgList);
        }

        // Copy remote address, port, and related meta info (such as ASN org and GEO info).
        private void AllRemoteHostInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure there is something to copy.
            if (connectionList == null || connectionList.Count == 0
                || ConnectionListView == null || ConnectionListView.SelectedRows.Count == 0)
                return;

            string remoteHostInfoList = String.Empty;

            foreach (DataGridViewRow row in GetSelectedRows())
                remoteHostInfoList += ((row.Cells[(int)ConnListIndex.DST_HOST].Value.ToString()
                                       + ":" + row.Cells[(int)ConnListIndex.DST_PORT].Value.ToString()).PadRight(21)
                                       + " " + row.Cells[(int)ConnListIndex.ISO].Value.ToString().PadRight(6)
                                       + " " + row.Cells[(int)ConnListIndex.ASN].Value.ToString().PadRight(96)
                                       + " " + row.Cells[(int)ConnListIndex.NOTE].Value.ToString()
                                       + "\n");

            if (remoteHostInfoList != String.Empty)
                Clipboard.SetText(remoteHostInfoList);
        }

        // Saves connectionlist to file either with dialog or manually if a dialog was already used.
        private void SaveConnListToFile(bool useSaveDialog)
        {
            // Create output stream for file writing.
            Stream saveFileStream;

            // Use save file dialog if requested.
            if (useSaveDialog)
            {
                // Create and setup save file dialog.
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Title = "Save As",
                    Filter = saveFileFilter,
                    FilterIndex = 1,
                    RestoreDirectory = true
                };

                // If a file was already saved to before, select that as the assumed save file.
                if (saveFileName != default)
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
            saveFileWriter.Write(GetConnsFromRows(ConnectionListView.Rows.Cast<DataGridViewRow>().ToList()));

            // Write all data to file before close.
            saveFileWriter.Flush(); 
            // Close file.
            saveFileStream.Close();

            SaveMenuItem.Enabled = true;
        }

        // Save all connections in list to selected file.
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure there is something to save.
            if (ConnectionListView == default(DataGridView) || ConnectionListView.Rows.Count == 0)
            {
                // Inform user of failure to save and exit.
                MessageBox.Show("Error: connection list is empty, there is nothing to save!", Program.Name);

                return;
            }

            SaveConnListToFile(false);
        }

        // Save all connections to new file.
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure there is something to save.
            if (ConnectionListView == default(DataGridView) || ConnectionListView.Rows.Count == 0)
            {
                // Inform user of failure to save and exit.
                MessageBox.Show("Error: connection list is empty, there is nothing to save!", Program.Name);

                return;
            }

            SaveConnListToFile(true);
        }

        // 
        // Other UI and control event related members.
        // 

        // Determine if the user can start a capture or not based on valid drop down selection.
        private void InterfaceDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if interface drop down is on a valid option.
            if (InterfaceDropDownList.SelectedIndex > 0)
            {
                StartCaptureButton.Enabled = true;
                ClearConnsOnStartCheckBox.Enabled = true;
                RemoveLocalConnectionsCheckBox.Enabled = true;
                ForceRawCheckBox.Enabled = true;

                if (!ForceRawCheckBox.Checked)
                {
                    CaptureFilter.Enabled = true;
                    CaptureFilterLabel.Enabled = true;
                    ClearCaptureFilterButton.Enabled = true;
                }
            }
            // Shouldn't be possible after first selection but just to be safe here it is.
            else
            {
                StartCaptureButton.Enabled = false;
                ClearConnsOnStartCheckBox.Enabled = false;
                RemoveLocalConnectionsCheckBox.Enabled = false;
                ForceRawCheckBox.Enabled = false;
                CaptureFilter.Enabled = false;
                CaptureFilterLabel.Enabled = false;
                ClearCaptureFilterButton.Enabled = false;
            }
        }

        // Build capture filter from user selected controls for use with shim.
        private int GetCaptureFilter(out string captureFilter)
        {
            // Init string.
            captureFilter = String.Empty;

            // Don't copy filter info if forcing raw interface.
            if (ForceRawCheckBox.Checked)
                return 0;

            // Check and see if user opted to use complexFilter (directly using libpcap filter string).
            if (CaptureFilter.Text.Trim().Length > 0)
                captureFilter = CaptureFilter.Text.Trim().ToLower();

            return 0;
        }

        // Starts capture of packets.
        private void StartCapture()
        {
            if (InterfaceDropDownList.SelectedIndex > 0)
            {
                // Get libpcap syntax filter string to pass to ShimDotNet.
                if (GetCaptureFilter(out string captureFilter) == -1)
                    return;

                // Open pcap live session (offset index by -1 because of invalid first entry in drop down list).
                if (captureEngine.startCapture((InterfaceDropDownList.SelectedIndex - 1), captureFilter, ForceRawCheckBox.Checked) == -1)
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
                    connectionList = new ConnectionList
                    {
                        AllowNew = true,
                        AllowEdit = true,
                        AllowRemove = true,
                        RaiseListChangedEvents = true
                    };

                    // Setup BindSoure for binding to DataGridView.
                    connectionSource = new BindingSource { DataSource = connectionList };

                    // Setup DataGridView and bind BindSource.
                    ConnectionListView.AutoGenerateColumns = false;
                    ConnectionListView.ColumnHeadersVisible = true;
                    ConnectionListView.DataSource = connectionSource;
                }

                // Don't select a sell by defualt on start.
                ConnectionListView.ClearSelection();

                // Clear connections list if settings is set.
                if (ClearConnsOnStartCheckBox.Checked)
                    ClearConnList();

                // Init caputre worker.
                CaptureWorker_Start();

                // Start packet batching timer.
                packetTimer.Start();

                // Start displayTimer.
                displayTimer.Start();

                // Start timeoutTimer if timout is set by user.
                if (TimeoutCheckBox.Checked)
                    timeoutTimer.Start();

                // Toggle relevant controls.
                // Disable Init Page elements.
                RefreshInterfacesButton.Enabled = false;
                StartCaptureButton.Enabled = false;
                InterfaceLabel.Enabled = false;
                InterfaceDropDownList.Enabled = false;
                ClearConnsOnStartCheckBox.Enabled = false;
                RemoveLocalConnectionsCheckBox.Enabled = false;
                ForceRawCheckBox.Enabled = false;
                CaptureFilter.Enabled = false;
                CaptureFilterLabel.Enabled = false;
                ClearCaptureFilterButton.Enabled = false;

                // Enable Connection page elements.
                ConnectionGroup.Enabled = true;

                // Allow user to stop background worker.
                StopCaptureButton.Enabled = true;

                // Switch over to connections veiw after capture start.
                TabControl.SelectTab(ConnectionPage);
            }
            else
            {
                MessageBox.Show("Error: you must select an interface!", Program.Name);
            }
        }

        // Starts the whole pcap process after user presses button.
        private void CaptureStartButton_Click(object sender, EventArgs e)
        {
            StartCapture();
        }

        private void StopCapture()
        {
            // Don't allow user to stop background worker untill a new one has been started.
            StopCaptureButton.Enabled = false;

            // Stop Capture.
            if (captureWorker != default(BackgroundWorker) && captureWorker.IsBusy)
            {
                captureWorker.CancelAsync();
                _captureWorkerDone.WaitOne();
            }

            // Do ShimDotNet cleanup.
            captureEngine.stopCapture();

            // Toggle relavant controls.
            // Disable Connection page elements.
            ConnectionGroup.Enabled = false;

            // Enable Init Page elements.
            RefreshInterfacesButton.Enabled = true;
            StartCaptureButton.Enabled = true;
            InterfaceLabel.Enabled = true;
            InterfaceDropDownList.Enabled = true;
            ClearConnsOnStartCheckBox.Enabled = true;
            RemoveLocalConnectionsCheckBox.Enabled = true;
            ForceRawCheckBox.Enabled = true;

            if (!ForceRawCheckBox.Checked)
            {
                CaptureFilter.Enabled = true;
                CaptureFilterLabel.Enabled = true;
                ClearCaptureFilterButton.Enabled = true;
            }

            // Stop timers.
            packetTimer.Stop();
            timeoutTimer.Stop();
            displayTimer.Stop();
        }

        // Kill background thread doing pcap processing when user presses button.
        private void CaptureStopButton_Click(object sender, EventArgs e)
        {
            StopCapture();
        }

        // Refreshes interface list / info.
        private void RefreshInterfacesButton_Click(object sender, EventArgs e)
        {
            BuildDeviceList();
        }

        // Removes any user filter settings.
        private void ClearFiltersButton_Click(object sender, EventArgs e)
        {
            // Remove any text in filter text boxes.
            CaptureFilter.Clear();
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
            // Close the form.
            this.Close();
        }

        // Open settings file.
        private void SettingsMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Program.ConfigFileName);
        }

        // Show about page (well really a msgbox becuase I don't want to waste a form on an about page).
        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            // Try to open the readme file for documentation information.
            try
            {
                System.Diagnostics.Process.Start(Program.ReadmeFileName);
            }
            catch(System.ComponentModel.Win32Exception)
            {
                // If it fails, open the github repo.
                try
                {
                    System.Diagnostics.Process.Start(Program.GithubWEB_RepoURL);
                }
                catch(System.ComponentModel.WarningException)
                {
                    MessageBox.Show("Error: neither the readme file, nor the github webpage, could be opened!", Program.Name);
                }
            }
        }

        // Disable capture filter group if force raw is checked (and undo if unchecked).
        private void ForceRawCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ForceRawCheckBox.Checked)
            {
                CaptureFilter.Enabled = false;
                CaptureFilterLabel.Enabled = false;
                ClearCaptureFilterButton.Enabled = false;
            }
            else
            {
                CaptureFilter.Enabled = true;
                CaptureFilterLabel.Enabled = true;
                ClearCaptureFilterButton.Enabled = true;
            }
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
        private void TimeLimit_TextChanged(object sender, EventArgs e)
        {
            // Make sure we have a valid timeout value.
            if (TimeLimit.Text.Trim().Length > 0)
            {
                if (Filter.nonNumericRegex.Match(TimeLimit.Text.Trim()).Length > 0)
                {
                    MessageBox.Show("Error: invalid timeout limit!", Program.Name);
                    TimeLimit.Text = timeoutInactivityLimit.ToString();

                    return;
                }

                if (!Int32.TryParse(TimeLimit.Text.Trim(), out timeoutInactivityLimit))
                    MessageBox.Show("Error: invalid timeout limit!", Program.Name);
            }
        }

        // Show dropdown menu on mouse hover.
        private void CopyComponentMenuItem_MouseHover(object sender, EventArgs e)
        {
            CopyComponentMenu.ShowDropDown();
        }

        // Show libpcap and npcap version info.
        private void LibpcapVersionInfoMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(captureEngine.getLibVersion(), Program.Name);
        }

        // Check if there is a newer release by comparing embeded release tag to latest release tag.
        private void CheckForUpdatesMenuItem_Click(object sender, EventArgs e)
        {
            // Start update check background thread.
            UpdateWorker_Start();
        }

        // 
        // Titlebar related methods.
        // 

        private FormWindowState _lastSeenState = FormWindowState.Normal;

        // Anytime the window size is changed, we need to make sure the right maximize button is visible.
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            // Check if a change is needed, and then apply it.
            if (WindowState != _lastSeenState && WindowState != FormWindowState.Minimized)
            {
                // Hide maximize button and show unmaximize button.
                if (this.WindowState == FormWindowState.Maximized)
                {
                    UnmaximizeButton.Enabled = true;
                    UnmaximizeButton.Visible = true;
                    MaximizeButton.Visible = false;
                    MaximizeButton.Enabled = false;
                }
                // Hide unmaximize button and show maximize button.
                else
                {
                    MaximizeButton.Enabled = true;
                    MaximizeButton.Visible = true;
                    UnmaximizeButton.Visible = false;
                    UnmaximizeButton.Enabled = false;
                }

                // Flag current state.
                _lastSeenState = WindowState;
            }
        }

        // Maximize the form / window.
        private void MaximizeForm()
        {
            // Set window state to mzximized (triggers maximize event).
            this.WindowState = FormWindowState.Maximized;
        }

        // Minimize form / window.
        private void MinimizeForm()
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // Unmaximize the form / window.
        private void UnmaximizeForm()
        {
            // Set window state to normal (triggers unmaximize event).
            this.WindowState = FormWindowState.Normal;
        }

        // Exit program on button click.
        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Maximize window on button click.
        private void MaximizeButton_Click(object sender, EventArgs e)
        {
            MaximizeForm();
        }

        // Unmaximize window on button click.
        private void UnmaximizeButton_Click(object sender, EventArgs e)
        {
            UnmaximizeForm();
        }

        // Minimize window on button click.
        private void MinimizeButton_Click(object sender, EventArgs e)
        {
            MinimizeForm();
        }

        // Send drag form event on label mouse down.
        private void TitleBarLebel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        // Maximize form on label double click.
        private void TitleBarLebel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (WindowState == FormWindowState.Normal)
                    MaximizeForm();
                else if (WindowState == FormWindowState.Maximized)
                    UnmaximizeForm();
            }
        }

        // Combat flickering when changing tab due to tab background color differences.
        private void TabControl_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == ConnectionPage)
            {
                ConnectionListView.Visible = false;
                ConnectionPage.Invalidate();
            }
            else
                ConnectionListView.Visible = true;
        }
    }
}
