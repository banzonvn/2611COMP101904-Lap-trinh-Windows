using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using EduManager.BLL;
using EduManager.Models;

namespace EduManager.GUI
{
    /// <summary>
    /// Giao diện Quản lý Môn học (Presentation Layer - GUI).
    /// Tuân thủ nghiêm ngặt mô hình ba lớp:
    /// - Không viết trực tiếp câu lệnh SQL.
    /// - Không chứa DbContext hay xử lý truy cập database.
    /// - Ủy quyền toàn bộ logic thẩm định dữ liệu cho BLL.
    /// - Thiết kế giao diện Warm Slate & Modern Sage, không dùng màu xanh đậm, không chồng chữ.
    /// </summary>
    public partial class FrmMonHoc : Form
    {
        private readonly MonHocBLL _monHocBLL;
        private List<MonHoc> _danhSachHienTai = new List<MonHoc>();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool SuppressMessageBox { get; set; } = false;

        // --- Controls theo yêu cầu đề bài ---
        private Panel _pnlHeader = null!;
        private Label _lblHeaderTag = null!;
        private Label _lblHeaderTitle = null!;
        private Label _lblHeaderSubtitle = null!;
        private Label _lblUserBadge = null!;

        private Panel _pnlKpiContainer = null!;
        private Panel _cardTongMon = null!;
        private Label _lblCardTongMonVal = null!;
        private Label _lblCardTongMonSub = null!;

        private Panel _cardConDung = null!;
        private Label _lblCardConDungVal = null!;
        private Label _lblCardConDungSub = null!;

        private Panel _cardNgungDung = null!;
        private Label _lblCardNgungDungVal = null!;
        private Label _lblCardNgungDungSub = null!;

        private Panel _cardTinChi = null!;
        private Label _lblCardTinChiVal = null!;
        private Label _lblCardTinChiSub = null!;

        private Panel _pnlEditorCard = null!;
        private Label _lblMaMonHoc = null!;
        private TextBox _txtMaMonHoc = null!;
        private Label _lblTenMonHoc = null!;
        private TextBox _txtTenMonHoc = null!;
        private Label _lblSoTinChi = null!;
        private NumericUpDown _nudSoTinChi = null!;
        private CheckBox _chkTrangThai = null!;

        private Button _btnThem = null!;
        private Button _btnSua = null!;
        private Button _btnXoa = null!;
        private Button _btnLamMoi = null!;

        private Label _lblTimKiem = null!;
        private TextBox _txtTimKiem = null!;
        private Button _btnTimKiem = null!;

        private Panel _pnlGridWrapper = null!;
        private Panel _pnlAlertBanner = null!;
        private Label _lblAlertBannerText = null!;
        private DataGridView _dgvMonHoc = null!;

        private StatusStrip _statusStrip = null!;
        private ToolStripStatusLabel _statusLabel = null!;
        private ToolStripStatusLabel _statusSpring = null!;
        private ToolStripStatusLabel _statusAuthor = null!;

        // Public aliases để khớp hoàn toàn với tên control yêu cầu trong đề bài Lab 14
        public TextBox txtMaMonHoc => _txtMaMonHoc;
        public TextBox txtTenMonHoc => _txtTenMonHoc;
        public NumericUpDown nudSoTinChi => _nudSoTinChi;
        public CheckBox chkTrangThai => _chkTrangThai;
        public TextBox txtTimKiem => _txtTimKiem;
        public DataGridView dgvMonHoc => _dgvMonHoc;
        public Button btnThem => _btnThem;
        public Button btnSua => _btnSua;
        public Button btnXoa => _btnXoa;
        public Button btnLamMoi => _btnLamMoi;
        public Button btnTimKiem => _btnTimKiem;

        public FrmMonHoc()
        {
            _monHocBLL = new MonHocBLL();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            // Thiết lập kích thước rộng rãi, hiện đại
            this.Text = "Hệ thống EduManager - Quản lý môn học (EF Core & 3-Layer) | Lê Hoàng Quân";
            this.Size = new Size(1180, 750);
            this.MinimumSize = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ColorTranslator.FromHtml("#F8FAFC");
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            // ==========================================
            // 1. TOP HEADER BANNER (Màu sáng, vạch Sage ngọc bích, không xanh đậm)
            // ==========================================
            _pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 90,
                BackColor = Color.White,
                Padding = new Padding(24, 10, 24, 10)
            };
            _pnlHeader.Paint += (s, e) =>
            {
                using var p = new Pen(ColorTranslator.FromHtml("#E2E8F0"), 1);
                e.Graphics.DrawLine(p, 0, _pnlHeader.Height - 1, _pnlHeader.Width, _pnlHeader.Height - 1);
                // Vạch accent Sage tinh tế
                using var topBrush = new SolidBrush(ColorTranslator.FromHtml("#0D9488"));
                e.Graphics.FillRectangle(topBrush, 0, 0, _pnlHeader.Width, 3);
            };

