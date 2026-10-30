using System.Drawing.Imaging;
using Lab15.Views;

namespace Lab15;

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

        Application.Run(new FrmStudent());
    }

    private static void RunHeadlessCapture()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        // Target folders
        string projectDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", ".."));
        string lab15Dir = Path.GetFullPath(Path.Combine(projectDir, ".."));

        string imgDir1 = Path.Combine(projectDir, "images");
        string imgDir2 = Path.Combine(lab15Dir, "images");

        Directory.CreateDirectory(imgDir1);
        Directory.CreateDirectory(imgDir2);

        using var form = new FrmStudent();
        form.Show();
        Application.DoEvents();
        Thread.Sleep(200);

        void SaveShot(string filename)
        {
            Application.DoEvents();
            Thread.Sleep(150);
            using var bmp = new Bitmap(form.Width, form.Height);
            form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
            bmp.Save(Path.Combine(imgDir1, filename), ImageFormat.Png);
            bmp.Save(Path.Combine(imgDir2, filename), ImageFormat.Png);
            Console.WriteLine($"[Captured]: {filename}");
        }

        var presenter = form.Presenter;

        // 1. Khởi chạy và hiển thị danh sách ban đầu
        presenter.LoadStudents();
        SaveShot("01_khoi_chay_va_hien_thi_danh_sach.png");

        // 2. Thêm sinh viên thành công
        form.StudentId = "51.01.104.120";
        form.FullName = "Nguyễn Gia Bảo";
        form.ClassName = "51.CNTT.A";
        form.AverageScoreText = "9.5";
        presenter.AddStudent();
        SaveShot("02_them_sinh_vien_thanh_cong.png");

        // 3. Validation lỗi mã sinh viên để trống
        form.StudentId = "";
        form.FullName = "Trần Đình Trọng";
        form.ClassName = "51.CNTT.A";
        form.AverageScoreText = "8.0";
        presenter.AddStudent();
        SaveShot("03_validation_loi_ma_sv_de_trong.png");

        // 4. Validation lỗi họ tên để trống
        form.StudentId = "51.01.104.121";
        form.FullName = "";
        form.ClassName = "51.CNTT.A";
        form.AverageScoreText = "8.0";
        presenter.AddStudent();
        SaveShot("04_validation_loi_ho_ten_de_trong.png");

        // 5. Validation lỗi trùng mã sinh viên
        form.StudentId = "51.01.104.082"; // Đã có của Lê Hoàng Quân
        form.FullName = "Lê Hoàng Quân (Bản sao)";
        form.ClassName = "51.CNTT.A";
        form.AverageScoreText = "9.0";
        presenter.AddStudent();
        SaveShot("05_validation_loi_trung_ma_sinh_vien.png");

        // 6. Validation lỗi điểm không hợp lệ (> 10)
        form.StudentId = "51.01.104.122";
        form.FullName = "Phan Văn Hậu";
        form.ClassName = "CTK47A";
        form.AverageScoreText = "15.5";
        presenter.AddStudent();
        SaveShot("06_validation_loi_diem_khong_hop_le.png");

        // 7. Chọn dòng hiển thị lên input
        presenter.SelectStudent("51.01.104.082");
        SaveShot("07_chon_dong_hien_thi_len_input.png");

        // 8. Sửa thông tin sinh viên thành công
        form.AverageScoreText = "10.0";
        presenter.UpdateStudent();
        SaveShot("08_sua_thong_tin_sinh_vien_thanh_cong.png");

        // 9. Tìm kiếm theo họ tên sinh viên
        form.SearchKeyword = "Hoàng";
        presenter.SearchStudent();
        SaveShot("09_tim_kiem_theo_ho_ten_sinh_vien.png");

        // 10. Tìm kiếm theo mã sinh viên
        form.SearchKeyword = "47.01";
        presenter.SearchStudent();
        SaveShot("10_tim_kiem_theo_ma_sinh_vien.png");

        // 11. Xóa sinh viên thành công
        form.StudentId = "47.01.104.088"; // Ngô Kiến Huy
        presenter.DeleteStudent();
        SaveShot("11_xoa_sinh_vien_thanh_cong.png");

        // 12. Làm mới form và nạp lại toàn bộ
        form.SearchKeyword = "";
        presenter.ClearInput();
        SaveShot("12_lam_moi_form_va_load_lai_toan_bo.png");

        form.Close();
        Console.WriteLine("All 12 screenshots generated successfully!");
    }
}
