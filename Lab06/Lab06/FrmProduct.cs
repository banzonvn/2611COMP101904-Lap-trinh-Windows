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
        // Sử dụng BindingList và BindingSource làm nguồn kết nối dữ liệu bộ nhớ
        private readonly BindingList<Product> _dataCollection = new BindingList<Product>();
        private readonly BindingSource _dataBinder = new BindingSource();

        private string _pendingImagePath = string.Empty;

        public FrmProduct()
        {
            InitializeComponent();
        }

        private void FrmProduct_Load(object sender, EventArgs e)
        {
            InitCategoryLookup();
            BindDataSources();
            LoadMockProducts();
            StylizeGridColumns();
            RefreshStatus("Hệ thống khởi chạy thành công.");
        }

        #region Helpers & Data Flow

        private void InitCategoryLookup()
        {
            cboLoaiSP.Items.Clear();
            cboLoaiSP.Items.AddRange(new object[] { "Điện tử", "Văn phòng", "Gia dụng", "Thời trang" });
            cboLoaiSP.SelectedIndex = 0;
        }

        private void BindDataSources()
        {
            _dataBinder.DataSource = _dataCollection;
            dgvProducts.DataSource = _dataBinder;
        }

        private void LoadMockProducts()
        {
            _dataCollection.Add(new Product("SP001", "Màn hình Dell UltraSharp", "Điện tử", 8450000m, 15, DateTime.Today.AddDays(-10), true));
            _dataCollection.Add(new Product("SP002", "Bút ký Parker IM", "Văn phòng", 450000m, 50, DateTime.Today.AddDays(-20), true));
            _dataCollection.Add(new Product("SP003", "Máy hút bụi Panasonic", "Gia dụng", 3200000m, 8, DateTime.Today.AddDays(-4), true));
            _dataCollection.Add(new Product("SP004", "Áo Polo Pima Cotton", "Thời trang", 380000m, 80, DateTime.Today.AddDays(-1), false));
        }

        private void StylizeGridColumns()
        {
            if (dgvProducts.Columns.Count == 0) return;

            // BẬT DÒNG NÀY ĐỂ BẢNG TỰ ĐỘNG GIÃN KÍN MÀN HÌNH (XÓA KHOẢNG TRẮNG THỪA)
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            var c = dgvProducts.Columns;
            if (c["MaSP"] != null) { c["MaSP"].HeaderText = "Mã SP"; c["MaSP"].FillWeight = 70; }
            if (c["TenSP"] != null) { c["TenSP"].HeaderText = "Tên sản phẩm"; c["TenSP"].FillWeight = 160; }
            if (c["LoaiSP"] != null) { c["LoaiSP"].HeaderText = "Loại hàng"; c["LoaiSP"].FillWeight = 90; }

            if (c["DonGia"] != null)
            {
                c["DonGia"].HeaderText = "Đơn giá (VNĐ)";
                c["DonGia"].DefaultCellStyle.Format = "N0";
                c["DonGia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                c["DonGia"].FillWeight = 100;
            }
            if (c["SoLuong"] != null)
            {
                c["SoLuong"].HeaderText = "Tồn kho";
                c["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                c["SoLuong"].FillWeight = 65;
            }
            if (c["NgayNhap"] != null)
            {
                c["NgayNhap"].HeaderText = "Ngày nhập";
                c["NgayNhap"].DefaultCellStyle.Format = "dd/MM/yyyy";
                c["NgayNhap"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                c["NgayNhap"].FillWeight = 85;
            }
            if (c["ConKinhDoanh"] != null)
            {
                c["ConKinhDoanh"].HeaderText = "Kinh doanh";
                c["ConKinhDoanh"].FillWeight = 80;
            }
            if (c["ThanhTien"] != null)
            {
                c["ThanhTien"].HeaderText = "Thành tiền (VNĐ)";
                c["ThanhTien"].DefaultCellStyle.Format = "N0";
                c["ThanhTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                c["ThanhTien"].FillWeight = 110;
            }

            if (c["DuongDanAnh"] != null) c["DuongDanAnh"].Visible = false;
        }

        private void RefreshStatus(string notification)
        {
            lblStatus.Text = notification;
            lblProductCount.Text = $"Tổng sản phẩm: {_dataCollection.Count}";

            decimal totalVal = _dataCollection.Sum(p => p.ThanhTien);
            lblInventoryValue.Text = $"Giá trị kho: {totalVal:N0} VND";
        }

        private void ResetInputFields()
        {
            txtMaSP.Clear();
            txtTenSP.Clear();
            cboLoaiSP.SelectedIndex = 0;
            numDonGia.Value = 0;
            numSoLuong.Value = 0;
            dtpNgayNhap.Value = DateTime.Today;
            chkConKinhDoanh.Checked = true;

            _pendingImagePath = string.Empty;
            picProduct.Image?.Dispose();
            picProduct.Image = null;

            txtMaSP.Enabled = true;
            errValidator.Clear();
            txtMaSP.Focus();
        }

        private void FillFormFromModel(Product product)
        {
            if (product == null) return;

            txtMaSP.Text = product.MaSP;
            txtTenSP.Text = product.TenSP;
            cboLoaiSP.SelectedItem = product.LoaiSP;
            numDonGia.Value = product.DonGia;
            numSoLuong.Value = product.SoLuong;
            dtpNgayNhap.Value = product.NgayNhap <= DateTime.Today ? product.NgayNhap : DateTime.Today;
            chkConKinhDoanh.Checked = product.ConKinhDoanh;

            _pendingImagePath = product.DuongDanAnh;
            LoadImageSafe(_pendingImagePath);

            txtMaSP.Enabled = false;
            errValidator.Clear();
        }

        private void LoadImageSafe(string path)
        {
            picProduct.Image?.Dispose();
            picProduct.Image = null;

            if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
            {
                try
                {
                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        picProduct.Image = Image.FromStream(fs);
                    }
                }
                catch
                {
                    picProduct.Image = null;
                }
            }
        }

        private bool ValidateFormInputs(bool isNewRecord)
        {
            errValidator.Clear();
            bool isClean = true;

            string id = txtMaSP.Text.Trim();
            if (string.IsNullOrWhiteSpace(id))
            {
                errValidator.SetError(txtMaSP, "Mã sản phẩm bắt buộc phải nhập.");
                isClean = false;
            }
            else if (isNewRecord && _dataCollection.Any(p => p.MaSP.Equals(id, StringComparison.OrdinalIgnoreCase)))
            {
                errValidator.SetError(txtMaSP, "Mã sản phẩm bị trùng lặp.");
                isClean = false;
            }

            if (string.IsNullOrWhiteSpace(txtTenSP.Text.Trim()))
            {
                errValidator.SetError(txtTenSP, "Tên sản phẩm không được bỏ trống.");
                isClean = false;
            }

            if (numDonGia.Value <= 0)
            {
                errValidator.SetError(numDonGia, "Đơn giá phải lớn hơn 0 VNĐ.");
                isClean = false;
            }

            if (numSoLuong.Value < 0)
            {
                errValidator.SetError(numSoLuong, "Số lượng không được âm.");
                isClean = false;
            }

            if (dtpNgayNhap.Value.Date > DateTime.Today)
            {
                errValidator.SetError(dtpNgayNhap, "Ngày nhập không được lớn hơn ngày hiện tại.");
                isClean = false;
            }

            return isClean;
        }

        #endregion

        #region Business Actions

        private void TriggerAddLogic(object sender, EventArgs e)
        {
            if (!txtMaSP.Enabled)
            {
                MessageBox.Show("Form đang ở chế độ chỉnh sửa. Bấm 'Làm mới' trước khi thêm.", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!ValidateFormInputs(isNewRecord: true)) return;

            var newProd = new Product(
                txtMaSP.Text,
                txtTenSP.Text,
                cboLoaiSP.SelectedItem?.ToString() ?? "Điện tử",
                numDonGia.Value,
                (int)numSoLuong.Value,
                dtpNgayNhap.Value,
                chkConKinhDoanh.Checked,
                _pendingImagePath
            );

            _dataCollection.Add(newProd);
            ResetInputFields();
            RefreshStatus($"Đã thêm mới sản phẩm: {newProd.TenSP}");
        }

        private void TriggerEditLogic(object sender, EventArgs e)
        {
            if (_dataBinder.Current is not Product current)
            {
                MessageBox.Show("Hãy chọn một sản phẩm từ danh sách cần sửa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtMaSP.Text != current.MaSP)
            {
                FillFormFromModel(current);
                RefreshStatus($"Đã chọn sản phẩm [{current.MaSP}]. Bấm 'Sửa' lần nữa để lưu thay đổi.");
                return;
            }

            if (!ValidateFormInputs(isNewRecord: false)) return;

            current.TenSP = txtTenSP.Text.Trim();
            current.LoaiSP = cboLoaiSP.SelectedItem?.ToString() ?? "Điện tử";
            current.DonGia = numDonGia.Value;
            current.SoLuong = (int)numSoLuong.Value;
            current.NgayNhap = dtpNgayNhap.Value;
            current.ConKinhDoanh = chkConKinhDoanh.Checked;
            current.DuongDanAnh = _pendingImagePath;

            _dataBinder.ResetCurrentItem();
            ResetInputFields();
            RefreshStatus($"Đã cập nhật thông tin sản phẩm: {current.MaSP}");
        }

        private void TriggerDeleteLogic(object sender, EventArgs e)
        {
            if (_dataBinder.Current is not Product target)
            {
                MessageBox.Show("Hãy chọn sản phẩm muốn xóa từ bảng danh sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dialog = MessageBox.Show(
                $"Xác nhận xóa sản phẩm [{target.MaSP}] - '{target.TenSP}'?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (dialog == DialogResult.Yes)
            {
                string id = target.MaSP;
                _dataCollection.Remove(target);
                ResetInputFields();
                RefreshStatus($"Đã xóa sản phẩm '{id}' khỏi hệ thống.");
            }
        }

        private void TriggerResetLogic(object sender, EventArgs e)
        {
            _dataBinder.DataSource = _dataCollection;
            txtSearchKeyword.Clear();
            ResetInputFields();
            RefreshStatus("Đã làm mới lại toàn bộ dữ liệu.");
        }

        private void TriggerSearchLogic(object sender, EventArgs e)
        {
            ExecuteQuery();
        }

        private void txtSearchKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ExecuteQuery();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void ExecuteQuery()
        {
            string keyword = txtSearchKeyword.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                _dataBinder.DataSource = _dataCollection;
                RefreshStatus("Hiển thị tất cả sản phẩm.");
                return;
            }

            var matches = _dataCollection.Where(p =>
                p.MaSP.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                p.TenSP.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0
            ).ToList();

            _dataBinder.DataSource = new BindingList<Product>(matches);
            RefreshStatus($"Tìm kiếm: Thấy {matches.Count} kết quả khớp từ khóa.");
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            if (dlgOpenFile.ShowDialog() == DialogResult.OK)
            {
                _pendingImagePath = dlgOpenFile.FileName;
                LoadImageSafe(_pendingImagePath);
                RefreshStatus("Đã chọn đường dẫn ảnh mới.");
            }
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && _dataBinder.Current is Product item)
            {
                FillFormFromModel(item);
                lblStatus.Text = $"Chi tiết mặt hàng: {item.MaSP}";
            }
        }

        private void dgvProducts_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dgvProducts.ClearSelection();
                dgvProducts.Rows[e.RowIndex].Selected = true;
                _dataBinder.Position = e.RowIndex;
            }
        }

        private void ctxViewDetail_Click(object sender, EventArgs e)
        {
            if (_dataBinder.Current is not Product p) return;

            string detail = $"BÁO CÁO CHI TIẾT SẢN PHẨM\n" +
                            $"====================================\n" +
                            $"• Mã số: {p.MaSP}\n" +
                            $"• Tên sản phẩm: {p.TenSP}\n" +
                            $"• Phân loại: {p.LoaiSP}\n" +
                            $"• Đơn giá: {p.DonGia:N0} VNĐ\n" +
                            $"• Số lượng tồn: {p.SoLuong}\n" +
                            $"• Tổng giá trị: {p.ThanhTien:N0} VNĐ\n" +
                            $"• Ngày nhập kho: {p.NgayNhap:dd/MM/yyyy}\n" +
                            $"• Kinh doanh: {(p.ConKinhDoanh ? "Đang kinh doanh" : "Ngừng kinh doanh")}\n" +
                            $"• Đường dẫn ảnh: {(string.IsNullOrEmpty(p.DuongDanAnh) ? "(Không có)" : p.DuongDanAnh)}";

            MessageBox.Show(detail, "Xem chi tiết", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void mnuFileExport_Click(object sender, EventArgs e)
        {
            if (dlgSaveFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("DANH SÁCH SẢN PHẨM KHO CỬA HÀNG");
                    sb.AppendLine($"Thời gian xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                    sb.AppendLine(new string('=', 65));
                    sb.AppendLine(string.Format("{0,-8} | {1,-25} | {2,-10} | {3,12} | {4,5}", "Mã SP", "Tên sản phẩm", "Loại hàng", "Đơn giá", "SL"));
                    sb.AppendLine(new string('-', 65));

                    foreach (var item in _dataCollection)
                    {
                        sb.AppendLine(string.Format("{0,-8} | {1,-25} | {2,-10} | {3,12:N0} | {4,5}",
                            item.MaSP,
                            item.TenSP.Length > 25 ? item.TenSP.Substring(0, 22) + "..." : item.TenSP,
                            item.LoaiSP,
                            item.DonGia,
                            item.SoLuong));
                    }

                    sb.AppendLine(new string('=', 65));
                    sb.AppendLine($"TỔNG MẶT HÀNG: {_dataCollection.Count}");
                    sb.AppendLine($"TỔNG GIÁ TRỊ TỒN KHO: {_dataCollection.Sum(x => x.ThanhTien):N0} VNĐ");

                    File.WriteAllText(dlgSaveFile.FileName, sb.ToString(), Encoding.UTF8);
                    RefreshStatus($"Xuất file thành công: {Path.GetFileName(dlgSaveFile.FileName)}");
                    MessageBox.Show("Đã xuất file văn bản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xuất file: {ex.Message}", "Cảnh báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mnuHelpAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chương trình Quản lý sản phẩm\nKhoa Công nghệ Thông tin - HCMUE", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion
    }
}