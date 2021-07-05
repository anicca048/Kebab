
namespace Kebab
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ToolBar = new System.Windows.Forms.MenuStrip();
            this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.PreferencesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.CheckForUpdatesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.LibpcapVersionInfoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.AboutMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ConnectionPage = new System.Windows.Forms.TabPage();
            this.DisplayFilterGroupBox = new System.Windows.Forms.GroupBox();
            this.TimeLimit = new System.Windows.Forms.TextBox();
            this.ClearDisplayFilterButton = new System.Windows.Forms.Button();
            this.DisplayFilterStr = new System.Windows.Forms.TextBox();
            this.DisplayFilterStringLabel = new System.Windows.Forms.Label();
            this.TimeoutCheckBox = new System.Windows.Forms.CheckBox();
            this.ClearConnectionsButton = new System.Windows.Forms.Button();
            this.ConnectionGridView = new System.Windows.Forms.DataGridView();
            this.ConnectionContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CopyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyComponentMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.LocalAddressMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LocalPortMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LocalAddressPortMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.RemoteAddressMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RemotePortMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RemoteAddressPortMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ISOMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ASNOrganizationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.AllRemoteHostInformationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CapturePage = new System.Windows.Forms.TabPage();
            this.CaptureOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.ForceRawCheckBox = new System.Windows.Forms.CheckBox();
            this.ClearConnsOnStartCheckBox = new System.Windows.Forms.CheckBox();
            this.RefreshInterfacesButton = new System.Windows.Forms.Button();
            this.InterfaceLabel = new System.Windows.Forms.Label();
            this.RemoveLocalConnectionsCheckBox = new System.Windows.Forms.CheckBox();
            this.InterfaceDropDownList = new System.Windows.Forms.ComboBox();
            this.CaptureStopButton = new System.Windows.Forms.Button();
            this.CaptureStartButton = new System.Windows.Forms.Button();
            this.CaptureFilterGroupBox = new System.Windows.Forms.GroupBox();
            this.AnyPortFilter = new System.Windows.Forms.TextBox();
            this.AnyPortLabel = new System.Windows.Forms.Label();
            this.AnyIPFilter = new System.Windows.Forms.TextBox();
            this.AnyIPLabel = new System.Windows.Forms.Label();
            this.FilterStringLabel = new System.Windows.Forms.Label();
            this.CaptureFilterStr = new System.Windows.Forms.TextBox();
            this.ClearFiltersButton = new System.Windows.Forms.Button();
            this.ProtocolLabel = new System.Windows.Forms.Label();
            this.UDPCheckBox = new System.Windows.Forms.CheckBox();
            this.TCPCheckBox = new System.Windows.Forms.CheckBox();
            this.SourceIPLabel = new System.Windows.Forms.Label();
            this.DestinationIPLabel = new System.Windows.Forms.Label();
            this.SourceIPFilter = new System.Windows.Forms.TextBox();
            this.DestinationPortLabel = new System.Windows.Forms.Label();
            this.DestinationIPFilter = new System.Windows.Forms.TextBox();
            this.SourcePortLabel = new System.Windows.Forms.Label();
            this.SourcePortFilter = new System.Windows.Forms.TextBox();
            this.DestinationPortFilter = new System.Windows.Forms.TextBox();
            this.TabControl = new System.Windows.Forms.TabControl();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.TitleBarLebel = new System.Windows.Forms.Label();
            this.ExitButton = new System.Windows.Forms.Button();
            this.MaximizeButton = new System.Windows.Forms.Button();
            this.MinimizeButton = new System.Windows.Forms.Button();
            this.UnmaximizeButton = new System.Windows.Forms.Button();
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SrcHost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SrcPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DstHost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DstPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PacketCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DstGeo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DstASNOrg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ToolBar.SuspendLayout();
            this.ConnectionPage.SuspendLayout();
            this.DisplayFilterGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConnectionGridView)).BeginInit();
            this.ConnectionContextMenu.SuspendLayout();
            this.CapturePage.SuspendLayout();
            this.CaptureOptionsGroupBox.SuspendLayout();
            this.CaptureFilterGroupBox.SuspendLayout();
            this.TabControl.SuspendLayout();
            this.MainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolBar
            // 
            this.ToolBar.BackColor = System.Drawing.Color.Transparent;
            this.ToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.EditMenu,
            this.HelpMenu});
            this.ToolBar.Location = new System.Drawing.Point(0, 0);
            this.ToolBar.Name = "ToolBar";
            this.ToolBar.Size = new System.Drawing.Size(1400, 24);
            this.ToolBar.TabIndex = 1;
            // 
            // FileMenu
            // 
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveMenuItem,
            this.SaveAsMenuItem,
            this.MenuSeparator3,
            this.ExitMenuItem});
            this.FileMenu.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FileMenu.Name = "FileMenu";
            this.FileMenu.Size = new System.Drawing.Size(37, 20);
            this.FileMenu.Text = "File";
            // 
            // SaveMenuItem
            // 
            this.SaveMenuItem.Enabled = false;
            this.SaveMenuItem.Name = "SaveMenuItem";
            this.SaveMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.SaveMenuItem.Size = new System.Drawing.Size(186, 22);
            this.SaveMenuItem.Text = "Save";
            this.SaveMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // SaveAsMenuItem
            // 
            this.SaveAsMenuItem.Name = "SaveAsMenuItem";
            this.SaveAsMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.SaveAsMenuItem.Size = new System.Drawing.Size(186, 22);
            this.SaveAsMenuItem.Text = "Save As...";
            this.SaveAsMenuItem.Click += new System.EventHandler(this.SaveAsToolStripMenuItem_Click);
            // 
            // MenuSeparator3
            // 
            this.MenuSeparator3.BackColor = System.Drawing.SystemColors.Control;
            this.MenuSeparator3.Name = "MenuSeparator3";
            this.MenuSeparator3.Size = new System.Drawing.Size(183, 6);
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Name = "ExitMenuItem";
            this.ExitMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.ExitMenuItem.Size = new System.Drawing.Size(186, 22);
            this.ExitMenuItem.Text = "Exit";
            this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // EditMenu
            // 
            this.EditMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PreferencesMenuItem});
            this.EditMenu.ForeColor = System.Drawing.SystemColors.ControlText;
            this.EditMenu.Name = "EditMenu";
            this.EditMenu.Size = new System.Drawing.Size(39, 20);
            this.EditMenu.Text = "Edit";
            // 
            // PreferencesMenuItem
            // 
            this.PreferencesMenuItem.Name = "PreferencesMenuItem";
            this.PreferencesMenuItem.Size = new System.Drawing.Size(135, 22);
            this.PreferencesMenuItem.Text = "Preferences";
            this.PreferencesMenuItem.Click += new System.EventHandler(this.PreferencesToolStripMenuItem_Click);
            // 
            // HelpMenu
            // 
            this.HelpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CheckForUpdatesMenuItem,
            this.MenuSeparator5,
            this.LibpcapVersionInfoMenuItem,
            this.MenuSeparator4,
            this.AboutMenu});
            this.HelpMenu.ForeColor = System.Drawing.SystemColors.ControlText;
            this.HelpMenu.Name = "HelpMenu";
            this.HelpMenu.Size = new System.Drawing.Size(44, 20);
            this.HelpMenu.Text = "Help";
            // 
            // CheckForUpdatesMenuItem
            // 
            this.CheckForUpdatesMenuItem.Name = "CheckForUpdatesMenuItem";
            this.CheckForUpdatesMenuItem.Size = new System.Drawing.Size(181, 22);
            this.CheckForUpdatesMenuItem.Text = "Check for Updates";
            this.CheckForUpdatesMenuItem.Click += new System.EventHandler(this.CheckForUpdatesToolStripMenuItem_Click);
            // 
            // MenuSeparator5
            // 
            this.MenuSeparator5.Name = "MenuSeparator5";
            this.MenuSeparator5.Size = new System.Drawing.Size(178, 6);
            // 
            // LibpcapVersionInfoMenuItem
            // 
            this.LibpcapVersionInfoMenuItem.Name = "LibpcapVersionInfoMenuItem";
            this.LibpcapVersionInfoMenuItem.Size = new System.Drawing.Size(181, 22);
            this.LibpcapVersionInfoMenuItem.Text = "Libpcap Version Info";
            this.LibpcapVersionInfoMenuItem.Click += new System.EventHandler(this.LibpcapVersionInfoToolStripMenuItem_Click);
            // 
            // MenuSeparator4
            // 
            this.MenuSeparator4.Name = "MenuSeparator4";
            this.MenuSeparator4.Size = new System.Drawing.Size(178, 6);
            // 
            // AboutMenu
            // 
            this.AboutMenu.Name = "AboutMenu";
            this.AboutMenu.Size = new System.Drawing.Size(181, 22);
            this.AboutMenu.Text = "About";
            this.AboutMenu.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // ConnectionPage
            // 
            this.ConnectionPage.Controls.Add(this.DisplayFilterGroupBox);
            this.ConnectionPage.Controls.Add(this.ConnectionGridView);
            this.ConnectionPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectionPage.Location = new System.Drawing.Point(4, 29);
            this.ConnectionPage.Name = "ConnectionPage";
            this.ConnectionPage.Padding = new System.Windows.Forms.Padding(3);
            this.ConnectionPage.Size = new System.Drawing.Size(1392, 526);
            this.ConnectionPage.TabIndex = 1;
            this.ConnectionPage.Text = "Connections";
            this.ConnectionPage.UseVisualStyleBackColor = true;
            // 
            // DisplayFilterGroupBox
            // 
            this.DisplayFilterGroupBox.Controls.Add(this.TimeLimit);
            this.DisplayFilterGroupBox.Controls.Add(this.ClearDisplayFilterButton);
            this.DisplayFilterGroupBox.Controls.Add(this.DisplayFilterStr);
            this.DisplayFilterGroupBox.Controls.Add(this.DisplayFilterStringLabel);
            this.DisplayFilterGroupBox.Controls.Add(this.TimeoutCheckBox);
            this.DisplayFilterGroupBox.Controls.Add(this.ClearConnectionsButton);
            this.DisplayFilterGroupBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DisplayFilterGroupBox.Enabled = false;
            this.DisplayFilterGroupBox.Location = new System.Drawing.Point(3, 463);
            this.DisplayFilterGroupBox.Name = "DisplayFilterGroupBox";
            this.DisplayFilterGroupBox.Size = new System.Drawing.Size(1386, 60);
            this.DisplayFilterGroupBox.TabIndex = 2;
            this.DisplayFilterGroupBox.TabStop = false;
            this.DisplayFilterGroupBox.Text = "Display Filter";
            // 
            // TimeLimit
            // 
            this.TimeLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TimeLimit.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimeLimit.Location = new System.Drawing.Point(1169, 28);
            this.TimeLimit.MaxLength = 3;
            this.TimeLimit.Name = "TimeLimit";
            this.TimeLimit.Size = new System.Drawing.Size(53, 26);
            this.TimeLimit.TabIndex = 5;
            this.TimeLimit.Text = "10";
            this.TimeLimit.TextChanged += new System.EventHandler(this.TimeLimit_TextChanged);
            // 
            // ClearDisplayFilterButton
            // 
            this.ClearDisplayFilterButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearDisplayFilterButton.Location = new System.Drawing.Point(957, 27);
            this.ClearDisplayFilterButton.MinimumSize = new System.Drawing.Size(98, 27);
            this.ClearDisplayFilterButton.Name = "ClearDisplayFilterButton";
            this.ClearDisplayFilterButton.Size = new System.Drawing.Size(111, 28);
            this.ClearDisplayFilterButton.TabIndex = 3;
            this.ClearDisplayFilterButton.Text = "Clear Filter";
            this.ClearDisplayFilterButton.UseVisualStyleBackColor = true;
            this.ClearDisplayFilterButton.Click += new System.EventHandler(this.ClearDisplayFiltersButton_Click);
            // 
            // DisplayFilterStr
            // 
            this.DisplayFilterStr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DisplayFilterStr.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DisplayFilterStr.Location = new System.Drawing.Point(106, 28);
            this.DisplayFilterStr.MaxLength = 1024;
            this.DisplayFilterStr.Name = "DisplayFilterStr";
            this.DisplayFilterStr.Size = new System.Drawing.Size(845, 26);
            this.DisplayFilterStr.TabIndex = 2;
            this.DisplayFilterStr.WordWrap = false;
            this.DisplayFilterStr.TextChanged += new System.EventHandler(this.DisplayFilter_TextChanged);
            // 
            // DisplayFilterStringLabel
            // 
            this.DisplayFilterStringLabel.AutoSize = true;
            this.DisplayFilterStringLabel.Location = new System.Drawing.Point(6, 30);
            this.DisplayFilterStringLabel.Name = "DisplayFilterStringLabel";
            this.DisplayFilterStringLabel.Size = new System.Drawing.Size(94, 20);
            this.DisplayFilterStringLabel.TabIndex = 0;
            this.DisplayFilterStringLabel.Text = "Filter String:";
            // 
            // TimeoutCheckBox
            // 
            this.TimeoutCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TimeoutCheckBox.AutoSize = true;
            this.TimeoutCheckBox.Location = new System.Drawing.Point(1074, 29);
            this.TimeoutCheckBox.Name = "TimeoutCheckBox";
            this.TimeoutCheckBox.Size = new System.Drawing.Size(89, 24);
            this.TimeoutCheckBox.TabIndex = 4;
            this.TimeoutCheckBox.Text = "Timeout:";
            this.TimeoutCheckBox.UseVisualStyleBackColor = true;
            this.TimeoutCheckBox.CheckedChanged += new System.EventHandler(this.TimeoutCheckBox_CheckedChanged);
            // 
            // ClearConnectionsButton
            // 
            this.ClearConnectionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearConnectionsButton.Location = new System.Drawing.Point(1228, 27);
            this.ClearConnectionsButton.MinimumSize = new System.Drawing.Size(98, 27);
            this.ClearConnectionsButton.Name = "ClearConnectionsButton";
            this.ClearConnectionsButton.Size = new System.Drawing.Size(152, 28);
            this.ClearConnectionsButton.TabIndex = 6;
            this.ClearConnectionsButton.Text = "Clear Connections";
            this.ClearConnectionsButton.UseVisualStyleBackColor = true;
            this.ClearConnectionsButton.Click += new System.EventHandler(this.ClearConnectionsButton_Click);
            // 
            // ConnectionGridView
            // 
            this.ConnectionGridView.AllowUserToAddRows = false;
            this.ConnectionGridView.AllowUserToDeleteRows = false;
            this.ConnectionGridView.AllowUserToResizeRows = false;
            this.ConnectionGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ConnectionGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.ConnectionGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ConnectionGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.ConnectionGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.ConnectionGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(31)))), ((int)(((byte)(36)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(31)))), ((int)(((byte)(36)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ConnectionGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ConnectionGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ConnectionGridView.ColumnHeadersVisible = false;
            this.ConnectionGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Number,
            this.Type,
            this.SrcHost,
            this.SrcPort,
            this.State,
            this.DstHost,
            this.DstPort,
            this.PacketCount,
            this.DataSize,
            this.DstGeo,
            this.DstASNOrg});
            this.ConnectionGridView.ContextMenuStrip = this.ConnectionContextMenu;
            this.ConnectionGridView.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(204)))), ((int)(((byte)(132)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ConnectionGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.ConnectionGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConnectionGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.ConnectionGridView.EnableHeadersVisualStyles = false;
            this.ConnectionGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(43)))), ((int)(((byte)(46)))));
            this.ConnectionGridView.Location = new System.Drawing.Point(3, 3);
            this.ConnectionGridView.Name = "ConnectionGridView";
            this.ConnectionGridView.ReadOnly = true;
            this.ConnectionGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.ConnectionGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.ConnectionGridView.RowHeadersVisible = false;
            this.ConnectionGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ConnectionGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ConnectionGridView.ShowCellErrors = false;
            this.ConnectionGridView.ShowCellToolTips = false;
            this.ConnectionGridView.ShowEditingIcon = false;
            this.ConnectionGridView.ShowRowErrors = false;
            this.ConnectionGridView.Size = new System.Drawing.Size(1386, 520);
            this.ConnectionGridView.StandardTab = true;
            this.ConnectionGridView.TabIndex = 1;
            this.ConnectionGridView.TabStop = false;
            this.ConnectionGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ConnectionGridView_CellClick);
            this.ConnectionGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.ConnectionGridView_ColumnHeaderMouseClick);
            this.ConnectionGridView.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.ConnectionGridView_RowsAdded);
            this.ConnectionGridView.SelectionChanged += new System.EventHandler(this.ConnectionGridView_SelectionChanged);
            this.ConnectionGridView.Sorted += new System.EventHandler(this.ConnectionGridView_Sorted);
            this.ConnectionGridView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ConnectionGridView_KeyUp);
            // 
            // ConnectionContextMenu
            // 
            this.ConnectionContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CopyMenuItem,
            this.CopyComponentMenu});
            this.ConnectionContextMenu.Name = "ConnectionContextMenuStrip";
            this.ConnectionContextMenu.Size = new System.Drawing.Size(187, 48);
            // 
            // CopyMenuItem
            // 
            this.CopyMenuItem.Enabled = false;
            this.CopyMenuItem.Name = "CopyMenuItem";
            this.CopyMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.CopyMenuItem.Size = new System.Drawing.Size(186, 22);
            this.CopyMenuItem.Text = "Copy  Row(s)";
            this.CopyMenuItem.Click += new System.EventHandler(this.CopyToolStripMenuItem_Click);
            // 
            // CopyComponentMenu
            // 
            this.CopyComponentMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LocalAddressMenuItem,
            this.LocalPortMenuItem,
            this.LocalAddressPortMenuItem,
            this.MenuSeparator2,
            this.RemoteAddressMenuItem,
            this.RemotePortMenuItem,
            this.RemoteAddressPortMenuItem,
            this.MenuSeparator1,
            this.ISOMenuItem,
            this.ASNOrganizationMenuItem,
            this.MenuSeparator6,
            this.AllRemoteHostInformationMenuItem});
            this.CopyComponentMenu.Enabled = false;
            this.CopyComponentMenu.Name = "CopyComponentMenu";
            this.CopyComponentMenu.Size = new System.Drawing.Size(186, 22);
            this.CopyComponentMenu.Text = "Copy Component(s)";
            this.CopyComponentMenu.MouseHover += new System.EventHandler(this.CopyComponentToolStripMenuItem_MouseHover);
            // 
            // LocalAddressMenuItem
            // 
            this.LocalAddressMenuItem.Name = "LocalAddressMenuItem";
            this.LocalAddressMenuItem.Size = new System.Drawing.Size(300, 22);
            this.LocalAddressMenuItem.Text = "Local Address";
            this.LocalAddressMenuItem.Click += new System.EventHandler(this.LocalAddressToolStripMenuItem_Click);
            // 
            // LocalPortMenuItem
            // 
            this.LocalPortMenuItem.Name = "LocalPortMenuItem";
            this.LocalPortMenuItem.Size = new System.Drawing.Size(300, 22);
            this.LocalPortMenuItem.Text = "Local Port";
            this.LocalPortMenuItem.Click += new System.EventHandler(this.LocalPortToolStripMenuItem_Click);
            // 
            // LocalAddressPortMenuItem
            // 
            this.LocalAddressPortMenuItem.Name = "LocalAddressPortMenuItem";
            this.LocalAddressPortMenuItem.Size = new System.Drawing.Size(300, 22);
            this.LocalAddressPortMenuItem.Text = "Local Address : Local Port";
            this.LocalAddressPortMenuItem.Click += new System.EventHandler(this.LocalAddressPortToolStripMenuItem_Click);
            // 
            // MenuSeparator2
            // 
            this.MenuSeparator2.Name = "MenuSeparator2";
            this.MenuSeparator2.Size = new System.Drawing.Size(297, 6);
            // 
            // RemoteAddressMenuItem
            // 
            this.RemoteAddressMenuItem.Name = "RemoteAddressMenuItem";
            this.RemoteAddressMenuItem.Size = new System.Drawing.Size(300, 22);
            this.RemoteAddressMenuItem.Text = "Remote Address";
            this.RemoteAddressMenuItem.Click += new System.EventHandler(this.RemoteAddressToolStripMenuItem_Click);
            // 
            // RemotePortMenuItem
            // 
            this.RemotePortMenuItem.Name = "RemotePortMenuItem";
            this.RemotePortMenuItem.Size = new System.Drawing.Size(300, 22);
            this.RemotePortMenuItem.Text = "Remote Port";
            this.RemotePortMenuItem.Click += new System.EventHandler(this.RemotePortToolStripMenuItem_Click);
            // 
            // RemoteAddressPortMenuItem
            // 
            this.RemoteAddressPortMenuItem.Name = "RemoteAddressPortMenuItem";
            this.RemoteAddressPortMenuItem.Size = new System.Drawing.Size(300, 22);
            this.RemoteAddressPortMenuItem.Text = "Remote Address : Remote Port";
            this.RemoteAddressPortMenuItem.Click += new System.EventHandler(this.RemoteAddressPortToolStripMenuItem_Click);
            // 
            // MenuSeparator1
            // 
            this.MenuSeparator1.Name = "MenuSeparator1";
            this.MenuSeparator1.Size = new System.Drawing.Size(297, 6);
            // 
            // ISOMenuItem
            // 
            this.ISOMenuItem.Name = "ISOMenuItem";
            this.ISOMenuItem.Size = new System.Drawing.Size(300, 22);
            this.ISOMenuItem.Text = "ISO";
            this.ISOMenuItem.Click += new System.EventHandler(this.ISOToolStripMenuItem_Click);
            // 
            // ASNOrganizationMenuItem
            // 
            this.ASNOrganizationMenuItem.Name = "ASNOrganizationMenuItem";
            this.ASNOrganizationMenuItem.Size = new System.Drawing.Size(300, 22);
            this.ASNOrganizationMenuItem.Text = "ASN Organization";
            this.ASNOrganizationMenuItem.Click += new System.EventHandler(this.ASNOrganizationToolStripMenuItem_Click);
            // 
            // MenuSeparator6
            // 
            this.MenuSeparator6.Name = "MenuSeparator6";
            this.MenuSeparator6.Size = new System.Drawing.Size(297, 6);
            // 
            // AllRemoteHostInformationMenuItem
            // 
            this.AllRemoteHostInformationMenuItem.Name = "AllRemoteHostInformationMenuItem";
            this.AllRemoteHostInformationMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.AllRemoteHostInformationMenuItem.Size = new System.Drawing.Size(300, 22);
            this.AllRemoteHostInformationMenuItem.Text = "All Remote Host Information";
            this.AllRemoteHostInformationMenuItem.Click += new System.EventHandler(this.AllRemoteHostInformationToolStripMenuItem_Click);
            // 
            // CapturePage
            // 
            this.CapturePage.Controls.Add(this.CaptureOptionsGroupBox);
            this.CapturePage.Controls.Add(this.CaptureFilterGroupBox);
            this.CapturePage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CapturePage.Location = new System.Drawing.Point(4, 29);
            this.CapturePage.Name = "CapturePage";
            this.CapturePage.Padding = new System.Windows.Forms.Padding(3);
            this.CapturePage.Size = new System.Drawing.Size(1392, 526);
            this.CapturePage.TabIndex = 0;
            this.CapturePage.Text = "Capture";
            this.CapturePage.UseVisualStyleBackColor = true;
            // 
            // CaptureOptionsGroupBox
            // 
            this.CaptureOptionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CaptureOptionsGroupBox.Controls.Add(this.ForceRawCheckBox);
            this.CaptureOptionsGroupBox.Controls.Add(this.ClearConnsOnStartCheckBox);
            this.CaptureOptionsGroupBox.Controls.Add(this.RefreshInterfacesButton);
            this.CaptureOptionsGroupBox.Controls.Add(this.InterfaceLabel);
            this.CaptureOptionsGroupBox.Controls.Add(this.RemoveLocalConnectionsCheckBox);
            this.CaptureOptionsGroupBox.Controls.Add(this.InterfaceDropDownList);
            this.CaptureOptionsGroupBox.Controls.Add(this.CaptureStopButton);
            this.CaptureOptionsGroupBox.Controls.Add(this.CaptureStartButton);
            this.CaptureOptionsGroupBox.Location = new System.Drawing.Point(6, 6);
            this.CaptureOptionsGroupBox.Name = "CaptureOptionsGroupBox";
            this.CaptureOptionsGroupBox.Size = new System.Drawing.Size(1380, 218);
            this.CaptureOptionsGroupBox.TabIndex = 12;
            this.CaptureOptionsGroupBox.TabStop = false;
            this.CaptureOptionsGroupBox.Text = "Capture Options";
            // 
            // ForceRawCheckBox
            // 
            this.ForceRawCheckBox.AutoSize = true;
            this.ForceRawCheckBox.Enabled = false;
            this.ForceRawCheckBox.Location = new System.Drawing.Point(139, 174);
            this.ForceRawCheckBox.Name = "ForceRawCheckBox";
            this.ForceRawCheckBox.Size = new System.Drawing.Size(211, 24);
            this.ForceRawCheckBox.TabIndex = 6;
            this.ForceRawCheckBox.Text = "Force Raw Interface Type";
            this.ForceRawCheckBox.UseVisualStyleBackColor = true;
            this.ForceRawCheckBox.CheckedChanged += new System.EventHandler(this.ForceRawCheckBox_CheckedChanged);
            // 
            // ClearConnsOnStartCheckBox
            // 
            this.ClearConnsOnStartCheckBox.AutoSize = true;
            this.ClearConnsOnStartCheckBox.Checked = true;
            this.ClearConnsOnStartCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ClearConnsOnStartCheckBox.Enabled = false;
            this.ClearConnsOnStartCheckBox.Location = new System.Drawing.Point(139, 114);
            this.ClearConnsOnStartCheckBox.Name = "ClearConnsOnStartCheckBox";
            this.ClearConnsOnStartCheckBox.Size = new System.Drawing.Size(277, 24);
            this.ClearConnsOnStartCheckBox.TabIndex = 4;
            this.ClearConnsOnStartCheckBox.Text = "Clear Connections on Start Sniffing";
            this.ClearConnsOnStartCheckBox.UseVisualStyleBackColor = true;
            // 
            // RefreshInterfacesButton
            // 
            this.RefreshInterfacesButton.Location = new System.Drawing.Point(441, 68);
            this.RefreshInterfacesButton.Name = "RefreshInterfacesButton";
            this.RefreshInterfacesButton.Size = new System.Drawing.Size(205, 40);
            this.RefreshInterfacesButton.TabIndex = 3;
            this.RefreshInterfacesButton.Text = "Refresh Interfaces";
            this.RefreshInterfacesButton.UseVisualStyleBackColor = true;
            this.RefreshInterfacesButton.Click += new System.EventHandler(this.RefreshInterfacesButton_Click);
            // 
            // InterfaceLabel
            // 
            this.InterfaceLabel.Location = new System.Drawing.Point(20, 38);
            this.InterfaceLabel.Name = "InterfaceLabel";
            this.InterfaceLabel.Size = new System.Drawing.Size(77, 20);
            this.InterfaceLabel.TabIndex = 0;
            this.InterfaceLabel.Text = "Interface:";
            // 
            // RemoveLocalConnectionsCheckBox
            // 
            this.RemoveLocalConnectionsCheckBox.AutoSize = true;
            this.RemoveLocalConnectionsCheckBox.Checked = true;
            this.RemoveLocalConnectionsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RemoveLocalConnectionsCheckBox.Enabled = false;
            this.RemoveLocalConnectionsCheckBox.Location = new System.Drawing.Point(139, 144);
            this.RemoveLocalConnectionsCheckBox.Name = "RemoveLocalConnectionsCheckBox";
            this.RemoveLocalConnectionsCheckBox.Size = new System.Drawing.Size(222, 24);
            this.RemoveLocalConnectionsCheckBox.TabIndex = 5;
            this.RemoveLocalConnectionsCheckBox.Text = "Remove Local Connections";
            this.RemoveLocalConnectionsCheckBox.UseVisualStyleBackColor = true;
            // 
            // InterfaceDropDownList
            // 
            this.InterfaceDropDownList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InterfaceDropDownList.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InterfaceDropDownList.FormattingEnabled = true;
            this.InterfaceDropDownList.Location = new System.Drawing.Point(139, 36);
            this.InterfaceDropDownList.Name = "InterfaceDropDownList";
            this.InterfaceDropDownList.Size = new System.Drawing.Size(507, 27);
            this.InterfaceDropDownList.TabIndex = 0;
            this.InterfaceDropDownList.SelectedIndexChanged += new System.EventHandler(this.InterfaceDropDownList_SelectedIndexChanged);
            // 
            // CaptureStopButton
            // 
            this.CaptureStopButton.Enabled = false;
            this.CaptureStopButton.Location = new System.Drawing.Point(290, 68);
            this.CaptureStopButton.Name = "CaptureStopButton";
            this.CaptureStopButton.Size = new System.Drawing.Size(145, 40);
            this.CaptureStopButton.TabIndex = 2;
            this.CaptureStopButton.Text = "Stop Sniffing";
            this.CaptureStopButton.UseVisualStyleBackColor = true;
            this.CaptureStopButton.Click += new System.EventHandler(this.CaptureStopButton_Click);
            // 
            // CaptureStartButton
            // 
            this.CaptureStartButton.BackColor = System.Drawing.Color.Transparent;
            this.CaptureStartButton.Enabled = false;
            this.CaptureStartButton.Location = new System.Drawing.Point(139, 68);
            this.CaptureStartButton.Name = "CaptureStartButton";
            this.CaptureStartButton.Size = new System.Drawing.Size(145, 40);
            this.CaptureStartButton.TabIndex = 1;
            this.CaptureStartButton.Text = "Start Sniffing";
            this.CaptureStartButton.UseVisualStyleBackColor = false;
            this.CaptureStartButton.Click += new System.EventHandler(this.CaptureStartButton_Click);
            // 
            // CaptureFilterGroupBox
            // 
            this.CaptureFilterGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CaptureFilterGroupBox.Controls.Add(this.AnyPortFilter);
            this.CaptureFilterGroupBox.Controls.Add(this.AnyPortLabel);
            this.CaptureFilterGroupBox.Controls.Add(this.AnyIPFilter);
            this.CaptureFilterGroupBox.Controls.Add(this.AnyIPLabel);
            this.CaptureFilterGroupBox.Controls.Add(this.FilterStringLabel);
            this.CaptureFilterGroupBox.Controls.Add(this.CaptureFilterStr);
            this.CaptureFilterGroupBox.Controls.Add(this.ClearFiltersButton);
            this.CaptureFilterGroupBox.Controls.Add(this.ProtocolLabel);
            this.CaptureFilterGroupBox.Controls.Add(this.UDPCheckBox);
            this.CaptureFilterGroupBox.Controls.Add(this.TCPCheckBox);
            this.CaptureFilterGroupBox.Controls.Add(this.SourceIPLabel);
            this.CaptureFilterGroupBox.Controls.Add(this.DestinationIPLabel);
            this.CaptureFilterGroupBox.Controls.Add(this.SourceIPFilter);
            this.CaptureFilterGroupBox.Controls.Add(this.DestinationPortLabel);
            this.CaptureFilterGroupBox.Controls.Add(this.DestinationIPFilter);
            this.CaptureFilterGroupBox.Controls.Add(this.SourcePortLabel);
            this.CaptureFilterGroupBox.Controls.Add(this.SourcePortFilter);
            this.CaptureFilterGroupBox.Controls.Add(this.DestinationPortFilter);
            this.CaptureFilterGroupBox.Enabled = false;
            this.CaptureFilterGroupBox.Location = new System.Drawing.Point(6, 230);
            this.CaptureFilterGroupBox.Name = "CaptureFilterGroupBox";
            this.CaptureFilterGroupBox.Size = new System.Drawing.Size(1380, 293);
            this.CaptureFilterGroupBox.TabIndex = 12;
            this.CaptureFilterGroupBox.TabStop = false;
            this.CaptureFilterGroupBox.Text = "Capture Filter";
            // 
            // AnyPortFilter
            // 
            this.AnyPortFilter.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AnyPortFilter.Location = new System.Drawing.Point(515, 36);
            this.AnyPortFilter.MaxLength = 5;
            this.AnyPortFilter.Name = "AnyPortFilter";
            this.AnyPortFilter.Size = new System.Drawing.Size(131, 26);
            this.AnyPortFilter.TabIndex = 8;
            this.AnyPortFilter.WordWrap = false;
            // 
            // AnyPortLabel
            // 
            this.AnyPortLabel.AutoSize = true;
            this.AnyPortLabel.Location = new System.Drawing.Point(436, 38);
            this.AnyPortLabel.Name = "AnyPortLabel";
            this.AnyPortLabel.Size = new System.Drawing.Size(73, 20);
            this.AnyPortLabel.TabIndex = 0;
            this.AnyPortLabel.Text = "Any Port:";
            // 
            // AnyIPFilter
            // 
            this.AnyIPFilter.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AnyIPFilter.Location = new System.Drawing.Point(139, 36);
            this.AnyIPFilter.MaxLength = 15;
            this.AnyIPFilter.Name = "AnyIPFilter";
            this.AnyIPFilter.Size = new System.Drawing.Size(284, 26);
            this.AnyIPFilter.TabIndex = 7;
            this.AnyIPFilter.WordWrap = false;
            // 
            // AnyIPLabel
            // 
            this.AnyIPLabel.AutoSize = true;
            this.AnyIPLabel.Location = new System.Drawing.Point(20, 38);
            this.AnyIPLabel.Name = "AnyIPLabel";
            this.AnyIPLabel.Size = new System.Drawing.Size(59, 20);
            this.AnyIPLabel.TabIndex = 0;
            this.AnyIPLabel.Text = "Any IP:";
            // 
            // FilterStringLabel
            // 
            this.FilterStringLabel.AutoSize = true;
            this.FilterStringLabel.Location = new System.Drawing.Point(20, 165);
            this.FilterStringLabel.Name = "FilterStringLabel";
            this.FilterStringLabel.Size = new System.Drawing.Size(94, 20);
            this.FilterStringLabel.TabIndex = 0;
            this.FilterStringLabel.Text = "Filter String:";
            // 
            // CaptureFilterStr
            // 
            this.CaptureFilterStr.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CaptureFilterStr.Location = new System.Drawing.Point(139, 162);
            this.CaptureFilterStr.MaxLength = 1024;
            this.CaptureFilterStr.Name = "CaptureFilterStr";
            this.CaptureFilterStr.Size = new System.Drawing.Size(507, 26);
            this.CaptureFilterStr.TabIndex = 15;
            // 
            // ClearFiltersButton
            // 
            this.ClearFiltersButton.Location = new System.Drawing.Point(139, 194);
            this.ClearFiltersButton.Name = "ClearFiltersButton";
            this.ClearFiltersButton.Size = new System.Drawing.Size(110, 40);
            this.ClearFiltersButton.TabIndex = 16;
            this.ClearFiltersButton.Text = "Clear Filters";
            this.ClearFiltersButton.UseVisualStyleBackColor = true;
            this.ClearFiltersButton.Click += new System.EventHandler(this.ClearFiltersButton_Click);
            // 
            // ProtocolLabel
            // 
            this.ProtocolLabel.AutoSize = true;
            this.ProtocolLabel.Location = new System.Drawing.Point(20, 133);
            this.ProtocolLabel.Name = "ProtocolLabel";
            this.ProtocolLabel.Size = new System.Drawing.Size(71, 20);
            this.ProtocolLabel.TabIndex = 0;
            this.ProtocolLabel.Text = "Protocol:";
            // 
            // UDPCheckBox
            // 
            this.UDPCheckBox.AutoSize = true;
            this.UDPCheckBox.Checked = true;
            this.UDPCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UDPCheckBox.Location = new System.Drawing.Point(203, 132);
            this.UDPCheckBox.Name = "UDPCheckBox";
            this.UDPCheckBox.Size = new System.Drawing.Size(62, 24);
            this.UDPCheckBox.TabIndex = 14;
            this.UDPCheckBox.Text = "UDP";
            this.UDPCheckBox.UseVisualStyleBackColor = true;
            // 
            // TCPCheckBox
            // 
            this.TCPCheckBox.AutoSize = true;
            this.TCPCheckBox.Checked = true;
            this.TCPCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TCPCheckBox.Location = new System.Drawing.Point(139, 132);
            this.TCPCheckBox.Name = "TCPCheckBox";
            this.TCPCheckBox.Size = new System.Drawing.Size(58, 24);
            this.TCPCheckBox.TabIndex = 13;
            this.TCPCheckBox.Text = "TCP";
            this.TCPCheckBox.UseVisualStyleBackColor = true;
            // 
            // SourceIPLabel
            // 
            this.SourceIPLabel.AutoSize = true;
            this.SourceIPLabel.Location = new System.Drawing.Point(20, 71);
            this.SourceIPLabel.Name = "SourceIPLabel";
            this.SourceIPLabel.Size = new System.Drawing.Size(83, 20);
            this.SourceIPLabel.TabIndex = 0;
            this.SourceIPLabel.Text = "Source IP:";
            // 
            // DestinationIPLabel
            // 
            this.DestinationIPLabel.AutoSize = true;
            this.DestinationIPLabel.Location = new System.Drawing.Point(20, 103);
            this.DestinationIPLabel.Name = "DestinationIPLabel";
            this.DestinationIPLabel.Size = new System.Drawing.Size(113, 20);
            this.DestinationIPLabel.TabIndex = 0;
            this.DestinationIPLabel.Text = "Destination IP:";
            // 
            // SourceIPFilter
            // 
            this.SourceIPFilter.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SourceIPFilter.Location = new System.Drawing.Point(139, 68);
            this.SourceIPFilter.MaxLength = 15;
            this.SourceIPFilter.Name = "SourceIPFilter";
            this.SourceIPFilter.Size = new System.Drawing.Size(284, 26);
            this.SourceIPFilter.TabIndex = 9;
            this.SourceIPFilter.WordWrap = false;
            // 
            // DestinationPortLabel
            // 
            this.DestinationPortLabel.AutoSize = true;
            this.DestinationPortLabel.Location = new System.Drawing.Point(429, 103);
            this.DestinationPortLabel.Name = "DestinationPortLabel";
            this.DestinationPortLabel.Size = new System.Drawing.Size(80, 20);
            this.DestinationPortLabel.TabIndex = 0;
            this.DestinationPortLabel.Text = "Dest Port:";
            // 
            // DestinationIPFilter
            // 
            this.DestinationIPFilter.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DestinationIPFilter.Location = new System.Drawing.Point(139, 100);
            this.DestinationIPFilter.MaxLength = 15;
            this.DestinationIPFilter.Name = "DestinationIPFilter";
            this.DestinationIPFilter.Size = new System.Drawing.Size(284, 26);
            this.DestinationIPFilter.TabIndex = 11;
            this.DestinationIPFilter.WordWrap = false;
            // 
            // SourcePortLabel
            // 
            this.SourcePortLabel.AutoSize = true;
            this.SourcePortLabel.Location = new System.Drawing.Point(439, 71);
            this.SourcePortLabel.Name = "SourcePortLabel";
            this.SourcePortLabel.Size = new System.Drawing.Size(70, 20);
            this.SourcePortLabel.TabIndex = 0;
            this.SourcePortLabel.Text = "Src Port:";
            // 
            // SourcePortFilter
            // 
            this.SourcePortFilter.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SourcePortFilter.Location = new System.Drawing.Point(515, 68);
            this.SourcePortFilter.MaxLength = 5;
            this.SourcePortFilter.Name = "SourcePortFilter";
            this.SourcePortFilter.Size = new System.Drawing.Size(131, 26);
            this.SourcePortFilter.TabIndex = 10;
            this.SourcePortFilter.WordWrap = false;
            // 
            // DestinationPortFilter
            // 
            this.DestinationPortFilter.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DestinationPortFilter.Location = new System.Drawing.Point(515, 100);
            this.DestinationPortFilter.MaxLength = 5;
            this.DestinationPortFilter.Name = "DestinationPortFilter";
            this.DestinationPortFilter.Size = new System.Drawing.Size(131, 26);
            this.DestinationPortFilter.TabIndex = 12;
            this.DestinationPortFilter.WordWrap = false;
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.CapturePage);
            this.TabControl.Controls.Add(this.ConnectionPage);
            this.TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TabControl.HotTrack = true;
            this.TabControl.Location = new System.Drawing.Point(0, 24);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(1400, 559);
            this.TabControl.TabIndex = 0;
            // 
            // MainPanel
            // 
            this.MainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainPanel.BackColor = System.Drawing.SystemColors.Window;
            this.MainPanel.Controls.Add(this.TabControl);
            this.MainPanel.Controls.Add(this.ToolBar);
            this.MainPanel.Location = new System.Drawing.Point(2, 26);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(1400, 583);
            this.MainPanel.TabIndex = 0;
            // 
            // TitleBarLebel
            // 
            this.TitleBarLebel.AutoSize = true;
            this.TitleBarLebel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleBarLebel.ForeColor = System.Drawing.Color.White;
            this.TitleBarLebel.Location = new System.Drawing.Point(12, 3);
            this.TitleBarLebel.Name = "TitleBarLebel";
            this.TitleBarLebel.Size = new System.Drawing.Size(50, 18);
            this.TitleBarLebel.TabIndex = 0;
            this.TitleBarLebel.Text = "Kebab";
            this.TitleBarLebel.DoubleClick += new System.EventHandler(this.TitleBarLebel_DoubleClick);
            this.TitleBarLebel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleBarLebel_MouseDown);
            // 
            // ExitButton
            // 
            this.ExitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ExitButton.BackColor = System.Drawing.Color.Transparent;
            this.ExitButton.FlatAppearance.BorderSize = 0;
            this.ExitButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(17)))), ((int)(((byte)(35)))));
            this.ExitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExitButton.Image = ((System.Drawing.Image)(resources.GetObject("ExitButton.Image")));
            this.ExitButton.Location = new System.Drawing.Point(1378, 0);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(26, 26);
            this.ExitButton.TabIndex = 0;
            this.ExitButton.TabStop = false;
            this.ExitButton.UseVisualStyleBackColor = false;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // MaximizeButton
            // 
            this.MaximizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MaximizeButton.BackColor = System.Drawing.Color.Transparent;
            this.MaximizeButton.FlatAppearance.BorderSize = 0;
            this.MaximizeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(64)))));
            this.MaximizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MaximizeButton.Image = ((System.Drawing.Image)(resources.GetObject("MaximizeButton.Image")));
            this.MaximizeButton.Location = new System.Drawing.Point(1352, 0);
            this.MaximizeButton.Name = "MaximizeButton";
            this.MaximizeButton.Size = new System.Drawing.Size(26, 26);
            this.MaximizeButton.TabIndex = 0;
            this.MaximizeButton.TabStop = false;
            this.MaximizeButton.UseVisualStyleBackColor = false;
            this.MaximizeButton.Click += new System.EventHandler(this.MaximizeButton_Click);
            // 
            // MinimizeButton
            // 
            this.MinimizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MinimizeButton.BackColor = System.Drawing.Color.Transparent;
            this.MinimizeButton.FlatAppearance.BorderSize = 0;
            this.MinimizeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(64)))));
            this.MinimizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MinimizeButton.Image = ((System.Drawing.Image)(resources.GetObject("MinimizeButton.Image")));
            this.MinimizeButton.Location = new System.Drawing.Point(1326, 0);
            this.MinimizeButton.Name = "MinimizeButton";
            this.MinimizeButton.Size = new System.Drawing.Size(26, 26);
            this.MinimizeButton.TabIndex = 0;
            this.MinimizeButton.TabStop = false;
            this.MinimizeButton.UseVisualStyleBackColor = false;
            this.MinimizeButton.Click += new System.EventHandler(this.MinimizeButton_Click);
            // 
            // UnmaximizeButton
            // 
            this.UnmaximizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UnmaximizeButton.BackColor = System.Drawing.Color.Transparent;
            this.UnmaximizeButton.FlatAppearance.BorderSize = 0;
            this.UnmaximizeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(64)))));
            this.UnmaximizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UnmaximizeButton.Image = ((System.Drawing.Image)(resources.GetObject("UnmaximizeButton.Image")));
            this.UnmaximizeButton.Location = new System.Drawing.Point(1352, 0);
            this.UnmaximizeButton.Name = "UnmaximizeButton";
            this.UnmaximizeButton.Size = new System.Drawing.Size(26, 26);
            this.UnmaximizeButton.TabIndex = 0;
            this.UnmaximizeButton.TabStop = false;
            this.UnmaximizeButton.UseVisualStyleBackColor = false;
            this.UnmaximizeButton.Visible = false;
            this.UnmaximizeButton.Click += new System.EventHandler(this.UnmaximizeButton_Click);
            // 
            // Number
            // 
            this.Number.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Number.DataPropertyName = "Number";
            this.Number.HeaderText = "#";
            this.Number.MinimumWidth = 60;
            this.Number.Name = "Number";
            this.Number.ReadOnly = true;
            this.Number.Width = 60;
            // 
            // Type
            // 
            this.Type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Type.DataPropertyName = "Type";
            this.Type.HeaderText = "Type";
            this.Type.MinimumWidth = 70;
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            this.Type.Width = 70;
            // 
            // SrcHost
            // 
            this.SrcHost.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.SrcHost.DataPropertyName = "SrcHost";
            this.SrcHost.HeaderText = "Local Address";
            this.SrcHost.MinimumWidth = 160;
            this.SrcHost.Name = "SrcHost";
            this.SrcHost.ReadOnly = true;
            this.SrcHost.Width = 160;
            // 
            // SrcPort
            // 
            this.SrcPort.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.SrcPort.DataPropertyName = "SrcPort";
            this.SrcPort.HeaderText = "Port";
            this.SrcPort.MinimumWidth = 70;
            this.SrcPort.Name = "SrcPort";
            this.SrcPort.ReadOnly = true;
            this.SrcPort.Width = 70;
            // 
            // State
            // 
            this.State.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.State.DataPropertyName = "State";
            this.State.HeaderText = "RXTX";
            this.State.MinimumWidth = 80;
            this.State.Name = "State";
            this.State.ReadOnly = true;
            this.State.Width = 80;
            // 
            // DstHost
            // 
            this.DstHost.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.DstHost.DataPropertyName = "DstHost";
            this.DstHost.HeaderText = "Remote Address";
            this.DstHost.MinimumWidth = 160;
            this.DstHost.Name = "DstHost";
            this.DstHost.ReadOnly = true;
            this.DstHost.Width = 160;
            // 
            // DstPort
            // 
            this.DstPort.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.DstPort.DataPropertyName = "DstPort";
            this.DstPort.HeaderText = "Port";
            this.DstPort.MinimumWidth = 70;
            this.DstPort.Name = "DstPort";
            this.DstPort.ReadOnly = true;
            this.DstPort.Width = 70;
            // 
            // PacketCount
            // 
            this.PacketCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.PacketCount.DataPropertyName = "PacketCount";
            this.PacketCount.HeaderText = "Packets Sent";
            this.PacketCount.MinimumWidth = 145;
            this.PacketCount.Name = "PacketCount";
            this.PacketCount.ReadOnly = true;
            this.PacketCount.Width = 145;
            // 
            // DataSize
            // 
            this.DataSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.DataSize.DataPropertyName = "DataSize";
            this.DataSize.HeaderText = "Bytes Sent";
            this.DataSize.MinimumWidth = 182;
            this.DataSize.Name = "DataSize";
            this.DataSize.ReadOnly = true;
            this.DataSize.Width = 182;
            // 
            // DstGeo
            // 
            this.DstGeo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.DstGeo.DataPropertyName = "DstGeo";
            this.DstGeo.HeaderText = "ISO";
            this.DstGeo.MinimumWidth = 80;
            this.DstGeo.Name = "DstGeo";
            this.DstGeo.ReadOnly = true;
            this.DstGeo.Width = 80;
            // 
            // DstASNOrg
            // 
            this.DstASNOrg.DataPropertyName = "DstASNOrg";
            this.DstASNOrg.HeaderText = "ASN Organization";
            this.DstASNOrg.MinimumWidth = 220;
            this.DstASNOrg.Name = "DstASNOrg";
            this.DstASNOrg.ReadOnly = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.ClientSize = new System.Drawing.Size(1404, 611);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.MinimizeButton);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.TitleBarLebel);
            this.Controls.Add(this.MaximizeButton);
            this.Controls.Add(this.UnmaximizeButton);
            this.DoubleBuffered = true;
            this.Enabled = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.ToolBar;
            this.MinimumSize = new System.Drawing.Size(1080, 575);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kebab";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DoubleClick += new System.EventHandler(this.MainForm_DoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.ToolBar.ResumeLayout(false);
            this.ToolBar.PerformLayout();
            this.ConnectionPage.ResumeLayout(false);
            this.DisplayFilterGroupBox.ResumeLayout(false);
            this.DisplayFilterGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConnectionGridView)).EndInit();
            this.ConnectionContextMenu.ResumeLayout(false);
            this.CapturePage.ResumeLayout(false);
            this.CaptureOptionsGroupBox.ResumeLayout(false);
            this.CaptureOptionsGroupBox.PerformLayout();
            this.CaptureFilterGroupBox.ResumeLayout(false);
            this.CaptureFilterGroupBox.PerformLayout();
            this.TabControl.ResumeLayout(false);
            this.MainPanel.ResumeLayout(false);
            this.MainPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip ToolBar;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.ToolStripMenuItem ExitMenuItem;
        private System.Windows.Forms.TabPage ConnectionPage;
        private System.Windows.Forms.GroupBox DisplayFilterGroupBox;
        private System.Windows.Forms.Button ClearDisplayFilterButton;
        private System.Windows.Forms.TextBox DisplayFilterStr;
        private System.Windows.Forms.Label DisplayFilterStringLabel;
        private System.Windows.Forms.CheckBox TimeoutCheckBox;
        private System.Windows.Forms.DataGridView ConnectionGridView;
        private System.Windows.Forms.TabPage CapturePage;
        private System.Windows.Forms.GroupBox CaptureOptionsGroupBox;
        private System.Windows.Forms.Button RefreshInterfacesButton;
        private System.Windows.Forms.Label InterfaceLabel;
        private System.Windows.Forms.ComboBox InterfaceDropDownList;
        private System.Windows.Forms.Button CaptureStopButton;
        private System.Windows.Forms.Button CaptureStartButton;
        private System.Windows.Forms.GroupBox CaptureFilterGroupBox;
        private System.Windows.Forms.Button ClearFiltersButton;
        private System.Windows.Forms.Label ProtocolLabel;
        private System.Windows.Forms.CheckBox UDPCheckBox;
        private System.Windows.Forms.CheckBox TCPCheckBox;
        private System.Windows.Forms.Label SourceIPLabel;
        private System.Windows.Forms.Label DestinationIPLabel;
        private System.Windows.Forms.TextBox SourceIPFilter;
        private System.Windows.Forms.Label DestinationPortLabel;
        private System.Windows.Forms.TextBox DestinationIPFilter;
        private System.Windows.Forms.Label SourcePortLabel;
        private System.Windows.Forms.TextBox SourcePortFilter;
        private System.Windows.Forms.TextBox DestinationPortFilter;
        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.ContextMenuStrip ConnectionContextMenu;
        private System.Windows.Forms.ToolStripMenuItem CopyComponentMenu;
        private System.Windows.Forms.ToolStripMenuItem LocalAddressPortMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RemoteAddressPortMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CopyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpMenu;
        private System.Windows.Forms.ToolStripMenuItem AboutMenu;
        private System.Windows.Forms.Label FilterStringLabel;
        private System.Windows.Forms.TextBox CaptureFilterStr;
        private System.Windows.Forms.ToolStripMenuItem LocalAddressMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LocalPortMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RemoteAddressMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RemotePortMenuItem;
        private System.Windows.Forms.ToolStripSeparator MenuSeparator2;
        private System.Windows.Forms.ToolStripMenuItem SaveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveAsMenuItem;
        private System.Windows.Forms.ToolStripSeparator MenuSeparator3;
        private System.Windows.Forms.TextBox TimeLimit;
        private System.Windows.Forms.ToolStripSeparator MenuSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ISOMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ASNOrganizationMenuItem;
        private System.Windows.Forms.CheckBox ClearConnsOnStartCheckBox;
        private System.Windows.Forms.ToolStripMenuItem LibpcapVersionInfoMenuItem;
        private System.Windows.Forms.ToolStripSeparator MenuSeparator4;
        private System.Windows.Forms.ToolStripMenuItem CheckForUpdatesMenuItem;
        private System.Windows.Forms.ToolStripSeparator MenuSeparator5;
        private System.Windows.Forms.CheckBox RemoveLocalConnectionsCheckBox;
        private System.Windows.Forms.ToolStripSeparator MenuSeparator6;
        private System.Windows.Forms.ToolStripMenuItem AllRemoteHostInformationMenuItem;
        private System.Windows.Forms.Button ClearConnectionsButton;
        private System.Windows.Forms.CheckBox ForceRawCheckBox;
        private System.Windows.Forms.TextBox AnyPortFilter;
        private System.Windows.Forms.Label AnyPortLabel;
        private System.Windows.Forms.TextBox AnyIPFilter;
        private System.Windows.Forms.Label AnyIPLabel;
        private System.Windows.Forms.ToolStripMenuItem EditMenu;
        private System.Windows.Forms.ToolStripMenuItem PreferencesMenuItem;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.Label TitleBarLebel;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button MaximizeButton;
        private System.Windows.Forms.Button MinimizeButton;
        private System.Windows.Forms.Button UnmaximizeButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrcHost;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrcPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn State;
        private System.Windows.Forms.DataGridViewTextBoxColumn DstHost;
        private System.Windows.Forms.DataGridViewTextBoxColumn DstPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn PacketCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn DstGeo;
        private System.Windows.Forms.DataGridViewTextBoxColumn DstASNOrg;
    }
}

