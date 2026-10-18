using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Lab07.Models;

namespace Lab07
{
    public partial class FrmCustomer : Form
    {
        private readonly BindingList<Customer> _customers = new BindingList<Customer>();
        private readonly BindingSource _bs = new BindingSource();

        public FrmCustomer()
        {
            InitializeComponent();
        }

        private void FrmCustomer_Load(object sender, EventArgs e)
        {
            cboLoaiKH.Items.AddRange(new object[] { "Thường", "Thân thiết", "VIP" });
            cboLoaiKH.SelectedIndex = 0;

            _bs.DataSource = _customers;
            dgvCustomers.DataSource = _bs;

            // Seed data
            _customers.Add(new Customer("KH001", "Nguyễn Văn An", "0901234567", "Thân thiết"));
            _customers.Add(new Customer("KH002", "Trần Thị Bình", "0912345678", "VIP"));
            _customers.Add(new Customer("KH003", "Lê Minh Châu", "0923456789", "Thường"));

            SetupGrid();
            UpdateStatus();
        }

        private void SetupGrid()
        {
            dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCustomers.ReadOnly = true;
            dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCustomers.MultiSelect = false;

            var cols = dgvCustomers.Columns;
            if (cols["MaKH"] != null) { cols["MaKH"].HeaderText = "Mã KH"; cols["MaKH"].FillWeight = 80; }
            if (cols["HoTen"] != null) { cols["HoTen"].HeaderText = "Họ và tên"; cols["HoTen"].FillWeight = 200; }
            if (cols["SoDienThoai"] != null) { cols["SoDienThoai"].HeaderText = "Số điện thoại"; cols["SoDienThoai"].FillWeight = 130; }
            if (cols["LoaiKH"] != null) { cols["LoaiKH"].HeaderText = "Loại KH"; cols["LoaiKH"].FillWeight = 100; }
        }

        private void UpdateStatus()
        {
            lblCount.Text = $"Tổng: {_customers.Count} khách hàng";
        }

        private bool ValidateInput(bool checkDuplicate)
        {
            errProv.Clear();
            bool ok = true;

            if (string.IsNullOrWhiteSpace(txtMaKH.Text))
            {
                errProv.SetError(txtMaKH, "Mã khách hàng không được rỗng.");
                ok = false;
            }
            else if (checkDuplicate && _customers.Any(c => c.MaKH.Equals(txtMaKH.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                errProv.SetError(txtMaKH, "Mã khách hàng đã tồn tại.");
                ok = false;
            }

            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                errProv.SetError(txtHoTen, "Họ tên không được rỗng.");
                ok = false;
            }

            if (string.IsNullOrWhiteSpace(txtSoDT.Text))
            {
                errProv.SetError(txtSoDT, "Số điện thoại không được rỗng.");
                ok = false;
            }

            return ok;
        }

        private void ClearInput()
        {
            txtMaKH.Clear();
            txtHoTen.Clear();
            txtSoDT.Clear();
            cboLoaiKH.SelectedIndex = 0;
            txtMaKH.Enabled = true;
            errProv.Clear();
            txtMaKH.Focus();
        }

        private void LoadToForm(Customer c)
        {
            txtMaKH.Text = c.MaKH;
            txtHoTen.Text = c.HoTen;
            txtSoDT.Text = c.SoDienThoai;
            cboLoaiKH.SelectedItem = c.LoaiKH;
            txtMaKH.Enabled = false;
            errProv.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!txtMaKH.Enabled)
            {
                MessageBox.Show("Đang ở chế độ sửa. Bấm Làm mới để thêm mới.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!ValidateInput(true)) return;

            _customers.Add(new Customer(txtMaKH.Text, txtHoTen.Text,
                txtSoDT.Text, cboLoaiKH.SelectedItem?.ToString() ?? "Thường"));
            ClearInput();
            UpdateStatus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_bs.Current is not Customer c)
            {
                MessageBox.Show("Chọn khách hàng trong danh sách trước.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtMaKH.Text != c.MaKH)
            {
                LoadToForm(c);
                return;
            }

            if (!ValidateInput(false)) return;

            c.HoTen = txtHoTen.Text.Trim();
            c.SoDienThoai = txtSoDT.Text.Trim();
            c.LoaiKH = cboLoaiKH.SelectedItem?.ToString() ?? "Thường";

            _bs.ResetCurrentItem();
            ClearInput();
            UpdateStatus();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            _bs.DataSource = _customers;
            ClearInput();
            UpdateStatus();
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && _bs.Current is Customer c)
                LoadToForm(c);
        }
    }
}
