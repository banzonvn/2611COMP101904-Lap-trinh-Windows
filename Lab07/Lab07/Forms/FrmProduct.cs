using System.ComponentModel;
using System.Globalization;
using Lab07.Data;
using Lab07.Models;

namespace Lab07.Forms;

public partial class FrmProduct : Form
{
    private readonly BindingSource _bindingSource = new();

    // KPI Cards
    private Panel pnlCardTotal = null!;
    private Panel pnlCardQty = null!;
    private Panel pnlCardValue = null!;
    private Label lblValTotal = null!;
    private Label lblValQty = null!;
    private Label lblValValue = null!;

    // Input Group
    private GroupBox grpInput = null!;
    private TextBox txtProductId = null!;
    private TextBox txtProductName = null!;
    private ComboBox cboCategory = null!;
    private TextBox txtPrice = null!;
    private NumericUpDown nudQuantity = null!;
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
    private DataGridView dgvProducts = null!;

    public FrmProduct()
    {
        InitializeCustomComponents();
        LoadProductData();
    }

    private void InitializeCustomComponents()
    {
        SuspendLayout();

        Text = "Quản lý sản phẩm - ShopManagerMini";
        ClientSize = new Size(1020, 580);
        MinimumSize = new Size(1020, 580);
        BackColor = Color.FromArgb(248, 250, 252); // Slate-50
        Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
        ForeColor = Color.FromArgb(51, 65, 85);

        // Header Label
        var lblHeader = new Label
        {
            Text = "DANH MỤC SẢN PHẨM & TỒN KHO",
            Font = new Font("Segoe UI", 12.5F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(15, 23, 42),
            Location = new Point(20, 10),
            AutoSize = true,
            UseMnemonic = false
        };

        var lblSubheader = new Label
        {
            Text = "Quản lý thêm, sửa, xóa, tra cứu thông tin sản phẩm và giá trị tồn kho của cửa hàng",
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

        pnlCardTotal = CreateKpiCard("TỔNG MẶT HÀNG", "10", Color.FromArgb(51, 65, 85), Color.FromArgb(203, 213, 225), 0, 310, out lblValTotal);
        pnlCardQty = CreateKpiCard("TỔNG SỐ LƯỢNG TỒN", "418 cái", Color.FromArgb(13, 148, 136), Color.FromArgb(153, 246, 228), 325, 310, out lblValQty);
        pnlCardValue = CreateKpiCard("TỔNG GIÁ TRỊ TỒN KHO", "18,250,000 đ", Color.FromArgb(217, 119, 6), Color.FromArgb(254, 215, 170), 650, 330, out lblValValue);

        pnlKpi.Controls.AddRange(new Control[] { pnlCardTotal, pnlCardQty, pnlCardValue });

        var fontRegular = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

        // 2. Input Group
        grpInput = new GroupBox
        {
            Text = " Thông tin chi tiết sản phẩm ",
            Location = new Point(20, 125),
            Size = new Size(650, 148),
            BackColor = Color.White,
            ForeColor = Color.FromArgb(15, 23, 42),
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point)
        };

        var lblId = new Label { Text = "Mã SP:", Location = new Point(18, 26), AutoSize = true, Font = fontRegular, ForeColor = Color.FromArgb(51, 65, 85) };
        txtProductId = new TextBox { Location = new Point(78, 23), Size = new Size(130, 26), Font = fontRegular };

        var lblName = new Label { Text = "Tên SP:", Location = new Point(220, 26), AutoSize = true, Font = fontRegular, ForeColor = Color.FromArgb(51, 65, 85) };
        txtProductName = new TextBox { Location = new Point(280, 23), Size = new Size(350, 26), Font = fontRegular };

        var lblCat = new Label { Text = "Loại SP:", Location = new Point(18, 62), AutoSize = true, Font = fontRegular, ForeColor = Color.FromArgb(51, 65, 85) };
        cboCategory = new ComboBox { Location = new Point(78, 59), Size = new Size(130, 26), Font = fontRegular, DropDownStyle = ComboBoxStyle.DropDownList };
        cboCategory.Items.AddRange(new object[] { "Bánh kẹo", "Đồ uống", "Hóa mỹ phẩm", "Thực phẩm đóng hộp", "Gia dụng" });
        cboCategory.SelectedIndex = 0;

        var lblPrice = new Label { Text = "Đơn giá:", Location = new Point(220, 62), AutoSize = true, Font = fontRegular, ForeColor = Color.FromArgb(51, 65, 85) };
        txtPrice = new TextBox { Location = new Point(280, 59), Size = new Size(150, 26), Font = fontRegular, Text = "10000" };

        var lblQty = new Label { Text = "Số lượng:", Location = new Point(445, 62), AutoSize = true, Font = fontRegular, ForeColor = Color.FromArgb(51, 65, 85) };
        nudQuantity = new NumericUpDown { Location = new Point(515, 59), Size = new Size(115, 26), Font = fontRegular, Maximum = 99999, Minimum = 0, Value = 10 };

        // Buttons
        btnAdd = CreateActionButton("Thêm mới", Color.FromArgb(13, 148, 136), 78, 100);
        btnUpdate = CreateActionButton("Cập nhật", Color.FromArgb(71, 85, 105), 215, 100);
        btnDelete = CreateActionButton("Xóa", Color.FromArgb(225, 29, 72), 352, 100);
        btnClear = CreateActionButton("Làm mới", Color.FromArgb(100, 116, 139), 489, 100);

        btnAdd.Click += (s, e) => AddProduct();
        btnUpdate.Click += (s, e) => UpdateProduct();
        btnDelete.Click += (s, e) => DeleteProduct();
        btnClear.Click += (s, e) => ClearInput();

        grpInput.Controls.AddRange(new Control[]
        {
            lblId, txtProductId, lblName, txtProductName, lblCat, cboCategory,
            lblPrice, txtPrice, lblQty, nudQuantity, btnAdd, btnUpdate, btnDelete, btnClear
        });

        // 3. Search Group
        grpSearch = new GroupBox
        {
            Text = " Tra cứu sản phẩm ",
            Location = new Point(680, 125),
            Size = new Size(320, 148),
            BackColor = Color.White,
            ForeColor = Color.FromArgb(15, 23, 42),
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point)
        };

        var lblSearchNote = new Label
        {
            Text = "Nhập tên hoặc mã sản phẩm:",
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
                SearchProduct();
            }
        };

