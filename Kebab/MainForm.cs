
using System;
using System.Reflection;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Kebab
{
    public partial class MainForm : Form
    {
        // Name of program for repeated use.
        private static string programName = "Kebab";
        // Breif description of the program.
        private static string aboutPage = "Written in C#, " + programName + " is published for free under the terms of the MIT opensource license.\n" +
                                          "\n" +
                                          programName + " uses PcapDotNet, which is published for free under a custom opensource license.\n" +
                                          "\n" +
                                          "Program and legal documentation is included in the *Readme* and *License* files.\n" +
                                          "\n" +
                                          "Further documentation and source code can be found on the project's github page.\n" +
                                          "\n" +
                                          "Project Github page URL: https://github.com/anicca048/Kebab\n" +
                                          "PcapDotNet Github page URL: https://github.com/PcapDotNet/Pcap.Net";

        // Interface drop down list data source.
        private BindingList<string> deviceList;

        // Grid view data source (connection list).
        private BindingSource connectionSource;
        private ConnectionList connectionList;

        // Connection packet queue and lock for background workers (so as not to overload the UI loop.)
        private Queue<L4Packet> pendingPackets = new Queue<L4Packet>();
        private readonly object pendingPacketsLock = new object();

        // Packet capture session.
        private CaptureSession captureSession;

        // Background worker for processing packets.
        private BackgroundWorker _captureWorker;

        // Timers for updating the connection stats.
        System.Windows.Forms.Timer packetTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer timeoutTimer = new System.Windows.Forms.Timer();

        // Minimal wait time between packet batch processing in milliseconds.
        private const int packetBatchInterval = 10;
        // Batch packet number limit.
        private const int packetBatchSize = 500;
        // Minimal wait time between inactive entry removal operations in milliseconds.
        private const int timeoutInterval = 500;
        // Inactive entry time limit in seconds.
        private const int timeoutLimit = 10;

        // Indicates that the form closing event has been called and that the worker completion method needs to close the form.
        private bool formClosePending = false;

        // 
        // Main form directly related methods.
        // 

        public MainForm()
        {
            // Combat form flickering and resize drawing glitching.
            this.DoubleBuffered = true;

            // Sets up form components / gui elements.
            InitializeComponent();

            // Add timer event handlers.
            packetTimer.Tick += new EventHandler(UpdateConnList);
            timeoutTimer.Tick += new EventHandler(TimeoutConnList);

            // Set timer intervals.
            packetTimer.Interval = packetBatchInterval;
            timeoutTimer.Interval = timeoutInterval;
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

            // Attempt pcap and background worker init.
            try
            {
                // Create new capture session object (will do some basic pcap initialization).
                captureSession = new CaptureSession();
                captureSession.SetupDeviceList();
            }
            catch (CaptureSessionException ex)
            {
                // Display CaptureSession initialization error and exit program.
                MessageBox.Show(ex.Message, programName);
                System.Environment.Exit(1);
            }

            // Create new device / interface list.
            deviceList = new BindingList<string>(captureSession.DeviceDisplayList);
            // Add an invalid entry ontop to force user to select an interface instead of just going with the first one.
            deviceList.Insert(0, "Select Interface");
            // BInd device list to drop down menu.
            InterfaceDropDownList.DataSource = deviceList;

            // Enable form after init is done.
            this.Enabled = true;
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

            // Otherwise let the form close naturally.
            base.OnFormClosing(e);
        }

        // 
        // Background worker related methods
        // 

        // Last background worker does form cleanup.
        private void DoBWCleanup(object sender, RunWorkerCompletedEventArgs e)
        {
            // Check if user is waiting for form to close.
            if (formClosePending)
                this.Close();

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

            try
            {
                // Start packet capture loop.
                while (!thisWorker.CancellationPending)
                {
                    L4Packet pkt;

                    // Process a packet / connection, and add it to queue.
                    if (captureSession.CapturePacket(out pkt) != -1)
                    {
                        // Obtain lock on queue and then add connection / packet.
                        lock (pendingPacketsLock)
                        {
                            pendingPackets.Enqueue(pkt);
                        }
                    }
                }
            }
            catch (CaptureSessionException ex)
            {
                MessageBox.Show(ex.Message, programName);
            }
        }

        // 
        // Display filter related methods.
        // 

        // Checks if display filters match a connection.
        private bool CheckDisplayFilter(object DataSource)
        {
            // Convert dataGridViewRow data source to actual type.
            Connection connection = ((Connection)(DataSource));

            // Ceck if we have a display filter to use.
            bool IPFilter = (IPDisplayFilter.Text.Trim().Length > 0);
            bool PortFilter = (PortDisplayFilter.Text.Trim().Length > 0);

            if (IPFilter && PortFilter)
            {
                if (((connection.Source.ToString() == IPDisplayFilter.Text.Trim()) ||
                     (connection.Destination.ToString() == IPDisplayFilter.Text.Trim())) &&
                    ((connection.SrcPort.ToString() == PortDisplayFilter.Text.Trim()) ||
                     (connection.DstPort.ToString() == PortDisplayFilter.Text.Trim())))
                    return true;
                else
                    return false;
            }
            else if (IPFilter)
            {
                if ((connection.Source.ToString() == IPDisplayFilter.Text.Trim()) ||
                    (connection.Destination.ToString() == IPDisplayFilter.Text.Trim()))
                    return true;
                else
                    return false;
            }
            else if (PortFilter)
            {
                if ((connection.SrcPort.ToString() == PortDisplayFilter.Text.Trim()) ||
                    (connection.DstPort.ToString() == PortDisplayFilter.Text.Trim()))
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
        private void EnforceDisplayFilter()
        {
            // Mark displayfilter flag true.
            if (!_displayFilterSet)
                _displayFilterSet = true;

            // Loop through connectionList and hide any connections that don't match filter (and unhide ones that do).
            foreach (DataGridViewRow row in ConnectionGridView.Rows)
            {
                if (!CheckDisplayFilter(row.DataBoundItem))
                {
                    row.DefaultCellStyle.ForeColor = System.Drawing.Color.Gray;
                    row.DefaultCellStyle.Font = ConnectionGridView.DefaultCellStyle.Font;
                }
                else
                {
                    row.DefaultCellStyle.ForeColor = Color.Red;
                    row.DefaultCellStyle.Font = new Font(ConnectionGridView.DefaultCellStyle.Font.Name,
                                                         ConnectionGridView.DefaultCellStyle.Font.Size,
                                                         FontStyle.Bold);
                }
            }

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
            // Mark displayfilter flag false.
            if (_displayFilterSet)
                _displayFilterSet = false;

            // Remove displayfilter font changes.
            foreach (DataGridViewRow row in ConnectionGridView.Rows)
            {
                row.DefaultCellStyle.ForeColor = ConnectionGridView.DefaultCellStyle.ForeColor;
                row.DefaultCellStyle.Font = ConnectionGridView.DefaultCellStyle.Font;
            }

            // Show displayfilter changes.
            ConnectionGridView.Update();
        }

        // Force repaint for display filter adhearence.
        private void IPDisplayFilter_TextChanged(object sender, EventArgs e)
        {
            if (!((IPDisplayFilter.Text.Trim().Length > 0) || (PortDisplayFilter.Text.Trim().Length > 0)))
            {
                if (_displayFilterSet)
                    RemoveDisplayFilter();

                return;
            }

            EnforceDisplayFilter();
        }

        // Force repaint for display filter adhearence.
        private void PortDisplayFilter_TextChanged(object sender, EventArgs e)
        {
            if (!((IPDisplayFilter.Text.Trim().Length > 0) || (PortDisplayFilter.Text.Trim().Length > 0)))
            {
                if (_displayFilterSet)
                    RemoveDisplayFilter();

                return;
            }

            EnforceDisplayFilter();
        }

        // Force repaint for display filter adhearence.
        private void ConnectionGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (_displayFilterSet)
            {
                EnforceDisplayFilter();
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
                L4Packet pkt;

                // Grab first connection in the queue for processing.
                lock (pendingPacketsLock)
                {
                    pkt = pendingPackets.Dequeue();
                }

                // Make sure we don't add a new connection when a matching one already exists and was updated.
                bool packetMatched = false;

                // Loop through connections list and see if we have a new connection.
                foreach (Connection tmpConn in connectionList)
                {
                    // Find matching connections(regardless of source and dest orientation).
                    if (tmpConn.PacketMatch(pkt) != Match.NO_MATCH)
                    {
                        tmpConn.PacketCount++;
                        tmpConn.DataSent += pkt.PayloadSize;
                        tmpConn.TimeStamp = DateTime.Now;

                        // Check if direction needs to be updated to both ways.
                        if (tmpConn.State.Direction != TransmissionDirection.TWO_WAY)
                        {
                            if (tmpConn.PacketMatch(pkt) == Match.REV_MATCH)
                                tmpConn.State.Direction = TransmissionDirection.TWO_WAY;
                            else if (tmpConn.State.Direction != pkt.Direction)
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
                    Connection conn = new Connection(pkt);

                    // Need to do a check to make sure that local and loobpack connections get priority for the Source address.
                    if (conn.Destination.AddressIsLocal() && !conn.Source.AddressIsLocal())
                    {
                        // Swap source and destination ip addresses and ports.
                        conn.Destination.Address = pkt.Source;
                        conn.DstPort = pkt.SrcPort;
                        conn.Source.Address = pkt.Destination;
                        conn.SrcPort = pkt.DstPort;

                        // Flip direction.
                        conn.State.Direction = TransmissionDirection.REV_ONE_WAY;
                    }

                    // Asing the connection a number.
                    conn.Number = ConnectionList.GetNewConnNumber(connectionList as IList<Connection>);
                    // And add it to the list.
                    connectionList.Add(conn);

                    // Finaly, we have one less to process.
                    packetsToProcess--;
                }
            }
        }

        // Runs connection timeout removal operations on Connection List.
        private void TimeoutConnList(object sender, EventArgs e)
        {
            // Loop through connections list and see if we have any inactive connections to remove.
            while (true)
            {
                // Flag to see if we removed any connections.
                bool removedConnection = false;

                // Loop through connectionList and remove an inactive entry if we find one.
                foreach (Connection loopConn in connectionList)
                {
                    // Find Inactive connetion based on timestamp.
                    if ((DateTime.Now - loopConn.TimeStamp).TotalSeconds >= 10)
                    {
                        // Remove inactive entry and mark flag so that we will do another inactive connection check.
                        connectionList.Remove(loopConn);
                        removedConnection = true;

                        // Must break beause loop limit was changed.
                        break;
                    }
                }

                // Stop checking for inactive connections if all have been checked.
                if (!removedConnection)
                    break;
            }
        }

        // Allows thread safe clearing of binded data list from background worker.
        private void ClearConnList()
        {
            // Clear the binded list of entries.
            ConnectionGridView.ClearSelection();
            connectionList.Clear();
            
            // Make control redraw.
            ConnectionGridView.Update();
        }

        // 
        // UI and control event related members.
        // 

        // Disables certian bulk UI elements on when a capture is started.
        private void ChangeOnStartCapture(object sender, EventArgs e)
        {
            // Disable Init Page elements.
            RefreshInterfacesButton.Enabled = false;
            CaptureStartButton.Enabled = false;
            InterfaceLabel.Enabled = false;
            InterfaceDropDownList.Enabled = false;
            CaptureFilterGroupBox.Enabled = false;

            // Enable Connection page elements.
            ClearDisplayFilter();
            DisplayFilterGroupBox.Enabled = true;
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

        // Starts the whole pcap process after user presses button.
        private void CaptureStartButton_Click(object sender, EventArgs e)
        {
            if (InterfaceDropDownList.SelectedIndex > 0)
            {
                // Change UI elements to reflect new capture state.
                ChangeOnStartCapture(sender, e);

                try
                {
                    // Ensure at least one protocol (tcp or udp) is selected in Filter Options.
                    if (!TCPCheckBox.Checked && !UDPCheckBox.Checked)
                        throw new CaptureSessionException("Error: you must select at least one protocol!");

                    // Create new SessionFilter object based on user filter settings to pass to SetupCapture().
                    SessionFilter simpleFilter = new SessionFilter { TCP = TCPCheckBox.Checked, UDP = UDPCheckBox.Checked };
                    
                    // Add user IP and Port filter settings to SessionFilter.
                    if (SourceIPFilter.Text.Trim().Length > 0)
                        simpleFilter.SourceIP = SourceIPFilter.Text.Trim();
                    if (DestinationIPFilter.Text.Trim().Length > 0)
                        simpleFilter.DestinationIP = DestinationIPFilter.Text.Trim();
                    if (SourcePortFilter.Text.Trim().Length > 0)
                        simpleFilter.SourcePort = SourcePortFilter.Text.Trim();
                    if (DestinationPortFilter.Text.Trim().Length > 0)
                        simpleFilter.DestinationPort = DestinationPortFilter.Text.Trim();

                    // Check and see if user opted to use complexFilter (directly using libpcap filter string).
                    string complexFilterStr = String.Empty;

                    if (complexFilter.Text.Trim().Length > 0)
                        complexFilterStr = complexFilter.Text.Trim();

                    // Open pcap live session (offset index by -1 because of invalid first entry in drop down list).
                    captureSession.SetupCapture((InterfaceDropDownList.SelectedIndex - 1), simpleFilter, complexFilterStr);
                }
                catch (CaptureSessionException ex)
                {
                    // If an error happens here we want the user to be able to try another interface.
                    MessageBox.Show(ex.Message, programName);
                    ChangeOnStopCapture(sender, e);
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

                // Clear connections list.
                ClearConnList();

                // Sets up all background workers.
                DoBWSetup();

                // Start packet batching timer.
                packetTimer.Start();

                // Allow user to stop background worker.
                CaptureStopButton.Enabled = true;

                // Switch over to connections veiw after capture start.
                TabControl.SelectTab(ConnectionPage);
            }
            else
            {
                MessageBox.Show("Error: you must select an interface!", programName);
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
        }

        // Refreshes interface list / info.
        private void RefreshInterfacesButton_Click(object sender, EventArgs e)
        {
            // Attempt to regrab interfaces.
            try
            {
                // Refresh interfaces.
                captureSession.SetupDeviceList();
            }
            catch (CaptureSessionException ex)
            {
                // Display CaptureSession initialization error and exit program.
                MessageBox.Show(ex.Message, programName);
                System.Environment.Exit(1);
            }

            // Clear interface list.
            deviceList.Clear();

            // Add an invalid entry ontop to force user to select an interface instead of just going with the first one.
            deviceList.Add("Select Interface");

            // Add refreshed device list devices.
            foreach (string device in captureSession.DeviceDisplayList)
                deviceList.Add(device);
        }

        // Removes any user filter settings.
        private void ClearFiltersButton_Click(object sender, EventArgs e)
        {
            // Reset protocol check boxes to default.
            TCPCheckBox.Checked = true;
            UDPCheckBox.Checked = true;

            // Remove any text in filter text boxes.
            SourceIPFilter.Clear();
            SourcePortFilter.Clear();
            DestinationIPFilter.Clear();
            DestinationPortFilter.Clear();
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
            MessageBox.Show(aboutPage, programName);
        }

        // Decide if filter timer needs to be started or stoped.
        private void TimeoutCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.TimeoutCheckBox.Checked)
                this.timeoutTimer.Start();
            else
                this.timeoutTimer.Stop();
        }

        // Copy all selected rows with right click.
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((connectionList == null) || (connectionList.Count == 0) ||
                    (ConnectionGridView == null) || (ConnectionGridView.SelectedRows.Count == 0))
                return;

            DataObject dataObj = ConnectionGridView.GetClipboardContent();

            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        // Show dropdown menu on mouse hover.
        private void copyComponentToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            copyComponentToolStripMenuItem.ShowDropDown();
        }

        // Copy local address of connection for all selected connections.
        private void localAddressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((connectionList == null) || (connectionList.Count == 0) ||
                (ConnectionGridView == null) || (ConnectionGridView.SelectedRows.Count == 0))
                return;

            string copyString = "";

            foreach (DataGridViewRow row in ConnectionGridView.SelectedRows)
                copyString += (row.Cells[2].Value.ToString() + "\n");

            if (copyString != "")
                Clipboard.SetText(copyString);
        }

        // Copy local port of connection for all selected connections.
        private void localPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((connectionList == null) || (connectionList.Count == 0) ||
                (ConnectionGridView == null) || (ConnectionGridView.SelectedRows.Count == 0))
                return;

            string copyString = "";

            foreach (DataGridViewRow row in ConnectionGridView.SelectedRows)
                copyString += (row.Cells[3].Value.ToString() + "\n");

            if (copyString != "")
                Clipboard.SetText(copyString);
        }

        // Copy local address and port pair of connection for all selected connections.
        private void localAddressPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((connectionList == null) || (connectionList.Count == 0) ||
                (ConnectionGridView == null) || (ConnectionGridView.SelectedRows.Count == 0))
                return;

            string copyString = "";

            foreach (DataGridViewRow row in ConnectionGridView.SelectedRows)
                copyString += (row.Cells[2].Value.ToString() + ":" + row.Cells[3].Value.ToString() + "\n");

            if (copyString != "")
                Clipboard.SetText(copyString);
        }

        // Copy remote address of connection for all selected connections.
        private void remoteAddressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((connectionList == null) || (connectionList.Count == 0) ||
                (ConnectionGridView == null) || (ConnectionGridView.SelectedRows.Count == 0))
                return;

            string copyString = "";

            foreach (DataGridViewRow row in ConnectionGridView.SelectedRows)
                copyString += (row.Cells[5].Value.ToString() + "\n");

            if (copyString != "")
                Clipboard.SetText(copyString);
        }

        // Copy remote pair of connection for all selected connections.
        private void remotePortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((connectionList == null) || (connectionList.Count == 0) ||
                (ConnectionGridView == null) || (ConnectionGridView.SelectedRows.Count == 0))
                return;

            string copyString = "";

            foreach (DataGridViewRow row in ConnectionGridView.SelectedRows)
                copyString += (row.Cells[6].Value.ToString() + "\n");

            if (copyString != "")
                Clipboard.SetText(copyString);
        }

        // Copy remote address and port pair of connection for all selected connections.
        private void remoteAddressPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((connectionList == null) || (connectionList.Count == 0) ||
                (ConnectionGridView == null) || (ConnectionGridView.SelectedRows.Count == 0))
                return;

            string copyString = "";

            foreach (DataGridViewRow row in ConnectionGridView.SelectedRows)
                copyString += (row.Cells[5].Value.ToString() + ":" + row.Cells[6].Value.ToString() + "\n");

            if (copyString != "")
                Clipboard.SetText(copyString);
        }
    }
}
