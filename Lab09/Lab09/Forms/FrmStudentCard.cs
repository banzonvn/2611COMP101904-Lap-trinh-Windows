using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Media;
using System.Windows.Forms;
using StudentCardPrinter.Models;
using StudentCardPrinter.Services;

namespace StudentCardPrinter.Forms
{
    public partial class FrmStudentCard : Form
    {
        // 1. Nhóm Điều khiển Nhập liệu thông tin sinh viên
        private readonly TextBox txtStudentId = new TextBox();
        private readonly TextBox txtFullName = new TextBox();
        private readonly ComboBox cboClass = new ComboBox();
        private readonly ComboBox cboFaculty = new ComboBox();
        private readonly TextBox txtCourse = new TextBox();
        private readonly DateTimePicker dtpBirthDate = new DateTimePicker();
        private readonly ComboBox cboGender = new ComboBox();
        private readonly PictureBox picAvatar = new PictureBox();
        private readonly Button btnChoosePhoto = new Button();
        private readonly Button btnClear = new Button();
        private readonly Button btnSampleData = new Button();

        // 2. Nhóm Điều khiển Tùy biến thẻ (Mở rộng & Tạo nét độc đáo)
        private readonly ComboBox cboTheme = new ComboBox();
        private readonly Button btnCustomBg = new Button();
        private readonly CheckBox chkShowQr = new CheckBox();
        private readonly CheckBox chkShowChip = new CheckBox();
        private readonly Button btnSaveStudent = new Button();

        // 3. Nhóm Điều khiển Xem trước & In ấn
        private readonly Panel pnlCardPreview = new DoubleBufferedPanel();
        private readonly Button btnPreviewPrint = new Button();
        private readonly Button btnPrint = new Button();
        private readonly Button btnSavePng = new Button();

        // 4. Nhóm Quản lý Danh sách thẻ đã lưu (Mở rộng)
        private readonly DataGridView dgvStudents = new DataGridView();
        private readonly BindingList<StudentCardInfo> studentList = new BindingList<StudentCardInfo>();

        // 5. Thành phần In ấn & Hỗ trợ Multimedia, Timer
        private readonly PrintDocument printDocument1 = new PrintDocument();
        private readonly PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog();
        private readonly PrintDialog printDialog1 = new PrintDialog();
        private readonly System.Windows.Forms.Timer timerClock = new System.Windows.Forms.Timer();
        private readonly StatusStrip statusStrip1 = new StatusStrip();
        private readonly ToolStripStatusLabel lblStatus = new ToolStripStatusLabel();
        private readonly ToolStripStatusLabel lblClock = new ToolStripStatusLabel();

        // Cấu hình Theme hiện hành
        private CardTheme currentTheme = CardTheme.PredefinedThemes[0];

        public FrmStudentCard()
        {
            InitializeComponent();
            BuildUserInterface();
            InitializeData();
            RegisterEvents();
            UpdateStatus("Sẵn sàng thiết kế thẻ sinh viên.");
        }

        #region Bố cục Giao diện người dùng (Chia 2 vùng rõ ràng, chuẩn công thái học)

        private void BuildUserInterface()
        {
            Text = "COMP1019 - Thiết kế và in thẻ sinh viên [Lê Hoàng Quân - MSSV: 51.01.104.082]";
            Width = 1260;
            Height = 790;
            MinimumSize = new Size(1150, 740);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 9f);
            BackColor = Color.FromArgb(245, 247, 250);

            // Bố cục chia 2 vùng: Cột Trái (440px) và Cột Phải (Phần còn lại)
            Panel leftPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 440,
                AutoScroll = true,
                Padding = new Padding(12),
                BackColor = Color.FromArgb(245, 247, 250)
            };

            #region CỘT TRÁI - NHẬP LIỆU & TÙY BIẾN

