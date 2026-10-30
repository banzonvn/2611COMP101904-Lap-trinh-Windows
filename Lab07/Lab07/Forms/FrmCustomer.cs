using System.ComponentModel;
using Lab07.Data;
using Lab07.Models;

namespace Lab07.Forms;

public partial class FrmCustomer : Form
{
    private readonly BindingSource _bindingSource = new();

    // KPI Cards
    private Panel pnlCardTotal = null!;
    private Panel pnlCardVip = null!;
    private Panel pnlCardLoyal = null!;
    private Label lblValTotal = null!;
    private Label lblValVip = null!;
    private Label lblValLoyal = null!;

    // Input Group
    private GroupBox grpInput = null!;
    private TextBox txtCustomerId = null!;
    private TextBox txtFullName = null!;
    private TextBox txtPhone = null!;
    private ComboBox cboCustomerType = null!;
    private NumericUpDown nudPoints = null!;
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

    // DataGridView
    private GroupBox grpGrid = null!;
    private DataGridView dgvCustomers = null!;

    public FrmCustomer()
    {
        InitializeCustomComponents();
        LoadCustomerData();
    }

    private void InitializeCustomComponents()
    {
        SuspendLayout();

        Text = "Quản lý khách hàng - ShopManagerMini";
        ClientSize = new Size(1020, 580);
        MinimumSize = new Size(1020, 580);
        BackColor = Color.FromArgb(248, 250, 252);
        Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
        ForeColor = Color.FromArgb(51, 65, 85);

        // Header Label
        var lblHeader = new Label
        {
            Text = "DANH SÁCH KHÁCH HÀNG & HỘI VIÊN",
            Font = new Font("Segoe UI", 12.5F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(15, 23, 42),
            Location = new Point(20, 10),
            AutoSize = true,
            UseMnemonic = false
        };

        var lblSubheader = new Label
        {
            Text = "Quản lý thông tin hội viên, phân loại khách hàng và điểm tích lũy",
            Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(100, 116, 139),
            Location = new Point(22, 34),
            AutoSize = true
        };

        // 1. KPI Cards Container
        var pnlKpi = new Panel
        {
            Location = new Point(20, 56),
            Size = new Size(980, 62),
            BackColor = Color.Transparent
        };

        pnlCardTotal = CreateKpiCard("TỔNG HỘI VIÊN", "8", Color.FromArgb(51, 65, 85), Color.FromArgb(203, 213, 225), 0, 310, out lblValTotal);
        pnlCardVip = CreateKpiCard("KHÁCH HÀNG VIP", "3 khách", Color.FromArgb(217, 119, 6), Color.FromArgb(254, 215, 170), 325, 310, out lblValVip);
        pnlCardLoyal = CreateKpiCard("KHÁCH THÂN THIẾT", "2 khách", Color.FromArgb(13, 148, 136), Color.FromArgb(153, 246, 228), 650, 330, out lblValLoyal);

        pnlKpi.Controls.AddRange(new Control[] { pnlCardTotal, pnlCardVip, pnlCardLoyal });

        var fontRegular = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

        // 2. Input Group
        grpInput = new GroupBox
        {
            Text = " Thông tin chi tiết khách hàng ",
            Location = new Point(20, 125),
            Size = new Size(650, 148),
            BackColor = Color.White,
            ForeColor = Color.FromArgb(15, 23, 42),
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point)
        };

        var lblId = new Label { Text = "Mã KH:", Location = new Point(18, 26), AutoSize = true, Font = fontRegular, ForeColor = Color.FromArgb(51, 65, 85) };
        txtCustomerId = new TextBox { Location = new Point(78, 23), Size = new Size(130, 26), Font = fontRegular };

        var lblName = new Label { Text = "Họ tên:", Location = new Point(220, 26), AutoSize = true, Font = fontRegular, ForeColor = Color.FromArgb(51, 65, 85) };
        txtFullName = new TextBox { Location = new Point(280, 23), Size = new Size(350, 26), Font = fontRegular };

        var lblPhone = new Label { Text = "Số ĐT:", Location = new Point(18, 62), AutoSize = true, Font = fontRegular, ForeColor = Color.FromArgb(51, 65, 85) };
        txtPhone = new TextBox { Location = new Point(78, 59), Size = new Size(130, 26), Font = fontRegular };

        var lblType = new Label { Text = "Loại KH:", Location = new Point(220, 62), AutoSize = true, Font = fontRegular, ForeColor = Color.FromArgb(51, 65, 85) };
        cboCustomerType = new ComboBox { Location = new Point(285, 59), Size = new Size(145, 26), Font = fontRegular, DropDownStyle = ComboBoxStyle.DropDownList };
        cboCustomerType.Items.AddRange(new object[] { "Thường", "Thân thiết", "VIP" });
        cboCustomerType.SelectedIndex = 0;

        var lblPoints = new Label { Text = "Điểm:", Location = new Point(445, 62), AutoSize = true, Font = fontRegular, ForeColor = Color.FromArgb(51, 65, 85) };
        nudPoints = new NumericUpDown { Location = new Point(515, 59), Size = new Size(115, 26), Font = fontRegular, Maximum = 999999, Minimum = 0, Value = 100 };

        // Buttons
        btnAdd = CreateActionButton("Thêm mới", Color.FromArgb(13, 148, 136), 78, 100);
        btnUpdate = CreateActionButton("Cập nhật", Color.FromArgb(71, 85, 105), 215, 100);
        btnDelete = CreateActionButton("Xóa", Color.FromArgb(225, 29, 72), 352, 100);
        btnClear = CreateActionButton("Làm mới", Color.FromArgb(100, 116, 139), 489, 100);

        btnAdd.Click += (s, e) => AddCustomer();
        btnUpdate.Click += (s, e) => UpdateCustomer();
        btnDelete.Click += (s, e) => DeleteCustomer();
        btnClear.Click += (s, e) => ClearInput();

        grpInput.Controls.AddRange(new Control[]
        {
            lblId, txtCustomerId, lblName, txtFullName, lblPhone, txtPhone,
            lblType, cboCustomerType, lblPoints, nudPoints, btnAdd, btnUpdate, btnDelete, btnClear
        });

        // 3. Search Group
        grpSearch = new GroupBox
        {
            Text = " Tra cứu khách hàng ",
            Location = new Point(680, 125),
            Size = new Size(320, 148),
            BackColor = Color.White,
            ForeColor = Color.FromArgb(15, 23, 42),
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point)
        };

