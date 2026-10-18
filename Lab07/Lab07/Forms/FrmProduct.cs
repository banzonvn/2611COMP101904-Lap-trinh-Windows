using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Lab07.Models;

namespace Lab07
{
    public partial class FrmProduct : Form
    {
        private readonly BindingList<Product> _products = new BindingList<Product>();
        private readonly BindingSource _bs = new BindingSource();

        public FrmProduct()
        {
            InitializeComponent();
        }

        private void FrmProduct_Load(object sender, EventArgs e)
        {
            cboLoaiSP.Items.AddRange(new object[] { "Điện tử", "Văn phòng", "Gia dụng", "Thời trang" });
            cboLoaiSP.SelectedIndex = 0;

            _bs.DataSource = _products;
            dgvProducts.DataSource = _bs;

            // Seed data mẫu
            _products.Add(new Product("SP001", "Laptop Dell XPS", "Điện tử", 28000000m, 5));
            _products.Add(new Product("SP002", "Ghế văn phòng", "Văn phòng", 3500000m, 20));
            _products.Add(new Product("SP003", "Nồi cơm điện", "Gia dụng", 750000m, 30));

            SetupGrid();
            UpdateStatus();
        }

        private void SetupGrid()
        {
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.ReadOnly = true;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;

            var cols = dgvProducts.Columns;
            if (cols["MaSP"] != null) { cols["MaSP"].HeaderText = "Mã SP"; cols["MaSP"].FillWeight = 80; }
            if (cols["TenSP"] != null) { cols["TenSP"].HeaderText = "Tên sản phẩm"; cols["TenSP"].FillWeight = 200; }
            if (cols["LoaiSP"] != null) { cols["LoaiSP"].HeaderText = "Loại"; cols["LoaiSP"].FillWeight = 100; }
            if (cols["DonGia"] != null)
            {
                cols["DonGia"].HeaderText = "Đơn giá (VNĐ)";
                cols["DonGia"].DefaultCellStyle.Format = "N0";
                cols["DonGia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                cols["DonGia"].FillWeight = 120;
            }
            if (cols["SoLuong"] != null)
            {
                cols["SoLuong"].HeaderText = "Số lượng";
                cols["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                cols["SoLuong"].FillWeight = 80;
            }
        }

        private void UpdateStatus()
        {
            lblCount.Text = $"Tổng: {_products.Count} sản phẩm";
        }

        private bool ValidateInput(bool checkDuplicate)
        {
            errProv.Clear();
            bool ok = true;

            if (string.IsNullOrWhiteSpace(txtMaSP.Text))
            {
                errProv.SetError(txtMaSP, "Mã sản phẩm không được rỗng.");
                ok = false;
            }
            else if (checkDuplicate && _products.Any(p => p.MaSP.Equals(txtMaSP.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                errProv.SetError(txtMaSP, "Mã này đã tồn tại.");
                ok = false;
            }

            if (string.IsNullOrWhiteSpace(txtTenSP.Text))
            {
                errProv.SetError(txtTenSP, "Tên sản phẩm không được rỗng.");
                ok = false;
            }

            if (numDonGia.Value <= 0)
            {
                errProv.SetError(numDonGia, "Đơn giá phải lớn hơn 0.");
                ok = false;
            }

            if (numSoLuong.Value < 0)
            {
                errProv.SetError(numSoLuong, "Số lượng không được âm.");
                ok = false;
            }

            return ok;
        }

        private void ClearInput()
        {
            txtMaSP.Clear();
            txtTenSP.Clear();
            cboLoaiSP.SelectedIndex = 0;
            numDonGia.Value = 0;
            numSoLuong.Value = 0;
            txtMaSP.Enabled = true;
            errProv.Clear();
            txtMaSP.Focus();
        }

        private void LoadToForm(Product p)
        {
            txtMaSP.Text = p.MaSP;
            txtTenSP.Text = p.TenSP;
            cboLoaiSP.SelectedItem = p.LoaiSP;
            numDonGia.Value = p.DonGia;
            numSoLuong.Value = p.SoLuong;
            txtMaSP.Enabled = false;
            errProv.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!txtMaSP.Enabled)
            {
                MessageBox.Show("Đang ở chế độ sửa. Bấm Làm mới để thêm mới.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!ValidateInput(true)) return;

            _products.Add(new Product(txtMaSP.Text, txtTenSP.Text,
                cboLoaiSP.SelectedItem?.ToString() ?? "Điện tử",
                numDonGia.Value, (int)numSoLuong.Value));

            ClearInput();
            UpdateStatus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_bs.Current is not Product p)
            {
                MessageBox.Show("Chọn sản phẩm trong danh sách trước.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtMaSP.Text != p.MaSP)
            {
                LoadToForm(p);
                return;
            }

            if (!ValidateInput(false)) return;

            p.TenSP = txtTenSP.Text.Trim();
            p.LoaiSP = cboLoaiSP.SelectedItem?.ToString() ?? "Điện tử";
            p.DonGia = numDonGia.Value;
            p.SoLuong = (int)numSoLuong.Value;

            _bs.ResetCurrentItem();
            ClearInput();
            UpdateStatus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_bs.Current is not Product p)
            {
                MessageBox.Show("Chọn sản phẩm cần xóa trước.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Xóa sản phẩm [{p.MaSP}] – {p.TenSP}?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _products.Remove(p);
                ClearInput();
                UpdateStatus();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            _bs.DataSource = _products;
            txtSearch.Clear();
            ClearInput();
            UpdateStatus();
        }

        private void btnSearch_Click(object sender, EventArgs e) => DoSearch();
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { DoSearch(); e.Handled = true; e.SuppressKeyPress = true; }
        }

        private void DoSearch()
        {
            string kw = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(kw))
            {
                _bs.DataSource = _products;
                UpdateStatus();
                return;
            }
            var result = _products.Where(p =>
                p.MaSP.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0 ||
                p.TenSP.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            _bs.DataSource = new BindingList<Product>(result);
            lblCount.Text = $"Tìm thấy: {result.Count} sản phẩm";
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && _bs.Current is Product p)
                LoadToForm(p);
        }
    }
}