            // 1. GroupBox Thông tin sinh viên
            GroupBox grpStudentInfo = new GroupBox
            {
                Text = " Thông tin sinh viên ",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(12, 10),
                Size = new Size(410, 460),
                ForeColor = Color.FromArgb(20, 45, 85)
            };

            int lblX = 15, txtX = 125, fieldW = 265, rowH = 36, startY = 28;

            AddFormRow(grpStudentInfo, "Mã số SV (*):", txtStudentId, lblX, txtX, fieldW, startY);
            AddFormRow(grpStudentInfo, "Họ và tên (*):", txtFullName, lblX, txtX, fieldW, startY + rowH);
            AddFormRow(grpStudentInfo, "Lớp học:", cboClass, lblX, txtX, fieldW, startY + rowH * 2);
            AddFormRow(grpStudentInfo, "Khoa đào tạo:", cboFaculty, lblX, txtX, fieldW, startY + rowH * 3);
            AddFormRow(grpStudentInfo, "Khóa học:", txtCourse, lblX, txtX, fieldW, startY + rowH * 4);
            AddFormRow(grpStudentInfo, "Ngày sinh:", dtpBirthDate, lblX, txtX, fieldW, startY + rowH * 5);
            AddFormRow(grpStudentInfo, "Giới tính:", cboGender, lblX, txtX, fieldW, startY + rowH * 6);

            // Cấu hình DateTimePicker
            dtpBirthDate.Format = DateTimePickerFormat.Custom;
            dtpBirthDate.CustomFormat = "dd/MM/yyyy";
            dtpBirthDate.Value = new DateTime(2004, 1, 1);

            // Phần ảnh đại diện
            Label lblAvatar = new Label
            {
                Text = "Ảnh đại diện (*):",
                Location = new Point(lblX, startY + rowH * 7 + 2),
                Size = new Size(105, 22),
                Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                ForeColor = Color.Black
            };
            grpStudentInfo.Controls.Add(lblAvatar);

            picAvatar.Location = new Point(txtX, startY + rowH * 7);
            picAvatar.Size = new Size(110, 140);
            picAvatar.BorderStyle = BorderStyle.FixedSingle;
            picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            picAvatar.BackColor = Color.White;
            grpStudentInfo.Controls.Add(picAvatar);

            // Cụm nút bấm bên cạnh PictureBox
            btnChoosePhoto.Text = "Chọn ảnh...";
            btnChoosePhoto.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnChoosePhoto.Location = new Point(txtX + 120, startY + rowH * 7);
            btnChoosePhoto.Size = new Size(145, 34);
            btnChoosePhoto.BackColor = Color.FromArgb(235, 242, 252);
            btnChoosePhoto.ForeColor = Color.FromArgb(20, 50, 105);
            btnChoosePhoto.Cursor = Cursors.Hand;
            grpStudentInfo.Controls.Add(btnChoosePhoto);

            btnSampleData.Text = "Nạp dữ liệu mẫu";
            btnSampleData.Font = new Font("Segoe UI", 8.5f);
            btnSampleData.Location = new Point(txtX + 120, startY + rowH * 7 + 42);
            btnSampleData.Size = new Size(145, 32);
            btnSampleData.BackColor = Color.FromArgb(240, 243, 246);
            btnSampleData.Cursor = Cursors.Hand;
            grpStudentInfo.Controls.Add(btnSampleData);

            btnClear.Text = "Làm mới (Clear)";
            btnClear.Font = new Font("Segoe UI", 8.5f);
            btnClear.Location = new Point(txtX + 120, startY + rowH * 7 + 82);
            btnClear.Size = new Size(145, 32);
            btnClear.BackColor = Color.FromArgb(254, 242, 242);
            btnClear.ForeColor = Color.DarkRed;
            btnClear.Cursor = Cursors.Hand;
            grpStudentInfo.Controls.Add(btnClear);

            leftPanel.Controls.Add(grpStudentInfo);

