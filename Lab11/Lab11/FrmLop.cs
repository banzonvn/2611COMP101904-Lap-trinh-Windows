using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using COMP1019_Buoi11_Lab11.Data;
using COMP1019_Buoi11_Lab11.Models;

namespace COMP1019_Buoi11_Lab11
{
    public enum AlertType { Success, Warning, Error, Info }

    public class FrmLop : Form
    {
        // Database service
        private readonly DatabaseService _dbService = new();

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public bool SuppressMessageBox { get; set; } = false;

        // UI Controls - Required by Lab11
        public Label lblMaLop = null!;
        public TextBox txtMaLop = null!;
        public Label lblTenLop = null!;
        public TextBox txtTenLop = null!;
        public TextBox txtTimKiem = null!;
        public Button btnTimKiem = null!;
        public DataGridView dgvLop = null!;
        public Button btnThem = null!;
        public Button btnSua = null!;
        public Button btnXoa = null!;
        public Button btnLamMoi = null!;

        // Advanced / Distinctive UI Controls
        private Panel _pnlHeader = null!;
        private Label _lblHeaderTitle = null!;
        private Label _lblHeaderSub = null!;
        private Label _lblStudentBadge = null!;
        private Button _btnTestConn = null!;

        private Panel _pnlLeftInput = null!;
        private Panel _pnlRightGrid = null!;
        private Panel _pnlAlert = null!;
        private Label _lblAlertIcon = null!;
        private Label _lblAlertText = null!;

        private TabControl _tabDetail = null!;
        public TabPage _tabStudents = null!;
        public TabPage _tabSqlLog = null!;
        private DataGridView _dgvStudents = null!;
        private RichTextBox _rtbSqlLog = null!;

        private StatusStrip _statusStrip = null!;
        private ToolStripStatusLabel _tslStatus = null!;
        private ToolStripStatusLabel _tslRowCount = null!;
        private ToolStripStatusLabel _tslEngine = null!;

        public FrmLop()
        {
            InitializeComponent();
            _dbService.OnSqlExecuted += HandleSqlLog;
        }

        private void InitializeComponent()
        {
            this.Text = "Quản lý Lớp học - ADO.NET & SQL Server | COMP1019 Lab 11 - Lê Hoàng Quân (51.01.104.082)";
            this.Size = new Size(1280, 800);
            this.MinimumSize = new Size(1050, 680);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            this.BackColor = Color.FromArgb(248, 250, 252);

            // 1. Header Panel
            _pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 85,
                BackColor = Color.FromArgb(15, 23, 42)
            };
            _pnlHeader.Paint += PnlHeader_Paint;

            _lblHeaderTitle = new Label
            {
                Text = "QUẢN LÝ LỚP HỌC (ADO.NET & SQL SERVER CRUD)",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 13.5f, FontStyle.Bold),
                Location = new Point(20, 14),
                AutoSize = true
            };

            _lblHeaderSub = new Label
            {
                Text = "COMP1019 - Lập trình trên Windows | ThS. Lê Thanh Thoại - Khoa CNTT, HCMUE",
                ForeColor = Color.FromArgb(148, 163, 184),
                Font = new Font("Segoe UI", 9.2f),
                Location = new Point(22, 45),
                AutoSize = true
            };

