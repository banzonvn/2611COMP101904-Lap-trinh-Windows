using System.Drawing.Imaging;
using Lab07.Data;
using Lab07.Forms;
using Lab07.Models;

namespace Lab07;

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

        // Vòng lặp đăng nhập - đăng xuất chuẩn
        while (true)
        {
            using var loginForm = new FrmLogin();
            if (loginForm.ShowDialog() == DialogResult.OK && loginForm.CurrentUser != null)
            {
                using var mainForm = new FrmMain(loginForm.CurrentUser);
                Application.Run(mainForm);
                break;
            }
            else
            {
                break; // Hủy đăng nhập -> thoát
            }
        }
    }

    private static void RunHeadlessCapture()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string projectDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", ".."));
        string lab07Dir = Path.GetFullPath(Path.Combine(projectDir, ".."));

        string imgDir1 = Path.Combine(projectDir, "images");
        string imgDir2 = Path.Combine(lab07Dir, "images");

        Directory.CreateDirectory(imgDir1);
        Directory.CreateDirectory(imgDir2);

        void SaveFormShot(Form form, string filename)
        {
            form.Show();
            Application.DoEvents();
            Thread.Sleep(150);
            using var bmp = new Bitmap(form.Width, form.Height);
            form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
            bmp.Save(Path.Combine(imgDir1, filename), ImageFormat.Png);
            bmp.Save(Path.Combine(imgDir2, filename), ImageFormat.Png);
            Console.WriteLine($"[Captured]: {filename}");
        }

        // ========================================================
        // 1. Chụp FrmLogin: Giao diện ban đầu
        // ========================================================
        using (var frmLogin = new FrmLogin())
        {
            frmLogin.SetCredentials("admin", "123");
            SaveFormShot(frmLogin, "01_dang_nhap_giao_dien.png");

            // 3. FrmLogin: Cảnh báo bỏ trống
            frmLogin.SetCredentials("", "");
            frmLogin.PerformLogin(showMsgBox: false);
            SaveFormShot(frmLogin, "03_dang_nhap_loi_bo_trong.png");

            // 4. FrmLogin: Cảnh báo sai mật khẩu
            frmLogin.SetCredentials("admin", "sai_mat_khau");
            frmLogin.PerformLogin(showMsgBox: false);
            SaveFormShot(frmLogin, "04_dang_nhap_loi_sai_mat_khau.png");
        }

        // ========================================================
        // 2. Chụp FrmMain: Sau khi đăng nhập thành công
        // ========================================================
        var adminUser = ShopDataStore.Authenticate("admin", "123")!;
        using var frmMain = new FrmMain(adminUser);
        frmMain.Show();
        Application.DoEvents();
        Thread.Sleep(200);

        SaveFormShot(frmMain, "02_dang_nhap_thanh_cong_admin.png");

        // 5. Mở FrmProduct dạng MDI
        var frmProduct = frmMain.OpenProductManagement();
        frmProduct.WindowState = FormWindowState.Normal;
        frmProduct.Location = new Point(40, 20);
        Application.DoEvents();
        Thread.Sleep(150);
        SaveFormShot(frmMain, "05_mo_form_san_pham_mdi.png");

        // 6. Thêm sản phẩm hợp lệ
        frmProduct.SetInputFields("SP011", "Trà sữa Ô long nướng 450ml", "Đồ uống", 22000m, 50);
        frmProduct.AddProduct(showDialog: false);
        SaveFormShot(frmMain, "06_san_pham_them_thanh_cong.png");

        // 7. Validation trùng mã sản phẩm
        frmProduct.SetInputFields("SP001", "Bánh Cosy mè giòn (Trùng mã)", "Bánh kẹo", 35000m, 20);
        frmProduct.AddProduct(showDialog: false);
        SaveFormShot(frmMain, "07_san_pham_validation_loi_trung_ma.png");

        // 8. Validation đơn giá <= 0 hoặc số lượng âm
        frmProduct.SetInputFields("SP012", "Sản phẩm giá âm", "Gia dụng", -5000m, 10);
        frmProduct.AddProduct(showDialog: false);
        SaveFormShot(frmMain, "08_san_pham_validation_gia_so_luong_am.png");

        // 9. Cập nhật thông tin sản phẩm
        frmProduct.SetInputFields("SP001", "Bánh Cosy mè giòn 288g (Giảm giá)", "Bánh kẹo", 32000m, 60);
        frmProduct.UpdateProduct(showDialog: false);
        SaveFormShot(frmMain, "09_san_pham_sua_thanh_cong.png");

        // 10. Tìm kiếm sản phẩm
        frmProduct.SetSearchKeyword("Coca");
        frmProduct.SearchProduct();
        SaveFormShot(frmMain, "10_san_pham_tim_kiem.png");

        // 11. Xóa sản phẩm
        frmProduct.LoadProductData();
        frmProduct.SetInputFields("SP010", "Khăn ướt Nano Bạc gói 80 tờ", "Gia dụng", 28000m, 0);
        frmProduct.DeleteProduct(confirm: false);
        SaveFormShot(frmMain, "11_san_pham_xoa_thanh_cong.png");

        // 12. Mở FrmCustomer dạng MDI (Ẩn tạm thời FrmProduct để màn hình khách hàng nổi bật)
        frmProduct.Hide();
        var frmCustomer = frmMain.OpenCustomerManagement();
        frmCustomer.WindowState = FormWindowState.Normal;
        frmCustomer.Location = new Point(40, 20);
        frmCustomer.Show();
        frmCustomer.BringToFront();
        frmCustomer.Activate();
        Application.DoEvents();
        Thread.Sleep(150);
        SaveFormShot(frmMain, "12_mo_form_khach_hang_mdi.png");

        // 13. Thêm khách hàng mới VIP
        frmCustomer.SetInputFields("KH009", "Trần Đình Trọng", "0938889999", "VIP", 500);
        frmCustomer.AddCustomer(showDialog: false);
        SaveFormShot(frmMain, "13_khach_hang_them_moi.png");

        // 14. Tìm kiếm khách hàng
        frmCustomer.SetSearchKeyword("Quân");
        frmCustomer.SearchCustomer();
        SaveFormShot(frmMain, "14_khach_hang_tim_kiem.png");

        // 15. Kiểm tra chống mở trùng Form MDI: Kích hoạt lại FrmProduct từ menu
        frmCustomer.Location = new Point(30, 20);
        frmProduct.Location = new Point(80, 50);
        var productAgain = frmMain.OpenProductManagement();
        productAgain.BringToFront();
        productAgain.Activate();
        Application.DoEvents();
        Thread.Sleep(150);
        SaveFormShot(frmMain, "15_chong_mo_trung_form_con.png");

        // 16. Mở FrmAbout
        using (var frmAbout = new FrmAbout())
        {
            SaveFormShot(frmAbout, "16_hop_thoai_gioi_thieu_about.png");
        }

        // 17. Trạng thái Form chính với thông báo chuẩn bị thoát / đăng xuất
        SaveFormShot(frmMain, "17_dang_xuat_va_thoat_xac_nhan.png");

        frmMain.Close();
        Console.WriteLine("All 17 screenshots captured successfully!");
        Environment.Exit(0);
    }
}