            // 2. GroupBox Tùy biến thẻ (Mở rộng)
            GroupBox grpTheme = new GroupBox
            {
                Text = " Tùy biến phong cách thẻ (Themes) ",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(12, 480),
                Size = new Size(410, 160),
                ForeColor = Color.FromArgb(20, 45, 85)
            };

            Label lblTheme = new Label
            {
                Text = "Bộ màu:",
                Location = new Point(15, 26),
                Size = new Size(80, 22),
                Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                ForeColor = Color.Black
            };
            cboTheme.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTheme.Location = new Point(100, 23);
            cboTheme.Size = new Size(290, 25);
            cboTheme.Font = new Font("Segoe UI", 9f);
            grpTheme.Controls.Add(lblTheme);
            grpTheme.Controls.Add(cboTheme);

            btnCustomBg.Text = "Đổi màu nền thẻ...";
            btnCustomBg.Font = new Font("Segoe UI", 8.5f);
            btnCustomBg.Location = new Point(15, 62);
            btnCustomBg.Size = new Size(185, 30);
            btnCustomBg.Cursor = Cursors.Hand;
            grpTheme.Controls.Add(btnCustomBg);

            btnSaveStudent.Text = "Lưu thẻ vào danh sách";
            btnSaveStudent.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            btnSaveStudent.Location = new Point(205, 62);
            btnSaveStudent.Size = new Size(185, 30);
            btnSaveStudent.BackColor = Color.FromArgb(235, 245, 240);
            btnSaveStudent.ForeColor = Color.FromArgb(20, 90, 50);
            btnSaveStudent.Cursor = Cursors.Hand;
            grpTheme.Controls.Add(btnSaveStudent);

            chkShowQr.Text = "Mã QR Code sinh viên";
            chkShowQr.Checked = true;
            chkShowQr.Font = new Font("Segoe UI", 9f);
            chkShowQr.ForeColor = Color.Black;
            chkShowQr.Location = new Point(15, 108);
            chkShowQr.Size = new Size(180, 24);
            grpTheme.Controls.Add(chkShowQr);

            chkShowChip.Text = "Vi mạch Chip bảo mật";
            chkShowChip.Checked = true;
            chkShowChip.Font = new Font("Segoe UI", 9f);
            chkShowChip.ForeColor = Color.Black;
            chkShowChip.Location = new Point(205, 108);
            chkShowChip.Size = new Size(185, 24);
            grpTheme.Controls.Add(chkShowChip);

            leftPanel.Controls.Add(grpTheme);

            #endregion

            #region CỘT PHẢI - PREVIEW THẺ GDI+, THAO TÁC IN & DANH SÁCH

