using System.ComponentModel;
using System.Drawing.Drawing2D;
using Lab15.Models;
using Lab15.Presenters;

namespace Lab15.Views;

public partial class FrmStudent : Form, IStudentView
{
    private readonly StudentPresenter _presenter;
    private readonly BindingSource _bindingSource = new();

    // UI Controls
    private Panel pnlHeader = null!;
    private Label lblTitle = null!;
    private Label lblSubtitle = null!;
    private Label lblBadge = null!;

    // KPI Cards
    private Panel pnlCardTotal = null!;
    private Panel pnlCardAvg = null!;
    private Panel pnlCardPass = null!;
    private Panel pnlCardFail = null!;
    private Label lblValTotal = null!;
    private Label lblValAvg = null!;
    private Label lblValPass = null!;
    private Label lblValFail = null!;

    // Input Group
    private GroupBox grpInput = null!;
    private TextBox txtStudentId = null!;
    private TextBox txtFullName = null!;
    private ComboBox cboClass = null!;
    private TextBox txtAverageScore = null!;
    private Button btnAdd = null!;
    private Button btnUpdate = null!;
    private Button btnDelete = null!;
    private Button btnClear = null!;

    // Search Group
    private GroupBox grpSearch = null!;
    private TextBox txtSearch = null!;
    private Button btnSearch = null!;
    private Button btnShowAll = null!;

    // Alert Banner
    private Panel pnlAlert = null!;
    private Label lblAlertIcon = null!;
    private Label lblAlertText = null!;

    // Grid Group
    private GroupBox grpGrid = null!;
    private DataGridView dgvStudents = null!;

    // Status Strip
    private StatusStrip statusStrip = null!;
    private ToolStripStatusLabel lblStatusText = null!;
    private ToolStripStatusLabel lblStatusCount = null!;
    private ToolStripStatusLabel lblStatusArch = null!;

    public FrmStudent()
    {
        InitializeCustomComponents();
        _presenter = new StudentPresenter(this, new Repositories.StudentRepository());
        _presenter.LoadStudents();
    }

