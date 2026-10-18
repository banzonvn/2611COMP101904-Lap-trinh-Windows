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
            this.mnuManageAdd.Click += new System.EventHandler(this.ExecuteAddAction);
            // 
            // mnuManageEdit
            // 
            this.mnuManageEdit.Name = "mnuManageEdit";
            this.mnuManageEdit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.mnuManageEdit.Size = new System.Drawing.Size(161, 22);
            this.mnuManageEdit.Text = "&Sửa";
            this.mnuManageEdit.Click += new System.EventHandler(this.ExecuteEditAction);
            // 
            // mnuManageDelete
            // 
            this.mnuManageDelete.Name = "mnuManageDelete";
            this.mnuManageDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.mnuManageDelete.Size = new System.Drawing.Size(161, 22);
            this.mnuManageDelete.Text = "&Xóa";
            this.mnuManageDelete.Click += new System.EventHandler(this.ExecuteDeleteAction);
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
            this.tlsActions.Padding = new System.Windows.Forms.Padding(10, 3, 1, 3);
            this.tlsActions.Size = new System.Drawing.Size(1004, 31);
            this.tlsActions.TabIndex = 1;
            // 
            // btnToolAdd
            // 
            this.btnToolAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnToolAdd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.btnToolAdd.Name = "btnToolAdd";
            this.btnToolAdd.Size = new System.Drawing.Size(43, 22);
            this.btnToolAdd.Text = "Thêm";
            this.btnToolAdd.Click += new System.EventHandler(this.ExecuteAddAction);
            // 
            // btnToolEdit
            // 
            this.btnToolEdit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnToolEdit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.btnToolEdit.Name = "btnToolEdit";
            this.btnToolEdit.Size = new System.Drawing.Size(32, 22);
            this.btnToolEdit.Text = "Sửa";
            this.btnToolEdit.Click += new System.EventHandler(this.ExecuteEditAction);
            // 
            // btnToolDelete
            // 
            this.btnToolDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnToolDelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.btnToolDelete.Name = "btnToolDelete";
            this.btnToolDelete.Size = new System.Drawing.Size(32, 22);
            this.btnToolDelete.Text = "Xóa";
            this.btnToolDelete.Click += new System.EventHandler(this.ExecuteDeleteAction);
            // 
            // btnToolRefresh
            // 
            this.btnToolRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnToolRefresh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.btnToolRefresh.Name = "btnToolRefresh";
            this.btnToolRefresh.Size = new System.Drawing.Size(58, 22);
            this.btnToolRefresh.Text = "Làm mới";
            this.btnToolRefresh.Click += new System.EventHandler(this.ExecuteRefreshAction);
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
            this.txtSearchKeyword.Size = new System.Drawing.Size(200, 25);
            this.txtSearchKeyword.ToolTipText = "Nhập mã hoặc tên sản phẩm cần tìm...";
            this.txtSearchKeyword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchKeyword_KeyDown);
            // 
            // btnToolSearch
            // 
            this.btnToolSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnToolSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.btnToolSearch.Name = "btnToolSearch";
            this.btnToolSearch.Size = new System.Drawing.Size(60, 22);
            this.btnToolSearch.Text = "Tìm kiếm";
            this.btnToolSearch.Click += new System.EventHandler(this.ExecuteSearchAction);
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
            this.grpProductInfo.Location = new System.Drawing.Point(0, 56);
            this.grpProductInfo.Name = "grpProductInfo";
            this.grpProductInfo.Padding = new System.Windows.Forms.Padding(12);
            this.grpProductInfo.Size = new System.Drawing.Size(1004, 195);
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
            this.btnSelectImage.Location = new System.Drawing.Point(835, 153);
            this.btnSelectImage.Name = "btnSelectImage";
            this.btnSelectImage.Size = new System.Drawing.Size(130, 28);
            this.btnSelectImage.TabIndex = 14;
            this.btnSelectImage.Text = "Chọn ảnh...";
            this.btnSelectImage.UseVisualStyleBackColor = false;
            this.btnSelectImage.Click += new System.EventHandler(this.btnSelectImage_Click);
            // 
            // picProduct
            // 
            this.picProduct.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.picProduct.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picProduct.Location = new System.Drawing.Point(835, 23);
            this.picProduct.Name = "picProduct";
            this.picProduct.Size = new System.Drawing.Size(130, 124);
            this.picProduct.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picProduct.TabIndex = 13;
            this.picProduct.TabStop = false;
            // 
            // chkConKinhDoanh
            // 
            this.chkConKinhDoanh.AutoSize = true;
            this.chkConKinhDoanh.Checked = true;
            this.chkConKinhDoanh.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkConKinhDoanh.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.chkConKinhDoanh.Location = new System.Drawing.Point(520, 150);
            this.chkConKinhDoanh.Name = "chkConKinhDoanh";
            this.chkConKinhDoanh.Size = new System.Drawing.Size(117, 21);
            this.chkConKinhDoanh.TabIndex = 12;
            this.chkConKinhDoanh.Text = "Còn kinh doanh";
            this.chkConKinhDoanh.UseVisualStyleBackColor = true;
            // 
            // dtpNgayNhap
            // 
            this.dtpNgayNhap.CustomFormat = "dd/MM/yyyy";
            this.dtpNgayNhap.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dtpNgayNhap.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayNhap.Location = new System.Drawing.Point(520, 108);
            this.dtpNgayNhap.Name = "dtpNgayNhap";
            this.dtpNgayNhap.Size = new System.Drawing.Size(260, 24);
            this.dtpNgayNhap.TabIndex = 11;
            // 
            // numSoLuong
            // 
            this.numSoLuong.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.numSoLuong.Location = new System.Drawing.Point(520, 68);
            this.numSoLuong.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.numSoLuong.Name = "numSoLuong";
            this.numSoLuong.Size = new System.Drawing.Size(260, 24);
            this.numSoLuong.TabIndex = 10;
            // 
            // numDonGia
            // 
            this.numDonGia.DecimalPlaces = 0;
            this.numDonGia.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.numDonGia.Increment = new decimal(new int[] { 50000, 0, 0, 0 });
            this.numDonGia.Location = new System.Drawing.Point(520, 28);
            this.numDonGia.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
            this.numDonGia.Name = "numDonGia";
            this.numDonGia.Size = new System.Drawing.Size(260, 24);
            this.numDonGia.TabIndex = 9;
            this.numDonGia.ThousandsSeparator = true;
            // 
            // cboLoaiSP
            // 
            this.cboLoaiSP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoaiSP.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboLoaiSP.FormattingEnabled = true;
            this.cboLoaiSP.Location = new System.Drawing.Point(125, 108);
            this.cboLoaiSP.Name = "cboLoaiSP";
            this.cboLoaiSP.Size = new System.Drawing.Size(260, 25);
            this.cboLoaiSP.TabIndex = 8;
            // 
            // txtTenSP
            // 
            this.txtTenSP.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtTenSP.Location = new System.Drawing.Point(125, 68);
            this.txtTenSP.Name = "txtTenSP";
            this.txtTenSP.Size = new System.Drawing.Size(260, 24);
            this.txtTenSP.TabIndex = 7;
            // 
            // txtMaSP
            // 
            this.txtMaSP.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtMaSP.Location = new System.Drawing.Point(125, 28);
            this.txtMaSP.Name = "txtMaSP";
            this.txtMaSP.Size = new System.Drawing.Size(260, 24);
            this.txtMaSP.TabIndex = 6;
            // 
            // lblNgayNhap
            // 
            this.lblNgayNhap.AutoSize = true;
            this.lblNgayNhap.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblNgayNhap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblNgayNhap.Location = new System.Drawing.Point(430, 113);
            this.lblNgayNhap.Name = "lblNgayNhap";
            this.lblNgayNhap.Size = new System.Drawing.Size(68, 15);
            this.lblNgayNhap.TabIndex = 5;
            this.lblNgayNhap.Text = "Ngày nhập:";
            // 
            // lblSoLuong
            // 
            this.lblSoLuong.AutoSize = true;
            this.lblSoLuong.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSoLuong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblSoLuong.Location = new System.Drawing.Point(430, 73);
            this.lblSoLuong.Name = "lblSoLuong";
            this.lblSoLuong.Size = new System.Drawing.Size(57, 15);
            this.lblSoLuong.TabIndex = 4;
            this.lblSoLuong.Text = "Số lượng:";
            // 
            // lblDonGia
            // 
            this.lblDonGia.AutoSize = true;
            this.lblDonGia.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDonGia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblDonGia.Location = new System.Drawing.Point(430, 33);
            this.lblDonGia.Name = "lblDonGia";
            this.lblDonGia.Size = new System.Drawing.Size(51, 15);
            this.lblDonGia.TabIndex = 3;
            this.lblDonGia.Text = "Đơn giá:";
            // 
            // lblLoaiSP
            // 
            this.lblLoaiSP.AutoSize = true;
            this.lblLoaiSP.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblLoaiSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblLoaiSP.Location = new System.Drawing.Point(25, 113);
            this.lblLoaiSP.Name = "lblLoaiSP";
            this.lblLoaiSP.Size = new System.Drawing.Size(86, 15);
            this.lblLoaiSP.TabIndex = 2;
            this.lblLoaiSP.Text = "Loại sản phẩm:";
            // 
            // lblTenSP
            // 
            this.lblTenSP.AutoSize = true;
            this.lblTenSP.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTenSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblTenSP.Location = new System.Drawing.Point(25, 73);
            this.lblTenSP.Name = "lblTenSP";
            this.lblTenSP.Size = new System.Drawing.Size(83, 15);
            this.lblTenSP.TabIndex = 1;
            this.lblTenSP.Text = "Tên sản phẩm:";
            // 
            // lblMaSP
            // 
            this.lblMaSP.AutoSize = true;
            this.lblMaSP.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMaSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblMaSP.Location = new System.Drawing.Point(25, 33);
            this.lblMaSP.Name = "lblMaSP";
            this.lblMaSP.Size = new System.Drawing.Size(82, 15);
            this.lblMaSP.TabIndex = 0;
            this.lblMaSP.Text = "Mã sản phẩm:";
            // 
            // dgvProducts
            // 
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.dgvProducts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvProducts.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvProducts.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvProducts.ColumnHeadersHeight = 36;
            this.dgvProducts.ContextMenuStrip = this.ctxGridMenu;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvProducts.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProducts.EnableHeadersVisualStyles = false;
            this.dgvProducts.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.dgvProducts.Location = new System.Drawing.Point(0, 251);
            this.dgvProducts.MultiSelect = false;
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.RowHeadersVisible = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.dgvProducts.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvProducts.RowTemplate.Height = 30;
            this.dgvProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.Size = new System.Drawing.Size(1004, 364);
            this.dgvProducts.TabIndex = 4;
            this.dgvProducts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProducts_CellClick);
            this.dgvProducts.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvProducts_CellMouseDown);
            // 
            // ctxGridMenu
            // 
            this.ctxGridMenu.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.ctxGridMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxEdit,
            this.ctxDelete,
            this.ctxViewDetail});
            this.ctxGridMenu.Name = "ctxGridMenu";
            this.ctxGridMenu.Size = new System.Drawing.Size(147, 70);
            // 
            // ctxEdit
            // 
            this.ctxEdit.Name = "ctxEdit";
            this.ctxEdit.Size = new System.Drawing.Size(146, 22);
            this.ctxEdit.Text = "Sửa";
            this.ctxEdit.Click += new System.EventHandler(this.ExecuteEditAction);
            // 
            // ctxDelete
            // 
            this.ctxDelete.Name = "ctxDelete";
            this.ctxDelete.Size = new System.Drawing.Size(146, 22);
            this.ctxDelete.Text = "Xóa";
            this.ctxDelete.Click += new System.EventHandler(this.ExecuteDeleteAction);
            // 
            // ctxViewDetail
            // 
            this.ctxViewDetail.Name = "ctxViewDetail";
            this.ctxViewDetail.Size = new System.Drawing.Size(146, 22);
            this.ctxViewDetail.Text = "Xem chi tiết";
            this.ctxViewDetail.Click += new System.EventHandler(this.ctxViewDetail_Click);
            // 
            // errValidator
            // 
            this.errValidator.ContainerControl = this;
            // 
            // dlgOpenFile
            // 
            this.dlgOpenFile.Filter = "Ảnh sản phẩm (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            this.dlgOpenFile.Title = "Chọn hình ảnh sản phẩm";
            // 
            // dlgSaveFile
            // 
            this.dlgSaveFile.Filter = "Tài liệu Text (*.txt)|*.txt";
            this.dlgSaveFile.Title = "Lưu danh sách sản phẩm";
            // 
            // FrmProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1004, 641);
            this.Controls.Add(this.dgvProducts);
            this.Controls.Add(this.grpProductInfo);
            this.Controls.Add(this.stStrip);
            this.Controls.Add(this.tlsActions);
            this.Controls.Add(this.mnuMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.MainMenuStrip = this.mnuMain;
            this.MinimumSize = new System.Drawing.Size(950, 600);
            this.Name = "FrmProduct";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý sản phẩm";
            this.Load += new System.EventHandler(this.FrmProduct_Load);
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.tlsActions.ResumeLayout(false);
            this.tlsActions.PerformLayout();
            this.stStrip.ResumeLayout(false);
            this.stStrip.PerformLayout();
            this.grpProductInfo.ResumeLayout(false);
            this.grpProductInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picProduct)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSoLuong)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDonGia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.ctxGridMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errValidator)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuFileExport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuFileExit;
        private System.Windows.Forms.ToolStripMenuItem mnuManage;
        private System.Windows.Forms.ToolStripMenuItem mnuManageAdd;
        private System.Windows.Forms.ToolStripMenuItem mnuManageEdit;
        private System.Windows.Forms.ToolStripMenuItem mnuManageDelete;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mnuHelpAbout;
        private System.Windows.Forms.ToolStrip tlsActions;
        private System.Windows.Forms.ToolStripButton btnToolAdd;
        private System.Windows.Forms.ToolStripButton btnToolEdit;
        private System.Windows.Forms.ToolStripButton btnToolDelete;
        private System.Windows.Forms.ToolStripButton btnToolRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox txtSearchKeyword;
        private System.Windows.Forms.ToolStripButton btnToolSearch;
        private System.Windows.Forms.StatusStrip stStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblProductCount;
        private System.Windows.Forms.ToolStripStatusLabel lblInventoryValue;
        private System.Windows.Forms.GroupBox grpProductInfo;
        private System.Windows.Forms.Label lblLoaiSP;
        private System.Windows.Forms.Label lblTenSP;
        private System.Windows.Forms.Label lblMaSP;
        private System.Windows.Forms.Label lblNgayNhap;
        private System.Windows.Forms.Label lblSoLuong;
        private System.Windows.Forms.Label lblDonGia;
        private System.Windows.Forms.TextBox txtTenSP;
        private System.Windows.Forms.TextBox txtMaSP;
        private System.Windows.Forms.ComboBox cboLoaiSP;
        private System.Windows.Forms.NumericUpDown numSoLuong;
        private System.Windows.Forms.NumericUpDown numDonGia;
        private System.Windows.Forms.DateTimePicker dtpNgayNhap;
        private System.Windows.Forms.CheckBox chkConKinhDoanh;
        private System.Windows.Forms.PictureBox picProduct;
        private System.Windows.Forms.Button btnSelectImage;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.ContextMenuStrip ctxGridMenu;
        private System.Windows.Forms.ToolStripMenuItem ctxEdit;
        private System.Windows.Forms.ToolStripMenuItem ctxDelete;
        private System.Windows.Forms.ToolStripMenuItem ctxViewDetail;
        private System.Windows.Forms.ErrorProvider errValidator;
        private System.Windows.Forms.OpenFileDialog dlgOpenFile;
        private System.Windows.Forms.SaveFileDialog dlgSaveFile;
    }
}