            Panel rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                BackColor = Color.FromArgb(245, 247, 250)
            };

            // 1. GroupBox Xem trước thẻ
            GroupBox grpPreview = new GroupBox
            {
                Text = " Bản xem trước thẻ sinh viên (GDI+ Preview) ",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 440,
                ForeColor = Color.FromArgb(20, 45, 85)
            };

            pnlCardPreview.Dock = DockStyle.Fill;
            pnlCardPreview.BackColor = Color.FromArgb(240, 242, 246);
            pnlCardPreview.Padding = new Padding(10);
            grpPreview.Controls.Add(pnlCardPreview);

            // 2. Thanh nút bấm thao tác In ấn & Xuất ảnh
            Panel pnlActions = new Panel
            {
                Dock = DockStyle.Top,
                Height = 54,
                Padding = new Padding(0, 8, 0, 8)
            };

            btnPreviewPrint.Text = "Xem trước bản in (F4)";
            btnPreviewPrint.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnPreviewPrint.Size = new Size(210, 38);
            btnPreviewPrint.Location = new Point(0, 8);
            btnPreviewPrint.BackColor = Color.FromArgb(20, 50, 105);
            btnPreviewPrint.ForeColor = Color.White;
            btnPreviewPrint.Cursor = Cursors.Hand;

            btnPrint.Text = "In thẻ sinh viên";
            btnPrint.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnPrint.Size = new Size(160, 38);
            btnPrint.Location = new Point(220, 8);
            btnPrint.BackColor = Color.FromArgb(24, 110, 60);
            btnPrint.ForeColor = Color.White;
            btnPrint.Cursor = Cursors.Hand;

            btnSavePng.Text = "Xuất file ảnh PNG";
            btnSavePng.Font = new Font("Segoe UI", 9f);
            btnSavePng.Size = new Size(160, 38);
            btnSavePng.Location = new Point(390, 8);
            btnSavePng.BackColor = Color.FromArgb(235, 242, 252);
            btnSavePng.ForeColor = Color.FromArgb(20, 50, 105);
            btnSavePng.Cursor = Cursors.Hand;

            pnlActions.Controls.Add(btnPreviewPrint);
            pnlActions.Controls.Add(btnPrint);
            pnlActions.Controls.Add(btnSavePng);

            // 3. GroupBox Danh sách sinh viên đã lưu
            GroupBox grpList = new GroupBox
            {
                Text = " Danh sách thẻ sinh viên đã lưu trong phiên làm việc ",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Dock = DockStyle.Fill,
                ForeColor = Color.FromArgb(20, 45, 85)
            };

            dgvStudents.Dock = DockStyle.Fill;
            dgvStudents.BackgroundColor = Color.White;
            dgvStudents.BorderStyle = BorderStyle.None;
            dgvStudents.ReadOnly = true;
            dgvStudents.AllowUserToAddRows = false;
            dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStudents.MultiSelect = false;
            dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStudents.RowHeadersVisible = false;
            dgvStudents.Font = new Font("Segoe UI", 9f);
            grpList.Controls.Add(dgvStudents);

            rightPanel.Controls.Add(grpList);
            rightPanel.Controls.Add(pnlActions);
            rightPanel.Controls.Add(grpPreview);

            #endregion

            // 4. StatusStrip phía dưới
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblStatus, lblClock });
            lblStatus.Spring = true;
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            lblClock.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            lblClock.BorderSides = ToolStripStatusLabelBorderSides.Left;

            // Thêm các Panel vào Form chính
            Controls.Add(rightPanel);
            Controls.Add(leftPanel);
            Controls.Add(statusStrip1);
        }

        private static void AddFormRow(Control parent, string labelText, Control inputControl, int lblX, int txtX, int width, int y)
        {
            Label lbl = new Label
            {
                Text = labelText,
                Location = new Point(lblX, y + 3),
                Size = new Size(105, 22),
                Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                ForeColor = Color.Black
            };
            inputControl.Location = new Point(txtX, y);
            inputControl.Size = new Size(width, 26);
            inputControl.Font = new Font("Segoe UI", 9f);

            parent.Controls.Add(lbl);
            parent.Controls.Add(inputControl);
        }

        #endregion

        #region Khởi tạo Dữ liệu & Đăng ký Sự kiện

        private void InitializeData()
        {
            // Danh sách lớp học mẫu
            cboClass.Items.AddRange(new object[]
            {
                "K50.CNTT.A", "K50.CNTT.B", "K49.CNTT.A", "K49.CNTT.B",
                "K50.TOAN.A", "K50.SU.A", "K51.CNTT.A"
            });
            cboClass.SelectedIndex = 0;

            // Danh sách khoa đào tạo mẫu
            cboFaculty.Items.AddRange(new object[]
            {
                "Công nghệ thông tin",
                "Toán - Tin học",
                "Vật lý",
                "Hóa học",
                "Ngữ văn",
                "Tiếng Anh"
            });
            cboFaculty.SelectedIndex = 0;

            // Danh sách giới tính
            cboGender.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });
            cboGender.SelectedIndex = 0;

            txtCourse.Text = "K50 (2024 - 2028)";

            // Nạp danh sách Themes
            foreach (var t in CardTheme.PredefinedThemes)
            {
                cboTheme.Items.Add(t.Name);
            }
            cboTheme.SelectedIndex = 0;

            // Cấu hình DataGridView
            dgvStudents.AutoGenerateColumns = false;
            dgvStudents.Columns.Clear();
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "StudentId", HeaderText = "Mã số SV", Width = 110, MinimumWidth = 90 });
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FullName", HeaderText = "Họ và tên", Width = 160, MinimumWidth = 130 });
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ClassName", HeaderText = "Lớp", Width = 100, MinimumWidth = 90 });
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Faculty", HeaderText = "Khoa", Width = 150, MinimumWidth = 120 });
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Course", HeaderText = "Khóa học", Width = 130, MinimumWidth = 100 });
            dgvStudents.DataSource = studentList;

            // Đồng hồ thời gian thực
            timerClock.Interval = 1000;
            timerClock.Tick += (s, e) => lblClock.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            timerClock.Start();
        }

        private void RegisterEvents()
        {
            // Các nút bấm
            btnChoosePhoto.Click += BtnChoosePhoto_Click;
            btnSampleData.Click += BtnSampleData_Click;
            btnClear.Click += BtnClear_Click;
            btnPreviewPrint.Click += BtnPreviewPrint_Click;
            btnPrint.Click += BtnPrint_Click;
            btnSavePng.Click += BtnSavePng_Click;
            btnCustomBg.Click += BtnCustomBg_Click;
            btnSaveStudent.Click += BtnSaveStudent_Click;

            // Tùy biến Theme & Mã QR
            cboTheme.SelectedIndexChanged += CboTheme_SelectedIndexChanged;
            chkShowQr.CheckedChanged += (s, e) => { currentTheme.ShowQrCode = chkShowQr.Checked; pnlCardPreview.Invalidate(); };
            chkShowChip.CheckedChanged += (s, e) => { currentTheme.ShowSmartChip = chkShowChip.Checked; pnlCardPreview.Invalidate(); };

            // Preview thẻ
            pnlCardPreview.Paint += PnlCardPreview_Paint;

            // In ấn (CHỈ GẮN 1 LẦN DUY NHẤT để tránh bị lặp nội dung khi bấm in nhiều lần)
            printDocument1.PrintPage += PrintDocument1_PrintPage;

            // Khi người dùng thay đổi dữ liệu nhập -> Tự động Invalidate để vẽ lại preview thời gian thực
            txtStudentId.TextChanged += InputChanged;
            txtFullName.TextChanged += InputChanged;
            txtCourse.TextChanged += InputChanged;
            cboClass.SelectedIndexChanged += InputChanged;
            cboFaculty.SelectedIndexChanged += InputChanged;
            cboGender.SelectedIndexChanged += InputChanged;
            dtpBirthDate.ValueChanged += InputChanged;

            // Khi chọn một sinh viên từ danh sách
            dgvStudents.SelectionChanged += DgvStudents_SelectionChanged;
        }

        private void InputChanged(object? sender, EventArgs e)
        {
            pnlCardPreview.Invalidate();
        }

        #endregion

        #region Xử lý Chọn ảnh an toàn (Không khóa file)

        private void BtnChoosePhoto_Click(object? sender, EventArgs e)
        {
            using OpenFileDialog dlg = new OpenFileDialog
            {
                Title = "Chọn ảnh đại diện sinh viên",
                Filter = "File ảnh (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|Tất cả các file (*.*)|*.*"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadImageSafe(dlg.FileName);
            }
        }

        /// <summary>
        /// Nạp ảnh an toàn: Clone ảnh vào Bitmap mới để tránh khóa file gốc (File Lock),
        /// đồng thời chủ động Dispose ảnh cũ để giải phóng tài nguyên đồ họa.
        /// </summary>
        public bool LoadImageSafe(string filePath)
        {
            try
            {
                using (Image temp = Image.FromFile(filePath))
                {
                    Bitmap copy = new Bitmap(temp);
                    picAvatar.Image?.Dispose();
                    picAvatar.Image = copy;
                }

                pnlCardPreview.Invalidate();
                UpdateStatus($"Đã nạp ảnh đại diện: {Path.GetFileName(filePath)}");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"File được chọn không phải là ảnh hợp lệ hoặc bị lỗi:\n{ex.Message}",
                    "Lỗi định dạng ảnh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        #endregion

        #region Xử lý Preview thẻ & In ấn bằng GDI+

        public StudentCardInfo GetStudentInfo()
        {
            return new StudentCardInfo
            {
                StudentId = txtStudentId.Text.Trim(),
                FullName = txtFullName.Text.Trim(),
                ClassName = cboClass.Text,
                Faculty = cboFaculty.Text,
                Course = txtCourse.Text.Trim(),
                BirthDate = dtpBirthDate.Value,
                Gender = cboGender.Text,
                Photo = picAvatar.Image
            };
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của dữ liệu trước khi preview in hoặc in thẻ.
        /// </summary>
        public bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtStudentId.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã số sinh viên (bắt buộc).", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStudentId.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Vui lòng nhập Họ và tên sinh viên (bắt buộc).", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return false;
            }

            if (picAvatar.Image == null)
            {
                MessageBox.Show("Vui lòng chọn ảnh đại diện cho sinh viên trước khi in thẻ.", "Thiếu ảnh đại diện", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnChoosePhoto.Focus();
                return false;
            }

            return true;
        }

        private void PnlCardPreview_Paint(object? sender, PaintEventArgs e)
        {
            // Tính toán kích thước hiển thị cân đối giữa Panel
            int cardW = Math.Min(600, pnlCardPreview.Width - 40);
            int cardH = (int)(cardW / 1.6f); // Tỉ lệ xấp xỉ CR80 (1.586)
            int cardX = (pnlCardPreview.Width - cardW) / 2;
            int cardY = (pnlCardPreview.Height - cardH) / 2;

            Rectangle cardRect = new Rectangle(cardX, cardY, cardW, cardH);
            StudentCardRenderer.Render(e.Graphics, cardRect, GetStudentInfo(), currentTheme);
        }

        private void BtnPreviewPrint_Click(object? sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.Width = 980;
            printPreviewDialog1.Height = 720;
            printPreviewDialog1.StartPosition = FormStartPosition.CenterScreen;

            SystemSounds.Asterisk.Play();
            UpdateStatus("Đang mở cửa sổ xem trước bản in (Print Preview)...");
            printPreviewDialog1.ShowDialog();
        }

        private void BtnPrint_Click(object? sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            printDialog1.Document = printDocument1;
            printDialog1.UseEXDialog = true;

            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    printDocument1.Print();
                    SystemSounds.Asterisk.Play();
                    MessageBox.Show("Lệnh in thẻ đã được gửi thành công đến máy in!", "In thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateStatus("Đã in thẻ sinh viên thành công.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi trong quá trình in:\n{ex.Message}", "Lỗi in ấn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void PrintDocument1_PrintPage(object? sender, PrintPageEventArgs e)
        {
            if (e.Graphics == null) return;

            // Kích thước chuẩn khi in ra trang giấy (560 x 350 pixel tại 96 DPI)
            Rectangle cardRect = new Rectangle(e.MarginBounds.Left, e.MarginBounds.Top, 560, 350);
            StudentCardRenderer.Render(e.Graphics, cardRect, GetStudentInfo(), currentTheme);

            e.HasMorePages = false; // Chỉ in 1 thẻ duy nhất
        }

        #endregion

        #region Chức năng Tùy biến & Mở rộng (Export PNG, Themes, List)

        private void CboTheme_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cboTheme.SelectedIndex >= 0 && cboTheme.SelectedIndex < CardTheme.PredefinedThemes.Length)
            {
                currentTheme = CardTheme.PredefinedThemes[cboTheme.SelectedIndex];
                chkShowQr.Checked = currentTheme.ShowQrCode;
                chkShowChip.Checked = currentTheme.ShowSmartChip;
                pnlCardPreview.Invalidate();
                UpdateStatus($"Đã áp dụng theme: {currentTheme.Name}");
            }
        }

        private void BtnCustomBg_Click(object? sender, EventArgs e)
        {
            using ColorDialog dlg = new ColorDialog
            {
                Color = currentTheme.CardBgColor,
                FullOpen = true
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                currentTheme.CardBgColor = dlg.Color;
                pnlCardPreview.Invalidate();
                UpdateStatus($"Đã đổi màu nền thẻ thành: RGB({dlg.Color.R}, {dlg.Color.G}, {dlg.Color.B})");
            }
        }

        private void BtnSavePng_Click(object? sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            using SaveFileDialog sfd = new SaveFileDialog
            {
                Title = "Lưu thẻ sinh viên ra file ảnh PNG",
                Filter = "PNG Image (*.png)|*.png|JPEG Image (*.jpg)|*.jpg",
                FileName = $"StudentCard_{txtStudentId.Text.Trim()}_{DateTime.Now:yyyyMMdd}.png"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using Bitmap bmp = new Bitmap(560, 350);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        StudentCardRenderer.Render(g, new Rectangle(0, 0, 560, 350), GetStudentInfo(), currentTheme);
                    }

                    bmp.Save(sfd.FileName, ImageFormat.Png);
                    SystemSounds.Asterisk.Play();
                    MessageBox.Show($"Đã lưu ảnh thẻ thành công tại:\n{sfd.FileName}", "Lưu ảnh hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateStatus($"Đã xuất file ảnh thẻ: {Path.GetFileName(sfd.FileName)}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lưu file ảnh:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnSaveStudent_Click(object? sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            StudentCardInfo info = GetStudentInfo();

            // Kiểm tra trùng mã sinh viên trong danh sách
            for (int i = 0; i < studentList.Count; i++)
            {
                if (studentList[i].StudentId.Equals(info.StudentId, StringComparison.OrdinalIgnoreCase))
                {
                    studentList[i] = info.Clone();
                    dgvStudents.Refresh();
                    UpdateStatus($"Đã cập nhật thông tin sinh viên: {info.FullName} ({info.StudentId})");
                    return;
                }
            }

            studentList.Add(info.Clone());
            SystemSounds.Asterisk.Play();
            UpdateStatus($"Đã lưu sinh viên vào danh sách tạm. Tổng cộng: {studentList.Count} thẻ.");
        }

        private void DgvStudents_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow != null && dgvStudents.CurrentRow.DataBoundItem is StudentCardInfo selected)
            {
                SetStudentData(selected);
                UpdateStatus($"Đã tải thông tin sinh viên từ danh sách: {selected.FullName}");
            }
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            txtStudentId.Clear();
            txtFullName.Clear();
            if (cboClass.Items.Count > 0) cboClass.SelectedIndex = 0;
            if (cboFaculty.Items.Count > 0) cboFaculty.SelectedIndex = 0;
            if (cboGender.Items.Count > 0) cboGender.SelectedIndex = 0;
            txtCourse.Text = "K50 (2024 - 2028)";
            dtpBirthDate.Value = new DateTime(2004, 1, 1);

            picAvatar.Image?.Dispose();
            picAvatar.Image = null;

            pnlCardPreview.Invalidate();
            txtStudentId.Focus();
            UpdateStatus("Đã làm mới Form về trạng thái ban đầu.");
        }

        public void BtnSampleData_Click(object? sender, EventArgs e)
        {
            LoadSampleStudent(0);
        }

        public void LoadSampleStudent(int sampleIndex = 0)
        {
            StudentCardInfo sample = new StudentCardInfo
            {
                StudentId = "51.01.104.082",
                FullName = "Lê Hoàng Quân",
                ClassName = "K50.CNTT.A",
                Faculty = "Công nghệ thông tin",
                Course = "K50 (2024 - 2028)",
                BirthDate = new DateTime(2004, 8, 15),
                Gender = "Nam"
            };

            if (sampleIndex == 1)
            {
                sample.StudentId = "51.01.104.015";
                sample.FullName = "Nguyễn Thị Mai";
                sample.ClassName = "K50.CNTT.B";
                sample.Faculty = "Toán - Tin học";
                sample.Course = "K50 (2024 - 2028)";
                sample.Gender = "Nữ";
                sample.BirthDate = new DateTime(2004, 10, 20);
            }

            // Tạo ảnh chân dung sinh viên mẫu bằng GDI+
            Bitmap sampleAvatar = new Bitmap(200, 260);
            using (Graphics g = Graphics.FromImage(sampleAvatar))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.FromArgb(235, 240, 248));

                using Brush suitBrush = new SolidBrush(Color.FromArgb(24, 48, 89));
                g.FillPie(suitBrush, 20, 160, 160, 180, 180, 180);

                using Brush skinBrush = new SolidBrush(Color.FromArgb(255, 218, 185));
                g.FillEllipse(skinBrush, 60, 50, 80, 100);

                using Brush hairBrush = new SolidBrush(Color.FromArgb(30, 30, 30));
                g.FillPie(hairBrush, 55, 30, 90, 80, 180, 180);

                using Font font = new Font("Arial", 11f, FontStyle.Bold);
                using StringFormat sf = new StringFormat { Alignment = StringAlignment.Center };
                g.DrawString(sample.FullName, font, Brushes.Navy, 100, 225, sf);
            }

            sample.Photo = sampleAvatar;
            SetStudentData(sample);
            UpdateStatus("Đã nạp dữ liệu sinh viên mẫu thành công.");
        }

        public void SetStudentData(StudentCardInfo info)
        {
            txtStudentId.Text = info.StudentId;
            txtFullName.Text = info.FullName;
            cboClass.Text = info.ClassName;
            cboFaculty.Text = info.Faculty;
            txtCourse.Text = info.Course;
            dtpBirthDate.Value = info.BirthDate;
            cboGender.Text = info.Gender;

            if (info.Photo != null && picAvatar.Image != info.Photo)
            {
                try
                {
                    Bitmap newBmp = new Bitmap(info.Photo);
                    picAvatar.Image?.Dispose();
                    picAvatar.Image = newBmp;
                }
                catch
                {
                    picAvatar.Image = null;
                }
            }
            else if (info.Photo == null)
            {
                picAvatar.Image?.Dispose();
                picAvatar.Image = null;
            }

            pnlCardPreview.Invalidate();
        }

        public void SetTheme(int index)
        {
            if (index >= 0 && index < cboTheme.Items.Count)
            {
                cboTheme.SelectedIndex = index;
            }
        }

        public void SetCustomBgColor(Color color)
        {
            currentTheme.CardBgColor = color;
            pnlCardPreview.Invalidate();
        }

        public void SetShowQr(bool show)
        {
            chkShowQr.Checked = show;
        }

        public void SetShowChip(bool show)
        {
            chkShowChip.Checked = show;
        }

        public void TriggerSaveStudent()
        {
            BtnSaveStudent_Click(this, EventArgs.Empty);
        }

        public void TriggerClear()
        {
            BtnClear_Click(this, EventArgs.Empty);
        }

        public void UpdateStatus(string message)
        {
            lblStatus.Text = message;
        }

        #endregion
    }

    /// <summary>
    /// Custom Panel bật DoubleBuffered để tránh hiện tượng nhấp nháy khi vẽ GDI+.
    /// </summary>
    public class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            UpdateStyles();
        }
    }
}