        var lblSearchNote = new Label
        {
            Text = "Nhập họ tên hoặc số điện thoại:",
            Location = new Point(18, 26),
            AutoSize = true,
            Font = fontRegular,
            ForeColor = Color.FromArgb(100, 116, 139)
        };

        txtSearch = new TextBox
        {
            Location = new Point(18, 52),
            Size = new Size(284, 26),
            Font = fontRegular
        };
        txtSearch.KeyDown += (s, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                SearchCustomer();
            }
        };

        btnSearch = CreateActionButton("Tìm kiếm", Color.FromArgb(217, 119, 6), 18, 95);
        btnSearch.Size = new Size(135, 36);
        btnSearch.Click += (s, e) => SearchCustomer();

        btnShowAll = CreateActionButton("Xem tất cả", Color.FromArgb(71, 85, 105), 167, 95);
        btnShowAll.Size = new Size(135, 36);
        btnShowAll.Click += (s, e) =>
        {
            txtSearch.Clear();
            LoadCustomerData();
        };

        grpSearch.Controls.AddRange(new Control[] { lblSearchNote, txtSearch, btnSearch, btnShowAll });

        // 4. Alert Banner
        pnlAlert = new Panel
        {
            Location = new Point(20, 280),
            Size = new Size(980, 30),
            BackColor = Color.FromArgb(240, 253, 250),
            BorderStyle = BorderStyle.FixedSingle
        };

        lblAlertIcon = new Label
        {
            Text = "✓",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(13, 148, 136),
            Location = new Point(8, 3),
            Size = new Size(24, 22),
            TextAlign = ContentAlignment.MiddleCenter
        };

        lblAlertText = new Label
        {
            Text = "Hệ thống sẵn sàng. Chọn khách hàng trên danh sách để xem chi tiết.",
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(15, 23, 42),
            Location = new Point(35, 3),
            Size = new Size(935, 22),
            TextAlign = ContentAlignment.MiddleLeft
        };

        pnlAlert.Controls.AddRange(new Control[] { lblAlertIcon, lblAlertText });

        // 5. DataGridView Group
        grpGrid = new GroupBox
        {
            Text = " Danh sách khách hàng thân thiết ",
            Location = new Point(20, 318),
            Size = new Size(980, 245),
            BackColor = Color.White,
            ForeColor = Color.FromArgb(15, 23, 42),
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point)
        };

        dgvCustomers = new DataGridView
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

        dgvCustomers.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(241, 245, 249),
            ForeColor = Color.FromArgb(51, 65, 85),
            Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
            Alignment = DataGridViewContentAlignment.MiddleLeft,
            Padding = new Padding(8, 0, 0, 0)
        };
        dgvCustomers.ColumnHeadersHeight = 34;
        dgvCustomers.EnableHeadersVisualStyles = false;
        dgvCustomers.RowTemplate.Height = 30;

        dgvCustomers.DefaultCellStyle = new DataGridViewCellStyle
        {
            SelectionBackColor = Color.FromArgb(204, 251, 241),
            SelectionForeColor = Color.FromArgb(15, 23, 42),
            Padding = new Padding(8, 0, 0, 0)
        };

        dgvCustomers.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(248, 250, 252),
            SelectionBackColor = Color.FromArgb(204, 251, 241),
            SelectionForeColor = Color.FromArgb(15, 23, 42),
            Padding = new Padding(8, 0, 0, 0)
        };

        // Columns
        dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(Customer.CustomerId),
            HeaderText = "Mã KH",
            Width = 100
        });

        dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(Customer.FullName),
            HeaderText = "Họ và Tên",
            Width = 280
        });

        dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(Customer.PhoneNumber),
            HeaderText = "Số Điện Thoại",
            Width = 160
        });

        dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(Customer.CustomerType),
            HeaderText = "Loại Khách Hàng",
            Width = 170,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });

        dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(Customer.Points),
            HeaderText = "Điểm Tích Lũy",
            Width = 130,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "#,##0" }
        });

        dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(Customer.CreatedAt),
            HeaderText = "Ngày Đăng Ký",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, Format = "dd/MM/yyyy" }
        });

        dgvCustomers.CellFormatting += (s, e) =>
        {
            if (e.RowIndex >= 0 && dgvCustomers.Columns[e.ColumnIndex].DataPropertyName == nameof(Customer.CustomerType))
            {
                var val = e.Value?.ToString();
                if (val == "VIP")
                {
                    e.CellStyle!.ForeColor = Color.FromArgb(217, 119, 6); // Warm Amber
                    e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
                else if (val == "Thân thiết")
                {
                    e.CellStyle!.ForeColor = Color.FromArgb(13, 148, 136); // Teal
                    e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
            }
        };

        dgvCustomers.CellClick += (s, e) =>
        {
            if (e.RowIndex >= 0 && dgvCustomers.Rows[e.RowIndex].DataBoundItem is Customer c)
            {
                SelectCustomer(c);
            }
        };

        grpGrid.Controls.Add(dgvCustomers);

        Controls.AddRange(new Control[] { grpGrid, pnlAlert, grpSearch, grpInput, pnlKpi, lblSubheader, lblHeader });

        ResumeLayout(false);
        PerformLayout();
    }

    private Panel CreateKpiCard(string title, string value, Color valColor, Color borderColor, int x, int width, out Label valLabel)
    {
        var card = new Panel
        {
            Location = new Point(x, 0),
            Size = new Size(width, 65),
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
            Font = new Font("Segoe UI", 8F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(100, 116, 139),
            Location = new Point(14, 10),
            AutoSize = true
        };

        valLabel = new Label
        {
            Text = value,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = valColor,
            Location = new Point(12, 28),
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
            Size = new Size(130, 34),
            BackColor = backColor,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
            Cursor = Cursors.Hand,
            FlatAppearance = { BorderSize = 0 }
        };
    }

    public void LoadCustomerData()
    {
        _bindingSource.DataSource = ShopDataStore.Customers;
        dgvCustomers.DataSource = _bindingSource;
        UpdateKpi();
        ShowAlertMessage($"Đã tải {ShopDataStore.Customers.Count} khách hàng trong cơ sở dữ liệu.", true);
    }

    private void UpdateKpi()
    {
        int count = ShopDataStore.Customers.Count;
        int vip = ShopDataStore.Customers.Count(c => c.CustomerType == "VIP");
        int loyal = ShopDataStore.Customers.Count(c => c.CustomerType == "Thân thiết");

        lblValTotal.Text = $"{count} khách";
        lblValVip.Text = $"{vip} VIP";
        lblValLoyal.Text = $"{loyal} Thân thiết";
    }

    public void SetInputFields(string id, string name, string phone, string type, int points)
    {
        txtCustomerId.Text = id;
        txtFullName.Text = name;
        txtPhone.Text = phone;
        int idx = cboCustomerType.Items.IndexOf(type);
        cboCustomerType.SelectedIndex = idx >= 0 ? idx : 0;
        nudPoints.Value = Math.Max(0, points);
    }

    private void SelectCustomer(Customer c)
    {
        SetInputFields(c.CustomerId, c.FullName, c.PhoneNumber, c.CustomerType, c.Points);
        ShowAlertMessage($"Đã chọn khách hàng: {c.FullName} (Mã: {c.CustomerId})", true);
    }

    public bool AddCustomer(bool showDialog = false)
    {
        string id = txtCustomerId.Text.Trim();
        string name = txtFullName.Text.Trim();
        string phone = txtPhone.Text.Trim();
        string type = cboCustomerType.SelectedItem?.ToString() ?? "Thường";
        int points = (int)nudPoints.Value;

        // 1. Kiểm tra rỗng
        if (string.IsNullOrEmpty(id))
        {
            ShowAlertMessage("Lỗi nhập liệu: Mã khách hàng không được để trống!", false);
            if (showDialog) MessageBox.Show("Mã khách hàng không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtCustomerId.Focus();
            return false;
        }

        if (string.IsNullOrEmpty(name))
        {
            ShowAlertMessage("Lỗi nhập liệu: Họ và tên khách hàng không được để trống!", false);
            if (showDialog) MessageBox.Show("Họ và tên khách hàng không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtFullName.Focus();
            return false;
        }

        if (string.IsNullOrEmpty(phone))
        {
            ShowAlertMessage("Lỗi nhập liệu: Số điện thoại không được để trống!", false);
            if (showDialog) MessageBox.Show("Số điện thoại không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtPhone.Focus();
            return false;
        }

        // 2. Kiểm tra trùng mã
        if (ShopDataStore.CustomerExists(id))
        {
            ShowAlertMessage($"Lỗi ràng buộc: Mã khách hàng '{id}' đã tồn tại trong danh sách!", false);
            if (showDialog) MessageBox.Show($"Mã khách hàng '{id}' đã tồn tại!", "Trùng mã", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtCustomerId.Focus();
            return false;
        }

        var newCustomer = new Customer(id, name, phone, type, points);
        ShopDataStore.Customers.Add(newCustomer);
        UpdateKpi();
        ClearInput();
        ShowAlertMessage($"Thêm thành công khách hàng: {newCustomer.FullName} ({newCustomer.CustomerType})", true);
        return true;
    }

    public bool UpdateCustomer(bool showDialog = false)
    {
        string id = txtCustomerId.Text.Trim();
        string name = txtFullName.Text.Trim();
        string phone = txtPhone.Text.Trim();
        string type = cboCustomerType.SelectedItem?.ToString() ?? "Thường";

        if (string.IsNullOrEmpty(id))
        {
            ShowAlertMessage("Vui lòng chọn hoặc nhập mã khách hàng cần sửa!", false);
            return false;
        }

        var existing = ShopDataStore.Customers.FirstOrDefault(c => c.CustomerId.Equals(id, StringComparison.OrdinalIgnoreCase));
        if (existing == null)
        {
            ShowAlertMessage($"Không tìm thấy khách hàng có mã '{id}' để cập nhật!", false);
            return false;
        }

        if (string.IsNullOrEmpty(name))
        {
            ShowAlertMessage("Họ tên khách hàng không được để trống!", false);
            return false;
        }

        existing.FullName = name;
        existing.PhoneNumber = phone;
        existing.CustomerType = type;
        existing.Points = (int)nudPoints.Value;

        _bindingSource.ResetBindings(false);
        UpdateKpi();
        ShowAlertMessage($"Cập nhật thông tin khách hàng '{id}' thành công!", true);
        return true;
    }

    public bool DeleteCustomer(bool confirm = false)
    {
        string id = txtCustomerId.Text.Trim();
        if (string.IsNullOrEmpty(id))
        {
            ShowAlertMessage("Vui lòng chọn khách hàng cần xóa trên danh sách!", false);
            return false;
        }

        var existing = ShopDataStore.Customers.FirstOrDefault(c => c.CustomerId.Equals(id, StringComparison.OrdinalIgnoreCase));
        if (existing == null)
        {
            ShowAlertMessage($"Không tìm thấy khách hàng có mã '{id}' để xóa!", false);
            return false;
        }

        if (confirm)
        {
            var res = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa khách hàng '{existing.FullName}' (Mã: {id}) không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (res != DialogResult.Yes) return false;
        }

        ShopDataStore.Customers.Remove(existing);
        UpdateKpi();
        ClearInput();
        ShowAlertMessage($"Đã xóa thành công khách hàng có mã '{id}' khỏi hệ thống!", true);
        return true;
    }

    public void SearchCustomer()
    {
        string kw = txtSearch.Text.Trim();
        if (string.IsNullOrEmpty(kw))
        {
            LoadCustomerData();
            return;
        }

        var filtered = ShopDataStore.Customers.Where(c =>
            c.CustomerId.Contains(kw, StringComparison.OrdinalIgnoreCase) ||
            c.FullName.Contains(kw, StringComparison.OrdinalIgnoreCase) ||
            c.PhoneNumber.Contains(kw, StringComparison.OrdinalIgnoreCase) ||
            c.CustomerType.Contains(kw, StringComparison.OrdinalIgnoreCase)
        ).ToList();

        _bindingSource.DataSource = filtered;
        dgvCustomers.DataSource = _bindingSource;
        ShowAlertMessage($"Tìm thấy {filtered.Count} khách hàng phù hợp với từ khóa '{kw}'.", true);
    }

    public void ClearInput()
    {
        txtCustomerId.Clear();
        txtFullName.Clear();
        txtPhone.Clear();
        cboCustomerType.SelectedIndex = 0;
        nudPoints.Value = 100;
        txtCustomerId.Focus();
    }

    public void ShowAlertMessage(string message, bool isSuccess)
    {
        lblAlertText.Text = message;
        if (isSuccess)
        {
            pnlAlert.BackColor = Color.FromArgb(240, 253, 250);
            lblAlertIcon.ForeColor = Color.FromArgb(13, 148, 136);
            lblAlertIcon.Text = "✓";
        }
        else
        {
            pnlAlert.BackColor = Color.FromArgb(254, 242, 242);
            lblAlertIcon.ForeColor = Color.FromArgb(220, 38, 38);
            lblAlertIcon.Text = "⚠";
        }
    }

    public void SetSearchKeyword(string kw)
    {
        txtSearch.Text = kw;
    }
}
