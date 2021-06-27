﻿
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
            this.TopMenu = new System.Windows.Forms.MenuStrip();
            this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.libpcapVersionInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConnectionPage = new System.Windows.Forms.TabPage();
            this.DisplayFilterGroupBox = new System.Windows.Forms.GroupBox();
            this.timeLimit = new System.Windows.Forms.TextBox();
            this.ClearDisplayFilterButton = new System.Windows.Forms.Button();
            this.DisplayFilter = new System.Windows.Forms.TextBox();
            this.DisplayFilterStringLabel = new System.Windows.Forms.Label();
            this.TimeoutCheckBox = new System.Windows.Forms.CheckBox();
            this.ClearConnectionsButton = new System.Windows.Forms.Button();
            this.ConnectionGridView = new System.Windows.Forms.DataGridView();
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Source = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SrcPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Destination = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DstPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PacketCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ByteCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DstGeo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DstASNOrg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConnectionContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyComponentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.localAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.localPortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.localAddressPortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.remoteAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remotePortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remoteAddressPortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.iSOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aSNOrganizationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.allRemoteHostInformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.TopMenu.SuspendLayout();
            this.ConnectionPage.SuspendLayout();
            this.DisplayFilterGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConnectionGridView)).BeginInit();
            this.ConnectionContextMenuStrip.SuspendLayout();
            this.CapturePage.SuspendLayout();
            this.CaptureOptionsGroupBox.SuspendLayout();
            this.CaptureFilterGroupBox.SuspendLayout();
            this.TabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // TopMenu
            // 
            this.TopMenu.BackColor = System.Drawing.SystemColors.Window;
            this.TopMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.HelpMenu});
            this.TopMenu.Location = new System.Drawing.Point(0, 0);
            this.TopMenu.Name = "TopMenu";
            this.TopMenu.Size = new System.Drawing.Size(1404, 24);
            this.TopMenu.TabIndex = 1;
            // 
            // FileMenu
            // 
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator3,
            this.ExitMenuItem});
            this.FileMenu.Name = "FileMenu";
            this.FileMenu.Size = new System.Drawing.Size(37, 20);
            this.FileMenu.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(183, 6);
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Name = "ExitMenuItem";
            this.ExitMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.ExitMenuItem.Size = new System.Drawing.Size(186, 22);
            this.ExitMenuItem.Text = "Exit";
            this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // HelpMenu
            // 
            this.HelpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkForUpdatesToolStripMenuItem,
            this.toolStripSeparator5,
            this.libpcapVersionInfoToolStripMenuItem,
            this.toolStripSeparator4,
            this.aboutToolStripMenuItem});
            this.HelpMenu.Name = "HelpMenu";
            this.HelpMenu.Size = new System.Drawing.Size(44, 20);
            this.HelpMenu.Text = "Help";
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.checkForUpdatesToolStripMenuItem.Text = "Check for Updates";
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(178, 6);
            // 
            // libpcapVersionInfoToolStripMenuItem
            // 
            this.libpcapVersionInfoToolStripMenuItem.Name = "libpcapVersionInfoToolStripMenuItem";
            this.libpcapVersionInfoToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.libpcapVersionInfoToolStripMenuItem.Text = "Libpcap Version Info";
            this.libpcapVersionInfoToolStripMenuItem.Click += new System.EventHandler(this.libpcapVersionInfoToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(178, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // ConnectionPage
            // 
            this.ConnectionPage.Controls.Add(this.DisplayFilterGroupBox);
            this.ConnectionPage.Controls.Add(this.ConnectionGridView);
            this.ConnectionPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectionPage.Location = new System.Drawing.Point(4, 29);
            this.ConnectionPage.Name = "ConnectionPage";
            this.ConnectionPage.Padding = new System.Windows.Forms.Padding(3);
            this.ConnectionPage.Size = new System.Drawing.Size(1396, 554);
            this.ConnectionPage.TabIndex = 1;
            this.ConnectionPage.Text = "Connections";
            this.ConnectionPage.UseVisualStyleBackColor = true;
            // 
            // DisplayFilterGroupBox
            // 
            this.DisplayFilterGroupBox.Controls.Add(this.timeLimit);
            this.DisplayFilterGroupBox.Controls.Add(this.ClearDisplayFilterButton);
            this.DisplayFilterGroupBox.Controls.Add(this.DisplayFilter);
            this.DisplayFilterGroupBox.Controls.Add(this.DisplayFilterStringLabel);
            this.DisplayFilterGroupBox.Controls.Add(this.TimeoutCheckBox);
            this.DisplayFilterGroupBox.Controls.Add(this.ClearConnectionsButton);
            this.DisplayFilterGroupBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DisplayFilterGroupBox.Enabled = false;
            this.DisplayFilterGroupBox.Location = new System.Drawing.Point(3, 491);
            this.DisplayFilterGroupBox.Name = "DisplayFilterGroupBox";
            this.DisplayFilterGroupBox.Size = new System.Drawing.Size(1390, 60);
            this.DisplayFilterGroupBox.TabIndex = 2;
            this.DisplayFilterGroupBox.TabStop = false;
            this.DisplayFilterGroupBox.Text = "Display Filter";
            // 
            // timeLimit
            // 
            this.timeLimit.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeLimit.Location = new System.Drawing.Point(831, 28);
            this.timeLimit.MaxLength = 3;
            this.timeLimit.Name = "timeLimit";
            this.timeLimit.Size = new System.Drawing.Size(53, 26);
            this.timeLimit.TabIndex = 5;
            this.timeLimit.Text = "30";
            this.timeLimit.TextChanged += new System.EventHandler(this.timeLimit_TextChanged);
            // 
            // ClearDisplayFilterButton
            // 
            this.ClearDisplayFilterButton.Location = new System.Drawing.Point(619, 27);
            this.ClearDisplayFilterButton.MinimumSize = new System.Drawing.Size(98, 27);
            this.ClearDisplayFilterButton.Name = "ClearDisplayFilterButton";
            this.ClearDisplayFilterButton.Size = new System.Drawing.Size(111, 28);
            this.ClearDisplayFilterButton.TabIndex = 3;
            this.ClearDisplayFilterButton.Text = "Clear Filter";
            this.ClearDisplayFilterButton.UseVisualStyleBackColor = true;
            this.ClearDisplayFilterButton.Click += new System.EventHandler(this.ClearDisplayFiltersButton_Click);
            // 
            // DisplayFilter
            // 
            this.DisplayFilter.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DisplayFilter.Location = new System.Drawing.Point(106, 28);
            this.DisplayFilter.MaxLength = 1024;
            this.DisplayFilter.Name = "DisplayFilter";
            this.DisplayFilter.Size = new System.Drawing.Size(507, 26);
            this.DisplayFilter.TabIndex = 1;
            this.DisplayFilter.WordWrap = false;
            this.DisplayFilter.TextChanged += new System.EventHandler(this.DisplayFilter_TextChanged);
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
            this.TimeoutCheckBox.AutoSize = true;
            this.TimeoutCheckBox.Location = new System.Drawing.Point(736, 29);
            this.TimeoutCheckBox.Name = "TimeoutCheckBox";
            this.TimeoutCheckBox.Size = new System.Drawing.Size(89, 24);
            this.TimeoutCheckBox.TabIndex = 4;
            this.TimeoutCheckBox.Text = "Timeout:";
            this.TimeoutCheckBox.UseVisualStyleBackColor = true;
            this.TimeoutCheckBox.CheckedChanged += new System.EventHandler(this.TimeoutCheckBox_CheckedChanged);
            // 
            // ClearConnectionsButton
            // 
            this.ClearConnectionsButton.Location = new System.Drawing.Point(890, 27);
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
            this.ConnectionGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(38)))), ((int)(((byte)(46)))));
            this.ConnectionGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ConnectionGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.ConnectionGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.ConnectionGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(37)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(37)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ConnectionGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ConnectionGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ConnectionGridView.ColumnHeadersVisible = false;
            this.ConnectionGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Number,
            this.Type,
            this.Source,
            this.SrcPort,
            this.State,
            this.Destination,
            this.DstPort,
            this.PacketCount,
            this.ByteCount,
            this.DstGeo,
            this.DstASNOrg});
            this.ConnectionGridView.ContextMenuStrip = this.ConnectionContextMenuStrip;
            this.ConnectionGridView.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(38)))), ((int)(((byte)(46)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(204)))), ((int)(((byte)(132)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(38)))), ((int)(((byte)(46)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ConnectionGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.ConnectionGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConnectionGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.ConnectionGridView.EnableHeadersVisualStyles = false;
            this.ConnectionGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(38)))), ((int)(((byte)(46)))));
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
            this.ConnectionGridView.Size = new System.Drawing.Size(1390, 548);
            this.ConnectionGridView.StandardTab = true;
            this.ConnectionGridView.TabIndex = 0;
            this.ConnectionGridView.TabStop = false;
            this.ConnectionGridView.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.ConnectionGridView_RowsAdded);
            this.ConnectionGridView.Sorted += new System.EventHandler(this.ConnectionGridView_Sorted);
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
            // Source
            // 
            this.Source.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Source.DataPropertyName = "Source";
            this.Source.HeaderText = "Local Address";
            this.Source.MinimumWidth = 160;
            this.Source.Name = "Source";
            this.Source.ReadOnly = true;
            this.Source.Width = 160;
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
            // Destination
            // 
            this.Destination.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Destination.DataPropertyName = "Destination";
            this.Destination.HeaderText = "Remote Address";
            this.Destination.MinimumWidth = 160;
            this.Destination.Name = "Destination";
            this.Destination.ReadOnly = true;
            this.Destination.Width = 160;
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
            // ByteCount
            // 
            this.ByteCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ByteCount.DataPropertyName = "ByteCount";
            this.ByteCount.HeaderText = "Bytes Sent";
            this.ByteCount.MinimumWidth = 182;
            this.ByteCount.Name = "ByteCount";
            this.ByteCount.ReadOnly = true;
            this.ByteCount.Width = 182;
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
            // ConnectionContextMenuStrip
            // 
            this.ConnectionContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.copyComponentToolStripMenuItem});
            this.ConnectionContextMenuStrip.Name = "ConnectionContextMenuStrip";
            this.ConnectionContextMenuStrip.Size = new System.Drawing.Size(187, 48);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.copyToolStripMenuItem.Text = "Copy  Row(s)";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // copyComponentToolStripMenuItem
            // 
            this.copyComponentToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.localAddressToolStripMenuItem,
            this.localPortToolStripMenuItem,
            this.localAddressPortToolStripMenuItem,
            this.toolStripSeparator2,
            this.remoteAddressToolStripMenuItem,
            this.remotePortToolStripMenuItem,
            this.remoteAddressPortToolStripMenuItem,
            this.toolStripSeparator1,
            this.iSOToolStripMenuItem,
            this.aSNOrganizationToolStripMenuItem,
            this.toolStripSeparator6,
            this.allRemoteHostInformationToolStripMenuItem});
            this.copyComponentToolStripMenuItem.Name = "copyComponentToolStripMenuItem";
            this.copyComponentToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.copyComponentToolStripMenuItem.Text = "Copy Component(s)";
            this.copyComponentToolStripMenuItem.MouseHover += new System.EventHandler(this.copyComponentToolStripMenuItem_MouseHover);
            // 
            // localAddressToolStripMenuItem
            // 
            this.localAddressToolStripMenuItem.Name = "localAddressToolStripMenuItem";
            this.localAddressToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.localAddressToolStripMenuItem.Text = "Local Address";
            this.localAddressToolStripMenuItem.Click += new System.EventHandler(this.localAddressToolStripMenuItem_Click);
            // 
            // localPortToolStripMenuItem
            // 
            this.localPortToolStripMenuItem.Name = "localPortToolStripMenuItem";
            this.localPortToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.localPortToolStripMenuItem.Text = "Local Port";
            this.localPortToolStripMenuItem.Click += new System.EventHandler(this.localPortToolStripMenuItem_Click);
            // 
            // localAddressPortToolStripMenuItem
            // 
            this.localAddressPortToolStripMenuItem.Name = "localAddressPortToolStripMenuItem";
            this.localAddressPortToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.localAddressPortToolStripMenuItem.Text = "Local Address : Local Port";
            this.localAddressPortToolStripMenuItem.Click += new System.EventHandler(this.localAddressPortToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(297, 6);
            // 
            // remoteAddressToolStripMenuItem
            // 
            this.remoteAddressToolStripMenuItem.Name = "remoteAddressToolStripMenuItem";
            this.remoteAddressToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.remoteAddressToolStripMenuItem.Text = "Remote Address";
            this.remoteAddressToolStripMenuItem.Click += new System.EventHandler(this.remoteAddressToolStripMenuItem_Click);
            // 
            // remotePortToolStripMenuItem
            // 
            this.remotePortToolStripMenuItem.Name = "remotePortToolStripMenuItem";
            this.remotePortToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.remotePortToolStripMenuItem.Text = "Remote Port";
            this.remotePortToolStripMenuItem.Click += new System.EventHandler(this.remotePortToolStripMenuItem_Click);
            // 
            // remoteAddressPortToolStripMenuItem
            // 
            this.remoteAddressPortToolStripMenuItem.Name = "remoteAddressPortToolStripMenuItem";
            this.remoteAddressPortToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.remoteAddressPortToolStripMenuItem.Text = "Remote Address : Remote Port";
            this.remoteAddressPortToolStripMenuItem.Click += new System.EventHandler(this.remoteAddressPortToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(297, 6);
            // 
            // iSOToolStripMenuItem
            // 
            this.iSOToolStripMenuItem.Name = "iSOToolStripMenuItem";
            this.iSOToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.iSOToolStripMenuItem.Text = "ISO";
            this.iSOToolStripMenuItem.Click += new System.EventHandler(this.iSOToolStripMenuItem_Click);
            // 
            // aSNOrganizationToolStripMenuItem
            // 
            this.aSNOrganizationToolStripMenuItem.Name = "aSNOrganizationToolStripMenuItem";
            this.aSNOrganizationToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.aSNOrganizationToolStripMenuItem.Text = "ASN Organization";
            this.aSNOrganizationToolStripMenuItem.Click += new System.EventHandler(this.aSNOrganizationToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(297, 6);
            // 
            // allRemoteHostInformationToolStripMenuItem
            // 
            this.allRemoteHostInformationToolStripMenuItem.Name = "allRemoteHostInformationToolStripMenuItem";
            this.allRemoteHostInformationToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.allRemoteHostInformationToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.allRemoteHostInformationToolStripMenuItem.Text = "All Remote Host Information";
            this.allRemoteHostInformationToolStripMenuItem.Click += new System.EventHandler(this.allRemoteHostInformationToolStripMenuItem_Click);
            // 
            // CapturePage
            // 
            this.CapturePage.Controls.Add(this.CaptureOptionsGroupBox);
            this.CapturePage.Controls.Add(this.CaptureFilterGroupBox);
            this.CapturePage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CapturePage.Location = new System.Drawing.Point(4, 29);
            this.CapturePage.Name = "CapturePage";
            this.CapturePage.Padding = new System.Windows.Forms.Padding(3);
            this.CapturePage.Size = new System.Drawing.Size(1396, 554);
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
            this.CaptureOptionsGroupBox.Size = new System.Drawing.Size(1384, 218);
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
            this.CaptureStartButton.Enabled = false;
            this.CaptureStartButton.Location = new System.Drawing.Point(139, 69);
            this.CaptureStartButton.Name = "CaptureStartButton";
            this.CaptureStartButton.Size = new System.Drawing.Size(145, 40);
            this.CaptureStartButton.TabIndex = 1;
            this.CaptureStartButton.Text = "Start Sniffing";
            this.CaptureStartButton.UseVisualStyleBackColor = true;
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
            this.CaptureFilterGroupBox.Size = new System.Drawing.Size(1384, 321);
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
            this.TabControl.Size = new System.Drawing.Size(1404, 587);
            this.TabControl.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1404, 611);
            this.Controls.Add(this.TabControl);
            this.Controls.Add(this.TopMenu);
            this.DoubleBuffered = true;
            this.Enabled = false;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.TopMenu;
            this.MinimumSize = new System.Drawing.Size(1080, 590);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kebab";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.TopMenu.ResumeLayout(false);
            this.TopMenu.PerformLayout();
            this.ConnectionPage.ResumeLayout(false);
            this.DisplayFilterGroupBox.ResumeLayout(false);
            this.DisplayFilterGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConnectionGridView)).EndInit();
            this.ConnectionContextMenuStrip.ResumeLayout(false);
            this.CapturePage.ResumeLayout(false);
            this.CaptureOptionsGroupBox.ResumeLayout(false);
            this.CaptureOptionsGroupBox.PerformLayout();
            this.CaptureFilterGroupBox.ResumeLayout(false);
            this.CaptureFilterGroupBox.PerformLayout();
            this.TabControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip TopMenu;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.ToolStripMenuItem ExitMenuItem;
        private System.Windows.Forms.TabPage ConnectionPage;
        private System.Windows.Forms.GroupBox DisplayFilterGroupBox;
        private System.Windows.Forms.Button ClearDisplayFilterButton;
        private System.Windows.Forms.TextBox DisplayFilter;
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
        private System.Windows.Forms.ContextMenuStrip ConnectionContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem copyComponentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem localAddressPortToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem remoteAddressPortToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpMenu;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Label FilterStringLabel;
        private System.Windows.Forms.TextBox CaptureFilterStr;
        private System.Windows.Forms.ToolStripMenuItem localAddressToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem localPortToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem remoteAddressToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem remotePortToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.TextBox timeLimit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem iSOToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aSNOrganizationToolStripMenuItem;
        private System.Windows.Forms.CheckBox ClearConnsOnStartCheckBox;
        private System.Windows.Forms.ToolStripMenuItem libpcapVersionInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.CheckBox RemoveLocalConnectionsCheckBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem allRemoteHostInformationToolStripMenuItem;
        private System.Windows.Forms.Button ClearConnectionsButton;
        private System.Windows.Forms.CheckBox ForceRawCheckBox;
        private System.Windows.Forms.TextBox AnyPortFilter;
        private System.Windows.Forms.Label AnyPortLabel;
        private System.Windows.Forms.TextBox AnyIPFilter;
        private System.Windows.Forms.Label AnyIPLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Source;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrcPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn State;
        private System.Windows.Forms.DataGridViewTextBoxColumn Destination;
        private System.Windows.Forms.DataGridViewTextBoxColumn DstPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn PacketCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ByteCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn DstGeo;
        private System.Windows.Forms.DataGridViewTextBoxColumn DstASNOrg;
    }
}