            _lblHeaderTag = new Label
            {
                Text = "LAB 14 • ENTITY FRAMEWORK CORE & 3-LAYER ARCHITECTURE",
                Font = new Font("Segoe UI", 8.25f, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#0F766E"),
                BackColor = ColorTranslator.FromHtml("#F0FDFA"),
                Location = new Point(24, 12),
                Size = new Size(390, 22),
                TextAlign = ContentAlignment.MiddleCenter,
                UseMnemonic = false
            };

            _lblHeaderTitle = new Label
            {
                Text = "QUẢN LÝ MÔN HỌC - HỆ THỐNG EDUMANAGER",
                Font = new Font("Segoe UI", 13.5f, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#0F172A"),
                Location = new Point(24, 38),
                AutoSize = true
            };

            _lblHeaderSubtitle = new Label
            {
                Text = "Học phần COMP1019 - Lập trình trên Windows • Giảng viên: ThS. Lê Thanh Thoại",
                Font = new Font("Segoe UI", 9.25f, FontStyle.Regular),
                ForeColor = ColorTranslator.FromHtml("#64748B"),
                Location = new Point(24, 66),
                AutoSize = true
            };

            _lblUserBadge = new Label
            {
                Text = "Sinh viên thực hiện:\nLê Hoàng Quân - 51.01.104.082",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#334155"),
                BackColor = ColorTranslator.FromHtml("#F1F5F9"),
                Size = new Size(250, 56),
                Location = new Point(890, 16),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                TextAlign = ContentAlignment.MiddleCenter,
                UseMnemonic = false
            };

            _pnlHeader.Controls.AddRange(new Control[] { _lblHeaderTag, _lblHeaderTitle, _lblHeaderSubtitle, _lblUserBadge });
            _pnlHeader.Resize += (s, e) =>
            {
                _lblUserBadge.Location = new Point(_pnlHeader.Width - 250 - 24, 16);
            };

            // ==========================================
            // 2. KPI METRIC CARDS (4 thẻ chỉ số thông thoáng, không chồng chữ)
            // ==========================================
            _pnlKpiContainer = new Panel
            {
                Dock = DockStyle.Top,
                Height = 84,
                BackColor = ColorTranslator.FromHtml("#F8FAFC"),
                Padding = new Padding(24, 6, 24, 6)
            };

            _cardTongMon = CreateKpiCard("TỔNG SỐ MÔN HỌC", "0", "Đang quản lý",
                ColorTranslator.FromHtml("#FFFFFF"), ColorTranslator.FromHtml("#E2E8F0"),
                ColorTranslator.FromHtml("#475569"), ColorTranslator.FromHtml("#0F172A"),
                out _, out _lblCardTongMonVal, out _lblCardTongMonSub);

            _cardConDung = CreateKpiCard("ĐANG ÁP DỤNG", "0", "Môn còn hiệu lực",
                ColorTranslator.FromHtml("#F0FDF4"), ColorTranslator.FromHtml("#BBF7D0"),
                ColorTranslator.FromHtml("#15803D"), ColorTranslator.FromHtml("#166534"),
                out _, out _lblCardConDungVal, out _lblCardConDungSub);

            _cardNgungDung = CreateKpiCard("NGỪNG ÁP DỤNG", "0", "Môn tạm ngưng",
                ColorTranslator.FromHtml("#FEF2F2"), ColorTranslator.FromHtml("#FECACA"),
                ColorTranslator.FromHtml("#B91C1C"), ColorTranslator.FromHtml("#991B1B"),
                out _, out _lblCardNgungDungVal, out _lblCardNgungDungSub);

            _cardTinChi = CreateKpiCard("TÍN CHỈ TRUNG BÌNH", "0.0", "Tổng: 0 TC",
                ColorTranslator.FromHtml("#FFFBEB"), ColorTranslator.FromHtml("#FDE68A"),
                ColorTranslator.FromHtml("#B45309"), ColorTranslator.FromHtml("#92400E"),
                out _, out _lblCardTinChiVal, out _lblCardTinChiSub);

            _pnlKpiContainer.Controls.AddRange(new Control[] { _cardTongMon, _cardConDung, _cardNgungDung, _cardTinChi });
            _pnlKpiContainer.Resize += (s, e) => LayoutKpiCards();

            // ==========================================
            // 3. EDITOR CARD & SEARCH TOOLBAR (Rộng rãi, AutoSize chống đè chữ)
            // ==========================================
            _pnlEditorCard = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                BackColor = Color.White,
                Padding = new Padding(24, 10, 24, 10)
            };
            _pnlEditorCard.Paint += (s, e) =>
            {
                using var p = new Pen(ColorTranslator.FromHtml("#E2E8F0"), 1);
                e.Graphics.DrawLine(p, 0, _pnlEditorCard.Height - 1, _pnlEditorCard.Width, _pnlEditorCard.Height - 1);
            };

            // Dòng 1: Nhập liệu Môn học
            _lblMaMonHoc = new Label
            {
                Text = "Mã môn học:",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#475569"),
                Location = new Point(24, 12),
                AutoSize = true
            };
            _txtMaMonHoc = new TextBox
            {
                Font = new Font("Segoe UI", 9.5f),
                Location = new Point(24, 34),
                Size = new Size(130, 30),
                CharacterCasing = CharacterCasing.Upper,
                MaxLength = 10,
                PlaceholderText = "VD: COMP1019"
            };

            _lblTenMonHoc = new Label
            {
                Text = "Tên môn học:",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#475569"),
                Location = new Point(170, 12),
                AutoSize = true
            };
            _txtTenMonHoc = new TextBox
            {
                Font = new Font("Segoe UI", 9.5f),
                Location = new Point(170, 34),
                Size = new Size(270, 30),
                MaxLength = 100,
                PlaceholderText = "Nhập tên môn học..."
            };

            _lblSoTinChi = new Label
            {
                Text = "Số tín chỉ (1-6):",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#475569"),
                Location = new Point(460, 12),
                AutoSize = true
            };
            _nudSoTinChi = new NumericUpDown
            {
                Font = new Font("Segoe UI", 9.5f),
                Location = new Point(460, 34),
                Size = new Size(110, 30),
                Minimum = 1,
                Maximum = 6,
                Value = 3,
                TextAlign = HorizontalAlignment.Center
            };

            _chkTrangThai = new CheckBox
            {
                Text = "Còn sử dụng",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#0F766E"),
                Location = new Point(585, 36),
                AutoSize = true,
                Checked = true,
                Cursor = Cursors.Hand
            };

            // Các nút thao tác CRUD
            _btnThem = new Button
            {
                Text = "Thêm Mới",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#0D9488"), // Warm Teal
                FlatStyle = FlatStyle.Flat,
                Location = new Point(715, 32),
                Size = new Size(95, 33),
                Cursor = Cursors.Hand
            };
            _btnThem.FlatAppearance.BorderSize = 0;
            _btnThem.Click += btnThem_Click;

            _btnSua = new Button
            {
                Text = "Cập Nhật",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#475569"), // Slate
                FlatStyle = FlatStyle.Flat,
                Location = new Point(820, 32),
                Size = new Size(95, 33),
                Cursor = Cursors.Hand
            };
            _btnSua.FlatAppearance.BorderSize = 0;
            _btnSua.Click += btnSua_Click;

            _btnXoa = new Button
            {
                Text = "Xóa Môn",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#E11D48"), // Rose Coral
                FlatStyle = FlatStyle.Flat,
                Location = new Point(925, 32),
                Size = new Size(95, 33),
                Cursor = Cursors.Hand
            };
            _btnXoa.FlatAppearance.BorderSize = 0;
            _btnXoa.Click += btnXoa_Click;

            _btnLamMoi = new Button
            {
                Text = "Làm Mới",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#475569"),
                BackColor = ColorTranslator.FromHtml("#F1F5F9"),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(1030, 32),
                Size = new Size(95, 33),
                Cursor = Cursors.Hand
            };
            _btnLamMoi.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#CBD5E1");
            _btnLamMoi.Click += btnLamMoi_Click;

            // Dòng 2: Thanh tìm kiếm môn học
            _lblTimKiem = new Label
            {
                Text = "Tìm kiếm:",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#475569"),
                Location = new Point(24, 78),
                AutoSize = true
            };
            _txtTimKiem = new TextBox
            {
                Font = new Font("Segoe UI", 9.5f),
                Location = new Point(100, 75),
                Size = new Size(340, 30),
                PlaceholderText = "Nhập mã hoặc tên môn học cần tìm kiếm..."
            };
            _txtTimKiem.TextChanged += txtTimKiem_TextChanged;

            _btnTimKiem = new Button
            {
                Text = "Tìm Kiếm",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#D97706"), // Warm Amber
                FlatStyle = FlatStyle.Flat,
                Location = new Point(460, 73),
                Size = new Size(110, 33),
                Cursor = Cursors.Hand
            };
            _btnTimKiem.FlatAppearance.BorderSize = 0;
            _btnTimKiem.Click += btnTimKiem_Click;

            _pnlEditorCard.Controls.AddRange(new Control[]
            {
                _lblMaMonHoc, _txtMaMonHoc,
                _lblTenMonHoc, _txtTenMonHoc,
                _lblSoTinChi, _nudSoTinChi,
                _chkTrangThai,
                _btnThem, _btnSua, _btnXoa, _btnLamMoi,
                _lblTimKiem, _txtTimKiem, _btnTimKiem
            });

            // ==========================================
            // 4. MAIN DATAGRIDVIEW + ALERT BANNER
            // ==========================================
            _pnlGridWrapper = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ColorTranslator.FromHtml("#F8FAFC"),
                Padding = new Padding(24, 6, 24, 12)
            };