            _lblStudentBadge = new Label
            {
                Text = "Sinh viên: Lê Hoàng Quân | MSSV: 51.01.104.082 | Lớp: 51.CNTT.A",
                ForeColor = Color.FromArgb(56, 189, 248),
                BackColor = Color.FromArgb(30, 41, 59),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(780, 16),
                Padding = new Padding(10, 6, 10, 6),
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            _btnTestConn = new Button
            {
                Text = "⚡ Kiểm Tra Kết Nối",
                BackColor = Color.FromArgb(51, 65, 85),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.0f, FontStyle.Bold),
                Location = new Point(780, 48),
                Size = new Size(160, 28),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            _btnTestConn.FlatAppearance.BorderSize = 0;
            _btnTestConn.Click += (s, e) => CheckConnectionLive();

            _pnlHeader.Controls.AddRange(new Control[] { _lblHeaderTitle, _lblHeaderSub, _lblStudentBadge, _btnTestConn });

            // 2. Status Strip
            _statusStrip = new StatusStrip
            {
                BackColor = Color.FromArgb(241, 245, 249),
                Font = new Font("Segoe UI", 9.0f)
            };

            _tslStatus = new ToolStripStatusLabel("Hệ thống đã sẵn sàng.") { Spring = true, TextAlign = ContentAlignment.MiddleLeft };
            _tslRowCount = new ToolStripStatusLabel("Tổng số: 0 lớp") { BorderSides = ToolStripStatusLabelBorderSides.Left, BorderStyle = Border3DStyle.Etched };
            _tslEngine = new ToolStripStatusLabel("Động cơ: SQL Server (EduManagerDB)") { BorderSides = ToolStripStatusLabelBorderSides.Left, BorderStyle = Border3DStyle.Etched, ForeColor = Color.FromArgb(30, 64, 175), Font = new Font("Segoe UI", 9.0f, FontStyle.Bold) };

            _statusStrip.Items.AddRange(new ToolStripItem[] { _tslStatus, _tslRowCount, _tslEngine });

            // 3. Left Panel - Input Controls (Form Information)
            _pnlLeftInput = new Panel
            {
                Dock = DockStyle.Left,
                Width = 380,
                BackColor = Color.White,
                Padding = new Padding(18)
            };

            var lblBoxTitle = new Label
            {
                Text = "THÔNG TIN LỚP HỌC",
                Font = new Font("Segoe UI", 11.0f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Dock = DockStyle.Top,
                Height = 30
            };

            var pnlInputForm = new Panel
            {
                Dock = DockStyle.Top,
                Height = 220,
                Padding = new Padding(0, 10, 0, 10)
            };

            lblMaLop = new Label
            {
                Text = "Mã lớp (*):",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 65, 85),
                Location = new Point(0, 12),
                AutoSize = true
            };

            txtMaLop = new TextBox
            {
                Location = new Point(0, 36),
                Size = new Size(340, 29),
                Font = new Font("Segoe UI", 10.0f)
            };

            lblTenLop = new Label
            {
                Text = "Tên lớp học (*):",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 65, 85),
                Location = new Point(0, 78),
                AutoSize = true
            };

            txtTenLop = new TextBox
            {
                Location = new Point(0, 102),
                Size = new Size(340, 29),
                Font = new Font("Segoe UI", 10.0f)
            };

            var lblNote = new Label
            {
                Text = "(*) Lưu ý: Mã lớp là khóa chính, không được trùng lặp và không được sửa sau khi chọn dòng.",
                ForeColor = Color.FromArgb(100, 116, 139),
                Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                Location = new Point(0, 142),
                Size = new Size(340, 60)
            };

            pnlInputForm.Controls.AddRange(new Control[] { lblMaLop, txtMaLop, lblTenLop, txtTenLop, lblNote });

