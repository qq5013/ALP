namespace App
{
    partial class Main
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemDelCraneTask = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemCellCode = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemReassign = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemStateChange = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem12 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem13 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem14 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem15 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem16 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem17 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem18 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem19 = new System.Windows.Forms.ToolStripMenuItem();
            this.bsMain = new System.Windows.Forms.BindingSource(this.components);
            this.pnlTab = new System.Windows.Forms.Panel();
            this.tabForm = new System.Windows.Forms.TabControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_InStockTask = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_CellMonitor = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_TaskQuery = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_StartCrane = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Close = new System.Windows.Forms.ToolStripButton();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvMain = new System.Windows.Forms.DataGridView();
            this.Column5 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.Column1 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.Column11 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.colPalletCode = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.colCraneNo = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.Column3 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.colFromStation = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.colToStation = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.Column13 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.Column8 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.lbLog = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.taskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inStockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Monitor = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Cell = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_TaskQuery = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_CraneHandle = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsMain)).BeginInit();
            this.pnlTab.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemDelCraneTask,
            this.ToolStripMenuItemCellCode,
            this.ToolStripMenuItemReassign,
            this.ToolStripMenuItemStateChange});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 92);
            // 
            // ToolStripMenuItemDelCraneTask
            // 
            this.ToolStripMenuItemDelCraneTask.Name = "ToolStripMenuItemDelCraneTask";
            this.ToolStripMenuItemDelCraneTask.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItemDelCraneTask.Text = "下发取消任务";
            this.ToolStripMenuItemDelCraneTask.Click += new System.EventHandler(this.ToolStripMenuItemDelCraneTask_Click);
            // 
            // ToolStripMenuItemCellCode
            // 
            this.ToolStripMenuItemCellCode.Name = "ToolStripMenuItemCellCode";
            this.ToolStripMenuItemCellCode.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItemCellCode.Text = "重新分配货位";
            this.ToolStripMenuItemCellCode.Click += new System.EventHandler(this.ToolStripMenuItemCellCode_Click);
            // 
            // ToolStripMenuItemReassign
            // 
            this.ToolStripMenuItemReassign.Name = "ToolStripMenuItemReassign";
            this.ToolStripMenuItemReassign.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItemReassign.Text = "重新下发任务";
            this.ToolStripMenuItemReassign.Click += new System.EventHandler(this.ToolStripMenuItemReassign_Click);
            // 
            // ToolStripMenuItemStateChange
            // 
            this.ToolStripMenuItemStateChange.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem10,
            this.ToolStripMenuItem11,
            this.ToolStripMenuItem12,
            this.ToolStripMenuItem13,
            this.ToolStripMenuItem14,
            this.ToolStripMenuItem15,
            this.ToolStripMenuItem16,
            this.ToolStripMenuItem17,
            this.ToolStripMenuItem18,
            this.ToolStripMenuItem19});
            this.ToolStripMenuItemStateChange.Name = "ToolStripMenuItemStateChange";
            this.ToolStripMenuItemStateChange.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItemStateChange.Text = "任务状态切换";
            // 
            // ToolStripMenuItem10
            // 
            this.ToolStripMenuItem10.Name = "ToolStripMenuItem10";
            this.ToolStripMenuItem10.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItem10.Text = "等待";
            this.ToolStripMenuItem10.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem11
            // 
            this.ToolStripMenuItem11.Name = "ToolStripMenuItem11";
            this.ToolStripMenuItem11.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItem11.Text = "请求入库";
            this.ToolStripMenuItem11.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem12
            // 
            this.ToolStripMenuItem12.Name = "ToolStripMenuItem12";
            this.ToolStripMenuItem12.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItem12.Text = "入库站台";
            this.ToolStripMenuItem12.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem13
            // 
            this.ToolStripMenuItem13.Name = "ToolStripMenuItem13";
            this.ToolStripMenuItem13.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItem13.Text = "上架执行";
            this.ToolStripMenuItem13.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem14
            // 
            this.ToolStripMenuItem14.Name = "ToolStripMenuItem14";
            this.ToolStripMenuItem14.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItem14.Text = "下架执行";
            this.ToolStripMenuItem14.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem15
            // 
            this.ToolStripMenuItem15.Name = "ToolStripMenuItem15";
            this.ToolStripMenuItem15.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItem15.Text = "到达出库站台";
            // 
            // ToolStripMenuItem16
            // 
            this.ToolStripMenuItem16.Name = "ToolStripMenuItem16";
            this.ToolStripMenuItem16.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItem16.Text = "出库输送";
            // 
            // ToolStripMenuItem17
            // 
            this.ToolStripMenuItem17.Name = "ToolStripMenuItem17";
            this.ToolStripMenuItem17.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItem17.Text = "完成";
            this.ToolStripMenuItem17.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem18
            // 
            this.ToolStripMenuItem18.Name = "ToolStripMenuItem18";
            this.ToolStripMenuItem18.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItem18.Text = "盘点中";
            // 
            // ToolStripMenuItem19
            // 
            this.ToolStripMenuItem19.Name = "ToolStripMenuItem19";
            this.ToolStripMenuItem19.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItem19.Text = "取消";
            this.ToolStripMenuItem19.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // pnlTab
            // 
            this.pnlTab.BackColor = System.Drawing.SystemColors.Menu;
            this.pnlTab.Controls.Add(this.tabForm);
            this.pnlTab.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTab.Location = new System.Drawing.Point(0, 76);
            this.pnlTab.Name = "pnlTab";
            this.pnlTab.Size = new System.Drawing.Size(1284, 23);
            this.pnlTab.TabIndex = 14;
            this.pnlTab.Visible = false;
            // 
            // tabForm
            // 
            this.tabForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabForm.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabForm.Location = new System.Drawing.Point(0, 0);
            this.tabForm.Name = "tabForm";
            this.tabForm.SelectedIndex = 0;
            this.tabForm.Size = new System.Drawing.Size(1284, 23);
            this.tabForm.TabIndex = 6;
            this.tabForm.SelectedIndexChanged += new System.EventHandler(this.tabForm_SelectedIndexChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_InStockTask,
            this.toolStripButton_CellMonitor,
            this.toolStripButton_TaskQuery,
            this.toolStripButton_StartCrane,
            this.toolStripButton_Close});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1284, 52);
            this.toolStrip1.TabIndex = 13;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_InStockTask
            // 
            this.toolStripButton_InStockTask.AutoSize = false;
            this.toolStripButton_InStockTask.Image = global::App.Properties.Resources.down;
            this.toolStripButton_InStockTask.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_InStockTask.Name = "toolStripButton_InStockTask";
            this.toolStripButton_InStockTask.Size = new System.Drawing.Size(60, 50);
            this.toolStripButton_InStockTask.Text = "WMS任務";
            this.toolStripButton_InStockTask.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_InStockTask.Click += new System.EventHandler(this.toolStripButton_InStockTask_Click);
            // 
            // toolStripButton_CellMonitor
            // 
            this.toolStripButton_CellMonitor.AutoSize = false;
            this.toolStripButton_CellMonitor.Image = global::App.Properties.Resources.monitor;
            this.toolStripButton_CellMonitor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_CellMonitor.Name = "toolStripButton_CellMonitor";
            this.toolStripButton_CellMonitor.Size = new System.Drawing.Size(70, 50);
            this.toolStripButton_CellMonitor.Text = "货位查詢";
            this.toolStripButton_CellMonitor.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_CellMonitor.Click += new System.EventHandler(this.toolStripButton_CellMonitor_Click);
            // 
            // toolStripButton_TaskQuery
            // 
            this.toolStripButton_TaskQuery.AutoSize = false;
            this.toolStripButton_TaskQuery.Image = global::App.Properties.Resources.zoom;
            this.toolStripButton_TaskQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_TaskQuery.Name = "toolStripButton_TaskQuery";
            this.toolStripButton_TaskQuery.Size = new System.Drawing.Size(60, 50);
            this.toolStripButton_TaskQuery.Text = "任務查詢";
            this.toolStripButton_TaskQuery.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_TaskQuery.Click += new System.EventHandler(this.toolStripButton_TaskQuery_Click);
            // 
            // toolStripButton_StartCrane
            // 
            this.toolStripButton_StartCrane.AutoSize = false;
            this.toolStripButton_StartCrane.Image = global::App.Properties.Resources.process_remove;
            this.toolStripButton_StartCrane.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_StartCrane.Name = "toolStripButton_StartCrane";
            this.toolStripButton_StartCrane.Size = new System.Drawing.Size(70, 50);
            this.toolStripButton_StartCrane.Text = "聯機自動";
            this.toolStripButton_StartCrane.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_StartCrane.Click += new System.EventHandler(this.toolStripButton_StartCrane_Click);
            // 
            // toolStripButton_Close
            // 
            this.toolStripButton_Close.AutoSize = false;
            this.toolStripButton_Close.Image = global::App.Properties.Resources.remove;
            this.toolStripButton_Close.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Close.Name = "toolStripButton_Close";
            this.toolStripButton_Close.Size = new System.Drawing.Size(60, 50);
            this.toolStripButton_Close.Text = "退出系統";
            this.toolStripButton_Close.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_Close.Click += new System.EventHandler(this.toolStripButton_Close_Click);
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.splitContainer1);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 340);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1284, 220);
            this.pnlBottom.TabIndex = 9;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvMain);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lbLog);
            this.splitContainer1.Size = new System.Drawing.Size(1284, 220);
            this.splitContainer1.SplitterDistance = 944;
            this.splitContainer1.TabIndex = 2;
            // 
            // dgvMain
            // 
            this.dgvMain.AllowUserToAddRows = false;
            this.dgvMain.AllowUserToDeleteRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dgvMain.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvMain.AutoGenerateColumns = false;
            this.dgvMain.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column5,
            this.Column1,
            this.Column11,
            this.colPalletCode,
            this.colCraneNo,
            this.Column3,
            this.colFromStation,
            this.colToStation,
            this.Column13,
            this.Column8});
            this.dgvMain.DataSource = this.bsMain;
            this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMain.Location = new System.Drawing.Point(0, 0);
            this.dgvMain.Name = "dgvMain";
            this.dgvMain.ReadOnly = true;
            this.dgvMain.RowHeadersWidth = 20;
            this.dgvMain.RowTemplate.Height = 23;
            this.dgvMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMain.Size = new System.Drawing.Size(944, 220);
            this.dgvMain.TabIndex = 7;
            this.dgvMain.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvMain_CellMouseClick);
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "TaskNo";
            this.Column5.FilteringEnabled = false;
            this.Column5.Frozen = true;
            this.Column5.HeaderText = "任務號";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "Type";
            this.Column1.FilteringEnabled = false;
            this.Column1.Frozen = true;
            this.Column1.HeaderText = "任務類型";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.Width = 80;
            // 
            // Column11
            // 
            this.Column11.DataPropertyName = "StateDesc";
            this.Column11.FilteringEnabled = false;
            this.Column11.Frozen = true;
            this.Column11.HeaderText = "狀態";
            this.Column11.Name = "Column11";
            this.Column11.ReadOnly = true;
            this.Column11.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column11.Width = 70;
            // 
            // colPalletCode
            // 
            this.colPalletCode.DataPropertyName = "PalletCode";
            this.colPalletCode.FilteringEnabled = false;
            this.colPalletCode.Frozen = true;
            this.colPalletCode.HeaderText = "盤/箱條碼";
            this.colPalletCode.Name = "colPalletCode";
            this.colPalletCode.ReadOnly = true;
            this.colPalletCode.Width = 120;
            // 
            // colCraneNo
            // 
            this.colCraneNo.DataPropertyName = "CraneNo";
            this.colCraneNo.FillWeight = 80F;
            this.colCraneNo.FilteringEnabled = false;
            this.colCraneNo.Frozen = true;
            this.colCraneNo.HeaderText = "堆垛機";
            this.colCraneNo.Name = "colCraneNo";
            this.colCraneNo.ReadOnly = true;
            this.colCraneNo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCraneNo.Width = 70;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "AisleNo";
            this.Column3.FillWeight = 80F;
            this.Column3.FilteringEnabled = false;
            this.Column3.Frozen = true;
            this.Column3.HeaderText = "巷道編號";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 80;
            // 
            // colFromStation
            // 
            this.colFromStation.DataPropertyName = "FromStation";
            this.colFromStation.FilteringEnabled = false;
            this.colFromStation.HeaderText = "起始地址";
            this.colFromStation.Name = "colFromStation";
            this.colFromStation.ReadOnly = true;
            this.colFromStation.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colToStation
            // 
            this.colToStation.DataPropertyName = "ToStation";
            this.colToStation.FilteringEnabled = false;
            this.colToStation.HeaderText = "目標地址";
            this.colToStation.Name = "colToStation";
            this.colToStation.ReadOnly = true;
            this.colToStation.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Column13
            // 
            this.Column13.DataPropertyName = "RequestDate";
            this.Column13.FilteringEnabled = false;
            this.Column13.HeaderText = "請求時間";
            this.Column13.Name = "Column13";
            this.Column13.ReadOnly = true;
            this.Column13.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Column8
            // 
            this.Column8.DataPropertyName = "StartDate";
            this.Column8.FilteringEnabled = false;
            this.Column8.HeaderText = "開始時間";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // lbLog
            // 
            this.lbLog.BackColor = System.Drawing.SystemColors.Window;
            this.lbLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbLog.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbLog.FullRowSelect = true;
            this.lbLog.LabelWrap = false;
            this.lbLog.Location = new System.Drawing.Point(0, 0);
            this.lbLog.Name = "lbLog";
            this.lbLog.ShowGroups = false;
            this.lbLog.Size = new System.Drawing.Size(336, 220);
            this.lbLog.TabIndex = 10;
            this.lbLog.UseCompatibleStateImageBehavior = false;
            this.lbLog.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Header";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "DateTime";
            this.columnHeader2.Width = 143;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Log";
            this.columnHeader3.Width = 800;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.taskToolStripMenuItem,
            this.ToolStripMenuItem_Monitor});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1284, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // taskToolStripMenuItem
            // 
            this.taskToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inStockToolStripMenuItem});
            this.taskToolStripMenuItem.Name = "taskToolStripMenuItem";
            this.taskToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.taskToolStripMenuItem.Text = "任務操作";
            // 
            // inStockToolStripMenuItem
            // 
            this.inStockToolStripMenuItem.Name = "inStockToolStripMenuItem";
            this.inStockToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.inStockToolStripMenuItem.Text = "WMS任務";
            this.inStockToolStripMenuItem.Click += new System.EventHandler(this.inStockToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem_Monitor
            // 
            this.ToolStripMenuItem_Monitor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_Cell,
            this.ToolStripMenuItem_TaskQuery,
            this.ToolStripMenuItem_CraneHandle});
            this.ToolStripMenuItem_Monitor.Name = "ToolStripMenuItem_Monitor";
            this.ToolStripMenuItem_Monitor.Size = new System.Drawing.Size(68, 20);
            this.ToolStripMenuItem_Monitor.Text = "調度監控";
            // 
            // ToolStripMenuItem_Cell
            // 
            this.ToolStripMenuItem_Cell.Name = "ToolStripMenuItem_Cell";
            this.ToolStripMenuItem_Cell.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItem_Cell.Text = "貨位查詢";
            this.ToolStripMenuItem_Cell.Click += new System.EventHandler(this.ToolStripMenuItem_Cell_Click);
            // 
            // ToolStripMenuItem_TaskQuery
            // 
            this.ToolStripMenuItem_TaskQuery.Name = "ToolStripMenuItem_TaskQuery";
            this.ToolStripMenuItem_TaskQuery.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItem_TaskQuery.Text = "任務查詢";
            this.ToolStripMenuItem_TaskQuery.Click += new System.EventHandler(this.ToolStripMenuItem_TaskQuery_Click);
            // 
            // ToolStripMenuItem_CraneHandle
            // 
            this.ToolStripMenuItem_CraneHandle.Name = "ToolStripMenuItem_CraneHandle";
            this.ToolStripMenuItem_CraneHandle.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItem_CraneHandle.Text = "堆垛機管理";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 560);
            this.Controls.Add(this.pnlTab);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Main";
            this.Text = "倉儲調度監控系統";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.Shown += new System.EventHandler(this.Main_Shown);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsMain)).EndInit();
            this.pnlTab.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem taskToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inStockToolStripMenuItem;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Monitor;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Cell;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_InStockTask;
        private System.Windows.Forms.ToolStripButton toolStripButton_Close;
        private System.Windows.Forms.Panel pnlTab;
        private System.Windows.Forms.TabControl tabForm;
        private System.Windows.Forms.ToolStripButton toolStripButton_StartCrane;
        private System.Windows.Forms.ToolStripButton toolStripButton_CellMonitor;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemDelCraneTask;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemCellCode;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemReassign;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemStateChange;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem10;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem11;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem12;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem13;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem14;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem17;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem19;
        private System.Windows.Forms.BindingSource bsMain;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem15;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem16;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem18;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvMain;
        private System.Windows.Forms.ListView lbLog;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ToolStripButton toolStripButton_TaskQuery;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column5;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column1;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column11;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn colPalletCode;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn colCraneNo;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column3;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn colFromStation;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn colToStation;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column13;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column8;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_TaskQuery;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_CraneHandle;
    }
}