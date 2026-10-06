using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Lab05
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "ĐĂNG KÝ KHÓA HỌC";

            numSoThang.Minimum = 1;
            numSoThang.Maximum = 12;
            numSoThang.Value = 1;

            List<Course> listCourses = new List<Course>()
            {
                new Course("C# WinForms cơ bản", 800000),
                new Course("SQL Server cơ bản", 700000),
                new Course("Web Frontend cơ bản", 750000),
                new Course("Lập trình Python cơ bản", 650000)
            };

            cboKhoaHoc.DataSource = listCourses;
            cboKhoaHoc.DisplayMember = "ToString";

            if (cboKhoaHoc.Items.Count > 0)
            {
                cboKhoaHoc.SelectedIndex = 0;
            }
            radOnline.Checked = true;

            CapNhatTongTien();
        }

        private decimal TinhTongHocPhi()
        {
            if (cboKhoaHoc.SelectedItem is Course selectedCourse)
            {
                return selectedCourse.HocPhiThang * numSoThang.Value;
            }
            return 0;
        }

        private void CapNhatTongTien()
        {
            decimal tongTien = TinhTongHocPhi();
            lblTongTien.Text = $"{tongTien:N0} VND";
        }

        private void cboKhoaHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            CapNhatTongTien();
        }

        private void numSoThang_ValueChanged(object sender, EventArgs e)
        {
            CapNhatTongTien();
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            string hoTen = txtHoTen.Text.Trim();
            string soDienThoai = txtSoDienThoai.Text.Trim();

            if (string.IsNullOrWhiteSpace(hoTen))
            {
                MessageBox.Show("Vui lòng nhập họ tên học viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(soDienThoai))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return;
            }

            if (cboKhoaHoc.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn khóa học!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboKhoaHoc.Focus();
                return;
            }

            Course course = (Course)cboKhoaHoc.SelectedItem;
            string hinhThuc = radOnline.Checked ? "Online" : "Offline (Trực tiếp)";
            int soThang = (int)numSoThang.Value;
            decimal tongTien = TinhTongHocPhi();
            string emailStatus = chkNhanEmail.Checked ? "Có" : "Không";

            string thongTinPhieu = "===== PHIẾU ĐĂNG KÝ KHÓA HỌC =====\n\n" +
                                  $"- Họ và tên: {hoTen}\n" +
                                  $"- Số điện thoại: {soDienThoai}\n" +
                                  $"- Ngày sinh: {dtpNgaySinh.Value:dd/MM/yyyy}\n" +
                                  $"- Khóa học: {course.TenKhoaHoc}\n" +
                                  $"- Hình thức học: {hinhThuc}\n" +
                                  $"- Thời gian học: {soThang} tháng\n" +
                                  $"- Tổng học phí: {tongTien:N0} VND\n" +
                                  $"- Nhận email thông báo: {emailStatus}\n\n" +
                                  "Đăng ký thành công!";

            MessageBox.Show(thongTinPhieu, "Xác nhận đăng ký", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtHoTen.Clear();
            txtSoDienThoai.Clear();
            dtpNgaySinh.Value = DateTime.Now;
            chkNhanEmail.Checked = false;

            if (cboKhoaHoc.Items.Count > 0)
            {
                cboKhoaHoc.SelectedIndex = 0;
            }

            radOnline.Checked = true;
            numSoThang.Value = 1;
            CapNhatTongTien();
            txtHoTen.Focus();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn thoát chương trình không?",
                    "Xác nhận thoát",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}