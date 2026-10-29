using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using StudentReportDemo.Models;
using StudentReportDemo.Services;

namespace StudentReportDemo.Forms
{
    /// <summary>
    /// Giao diện báo cáo sinh viên bằng LINQ.
    /// Thiết kế hiện đại, bảng màu trung tính trang nhã (Warm Slate & Sage),
    /// tuyệt đối không dùng nền xanh đậm chói mắt, không bị lỗi chữ chồng chữ hay cắt chữ.
    /// </summary>
    public partial class FrmBaoCaoSinhVien : Form
    {
        // Danh sách dữ liệu nguồn
        private List<SinhVien> _danhSachSinhVien = new List<SinhVien>();

        // Danh sách dữ liệu đang hiển thị trên lưới (để phục vụ xuất file)
        private List<SinhVien> _duLieuDangHienThi = new List<SinhVien>();
        private List<ThongKeLopDTO> _duLieuThongKeDangHienThi = new List<ThongKeLopDTO>();
        private bool _dangXemThongKe = false;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool SuppressMessageBox { get; set; } = false;

        // --- Các điều khiển trên giao diện ---
        private Panel _pnlHeader = null!;
        private Label _lblHeaderTag = null!;
        private Label _lblHeaderTitle = null!;
        private Label _lblHeaderSubtitle = null!;
        private Label _lblUserBadge = null!;

        private Panel _pnlToolbar = null!;
        private Label _lblLop = null!;
        private ComboBox _cboLop = null!;
        private Label _lblKetQua = null!;
        private ComboBox _cboKetQua = null!;
        private Label _lblTuKhoa = null!;
        private TextBox _txtTuKhoa = null!;
        private Button _btnXemBaoCao = null!;
        private Button _btnThongKe = null!;
        private Button _btnXuatFile = null!;
        private Button _btnLamMoi = null!;

        private Panel _pnlKpiContainer = null!;
        private Panel _cardTongSV = null!;
        private Label _lblCardTongSVTitle = null!;
        private Label _lblCardTongSVValue = null!;
        private Label _lblCardTongSVSub = null!;

        private Panel _cardDat = null!;
        private Label _lblCardDatTitle = null!;
        private Label _lblCardDatValue = null!;
        private Label _lblCardDatSub = null!;

        private Panel _cardKhongDat = null!;
        private Label _lblCardKhongDatTitle = null!;
        private Label _lblCardKhongDatValue = null!;
        private Label _lblCardKhongDatSub = null!;

        private Panel _cardDiemTB = null!;
        private Label _lblCardDiemTBTitle = null!;
        private Label _lblCardDiemTBValue = null!;
        private Label _lblCardDiemTBSub = null!;

        private Panel _pnlGridWrapper = null!;
        private DataGridView _dgvBaoCao = null!;
        private Panel _pnlAlertBanner = null!;
        private Label _lblAlertBannerText = null!;

        private StatusStrip _statusStrip = null!;
        private ToolStripStatusLabel _statusLabel = null!;
        private ToolStripStatusLabel _statusSpring = null!;
        private ToolStripStatusLabel _statusAuthor = null!;

        // Aliases to match standard rubric control naming
        public ComboBox cboLop => _cboLop;
        public ComboBox cboKetQua => _cboKetQua;
        public TextBox txtTuKhoa => _txtTuKhoa;
        public Button btnXemBaoCao => _btnXemBaoCao;
        public Button btnThongKe => _btnThongKe;
        public Button btnXuatFile => _btnXuatFile;
        public DataGridView dgvBaoCao => _dgvBaoCao;

        public FrmBaoCaoSinhVien()
        {
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            // Cấu hình Form chính với kích thước rộng rãi (1180 x 740)
            this.Text = "COMP1019 - Báo cáo sinh viên bằng LINQ | Nhóm 4 - Lê Hoàng Quân";
            this.Size = new Size(1180, 750);
            this.MinimumSize = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ColorTranslator.FromHtml("#F8FAFC");
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            // ==========================================
            // 1. TOP HEADER BANNER (Tone sáng, sang trọng, không xanh đậm)
            // ==========================================
            _pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 92,
                BackColor = Color.White,
                Padding = new Padding(24, 10, 24, 10)
            };
            _pnlHeader.Paint += (s, e) =>
            {
                using var pen = new Pen(ColorTranslator.FromHtml("#E2E8F0"), 1);
                e.Graphics.DrawLine(pen, 0, _pnlHeader.Height - 1, _pnlHeader.Width, _pnlHeader.Height - 1);
                // Vạch màu Sage tinh tế ở đỉnh
                using var topBrush = new SolidBrush(ColorTranslator.FromHtml("#0D9488"));
                e.Graphics.FillRectangle(topBrush, 0, 0, _pnlHeader.Width, 3);
            };