    private void InitializeCustomComponents()
    {
        SuspendLayout();

        // Form settings
        Text = "Quản lý Sinh viên - Kiến trúc MVP (Model-View-Presenter)";
        ClientSize = new Size(1180, 730);
        MinimumSize = new Size(1180, 730);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.FromArgb(248, 250, 252); // Slate-50 modern neutral
        Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
        ForeColor = Color.FromArgb(51, 65, 85);

        // 1. Header Panel
        pnlHeader = new Panel
        {
            Location = new Point(0, 0),
            Size = new Size(1180, 75),
            Dock = DockStyle.Top,
            BackColor = Color.White
        };
        pnlHeader.Paint += (s, e) =>
        {
            using var pen = new Pen(Color.FromArgb(226, 232, 240), 1);
            e.Graphics.DrawLine(pen, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
        };

        lblTitle = new Label
        {
            Text = "HỆ THỐNG QUẢN LÝ SINH VIÊN (KIẾN TRÚC MVP)",
            Font = new Font("Segoe UI", 13.5F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(15, 23, 42), // Slate-900
            Location = new Point(24, 12),
            AutoSize = true
        };

        lblSubtitle = new Label
        {
            Text = "Môn: Lập trình trên Windows | Sinh viên: Lê Hoàng Quân - MSSV: 51.01.104.082 | Lớp: 51.CNTT.A",
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(100, 116, 139),
            Location = new Point(26, 42),
            AutoSize = true
        };

        lblBadge = new Label
        {
            Text = "MÔ HÌNH MVP / LAB 15",
            Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(13, 148, 136), // Teal-600
            BackColor = Color.FromArgb(240, 253, 250), // Teal-50
            Location = new Point(960, 22),
            Size = new Size(190, 30),
            TextAlign = ContentAlignment.MiddleCenter,
            UseMnemonic = false
        };

        pnlHeader.Controls.AddRange(new Control[] { lblTitle, lblSubtitle, lblBadge });

        // 2. KPI Cards Panel
        Panel pnlCardsContainer = new Panel
        {
            Location = new Point(20, 85),
            Size = new Size(1140, 75),
            BackColor = Color.Transparent
        };

        pnlCardTotal = CreateKpiCard("TỔNG SINH VIÊN", "12", Color.FromArgb(51, 65, 85), Color.FromArgb(203, 213, 225), 0, out lblValTotal);
        pnlCardAvg = CreateKpiCard("ĐIỂM TRUNG BÌNH", "7.2", Color.FromArgb(13, 148, 136), Color.FromArgb(153, 246, 228), 295, out lblValAvg);
        pnlCardPass = CreateKpiCard("SỐ LƯỢNG ĐẠT (>= 5.0)", "10", Color.FromArgb(22, 163, 74), Color.FromArgb(187, 247, 208), 590, out lblValPass);
        pnlCardFail = CreateKpiCard("CHƯA ĐẠT (< 5.0)", "2", Color.FromArgb(217, 119, 6), Color.FromArgb(254, 215, 170), 885, out lblValFail);

        pnlCardsContainer.Controls.AddRange(new Control[] { pnlCardTotal, pnlCardAvg, pnlCardPass, pnlCardFail });

        // 3. Input GroupBox
        grpInput = new GroupBox
        {
            Text = " Thông tin sinh viên ",
            Location = new Point(20, 170),
            Size = new Size(745, 150),
            BackColor = Color.White,
            ForeColor = Color.FromArgb(15, 23, 42),
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point)
        };

        var fontRegular = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

        var lblId = new Label { Text = "Mã SV:", Location = new Point(20, 28), AutoSize = true, Font = fontRegular, ForeColor = Color.FromArgb(51, 65, 85) };
        txtStudentId = new TextBox { Location = new Point(85, 25), Size = new Size(245, 26), Font = fontRegular };

        var lblName = new Label { Text = "Họ và tên:", Location = new Point(355, 28), AutoSize = true, Font = fontRegular, ForeColor = Color.FromArgb(51, 65, 85) };
        txtFullName = new TextBox { Location = new Point(440, 25), Size = new Size(280, 26), Font = fontRegular };

        var lblClass = new Label { Text = "Lớp học:", Location = new Point(20, 64), AutoSize = true, Font = fontRegular, ForeColor = Color.FromArgb(51, 65, 85) };
        cboClass = new ComboBox { Location = new Point(85, 61), Size = new Size(245, 26), Font = fontRegular, DropDownStyle = ComboBoxStyle.DropDownList };
        cboClass.Items.AddRange(new object[] { "51.CNTT.A", "CTK47A", "CTK47B" });
        cboClass.SelectedIndex = 0;

        var lblScore = new Label { Text = "Điểm TB:", Location = new Point(355, 64), AutoSize = true, Font = fontRegular, ForeColor = Color.FromArgb(51, 65, 85) };
        txtAverageScore = new TextBox { Location = new Point(440, 61), Size = new Size(280, 26), Font = fontRegular };

        // Buttons
        btnAdd = CreateActionButton("Thêm mới", Color.FromArgb(13, 148, 136), 85, 102);
        btnUpdate = CreateActionButton("Cập nhật", Color.FromArgb(71, 85, 105), 235, 102);
        btnDelete = CreateActionButton("Xóa", Color.FromArgb(225, 29, 72), 385, 102);
        btnClear = CreateActionButton("Làm mới", Color.FromArgb(100, 116, 139), 535, 102);

        btnAdd.Click += (s, e) => _presenter.AddStudent();
        btnUpdate.Click += (s, e) => _presenter.UpdateStudent();
        btnDelete.Click += (s, e) => _presenter.DeleteStudent();
        btnClear.Click += (s, e) => _presenter.ClearInput();

        grpInput.Controls.AddRange(new Control[] { lblId, txtStudentId, lblName, txtFullName, lblClass, cboClass, lblScore, txtAverageScore, btnAdd, btnUpdate, btnDelete, btnClear });

        // 4. Search GroupBox
        grpSearch = new GroupBox
        {
            Text = " Tra cứu và lọc dữ liệu ",
            Location = new Point(780, 170),
            Size = new Size(380, 150),
            BackColor = Color.White,
            ForeColor = Color.FromArgb(15, 23, 42),
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point)
        };

        var lblSearchNote = new Label
        {
            Text = "Nhập mã SV, họ tên hoặc tên lớp:",
            Location = new Point(20, 26),
            AutoSize = true,
            Font = fontRegular,
            ForeColor = Color.FromArgb(100, 116, 139)
        };

        txtSearch = new TextBox
        {
            Location = new Point(20, 52),
            Size = new Size(340, 26),
            Font = fontRegular
        };
        txtSearch.KeyDown += (s, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                _presenter.SearchStudent();
            }
        };

