using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace COMP1019_Lab10_EduManagerStudio
{
    public class FrmQueryStudio : Form
    {
        private readonly DatabaseEngine _engine = new();

        // UI Controls
        private Panel _pnlHeader = null!;
        private Label _lblTitle = null!;
        private Label _lblSub = null!;
        private Label _lblStudentBadge = null!;
        private Button _btnRun = null!;
        private Button _btnReset = null!;

        private Panel _pnlLeft = null!;
        private Label _lblLeftTitle = null!;
        private ListBox _lstQueries = null!;
        private TreeView _tvDbExplorer = null!;

        private Panel _pnlRight = null!;
        private Label _lblQueryName = null!;
        private Label _lblQueryDesc = null!;
        private RichTextBox _rtbSqlScript = null!;
        private TabControl _tabResults = null!;
        private TabPage _tabGrid = null!;
        private TabPage _tabMessages = null!;
        private TabPage _tabSchema = null!;

        private DataGridView _dgvResults = null!;
        private RichTextBox _rtbConsole = null!;
        private RichTextBox _rtbSchema = null!;

        private StatusStrip _statusStrip = null!;
        private ToolStripStatusLabel _tslStatus = null!;
        private ToolStripStatusLabel _tslRows = null!;
        private ToolStripStatusLabel _tslTime = null!;
        private ToolStripStatusLabel _tslDatabase = null!;

        public FrmQueryStudio()
        {
            InitializeComponent();
            PopulateExplorerTree();
            PopulateQueryList();
            SelectQuery(0);
        }

        private void InitializeComponent()
        {
            this.Text = "EduManager Studio - SQL Server Query Inspector | COMP1019 Lab 10 - Lê Hoàng Quân (51.01.104.082)";
            this.Size = new Size(1300, 850);
            this.MinimumSize = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            this.BackColor = Color.FromArgb(243, 244, 246);

            // 1. Header Panel
            _pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 85,
                BackColor = Color.FromArgb(15, 23, 42) // Dark Navy
            };
            _pnlHeader.Paint += PnlHeader_Paint;

            _lblTitle = new Label
            {
                Text = "EDUMANAGER DB - T-SQL QUERY & DATABASE STUDIO",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 13.5f, FontStyle.Bold),
                Location = new Point(20, 14),
                AutoSize = true
            };

            _lblSub = new Label
            {
                Text = "COMP1019 - Lập trình trên Windows | Lab 10: Thiết kế và cài đặt CSDL EduManagerDB",
                ForeColor = Color.FromArgb(148, 163, 184),
                Font = new Font("Segoe UI", 9.5f),
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

            _btnRun = new Button
            {
                Text = "▶ Thực Thi (F5)",
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(780, 48),
                Size = new Size(130, 30),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            _btnRun.FlatAppearance.BorderSize = 0;
            _btnRun.Click += (s, e) => RunCurrentQuery();

            _btnReset = new Button
            {
                Text = "⟲ Làm Mới CSDL",
                BackColor = Color.FromArgb(51, 65, 85),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.0f),
                Location = new Point(920, 48),
                Size = new Size(130, 30),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            _btnReset.FlatAppearance.BorderSize = 0;
            _btnReset.Click += (s, e) =>
            {
                _engine.ResetDatabase();
                RunCurrentQuery();
                MessageBox.Show("Đã khôi phục dữ liệu mẫu ban đầu của EduManagerDB thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            _pnlHeader.Controls.AddRange(new Control[] { _lblTitle, _lblSub, _lblStudentBadge, _btnRun, _btnReset });

            // 2. Status Strip
            _statusStrip = new StatusStrip
            {
                BackColor = Color.FromArgb(241, 245, 249),
                Font = new Font("Segoe UI", 9.0f)
            };

            _tslStatus = new ToolStripStatusLabel("Sẵn sàng thực thi truy vấn.") { Spring = true, TextAlign = ContentAlignment.MiddleLeft };
            _tslRows = new ToolStripStatusLabel("0 hàng") { BorderSides = ToolStripStatusLabelBorderSides.Left, BorderStyle = Border3DStyle.Etched };
            _tslTime = new ToolStripStatusLabel("Thời gian: 0 ms") { BorderSides = ToolStripStatusLabelBorderSides.Left, BorderStyle = Border3DStyle.Etched };
            _tslDatabase = new ToolStripStatusLabel("CSDL: EduManagerDB (LocalDB)") { BorderSides = ToolStripStatusLabelBorderSides.Left, BorderStyle = Border3DStyle.Etched, ForeColor = Color.FromArgb(30, 64, 175), Font = new Font("Segoe UI", 9.0f, FontStyle.Bold) };

            _statusStrip.Items.AddRange(new ToolStripItem[] { _tslStatus, _tslRows, _tslTime, _tslDatabase });

            // 3. Left Explorer Panel
            _pnlLeft = new Panel
            {
                Dock = DockStyle.Left,
                Width = 350,
                BackColor = Color.White,
                Padding = new Padding(12)
            };

            _lblLeftTitle = new Label
            {
                Text = "DANH SÁCH 12 TRUY VẤN YÊU CẦU",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Dock = DockStyle.Top,
                Height = 28
            };

            _lstQueries = new ListBox
            {
                Dock = DockStyle.Top,
                Height = 360,
                Font = new Font("Segoe UI", 9.2f),
                ItemHeight = 28,
                BorderStyle = BorderStyle.FixedSingle,
                DrawMode = DrawMode.OwnerDrawFixed
            };
            _lstQueries.DrawItem += LstQueries_DrawItem;
            _lstQueries.SelectedIndexChanged += (s, e) =>
            {
                if (_lstQueries.SelectedIndex >= 0)
                {
                    SelectQuery(_lstQueries.SelectedIndex);
                    RunCurrentQuery();
                }
            };

            var lblTreeTitle = new Label
            {
                Text = "CƠ SỞ DỮ LIỆU & LƯỢC ĐỒ (SCHEMA)",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Dock = DockStyle.Top,
                Height = 28,
                Padding = new Padding(0, 8, 0, 0)
            };

            _tvDbExplorer = new TreeView
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.0f),
                BorderStyle = BorderStyle.FixedSingle,
                ShowLines = true,
                ShowPlusMinus = true
            };

            _pnlLeft.Controls.Add(_tvDbExplorer);
            _pnlLeft.Controls.Add(lblTreeTitle);
            _pnlLeft.Controls.Add(_lstQueries);
            _pnlLeft.Controls.Add(_lblLeftTitle);

            // 4. Right Main Work Area
            _pnlRight = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding(14)
            };

            var pnlQueryHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 55,
                BackColor = Color.White,
                Padding = new Padding(12, 8, 12, 8)
            };

            _lblQueryName = new Label
            {
                Text = "Tên Truy Vấn",
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Dock = DockStyle.Top,
                Height = 22
            };

            _lblQueryDesc = new Label
            {
                Text = "Mô tả mục đích và kỹ thuật T-SQL sử dụng",
                Font = new Font("Segoe UI", 9.0f),
                ForeColor = Color.FromArgb(100, 116, 139),
                Dock = DockStyle.Fill
            };
            pnlQueryHeader.Controls.Add(_lblQueryDesc);
            pnlQueryHeader.Controls.Add(_lblQueryName);

            var lblSqlTitle = new Label
            {
                Text = "CÂU LỆNH T-SQL SCRIPT:",
                Font = new Font("Segoe UI", 8.8f, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                Dock = DockStyle.Top,
                Height = 22,
                Padding = new Padding(0, 6, 0, 0)
            };

            _rtbSqlScript = new RichTextBox
            {
                Dock = DockStyle.Top,
                Height = 160,
                Font = new Font("Consolas", 10.5f, FontStyle.Regular),
                BackColor = Color.FromArgb(30, 41, 59),
                ForeColor = Color.FromArgb(241, 245, 249),
                BorderStyle = BorderStyle.None,
                ReadOnly = true
            };

            var pnlSplitSpacer = new Panel
            {
                Dock = DockStyle.Top,
                Height = 10
            };

            _tabResults = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5f)
            };

            _tabGrid = new TabPage("Kết Quả Lưới Dữ Liệu (Results Grid)");
            _tabMessages = new TabPage("Thông Báo SQL Server (Messages)");
            _tabSchema = new TabPage("Lược Đồ & Ràng Buộc CSDL (Schema Inspector)");

            // DataGridView Setup
            _dgvResults = new DataGridView
            {
                Dock = DockStyle.Fill,
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
            _dgvResults.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(30, 41, 59),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(6)
            };
            _dgvResults.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.FromArgb(15, 23, 42),
                Font = new Font("Segoe UI", 9.2f),
                Padding = new Padding(5)
            };
            _dgvResults.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 250, 252)
            };
            _dgvResults.RowTemplate.Height = 32;

            _tabGrid.Controls.Add(_dgvResults);

            // Console output
            _rtbConsole = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10.0f),
                BackColor = Color.FromArgb(15, 23, 42),
                ForeColor = Color.FromArgb(226, 232, 240),
                BorderStyle = BorderStyle.None,
                ReadOnly = true
            };
            _tabMessages.Controls.Add(_rtbConsole);

            // Schema output
            _rtbSchema = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9.5f),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(15, 23, 42),
                BorderStyle = BorderStyle.None,
                ReadOnly = true
            };
            _tabSchema.Controls.Add(_rtbSchema);

            _tabResults.TabPages.Add(_tabGrid);
            _tabResults.TabPages.Add(_tabMessages);
            _tabResults.TabPages.Add(_tabSchema);

            _pnlRight.Controls.Add(_tabResults);
            _pnlRight.Controls.Add(pnlSplitSpacer);
            _pnlRight.Controls.Add(_rtbSqlScript);
            _pnlRight.Controls.Add(lblSqlTitle);
            _pnlRight.Controls.Add(pnlQueryHeader);

            // Add all to Form
            this.Controls.Add(_pnlRight);
            var splitter = new Splitter { Dock = DockStyle.Left, Width = 5, BackColor = Color.FromArgb(226, 232, 240) };
            this.Controls.Add(splitter);
            this.Controls.Add(_pnlLeft);
            this.Controls.Add(_pnlHeader);
            this.Controls.Add(_statusStrip);

            PopulateSchemaInspector();
        }

        private void PnlHeader_Paint(object? sender, PaintEventArgs e)
        {
            using var brush = new LinearGradientBrush(_pnlHeader.ClientRectangle,
                Color.FromArgb(15, 23, 42), Color.FromArgb(30, 41, 59), LinearGradientMode.Horizontal);
            e.Graphics.FillRectangle(brush, _pnlHeader.ClientRectangle);
        }

        private void PopulateExplorerTree()
        {
            _tvDbExplorer.Nodes.Clear();
            var root = new TreeNode("EduManagerDB (Cơ sở dữ liệu)") { ForeColor = Color.FromArgb(30, 64, 175) };

            var nTables = new TreeNode("Bảng Dữ Liệu (Tables - 6 bảng)");
            nTables.Nodes.Add("KHOA (MaKhoa, TenKhoa, DienThoai)");
            nTables.Nodes.Add("LOP (MaLop, TenLop, MaKhoa)");
            nTables.Nodes.Add("SINHVIEN (MaSV, HoTen, NgaySinh, GioiTinh, MaLop)");
            nTables.Nodes.Add("MONHOC (MaMH, TenMH, SoTinChi)");
            nTables.Nodes.Add("KETQUA (MaSV, MaMH, Diem)");
            nTables.Nodes.Add("TAIKHOAN (TenDangNhap, MatKhau, HoTen, VaiTro, TrangThai)");

            var nViews = new TreeNode("Khung Nhìn (Views - 2 views)");
            nViews.Nodes.Add("vw_DanhSachSinhVienDayDu");
            nViews.Nodes.Add("vw_BangDiemChiTiet");

            var nConstraints = new TreeNode("Ràng Buộc Toàn Vẹn (Constraints)");
            nConstraints.Nodes.Add("CK_SINHVIEN_GioiTinh (Nam/Nữ)");
            nConstraints.Nodes.Add("CK_MONHOC_SoTinChi (> 0)");
            nConstraints.Nodes.Add("CK_KETQUA_Diem (0.0 <= Diem <= 10.0)");
            nConstraints.Nodes.Add("FK_LOP_KHOA (ON UPDATE CASCADE)");
            nConstraints.Nodes.Add("FK_SINHVIEN_LOP (ON UPDATE CASCADE)");
            nConstraints.Nodes.Add("FK_KETQUA_SINHVIEN (ON DELETE CASCADE)");

            root.Nodes.Add(nTables);
            root.Nodes.Add(nViews);
            root.Nodes.Add(nConstraints);
            _tvDbExplorer.Nodes.Add(root);
            root.ExpandAll();
        }

        private void PopulateQueryList()
        {
            _lstQueries.Items.Clear();
            foreach (var q in _engine.PresetQueries)
            {
                _lstQueries.Items.Add(q.Title);
            }
        }

        private void LstQueries_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= _lstQueries.Items.Count) return;

            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            var backColor = isSelected ? Color.FromArgb(239, 246, 255) : Color.White;
            var textColor = isSelected ? Color.FromArgb(29, 78, 216) : Color.FromArgb(30, 41, 59);

            using var brush = new SolidBrush(backColor);
            e.Graphics.FillRectangle(brush, e.Bounds);

            if (isSelected)
            {
                using var p = new Pen(Color.FromArgb(59, 130, 246), 2);
                e.Graphics.DrawLine(p, e.Bounds.Left, e.Bounds.Top, e.Bounds.Left, e.Bounds.Bottom);
            }

            string text = _lstQueries.Items[e.Index].ToString()!;
            using var font = new Font("Segoe UI", 9.2f, isSelected ? FontStyle.Bold : FontStyle.Regular);
            using var textBrush = new SolidBrush(textColor);
            e.Graphics.DrawString(text, font, textBrush, e.Bounds.Left + 10, e.Bounds.Top + 5);
        }

        public void SelectQuery(int index)
        {
            if (index < 0 || index >= _engine.PresetQueries.Count) return;

            var q = _engine.PresetQueries[index];
            _lblQueryName.Text = q.Title;
            _lblQueryDesc.Text = $"{q.Description}  |  Mục tiêu: {q.Purpose}";
            _rtbSqlScript.Text = q.SqlScript;
            FormatSqlSyntax();
        }

        private void FormatSqlSyntax()
        {
            // Simple SQL keyword highlighting
            string[] keywords = { "SELECT", "FROM", "WHERE", "INNER JOIN", "LEFT JOIN", "ON", "ORDER BY",
                                  "GROUP BY", "HAVING", "UPDATE", "SET", "DELETE", "NOT EXISTS", "AS",
                                  "DECLARE", "NVARCHAR", "BEGIN TRY", "END TRY", "BEGIN CATCH", "END CATCH",
                                  "INSERT INTO", "VALUES", "COUNT", "AVG", "ROUND", "CAST", "CASE", "WHEN",
                                  "THEN", "ELSE", "END", "DESC", "ASC" };

            int selStart = _rtbSqlScript.SelectionStart;
            int selLength = _rtbSqlScript.SelectionLength;

            _rtbSqlScript.SelectAll();
            _rtbSqlScript.SelectionColor = Color.FromArgb(241, 245, 249);

            foreach (var kw in keywords)
            {
                int startIndex = 0;
                while (startIndex < _rtbSqlScript.TextLength)
                {
                    int wordIndex = _rtbSqlScript.Find(kw, startIndex, RichTextBoxFinds.WholeWord);
                    if (wordIndex != -1)
                    {
                        _rtbSqlScript.Select(wordIndex, kw.Length);
                        _rtbSqlScript.SelectionColor = Color.FromArgb(96, 165, 250); // Light blue
                        startIndex = wordIndex + kw.Length;
                    }
                    else
                        break;
                }
            }

            _rtbSqlScript.Select(selStart, selLength);
        }

        public void RunCurrentQuery()
        {
            int selectedIndex = _lstQueries.SelectedIndex;
            if (selectedIndex < 0) selectedIndex = 0;

            int queryId = selectedIndex + 1;
            var result = _engine.ExecuteQuery(queryId);

            if (result.DataTable != null)
            {
                _dgvResults.DataSource = result.DataTable;
                _tslRows.Text = $"{result.DataTable.Rows.Count} hàng trả về";
            }
            else
            {
                _dgvResults.DataSource = null;
                _tslRows.Text = $"{result.AffectedRows} hàng bị ảnh hưởng";
            }

            _rtbConsole.Text = result.ConsoleOutput;
            _tslStatus.Text = result.StatusMessage;
            _tslTime.Text = $"Thời gian: {result.ExecutionTimeMs:F1} ms";

            // If it's a constraint test (query 11 or 12), automatically focus Messages or Grid
            if (queryId == 11 || queryId == 12)
            {
                _tabResults.SelectedTab = _tabGrid;
            }
            else
            {
                _tabResults.SelectedTab = _tabGrid;
            }
        }

        private void PopulateSchemaInspector()
        {
            _rtbSchema.Text =
                "========================================================================================\n" +
                "LƯỢC ĐỒ QUAN HỆ CƠ SỞ DỮ LIỆU: EduManagerDB\n" +
                "Học phần: COMP1019 - Lập trình trên Windows | Khoa CNTT - ĐH Sư Phạm TP.HCM\n" +
                "Sinh viên: Lê Hoàng Quân | MSSV: 51.01.104.082 | Lớp: 51.CNTT.A\n" +
                "========================================================================================\n\n" +
                "1. BẢNG KHOA:\n" +
                "   - MaKhoa    VARCHAR(20) PRIMARY KEY\n" +
                "   - TenKhoa   NVARCHAR(100) NOT NULL\n" +
                "   - DienThoai VARCHAR(15)\n\n" +
                "2. BẢNG LOP:\n" +
                "   - MaLop     VARCHAR(20) PRIMARY KEY\n" +
                "   - TenLop    NVARCHAR(100) NOT NULL\n" +
                "   - MaKhoa    VARCHAR(20) FOREIGN KEY REFERENCES KHOA(MaKhoa) [ON UPDATE CASCADE]\n\n" +
                "3. BẢNG SINHVIEN:\n" +
                "   - MaSV      VARCHAR(20) PRIMARY KEY\n" +
                "   - HoTen     NVARCHAR(100) NOT NULL\n" +
                "   - NgaySinh  DATE\n" +
                "   - GioiTinh  NVARCHAR(10) CHECK (GioiTinh IN (N'Nam', N'Nữ'))\n" +
                "   - MaLop     VARCHAR(20) FOREIGN KEY REFERENCES LOP(MaLop) [ON UPDATE CASCADE]\n\n" +
                "4. BẢNG MONHOC:\n" +
                "   - MaMH      VARCHAR(20) PRIMARY KEY\n" +
                "   - TenMH     NVARCHAR(100) NOT NULL\n" +
                "   - SoTinChi  INT CHECK (SoTinChi > 0)\n\n" +
                "5. BẢNG KETQUA:\n" +
                "   - MaSV      VARCHAR(20) FOREIGN KEY REFERENCES SINHVIEN(MaSV) [ON DELETE CASCADE]\n" +
                "   - MaMH      VARCHAR(20) FOREIGN KEY REFERENCES MONHOC(MaMH)\n" +
                "   - Diem      DECIMAL(4,2) CHECK (Diem >= 0.0 AND Diem <= 10.0)\n" +
                "   - PRIMARY KEY (MaSV, MaMH)\n\n" +
                "6. BẢNG TAIKHOAN:\n" +
                "   - TenDangNhap VARCHAR(50) PRIMARY KEY\n" +
                "   - MatKhau     VARCHAR(100) NOT NULL\n" +
                "   - HoTen       NVARCHAR(100) NOT NULL\n" +
                "   - VaiTro      VARCHAR(20) DEFAULT 'User'\n" +
                "   - TrangThai   BIT DEFAULT 1\n\n" +
                "7. CÁC KHUNG NHÌN NÂNG CAO (VIEWS):\n" +
                "   - vw_DanhSachSinhVienDayDu (SINHVIEN JOIN LOP JOIN KHOA)\n" +
                "   - vw_BangDiemChiTiet (SINHVIEN JOIN LOP JOIN MONHOC JOIN KETQUA)\n";
        }

        public void ExecuteAndCapture(int queryIndex, string savePath)
        {
            _lstQueries.SelectedIndex = queryIndex;
            SelectQuery(queryIndex);
            RunCurrentQuery();
            Application.DoEvents();

            using var bmp = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bmp, new Rectangle(0, 0, this.Width, this.Height));
            bmp.Save(savePath, System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}
