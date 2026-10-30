using System.Drawing.Imaging;
using EduManager.GUI;

namespace EduManager
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();

            if (args.Length > 0 && args[0] == "--capture-all")
            {
                RunHeadlessCapture();
                return;
            }

            Application.Run(new FrmMonHoc());
        }

        private static void RunHeadlessCapture()
        {
            Console.WriteLine("=== CHẾ ĐỘ CHỤP ẢNH TỰ ĐỘNG LAB 14 (HEADLESS SCREENSHOT CAPTURE) ===");

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo? cur = new DirectoryInfo(baseDir);
            DirectoryInfo? outerLab14 = null;
            while (cur != null)
            {
                if (cur.Name == "Lab14" && cur.Parent != null && cur.Parent.Name.Contains("Lập trình window"))
                {
                    outerLab14 = cur;
                    break;
                }
                cur = cur.Parent;
            }

            string lab14Root = outerLab14?.FullName ?? @"C:\Users\Hoang Quan\Documents\Lập trình window Quan\Lab14";
            string imgDir1 = Path.Combine(lab14Root, "images");
            string imgDir2 = Path.Combine(lab14Root, "Lab14", "images");

            Directory.CreateDirectory(imgDir1);
            Directory.CreateDirectory(imgDir2);

            using var form = new FrmMonHoc();
            form.SuppressMessageBox = true;
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(50, 50);
            form.ClientSize = new Size(1160, 720);
            form.Show();
            Application.DoEvents();
            Thread.Sleep(300);

            void CaptureScreen(string filename, string description)
            {
                form.Refresh();
                Application.DoEvents();
                Thread.Sleep(200);

                using var bmp = new Bitmap(form.ClientSize.Width, form.ClientSize.Height);
                form.DrawToBitmap(bmp, new Rectangle(0, 0, form.ClientSize.Width, form.ClientSize.Height));

                string path1 = Path.Combine(imgDir1, filename);
                string path2 = Path.Combine(imgDir2, filename);

                bmp.Save(path1, ImageFormat.Png);
                bmp.Save(path2, ImageFormat.Png);
                Console.WriteLine($"[Captured] {filename} -> {description}");
            }

            // 1. Khởi chạy và hiển thị danh sách ban đầu
            form.LamMoiForm();
            CaptureScreen("01_khoi_chay_va_hien_thi_danh_sach.png", "Khởi chạy Form, nạp danh sách môn học từ CSDL qua EF Core");

            // 2. Thêm môn học mới thành công
            form.txtMaMonHoc.Text = "COMP1050";
            form.txtTenMonHoc.Text = "Trí tuệ nhân tạo nâng cao";
            form.nudSoTinChi.Value = 3;
            form.chkTrangThai.Checked = true;
            form.ThemMonHoc();
            CaptureScreen("02_them_mon_hoc_thanh_cong.png", "Thêm môn học COMP1050 mới thành công vào cơ sở dữ liệu");

            // 3. Validation: Báo lỗi khi Mã môn học để trống
            form.txtMaMonHoc.Text = "";
            form.txtTenMonHoc.Text = "Toán ứng dụng CNTT";
            form.nudSoTinChi.Value = 3;
            form.ThemMonHoc();
            CaptureScreen("03_validation_loi_ma_mon_de_trong.png", "Báo lỗi validation khi để trống Mã môn học");

            // 4. Validation: Báo lỗi khi Tên môn học để trống
            form.txtMaMonHoc.Text = "MATH2001";
            form.txtTenMonHoc.Text = "";
            form.nudSoTinChi.Value = 3;
            form.ThemMonHoc();
            CaptureScreen("04_validation_loi_ten_mon_de_trong.png", "Báo lỗi validation khi để trống Tên môn học");

            // 5. Validation: Báo lỗi khi trùng Mã môn học đã tồn tại
            form.txtMaMonHoc.Text = "COMP1019"; // Đã có
            form.txtTenMonHoc.Text = "Lập trình trên Windows Nâng cao";
            form.nudSoTinChi.Value = 3;
            form.ThemMonHoc();
            CaptureScreen("05_validation_loi_trung_ma_mon.png", "Báo lỗi validation khi thêm mã môn COMP1019 đã tồn tại");

            // 6. Chọn dòng trên DataGridView để hiển thị lên control nhập liệu
            form.txtTimKiem.Clear();
            form.LamMoiForm(resetAlert: true);
            form.txtMaMonHoc.Text = "COMP1019";
            form.txtTenMonHoc.Text = "Lập trình trên Windows";
            form.nudSoTinChi.Value = 3;
            form.chkTrangThai.Checked = true;
            if (form.dgvMonHoc.Rows.Count > 2)
            {
                form.dgvMonHoc.ClearSelection();
                form.dgvMonHoc.Rows[2].Selected = true;
            }
            CaptureScreen("06_chon_dong_hien_thi_len_input.png", "Chọn môn học COMP1019 hiển thị dữ liệu lên form nhập liệu");

            // 7. Sửa thông tin môn học thành công
            form.nudSoTinChi.Value = 4; // Đổi từ 3 lên 4 tín chỉ
            form.SuaMonHoc();
            CaptureScreen("07_sua_thong_tin_mon_hoc_thanh_cong.png", "Cập nhật số tín chỉ COMP1019 thành 4 tín chỉ thành công");

            // 8. Tìm kiếm môn học theo Mã môn
            form.txtTimKiem.Text = "COMP10";
            form.TimKiemMonHoc();
            CaptureScreen("08_tim_kiem_theo_ma_mon_hoc.png", "Tìm kiếm các môn học theo tiền tố mã COMP10");

            // 9. Tìm kiếm môn học theo Tên môn
            form.txtTimKiem.Text = "Dữ liệu";
            form.TimKiemMonHoc();
            CaptureScreen("09_tim_kiem_theo_ten_mon_hoc.png", "Tìm kiếm các môn học theo từ khóa tên 'Dữ liệu'");

            // 10. Tìm kiếm từ khóa không có kết quả
            form.txtTimKiem.Text = "XYZ999";
            form.TimKiemMonHoc();
            CaptureScreen("10_tim_kiem_khong_thay_ket_qua.png", "Hiển thị thông báo khi tìm kiếm từ khóa không tồn tại");

            // 11. Xóa môn học thành công
            form.txtTimKiem.Clear();
            form.LoadDanhSachMonHoc();
            form.txtMaMonHoc.Text = "COMP1050";
            form.XoaMonHoc();
            CaptureScreen("11_xoa_mon_hoc_thanh_cong.png", "Xóa môn học COMP1050 thành công khỏi cơ sở dữ liệu");

            // 12. Làm mới Form và load lại danh sách ban đầu
            form.LamMoiForm();
            CaptureScreen("12_lam_moi_form_va_load_lai_toan_bo.png", "Làm mới Form và nạp lại toàn bộ dữ liệu ban đầu");

            form.Close();
            Console.WriteLine("=== HOÀN TẤT CHỤP 12 ẢNH LAB 14 THÀNH CÔNG ===");
        }
    }
}
