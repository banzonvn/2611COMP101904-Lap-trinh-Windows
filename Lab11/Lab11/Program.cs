using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace COMP1019_Buoi11_Lab11
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();

            if (args.Length > 0 && args[0].Equals("--capture-all", StringComparison.OrdinalIgnoreCase))
            {
                RunCaptureMode();
                return;
            }

            Application.Run(new FrmLop());
        }

        private static void RunCaptureMode()
        {
            Console.WriteLine("[Lab11 Runner] Starting automatic screenshot generation for all 12 scenarios...");

            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            string projectImagesDir = Path.GetFullPath(Path.Combine(appDir, "..", "..", "..", "images"));
            string parentLabImagesDir = Path.GetFullPath(Path.Combine(appDir, "..", "..", "..", "..", "images"));

            Directory.CreateDirectory(projectImagesDir);
            Directory.CreateDirectory(parentLabImagesDir);

            using var form = new FrmLop();
            form.SuppressMessageBox = true; // Avoid modal popups during automated capture
            form.Show();
            Application.DoEvents();
            Thread.Sleep(300);

            void SaveStep(string fileName)
            {
                string p1 = Path.Combine(projectImagesDir, fileName);
                string p2 = Path.Combine(parentLabImagesDir, fileName);
                form.CaptureView(p1);
                File.Copy(p1, p2, true);
                Console.WriteLine($"[Captured]: {fileName}");
                Thread.Sleep(120);
            }

            // Step 1: Initial load
            form.LoadData();
            form.ShowAlert("Tải danh sách lớp học từ CSDL EduManagerDB thành công (4 lớp).", AlertType.Success);
            Application.DoEvents();
            SaveStep("01_khoi_chay_va_load_du_lieu.png");

            // Step 2: Select Row CNTT01 (Has 3 students)
            form.SelectRowByMaLop("CNTT01");
            form.SelectTab(0);
            Application.DoEvents();
            SaveStep("02_chon_dong_do_du_lieu_len_form.png");

            // Step 3: Insert valid new class
            form.ClearInput();
            form.txtMaLop.Text = "51.CNTT.CLC";
            form.txtTenLop.Text = "Cử nhân CNTT Khóa 51 CLC";
            Application.DoEvents();
            form.btnThem_Click(form.btnThem, EventArgs.Empty);
            Application.DoEvents();
            SaveStep("03_them_lop_moi_thanh_cong.png");

            // Step 4: Validation empty inputs
            form.ClearInput();
            form.txtMaLop.Text = "";
            form.txtTenLop.Text = "";
            Application.DoEvents();
            form.btnThem_Click(form.btnThem, EventArgs.Empty);
            Application.DoEvents();
            SaveStep("04_validation_rong_va_focus.png");

            // Step 5: Duplicate PK test
            form.ClearInput();
            form.txtMaLop.Text = "CNTT01";
            form.txtTenLop.Text = "Trùng mã lớp thử nghiệm";
            Application.DoEvents();
            form.btnThem_Click(form.btnThem, EventArgs.Empty);
            Application.DoEvents();
            SaveStep("05_validation_trung_ma_lop.png");

            // Step 6: Edit class (SPTOAN)
            form.ClearInput();
            form.SelectRowByMaLop("SPTOAN");
            form.txtTenLop.Text = "Sư phạm Toán học (K51 - Đã cập nhật)";
            Application.DoEvents();
            form.btnSua_Click(form.btnSua, EventArgs.Empty);
            Application.DoEvents();
            SaveStep("06_sua_ten_lop_thanh_cong.png");

            // Step 7: Edit without selection
            form.ClearInput();
            Application.DoEvents();
            form.btnSua_Click(form.btnSua, EventArgs.Empty);
            Application.DoEvents();
            SaveStep("07_validation_sua_khi_chua_chon.png");

            // Step 8: Delete empty class (51.CNTT.CLC with 0 students)
            form.SelectRowByMaLop("51.CNTT.CLC");
            Application.DoEvents();
            form.btnXoa_Click(form.btnXoa, EventArgs.Empty);
            Application.DoEvents();
            SaveStep("08_xoa_lop_chua_co_sinh_vien.png");

            // Step 9: Block delete class with students (CNTT01 with 3 students)
            form.SelectRowByMaLop("CNTT01");
            Application.DoEvents();
            form.btnXoa_Click(form.btnXoa, EventArgs.Empty);
            Application.DoEvents();
            SaveStep("09_chan_xoa_lop_dang_co_sinh_vien.png");

            // Step 10: Search by keyword "CNTT"
            form.txtTimKiem.Text = "CNTT";
            Application.DoEvents();
            form.btnTimKiem_Click(form.btnTimKiem, EventArgs.Empty);
            Application.DoEvents();
            SaveStep("10_tim_kiem_lop_theo_tu_khoa.png");

            // Step 11: Reset form to initial state
            form.btnLamMoi_Click(form.btnLamMoi, EventArgs.Empty);
            Application.DoEvents();
            SaveStep("11_lam_moi_form_ve_ban_dau.png");

            // Step 12: View live SQL Log
            form.SelectTab(1);
            Application.DoEvents();
            SaveStep("12_nhat_ky_lenh_sql_parameterized.png");

            Console.WriteLine("[Lab11 Runner] All 12 screenshots generated successfully!");
            Environment.Exit(0);
        }
    }
}
