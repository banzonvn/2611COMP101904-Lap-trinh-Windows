using System.Drawing.Imaging;
using StudentReportDemo.Forms;
using StudentReportDemo.Services;

namespace StudentReportDemo
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

            Application.Run(new FrmBaoCaoSinhVien());
        }

        private static void RunHeadlessCapture()
        {
            Console.WriteLine("=== CHẾ ĐỘ CHỤP ẢNH TỰ ĐỘNG LAB 13 (HEADLESS SCREENSHOT CAPTURE) ===");

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            // Tìm thư mục Lab13 gốc (nằm ngay dưới 'Lập trình window Quan')
            DirectoryInfo? cur = new DirectoryInfo(baseDir);
            DirectoryInfo? outerLab13 = null;
            while (cur != null)
            {
                if (cur.Name == "Lab13" && cur.Parent != null && cur.Parent.Name.Contains("Lập trình window"))
                {
                    outerLab13 = cur;
                    break;
                }
                cur = cur.Parent;
            }

            string lab13Root = outerLab13?.FullName ?? @"C:\Users\Hoang Quan\Documents\Lập trình window Quan\Lab13";
            string imgDir1 = Path.Combine(lab13Root, "images");
            string imgDir2 = Path.Combine(lab13Root, "Lab13", "images");
            string repDir1 = Path.Combine(lab13Root, "reports");
            string repDir2 = Path.Combine(lab13Root, "Lab13", "reports");

            Directory.CreateDirectory(imgDir1);
            Directory.CreateDirectory(imgDir2);
            Directory.CreateDirectory(repDir1);
            Directory.CreateDirectory(repDir2);

            using var form = new FrmBaoCaoSinhVien();
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

            // 1. Khởi chạy và hiển thị tất cả
            form.cboLop.SelectedValue = "ALL";
            form.cboKetQua.SelectedIndex = 0; // Tất cả
            form.txtTuKhoa.Clear();
            form.XemBaoCao();
            CaptureScreen("01_khoi_chay_va_hien_thi_tat_ca.png", "Khởi chạy và nạp toàn bộ danh sách sinh viên các lớp");

            // 2. Lọc theo lớp CTK47A (Có Lê Hoàng Quân)
            form.cboLop.SelectedValue = "CTK47A";
            form.cboKetQua.SelectedIndex = 0;
            form.txtTuKhoa.Clear();
            form.XemBaoCao();
            CaptureScreen("02_loc_theo_lop_ctk47a.png", "Lọc sinh viên lớp CNTT K47A (Có sinh viên Lê Hoàng Quân)");

            // 3. Lọc theo lớp CTK47B
            form.cboLop.SelectedValue = "CTK47B";
            form.cboKetQua.SelectedIndex = 0;
            form.txtTuKhoa.Clear();
            form.XemBaoCao();
            CaptureScreen("03_loc_theo_lop_ctk47b.png", "Lọc sinh viên lớp CNTT K47B");

            // 4. Lọc điều kiện sinh viên ĐẠT (Điểm >= 5.0)
            form.cboLop.SelectedValue = "ALL";
            form.cboKetQua.SelectedIndex = 1; // Đạt
            form.txtTuKhoa.Clear();
            form.XemBaoCao();
            CaptureScreen("04_loc_dieu_kien_dat.png", "Lọc danh sách sinh viên có kết quả ĐẠT (>= 5.0)");

            // 5. Lọc điều kiện sinh viên KHÔNG ĐẠT (Điểm < 5.0)
            form.cboLop.SelectedValue = "ALL";
            form.cboKetQua.SelectedIndex = 2; // Không đạt
            form.txtTuKhoa.Clear();
            form.XemBaoCao();
            CaptureScreen("05_loc_dieu_kien_khong_dat.png", "Lọc danh sách sinh viên có kết quả KHÔNG ĐẠT (< 5.0)");

            // 6. Tìm kiếm theo họ tên và mã sinh viên (Ví dụ "Quân")
            form.cboLop.SelectedValue = "ALL";
            form.cboKetQua.SelectedIndex = 0;
            form.txtTuKhoa.Text = "Quân";
            form.XemBaoCao();
            CaptureScreen("06_tim_kiem_theo_ho_ten_va_masv.png", "Tìm kiếm tức thời theo họ tên sinh viên");

            // 7. Thống kê theo từng lớp bằng LINQ GroupBy
            form.ThongKeTheoLop();
            CaptureScreen("07_thong_ke_theo_tung_lop_group_by.png", "Bảng thống kê số lượng, tỉ lệ đạt và điểm TB theo từng lớp bằng GroupBy");

            // 8. Báo lỗi / thông báo khi không có dữ liệu phù hợp
            form.cboLop.SelectedValue = "CTK47A";
            form.cboKetQua.SelectedIndex = 0;
            form.txtTuKhoa.Text = "KhangXYZ999";
            form.XemBaoCao();
            CaptureScreen("08_thong_bao_khi_khong_co_du_lieu_phu_hop.png", "Hiển thị thông báo khi không có dữ liệu phù hợp điều kiện lọc");

            // 9. Xuất báo cáo CSV thành công
            form.cboLop.SelectedValue = "CTK47A";
            form.cboKetQua.SelectedIndex = 0;
            form.txtTuKhoa.Clear();
            form.XemBaoCao();
            string csvPath1 = Path.Combine(repDir1, "BaoCao_CTK47A.csv");
            string csvPath2 = Path.Combine(repDir2, "BaoCao_CTK47A.csv");
            form.XuatFile(csvPath1);
            File.Copy(csvPath1, csvPath2, true);
            CaptureScreen("09_xuat_bao_cao_csv_thanh_cong.png", "Xuất báo cáo danh sách sinh viên ra tệp tin CSV thành công");

            // 10. Xuất báo cáo TXT có định dạng bảng và chữ ký
            string txtPath1 = Path.Combine(repDir1, "BaoCao_CTK47A.txt");
            string txtPath2 = Path.Combine(repDir2, "BaoCao_CTK47A.txt");
            form.XuatFile(txtPath1);
            File.Copy(txtPath1, txtPath2, true);
            CaptureScreen("10_xuat_bao_cao_txt_thanh_cong.png", "Xuất báo cáo định dạng bảng TXT có tiêu đề và chữ ký xác nhận");

            // Xuất thêm file thống kê lớp để lưu trong thư mục reports
            form.ThongKeTheoLop();
            string csvTk1 = Path.Combine(repDir1, "ThongKe_CacLop.csv");
            string csvTk2 = Path.Combine(repDir2, "ThongKe_CacLop.csv");
            form.XuatFile(csvTk1);
            File.Copy(csvTk1, csvTk2, true);

            // 11. Thẻ chỉ số KPI cập nhật linh hoạt
            form.cboLop.SelectedValue = "CTK48A";
            form.cboKetQua.SelectedIndex = 0;
            form.txtTuKhoa.Clear();
            form.XemBaoCao();
            CaptureScreen("11_the_chi_so_kpi_cap_nhat_linh_hoat.png", "Các thẻ chỉ số KPI tổng số, tỉ lệ đạt, điểm TB cập nhật theo bộ lọc");

            // 12. Làm mới khôi phục bộ lọc ban đầu
            form.cboLop.SelectedValue = "ALL";
            form.cboKetQua.SelectedIndex = 0;
            form.txtTuKhoa.Clear();
            form.XemBaoCao();
            CaptureScreen("12_lam_moi_khoi_phuc_bo_loc.png", "Làm mới và khôi phục toàn bộ tiêu chí lọc về trạng thái ban đầu");

            form.Close();
            Console.WriteLine("=== HOÀN TẤT CHỤP 12 ẢNH VÀ XUẤT CÁC TỆP BÁO CÁO MẪU ===");
        }
    }
}
