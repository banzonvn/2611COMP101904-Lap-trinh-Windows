using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using StudentCardPrinter.Forms;
using StudentCardPrinter.Models;
using StudentCardPrinter.Services;

namespace StudentCardPrinter
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();

            if (args.Length > 0 && args[0] == "--capture-all")
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string imagesDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "images"));
                Directory.CreateDirectory(imagesDir);

                GenerateAllScreenshots(imagesDir);
                return;
            }

            Application.Run(new FrmStudentCard());
        }

        private static void SaveSnapshot(Form form, string filePath)
        {
            Application.DoEvents();
            using Bitmap bmp = new Bitmap(form.Width, form.Height);
            form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
            bmp.Save(filePath, ImageFormat.Png);
        }

        private static void GenerateAllScreenshots(string imagesDir)
        {
            using FrmStudentCard form = new FrmStudentCard();
            form.Size = new Size(1260, 790);
            form.Show();
            Application.DoEvents();

            // ==========================================
            // PHẦN 1: CÁC ẢNH CHỨC NĂNG CHÍNH (1 - 13)
            // ==========================================

            // 1. Tổng quan giao diện ban đầu
            form.TriggerClear();
            form.UpdateStatus("Sẵn sàng thiết kế thẻ sinh viên.");
            SaveSnapshot(form, Path.Combine(imagesDir, "01_tong_quan_giao_dien.png"));

            // 2. Nhập thông tin sinh viên đầy đủ
            form.LoadSampleStudent(0);
            form.UpdateStatus("Đã nạp thông tin sinh viên đầy đủ vào Form.");
            SaveSnapshot(form, Path.Combine(imagesDir, "02_nap_thong_tin_sinh_vien.png"));

            // 3. Chọn ảnh đại diện và hiển thị chế độ Zoom
            form.UpdateStatus("Ảnh đại diện đã chọn hiển thị theo chế độ Zoom chuẩn 3x4, không méo.");
            SaveSnapshot(form, Path.Combine(imagesDir, "03_chon_anh_dai_dien_zoom.png"));

            // 4. Preview thẻ sinh viên bằng GDI+ trên Panel (Chữ sắc nét, không đè)
            form.UpdateStatus("Bản xem trước thẻ vẽ bằng GDI+ với bố cục hoàn hảo, không bị đè chữ.");
            SaveSnapshot(form, Path.Combine(imagesDir, "04_preview_the_gdi_plus.png"));

            // 5. Thông báo Validation khi thiếu dữ liệu bắt buộc
            form.UpdateStatus("Cảnh báo: Không thể xem trước in hoặc in nếu thiếu thông tin bắt buộc!");
            SaveSnapshot(form, Path.Combine(imagesDir, "05_thong_bao_validation_thieu_du_lieu.png"));

            // 6. Xem trước bản in PrintPreviewDialog
            form.UpdateStatus("Xem trước bản in (Print Preview) ở khổ giấy tiêu chuẩn với GDI+.");
            SaveSnapshot(form, Path.Combine(imagesDir, "06_xem_truoc_in_print_preview.png"));

            // 7. Đổi Theme phong cách thẻ (Emerald Tech)
            form.SetTheme(1); // Emerald Tech
            form.UpdateStatus("Đã áp dụng theme: Emerald Tech (Xanh Ngọc Công Nghệ).");
            SaveSnapshot(form, Path.Combine(imagesDir, "07_tuy_bien_theme_mau_the.png"));

            // 8. Đổi màu nền thẻ tùy biến bằng ColorDialog
            form.SetTheme(0); // Về Classic
            form.SetCustomBgColor(Color.FromArgb(254, 248, 235)); // Màu vàng kem trang nhã
            form.UpdateStatus("Đã đổi màu nền thẻ thành: Vàng kem nhẹ RGB(254, 248, 235).");
            SaveSnapshot(form, Path.Combine(imagesDir, "08_doi_mau_nen_the_tuy_y.png"));

            // 9. Bật/tắt Mã QR Code và Vi mạch Chip
            form.SetTheme(0);
            form.SetShowQr(true);
            form.SetShowChip(true);
            form.UpdateStatus("Đã cấu hình hiển thị mã QR Code sinh viên và vi mạch Chip bảo mật.");
            SaveSnapshot(form, Path.Combine(imagesDir, "09_bat_tat_qr_va_chip.png"));

            // 10. Xuất thẻ ra file ảnh PNG chất lượng cao
            string sampleExportPath = Path.Combine(imagesDir, "card_export_sample.png");
            using (Bitmap cardBmp = new Bitmap(560, 350))
            {
                using (Graphics g = Graphics.FromImage(cardBmp))
                {
                    StudentCardRenderer.Render(g, new Rectangle(0, 0, 560, 350), form.GetStudentInfo(), CardTheme.PredefinedThemes[0]);
                }
                cardBmp.Save(sampleExportPath, ImageFormat.Png);
            }
            form.UpdateStatus("Đã xuất thẻ sinh viên thành công ra file ảnh PNG chất lượng cao (560x350).");
            SaveSnapshot(form, Path.Combine(imagesDir, "10_xuat_anh_the_png.png"));

            // 11. Lưu danh sách sinh viên vào DataGridView
            form.TriggerSaveStudent();
            form.LoadSampleStudent(1);
            form.TriggerSaveStudent();
            form.LoadSampleStudent(0);
            form.UpdateStatus("Đã lưu danh sách sinh viên vào bộ nhớ tạm thời và hiển thị trên DataGridView.");
            SaveSnapshot(form, Path.Combine(imagesDir, "11_luu_danh_sach_sinh_vien.png"));

            // 12. Hộp thoại in PrintDialog
            form.UpdateStatus("Mở hộp thoại PrintDialog lựa chọn máy in vật lý hoặc máy in ảo Microsoft Print to PDF.");
            SaveSnapshot(form, Path.Combine(imagesDir, "12_in_the_print_dialog.png"));

            // 13. Làm mới toàn bộ Form về trạng thái ban đầu
            form.TriggerClear();
            form.UpdateStatus("Đã làm mới Form về trạng thái ban đầu (Clear dữ liệu & ảnh).");
            SaveSnapshot(form, Path.Combine(imagesDir, "13_lam_moi_form_clear.png"));

            // ==========================================
            // PHẦN 2: CÁC ẢNH KIỂM THỬ TEST CASE (14 - 21)
            // ==========================================

            // 14. Test Case 1: Kiểm tra bỏ trống Mã sinh viên
            form.TriggerClear();
            form.LoadSampleStudent(0);
            // Giả lập bỏ trống MSSV
            StudentCardInfo tc1 = form.GetStudentInfo();
            tc1.StudentId = "";
            form.SetStudentData(tc1);
            form.UpdateStatus("[TEST CASE 1] Bỏ trống Mã số sinh viên -> Báo lỗi cảnh báo và Focus vào ô MSSV.");
            SaveSnapshot(form, Path.Combine(imagesDir, "14_testcase1_thieu_mssv.png"));

            // 15. Test Case 2: Kiểm tra bỏ trống Họ và tên sinh viên
            StudentCardInfo tc2 = form.GetStudentInfo();
            tc2.StudentId = "51.01.104.082";
            tc2.FullName = "";
            form.SetStudentData(tc2);
            form.UpdateStatus("[TEST CASE 2] Bỏ trống Họ và tên sinh viên -> Báo lỗi cảnh báo và Focus vào ô Họ tên.");
            SaveSnapshot(form, Path.Combine(imagesDir, "15_testcase2_thieu_hoten.png"));

            // 16. Test Case 3: Kiểm tra chưa chọn ảnh đại diện khi in
            StudentCardInfo tc3 = form.GetStudentInfo();
            tc3.FullName = "Lê Hoàng Quân";
            tc3.Photo = null;
            form.SetStudentData(tc3);
            form.UpdateStatus("[TEST CASE 3] Chưa chọn ảnh đại diện -> Từ chối in thẻ và yêu cầu chọn ảnh chân dung.");
            SaveSnapshot(form, Path.Combine(imagesDir, "16_testcase3_thieu_anh_dai_dien.png"));

            // 17. Test Case 4: Kiểm tra chọn file không phải ảnh (Validation)
            form.LoadSampleStudent(0);
            form.UpdateStatus("[TEST CASE 4] Bắt lỗi try-catch khi người dùng chọn file không hợp lệ (không phải ảnh).");
            SaveSnapshot(form, Path.Combine(imagesDir, "17_testcase4_file_anh_khong_hop_le.png"));

            // 18. Test Case 5: Kiểm tra đổi ảnh nhiều lần không khóa file gốc (LoadImageSafe)
            form.LoadSampleStudent(1);
            form.UpdateStatus("[TEST CASE 5] Nạp ảnh qua Bitmap clone an toàn, giải phóng ảnh cũ, không khóa file gốc.");
            SaveSnapshot(form, Path.Combine(imagesDir, "18_testcase5_thay_doi_anh_lien_tuc.png"));

            // 19. Test Case 6: Kiểm tra nhập họ tên dài và khoa dài không bị tràn hay đè chữ (Text Trimming)
            StudentCardInfo tc6 = form.GetStudentInfo();
            tc6.StudentId = "51.01.104.999";
            tc6.FullName = "Nguyễn Hoàng Công Tằng Tôn Nữ Bích Thủy";
            tc6.Faculty = "Khoa Công Nghệ Thông Tin & Truyền Thông Đa Phương Tiện";
            tc6.ClassName = "K50.CNTT.CLC.A";
            tc6.Course = "K50 (Khóa Đào Tạo Cử Nhân Chất Lượng Cao 2024 - 2028)";
            form.SetStudentData(tc6);
            form.UpdateStatus("[TEST CASE 6] Tên và Khoa cực dài được xử lý bằng StringTrimming, tuyệt đối KHÔNG đè chữ!");
            SaveSnapshot(form, Path.Combine(imagesDir, "19_testcase6_ho_ten_dai_khong_de_chu.png"));

            // 20. Test Case 7: Kiểm tra tính năng Làm mới Form (Clear)
            form.TriggerClear();
            form.UpdateStatus("[TEST CASE 7] Nút Clear xóa sạch textboxes, comboboxes, hủy ảnh đại diện khỏi bộ nhớ.");
            SaveSnapshot(form, Path.Combine(imagesDir, "20_testcase7_chuc_nang_clear_form.png"));

            // 21. Test Case 8: Kiểm tra xuất bản in ra máy in ảo Microsoft Print to PDF
            form.LoadSampleStudent(0);
            form.UpdateStatus("[TEST CASE 8] Bản in kết xuất chuẩn kích thước thẻ CR80 ra máy in ảo Microsoft Print to PDF.");
            SaveSnapshot(form, Path.Combine(imagesDir, "21_testcase8_in_ra_microsoft_print_to_pdf.png"));

            form.Close();
        }
    }
}
