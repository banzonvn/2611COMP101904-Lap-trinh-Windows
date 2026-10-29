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

        // 2. Nhóm Điều khiển Tùy biến thẻ (Mở rộng & Tạo sự khác biệt)
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

        // 5. Thành phần In ấn & Hỗ trợ
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

        #region Bố cục Giao diện người dùng

        private void BuildUserInterface()
        {
            Text = "COMP1019 - Thiết kế và in thẻ sinh viên [Lê Hoàng Quân - MSSV: 51.01.104.082]";
            Width = 1220;
            Height = 780;
            MinimumSize = new Size(1100, 720);
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

            // GroupBox Thông tin sinh viên
            GroupBox grpStudentInfo = new GroupBox
            {
                Text = " Thông tin sinh viên ",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(12, 10),
                Size = new Size(410, 470),
                ForeColor = Color.FromArgb(20, 45, 85)
            };

            int lblX = 15, txtX = 135, fieldW = 250, rowH = 34, startY = 30;

            AddFormRow(grpStudentInfo, "Mã số SV (*):", txtStudentId, lblX, txtX, fieldW, startY);
            AddFormRow(grpStudentInfo, "Họ và tên (*):", txtFullName, lblX, txtX, fieldW, startY + rowH);
            AddFormRow(grpStudentInfo, "Lớp học:", cboClass, lblX, txtX, fieldW, startY + rowH * 2);
            AddFormRow(grpStudentInfo, "Khoa đào tạo:", cboFaculty, lblX, txtX, fieldW, startY + rowH * 3);
            AddFormRow(grpStudentInfo, "Khóa học:", txtCourse, lblX, txtX, fieldW, startY + rowH * 4);

            dtpBirthDate.Format = DateTimePickerFormat.Custom;
            dtpBirthDate.CustomFormat = "dd/MM/yyyy";
            AddFormRow(grpStudentInfo, "Ngày sinh:", dtpBirthDate, lblX, txtX, fieldW, startY + rowH * 5);
            AddFormRow(grpStudentInfo, "Giới tính:", cboGender, lblX, txtX, fieldW, startY + rowH * 6);

            // Ảnh đại diện
            Label lblAvatar = new Label
            {
                Text = "Ảnh thẻ (*):",
                Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(lblX, startY + rowH * 7 + 5),
                AutoSize = true
            };
            grpStudentInfo.Controls.Add(lblAvatar);

            picAvatar.Location = new Point(txtX, startY + rowH * 7);
            picAvatar.Size = new Size(100, 125);
            picAvatar.BorderStyle = BorderStyle.FixedSingle;
            picAvatar.BackColor = Color.White;
            picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            grpStudentInfo.Controls.Add(picAvatar);

            // Nút Thao tác ảnh và dữ liệu
            btnChoosePhoto.Text = "Chọn ảnh...";
            btnChoosePhoto.Font = new Font("Segoe UI", 9f);
            btnChoosePhoto.Size = new Size(135, 32);
            btnChoosePhoto.Location = new Point(txtX + 115, startY + rowH * 7 + 10);
            btnChoosePhoto.Cursor = Cursors.Hand;
            grpStudentInfo.Controls.Add(btnChoosePhoto);

            btnSampleData.Text = "Nạp mẫu";
            btnSampleData.Font = new Font("Segoe UI", 8.5f);
            btnSampleData.Size = new Size(65, 30);
            btnSampleData.Location = new Point(txtX + 115, startY + rowH * 7 + 50);
            btnSampleData.Cursor = Cursors.Hand;
            grpStudentInfo.Controls.Add(btnSampleData);

            btnClear.Text = "Làm mới";
            btnClear.Font = new Font("Segoe UI", 8.5f);
            btnClear.Size = new Size(65, 30);
            btnClear.Location = new Point(txtX + 185, startY + rowH * 7 + 50);
            btnClear.Cursor = Cursors.Hand;
            grpStudentInfo.Controls.Add(btnClear);

            // GroupBox Tùy biến thẻ (Mở rộng & Khác biệt)
            GroupBox grpCustom = new GroupBox
            {
                Text = " Tùy biến phong cách thẻ (Card Styling) ",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(12, 490),
                Size = new Size(410, 185),
                ForeColor = Color.FromArgb(20, 45, 85)
            };

            AddFormRow(grpCustom, "Theme thẻ:", cboTheme, 15, 120, 170, 28);

            btnCustomBg.Text = "Đổi nền";
            btnCustomBg.Font = new Font("Segoe UI", 8.5f);
            btnCustomBg.Size = new Size(85, 28);
            btnCustomBg.Location = new Point(300, 27);
            btnCustomBg.Cursor = Cursors.Hand;
            grpCustom.Controls.Add(btnCustomBg);

            chkShowQr.Text = "Tạo mã QR giả lập";
            chkShowQr.Font = new Font("Segoe UI", 9f);
            chkShowQr.ForeColor = Color.Black;
            chkShowQr.Checked = true;
            chkShowQr.Location = new Point(20, 70);
            chkShowQr.AutoSize = true;
            grpCustom.Controls.Add(chkShowQr);

            chkShowChip.Text = "Hiển thị chip thông minh";
            chkShowChip.Font = new Font("Segoe UI", 9f);
            chkShowChip.ForeColor = Color.Black;
            chkShowChip.Checked = true;
            chkShowChip.Location = new Point(190, 70);
            chkShowChip.AutoSize = true;
            grpCustom.Controls.Add(chkShowChip);

            btnSaveStudent.Text = "Lưu thẻ vào danh sách tạm (+)";
            btnSaveStudent.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnSaveStudent.BackColor = Color.FromArgb(232, 240, 254);
            btnSaveStudent.Size = new Size(375, 34);
            btnSaveStudent.Location = new Point(15, 115);
            btnSaveStudent.Cursor = Cursors.Hand;
            grpCustom.Controls.Add(btnSaveStudent);

            leftPanel.Controls.Add(grpStudentInfo);
            leftPanel.Controls.Add(grpCustom);

            #endregion

            #region CỘT PHẢI - XEM TRƯỚC THẺ & IN ẤN

            Panel rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                BackColor = Color.FromArgb(245, 247, 250)
            };

            // GroupBox Preview
            GroupBox grpPreview = new GroupBox
            {
                Text = " Bản xem trước thẻ sinh viên (Preview GDI+) ",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(12, 10),
                Size = new Size(720, 420),
                ForeColor = Color.FromArgb(20, 45, 85)
            };

            // Khung vẽ thẻ sinh viên (tỷ lệ chuẩn thẻ ngang 540 x 340)
            pnlCardPreview.Size = new Size(540, 340);
            pnlCardPreview.Location = new Point((grpPreview.Width - 540) / 2, 45);
            pnlCardPreview.BackColor = Color.Transparent;
            grpPreview.Controls.Add(pnlCardPreview);

            // Thanh nút bấm In ấn & Xuất file
            FlowLayoutPanel flpActions = new FlowLayoutPanel
            {
                Location = new Point(12, 440),
                Size = new Size(720, 45),
                FlowDirection = FlowDirection.LeftToRight
            };

            btnPreviewPrint.Text = "Xem trước in (Print Preview)";
            btnPreviewPrint.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnPreviewPrint.BackColor = Color.FromArgb(24, 76, 120);
            btnPreviewPrint.ForeColor = Color.White;
            btnPreviewPrint.Size = new Size(220, 38);
            btnPreviewPrint.Cursor = Cursors.Hand;

            btnPrint.Text = "In thẻ sinh viên (Print)";
            btnPrint.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnPrint.BackColor = Color.FromArgb(16, 110, 60);
            btnPrint.ForeColor = Color.White;
            btnPrint.Size = new Size(200, 38);
            btnPrint.Cursor = Cursors.Hand;

            btnSavePng.Text = "Xuất file ảnh (PNG)";
            btnSavePng.Font = new Font("Segoe UI", 9.5f);
            btnSavePng.BackColor = Color.FromArgb(240, 243, 248);
            btnSavePng.Size = new Size(160, 38);
            btnSavePng.Cursor = Cursors.Hand;

            flpActions.Controls.Add(btnPreviewPrint);
            flpActions.Controls.Add(btnPrint);
            flpActions.Controls.Add(btnSavePng);

            // GroupBox Danh sách sinh viên đã lưu tạm thời
            GroupBox grpList = new GroupBox
            {
                Text = " Danh sách sinh viên (Lưu tạm bộ nhớ - Click dòng để nạp thẻ) ",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Location = new Point(12, 495),
                Size = new Size(720, 180),
                ForeColor = Color.FromArgb(20, 45, 85)
            };

            dgvStudents.Dock = DockStyle.Fill;
            dgvStudents.Font = new Font("Segoe UI", 9f);
            dgvStudents.ReadOnly = true;
            dgvStudents.AllowUserToAddRows = false;
            dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStudents.MultiSelect = false;
            dgvStudents.BackgroundColor = Color.White;
            dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStudents.RowHeadersVisible = false;
            grpList.Controls.Add(dgvStudents);

            rightPanel.Controls.Add(grpPreview);
            rightPanel.Controls.Add(flpActions);
            rightPanel.Controls.Add(grpList);

            #endregion

            // StatusStrip phía dưới
            statusStrip1.Items.Add(lblStatus);
            lblStatus.Spring = true;
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;

            statusStrip1.Items.Add(lblClock);
            lblClock.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            Controls.Add(rightPanel);
            Controls.Add(leftPanel);
            Controls.Add(statusStrip1);
        }

        private void AddFormRow(Control parent, string labelText, Control inputControl, int lblX, int txtX, int width, int y)
        {
            Label lbl = new Label
            {
                Text = labelText,
                Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(lblX, y + 3),
                AutoSize = true
            };
            inputControl.Font = new Font("Segoe UI", 9f);
            inputControl.Location = new Point(txtX, y);
            inputControl.Width = width;

            parent.Controls.Add(lbl);
            parent.Controls.Add(inputControl);
        }

        #endregion

        #region Khởi tạo Dữ liệu & Đăng ký Sự kiện

        private void InitializeData()
        {
            // Danh sách Lớp mẫu
            cboClass.DropDownStyle = ComboBoxStyle.DropDownList;
            cboClass.Items.AddRange(new object[] { "K49.CNTT.A", "K50.CNTT.A", "K51.CNTT.A", "K52.CNTT.A", "K50.TOAN.A", "K51.SU.A" });
            cboClass.SelectedIndex = 1;

            // Danh sách Khoa mẫu
            cboFaculty.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFaculty.Items.AddRange(new object[] {
                "Công nghệ thông tin", "Toán - Tin học", "Vật lý",
                "Hóa học", "Sinh học", "Ngữ văn", "Tiếng Anh"
            });
            cboFaculty.SelectedIndex = 0;

            // Danh sách Giới tính
            cboGender.DropDownStyle = ComboBoxStyle.DropDownList;
            cboGender.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });
            cboGender.SelectedIndex = 0;

            // Khóa học mặc định
            txtCourse.Text = "K50 (2024 - 2028)";

            // Danh sách Themes
            cboTheme.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (var t in CardTheme.PredefinedThemes)
            {
                cboTheme.Items.Add(t.Name);
            }
            cboTheme.SelectedIndex = 0;

            // Cấu hình DataGridView sinh viên
            dgvStudents.AutoGenerateColumns = false;
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "StudentId", HeaderText = "MSSV", Width = 95 });
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FullName", HeaderText = "Họ và tên", Width = 150 });
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ClassName", HeaderText = "Lớp", Width = 95 });
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Faculty", HeaderText = "Khoa", Width = 150 });
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Course", HeaderText = "Khóa", Width = 80 });
            dgvStudents.DataSource = studentList;

            // Đồng hồ thời gian thực
            timerClock.Interval = 1000;
            timerClock.Tick += (s, e) => lblClock.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            timerClock.Start();
        }

        private void RegisterEvents()
        {
            // Tương tác nút
            btnChoosePhoto.Click += BtnChoosePhoto_Click;
            btnClear.Click += BtnClear_Click;
            btnSampleData.Click += BtnSampleData_Click;
            btnPreviewPrint.Click += BtnPreviewPrint_Click;
            btnPrint.Click += BtnPrint_Click;
            btnSavePng.Click += BtnSavePng_Click;
            btnCustomBg.Click += BtnCustomBg_Click;
            btnSaveStudent.Click += BtnSaveStudent_Click;

            // Sự kiện thay đổi dữ liệu nhập liệu -> Tự động kích hoạt Invalidate() vẽ lại thẻ
            txtStudentId.TextChanged += InputChanged;
            txtFullName.TextChanged += InputChanged;
            cboClass.SelectedIndexChanged += InputChanged;
            cboFaculty.SelectedIndexChanged += InputChanged;
            txtCourse.TextChanged += InputChanged;
            dtpBirthDate.ValueChanged += InputChanged;
            cboGender.SelectedIndexChanged += InputChanged;

            // Tùy biến giao diện thẻ
            cboTheme.SelectedIndexChanged += CboTheme_SelectedIndexChanged;
            chkShowQr.CheckedChanged += (s, e) => { currentTheme.ShowQrCode = chkShowQr.Checked; pnlCardPreview.Invalidate(); };
            chkShowChip.CheckedChanged += (s, e) => { currentTheme.ShowSmartChip = chkShowChip.Checked; pnlCardPreview.Invalidate(); };

            // Vẽ bản xem trước GDI+ trên Panel
            pnlCardPreview.Paint += PnlCardPreview_Paint;

            // Đăng ký sự kiện In ấn DUY NHẤT một lần (tránh lặp handler theo slide lý thuyết)
            printDocument1.PrintPage += PrintDocument1_PrintPage;

            // Click vào DataGridView để nạp thông tin sinh viên
            dgvStudents.SelectionChanged += DgvStudents_SelectionChanged;
        }

        #endregion

        #region Xử lý Dữ liệu thẻ & Giao diện

        private void InputChanged(object? sender, EventArgs e)
        {
            pnlCardPreview.Invalidate();
        }

        public StudentCardInfo GetCurrentStudentInfo()
        {
            return new StudentCardInfo
            {
                StudentId = txtStudentId.Text.Trim(),
                FullName = txtFullName.Text.Trim(),
                ClassName = cboClass.Text.Trim(),
                Faculty = cboFaculty.Text.Trim(),
                Course = txtCourse.Text.Trim(),
                BirthDate = dtpBirthDate.Value,
                Gender = cboGender.Text.Trim(),
                Photo = picAvatar.Image
            };
        }

        private void PnlCardPreview_Paint(object? sender, PaintEventArgs e)
        {
            StudentCardInfo info = GetCurrentStudentInfo();
            Rectangle cardRect = new Rectangle(0, 0, pnlCardPreview.Width - 1, pnlCardPreview.Height - 1);
            StudentCardRenderer.Render(e.Graphics, cardRect, info, currentTheme);
        }

        private void CboTheme_SelectedIndexChanged(object? sender, EventArgs e)
        {
            int idx = cboTheme.SelectedIndex;
            if (idx >= 0 && idx < CardTheme.PredefinedThemes.Length)
            {
                currentTheme = CardTheme.PredefinedThemes[idx];
                currentTheme.ShowQrCode = chkShowQr.Checked;
                currentTheme.ShowSmartChip = chkShowChip.Checked;
                pnlCardPreview.Invalidate();
                UpdateStatus($"Đã áp dụng giao diện thẻ: {currentTheme.Name}");
            }
        }

        private void BtnCustomBg_Click(object? sender, EventArgs e)
        {
            using ColorDialog cd = new ColorDialog { Color = currentTheme.CardBgColor, FullOpen = true };
            if (cd.ShowDialog() == DialogResult.OK)
            {
                currentTheme.CardBgColor = cd.Color;
                pnlCardPreview.Invalidate();
                UpdateStatus($"Đã đổi màu nền thẻ thành RGB({cd.Color.R}, {cd.Color.G}, {cd.Color.B})");
            }
        }

        #endregion

        #region Xử lý Ảnh an toàn (F1)

        private void BtnChoosePhoto_Click(object? sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Chọn ảnh đại diện sinh viên",
                Filter = "File ảnh (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|Tất cả các file (*.*)|*.*"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                LoadImageSafe(ofd.FileName);
            }
        }

        /// <summary>
        /// Nạp ảnh an toàn bằng Bitmap clone: Giải phóng file gốc ngay lập tức để không bị khóa file (File Lock),
        /// đồng thời Dispose ảnh cũ để tránh rò rỉ tài nguyên bộ nhớ theo đúng slide lý thuyết.
        /// </summary>
        public void LoadImageSafe(string filePath)
        {
            try
            {
                using Image temp = Image.FromFile(filePath);
                Bitmap copy = new Bitmap(temp);

                picAvatar.Image?.Dispose();
                picAvatar.Image = copy;

                pnlCardPreview.Invalidate();
                UpdateStatus($"Đã nạp ảnh thành công: {Path.GetFileName(filePath)}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"File được chọn không phải là ảnh hợp lệ hoặc bị lỗi:\n{ex.Message}",
                    "Lỗi nạp ảnh", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region In ấn & Xem trước (F4, F5)

        private void BtnPreviewPrint_Click(object? sender, EventArgs e)
        {
            StudentCardInfo info = GetCurrentStudentInfo();
            if (!info.Validate(out string errorField, out string errorMessage))
            {
                MessageBox.Show(errorMessage, "Chưa đủ thông tin in thẻ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                FocusErrorField(errorField);
                return;
            }

            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.Width = 980;
            printPreviewDialog1.Height = 720;
            printPreviewDialog1.StartPosition = FormStartPosition.CenterScreen;
            printPreviewDialog1.Text = "Xem trước bản in thẻ sinh viên - HCMUE Print Preview";

            UpdateStatus("Đang xem trước bản in thẻ sinh viên...");
            printPreviewDialog1.ShowDialog();
            UpdateStatus("Đã đóng cửa sổ xem trước bản in.");
        }

        private void BtnPrint_Click(object? sender, EventArgs e)
        {
            StudentCardInfo info = GetCurrentStudentInfo();
            if (!info.Validate(out string errorField, out string errorMessage))
            {
                MessageBox.Show(errorMessage, "Chưa đủ thông tin in thẻ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                FocusErrorField(errorField);
                return;
            }

            printDialog1.Document = printDocument1;
            printDialog1.UseEXDialog = true;

            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    UpdateStatus("Đang gửi lệnh in tới máy in...");
                    printDocument1.Print();
                    SystemSounds.Asterisk.Play();
                    MessageBox.Show("Thao tác in đã được gửi tới máy in thành công!",
                        "In thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateStatus("In thẻ sinh viên hoàn tất.");
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

            StudentCardInfo info = GetCurrentStudentInfo();

            // Tính toán kích thước thẻ khi in: Thẻ ngang chuẩn 520 x 320 đặt gọn gàng ở góc trên trang giấy
            int printX = e.MarginBounds.Left;
            int printY = e.MarginBounds.Top;
            int printW = 540;
            int printH = 340;
            Rectangle printCardRect = new Rectangle(printX, printY, printW, printH);

            // Dùng chung hàm vẽ duy nhất với Preview để đảm bảo tính nhất quán tuyệt đối
            StudentCardRenderer.Render(e.Graphics, printCardRect, info, currentTheme);

            e.HasMorePages = false;
        }

        private void FocusErrorField(string field)
        {
            if (field == nameof(StudentCardInfo.StudentId)) txtStudentId.Focus();
            else if (field == nameof(StudentCardInfo.FullName)) txtFullName.Focus();
            else if (field == nameof(StudentCardInfo.Photo)) btnChoosePhoto.Focus();
        }

        #endregion

        #region Các tính năng Nâng cao (Xuất PNG, Lưu danh sách, Nạp mẫu)

        private void BtnSavePng_Click(object? sender, EventArgs e)
        {
            StudentCardInfo info = GetCurrentStudentInfo();
            if (!info.Validate(out string errorField, out string errorMessage))
            {
                MessageBox.Show(errorMessage, "Chưa đủ thông tin xuất ảnh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                FocusErrorField(errorField);
                return;
            }

            using SaveFileDialog sfd = new SaveFileDialog
            {
                Title = "Lưu thẻ sinh viên thành file ảnh PNG",
                Filter = "PNG Image (*.png)|*.png|JPEG Image (*.jpg)|*.jpg",
                FileName = $"TheSV_{info.StudentId}_{DateTime.Now:yyyyMMdd}.png"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Kết xuất thẻ với độ phân giải cao (1080 x 680)
                    using Bitmap bmp = new Bitmap(1080, 680);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        StudentCardRenderer.Render(g, new Rectangle(0, 0, 1080, 680), info, currentTheme);
                    }

                    ImageFormat fmt = sfd.FileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ? ImageFormat.Jpeg : ImageFormat.Png;
                    bmp.Save(sfd.FileName, fmt);

                    SystemSounds.Asterisk.Play();
                    MessageBox.Show($"Đã xuất thẻ sinh viên thành công tại:\n{sfd.FileName}", "Xuất ảnh thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateStatus($"Đã lưu ảnh thẻ: {Path.GetFileName(sfd.FileName)}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lưu ảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnSaveStudent_Click(object? sender, EventArgs e)
        {
            StudentCardInfo info = GetCurrentStudentInfo();
            if (!info.Validate(out string errorField, out string errorMessage))
            {
                MessageBox.Show(errorMessage, "Chưa đủ thông tin để lưu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                FocusErrorField(errorField);
                return;
            }

            // Kiểm tra trùng mã sinh viên
            for (int i = 0; i < studentList.Count; i++)
            {
                if (studentList[i].StudentId.Equals(info.StudentId, StringComparison.OrdinalIgnoreCase))
                {
                    studentList[i] = info.Clone();
                    UpdateStatus($"Đã cập nhật sinh viên: {info.FullName} ({info.StudentId})");
                    return;
                }
            }

            studentList.Add(info.Clone());
            UpdateStatus($"Đã lưu sinh viên vào danh sách tạm. Tổng số: {studentList.Count}");
        }

        private void DgvStudents_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow != null && dgvStudents.CurrentRow.DataBoundItem is StudentCardInfo selected)
            {
                SetStudentData(selected);
                UpdateStatus($"Đã nạp sinh viên từ danh sách: {selected.FullName}");
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
            UpdateStatus("Đã làm mới toàn bộ Form về trạng thái ban đầu.");
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
                sample.ClassName = "K50.CNTT.A";
                sample.Gender = "Nữ";
                sample.BirthDate = new DateTime(2004, 10, 20);
            }

            // Tạo avatar mẫu chất lượng cao bằng GDI+
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
            UpdateStatus("Đã nạp thông tin sinh viên mẫu thành công.");
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

            picAvatar.Image?.Dispose();
            picAvatar.Image = info.Photo != null ? new Bitmap(info.Photo) : null;

            pnlCardPreview.Invalidate();
        }

        public void SetTheme(int index)
        {
            if (index >= 0 && index < cboTheme.Items.Count)
            {
                cboTheme.SelectedIndex = index;
            }
        }

        private void UpdateStatus(string message)
        {
            lblStatus.Text = message;
        }

        #endregion
    }

    /// <summary>
    /// Custom Panel bật DoubleBuffered để tránh giật lag, nhấp nháy khi vẽ preview GDI+
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
