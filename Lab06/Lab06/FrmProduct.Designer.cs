namespace Lab06
{
    partial class FrmProduct
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileExport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuManage = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuManageAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuManageEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuManageDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tlsActions = new System.Windows.Forms.ToolStrip();
            this.btnToolAdd = new System.Windows.Forms.ToolStripButton();
            this.btnToolEdit = new System.Windows.Forms.ToolStripButton();
            this.btnToolDelete = new System.Windows.Forms.ToolStripButton();
            this.btnToolRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.txtSearchKeyword = new System.Windows.Forms.ToolStripTextBox();
            this.btnToolSearch = new System.Windows.Forms.ToolStripButton();
            this.stStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblProductCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblInventoryValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpProductInfo = new System.Windows.Forms.GroupBox();
            this.btnSelectImage = new System.Windows.Forms.Button();
            this.picProduct = new System.Windows.Forms.PictureBox();
            this.chkConKinhDoanh = new System.Windows.Forms.CheckBox();
            this.dtpNgayNhap = new System.Windows.Forms.DateTimePicker();
            this.numSoLuong = new System.Windows.Forms.NumericUpDown();
            this.numDonGia = new System.Windows.Forms.NumericUpDown();
            this.cboLoaiSP = new System.Windows.Forms.ComboBox();
            this.txtTenSP = new System.Windows.Forms.TextBox();
            this.txtMaSP = new System.Windows.Forms.TextBox();
            this.lblNgayNhap = new System.Windows.Forms.Label();
            this.lblSoLuong = new System.Windows.Forms.Label();
            this.lblDonGia = new System.Windows.Forms.Label();
            this.lblLoaiSP = new System.Windows.Forms.Label();
            this.lblTenSP = new System.Windows.Forms.Label();
            this.lblMaSP = new System.Windows.Forms.Label();
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.ctxGridMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxViewDetail = new System.Windows.Forms.ToolStripMenuItem();
            this.errValidator = new System.Windows.Forms.ErrorProvider(this.components);
            this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.dlgSaveFile = new System.Windows.Forms.SaveFileDialog();
            this.mnuMain.SuspendLayout();
            this.tlsActions.SuspendLayout();
            this.stStrip.SuspendLayout();
            this.grpProductInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picProduct)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSoLuong)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDonGia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.ctxGridMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errValidator)).BeginInit();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.mnuMain.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuManage,
            this.mnuHelp});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Padding = new System.Windows.Forms.Padding(6, 2, 0, 2);
            this.mnuMain.Size = new System.Drawing.Size(1004, 25);
            this.mnuMain.TabIndex = 0;
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileExport,
            this.toolStripSeparator2,
            this.mnuFileExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(39, 21);
            this.mnuFile.Text = "&File";
            // 
            // mnuFileExport
            // 
            this.mnuFileExport.Name = "mnuFileExport";
            this.mnuFileExport.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.mnuFileExport.Size = new System.Drawing.Size(206, 22);
            this.mnuFileExport.Text = "&Xuất file .txt...";
            this.mnuFileExport.Click += new System.EventHandler(this.mnuFileExport_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(203, 6);
            // 
            // mnuFileExit
            // 
            this.mnuFileExit.Name = "mnuFileExit";
            this.mnuFileExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.mnuFileExit.Size = new System.Drawing.Size(206, 22);
            this.mnuFileExit.Text = "&Thoát";
            this.mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
            // 
            // mnuManage
            // 
            this.mnuManage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuManageAdd,
            this.mnuManageEdit,
            this.mnuManageDelete});
            this.mnuManage.Name = "mnuManage";
            this.mnuManage.Size = new System.Drawing.Size(66, 21);
            this.mnuManage.Text = "&Quản lý";
            // 
            // mnuManageAdd
            // 
            this.mnuManageAdd.Name = "mnuManageAdd";
            this.mnuManageAdd.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.mnuManageAdd.Size = new System.Drawing.Size(161, 22);
            this.mnuManageAdd.Text = "&Thêm";
            this.mnuManageAdd.Click += new System.EventHandler(this.TriggerAddLogic);
            // 
            // mnuManageEdit
            // 
            this.mnuManageEdit.Name = "mnuManageEdit";
            this.mnuManageEdit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.mnuManageEdit.Size = new System.Drawing.Size(161, 22);
            this.mnuManageEdit.Text = "&Sửa";
            this.mnuManageEdit.Click += new System.EventHandler(this.TriggerEditLogic);
            // 
            // mnuManageDelete
            // 
            this.mnuManageDelete.Name = "mnuManageDelete";
            this.mnuManageDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.mnuManageDelete.Size = new System.Drawing.Size(161, 22);
            this.mnuManageDelete.Text = "&Xóa";
            this.mnuManageDelete.Click += new System.EventHandler(this.TriggerDeleteLogic);
            // 
            // mnuHelp
            // 
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHelpAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(69, 21);
            this.mnuHelp.Text = "&Trợ giúp";
            // 
            // mnuHelpAbout
            // 
            this.mnuHelpAbout.Name = "mnuHelpAbout";
            this.mnuHelpAbout.Size = new System.Drawing.Size(148, 22);
            this.mnuHelpAbout.Text = "Thông &tin...";
            this.mnuHelpAbout.Click += new System.EventHandler(this.mnuHelpAbout_Click);
            // 
            // tlsActions
            // 
            this.tlsActions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.tlsActions.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tlsActions.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnToolAdd,
            this.btnToolEdit,
            this.btnToolDelete,
            this.btnToolRefresh,
            this.toolStripSeparator1,
            this.txtSearchKeyword,
            this.btnToolSearch});
            this.tlsActions.Location = new System.Drawing.Point(0, 25);
            this.tlsActions.Name = "tlsActions";
            this.tlsActions.Padding = new System.Windows.Forms.Padding(12, 4, 12, 4);
            this.tlsActions.Size = new System.Drawing.Size(1004, 33);
            this.tlsActions.TabIndex = 1;
            // 
            // btnToolAdd
            // 
            this.btnToolAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnToolAdd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.btnToolAdd.Name = "btnToolAdd";
            this.btnToolAdd.Size = new System.Drawing.Size(43, 22);
            this.btnToolAdd.Text = "Thêm";
            this.btnToolAdd.Click += new System.EventHandler(this.TriggerAddLogic);
            // 
            // btnToolEdit
            // 
            this.btnToolEdit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnToolEdit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.btnToolEdit.Name = "btnToolEdit";
            this.btnToolEdit.Size = new System.Drawing.Size(32, 22);
            this.btnToolEdit.Text = "Sửa";
            this.btnToolEdit.Click += new System.EventHandler(this.TriggerEditLogic);
            // 
            // btnToolDelete
            // 
            this.btnToolDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnToolDelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.btnToolDelete.Name = "btnToolDelete";
            this.btnToolDelete.Size = new System.Drawing.Size(32, 22);
            this.btnToolDelete.Text = "Xóa";
            this.btnToolDelete.Click += new System.EventHandler(this.TriggerDeleteLogic);
            // 
            // btnToolRefresh
            // 
            this.btnToolRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnToolRefresh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.btnToolRefresh.Name = "btnToolRefresh";
            this.btnToolRefresh.Size = new System.Drawing.Size(58, 22);
            this.btnToolRefresh.Text = "Làm mới";
            this.btnToolRefresh.Click += new System.EventHandler(this.TriggerResetLogic);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // txtSearchKeyword
            // 
            this.txtSearchKeyword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearchKeyword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSearchKeyword.Name = "txtSearchKeyword";
            this.txtSearchKeyword.Size = new System.Drawing.Size(220, 25);
            this.txtSearchKeyword.ToolTipText = "Nhập mã hoặc tên sản phẩm...";
            this.txtSearchKeyword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchKeyword_KeyDown);
            // 
            // btnToolSearch
            // 
            this.btnToolSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnToolSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.btnToolSearch.Name = "btnToolSearch";
            this.btnToolSearch.Size = new System.Drawing.Size(60, 22);
            this.btnToolSearch.Text = "Tìm kiếm";
            this.btnToolSearch.Click += new System.EventHandler(this.TriggerSearchLogic);
            // 
            // stStrip
            // 
            this.stStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.stStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.stStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblProductCount,
            this.lblInventoryValue});
            this.stStrip.Location = new System.Drawing.Point(0, 615);
            this.stStrip.Name = "stStrip";
            this.stStrip.Size = new System.Drawing.Size(1004, 26);
            this.stStrip.TabIndex = 2;
            // 
            // lblStatus
            // 
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(597, 21);
            this.lblStatus.Spring = true;
            this.lblStatus.Text = "Hệ thống sẵn sàng làm việc.";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblProductCount
            // 
            this.lblProductCount.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblProductCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblProductCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblProductCount.Name = "lblProductCount";
            this.lblProductCount.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblProductCount.Size = new System.Drawing.Size(133, 21);
            this.lblProductCount.Text = "Tổng sản phẩm: 0";
            // 
            // lblInventoryValue
            // 
            this.lblInventoryValue.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblInventoryValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblInventoryValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(83)))), ((int)(((byte)(9)))));
            this.lblInventoryValue.Name = "lblInventoryValue";
            this.lblInventoryValue.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblInventoryValue.Size = new System.Drawing.Size(127, 21);
            this.lblInventoryValue.Text = "Giá trị kho: 0 VND";
            // 
            // grpProductInfo
            // 
            this.grpProductInfo.BackColor = System.Drawing.Color.White;
            this.grpProductInfo.Controls.Add(this.btnSelectImage);
            this.grpProductInfo.Controls.Add(this.picProduct);
            this.grpProductInfo.Controls.Add(this.chkConKinhDoanh);
            this.grpProductInfo.Controls.Add(this.dtpNgayNhap);
            this.grpProductInfo.Controls.Add(this.numSoLuong);
            this.grpProductInfo.Controls.Add(this.numDonGia);
            this.grpProductInfo.Controls.Add(this.cboLoaiSP);
            this.grpProductInfo.Controls.Add(this.txtTenSP);
            this.grpProductInfo.Controls.Add(this.txtMaSP);
            this.grpProductInfo.Controls.Add(this.lblNgayNhap);
            this.grpProductInfo.Controls.Add(this.lblSoLuong);
            this.grpProductInfo.Controls.Add(this.lblDonGia);
            this.grpProductInfo.Controls.Add(this.lblLoaiSP);
            this.grpProductInfo.Controls.Add(this.lblTenSP);
            this.grpProductInfo.Controls.Add(this.lblMaSP);
            this.grpProductInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpProductInfo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpProductInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.grpProductInfo.Location = new System.Drawing.Point(0, 58);
            this.grpProductInfo.Name = "grpProductInfo";
            this.grpProductInfo.Padding = new System.Windows.Forms.Padding(16);
            this.grpProductInfo.Size = new System.Drawing.Size(1004, 180);
            this.grpProductInfo.TabIndex = 3;
            this.grpProductInfo.TabStop = false;
            this.grpProductInfo.Text = "THÔNG TIN SẢN PHẨM";
            // 
            // btnSelectImage
            // 
            this.btnSelectImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.btnSelectImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectImage.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSelectImage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.btnSelectImage.Location = new System.Drawing.Point(850, 138);
            this.btnSelectImage.Name = "btnSelectImage";
            this.btnSelectImage.Size = new System.Drawing.Size(120, 28);
            this.btn