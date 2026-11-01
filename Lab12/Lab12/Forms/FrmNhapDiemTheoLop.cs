using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using QLSinhVienLab12.DAL;
using QLSinhVienLab12.Models;

namespace QLSinhVienLab12.Forms
{
    public enum AlertType { Success, Warning, Error, Info }

    public class FrmNhapDiemTheoLop : Form
    {
        // Core DAL and BindingSource as required by Lab 12
        private readonly KetQuaDAL ketQuaDAL = new();
        private readonly BindingSource ketQuaSource = new();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool SuppressMessageBox { get; set; } = false;

        // Required UI Controls
        public ComboBox cboLop = null!;
        public ComboBox cboMonHoc = null!;
        public Button btnTaiDanhSach = null!;
        public DataGridView dgvKetQua = null!;
        public Button btnLuuTatCa = null!;

        // Advanced, Polish & Distinctive Controls
        public Button btnRollbackDemo = null!;
        public Button btnResetSample = null!;
        public Button btnTestConn = null!;

        private Panel _pnlHeader = null!;
        private Label _lblHeaderTitle = null!;
        private Label _lblHeaderSub = null!;
        private Label _lblStudentName = null!;
        private Label _lblStudentClass = null!;

        private Panel _pnlToolbarContainer = null!;

        private Panel _pnlAlert = null!;
        private Label _lblAlertIcon = null!;
        private Label _lblAlertText = null!;

        private Panel _pnlCenter = null!;
        private Panel _pnlStatsCard = null!;
        private TableLayoutPanel _tlpStats = null!;
        private Label _lblStatCount = null!;
        private Label _lblStatAvg = null!;
        private Label _lblStatPass = null!;
        private Label _lblStatHighLow = null!;

        private TabControl _tabBottom = null!;
        private TabPage _tabAudit = null!;
        private RichTextBox _rtbAuditLog = null!;

        private StatusStrip _statusStrip = null!;
        private ToolStripStatusLabel _tslStatus = null!;
        private ToolStripStatusLabel _tslBinding = null!;
        private ToolStripStatusLabel _tslEngine = null!;

        public FrmNhapDiemTheoLop()
        {
            InitializeComponent();
            ketQuaDAL.OnTransactionLog += HandleTransactionLog;
        }

        private void InitializeComponent()
        {
            this.Text = "Quản lý Kết quả học tập - Data Binding & SqlTransaction | COMP1019 Lab 12 - Lê Hoàng Quân";
            this.Size = new Size(1150, 780);
            this.MinimumSize = new Size(920, 620);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            this.BackColor = Color.FromArgb(248, 250, 252); // Soft Light Background

            // ================================================================
            // 1. TOP HEADER PANEL (TableLayoutPanel - Responsive, NO Overlap)
            // ================================================================
            _pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 74,
                BackColor = Color.White,
                Padding = new Padding(20, 10, 20, 10)
            };
            _pnlHeader.Paint += PnlHeader_Paint;