            // Banner thông báo lỗi / thành công nội bộ
            _pnlAlertBanner = new Panel
            {
                Dock = DockStyle.Top,
                Height = 38,
                BackColor = ColorTranslator.FromHtml("#FEF3C7"),
                Visible = false,
                Padding = new Padding(12, 6, 12, 6)
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
                Font = new Font("Segoe UI", 9.25f, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#92400E"),
                TextAlign = ContentAlignment.MiddleLeft,
                Text = "Thông báo hệ thống."
            };
            _pnlAlertBanner.Controls.Add(_lblAlertBannerText);

            // DataGridView dgvMonHoc
            _dgvMonHoc = new DataGridView
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
            _dgvMonHoc.ColumnHeadersHeight = 38;
            _dgvMonHoc.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _dgvMonHoc.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ColorTranslator.FromHtml("#F1F5F9"),
                ForeColor = ColorTranslator.FromHtml("#334155"),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 8, 0)
            };
            _dgvMonHoc.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = ColorTranslator.FromHtml("#1E293B"),
                SelectionBackColor = ColorTranslator.FromHtml("#E0F2FE"),
                SelectionForeColor = ColorTranslator.FromHtml("#0369A1"),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                Padding = new Padding(8, 4, 8, 4)
            };
            _dgvMonHoc.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ColorTranslator.FromHtml("#F8FAFC")
            };
            _dgvMonHoc.RowTemplate.Height = 34;
            _dgvMonHoc.CellClick += dgvMonHoc_CellClick;
            _dgvMonHoc.CellFormatting += dgvMonHoc_CellFormatting;

            _pnlGridWrapper.Controls.Add(_dgvMonHoc);
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
                Text = "COMP1019 - Buổi 14: EF Core & 3-Layer Architecture | MSSV: 51.01.104.082",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                ForeColor = ColorTranslator.FromHtml("#64748B")
            };
            _statusStrip.Items.AddRange(new ToolStripItem[] { _statusLabel, _statusSpring, _statusAuthor });

            // Thêm vào Form theo đúng thứ tự WinForms Dock
            this.Controls.Add(_pnlGridWrapper);
            this.Controls.Add(_pnlEditorCard);
            this.Controls.Add(_pnlKpiContainer);
            this.Controls.Add(_pnlHeader);
            this.Controls.Add(_statusStrip);
            _statusStrip.SendToBack();
            _pnlGridWrapper.BringToFront();

            this.Load += FrmMonHoc_Load;
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

            _cardTongMon.SetBounds(24, 6, cardW, 72);
            _cardConDung.SetBounds(24 + cardW + spacing, 6, cardW, 72);
            _cardNgungDung.SetBounds(24 + (cardW + spacing) * 2, 6, cardW, 72);
            _cardTinChi.SetBounds(24 + (cardW + spacing) * 3, 6, cardW, 72);

            foreach (var card in new[] { _cardTongMon, _cardConDung, _cardNgungDung, _cardTinChi })
            {
                foreach (Control c in card.Controls)
                {
                    c.Width = card.Width - 24;
                }
            }
        }

        private void FrmMonHoc_Load(object? sender, EventArgs e)
        {
            LoadDanhSachMonHoc();
            LayoutKpiCards();
        }

        /// <summary>
        /// Nạp toàn bộ danh sách môn học từ tầng BLL lên DataGridView.
        /// </summary>
        public void LoadDanhSachMonHoc()
        {
            try
            {
                _danhSachHienTai = _monHocBLL.GetAll();
                HienThiDanhSach(_danhSachHienTai);
                CapNhatKpiCards(_danhSachHienTai);
                _statusLabel.Text = $"Đã tải thành công {_danhSachHienTai.Count} môn học từ SQL Server (EF Core).";
            }
            catch (Exception ex)
            {
                ShowAlert(ex.Message, isError: true);
            }
        }

        private void HienThiDanhSach(List<MonHoc> danhSach)
        {
            _dgvMonHoc.DataSource = null;
            _dgvMonHoc.Columns.Clear();

            var dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("MaMonHoc", typeof(string));
            dt.Columns.Add("TenMonHoc", typeof(string));
            dt.Columns.Add("SoTinChi", typeof(int));
            dt.Columns.Add("TrangThai", typeof(string));

            int stt = 1;
            foreach (var m in danhSach)
            {
                dt.Rows.Add(stt++, m.MaMonHoc, m.TenMonHoc, m.SoTinChi, m.TinhTrangHienThi);
            }

            _dgvMonHoc.DataSource = dt;

            if (_dgvMonHoc.Columns.Count > 0)
            {
                _dgvMonHoc.Columns["STT"]!.HeaderText = "STT";
                _dgvMonHoc.Columns["STT"]!.FillWeight = 40;
                _dgvMonHoc.Columns["STT"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                _dgvMonHoc.Columns["MaMonHoc"]!.HeaderText = "Mã Môn Học";
                _dgvMonHoc.Columns["MaMonHoc"]!.FillWeight = 85;
                _dgvMonHoc.Columns["MaMonHoc"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                _dgvMonHoc.Columns["TenMonHoc"]!.HeaderText = "Tên Môn Học";
                _dgvMonHoc.Columns["TenMonHoc"]!.FillWeight = 220;

                _dgvMonHoc.Columns["SoTinChi"]!.HeaderText = "Số Tín Chỉ";
                _dgvMonHoc.Columns["SoTinChi"]!.FillWeight = 70;
                _dgvMonHoc.Columns["SoTinChi"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                _dgvMonHoc.Columns["TrangThai"]!.HeaderText = "Trạng Thái";
                _dgvMonHoc.Columns["TrangThai"]!.FillWeight = 85;
                _dgvMonHoc.Columns["TrangThai"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void CapNhatKpiCards(List<MonHoc> list)
        {
            var stats = _monHocBLL.GetStatistics(list);
            _lblCardTongMonVal.Text = stats.TongMon.ToString();
            _lblCardTongMonSub.Text = "Đang quản lý";

            _lblCardConDungVal.Text = stats.ConDung.ToString();
            _lblCardConDungSub.Text = stats.TongMon > 0 ? $"{stats.ConDung * 100.0 / stats.TongMon:0.0}% tổng môn" : "0%";

            _lblCardNgungDungVal.Text = stats.NgungDung.ToString();
            _lblCardNgungDungSub.Text = stats.NgungDung > 0 ? "Tạm ngưng đào tạo" : "0 môn tạm ngưng";

            _lblCardTinChiVal.Text = stats.TinChiTB.ToString("0.0");
            _lblCardTinChiSub.Text = $"Tổng số: {stats.TongTinChi} TC";
        }

        // ==========================================
        // CÁC THAO TÁC CRUD
        // ==========================================

        /// <summary>
        /// Thêm môn học mới thông qua tầng BLL.
        /// </summary>
        public void ThemMonHoc()
        {
            try
            {
                var monHoc = new MonHoc
                {
                    MaMonHoc = _txtMaMonHoc.Text,
                    TenMonHoc = _txtTenMonHoc.Text,
                    SoTinChi = (int)_nudSoTinChi.Value,
                    TrangThai = _chkTrangThai.Checked
                };

                // Gọi BLL kiểm tra và ủy quyền DAL lưu vào CSDL
                _monHocBLL.Add(monHoc);

                LoadDanhSachMonHoc();
                LamMoiForm(resetAlert: false);
                ShowAlert($"✅ Thêm môn học [{monHoc.MaMonHoc}] thành công!", isError: false);
                _statusLabel.Text = $"Đã thêm môn học {monHoc.MaMonHoc} thành công lúc {DateTime.Now:HH:mm:ss}.";

                if (!SuppressMessageBox)
                {
                    MessageBox.Show(
                        $"Thêm môn học [{monHoc.MaMonHoc}] thành công!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                ShowAlert(ex.Message, isError: true);
                if (!SuppressMessageBox)
                {
                    MessageBox.Show(ex.Message, "Lỗi kiểm tra dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// Sửa thông tin môn học thông qua tầng BLL.
        /// </summary>
        public void SuaMonHoc()
        {
            try
            {
                var monHoc = new MonHoc
                {
                    MaMonHoc = _txtMaMonHoc.Text,
                    TenMonHoc = _txtTenMonHoc.Text,
                    SoTinChi = (int)_nudSoTinChi.Value,
                    TrangThai = _chkTrangThai.Checked
                };

                _monHocBLL.Update(monHoc);

                LoadDanhSachMonHoc();
                ShowAlert($"✅ Cập nhật môn học [{monHoc.MaMonHoc}] thành công!", isError: false);
                _statusLabel.Text = $"Đã cập nhật môn học {monHoc.MaMonHoc} thành công.";

                if (!SuppressMessageBox)
                {
                    MessageBox.Show(
                        $"Cập nhật thông tin môn học [{monHoc.MaMonHoc}] thành công!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                ShowAlert(ex.Message, isError: true);
                if (!SuppressMessageBox)
                {
                    MessageBox.Show(ex.Message, "Lỗi cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// Xóa môn học sau khi xác nhận MessageBox theo yêu cầu đề bài.
        /// </summary>
        public void XoaMonHoc()
        {
            string maMon = _txtMaMonHoc.Text.Trim();
            if (string.IsNullOrWhiteSpace(maMon))
            {
                ShowAlert("Vui lòng chọn hoặc nhập mã môn học cần xóa.", isError: true);
                return;
            }

            if (!SuppressMessageBox)
            {
                var confirm = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa môn học [{maMon}] khỏi hệ thống?\nThao tác này không thể hoàn tác.",
                    "Xác nhận xóa môn học",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes) return;
            }

            try
            {
                _monHocBLL.Delete(maMon);

                LoadDanhSachMonHoc();
                LamMoiForm(resetAlert: false);
                ShowAlert($"✅ Đã xóa môn học [{maMon}] thành công!", isError: false);
                _statusLabel.Text = $"Đã xóa môn học {maMon} lúc {DateTime.Now:HH:mm:ss}.";

                if (!SuppressMessageBox)
                {
                    MessageBox.Show(
                        $"Đã xóa môn học [{maMon}] thành công!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                ShowAlert(ex.Message, isError: true);
                if (!SuppressMessageBox)
                {
                    MessageBox.Show(ex.Message, "Lỗi xóa dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Tìm kiếm môn học theo mã hoặc tên môn học.
        /// </summary>
        public void TimKiemMonHoc()
        {
            string tuKhoa = _txtTimKiem.Text.Trim();
            var ketQua = _monHocBLL.Search(tuKhoa);
            _danhSachHienTai = ketQua;
            HienThiDanhSach(ketQua);
            CapNhatKpiCards(ketQua);

            if (ketQua.Count == 0)
            {
                ShowAlert($"⚠️ Không tìm thấy môn học nào khớp với từ khóa \"{tuKhoa}\".", isError: true);
                _statusLabel.Text = $"Tìm kiếm \"{tuKhoa}\": 0 kết quả.";
            }
            else
            {
                ShowAlert($"✅ Tìm thấy {ketQua.Count} môn học phù hợp.", isError: false);
                _statusLabel.Text = $"Tìm kiếm \"{tuKhoa}\": {ketQua.Count} kết quả.";
            }
        }

        /// <summary>
        /// Làm mới các control nhập liệu và nạp lại toàn bộ dữ liệu.
        /// </summary>
        public void LamMoiForm(bool resetAlert = true)
        {
            _txtMaMonHoc.Clear();
            _txtMaMonHoc.ReadOnly = false;
            _txtTenMonHoc.Clear();
            _nudSoTinChi.Value = 3;
            _chkTrangThai.Checked = true;
            _txtTimKiem.Clear();
            if (resetAlert)
            {
                _pnlAlertBanner.Visible = false;
            }
            LoadDanhSachMonHoc();
            _statusLabel.Text = "Đã làm mới dữ liệu và sẵn sàng nhập môn học mới.";
        }

        private void ShowAlert(string message, bool isError)
        {
            _pnlAlertBanner.Visible = true;
            if (isError)
            {
                _pnlAlertBanner.BackColor = ColorTranslator.FromHtml("#FEF3C7");
                _lblAlertBannerText.ForeColor = ColorTranslator.FromHtml("#92400E");
            }
            else
            {
                _pnlAlertBanner.BackColor = ColorTranslator.FromHtml("#ECFDF5");
                _lblAlertBannerText.ForeColor = ColorTranslator.FromHtml("#065F46");
            }
            _lblAlertBannerText.Text = message;
            _pnlAlertBanner.Invalidate();
        }

        // ==========================================
        // SỰ KIỆN GIAO DIỆN
        // ==========================================
        private void btnThem_Click(object? sender, EventArgs e) => ThemMonHoc();
        private void btnSua_Click(object? sender, EventArgs e) => SuaMonHoc();
        private void btnXoa_Click(object? sender, EventArgs e) => XoaMonHoc();
        private void btnLamMoi_Click(object? sender, EventArgs e) => LamMoiForm();
        private void btnTimKiem_Click(object? sender, EventArgs e) => TimKiemMonHoc();
        private void txtTimKiem_TextChanged(object? sender, EventArgs e) => TimKiemMonHoc();

        private void dgvMonHoc_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _dgvMonHoc.Rows.Count) return;

            var row = _dgvMonHoc.Rows[e.RowIndex];
            string maMon = row.Cells["MaMonHoc"]?.Value?.ToString() ?? "";
            string tenMon = row.Cells["TenMonHoc"]?.Value?.ToString() ?? "";
            int soTinChi = Convert.ToInt32(row.Cells["SoTinChi"]?.Value ?? 3);
            string trangThaiStr = row.Cells["TrangThai"]?.Value?.ToString() ?? "";

            _txtMaMonHoc.Text = maMon;
            _txtTenMonHoc.Text = tenMon;
            _nudSoTinChi.Value = Math.Clamp(soTinChi, 1, 6);
            _chkTrangThai.Checked = trangThaiStr.Contains("Còn");

            _statusLabel.Text = $"Đang chọn môn học [{maMon}] - {tenMon}.";
        }

        private void dgvMonHoc_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _dgvMonHoc.Rows.Count) return;

            string colName = _dgvMonHoc.Columns[e.ColumnIndex].Name;

            if (colName == "TrangThai")
            {
                string val = e.Value?.ToString() ?? "";
                if (val.Contains("Còn"))
                {
                    e.CellStyle!.ForeColor = ColorTranslator.FromHtml("#15803D"); // Emerald green
                    e.CellStyle.Font = new Font(_dgvMonHoc.Font, FontStyle.Bold);
                }
                else
                {
                    e.CellStyle!.ForeColor = ColorTranslator.FromHtml("#DC2626"); // Red coral
                    e.CellStyle.Font = new Font(_dgvMonHoc.Font, FontStyle.Bold);
                }
            }
            else if (colName == "MaMonHoc")
            {
                e.CellStyle!.Font = new Font(_dgvMonHoc.Font, FontStyle.Bold);
                e.CellStyle.ForeColor = ColorTranslator.FromHtml("#0D9488"); // Teal accent
            }
            else if (colName == "SoTinChi")
            {
                e.CellStyle!.Font = new Font(_dgvMonHoc.Font, FontStyle.Bold);
            }
        }
    }
}