        btnSearch = CreateActionButton("Tìm kiếm", Color.FromArgb(217, 119, 6), 18, 95);
        btnSearch.Size = new Size(135, 36);
        btnSearch.Click += (s, e) => SearchProduct();

        btnShowAll = CreateActionButton("Xem tất cả", Color.FromArgb(71, 85, 105), 167, 95);
        btnShowAll.Size = new Size(135, 36);
        btnShowAll.Click += (s, e) =>
        {
            txtSearch.Clear();
            LoadProductData();
        };

        grpSearch.Controls.AddRange(new Control[] { lblSearchNote, txtSearch, btnSearch, btnShowAll });

        // 4. Alert Banner
        pnlAlert = new Panel
        {
            Location = new Point(20, 280),
            Size = new Size(980, 30),
            BackColor = Color.FromArgb(240, 253, 250), // Teal-50
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
            Text = "Hệ thống sẵn sàng. Chọn sản phẩm trên danh sách để xem hoặc chỉnh sửa.",
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
            Text = " Danh sách hàng hóa trong kho ",
            Location = new Point(20, 318),
            Size = new Size(980, 245),
            BackColor = Color.White,
            ForeColor = Color.FromArgb(15, 23, 42),
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point)
        };

        dgvProducts = new DataGridView
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

        dgvProducts.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(241, 245, 249),
            ForeColor = Color.FromArgb(51, 65, 85),
            Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
            Alignment = DataGridViewContentAlignment.MiddleLeft,
            Padding = new Padding(8, 0, 0, 0)
        };
        dgvProducts.ColumnHeadersHeight = 34;
        dgvProducts.EnableHeadersVisualStyles = false;
        dgvProducts.RowTemplate.Height = 30;

        dgvProducts.DefaultCellStyle = new DataGridViewCellStyle
        {
            SelectionBackColor = Color.FromArgb(204, 251, 241), // Teal-100
            SelectionForeColor = Color.FromArgb(15, 23, 42),
            Padding = new Padding(8, 0, 0, 0)
        };

        dgvProducts.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(248, 250, 252),
            SelectionBackColor = Color.FromArgb(204, 251, 241),
            SelectionForeColor = Color.FromArgb(15, 23, 42),
            Padding = new Padding(8, 0, 0, 0)
        };

        // Columns
        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(Product.ProductId),
            HeaderText = "Mã SP",
            Width = 90
        });

        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(Product.ProductName),
            HeaderText = "Tên Sản Phẩm",
            Width = 270
        });

        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(Product.Category),
            HeaderText = "Loại Hàng",
            Width = 140
        });

        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(Product.Price),
            HeaderText = "Đơn Giá",
            Width = 120,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "#,##0 VNĐ" }
        });

        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(Product.Quantity),
            HeaderText = "SL Tồn",
            Width = 85,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });

        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(Product.TotalAmount),
            HeaderText = "Thành Tiền",
            Width = 140,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "#,##0 VNĐ" }
        });

        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(Product.StockStatus),
            HeaderText = "Trạng Thái",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });

        dgvProducts.CellFormatting += (s, e) =>
        {
            if (e.RowIndex >= 0 && dgvProducts.Columns[e.ColumnIndex].DataPropertyName == nameof(Product.StockStatus))
            {
                var val = e.Value?.ToString();
                if (val == "Hết hàng")
                {
                    e.CellStyle!.ForeColor = Color.FromArgb(220, 38, 38);
                    e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
                else if (val == "Sắp hết")
                {
                    e.CellStyle!.ForeColor = Color.FromArgb(217, 119, 6);
                    e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
                else
                {
                    e.CellStyle!.ForeColor = Color.FromArgb(22, 163, 74);
                }
            }
        };

        dgvProducts.CellClick += (s, e) =>
        {
            if (e.RowIndex >= 0 && dgvProducts.Rows[e.RowIndex].DataBoundItem is Product p)
            {
                SelectProduct(p);
            }
        };

        grpGrid.Controls.Add(dgvProducts);

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

    public void LoadProductData()
    {
        _bindingSource.DataSource = ShopDataStore.Products;
        dgvProducts.DataSource = _bindingSource;
        UpdateKpi();
        ShowAlertMessage($"Đã tải {ShopDataStore.Products.Count} sản phẩm trong kho dữ liệu.", true);
    }

    private void UpdateKpi()
    {
        int count = ShopDataStore.Products.Count;
        int totalQty = ShopDataStore.Products.Sum(p => p.Quantity);
        decimal totalVal = ShopDataStore.Products.Sum(p => p.TotalAmount);

        lblValTotal.Text = $"{count} mặt hàng";
        lblValQty.Text = $"{totalQty:#,##0} cái";
        lblValValue.Text = $"{totalVal:#,##0} VNĐ";
    }

    public void SetInputFields(string id, string name, string category, decimal price, int quantity)
    {
        txtProductId.Text = id;
        txtProductName.Text = name;
        int idx = cboCategory.Items.IndexOf(category);
        cboCategory.SelectedIndex = idx >= 0 ? idx : 0;
        txtPrice.Text = price.ToString();
        nudQuantity.Value = Math.Max(0, quantity);
    }

    private void SelectProduct(Product p)
    {
        SetInputFields(p.ProductId, p.ProductName, p.Category, p.Price, p.Quantity);
        ShowAlertMessage($"Đã chọn sản phẩm: {p.ProductName} (Mã: {p.ProductId})", true);
    }

    public bool AddProduct(bool showDialog = false)
    {
        string id = txtProductId.Text.Trim();
        string name = txtProductName.Text.Trim();
        string category = cboCategory.SelectedItem?.ToString() ?? "Bánh kẹo";

        // 1. Kiểm tra rỗng
        if (string.IsNullOrEmpty(id))
        {
            ShowAlertMessage("Lỗi nhập liệu: Mã sản phẩm không được để trống!", false);
            if (showDialog) MessageBox.Show("Mã sản phẩm không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtProductId.Focus();
            return false;
        }

        if (string.IsNullOrEmpty(name))
        {
            ShowAlertMessage("Lỗi nhập liệu: Tên sản phẩm không được để trống!", false);
            if (showDialog) MessageBox.Show("Tên sản phẩm không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtProductName.Focus();
            return false;
        }

        // 2. Kiểm tra giá tiền
        if (!decimal.TryParse(txtPrice.Text.Trim().Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out decimal price) || price <= 0)
        {
            ShowAlertMessage("Lỗi nhập liệu: Đơn giá phải là số lớn hơn 0!", false);
            if (showDialog) MessageBox.Show("Đơn giá phải là số lớn hơn 0!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtPrice.Focus();
            return false;
        }

        int quantity = (int)nudQuantity.Value;
        if (quantity < 0)
        {
            ShowAlertMessage("Lỗi nhập liệu: Số lượng không được âm!", false);
            if (showDialog) MessageBox.Show("Số lượng không được âm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        // 3. Kiểm tra trùng mã
        if (ShopDataStore.ProductExists(id))
        {
            ShowAlertMessage($"Lỗi ràng buộc: Mã sản phẩm '{id}' đã tồn tại trong danh mục!", false);
            if (showDialog) MessageBox.Show($"Mã sản phẩm '{id}' đã tồn tại!", "Trùng mã", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtProductId.Focus();
            return false;
        }

        // Thêm mới
        var newProduct = new Product(id, name, category, price, quantity);
        ShopDataStore.Products.Add(newProduct);
        UpdateKpi();
        ClearInput();
        ShowAlertMessage($"Thêm thành công sản phẩm: {newProduct.ProductName} (Mã: {newProduct.ProductId})", true);
        return true;
    }

    public bool UpdateProduct(bool showDialog = false)
    {
        string id = txtProductId.Text.Trim();
        string name = txtProductName.Text.Trim();
        string category = cboCategory.SelectedItem?.ToString() ?? "Bánh kẹo";

        if (string.IsNullOrEmpty(id))
        {
            ShowAlertMessage("Vui lòng chọn hoặc nhập mã sản phẩm cần sửa!", false);
            return false;
        }

        var existing = ShopDataStore.Products.FirstOrDefault(p => p.ProductId.Equals(id, StringComparison.OrdinalIgnoreCase));
        if (existing == null)
        {
            ShowAlertMessage($"Không tìm thấy sản phẩm có mã '{id}' để cập nhật!", false);
            return false;
        }

        if (string.IsNullOrEmpty(name))
        {
            ShowAlertMessage("Tên sản phẩm không được để trống!", false);
            return false;
        }

        if (!decimal.TryParse(txtPrice.Text.Trim().Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out decimal price) || price <= 0)
        {
            ShowAlertMessage("Đơn giá phải là số lớn hơn 0!", false);
            return false;
        }

        existing.ProductName = name;
        existing.Category = category;
        existing.Price = price;
        existing.Quantity = (int)nudQuantity.Value;

        _bindingSource.ResetBindings(false);
        UpdateKpi();
        ShowAlertMessage($"Cập nhật thông tin sản phẩm '{id}' thành công!", true);
        return true;
    }

    public bool DeleteProduct(bool confirm = false)
    {
        string id = txtProductId.Text.Trim();
        if (string.IsNullOrEmpty(id))
        {
            ShowAlertMessage("Vui lòng chọn sản phẩm cần xóa trên danh sách!", false);
            return false;
        }

        var existing = ShopDataStore.Products.FirstOrDefault(p => p.ProductId.Equals(id, StringComparison.OrdinalIgnoreCase));
        if (existing == null)
        {
            ShowAlertMessage($"Không tìm thấy sản phẩm có mã '{id}' để xóa!", false);
            return false;
        }

        if (confirm)
        {
            var res = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa sản phẩm '{existing.ProductName}' (Mã: {id}) không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (res != DialogResult.Yes) return false;
        }

        ShopDataStore.Products.Remove(existing);
        UpdateKpi();
        ClearInput();
        ShowAlertMessage($"Đã xóa thành công sản phẩm có mã '{id}' khỏi kho dữ liệu!", true);
        return true;
    }

    public void SearchProduct()
    {
        string kw = txtSearch.Text.Trim();
        if (string.IsNullOrEmpty(kw))
        {
            LoadProductData();
            return;
        }

        var filtered = ShopDataStore.Products.Where(p =>
            p.ProductId.Contains(kw, StringComparison.OrdinalIgnoreCase) ||
            p.ProductName.Contains(kw, StringComparison.OrdinalIgnoreCase) ||
            p.Category.Contains(kw, StringComparison.OrdinalIgnoreCase)
        ).ToList();

        _bindingSource.DataSource = filtered;
        dgvProducts.DataSource = _bindingSource;
        ShowAlertMessage($"Tìm thấy {filtered.Count} sản phẩm phù hợp với từ khóa '{kw}'.", true);
    }

    public void ClearInput()
    {
        txtProductId.Clear();
        txtProductName.Clear();
        cboCategory.SelectedIndex = 0;
        txtPrice.Text = "10000";
        nudQuantity.Value = 10;
        txtProductId.Focus();
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