        btnSearch = CreateActionButton("Tìm kiếm", Color.FromArgb(217, 119, 6), 20, 95);
        btnSearch.Size = new Size(160, 36);
        btnSearch.Click += (s, e) => _presenter.SearchStudent();

        btnShowAll = CreateActionButton("Xem tất cả", Color.FromArgb(71, 85, 105), 200, 95);
        btnShowAll.Size = new Size(160, 36);
        btnShowAll.Click += (s, e) =>
        {
            txtSearch.Clear();
            _presenter.LoadStudents();
        };

        grpSearch.Controls.AddRange(new Control[] { lblSearchNote, txtSearch, btnSearch, btnShowAll });

        // 5. Alert Banner
        pnlAlert = new Panel
        {
            Location = new Point(20, 328),
            Size = new Size(1140, 36),
            BackColor = Color.FromArgb(240, 253, 250), // Teal-50
            BorderStyle = BorderStyle.FixedSingle
        };

        lblAlertIcon = new Label
        {
            Text = "✓",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(13, 148, 136),
            Location = new Point(10, 6),
            Size = new Size(25, 24),
            TextAlign = ContentAlignment.MiddleCenter
        };

        lblAlertText = new Label
        {
            Text = "Hệ thống sẵn sàng. Vui lòng thao tác qua các chức năng trên.",
            Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(15, 23, 42),
            Location = new Point(40, 6),
            Size = new Size(1080, 24),
            TextAlign = ContentAlignment.MiddleLeft
        };

        pnlAlert.Controls.AddRange(new Control[] { lblAlertIcon, lblAlertText });