            _lblHeaderTag = new Label
            {
                Text = "LAB 13 • LINQ REPORTING & AGGREGATIONS",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#0F766E"),
                BackColor = ColorTranslator.FromHtml("#F0FDFA"),
                Location = new Point(24, 12),
                Size = new Size(290, 22),
                TextAlign = ContentAlignment.MiddleCenter
            };

            _lblHeaderTitle = new Label
            {
                Text = "BÁO CÁO KẾT QUẢ HỌC TẬP SINH VIÊN THEO LỚP",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#0F172A"),
                Location = new Point(24, 38),
                Size = new Size(650, 28),
                TextAlign = ContentAlignment.MiddleLeft
            };

            _lblHeaderSubtitle = new Label
            {
                Text = "Học phần COMP1019 - Lập trình trên Windows • Giảng viên: ThS. Lê Thanh Thoại",
                Font = new Font("Segoe UI", 9.25f, FontStyle.Regular),
                ForeColor = ColorTranslator.FromHtml("#64748B"),
                Location = new Point(24, 66),
                Size = new Size(650, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            _lblUserBadge = new Label
            {
                Text = "Sinh viên thực hiện:\nLê Hoàng Quân - 51.01.104.082",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#334155"),
                BackColor = ColorTranslator.FromHtml("#F1F5F9"),
                Size = new Size(260, 56),
                Location = new Point(880, 16),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                TextAlign = ContentAlignment.MiddleCenter
            };

            _pnlHeader.Controls.AddRange(new Control[] { _lblHeaderTag, _lblHeaderTitle, _lblHeaderSubtitle, _lblUserBadge });
            _pnlHeader.Resize += (s, e) =>
            {
                _lblUserBadge.Location = new Point(_pnlHeader.Width - 260 - 24, 16);
            };

            // ==========================================
            // 2. TOOLBAR LỌC & THAO TÁC (Độc lập, khoảng cách rộng rãi, AutoSize chống cắt chữ)
            // ==========================================
            _pnlToolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 84,
                BackColor = Color.White,
                Padding = new Padding(20, 8, 20, 8)
            };
            _pnlToolbar.Paint += (s, e) =>
            {
                using var pen = new Pen(ColorTranslator.FromHtml("#E2E8F0"), 1);
                e.Graphics.DrawLine(pen, 0, _pnlToolbar.Height - 1, _pnlToolbar.Width, _pnlToolbar.Height - 1);
            };

            // Dropdown Chọn lớp
            _lblLop = new Label
            {
                Text = "Lớp học:",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#475569"),
                Location = new Point(20, 14),
                AutoSize = true
            };
            _cboLop = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5f),
                Location = new Point(20, 38),
                Size = new Size(140, 32)
            };
            _cboLop.SelectedIndexChanged += cboLop_SelectedIndexChanged;