            // Button Action Panel
            var pnlButtons = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120
            };

            btnThem = new Button
            {
                Text = "✚ Thêm Lớp",
                BackColor = Color.FromArgb(16, 185, 129), // Emerald Green
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(0, 10),
                Size = new Size(165, 38),
                Cursor = Cursors.Hand
            };
            btnThem.FlatAppearance.BorderSize = 0;
            btnThem.Click += btnThem_Click;

            btnSua = new Button
            {
                Text = "✎ Cập Nhật",
                BackColor = Color.FromArgb(59, 130, 246), // Dodger Blue
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(175, 10),
                Size = new Size(165, 38),
                Cursor = Cursors.Hand
            };
            btnSua.FlatAppearance.BorderSize = 0;
            btnSua.Click += btnSua_Click;

            btnXoa = new Button
            {
                Text = "✖ Xóa Lớp",
                BackColor = Color.FromArgb(239, 68, 68), // Rose Red
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(0, 58),
                Size = new Size(165, 38),
                Cursor = Cursors.Hand
            };
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.Click += btnXoa_Click;

            btnLamMoi = new Button
            {
                Text = "⟲ Làm Mới",
                BackColor = Color.FromArgb(100, 116, 139), // Slate
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(175, 58),
                Size = new Size(165, 38),
                Cursor = Cursors.Hand
            };
            btnLamMoi.FlatAppearance.BorderSize = 0;
            btnLamMoi.Click += btnLamMoi_Click;

            pnlButtons.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa, btnLamMoi });

            // Instructions Box
            var rtbInstructions = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.0f),
                BackColor = Color.FromArgb(248, 250, 252),
                ForeColor = Color.FromArgb(51, 65, 85),
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Text = "HƯỚNG DẪN THAO TÁC (LAB 11):\n\n" +
                       "• Thêm lớp: Nhập Mã và Tên lớp -> Nhấn [Thêm Lớp]. Hệ thống dùng ExecuteScalar kiểm tra trùng mã trước khi INSERT.\n\n" +
                       "• Sửa lớp: Nhấp chuột vào 1 dòng trong bảng -> Đổi tên lớp -> Nhấn [Cập Nhật].\n\n" +
                       "• Xóa lớp: Chọn dòng cần xóa -> Nhấn [Xóa Lớp]. Nếu lớp đang có sinh viên, hệ thống sẽ CHẶN XÓA để bảo vệ toàn vẹn khóa ngoại (FK_SINHVIEN_LOP)!\n\n" +
                       "• Tìm kiếm: Nhập từ khóa (mã hoặc tên) -> Nhấn [Tìm kiếm] hoặc Enter."
            };

            _pnlLeftInput.Controls.Add(rtbInstructions);
            _pnlLeftInput.Controls.Add(pnlButtons);
            _pnlLeftInput.Controls.Add(pnlInputForm);
            _pnlLeftInput.Controls.Add(lblBoxTitle);

            // 4. Right Panel - Grid & Inspector
            _pnlRightGrid = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(14)
            };

            // Notification / Alert Banner
            _pnlAlert = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                BackColor = Color.FromArgb(220, 252, 231),
                Visible = false,
                Padding = new Padding(10, 8, 10, 8)
            };

            _lblAlertIcon = new Label
            {
                Text = "✔",
                Font = new Font("Segoe UI", 11.0f, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            _lblAlertText = new Label
            {
                Text = "Thông báo",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(36, 12),
                AutoSize = true
            };

            _pnlAlert.Controls.AddRange(new Control[] { _lblAlertIcon, _lblAlertText });

            // Search Bar Panel
            var pnlSearch = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.White,
                Padding = new Padding(12, 10, 12, 10)
            };

            var lblSearch = new Label
            {
                Text = "Tìm kiếm lớp học:",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(12, 14),
                AutoSize = true
            };

            txtTimKiem = new TextBox
            {
                Location = new Point(145, 11),
                Size = new Size(320, 29),
                Font = new Font("Segoe UI", 9.8f),
                PlaceholderText = "Nhập mã lớp hoặc tên lớp cần tìm..."
            };
            txtTimKiem.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    btnTimKiem.PerformClick();
                }
            };

            btnTimKiem = new Button
            {
                Text = "🔍 Tìm kiếm",
                BackColor = Color.FromArgb(30, 41, 59),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.2f, FontStyle.Bold),
                Location = new Point(475, 10),
                Size = new Size(110, 30),
                Cursor = Cursors.Hand
            };
            btnTimKiem.FlatAppearance.BorderSize = 0;
            btnTimKiem.Click += btnTimKiem_Click;

            var btnClearSearch = new Button
            {
                Text = "✕ Xóa Lọc",
                BackColor = Color.FromArgb(226, 232, 240),
                ForeColor = Color.FromArgb(15, 23, 42),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.0f),
                Location = new Point(595, 10),
                Size = new Size(95, 30),
                Cursor = Cursors.Hand
            };
            btnClearSearch.FlatAppearance.BorderSize = 0;
            btnClearSearch.Click += (s, e) =>
            {
                txtTimKiem.Clear();
                LoadData();
                ShowAlert("Đã xóa bộ lọc tìm kiếm. Đang hiển thị tất cả các lớp.", AlertType.Info);
            };

            pnlSearch.Controls.AddRange(new Control[] { lblSearch, txtTimKiem, btnTimKiem, btnClearSearch });

            // Split Container for DataGridView (Top) and Student/SQL Inspector (Bottom)
            var pnlTableContainer = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 10, 0, 0)
            };

            dgvLop = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 310,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                EnableHeadersVisualStyles = false
            };
            dgvLop.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(30, 41, 59),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(6)
            };
            dgvLop.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.FromArgb(15, 23, 42),
                Font = new Font("Segoe UI", 9.2f),
                Padding = new Padding(5)
            };
            dgvLop.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 250, 252)
            };
            dgvLop.RowTemplate.Height = 34;
            dgvLop.CellClick += dgvLop_CellClick;

            var splitterDetail = new Splitter { Dock = DockStyle.Top, Height = 6, BackColor = Color.FromArgb(226, 232, 240) };

            _tabDetail = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5f)
            };

            _tabStudents = new TabPage("Danh Sách Sinh Viên Thuộc Lớp (Khóa Ngoại FK_SINHVIEN_LOP)");
            _tabSqlLog = new TabPage("Nhật Ký Thực Thi Câu Lệnh SQL (Parameterized Command Live Monitor)");

            // Detail Students Grid
            _dgvStudents = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                EnableHeadersVisualStyles = false
            };
            _dgvStudents.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(51, 65, 85),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.0f, FontStyle.Bold),
                Padding = new Padding(5)
            };
            _dgvStudents.DefaultCellStyle = new DataGridViewCellStyle
            {
                Padding = new Padding(4)
            };
            _tabStudents.Controls.Add(_dgvStudents);

            // SQL Log Box
            _rtbSqlLog = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9.5f),
                BackColor = Color.FromArgb(15, 23, 42),
                ForeColor = Color.FromArgb(226, 232, 240),
                BorderStyle = BorderStyle.None,
                ReadOnly = true
            };
            _tabSqlLog.Controls.Add(_rtbSqlLog);

            _tabDetail.TabPages.Add(_tabStudents);
            _tabDetail.TabPages.Add(_tabSqlLog);

            pnlTableContainer.Controls.Add(_tabDetail);
            pnlTableContainer.Controls.Add(splitterDetail);
            pnlTableContainer.Controls.Add(dgvLop);

            _pnlRightGrid.Controls.Add(pnlTableContainer);
            _pnlRightGrid.Controls.Add(pnlSearch);
            _pnlRightGrid.Controls.Add(_pnlAlert);

            // Add Splitter between Left and Right
            var mainSplitter = new Splitter { Dock = DockStyle.Left, Width = 5, BackColor = Color.FromArgb(226, 232, 240) };

            this.Controls.Add(_pnlRightGrid);
            this.Controls.Add(mainSplitter);
            this.Controls.Add(_pnlLeftInput);
            this.Controls.Add(_pnlHeader);
            this.Controls.Add(_statusStrip);

            this.Load += FrmLop_Load;
        }

        private void PnlHeader_Paint(object? sender, PaintEventArgs e)
        {
            using var brush = new LinearGradientBrush(_pnlHeader.ClientRectangle,
                Color.FromArgb(15, 23, 42), Color.FromArgb(30, 41, 59), LinearGradientMode.Horizontal);
            e.Graphics.FillRectangle(brush, _pnlHeader.ClientRectangle);
        }

        private void FrmLop_Load(object? sender, EventArgs e)
        {
            CheckConnectionLive();
            LoadData();
        }

        public void CheckConnectionLive()
        {
            bool isConnected = _dbService.CheckSqlServerConnection();
            if (isConnected)
            {
                _tslEngine.Text = "Động cơ: Microsoft SQL Server (EduManagerDB) 🟢 Online";
                _tslEngine.ForeColor = Color.FromArgb(16, 185, 129);
                ShowAlert("Kết nối thành công đến Microsoft SQL Server (Database: EduManagerDB).", AlertType.Success);
            }
            else
            {
                _tslEngine.Text = "Động cơ: In-Memory Engine (Mô phỏng SQL Server 2022 Schema) 🟡 Active";
                _tslEngine.ForeColor = Color.FromArgb(180, 83, 9);
                ShowAlert("Đang chạy ở chế độ In-Memory Fallback Engine (Tương thích chuẩn lược đồ SQL Server).", AlertType.Info);
            }
        }

        public void ShowAlert(string message, AlertType type)
        {
            _pnlAlert.Visible = true;
            _lblAlertText.Text = message;
            switch (type)
            {
                case AlertType.Success:
                    _pnlAlert.BackColor = Color.FromArgb(220, 252, 231);
                    _lblAlertText.ForeColor = Color.FromArgb(22, 101, 52);
                    _lblAlertIcon.Text = "✔";
                    _lblAlertIcon.ForeColor = Color.FromArgb(22, 101, 52);
                    break;
                case AlertType.Warning:
                    _pnlAlert.BackColor = Color.FromArgb(254, 243, 199);
                    _lblAlertText.ForeColor = Color.FromArgb(146, 64, 14);
                    _lblAlertIcon.Text = "⚠";
                    _lblAlertIcon.ForeColor = Color.FromArgb(146, 64, 14);
                    break;
                case AlertType.Error:
                    _pnlAlert.BackColor = Color.FromArgb(254, 226, 226);
                    _lblAlertText.ForeColor = Color.FromArgb(153, 27, 27);
                    _lblAlertIcon.Text = "✖";
                    _lblAlertIcon.ForeColor = Color.FromArgb(153, 27, 27);
                    break;
                default:
                    _pnlAlert.BackColor = Color.FromArgb(219, 234, 254);
                    _lblAlertText.ForeColor = Color.FromArgb(30, 64, 175);
                    _lblAlertIcon.Text = "ℹ";
                    _lblAlertIcon.ForeColor = Color.FromArgb(30, 64, 175);
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

        public void LoadData(string keyword = "")
        {
            try
            {
                DataTable dt = _dbService.LoadData(keyword);
                dgvLop.DataSource = dt;

                if (dgvLop.Columns.Contains("MaLop") && dgvLop.Columns["MaLop"] != null)
                {
                    dgvLop.Columns["MaLop"]!.HeaderText = "Mã Lớp Học";
                    dgvLop.Columns["MaLop"]!.FillWeight = 25;
                }
                if (dgvLop.Columns.Contains("TenLop") && dgvLop.Columns["TenLop"] != null)
                {
                    dgvLop.Columns["TenLop"]!.HeaderText = "Tên Lớp Học";
                    dgvLop.Columns["TenLop"]!.FillWeight = 50;
                }
                if (dgvLop.Columns.Contains("SoSinhVien") && dgvLop.Columns["SoSinhVien"] != null)
                {
                    dgvLop.Columns["SoSinhVien"]!.HeaderText = "Sĩ Số (Số SV)";
                    dgvLop.Columns["SoSinhVien"]!.FillWeight = 25;
                    dgvLop.Columns["SoSinhVien"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                _tslRowCount.Text = $"Tổng số: {dt.Rows.Count} lớp";
                _tslStatus.Text = string.IsNullOrWhiteSpace(keyword)
                    ? "Tải danh sách lớp học thành công."
                    : $"Tìm kiếm từ khóa '{keyword}' hoàn tất ({dt.Rows.Count} kết quả).";
            }
            catch (Exception ex)
            {
                Notify("Lỗi tải dữ liệu: " + ex.Message, "Lỗi kết nối", MessageBoxIcon.Error, AlertType.Error);
                _tslStatus.Text = "Lỗi tải dữ liệu.";
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtMaLop.Text))
            {
                Notify("Vui lòng nhập mã lớp học!", "Cảnh báo dữ liệu", MessageBoxIcon.Warning, AlertType.Warning);
                txtMaLop.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTenLop.Text))
            {
                Notify("Vui lòng nhập tên lớp học!", "Cảnh báo dữ liệu", MessageBoxIcon.Warning, AlertType.Warning);
                txtTenLop.Focus();
                return false;
            }

            return true;
        }

        public void btnThem_Click(object? sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            string ma = txtMaLop.Text.Trim();
            string ten = txtTenLop.Text.Trim();

            try
            {
                if (_dbService.IsMaLopExists(ma))
                {
                    Notify($"Mã lớp '{ma}' đã tồn tại trong cơ sở dữ liệu! Vui lòng chọn mã khác.",
                        "Vi phạm khóa chính (Duplicate Key)", MessageBoxIcon.Warning, AlertType.Warning);
                    txtMaLop.SelectAll();
                    txtMaLop.Focus();
                    return;
                }

                int rows = _dbService.InsertLop(ma, ten);
                if (rows > 0)
                {
                    Notify($"Thêm mới lớp '{ten}' ({ma}) thành công!", "Thông báo", MessageBoxIcon.Information, AlertType.Success);
                    LoadData();
                    ClearInput();
                }
            }
            catch (Exception ex)
            {
                Notify("Lỗi khi thêm lớp học: " + ex.Message, "Lỗi thực thi", MessageBoxIcon.Error, AlertType.Error);
            }
        }

        public void btnSua_Click(object? sender, EventArgs e)
        {
            string ma = txtMaLop.Text.Trim();
            string ten = txtTenLop.Text.Trim();

            if (string.IsNullOrWhiteSpace(ma))
            {
                Notify("Vui lòng chọn một lớp học từ danh sách bên phải trước khi cập nhật!",
                    "Chưa chọn lớp", MessageBoxIcon.Warning, AlertType.Warning);
                return;
            }

            if (!ValidateInput()) return;

            try
            {
                int rows = _dbService.UpdateLop(ma, ten);
                if (rows > 0)
                {
                    Notify($"Cập nhật thông tin lớp '{ma}' thành công!", "Thông báo", MessageBoxIcon.Information, AlertType.Success);
                    LoadData();
                    ClearInput();
                }
                else
                {
                    Notify("Không tìm thấy mã lớp cần sửa trong cơ sở dữ liệu!", "Cảnh báo", MessageBoxIcon.Warning, AlertType.Warning);
                }
            }
            catch (Exception ex)
            {
                Notify("Lỗi khi cập nhật lớp: " + ex.Message, "Lỗi thực thi", MessageBoxIcon.Error, AlertType.Error);
            }
        }

        public void btnXoa_Click(object? sender, EventArgs e)
        {
            string ma = txtMaLop.Text.Trim();

            if (string.IsNullOrWhiteSpace(ma))
            {
                Notify("Vui lòng chọn một lớp học từ danh sách bên phải trước khi xóa!",
                    "Chưa chọn lớp", MessageBoxIcon.Warning, AlertType.Warning);
                return;
            }

            try
            {
                int soSinhVien = _dbService.CountSinhVienByLop(ma);
                if (soSinhVien > 0)
                {
                    Notify(
                        $"Không thể xóa lớp '{ma}' vì đang có {soSinhVien} sinh viên theo học!\n\n" +
                        "Quy tắc toàn vẹn: Ràng buộc khóa ngoại FK_SINHVIEN_LOP ngăn chặn xóa lớp khi vẫn còn dữ liệu sinh viên tham chiếu.",
                        "Từ chối xóa - Vi phạm khóa ngoại",
                        MessageBoxIcon.Stop,
                        AlertType.Error);
                    return;
                }

                if (!SuppressMessageBox)
                {
                    DialogResult result = MessageBox.Show(
                        $"Bạn có chắc chắn muốn xóa vĩnh viễn lớp '{ma}' ({txtTenLop.Text})?",
                        "Xác nhận xóa lớp học",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.No) return;
                }

                int rows = _dbService.DeleteLop(ma);
                if (rows > 0)
                {
                    Notify($"Đã xóa lớp '{ma}' thành công khỏi cơ sở dữ liệu!", "Thành công", MessageBoxIcon.Information, AlertType.Success);
                    LoadData();
                    ClearInput();
                }
            }
            catch (Exception ex)
            {
                Notify("Lỗi khi xóa lớp học: " + ex.Message, "Lỗi thực thi", MessageBoxIcon.Error, AlertType.Error);
            }
        }

        public void btnTimKiem_Click(object? sender, EventArgs e)
        {
            string kw = txtTimKiem.Text.Trim();
            LoadData(kw);
            ShowAlert($"Đã áp dụng bộ lọc tìm kiếm theo từ khóa '{kw}'.", AlertType.Info);
        }

        public void btnLamMoi_Click(object? sender, EventArgs e)
        {
            ClearInput();
            LoadData();
            _tabStudents.Text = "Danh Sách Sinh Viên Thuộc Lớp (Khóa Ngoại FK_SINHVIEN_LOP)";
            _dgvStudents.DataSource = null;
            ShowAlert("Đã làm mới form và nạp lại toàn bộ dữ liệu từ CSDL.", AlertType.Info);
        }

        public void dgvLop_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvLop.Rows.Count) return;

            DataGridViewRow row = dgvLop.Rows[e.RowIndex];
            string maLop = row.Cells["MaLop"].Value?.ToString() ?? string.Empty;
            string tenLop = row.Cells["TenLop"].Value?.ToString() ?? string.Empty;

            txtMaLop.Text = maLop;
            txtTenLop.Text = tenLop;
            txtMaLop.ReadOnly = true; // Khóa Primary Key khi đang chọn sửa
            txtMaLop.BackColor = Color.FromArgb(241, 245, 249);

            // Load enrolled students in this class to demonstrate FK relationship
            DataTable dtStudents = _dbService.GetSinhVienByLop(maLop);
            _dgvStudents.DataSource = dtStudents;
            _tabStudents.Text = $"Sinh Viên Lớp {maLop} ({dtStudents.Rows.Count} bạn)";

            ShowAlert($"Đang chọn lớp: {maLop} - {tenLop} (Hiện có {dtStudents.Rows.Count} sinh viên).", AlertType.Info);
        }

        public void ClearInput()
        {
            txtMaLop.Clear();
            txtTenLop.Clear();
            txtTimKiem.Clear();
            txtMaLop.ReadOnly = false;
            txtMaLop.BackColor = Color.White;
            txtMaLop.Focus();
        }

        public void SelectTab(int index)
        {
            if (index >= 0 && index < _tabDetail.TabPages.Count)
            {
                _tabDetail.SelectedIndex = index;
            }
        }

        private void HandleSqlLog(SqlLogEntry entry)
        {
            if (this.IsDisposed) return;

            string statusText = entry.IsSuccess ? "[SUCCESS]" : "[ERROR]";
            string log = $"[{entry.Timestamp:HH:mm:ss.fff}] {statusText} ({entry.ExecutionTimeMs:F1} ms, Rows: {entry.RowsAffected})\r\n" +
                         $"SQL: {entry.CommandText}\r\n" +
                         $"PARAMS: {entry.Parameters}\r\n" +
                         $"INFO: {entry.Message}\r\n" +
                         new string('-', 75) + "\r\n";

            if (_rtbSqlLog.InvokeRequired)
            {
                _rtbSqlLog.Invoke(new Action(() =>
                {
                    _rtbSqlLog.AppendText(log);
                    _rtbSqlLog.SelectionStart = _rtbSqlLog.TextLength;
                    _rtbSqlLog.ScrollToCaret();
                }));
            }
            else
            {
                _rtbSqlLog.AppendText(log);
                _rtbSqlLog.SelectionStart = _rtbSqlLog.TextLength;
                _rtbSqlLog.ScrollToCaret();
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