            var tlpHeader = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 56f));
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 44f));

            var pnlHeaderLeft = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };

            _lblHeaderTitle = new Label
            {
                Text = "QUẢN LÝ KẾT QUẢ HỌC TẬP (DATA BINDING & TRANSACTION)",
                ForeColor = Color.FromArgb(15, 23, 42), // Deep Slate
                Font = new Font("Segoe UI", 12.0f, FontStyle.Bold),
                Location = new Point(0, 2),
                AutoSize = true,
                UseMnemonic = false
            };

            _lblHeaderSub = new Label
            {
                Text = "COMP1019 - Lập trình trên Windows | ThS. Lê Thanh Thoại - HCMUE",
                ForeColor = Color.FromArgb(100, 116, 139), // Slate 500
                Font = new Font("Segoe UI", 8.8f),
                Location = new Point(0, 28),
                AutoSize = true,
                UseMnemonic = false
            };
            pnlHeaderLeft.Controls.Add(_lblHeaderTitle);
            pnlHeaderLeft.Controls.Add(_lblHeaderSub);

            var pnlHeaderRight = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };

            _lblStudentName = new Label
            {
                Text = "Sinh viên: Lê Hoàng Quân  |  MSSV: 51.01.104.082",
                ForeColor = Color.FromArgb(2, 132, 199), // Clean Sky Blue 600
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 24,
                TextAlign = ContentAlignment.MiddleRight,
                UseMnemonic = false
            };

            _lblStudentClass = new Label
            {
                Text = "Lớp: 51.CNTT.A  |  Nhóm: 4  |  Học phần Buổi 12",
                ForeColor = Color.FromArgb(71, 85, 105), // Slate 600
                Font = new Font("Segoe UI", 8.8f),
                Dock = DockStyle.Top,
                Height = 22,
                TextAlign = ContentAlignment.MiddleRight,
                UseMnemonic = false
            };
            pnlHeaderRight.Controls.Add(_lblStudentClass);
            pnlHeaderRight.Controls.Add(_lblStudentName);

            tlpHeader.Controls.Add(pnlHeaderLeft, 0, 0);
            tlpHeader.Controls.Add(pnlHeaderRight, 1, 0);
            _pnlHeader.Controls.Add(tlpHeader);

            // ================================================================
            // 2. TOOLBAR CONTAINER (2 Distinct Rows - Zero Squishing / NO Overlap)
            // ================================================================
            _pnlToolbarContainer = new Panel
            {
                Dock = DockStyle.Top,
                Height = 94,
                BackColor = Color.White,
                Padding = new Padding(20, 10, 20, 10)
            };

            // Row 1: Filter Row (Lớp, Môn học, Tải Danh Sách)
            var pnlRow1 = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 34,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoScroll = false,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };

            var lblLop = new Label
            {
                Text = "Chọn lớp:",
                Font = new Font("Segoe UI", 9.2f, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 65, 85),
                Margin = new Padding(0, 5, 8, 0),
                AutoSize = true
            };

            cboLop = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 190,
                Font = new Font("Segoe UI", 9.2f),
                Margin = new Padding(0, 2, 24, 0)
            };

            var lblMonHoc = new Label
            {
                Text = "Chọn môn học:",
                Font = new Font("Segoe UI", 9.2f, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 65, 85),
                Margin = new Padding(0, 5, 8, 0),
                AutoSize = true
            };

            cboMonHoc = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 220,
                Font = new Font("Segoe UI", 9.2f),
                Margin = new Padding(0, 2, 20, 0)
            };

            btnTaiDanhSach = new Button
            {
                Text = "📥 Tải Danh Sách",
                BackColor = Color.FromArgb(224, 242, 254), // Soft sky-blue
                ForeColor = Color.FromArgb(3, 105, 161),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.0f, FontStyle.Bold),
                Size = new Size(170, 32),
                Margin = new Padding(0, 1, 0, 0),
                Cursor = Cursors.Hand
            };
            btnTaiDanhSach.FlatAppearance.BorderColor = Color.FromArgb(186, 230, 253);
            btnTaiDanhSach.FlatAppearance.BorderSize = 1;
            btnTaiDanhSach.Click += btnTaiDanhSach_Click;

            pnlRow1.Controls.AddRange(new Control[]
            {
                lblLop, cboLop,
                lblMonHoc, cboMonHoc,
                btnTaiDanhSach
            });

            // Row 2: Action Row (Lưu Transaction, Rollback Demo, Làm Mới, Kiểm Tra CSDL)
            var pnlRow2 = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 34,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoScroll = false,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };

            btnLuuTatCa = new Button
            {
                Text = "💾 Lưu Tất Cả",
                BackColor = Color.FromArgb(236, 253, 245), // Soft emerald
                ForeColor = Color.FromArgb(4, 120, 87),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.0f, FontStyle.Bold),
                Size = new Size(155, 32),
                Margin = new Padding(0, 0, 12, 0),
                Cursor = Cursors.Hand
            };
            btnLuuTatCa.FlatAppearance.BorderColor = Color.FromArgb(167, 243, 208);
            btnLuuTatCa.FlatAppearance.BorderSize = 1;
            btnLuuTatCa.Click += btnLuuTatCa_Click;

            btnRollbackDemo = new Button
            {
                Text = "🧪 Thử Rollback",
                BackColor = Color.FromArgb(255, 241, 242), // Soft rose
                ForeColor = Color.FromArgb(190, 18, 60),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.0f, FontStyle.Bold),
                Size = new Size(165, 32),
                Margin = new Padding(0, 0, 12, 0),
                Cursor = Cursors.Hand
            };
            btnRollbackDemo.FlatAppearance.BorderColor = Color.FromArgb(254, 205, 211);
            btnRollbackDemo.FlatAppearance.BorderSize = 1;
            btnRollbackDemo.Click += btnRollbackDemo_Click;

            btnResetSample = new Button
            {
                Text = "⟲ Làm Mới",
                BackColor = Color.FromArgb(241, 245, 249), // Soft slate
                ForeColor = Color.FromArgb(71, 85, 105),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.0f, FontStyle.Bold),
                Size = new Size(135, 32),
                Margin = new Padding(0, 0, 12, 0),
                Cursor = Cursors.Hand
            };
            btnResetSample.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnResetSample.FlatAppearance.BorderSize = 1;
            btnResetSample.Click += btnResetSample_Click;

            btnTestConn = new Button
            {
                Text = "⚡ Kiểm Tra DB",
                BackColor = Color.FromArgb(248, 250, 252),
                ForeColor = Color.FromArgb(30, 41, 59),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.0f, FontStyle.Bold),
                Size = new Size(155, 32),
                Margin = new Padding(0),
                Cursor = Cursors.Hand
            };
            btnTestConn.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnTestConn.FlatAppearance.BorderSize = 1;
            btnTestConn.Click += (s, e) => CheckConnectionLive();

            pnlRow2.Controls.AddRange(new Control[]
            {
                btnLuuTatCa,
                btnRollbackDemo,
                btnResetSample,
                btnTestConn
            });

            _pnlToolbarContainer.Controls.Add(pnlRow2);
            _pnlToolbarContainer.Controls.Add(pnlRow1);

            // ================================================================
            // 3. ALERT BANNER PANEL (Soft colors, clean message)
            // ================================================================
            _pnlAlert = new Panel
            {
                Dock = DockStyle.Top,
                Height = 36,
                BackColor = Color.FromArgb(236, 253, 245), // Default soft emerald
                Visible = false,
                Padding = new Padding(20, 6, 20, 6)
            };

            _lblAlertIcon = new Label
            {
                Text = "✔",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(4, 120, 87),
                Location = new Point(20, 8),
                AutoSize = true,
                UseMnemonic = false
            };

            _lblAlertText = new Label
            {
                Text = "Thông báo hệ thống",
                Font = new Font("Segoe UI", 9.2f, FontStyle.Bold),
                ForeColor = Color.FromArgb(6, 95, 70),
                Location = new Point(46, 8),
                AutoSize = true,
                UseMnemonic = false
            };
            _pnlAlert.Controls.Add(_lblAlertIcon);
            _pnlAlert.Controls.Add(_lblAlertText);

            // ================================================================
            // 4. CENTER WORKSPACE (Data Grid + Statistics + Audit Tab)
            // ================================================================
            _pnlCenter = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 10, 20, 10)
            };

            // DataGridView Setup
            dgvKetQua = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.CellSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                EnableHeadersVisualStyles = false,
                EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2
            };

            dgvKetQua.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(241, 245, 249), // Soft Slate
                ForeColor = Color.FromArgb(30, 41, 59),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(8)
            };

            dgvKetQua.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.FromArgb(15, 23, 42),
                Font = new Font("Segoe UI", 9.3f),
                Padding = new Padding(6),
                SelectionBackColor = Color.FromArgb(224, 242, 254),
                SelectionForeColor = Color.FromArgb(12, 74, 110)
            };

            dgvKetQua.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 250, 252)
            };
            dgvKetQua.RowTemplate.Height = 34;

            // Configure columns explicitly
            SetupGridColumns();

            // Real-time calculation on cell edit
            dgvKetQua.CellEndEdit += (s, e) => UpdateStatistics();
            dgvKetQua.CellFormatting += DgvKetQua_CellFormatting;
            dgvKetQua.DataError += (s, e) => { e.ThrowException = false; };

            // Stats Card (TableLayoutPanel: Fixed cells - NEVER overlaps text!)
            _pnlStatsCard = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 64,
                BackColor = Color.White,
                Padding = new Padding(12, 8, 12, 8)
            };

            _tlpStats = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            _tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            _tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            _tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            _tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));

            _lblStatCount = new Label
            {
                Text = "👥 Sĩ số: 0 sinh viên",
                ForeColor = Color.FromArgb(2, 132, 199), // Sky Blue
                Font = new Font("Segoe UI", 9.2f, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            _lblStatAvg = new Label
            {
                Text = "📊 Điểm TB: -- / 10.0",
                ForeColor = Color.FromArgb(5, 150, 105), // Emerald
                Font = new Font("Segoe UI", 9.2f, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            _lblStatPass = new Label
            {
                Text = "🎯 Tỉ lệ đạt (≥ 5.0): --",
                ForeColor = Color.FromArgb(13, 148, 136), // Teal
                Font = new Font("Segoe UI", 9.2f, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            _lblStatHighLow = new Label
            {
                Text = "⭐ Cao nhất / Thấp nhất: -- / --",
                ForeColor = Color.FromArgb(217, 119, 6), // Warm Amber
                Font = new Font("Segoe UI", 9.2f, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            _tlpStats.Controls.Add(_lblStatCount, 0, 0);
            _tlpStats.Controls.Add(_lblStatAvg, 1, 0);
            _tlpStats.Controls.Add(_lblStatPass, 2, 0);
            _tlpStats.Controls.Add(_lblStatHighLow, 3, 0);
            _pnlStatsCard.Controls.Add(_tlpStats);

            // Audit Tab (Transaction steps log)
            _tabBottom = new TabControl
            {
                Dock = DockStyle.Bottom,
                Height = 160,
                Font = new Font("Segoe UI", 9.2f)
            };

            _tabAudit = new TabPage("📋 Nhật Ký Giao Dịch & Stored Procedure (SqlTransaction Monitor)");

            _rtbAuditLog = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9.2f),
                BackColor = Color.FromArgb(248, 250, 252),
                ForeColor = Color.FromArgb(30, 41, 59),
                BorderStyle = BorderStyle.None,
                ReadOnly = true
            };
            _tabAudit.Controls.Add(_rtbAuditLog);
            _tabBottom.TabPages.Add(_tabAudit);

            var splitterBottom = new Splitter { Dock = DockStyle.Bottom, Height = 6, BackColor = Color.FromArgb(241, 245, 249) };

            _pnlCenter.Controls.Add(dgvKetQua);
            _pnlCenter.Controls.Add(_pnlStatsCard);
            _pnlCenter.Controls.Add(splitterBottom);
            _pnlCenter.Controls.Add(_tabBottom);

            // ================================================================
            // 5. STATUS STRIP (Bottom footer)
            // ================================================================
            _statusStrip = new StatusStrip
            {
                BackColor = Color.FromArgb(241, 245, 249),
                Font = new Font("Segoe UI", 9.0f),
                Padding = new Padding(12, 4, 12, 4)
            };

            _tslStatus = new ToolStripStatusLabel("Hệ thống đã sẵn sàng.")
            {
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0, 0, 16, 0)
            };
            _tslBinding = new ToolStripStatusLabel("Nạp: Sẵn sàng")
            {
                BorderSides = ToolStripStatusLabelBorderSides.Left,
                BorderStyle = Border3DStyle.Etched,
                Padding = new Padding(12, 0, 12, 0),
                Margin = new Padding(0)
            };
            _tslEngine = new ToolStripStatusLabel("Động cơ: In-Memory Mock")
            {
                BorderSides = ToolStripStatusLabelBorderSides.Left,
                BorderStyle = Border3DStyle.Etched,
                ForeColor = Color.FromArgb(30, 64, 175),
                Font = new Font("Segoe UI", 9.0f, FontStyle.Bold),
                Padding = new Padding(12, 0, 12, 0),
                Margin = new Padding(0)
            };

            _statusStrip.Items.AddRange(new ToolStripItem[] { _tslStatus, _tslBinding, _tslEngine });

            // Add all main components
            this.Controls.Add(_pnlCenter);
            this.Controls.Add(_pnlAlert);
            this.Controls.Add(_pnlToolbarContainer);
            this.Controls.Add(_pnlHeader);
            this.Controls.Add(_statusStrip);

            this.Load += FrmNhapDiemTheoLop_Load;
        }

        private void PnlHeader_Paint(object? sender, PaintEventArgs e)
        {
            // Subtle 3px top accent bar
            using var brushAccent = new SolidBrush(Color.FromArgb(2, 132, 199)); // Soft Sky Blue
            e.Graphics.FillRectangle(brushAccent, 0, 0, _pnlHeader.Width, 3);

            // 1px subtle bottom divider
            using var penBottom = new Pen(Color.FromArgb(226, 232, 240));
            e.Graphics.DrawLine(penBottom, 0, _pnlHeader.Height - 1, _pnlHeader.Width, _pnlHeader.Height - 1);
        }

        private void SetupGridColumns()
        {
            dgvKetQua.AutoGenerateColumns = false;
            dgvKetQua.Columns.Clear();
            dgvKetQua.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvKetQua.ColumnHeadersHeight = 36;

            var colMaSV = new DataGridViewTextBoxColumn
            {
                Name = "MaSV",
                DataPropertyName = "MaSV",
                HeaderText = "Mã Sinh Viên",
                FillWeight = 20,
                ReadOnly = true
            };

            var colHoTen = new DataGridViewTextBoxColumn
            {
                Name = "HoTen",
                DataPropertyName = "HoTen",
                HeaderText = "Họ và Tên Sinh Viên",
                FillWeight = 38,
                ReadOnly = true
            };

            var colDiem = new DataGridViewTextBoxColumn
            {
                Name = "Diem",
                DataPropertyName = "Diem",
                HeaderText = "Điểm Thi (0 - 10)",
                FillWeight = 20,
                ReadOnly = false
            };
            colDiem.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colDiem.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            var colGhiChu = new DataGridViewTextBoxColumn
            {
                Name = "GhiChu",
                DataPropertyName = "GhiChu",
                HeaderText = "Ghi Chú Đánh Giá",
                FillWeight = 28,
                ReadOnly = false
            };

            dgvKetQua.Columns.AddRange(new DataGridViewColumn[] { colMaSV, colHoTen, colDiem, colGhiChu });
        }

        private void FrmNhapDiemTheoLop_Load(object? sender, EventArgs e)
        {
            CheckConnectionLive();
            LoadComboBoxes();

            // Link BindingSource to DataGridView as required by Lab 12
            dgvKetQua.DataSource = ketQuaSource;

            // Auto-load first class & subject
            if (cboLop.Items.Count > 0 && cboMonHoc.Items.Count > 0)
            {
                cboLop.SelectedIndex = 0;
                cboMonHoc.SelectedIndex = 0;
                LoadKetQua();
            }
        }

        public void CheckConnectionLive()
        {
            bool isConnected = ketQuaDAL.CheckConnection();
            if (isConnected)
            {
                _tslEngine.Text = "Động cơ: SQL Server 🟢";
                _tslEngine.ForeColor = Color.FromArgb(4, 120, 87);
                ShowAlert("Kết nối thành công đến Microsoft SQL Server (Database: QLSinhVienLab12).", AlertType.Success);
            }
            else
            {
                _tslEngine.Text = "Động cơ: In-Memory Mock 🟡";
                _tslEngine.ForeColor = Color.FromArgb(180, 83, 9);
                ShowAlert("Đang chạy ở chế độ In-Memory Fallback Engine (Mô phỏng đầy đủ Stored Procedure & Transaction).", AlertType.Info);
            }
        }

        public void LoadComboBoxes()
        {
            try
            {
                // 1. Load Classes into cboLop
                DataTable dtLop = ketQuaDAL.GetLop();
                cboLop.DataSource = dtLop;
                cboLop.DisplayMember = "TenLop";
                cboLop.ValueMember = "MaLop";

                // 2. Load Subjects into cboMonHoc
                DataTable dtMon = ketQuaDAL.GetMonHoc();
                cboMonHoc.DataSource = dtMon;
                cboMonHoc.DisplayMember = "TenMon";
                cboMonHoc.ValueMember = "MaMon";

                _tslBinding.Text = $"Nạp: {dtLop.Rows.Count} lớp, {dtMon.Rows.Count} môn";
            }
            catch (Exception ex)
            {
                Notify("Lỗi tải danh mục: " + ex.Message, "Lỗi nạp dữ liệu", MessageBoxIcon.Error, AlertType.Error);
            }
        }

        public void LoadKetQua()
        {
            string maLop = cboLop.SelectedValue?.ToString() ?? "";
            string maMon = cboMonHoc.SelectedValue?.ToString() ?? "";

            if (string.IsNullOrEmpty(maLop) || string.IsNullOrEmpty(maMon))
            {
                ShowAlert("Vui lòng chọn lớp học và môn học hợp lệ.", AlertType.Warning);
                return;
            }

            try
            {
                // Call stored procedure sp_KetQua_GetByLopMon via DAL
                DataTable dt = ketQuaDAL.GetKetQuaByLopMon(maLop, maMon);

                // Bind to BindingSource
                ketQuaSource.DataSource = dt;

                UpdateStatistics();

                string lopText = cboLop.Text;
                string monText = cboMonHoc.Text;
                ShowAlert($"Đã tải danh sách {dt.Rows.Count} sinh viên cho lớp [{lopText}] - môn [{monText}].", AlertType.Info);
                _tslStatus.Text = $"Lớp: {maLop}  |  Môn: {maMon}  |  Sĩ số: {dt.Rows.Count} sinh viên";
            }
            catch (Exception ex)
            {
                Notify("Lỗi tải kết quả: " + ex.Message, "Lỗi thực thi", MessageBoxIcon.Error, AlertType.Error);
            }
        }

        public void btnTaiDanhSach_Click(object? sender, EventArgs e)
        {
            LoadKetQua();
        }

        public void btnLuuTatCa_Click(object? sender, EventArgs e)
        {
            try
            {
                // Flush edits from grid to DataTable
                ketQuaSource.EndEdit();

                if (ketQuaSource.DataSource is not DataTable table || table.Rows.Count == 0)
                {
                    Notify("Không có dữ liệu sinh viên nào để lưu!", "Thông báo", MessageBoxIcon.Warning, AlertType.Warning);
                    return;
                }

                // Strictly validate all rows before initiating transaction
                ValidateData(table);

                string maMon = cboMonHoc.SelectedValue?.ToString() ?? "";
                if (string.IsNullOrEmpty(maMon))
                {
                    Notify("Môn học chưa được chọn hợp lệ!", "Lỗi", MessageBoxIcon.Warning, AlertType.Warning);
                    return;
                }

                // Execute transaction in DAL
                int count = ketQuaDAL.SaveAll(maMon, table);

                Notify($"[GIAO DỊCH THÀNH CÔNG] Đã lưu và xác nhận an toàn (Commit) {count} dòng kết quả học tập vào cơ sở dữ liệu!",
                    "Thành công", MessageBoxIcon.Information, AlertType.Success);

                UpdateStatistics();
            }
            catch (Exception ex)
            {
                Notify("[GIAO DỊCH BỊ HỦY - ROLLBACK TOÀN BỘ]\n\n" + ex.Message +
                       "\n\nLưu ý: Không có bất kỳ dòng điểm nào bị lưu dở dang vào CSDL.",
                    "Lỗi Giao Dịch (Rollback)", MessageBoxIcon.Error, AlertType.Error);
            }
        }

        public void ValidateData(DataTable table)
        {
            int index = 0;
            foreach (DataRow row in table.Rows)
            {
                index++;
                if (row.RowState == DataRowState.Deleted) continue;

                string maSV = row["MaSV"]?.ToString() ?? "";
                string hoTen = row["HoTen"]?.ToString() ?? "";
                string diemText = row["Diem"]?.ToString()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(diemText))
                {
                    throw new Exception($"Dòng #{index} ({hoTen} - {maSV}): Điểm số không được để trống!");
                }

                if (!double.TryParse(diemText, out double diem))
                {
                    throw new Exception($"Dòng #{index} ({hoTen} - {maSV}): Điểm '{diemText}' không phải là định dạng số hợp lệ!");
                }

                if (diem < 0.0 || diem > 10.0)
                {
                    throw new Exception($"Dòng #{index} ({hoTen} - {maSV}): Điểm số ({diem}) không hợp lệ! Quy định điểm phải từ 0.0 đến 10.0.");
                }
            }
        }

        public void btnRollbackDemo_Click(object? sender, EventArgs e)
        {
            if (ketQuaSource.DataSource is not DataTable dt || dt.Rows.Count == 0)
            {
                ShowAlert("Vui lòng tải danh sách sinh viên trước khi thử nghiệm rollback!", AlertType.Warning);
                return;
            }

            // Purposely insert an invalid score (15.0) on the last row to trigger validation & rollback
            int lastIndex = dt.Rows.Count - 1;
            string targetMaSV = dt.Rows[lastIndex]["MaSV"]?.ToString() ?? "";
            dt.Rows[lastIndex]["Diem"] = 15.0; // VIOLATION!
            dt.Rows[lastIndex]["GhiChu"] = "Dữ liệu cố ý sai (15.0) để test Rollback";

            dgvKetQua.Refresh();
            ShowAlert($"Đã gán điểm sai (15.0) cho sinh viên {targetMaSV}. Đang kích hoạt lưu để minh họa Rollback...", AlertType.Warning);

            // Attempt save to trigger rollback
            btnLuuTatCa.PerformClick();
        }

        public void btnResetSample_Click(object? sender, EventArgs e)
        {
            ketQuaDAL.ResetMockData();
            LoadComboBoxes();
            LoadKetQua();
            ShowAlert("Đã làm mới và khôi phục dữ liệu mẫu ban đầu.", AlertType.Info);
        }

        public void UpdateStatistics()
        {
            if (ketQuaSource.DataSource is not DataTable dt || dt.Rows.Count == 0)
            {
                _lblStatCount.Text = "👥 Sĩ số: 0 sinh viên";
                _lblStatAvg.Text = "📊 Điểm TB: -- / 10.0";
                _lblStatPass.Text = "🎯 Tỉ lệ đạt (≥ 5.0): --";
                _lblStatHighLow.Text = "⭐ Cao nhất / Thấp nhất: -- / --";
                return;
            }

            int count = 0;
            double sum = 0.0;
            double max = double.MinValue;
            double min = double.MaxValue;
            int passCount = 0;

            foreach (DataRow row in dt.Rows)
            {
                if (row.RowState == DataRowState.Deleted) continue;
                if (double.TryParse(row["Diem"]?.ToString(), out double d))
                {
                    count++;
                    sum += d;
                    if (d > max) max = d;
                    if (d < min) min = d;
                    if (d >= 5.0) passCount++;
                }
            }

            if (count > 0)
            {
                double avg = sum / count;
                double passRate = (double)passCount / count * 100.0;

                _lblStatCount.Text = $"👥 Sĩ số: {count} sinh viên";
                _lblStatAvg.Text = $"📊 Điểm TB: {avg:F2} / 10.0";
                _lblStatPass.Text = $"🎯 Tỉ lệ đạt (≥ 5.0): {passRate:F1}% ({passCount}/{count})";
                _lblStatHighLow.Text = $"⭐ Cao nhất / Thấp: {max:F1} / {min:F1}";
            }
        }

        private void DgvKetQua_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvKetQua.Columns[e.ColumnIndex].Name == "Diem" && e.Value != null)
            {
                if (double.TryParse(e.Value.ToString(), out double diem))
                {
                    if (diem < 5.0)
                    {
                        // Coral Red text color for failing marks - NO ugly badge background!
                        e.CellStyle!.ForeColor = Color.FromArgb(220, 38, 38);
                        e.CellStyle!.Font = new Font(dgvKetQua.Font, FontStyle.Bold);
                    }
                    else if (diem >= 8.5)
                    {
                        // Teal text color for distinction
                        e.CellStyle!.ForeColor = Color.FromArgb(13, 148, 136);
                        e.CellStyle!.Font = new Font(dgvKetQua.Font, FontStyle.Bold);
                    }
                }
            }
        }

        public void ShowAlert(string message, AlertType type)
        {
            _pnlAlert.Visible = true;
            _lblAlertText.Text = message;
            switch (type)
            {
                case AlertType.Success:
                    _pnlAlert.BackColor = Color.FromArgb(236, 253, 245); // Soft Emerald 50
                    _lblAlertText.ForeColor = Color.FromArgb(6, 95, 70);
                    _lblAlertIcon.Text = "✔";
                    _lblAlertIcon.ForeColor = Color.FromArgb(4, 120, 87);
                    break;
                case AlertType.Warning:
                    _pnlAlert.BackColor = Color.FromArgb(255, 251, 235); // Soft Amber 50
                    _lblAlertText.ForeColor = Color.FromArgb(146, 64, 14);
                    _lblAlertIcon.Text = "⚠";
                    _lblAlertIcon.ForeColor = Color.FromArgb(217, 119, 6);
                    break;
                case AlertType.Error:
                    _pnlAlert.BackColor = Color.FromArgb(254, 242, 242); // Soft Rose 50
                    _lblAlertText.ForeColor = Color.FromArgb(153, 27, 27);
                    _lblAlertIcon.Text = "✖";
                    _lblAlertIcon.ForeColor = Color.FromArgb(190, 18, 60);
                    break;
                default:
                    _pnlAlert.BackColor = Color.FromArgb(240, 249, 255); // Soft Sky 50
                    _lblAlertText.ForeColor = Color.FromArgb(12, 74, 110);
                    _lblAlertIcon.Text = "ℹ";
                    _lblAlertIcon.ForeColor = Color.FromArgb(2, 132, 199);
                    break;
            }
            _tslStatus.Text = message;
        }

        private void Notify(string message, string title, MessageBoxIcon icon, AlertType alertType)
        {
            ShowAlert(message, alertType);
            if (!SuppressMessageBox)
            {
                MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
            }
        }

        private void HandleTransactionLog(TransactionLogEntry entry)
        {
            if (this.IsDisposed) return;

            string colorPrefix = entry.IsError ? "[FAILED]" : "[OK]";
            string log = $"[{entry.Timestamp:HH:mm:ss.fff}] {colorPrefix} {entry.Step} -> {entry.Detail}\r\n";

            if (_rtbAuditLog.InvokeRequired)
            {
                _rtbAuditLog.Invoke(new Action(() =>
                {
                    _rtbAuditLog.AppendText(log);
                    _rtbAuditLog.SelectionStart = _rtbAuditLog.TextLength;
                    _rtbAuditLog.ScrollToCaret();
                }));
            }
            else
            {
                _rtbAuditLog.AppendText(log);
                _rtbAuditLog.SelectionStart = _rtbAuditLog.TextLength;
                _rtbAuditLog.ScrollToCaret();
            }
        }

        public void CaptureView(string savePath)
        {
            Application.DoEvents();
            using var bmp = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bmp, new Rectangle(0, 0, this.Width, this.Height));
            bmp.Save(savePath, System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}