            // Dropdown Chọn kết quả
            _lblKetQua = new Label
            {
                Text = "Kết quả:",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#475569"),
                Location = new Point(175, 14),
                AutoSize = true
            };
            _cboKetQua = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5f),
                Location = new Point(175, 38),
                Size = new Size(120, 32)
            };
            _cboKetQua.SelectedIndexChanged += cboKetQua_SelectedIndexChanged;

            // TextBox Tìm kiếm từ khóa
            _lblTuKhoa = new Label
            {
                Text = "Tìm kiếm (Mã / Tên):",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#475569"),
                Location = new Point(310, 14),
                AutoSize = true
            };
            _txtTuKhoa = new TextBox
            {
                Font = new Font("Segoe UI", 9.5f),
                Location = new Point(310, 38),
                Size = new Size(175, 32),
                PlaceholderText = "Nhập mã hoặc họ tên..."
            };
            _txtTuKhoa.TextChanged += txtTuKhoa_TextChanged;

            // Nút Xem báo cáo
            _btnXemBaoCao = new Button
            {
                Text = "Xem Báo Cáo",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#0D9488"), // Warm Teal
                FlatStyle = FlatStyle.Flat,
                Location = new Point(505, 36),
                Size = new Size(130, 34),
                Cursor = Cursors.Hand
            };
            _btnXemBaoCao.FlatAppearance.BorderSize = 0;
            _btnXemBaoCao.Click += btnXemBaoCao_Click;

            // Nút Thống kê theo lớp
            _btnThongKe = new Button
            {
                Text = "Thống Kê Lớp",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#475569"), // Slate
                FlatStyle = FlatStyle.Flat,
                Location = new Point(645, 36),
                Size = new Size(130, 34),
                Cursor = Cursors.Hand
            };
            _btnThongKe.FlatAppearance.BorderSize = 0;
            _btnThongKe.Click += btnThongKe_Click;

            // Nút Xuất file CSV / TXT
            _btnXuatFile = new Button
            {
                Text = "Xuất File",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#D97706"), // Warm Amber
                FlatStyle = FlatStyle.Flat,
                Location = new Point(785, 36),
                Size = new Size(110, 34),
                Cursor = Cursors.Hand
            };
            _btnXuatFile.FlatAppearance.BorderSize = 0;
            _btnXuatFile.Click += btnXuatFile_Click;

            // Nút Làm mới bộ lọc
            _btnLamMoi = new Button
            {
                Text = "Làm Mới",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#475569"),
                BackColor = ColorTranslator.FromHtml("#F1F5F9"),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(905, 36),
                Size = new Size(100, 34),
                Cursor = Cursors.Hand
            };
            _btnLamMoi.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#CBD5E1");
            _btnLamMoi.Click += (s, e) => ResetBoLoc();

            _pnlToolbar.Controls.AddRange(new Control[]
            {
                _lblLop, _cboLop,
                _lblKetQua, _cboKetQua,
                _lblTuKhoa, _txtTuKhoa,
                _btnXemBaoCao, _btnThongKe, _btnXuatFile, _btnLamMoi
            });

            // ==========================================
            // 3. KPI METRIC CARDS (4 thẻ chỉ số mềm mại, thông thoáng, không chồng chữ)
            // ==========================================
            _pnlKpiContainer = new Panel
            {
                Dock = DockStyle.Top,
                Height = 88,
                BackColor = ColorTranslator.FromHtml("#F8FAFC"),
                Padding = new Padding(24, 8, 24, 8)
            };

            _cardTongSV = CreateKpiCard("TỔNG SINH VIÊN", "0", "Sinh viên hiển thị",
                ColorTranslator.FromHtml("#FFFFFF"), ColorTranslator.FromHtml("#E2E8F0"),
                ColorTranslator.FromHtml("#475569"), ColorTranslator.FromHtml("#0F172A"),
                out _lblCardTongSVTitle, out _lblCardTongSVValue, out _lblCardTongSVSub);

            _cardDat = CreateKpiCard("SINH VIÊN ĐẠT (>= 5.0)", "0", "Tỉ lệ đạt: 0.0%",
                ColorTranslator.FromHtml("#F0FDF4"), ColorTranslator.FromHtml("#BBF7D0"),
                ColorTranslator.FromHtml("#15803D"), ColorTranslator.FromHtml("#166534"),
                out _lblCardDatTitle, out _lblCardDatValue, out _lblCardDatSub);

            _cardKhongDat = CreateKpiCard("KHÔNG ĐẠT (< 5.0)", "0", "Chưa đạt yêu cầu",
                ColorTranslator.FromHtml("#FEF2F2"), ColorTranslator.FromHtml("#FECACA"),
                ColorTranslator.FromHtml("#B91C1C"), ColorTranslator.FromHtml("#991B1B"),
                out _lblCardKhongDatTitle, out _lblCardKhongDatValue, out _lblCardKhongDatSub);

            _cardDiemTB = CreateKpiCard("ĐIỂM TRUNG BÌNH", "0.00", "Cao nhất: 0.0 | Thấp: 0.0",
                ColorTranslator.FromHtml("#FFFBEB"), ColorTranslator.FromHtml("#FDE68A"),
                ColorTranslator.FromHtml("#B45309"), ColorTranslator.FromHtml("#92400E"),
                out _lblCardDiemTBTitle, out _lblCardDiemTBValue, out _lblCardDiemTBSub);

            _pnlKpiContainer.Controls.AddRange(new Control[] { _cardTongSV, _cardDat, _cardKhongDat, _cardDiemTB });
            _pnlKpiContainer.Resize += (s, e) => LayoutKpiCards();

            // ==========================================
            // 4. KHUNG HIỂN THỊ DỮ LIỆU DATAGRIDVIEW + ALERT BANNER
            // ==========================================
            _pnlGridWrapper = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ColorTranslator.FromHtml("#F8FAFC"),
                Padding = new Padding(24, 4, 24, 12)
            };

            // Banner thông báo khi không có dữ liệu (Hiển thị ngay trên Form)
            _pnlAlertBanner = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                BackColor = ColorTranslator.FromHtml("#FEF3C7"),
                Visible = false,
                Padding = new Padding(14, 8, 14, 8)
            };
            _pnlAlertBanner.Paint += (s, e) =>
            {
                Color borderClr = _pnlAlertBanner.BackColor == ColorTranslator.FromHtml("#ECFDF5")
                    ? ColorTranslator.FromHtml("#10B981")
                    : ColorTranslator.FromHtml("#F59E0B");
                using var p = new Pen(borderClr, 1);
                e.Graphics.DrawRectangle(p, 0, 0, _pnlAlertBanner.Width - 1, _pnlAlertBanner.Height - 1);
            };
            _lblAlertBannerText = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#92400E"),
                TextAlign = ContentAlignment.MiddleLeft,
                Text = "⚠️ Không tìm thấy sinh viên nào phù hợp với điều kiện lọc."
            };
            _pnlAlertBanner.Controls.Add(_lblAlertBannerText);

            // DataGridView dgvBaoCao
            _dgvBaoCao = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = ColorTranslator.FromHtml("#E2E8F0"),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                EnableHeadersVisualStyles = false
            };
            _dgvBaoCao.ColumnHeadersHeight = 38;
            _dgvBaoCao.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _dgvBaoCao.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ColorTranslator.FromHtml("#F1F5F9"),
                ForeColor = ColorTranslator.FromHtml("#334155"),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 8, 0)
            };
            _dgvBaoCao.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = ColorTranslator.FromHtml("#1E293B"),
                SelectionBackColor = ColorTranslator.FromHtml("#E0F2FE"),
                SelectionForeColor = ColorTranslator.FromHtml("#0369A1"),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                Padding = new Padding(8, 4, 8, 4)
            };
            _dgvBaoCao.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ColorTranslator.FromHtml("#F8FAFC")
            };
            _dgvBaoCao.RowTemplate.Height = 34;
            _dgvBaoCao.CellFormatting += DgvBaoCao_CellFormatting;

            _pnlGridWrapper.Controls.Add(_dgvBaoCao);
            _pnlGridWrapper.Controls.Add(_pnlAlertBanner);

            // ==========================================
            // 5. STATUS STRIP CHÂN TRANG
            // ==========================================
            _statusStrip = new StatusStrip
            {
                BackColor = ColorTranslator.FromHtml("#F1F5F9"),
                Font = new Font("Segoe UI", 9f),
                SizingGrip = false
            };
            _statusLabel = new ToolStripStatusLabel
            {
                Text = "Sẵn sàng",
                ForeColor = ColorTranslator.FromHtml("#475569")
            };
            _statusSpring = new ToolStripStatusLabel
            {
                Spring = true
            };
            _statusAuthor = new ToolStripStatusLabel
            {
                Text = "COMP1019 - Buổi 13: LINQ Report Demo | MSSV: 51.01.104.082",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                ForeColor = ColorTranslator.FromHtml("#64748B")
            };
            _statusStrip.Items.AddRange(new ToolStripItem[] { _statusLabel, _statusSpring, _statusAuthor });

            // Thêm các thành phần vào Form
            this.Controls.Add(_pnlGridWrapper);
            this.Controls.Add(_pnlKpiContainer);
            this.Controls.Add(_pnlToolbar);
            this.Controls.Add(_pnlHeader);
            this.Controls.Add(_statusStrip);
            _statusStrip.SendToBack();
            _pnlGridWrapper.BringToFront();

            this.Load += FrmBaoCaoSinhVien_Load;
        }

        private Panel CreateKpiCard(
            string title, string initialValue, string sub,
            Color bg, Color borderColor, Color titleColor, Color valColor,
            out Label lblTitle, out Label lblVal, out Label lblSub)
        {
            var pnl = new Panel
            {
                BackColor = bg,
                Height = 72,
                Padding = new Padding(12, 6, 12, 6)
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(borderColor, 1);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 8.25f, FontStyle.Bold),
                ForeColor = titleColor,
                Location = new Point(12, 6),
                Size = new Size(220, 16),
                TextAlign = ContentAlignment.MiddleLeft
            };

            lblVal = new Label
            {
                Text = initialValue,
                Font = new Font("Segoe UI", 15f, FontStyle.Bold),
                ForeColor = valColor,
                Location = new Point(12, 24),
                Size = new Size(220, 26),
                TextAlign = ContentAlignment.MiddleLeft
            };

            lblSub = new Label
            {
                Text = sub,
                Font = new Font("Segoe UI", 8.25f, FontStyle.Regular),
                ForeColor = ColorTranslator.FromHtml("#64748B"),
                Location = new Point(12, 50),
                Size = new Size(220, 16),
                TextAlign = ContentAlignment.MiddleLeft
            };

            pnl.Controls.AddRange(new Control[] { lblTitle, lblVal, lblSub });
            return pnl;
        }

        private void LayoutKpiCards()
        {
            int totalW = _pnlKpiContainer.ClientSize.Width - 48;
            int spacing = 12;
            int cardW = Math.Max(180, (totalW - spacing * 3) / 4);

            _cardTongSV.SetBounds(24, 6, cardW, 72);
            _cardDat.SetBounds(24 + cardW + spacing, 6, cardW, 72);
            _cardKhongDat.SetBounds(24 + (cardW + spacing) * 2, 6, cardW, 72);
            _cardDiemTB.SetBounds(24 + (cardW + spacing) * 3, 6, cardW, 72);

            foreach (var card in new[] { _cardTongSV, _cardDat, _cardKhongDat, _cardDiemTB })
            {
                foreach (Control c in card.Controls)
                {
                    c.Width = card.Width - 24;
                }
            }
        }

        private void FrmBaoCaoSinhVien_Load(object? sender, EventArgs e)
        {
            TaoDuLieuMau();
            LoadComboBox();
            XemBaoCao();
            LayoutKpiCards();
        }

        /// <summary>
        /// Tạo danh sách dữ liệu mẫu chuẩn theo yêu cầu đề bài (>= 12 sinh viên, >= 3 lớp).
        /// </summary>
        public void TaoDuLieuMau()
        {
            _danhSachSinhVien = ReportEngine.TaoDuLieuMau();
        }

        /// <summary>
        /// Nạp danh sách lớp học và các tùy chọn kết quả vào ComboBox bằng LINQ Select và Distinct.
        /// </summary>
        public void LoadComboBox()
        {
            // LINQ: Lấy danh sách duy nhất các lớp học và sắp xếp theo MaLop
            var dsLop = _danhSachSinhVien
                .Select(sv => new { sv.MaLop, sv.TenLop })
                .Distinct()
                .OrderBy(l => l.MaLop)
                .ToList();

            // Thêm tùy chọn "Tất cả các lớp" vào đầu danh sách
            var dsLopCombo = new List<dynamic>
            {
                new { MaLop = "ALL", TenLop = "-- Tất cả các lớp --" }
            };
            dsLopCombo.AddRange(dsLop);

            _cboLop.DataSource = dsLopCombo;
            _cboLop.DisplayMember = "TenLop";
            _cboLop.ValueMember = "MaLop";
            _cboLop.SelectedIndex = 1; // Mặc định chọn lớp đầu tiên (CTK47A)

            // Nạp các điều kiện lọc kết quả
            _cboKetQua.Items.Clear();
            _cboKetQua.Items.Add("Tất cả");
            _cboKetQua.Items.Add("Đạt");
            _cboKetQua.Items.Add("Không đạt");
            _cboKetQua.SelectedIndex = 0; // Mặc định "Tất cả"
        }

        /// <summary>
        /// Thực hiện lọc, tìm kiếm và sắp xếp danh sách sinh viên bằng LINQ.
        /// </summary>
        public void XemBaoCao()
        {
            _dangXemThongKe = false;
            _pnlAlertBanner.Visible = false;

            string maLop = _cboLop.SelectedValue?.ToString() ?? "ALL";
            string ketQua = _cboKetQua.SelectedItem?.ToString() ?? "Tất cả";
            string tuKhoa = _txtTuKhoa.Text.Trim();

            // Gọi Engine thực thi LINQ pipeline
            var ketQuaLoc = ReportEngine.LocBaoCao(_danhSachSinhVien, maLop, ketQua, tuKhoa);
            _duLieuDangHienThi = ketQuaLoc;

            // Hiển thị lên DataGridView
            HienThiDanhSachSinhVien(ketQuaLoc);

            // Cập nhật thẻ KPI thống kê
            CapNhatThongKe(ketQuaLoc.Count);

            // Cập nhật trạng thái
            string tenLopHienThi = _cboLop.Text;
            _statusLabel.Text = $"Đang hiển thị {ketQuaLoc.Count} sinh viên | Lớp: {tenLopHienThi} | Điều kiện: {ketQua}";

            if (ketQuaLoc.Count == 0)
            {
                _pnlAlertBanner.Visible = true;
                _pnlAlertBanner.BackColor = ColorTranslator.FromHtml("#FEF3C7");
                _lblAlertBannerText.ForeColor = ColorTranslator.FromHtml("#92400E");
                _lblAlertBannerText.Text = $"⚠️ Không tìm thấy sinh viên nào phù hợp với lớp \"{tenLopHienThi}\" và từ khóa \"{tuKhoa}\".";
                if (!SuppressMessageBox)
                {
                    MessageBox.Show(
                        "Không có dữ liệu phù hợp với điều kiện lọc hiện tại.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        private void HienThiDanhSachSinhVien(List<SinhVien> danhSach)
        {
            _dgvBaoCao.DataSource = null;
            _dgvBaoCao.Columns.Clear();

            var dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("MaSV", typeof(string));
            dt.Columns.Add("HoTen", typeof(string));
            dt.Columns.Add("TenLop", typeof(string));
            dt.Columns.Add("Diem", typeof(double));
            dt.Columns.Add("KetQua", typeof(string));
            dt.Columns.Add("XepLoai", typeof(string));

            int stt = 1;
            foreach (var sv in danhSach)
            {
                dt.Rows.Add(stt++, sv.MaSV, sv.HoTen, sv.TenLop, sv.Diem, sv.KetQua, sv.XepLoai);
            }

            _dgvBaoCao.DataSource = dt;

            if (_dgvBaoCao.Columns.Count > 0)
            {
                _dgvBaoCao.Columns["STT"]!.HeaderText = "STT";
                _dgvBaoCao.Columns["STT"]!.FillWeight = 35;
                _dgvBaoCao.Columns["STT"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                _dgvBaoCao.Columns["MaSV"]!.HeaderText = "Mã Sinh Viên";
                _dgvBaoCao.Columns["MaSV"]!.FillWeight = 85;
                _dgvBaoCao.Columns["MaSV"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                _dgvBaoCao.Columns["HoTen"]!.HeaderText = "Họ và Tên";
                _dgvBaoCao.Columns["HoTen"]!.FillWeight = 140;

                _dgvBaoCao.Columns["TenLop"]!.HeaderText = "Lớp Học";
                _dgvBaoCao.Columns["TenLop"]!.FillWeight = 80;

                _dgvBaoCao.Columns["Diem"]!.HeaderText = "Điểm TB";
                _dgvBaoCao.Columns["Diem"]!.FillWeight = 60;
                _dgvBaoCao.Columns["Diem"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                _dgvBaoCao.Columns["Diem"]!.DefaultCellStyle.Format = "0.0";

                _dgvBaoCao.Columns["KetQua"]!.HeaderText = "Kết Quả";
                _dgvBaoCao.Columns["KetQua"]!.FillWeight = 70;
                _dgvBaoCao.Columns["KetQua"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                _dgvBaoCao.Columns["XepLoai"]!.HeaderText = "Xếp Loại";
                _dgvBaoCao.Columns["XepLoai"]!.FillWeight = 75;
                _dgvBaoCao.Columns["XepLoai"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        /// <summary>
        /// Cập nhật các thẻ KPI thống kê tổng quan bằng LINQ Aggregations.
        /// </summary>
        public void CapNhatThongKe(int soDongHienThi)
        {
            var stats = ReportEngine.ThongKeTongQuan(_duLieuDangHienThi);

            _lblCardTongSVValue.Text = stats.TongSo.ToString();
            _lblCardTongSVSub.Text = $"Hiển thị: {soDongHienThi} SV";

            _lblCardDatValue.Text = stats.SoDat.ToString();
            _lblCardDatSub.Text = $"Tỉ lệ đạt: {stats.TiLeDat:0.0}%";

            _lblCardKhongDatValue.Text = stats.SoKhongDat.ToString();
            _lblCardKhongDatSub.Text = stats.SoKhongDat > 0 ? "Chưa đạt yêu cầu" : "100% sinh viên đạt";

            _lblCardDiemTBValue.Text = stats.DiemTB.ToString("0.00");
            _lblCardDiemTBSub.Text = stats.TongSo > 0
                ? $"Cao nhất: {stats.DiemMax:0.0} | Thấp: {stats.DiemMin:0.0}"
                : "Không có dữ liệu";
        }

        /// <summary>
        /// Thống kê chi tiết theo từng lớp học bằng LINQ GroupBy.
        /// </summary>
        public void ThongKeTheoLop()
        {
            _dangXemThongKe = true;
            _pnlAlertBanner.Visible = false;

            var dsThongKe = ReportEngine.ThongKeTheoLop(_danhSachSinhVien);
            _duLieuThongKeDangHienThi = dsThongKe;

            _dgvBaoCao.DataSource = null;
            _dgvBaoCao.Columns.Clear();

            var dt = new DataTable();
            dt.Columns.Add("MaLop", typeof(string));
            dt.Columns.Add("TenLop", typeof(string));
            dt.Columns.Add("TongSoSV", typeof(int));
            dt.Columns.Add("SoDat", typeof(int));
            dt.Columns.Add("SoKhongDat", typeof(int));
            dt.Columns.Add("TiLeDat", typeof(string));
            dt.Columns.Add("DiemTrungBinh", typeof(double));
            dt.Columns.Add("DiemCaoNhat", typeof(double));
            dt.Columns.Add("DiemThapNhat", typeof(double));

            foreach (var tk in dsThongKe)
            {
                dt.Rows.Add(
                    tk.MaLop,
                    tk.TenLop,
                    tk.TongSoSV,
                    tk.SoDat,
                    tk.SoKhongDat,
                    tk.TiLeDatHienThi,
                    tk.DiemTrungBinh,
                    tk.DiemCaoNhat,
                    tk.DiemThapNhat
                );
            }

            _dgvBaoCao.DataSource = dt;

            if (_dgvBaoCao.Columns.Count > 0)
            {
                _dgvBaoCao.Columns["MaLop"]!.HeaderText = "Mã Lớp";
                _dgvBaoCao.Columns["MaLop"]!.FillWeight = 70;
                _dgvBaoCao.Columns["MaLop"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                _dgvBaoCao.Columns["TenLop"]!.HeaderText = "Tên Lớp";
                _dgvBaoCao.Columns["TenLop"]!.FillWeight = 110;

                _dgvBaoCao.Columns["TongSoSV"]!.HeaderText = "Tổng SV";
                _dgvBaoCao.Columns["TongSoSV"]!.FillWeight = 60;
                _dgvBaoCao.Columns["TongSoSV"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                _dgvBaoCao.Columns["SoDat"]!.HeaderText = "Số Đạt";
                _dgvBaoCao.Columns["SoDat"]!.FillWeight = 60;
                _dgvBaoCao.Columns["SoDat"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                _dgvBaoCao.Columns["SoKhongDat"]!.HeaderText = "Số K.Đạt";
                _dgvBaoCao.Columns["SoKhongDat"]!.FillWeight = 60;
                _dgvBaoCao.Columns["SoKhongDat"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                _dgvBaoCao.Columns["TiLeDat"]!.HeaderText = "Tỉ Lệ Đạt";
                _dgvBaoCao.Columns["TiLeDat"]!.FillWeight = 70;
                _dgvBaoCao.Columns["TiLeDat"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                _dgvBaoCao.Columns["DiemTrungBinh"]!.HeaderText = "Điểm TB";
                _dgvBaoCao.Columns["DiemTrungBinh"]!.FillWeight = 65;
                _dgvBaoCao.Columns["DiemTrungBinh"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                _dgvBaoCao.Columns["DiemTrungBinh"]!.DefaultCellStyle.Format = "0.00";

                _dgvBaoCao.Columns["DiemCaoNhat"]!.HeaderText = "Điểm Max";
                _dgvBaoCao.Columns["DiemCaoNhat"]!.FillWeight = 65;
                _dgvBaoCao.Columns["DiemCaoNhat"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                _dgvBaoCao.Columns["DiemThapNhat"]!.HeaderText = "Điểm Min";
                _dgvBaoCao.Columns["DiemThapNhat"]!.FillWeight = 65;
                _dgvBaoCao.Columns["DiemThapNhat"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // Cập nhật card tổng quan cho toàn trường
            int tongTruong = _danhSachSinhVien.Count;
            int datTruong = _danhSachSinhVien.Count(sv => sv.Diem >= 5.0);
            int khongDatTruong = tongTruong - datTruong;
            double dtbTruong = _danhSachSinhVien.Average(sv => sv.Diem);

            _lblCardTongSVValue.Text = tongTruong.ToString();
            _lblCardTongSVSub.Text = $"Gồm {dsThongKe.Count} lớp học";

            _lblCardDatValue.Text = datTruong.ToString();
            _lblCardDatSub.Text = $"Tỉ lệ: {(double)datTruong / tongTruong * 100.0:0.0}%";

            _lblCardKhongDatValue.Text = khongDatTruong.ToString();
            _lblCardKhongDatSub.Text = "Toàn khóa học";

            _lblCardDiemTBValue.Text = dtbTruong.ToString("0.00");
            _lblCardDiemTBSub.Text = "Trung bình toàn khóa";

            _statusLabel.Text = $"Đã thống kê thành công {dsThongKe.Count} lớp bằng LINQ GroupBy.";
        }

        /// <summary>
        /// Xuất dữ liệu đang hiển thị ra tệp tin CSV hoặc TXT có xử lý ngoại lệ.
        /// </summary>
        public void XuatFile(string? forcedFilePath = null)
        {
            if (_dgvBaoCao.Rows.Count == 0)
            {
                if (!SuppressMessageBox)
                {
                    MessageBox.Show(
                        "Không có dữ liệu để xuất file.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                return;
            }

            string savePath = forcedFilePath ?? string.Empty;

            if (string.IsNullOrEmpty(savePath))
            {
                using var sfd = new SaveFileDialog
                {
                    Title = "Chọn nơi lưu tệp tin báo cáo",
                    Filter = "CSV File (*.csv)|*.csv|Văn bản Báo cáo TXT (*.txt)|*.txt",
                    FileName = _dangXemThongKe
                        ? $"ThongKeLop_{DateTime.Now:yyyyMMdd_HHmm}.csv"
                        : $"BaoCaoSinhVien_{DateTime.Now:yyyyMMdd_HHmm}.csv"
                };

                if (sfd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                savePath = sfd.FileName;
            }

            try
            {
                string ext = Path.GetExtension(savePath).ToLower();
                if (ext == ".csv")
                {
                    if (_dangXemThongKe)
                    {
                        ReportEngine.XuatFileCsvThongKe(_duLieuThongKeDangHienThi, savePath);
                    }
                    else
                    {
                        ReportEngine.XuatFileCsv(_duLieuDangHienThi, savePath);
                    }
                }
                else
                {
                    string tenLop = _cboLop.Text;
                    string dieuKien = _cboKetQua.SelectedItem?.ToString() ?? "Tất cả";
                    string tuKhoa = _txtTuKhoa.Text.Trim();
                    ReportEngine.XuatFileTxt(_duLieuDangHienThi, tenLop, dieuKien, tuKhoa, savePath);
                }

                _statusLabel.Text = $"Đã xuất tệp thành công: {Path.GetFileName(savePath)} ({DateTime.Now:HH:mm:ss})";
                _pnlAlertBanner.Visible = true;
                _pnlAlertBanner.BackColor = ColorTranslator.FromHtml("#ECFDF5");
                _lblAlertBannerText.ForeColor = ColorTranslator.FromHtml("#065F46");
                _lblAlertBannerText.Text = $"✅ Đã xuất báo cáo thành công ra tệp tin: {Path.GetFileName(savePath)}";

                if (!SuppressMessageBox)
                {
                    MessageBox.Show(
                        $"Xuất báo cáo thành công ra file:\n{savePath}",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _statusLabel.Text = $"Lỗi khi xuất file: {ex.Message}";
                if (!SuppressMessageBox)
                {
                    MessageBox.Show(
                        $"Lỗi trong quá trình xuất tệp tin:\n{ex.Message}",
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void ResetBoLoc()
        {
            _cboLop.SelectedIndex = 0; // Tất cả các lớp
            _cboKetQua.SelectedIndex = 0; // Tất cả
            _txtTuKhoa.Clear();
            XemBaoCao();
        }

        // ==========================================
        // SỰ KIỆN GIAO DIỆN
        // ==========================================
        private void btnXemBaoCao_Click(object? sender, EventArgs e)
        {
            XemBaoCao();
        }

        private void btnThongKe_Click(object? sender, EventArgs e)
        {
            ThongKeTheoLop();
        }

        private void btnXuatFile_Click(object? sender, EventArgs e)
        {
            XuatFile();
        }

        private void cboLop_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_cboLop.SelectedValue != null)
            {
                XemBaoCao();
            }
        }

        private void cboKetQua_SelectedIndexChanged(object? sender, EventArgs e)
        {
            XemBaoCao();
        }

        private void txtTuKhoa_TextChanged(object? sender, EventArgs e)
        {
            XemBaoCao();
        }

        private void DgvBaoCao_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _dgvBaoCao.Rows.Count) return;

            string colName = _dgvBaoCao.Columns[e.ColumnIndex].Name;

            if (colName == "KetQua")
            {
                string val = e.Value?.ToString() ?? "";
                if (val == "Đạt")
                {
                    e.CellStyle!.ForeColor = ColorTranslator.FromHtml("#15803D");
                    e.CellStyle.Font = new Font(_dgvBaoCao.Font, FontStyle.Bold);
                }
                else if (val == "Không đạt")
                {
                    e.CellStyle!.ForeColor = ColorTranslator.FromHtml("#DC2626");
                    e.CellStyle.Font = new Font(_dgvBaoCao.Font, FontStyle.Bold);
                }
            }
            else if (colName == "XepLoai")
            {
                string xl = e.Value?.ToString() ?? "";
                if (xl == "Xuất sắc" || xl == "Giỏi")
                {
                    e.CellStyle!.ForeColor = ColorTranslator.FromHtml("#0D9488");
                    e.CellStyle.Font = new Font(_dgvBaoCao.Font, FontStyle.Bold);
                }
            }
            else if (colName == "Diem" && e.Value is double d)
            {
                e.CellStyle!.Font = new Font(_dgvBaoCao.Font, FontStyle.Bold);
            }
        }
    }
}