        // 6. Grid GroupBox
        grpGrid = new GroupBox
        {
            Text = " Danh sách sinh viên (Hiển thị qua MVP Binding) ",
            Location = new Point(20, 370),
            Size = new Size(1140, 310),
            BackColor = Color.White,
            ForeColor = Color.FromArgb(15, 23, 42),
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point)
        };

        dgvStudents = new DataGridView
        {
            Dock = DockStyle.Fill,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
            GridColor = Color.FromArgb(241, 245, 249),
            RowHeadersVisible = false,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AutoGenerateColumns = false,
            Font = fontRegular
        };

        dgvStudents.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(241, 245, 249),
            ForeColor = Color.FromArgb(51, 65, 85),
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point),
            Alignment = DataGridViewContentAlignment.MiddleLeft,
            Padding = new Padding(8, 0, 0, 0)
        };
        dgvStudents.ColumnHeadersHeight = 36;
        dgvStudents.EnableHeadersVisualStyles = false;
        dgvStudents.RowTemplate.Height = 32;

        dgvStudents.DefaultCellStyle = new DataGridViewCellStyle
        {
            SelectionBackColor = Color.FromArgb(204, 251, 241), // Teal-100
            SelectionForeColor = Color.FromArgb(15, 23, 42),
            Padding = new Padding(8, 0, 0, 0)
        };

        dgvStudents.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(248, 250, 252),
            SelectionBackColor = Color.FromArgb(204, 251, 241),
            SelectionForeColor = Color.FromArgb(15, 23, 42),
            Padding = new Padding(8, 0, 0, 0)
        };

        // Columns definition
        dgvStudents.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(Student.StudentId),
            HeaderText = "Mã Sinh Viên",
            Width = 140
        });

        dgvStudents.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(Student.FullName),
            HeaderText = "Họ và Tên",
            Width = 270
        });

        dgvStudents.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(Student.ClassName),
            HeaderText = "Lớp Học",
            Width = 130
        });

        dgvStudents.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(Student.AverageScore),
            HeaderText = "Điểm TB",
            Width = 110,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, Format = "0.0#" }
        });

        dgvStudents.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(Student.Result),
            HeaderText = "Kết Quả",
            Width = 130,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });

        dgvStudents.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(Student.Classification),
            HeaderText = "Xếp Loại Học Lực",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
        });

        // Styling cells conditionally (Đạt / Không đạt)
        dgvStudents.CellFormatting += (s, e) =>
        {
            if (e.RowIndex >= 0 && dgvStudents.Columns[e.ColumnIndex].DataPropertyName == nameof(Student.Result))
            {
                var val = e.Value?.ToString();
                if (val == "Đạt")
                {
                    e.CellStyle!.ForeColor = Color.FromArgb(22, 163, 74); // Green-600
                    e.CellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                }
                else if (val == "Không đạt")
                {
                    e.CellStyle!.ForeColor = Color.FromArgb(220, 38, 38); // Red-600
                    e.CellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                }
            }
        };

        // Row click event -> Presenter
        dgvStudents.CellClick += (s, e) =>
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvStudents.Rows[e.RowIndex];
                if (row.DataBoundItem is Student student)
                {
                    _presenter.SelectStudent(student.StudentId);
                }
            }
        };

        grpGrid.Controls.Add(dgvStudents);

        // 7. StatusStrip
        statusStrip = new StatusStrip
        {
            BackColor = Color.White,
            Font = fontRegular,
            Dock = DockStyle.Bottom
        };

        lblStatusText = new ToolStripStatusLabel
        {
            Text = "Hệ thống sẵn sàng",
            AutoSize = false,
            Width = 420,
            TextAlign = ContentAlignment.MiddleLeft
        };

        lblStatusCount = new ToolStripStatusLabel
        {
            Text = "Tổng số: 12 sinh viên",
            BorderSides = ToolStripStatusLabelBorderSides.Left,
            BorderStyle = Border3DStyle.Etched,
            AutoSize = false,
            Width = 180,
            TextAlign = ContentAlignment.MiddleCenter
        };

        lblStatusArch = new ToolStripStatusLabel
        {
            Text = "Kiến trúc MVP (View - Presenter - Model)",
            BorderSides = ToolStripStatusLabelBorderSides.Left,
            BorderStyle = Border3DStyle.Etched,
            ForeColor = Color.FromArgb(13, 148, 136),
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            AutoSize = false,
            Width = 320,
            TextAlign = ContentAlignment.MiddleCenter
        };

        statusStrip.Items.AddRange(new ToolStripItem[] { lblStatusText, lblStatusCount, lblStatusArch });

        // Add controls to Form
        Controls.Add(grpGrid);
        Controls.Add(pnlAlert);
        Controls.Add(grpSearch);
        Controls.Add(grpInput);
        Controls.Add(pnlCardsContainer);
        Controls.Add(pnlHeader);
        Controls.Add(statusStrip);

        ResumeLayout(false);
        PerformLayout();
    }

    private Panel CreateKpiCard(string title, string value, Color valueColor, Color borderColor, int x, out Label valLabel)
    {
        var card = new Panel
        {
            Location = new Point(x, 0),
            Size = new Size(250, 75),
            BackColor = Color.White
        };
        card.Paint += (s, e) =>
        {
            using var pen = new Pen(borderColor, 1);
            e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
        };

        var lblHeader = new Label
        {
            Text = title,
            Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(100, 116, 139),
            Location = new Point(14, 12),
            AutoSize = true
        };

        valLabel = new Label
        {
            Text = value,
            Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = valueColor,
            Location = new Point(12, 32),
            AutoSize = true
        };

        card.Controls.AddRange(new Control[] { lblHeader, valLabel });
        return card;
    }

    private Button CreateActionButton(string text, Color backColor, int x, int y)
    {
        return new Button
        {
            Text = text,
            Location = new Point(x, y),
            Size = new Size(135, 34),
            BackColor = backColor,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
            Cursor = Cursors.Hand,
            FlatAppearance = { BorderSize = 0 }
        };
    }

    #region Implementation of IStudentView

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string StudentId
    {
        get => txtStudentId.Text.Trim();
        set => txtStudentId.Text = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string FullName
    {
        get => txtFullName.Text.Trim();
        set => txtFullName.Text = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string ClassName
    {
        get => cboClass.SelectedItem?.ToString() ?? "51.CNTT.A";
        set
        {
            int idx = cboClass.Items.IndexOf(value);
            cboClass.SelectedIndex = idx >= 0 ? idx : 0;
        }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string AverageScoreText
    {
        get => txtAverageScore.Text.Trim();
        set => txtAverageScore.Text = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string SearchKeyword
    {
        get => txtSearch.Text.Trim();
        set => txtSearch.Text = value;
    }

    public void ShowStudents(IEnumerable<Student> students)
    {
        _bindingSource.DataSource = students.ToList();
        dgvStudents.DataSource = _bindingSource;
    }

    public void ShowMessage(string message, bool isSuccess = true)
    {
        lblAlertText.Text = message;
        if (isSuccess)
        {
            pnlAlert.BackColor = Color.FromArgb(240, 253, 250); // Soft Teal/Green
            lblAlertIcon.ForeColor = Color.FromArgb(13, 148, 136);
            lblAlertIcon.Text = "✓";
            lblStatusText.Text = message;
            lblStatusText.ForeColor = Color.FromArgb(15, 23, 42);
        }
        else
        {
            pnlAlert.BackColor = Color.FromArgb(254, 242, 242); // Soft Red
            lblAlertIcon.ForeColor = Color.FromArgb(220, 38, 38);
            lblAlertIcon.Text = "⚠";
            lblStatusText.Text = message;
            lblStatusText.ForeColor = Color.FromArgb(220, 38, 38);
        }
    }

    public void ClearInput()
    {
        txtStudentId.Clear();
        txtFullName.Clear();
        cboClass.SelectedIndex = 0;
        txtAverageScore.Clear();
        txtStudentId.Focus();
    }

    public void SetInputFields(Student student)
    {
        StudentId = student.StudentId;
        FullName = student.FullName;
        ClassName = student.ClassName;
        AverageScoreText = student.AverageScore.ToString("0.0#");

        // Đồng bộ chọn dòng trên grid
        foreach (DataGridViewRow row in dgvStudents.Rows)
        {
            if (row.DataBoundItem is Student s && s.StudentId == student.StudentId)
            {
                row.Selected = true;
                dgvStudents.CurrentCell = row.Cells[0];
                break;
            }
        }
    }

    public void UpdateStatistics(int totalCount, double avgScore, int passCount, int failCount)
    {
        lblValTotal.Text = totalCount.ToString();
        lblValAvg.Text = avgScore.ToString("0.0#");
        lblValPass.Text = passCount.ToString();
        lblValFail.Text = failCount.ToString();
        lblStatusCount.Text = $"Tổng số: {totalCount} sinh viên";
    }

    #endregion

    // Helper for automation / screenshot testing
    public StudentPresenter Presenter => _presenter;
    public DataGridView Grid => dgvStudents;
}
