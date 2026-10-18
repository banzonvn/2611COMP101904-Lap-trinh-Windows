using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Lab06
{
    public partial class FrmProduct : Form
    {
        // Quản lý dữ liệu qua BindingList & BindingSource theo chuẩn Slide 22 & 24
        private readonly BindingList<Product> _inventoryRegistry = new BindingList<Product>();
        private readonly BindingSource _bsBridge = new BindingSource();

        private string _stagedPhotoPath = string.Empty;

        public FrmProduct()
        {
            InitializeComponent();
        }

        private void FrmProduct_Load(object sender, EventArgs e)
        {
            PopulateCategories();
            InitializeDataPipe();
            InjectBootstrapSeed();
            TuneGridPresentation();
            UpdateStatusIndicator("Sẵn sàng quản trị danh mục sản phẩm.");
        }

        #region Setup & Helpers

        private void PopulateCategories()
        {
            cboLoaiSP.Items.Clear();
            cboLoaiSP.Items.AddRange(new object[] { "Điện tử", "Văn phòng", "Gia dụng", "Thời trang" });
            cboLoaiSP.SelectedIndex = 0;
        }

        private void InitializeDataPipe()
        {
            _bsBridge.DataSource = _inventoryRegistry;
            dgvProducts.DataSource = _bsBridge;
        }

        private void InjectBootstrapSeed()
        {
            _inventoryRegistry.Add(new Product("SP001", "Laptop Lenovo ThinkPad", "Điện tử", 21500000m, 10, DateTime.Today.AddDays(-12), true));
            _inventoryRegistry.Add(new Product("SP002", "Bút dạ quang Staedtler", "Văn phòng", 25000m, 120, DateTime.Today.AddDays(-25), true));
            _inventoryRegistry.Add(new Product("SP003", "Bình đun siêu tốc Tefal", "Gia dụng", 890000m, 35, DateTime.Today.AddDays(-3), true));
            _inventoryRegistry.Add(new Product("SP004", "Quần Jeans Casual Fit", "Thời trang", 550000m, 40, DateTime.Today.AddDays(-2), false));
        }

        private void TuneGridPresentation()
        {
            if (dgvProducts.Columns.Count == 0) return;

            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            var cols = dgvProducts.Columns;
            if (cols["MaSP"] != null) { cols["MaSP"].HeaderText = "Mã SP"; cols["MaSP"].FillWeight = 80; }
            if (cols["TenSP"] != null) { cols["TenSP"].HeaderText = "Tên sản phẩm"; cols["TenSP"].FillWeight = 160; }
            if (cols["LoaiSP"] != null) { cols["LoaiSP"].HeaderText = "Phân loại"; cols["LoaiSP"].FillWeight = 100; }

            if (cols["DonGia"] != null)
            {
                cols["DonGia"].HeaderText = "Đơn giá (VNĐ)";
                cols["DonGia"].DefaultCellStyle.Format = "N0";
                cols["DonGia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                cols["DonGia"].FillWeight = 110;
            }
            if (cols["SoLuong"] != null)
            {
                cols["SoLuong"].HeaderText = "Tồn kho";
                cols["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                cols["SoLuong"].FillWeight = 75;
            }
            if (cols["NgayNhap"] != null)
            {
                cols["NgayNhap"].HeaderText = "Ngày nhập";
                cols["NgayNhap"].DefaultCellStyle.Format = "dd/MM/yyyy";
                cols["NgayNhap"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                cols["NgayNhap"].FillWeight = 95;
            }
            if (cols["ConKinhDoanh"] != null)
            {
                cols["ConKinhDoanh"].HeaderText = "Kinh doanh";
                cols["ConKinhDoanh"].FillWeight = 85;
            }
            if (cols["ThanhTien"] != null)
            {
                cols["ThanhTien"].HeaderText = "Thành tiền (VNĐ)";
                cols["ThanhTien"].DefaultCellStyle.Format = "N0";
                cols["ThanhTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                cols["ThanhTien"].FillWeight = 120;
            }

            // Ẩn cột đường dẫn ảnh khỏi Grid
            if (cols["DuongDanAnh"] != null) cols["DuongDanAnh"].Visible = false;
        }

        private void UpdateStatusIndicator(string msg)
        {
            lblStatus.Text = msg;
            lblProductCount.Text = $"Tổng sản phẩm: {_inventoryRegistry.Count}";

            decimal totalNetWorth = _inventoryRegistry.Sum(i => i.ThanhTien);
            lblInventoryValue.Text = $"Giá trị kho: {totalNetWorth:N0} VND";
        }

        private void PurgeEditorFields()
        {
            txtMaSP.Clear();
            txtTenSP.Clear();
            cboLoaiSP.SelectedIndex = 0;
            numDonGia.Value = 0;
            numSoLuong.Value = 0;
            dtpNgayNhap.Value = DateTime.Today;
            chkConKinhDoanh.Checked = true;

            _stagedPhotoPath = string.Empty;
            picProduct.Image?.Dispose();
            picProduct.Image = null;

            txtMaSP.Enabled = true;
            errValidator.Clear();
            txtMaSP.Focus();
        }

        private void CommitDataToForm(Product item)
        {
            if (item == null) return;

            txtMaSP.Text = item.MaSP;
            txtTenSP.Text = item.TenSP;
            cboLoaiSP.SelectedItem = item.LoaiSP;
            numDonGia.Value = item.DonGia;
            numSoLuong.Value = item.SoLuong;
            dtpNgayNhap.Value = item.NgayNhap <= DateTime.Today ? item.NgayNhap : DateTime.Today;
            chkConKinhDoanh.Checked = item.ConKinhDoanh;

            _stagedPhotoPath = item.DuongDanAnh;
            RenderProductImage(_stagedPhotoPath);

            // Khóa mã sản phẩm khi đang xem/sửa
            txtMaSP.Enabled = false;
            errValidator.Clear();
        }

        private void RenderProductImage(string path)
        {
            picProduct.Image?.Dispose();
            picProduct.Image = null;

            if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
            {
                try
                {
                    // Dùng FileStream đọc byte tránh lock file ảnh trong Windows
                    using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        picProduct.Image = Image.FromStream(stream);
                    }
                }
                catch
                {
                    picProduct.Image = null;
                }
            }
        }

        private bool ValidateBusinessRules(bool verifyUniqueId)
        {
            errValidator.Clear();
            bool pass = true;

            string id = txtMaSP.Text.Trim();
            if (string.IsNullOrWhiteSpace(id))
            {
                errValidator.SetError(txtMaSP, "Mã sản phẩm không được rỗng.");
                pass = false;
            }
            else if (verifyUniqueId && _inventoryRegistry.Any(x => x.MaSP.Equals(id, StringComparison.OrdinalIgnoreCase)))
            {
                errValidator.SetError(txtMaSP, "Mã sản phẩm này đã tồn tại trong hệ thống.");
                pass = false;
            }

            if (string.IsNullOrWhiteSpace(txtTenSP.Text.Trim()))
            {
                errValidator.SetError(txtTenSP, "Tên sản phẩm không được rỗng.");
                pass = false;
            }

            if (numDonGia.Value <= 0)
            {
                errValidator.SetError(numDonGia, "Đơn giá phải lớn hơn 0 VNĐ.");
                pass = false;
            }

            if (numSoLuong.Value < 0)
            {
                errValidator.SetError(numSoLuong, "Số lượng không được âm.");
                pass = false;
            }

            if (dtpNgayNhap.Value.Date > DateTime.Today)
            {
                errValidator.SetError(dtpNgayNhap, "Ngày nhập không được lớn hơn ngày hiện tại.");
                pass = false;
            }

            return pass;
        }

        #endregion

        #region Actions Handlers (Menu, Toolbar, ContextMenu dùng chung)

        private void ExecuteAddAction(object sender, EventArgs e)
        {
            if (!txtMaSP.Enabled)
            {
                MessageBox.Show("Bạn đang ở chế độ chỉnh sửa. Bấm 'Làm mới' để thêm sản phẩm mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!ValidateBusinessRules(verifyUniqueId: true)) return;

            var newEntity = new Product(
                txtMaSP.Text,
                txtTenSP.Text,
                cboLoaiSP.SelectedItem?.ToString() ?? "Điện tử",
                numDonGia.Value,
                (int)numSoLuong.Value,
                dtpNgayNhap.Value,
                chkConKinhDoanh.Checked,
                _stagedPhotoPath
            );

            _inventoryRegistry.Add(newEntity);
            PurgeEditorFields();
            UpdateStatusIndicator($"Đã thêm mới thành công sản phẩm: {newEntity.TenSP}");
        }

        private void ExecuteEditAction(object sender, EventArgs e)
        {
            if (_bsBridge.Current is not Product activeItem)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm từ danh sách trước khi sửa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Nếu người dùng bấm sửa mà chưa đẩy thông tin lên form
            if (txtMaSP.Text != activeItem.MaSP)
            {
                CommitDataToForm(activeItem);
                UpdateStatusIndicator($"Đã nạp sản phẩm '{activeItem.MaSP}' lên form. Chỉnh sửa và bấm 'Sửa' lần nữa để lưu.");
                return;
            }

            if (!ValidateBusinessRules(verifyUniqueId: false)) return;

            activeItem.TenSP = txtTenSP.Text.Trim();
            activeItem.LoaiSP = cboLoaiSP.SelectedItem?.ToString() ?? "Điện tử";
            activeItem.DonGia = numDonGia.Value;
            activeItem.SoLuong = (int)numSoLuong.Value;
            activeItem.NgayNhap = dtpNgayNhap.Value;
            activeItem.ConKinhDoanh = chkConKinhDoanh.Checked;
            activeItem.DuongDanAnh = _stagedPhotoPath;

            _bsBridge.ResetCurrentItem();
            PurgeEditorFields();
            UpdateStatusIndicator($"Cập nhật thành công sản phẩm: {activeItem.MaSP}");
        }

        private void ExecuteDeleteAction(object sender, EventArgs e)
        {
            if (_bsBridge.Current is not Product target)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa trên danh sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa sản phẩm [{target.MaSP}] - '{target.TenSP}' khỏi bộ nhớ?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.Yes)
            {
                string oldCode = target.MaSP;
                _inventoryRegistry.Remove(target);
                PurgeEditorFields();
                UpdateStatusIndicator($"Đã xóa thành công sản phẩm có mã: {oldCode}");
            }
        }

        private void ExecuteRefreshAction(object sender, EventArgs e)
        {
            _bsBridge.DataSource = _inventoryRegistry;
            txtSearchKeyword.Clear();
            PurgeEditorFields();
            UpdateStatusIndicator("Đã làm mới dữ liệu và form nhập liệu.");
        }

        private void ExecuteSearchAction(object sender, EventArgs e)
        {
            PerformLookup();
        }

        private void txtSearchKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PerformLookup();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void PerformLookup()
        {
            string token = txtSearchKeyword.Text.Trim();

            if (string.IsNullOrEmpty(token))
            {
                _bsBridge.DataSource = _inventoryRegistry;
                UpdateStatusIndicator("Hiển thị toàn bộ danh mục sản phẩm.");
                return;
            }

            var queryMatches = _inventoryRegistry.Where(x =>
                x.MaSP.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0 ||
                x.TenSP.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0
            ).ToList();

            _bsBridge.DataSource = new BindingList<Product>(queryMatches);
            UpdateStatusIndicator($"Kết quả tìm kiếm: Tìm thấy {queryMatches.Count} sản phẩm phù hợp.");
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            if (dlgOpenFile.ShowDialog() == DialogResult.OK)
            {
                _stagedPhotoPath = dlgOpenFile.FileName;
                RenderProductImage(_stagedPhotoPath);
                UpdateStatusIndicator("Đã nạp đường dẫn ảnh minh họa.");
            }
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && _bsBridge.Current is Product item)
            {
                CommitDataToForm(item);
                lblStatus.Text = $"Đang xem chi tiết sản phẩm: {item.MaSP}";
            }
        }

        private void dgvProducts_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dgvProducts.ClearSelection();
                dgvProducts.Rows[e.RowIndex].Selected = true;
                _bsBridge.Position = e.RowIndex;
            }
        }

        private void ctxViewDetail_Click(object sender, EventArgs e)
        {
            if (_bsBridge.Current is not Product p) return;

            string info = $"THÔNG TIN CHI TIẾT SẢN PHẨM\n" +
                          $"----------------------------------------\n" +
                          $"• Mã sản phẩm: {p.MaSP}\n" +
                          $"• Tên sản phẩm: {p.TenSP}\n" +
                          $"• Phân loại: {p.LoaiSP}\n" +
                          $"• Đơn giá: {p.DonGia:N0} VNĐ\n" +
                          $"• Số lượng tồn: {p.SoLuong}\n" +
                          $"• Tổng thành tiền: {p.ThanhTien:N0} VNĐ\n" +
                          $"• Ngày nhập kho: {p.NgayNhap:dd/MM/yyyy}\n" +
                          $"• Trạng thái kinh doanh: {(p.ConKinhDoanh ? "Đang kinh doanh" : "Ngừng kinh doanh")}\n" +
                          $"• File ảnh: {(string.IsNullOrEmpty(p.DuongDanAnh) ? "(Chưa chọn)" : p.DuongDanAnh)}";

            MessageBox.Show(info, "Chi tiết mặt hàng", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void mnuFileExport_Click(object sender, EventArgs e)
        {
            if (dlgSaveFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("DANH SÁCH BÁO CÁO SẢN PHẨM KHO");
                    sb.AppendLine($"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                    sb.AppendLine(new string('=', 65));
                    sb.AppendLine(string.Format("{0,-8} | {1,-25} | {2,-10} | {3,12} | {4,5}", "Mã SP", "Tên sản phẩm", "Loại", "Đơn giá", "SL"));
                    sb.AppendLine(new string('-', 65));

                    foreach (var item in _inventoryRegistry)
                    {
                        sb.AppendLine(string.Format("{0,-8} | {1,-25} | {2,-10} | {3,12:N0} | {4,5}",
                            item.MaSP,
                            item.TenSP.Length > 25 ? item.TenSP.Substring(0, 22) + "..." : item.TenSP,
                            item.LoaiSP,
                            item.DonGia,
                            item.SoLuong));
                    }

                    sb.AppendLine(new string('=', 65));
                    sb.AppendLine($"TỔNG SỐ LƯỢNG MẶT HÀNG: {_inventoryRegistry.Count}");
                    sb.AppendLine($"TỔNG GIÁ TRỊ TỒN KHO: {_inventoryRegistry.Sum(x => x.ThanhTien):N0} VNĐ");

                    File.WriteAllText(dlgSaveFile.FileName, sb.ToString(), Encoding.UTF8);
                    UpdateStatusIndicator($"Đã xuất file thành công: {Path.GetFileName(dlgSaveFile.FileName)}");
                    MessageBox.Show("Xuất file báo cáo thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mnuHelpAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Phần mềm Quản lý sản phẩm\nKhoa Công nghệ Thông tin - HCMUE", "Giới thiệu", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion
    }
}