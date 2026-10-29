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

        // Advanced & Polish Controls
        public Button btnRollbackDemo = null!;
        public Button btnResetSample = null!;
        private Panel _pnlHeader = null!;
        private Label _lblHeaderTitle = null!;
        private Label _lblHeaderSub = null!;
        private Label _lblStudentBadge = null!;
        private Button _btnTestConn = null!;

        private Panel _pnlToolbar = null!;
        private Panel _pnlAlert = null!;
        private Label _lblAlertIcon = null!;
        private Label _lblAlertText = null!;

        private Panel _pnlMainContainer = null!;
        private Panel _pnlBottom = null!;
        private Panel _pnlStatsCard = null!;
        private Label _lblStatsTitle = null!;
        private Label _lblStatAvg = null!;
        private Label _lblStatPass = null!;
        private Label _lblStatHighLow = null!;
        private Label _lblStatCount = null!;

        private Panel _pnlLogContainer = null!;
        private Label _lblLogTitle = null!;
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
            this.Text = "Quản lý Kết quả học tập - Data Binding, Stored Procedure & Transaction | COMP1019 Lab 12 - Lê Hoàng Quân";
            this.Size = new Size(1320, 840);
            this.MinimumSize = new Size(1100, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            this.BackColor = Color.FromArgb(248, 250, 252);

            // 1. Top Header Panel
            _pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 88,
                BackColor = Color.FromArgb(15, 23, 42) // Dark Navy Slate
            };
            _pnlHeader.Paint += PnlHeader_Paint;

            _lblHeaderTitle = new Label
            {
                Text = "QUẢN LÝ KẾT QUẢ HỌC TẬP (DATA BINDING & TRANSACTION)",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 13.5f, FontStyle.Bold),
                Location = new Point(22, 14),
                AutoSize = true
            };

            _lblHeaderSub = new Label
            {
                Text = "COMP1019 - Lập trình trên Windows | ThS. Lê Thanh Thoại - Khoa CNTT, HCMUE",
                ForeColor = Color.FromArgb(148, 163, 184),
                Font = new Font("Segoe UI", 9.2f),
                Location = new Point(24, 46),
                AutoSize = true
            };

            _lblStudentBadge = new Label
            {
                Text = "Sinh viên: Lê Hoàng Quân | MSSV: 51.01.104.082 | Lớp: 51.CNTT.A",
                ForeColor = Color.FromArgb(56, 189, 248),
                BackColor = Color.FromArgb(30, 41, 59),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(810, 14),
                Padding = new Padding(10, 6, 10, 6),
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            _btnTestConn = new Button
            {
                Text = "⚡ Kiểm Tra CSDL",
                BackColor = Color.FromArgb(51, 65, 85),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.0f, FontStyle.Bold),
                Location = new Point(810, 48),
                Size = new Size(150, 28),
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
            _tslBinding = new ToolStripStatusLabel("BindingSource: ketQuaSource") { BorderSides = ToolStripStatusLabelBorderSides.Left, BorderStyle = Border3DStyle.Etched };
            _tslEngine = new ToolStripStatusLabel("Động cơ: SQL Server (QLSinhVienLab12)") { BorderSides = ToolStripStatusLabelBorderSides.Left, BorderStyle = Border3DStyle.Etched, ForeColor = Color.FromArgb(30, 64, 175), Font = new Font("Segoe UI", 9.0f, FontStyle.Bold) };

            _statusStrip.Items.AddRange(new ToolStripItem[] { _tslStatus, _tslBinding, _tslEngine });

            // 3. Filter & Toolbar Panel (Ample spacing, NO text overlapping!)
            _pnlToolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 76,
                BackColor = Color.White,
                Padding = new Padding(18, 12, 18, 12)
            };

            var lblLop = new Label
            {
                Text = "Chọn Lớp Học:",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(18, 14),
                AutoSize = true
            };

            cboLop = new ComboBox
            {
                Location = new Point(18, 36),
                Size = new Size(250, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.8f)
            };

            var lblMonHoc = new Label
            {
                Text = "Chọn Môn Học:",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(285, 14),
                AutoSize = true
            };

            cboMonHoc = new ComboBox
            {
                Location = new Point(285, 36),
                Size = new Size(260, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.8f)
            };

            btnTaiDanhSach = new Button
            {
                Text = "📥 Tải Danh Sách",
                BackColor = Color.FromArgb(79, 70, 229), // Indigo 600
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(560, 34),
                Size = new Size(150, 33),
                Cursor = Cursors.Hand
            };
            btnTaiDanhSach.FlatAppearance.BorderSize = 0;
            btnTaiDanhSach.Click += btnTaiDanhSach_Click;

            btnLuuTatCa = new Button
            {
                Text = "💾 Lưu Tất Cả (Transaction)",
                BackColor = Color.FromArgb(16, 185, 129), // Emerald 600
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(720, 34),
                Size = new Size(215, 33),
                Cursor = Cursors.Hand
            };
            btnLuuTatCa.FlatAppearance.BorderSize = 0;
            btnLuuTatCa.Click += btnLuuTatCa_Click;

            btnRollbackDemo = new Button
            {
                Text = "⚡ Test Lỗi Rollback",
                BackColor = Color.FromArgb(245, 158, 11), // Amber 500
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.0f, FontStyle.Bold),
                Location = new Point(945, 34),
                Size = new Size(165, 33),
                Cursor = Cursors.Hand
            };
            btnRollbackDemo.FlatAppearance.BorderSize = 0;
            btnRollbackDemo.Click += btnRollbackDemo_Click;

            btnResetSample = new Button
            {
                Text = "⟲ Làm Mới",
                BackColor = Color.FromArgb(100, 116, 139), // Slate 500
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.0f, FontStyle.Bold),
                Location = new Point(1120, 34),
                Size = new Size(110, 33),
                Cursor = Cursors.Hand
            };
            btnResetSample.FlatAppearance.BorderSize = 0;
            btnResetSample.Click += btnResetSample_Click;

            _pnlToolbar.Controls.AddRange(new Control[] { lblLop, cboLop, lblMonHoc, cboMonHoc, btnTaiDanhSach, btnLuuTatCa, btnRollbackDemo, btnResetSample });

            // 4. Alert / Notification Banner
            _pnlAlert = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                BackColor = Color.FromArgb(236, 253, 245),
                Visible = false,
                Padding = new Padding(12, 8, 12, 8)
            };

            _lblAlertIcon = new Label
            {
                Text = "✔",
                Font = new Font("Segoe UI", 11.0f, FontStyle.Bold),
                Location = new Point(12, 10),
                AutoSize = true
            };

            _lblAlertText = new Label
            {
                Text = "Thông báo thao tác",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(38, 12),
                AutoSize = true
            };

            _pnlAlert.Controls.AddRange(new Control[] { _lblAlertIcon, _lblAlertText });

            // 5. Main Center Container (Grid + Bottom Split)
            _pnlMainContainer = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16, 8, 16, 8)
            };

            // DataGridView dgvKetQua
            dgvKetQua = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.CellSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                EnableHeadersVisualStyles = false
            };
            dgvKetQua.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(30, 41, 59),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(6)
            };
            dgvKetQua.ColumnHeadersHeight = 40;
            dgvKetQua.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.FromArgb(15, 23, 42),
                Font = new Font("Segoe UI", 9.5f),
                Padding = new Padding(4)
            };
            dgvKetQua.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 250, 252)
            };
            dgvKetQua.RowTemplate.Height = 36;
            dgvKetQua.CellValueChanged += dgvKetQua_CellValueChanged;
            dgvKetQua.DataError += (s, e) => e.Cancel = true;

            // 6. Bottom Split Panel (Analytics + Audit Log)
            _pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 220,
                Padding = new Padding(0, 10, 0, 0)
            };

            // Left: Statistics Card
            _pnlStatsCard = new Panel
            {
                Dock = DockStyle.Left,
                Width = 430,
                BackColor = Color.White,
                Padding = new Padding(14)
            };

            _lblStatsTitle = new Label
            {
                Text = "📊 THỐNG KÊ KẾT QUẢ ĐIỂM HỌC PHẦN",
                Font = new Font("Segoe UI", 10.0f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Dock = DockStyle.Top,
                Height = 28
            };

            _lblStatAvg = new Label
            {
                Text = "Điểm trung bình lớp:  Chưa có dữ liệu",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(79, 70, 229), // Indigo
                Location = new Point(14, 42),
                AutoSize = true
            };

            _lblStatPass = new Label
            {
                Text = "Tỉ lệ đạt (Điểm ≥ 5.0): Chưa tính",
                Font = new Font("Segoe UI", 9.2f),
                ForeColor = Color.FromArgb(16, 185, 129), // Emerald
                Location = new Point(14, 76),
                AutoSize = true
            };

            _lblStatHighLow = new Label
            {
                Text = "Điểm cao nhất / Thấp nhất: -- / --",
                Font = new Font("Segoe UI", 9.2f),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(14, 110),
                AutoSize = true
            };

            _lblStatCount = new Label
            {
                Text = "Sĩ số sinh viên đã nhập điểm: 0 bạn",
                Font = new Font("Segoe UI", 8.8f, FontStyle.Italic),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(14, 144),
                AutoSize = true
            };

            var lblEditNote = new Label
            {
                Text = "💡 Hướng dẫn: Người dùng được trực tiếp gõ sửa cột Điểm và Ghi Chú.",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                ForeColor = Color.FromArgb(14, 116, 144),
                Location = new Point(14, 178),
                AutoSize = true
            };

            _pnlStatsCard.Controls.AddRange(new Control[] { _lblStatsTitle, _lblStatAvg, _lblStatPass, _lblStatHighLow, _lblStatCount, lblEditNote });

            // Right: Audit Log
            _pnlLogContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(15, 23, 42),
                Padding = new Padding(12)
            };

            _lblLogTitle = new Label
            {
                Text = "⚡ NHẬT KÝ GIAO DỊCH (TRANSACTION AUDIT LOG & STORED PROCEDURES)",
                Font = new Font("Segoe UI", 9.2f, FontStyle.Bold),
                ForeColor = Color.FromArgb(56, 189, 248),
                Dock = DockStyle.Top,
                Height = 26
            };

            _rtbAuditLog = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9.2f),
                BackColor = Color.FromArgb(15, 23, 42),
                ForeColor = Color.FromArgb(226, 232, 240),
                BorderStyle = BorderStyle.None,
                ReadOnly = true
            };

            _pnlLogContainer.Controls.Add(_rtbAuditLog);
            _pnlLogContainer.Controls.Add(_lblLogTitle);

            var bottomSplitter = new Splitter { Dock = DockStyle.Left, Width = 8, BackColor = Color.FromArgb(248, 250, 252) };

            _pnlBottom.Controls.Add(_pnlLogContainer);
            _pnlBottom.Controls.Add(bottomSplitter);
            _pnlBottom.Controls.Add(_pnlStatsCard);

            // Add all to Main Container
            _pnlMainContainer.Controls.Add(dgvKetQua);
            _pnlMainContainer.Controls.Add(_pnlBottom);

            // Add all to Form
            this.Controls.Add(_pnlMainContainer);
            this.Controls.Add(_pnlAlert);
            this.Controls.Add(_pnlToolbar);
            this.Controls.Add(_pnlHeader);
            this.Controls.Add(_statusStrip);

            this.Load += FrmNhapDiemTheoLop_Load;
        }

        private void PnlHeader_Paint(object? sender, PaintEventArgs e)
        {
            using var brush = new LinearGradientBrush(_pnlHeader.ClientRectangle,
                Color.FromArgb(15, 23, 42), Color.FromArgb(30, 41, 59), LinearGradientMode.Horizontal);
            e.Graphics.FillRectangle(brush, _pnlHeader.ClientRectangle);
        }

        private void FrmNhapDiemTheoLop_Load(object? sender, EventArgs e)
        {
            CheckConnectionLive();
            LoadComboBox();
            SetupGridColumns();
            btnTaiDanhSach.PerformClick();
        }

        public void CheckConnectionLive()
        {
            bool isConnected = ketQuaDAL.CheckConnection();
            if (isConnected)
            {
                _tslEngine.Text = "Động cơ: SQL Server (QLSinhVienLab12) 🟢 Online";
                _tslEngine.ForeColor = Color.FromArgb(16, 185, 129);
                ShowAlert("Đã kết nối cơ sở dữ liệu SQL Server (QLSinhVienLab12) thành công.", AlertType.Success);
            }
            else
            {
                _tslEngine.Text = "Động cơ: In-Memory Engine (Mô phỏng SQL Server 2022 Schema) 🟡 Active";
                _tslEngine.ForeColor = Color.FromArgb(180, 83, 9);
                ShowAlert("Đang kích hoạt In-Memory Fallback Engine (Mô phỏng Transaction & SP chuẩn mực).", AlertType.Info);
            }
        }

        private void LoadComboBox()
        {
            try
            {
                // Binding LOP via Stored Procedure sp_Lop_GetAll
                DataTable dtLop = ketQuaDAL.GetLop();
                cboLop.DataSource = dtLop;
                cboLop.DisplayMember = "TenLop";
                cboLop.ValueMember = "MaLop";

                // Binding MONHOC via Stored Procedure sp_MonHoc_GetAll
                DataTable dtMonHoc = ketQuaDAL.GetMonHoc();
                cboMonHoc.DataSource = dtMonHoc;
                cboMonHoc.DisplayMember = "TenMon";
                cboMonHoc.ValueMember = "MaMon";

                if (cboLop.Items.Count > 0) cboLop.SelectedIndex = 0;
                if (cboMonHoc.Items.Count > 0) cboMonHoc.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Notify("Lỗi tải ComboBox: " + ex.Message, "Lỗi nạp dữ liệu", MessageBoxIcon.Error, AlertType.Error);
            }
        }

        private void SetupGridColumns()
        {
            dgvKetQua.AutoGenerateColumns = false;
            dgvKetQua.Columns.Clear();

            // 1. MaSV (ReadOnly)
            var colMaSV = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaSV",
                HeaderText = "Mã Sinh Viên",
                Name = "MaSV",
                ReadOnly = true,
                FillWeight = 20,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, BackColor = Color.FromArgb(248, 250, 252) }
            };

            // 2. HoTen (ReadOnly)
            var colHoTen = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "HoTen",
                HeaderText = "Họ và Tên Sinh Viên",
                Name = "HoTen",
                ReadOnly = true,
                FillWeight = 32,
                DefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 250, 252) }
            };

            // 3. Diem (Editable - Highlighted)
            var colDiem = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Diem",
                HeaderText = "Điểm Học Phần (0.0 - 10.0) ✎",
                Name = "Diem",
                ReadOnly = false,
                FillWeight = 24,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 10.0f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(30, 41, 59),
                    BackColor = Color.FromArgb(240, 253, 244) // Pale green highlight
                }
            };

            // 4. GhiChu (Editable)
            var colGhiChu = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "GhiChu",
                HeaderText = "Ghi Chú / Xếp Loại ✎",
                Name = "GhiChu",
                ReadOnly = false,
                FillWeight = 24,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.White
                }
            };

            dgvKetQua.Columns.AddRange(new DataGridViewColumn[] { colMaSV, colHoTen, colDiem, colGhiChu });
            dgvKetQua.DataSource = ketQuaSource;
        }

        public void btnTaiDanhSach_Click(object? sender, EventArgs e)
        {
            string maLop = cboLop.SelectedValue?.ToString() ?? "";
            string maMon = cboMonHoc.SelectedValue?.ToString() ?? "";

            if (string.IsNullOrEmpty(maLop) || string.IsNullOrEmpty(maMon))
            {
                Notify("Vui lòng chọn lớp học và môn học cần nhập điểm!", "Chưa chọn đủ tiêu chí", MessageBoxIcon.Warning, AlertType.Warning);
                return;
            }

            try
            {
                // Gọi Stored Procedure sp_KetQua_GetByLopMon
                DataTable table = ketQuaDAL.GetKetQuaByLopMon(maLop, maMon);
                ketQuaSource.DataSource = table;

                _tslStatus.Text = $"Đã tải danh sách {table.Rows.Count} sinh viên cho lớp '{cboLop.Text}', môn '{cboMonHoc.Text}'.";
                ShowAlert($"Tải danh sách sinh viên lớp '{cboLop.Text}' thành công ({table.Rows.Count} sinh viên).", AlertType.Success);
                UpdateStatistics();
            }
            catch (Exception ex)
            {
                Notify("Lỗi tải danh sách sinh viên: " + ex.Message, "Lỗi truy vấn", MessageBoxIcon.Error, AlertType.Error);
            }
        }

        public void btnLuuTatCa_Click(object? sender, EventArgs e)
        {
            try
            {
                // Kết thúc chỉnh sửa ô hiện tại để đẩy giá trị về DataTable
                ketQuaSource.EndEdit();
                if (ketQuaSource.DataSource is not DataTable table || table.Rows.Count == 0)
                {
                    Notify("Danh sách sinh viên đang trống! Vui lòng bấm [Tải Danh Sách] trước.", "Không có dữ liệu", MessageBoxIcon.Warning, AlertType.Warning);
                    return;
                }

                // 1. Kiểm tra tính hợp lệ dữ liệu (Validation) trước khi mở Transaction
                ValidateData(table);

                string maMon = cboMonHoc.SelectedValue?.ToString() ?? "";
                if (string.IsNullOrEmpty(maMon))
                {
                    Notify("Vui lòng chọn môn học cần lưu điểm!", "Lỗi môn học", MessageBoxIcon.Warning, AlertType.Warning);
                    return;
                }

                // 2. Thực thi Transaction lưu toàn bộ bảng điểm
                int count = ketQuaDAL.SaveAll(maMon, table);

                Notify($"Xác nhận giao dịch thành công (TRANSACTION COMMITTED)!\nĐã lưu an toàn kết quả học tập của {count} sinh viên vào CSDL.",
                    "Lưu điểm thành công", MessageBoxIcon.Information, AlertType.Success);

                _tslStatus.Text = $"Lưu thành công {count} bản ghi bằng Transaction.";
                UpdateStatistics();
            }
            catch (Exception ex)
            {
                Notify("GIAO DỊCH ĐÃ BỊ HỦY BỎ (TRANSACTION ROLLED BACK)!\nLý do lỗi: " + ex.Message,
                    "Lỗi lưu điểm", MessageBoxIcon.Error, AlertType.Error);
                _tslStatus.Text = "Transaction đã rollback do có lỗi dữ liệu.";
            }
        }

        public void btnRollbackDemo_Click(object? sender, EventArgs e)
        {
            // Cố ý đưa vào 1 dòng có điểm lỗi (ví dụ 15.0) để người dùng thấy rõ Transaction Rollback hoạt động
            if (ketQuaSource.DataSource is DataTable table && table.Rows.Count > 0)
            {
                table.Rows[table.Rows.Count - 1]["Diem"] = 15.0; // Điểm vượt trần > 10.0
                table.Rows[table.Rows.Count - 1]["GhiChu"] = "Cố tình nhập lỗi để thử Rollback";
                dgvKetQua.Refresh();
                UpdateStatistics();

                ShowAlert("Đã thiết lập điểm sinh viên cuối cùng thành 15.0 (Cố tình vi phạm). Hãy bấm [Lưu Tất Cả] để chứng minh Rollback!", AlertType.Warning);
            }
            else
            {
                Notify("Vui lòng bấm [Tải Danh Sách] trước khi chạy demo rollback!", "Thông báo", MessageBoxIcon.Information, AlertType.Info);
            }
        }

        public void btnResetSample_Click(object? sender, EventArgs e)
        {
            ketQuaDAL.InitializeMockDatabase();
            btnTaiDanhSach.PerformClick();
            ShowAlert("Đã khôi phục dữ liệu mẫu ban đầu của QLSinhVienLab12.", AlertType.Info);
        }

        private void ValidateData(DataTable table)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                if (row.RowState == DataRowState.Deleted) continue;

                string maSV = row["MaSV"]?.ToString() ?? $"Dòng {i + 1}";
                string hoTen = row["HoTen"]?.ToString() ?? "";
                string diemText = row["Diem"]?.ToString() ?? "";

                if (string.IsNullOrWhiteSpace(diemText))
                {
                    throw new Exception($"Điểm của sinh viên '{hoTen}' ({maSV}) không được để trống!");
                }

                if (!double.TryParse(diemText, out double diem))
                {
                    throw new Exception($"Điểm của sinh viên '{hoTen}' ({maSV}) phải là số thực hợp lệ (Hiện tại: '{diemText}')!");
                }

                if (diem < 0.0 || diem > 10.0)
                {
                    throw new Exception($"Điểm của sinh viên '{hoTen}' ({maSV}) là {diem}, vượt quá thang điểm quy định (0.0 đến 10.0)!");
                }
            }
        }

        private void dgvKetQua_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvKetQua.Columns[e.ColumnIndex].Name == "Diem")
            {
                UpdateStatistics();
            }
        }

        public void UpdateStatistics()
        {
            if (ketQuaSource.DataSource is not DataTable dt || dt.Rows.Count == 0)
            {
                _lblStatAvg.Text = "Điểm trung bình lớp:  Chưa có dữ liệu";
                _lblStatPass.Text = "Tỉ lệ đạt (Điểm ≥ 5.0): Chưa tính";
                _lblStatHighLow.Text = "Điểm cao nhất / Thấp nhất: -- / --";
                _lblStatCount.Text = "Sĩ số sinh viên: 0 bạn";
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

                _lblStatAvg.Text = $"Điểm trung bình lớp:  {avg:F2} / 10.0";
                _lblStatPass.Text = $"Tỉ lệ đạt (Điểm ≥ 5.0): {passRate:F1}% ({passCount}/{count} bạn đạt)";
                _lblStatHighLow.Text = $"Điểm cao nhất: {max:F1}  |  Điểm thấp nhất: {min:F1}";
                _lblStatCount.Text = $"Tổng số sinh viên trong danh sách: {count} bạn";
            }
        }

        public void ShowAlert(string message, AlertType type)
        {
            _pnlAlert.Visible = true;
            _lblAlertText.Text = message;
            switch (type)
            {
                case AlertType.Success:
                    _pnlAlert.BackColor = Color.FromArgb(236, 253, 245); // Emerald 50
                    _lblAlertText.ForeColor = Color.FromArgb(6, 95, 70); // Emerald 800
                    _lblAlertIcon.Text = "✔";
                    _lblAlertIcon.ForeColor = Color.FromArgb(16, 185, 129);
                    break;
                case AlertType.Warning:
                    _pnlAlert.BackColor = Color.FromArgb(255, 251, 235); // Amber 50
                    _lblAlertText.ForeColor = Color.FromArgb(146, 64, 14); // Amber 800
                    _lblAlertIcon.Text = "⚠";
                    _lblAlertIcon.ForeColor = Color.FromArgb(245, 158, 11);
                    break;
                case AlertType.Error:
                    _pnlAlert.BackColor = Color.FromArgb(254, 242, 242); // Rose 50
                    _lblAlertText.ForeColor = Color.FromArgb(153, 27, 27); // Rose 800
                    _lblAlertIcon.Text = "✖";
                    _lblAlertIcon.ForeColor = Color.FromArgb(239, 68, 68);
                    break;
                default:
                    _pnlAlert.BackColor = Color.FromArgb(239, 246, 255); // Blue 50
                    _lblAlertText.ForeColor = Color.FromArgb(30, 64, 175); // Blue 800
                    _lblAlertIcon.Text = "ℹ";
                    _lblAlertIcon.ForeColor = Color.FromArgb(59, 130, 246);
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
