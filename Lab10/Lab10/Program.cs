using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace COMP1019_Lab10_EduManagerStudio
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

            Application.Run(new FrmQueryStudio());
        }

        private static void RunCaptureMode()
        {
            Console.WriteLine("[EduManagerStudio] Starting automatic screenshot generation for all 12 queries...");

            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            // Target output directories
            string projectImagesDir = Path.GetFullPath(Path.Combine(appDir, "..", "..", "..", "images"));
            string parentLabImagesDir = Path.GetFullPath(Path.Combine(appDir, "..", "..", "..", "..", "images"));

            Directory.CreateDirectory(projectImagesDir);
            Directory.CreateDirectory(parentLabImagesDir);

            using var form = new FrmQueryStudio();
            form.Show();
            Application.DoEvents();
            Thread.Sleep(500);

            string[] fileNames = new[]
            {
                "01_liet_ke_tat_ca_sinh_vien.png",
                "02_sinh_vien_kem_lop_va_khoa.png",
                "03_tim_kiem_sinh_vien_theo_ten.png",
                "04_sinh_vien_thuoc_lop_cu_the.png",
                "05_cap_nhat_thong_tin_sinh_vien.png",
                "06_xoa_sinh_vien_chua_co_diem.png",
                "07_bang_diem_chi_tiet_theo_mon.png",
                "08_diem_trung_binh_va_xep_loai.png",
                "09_thong_ke_sinh_vien_theo_lop.png",
                "10_lop_co_tu_2_sinh_vien.png",
                "11_test_vi_pham_khoa_ngoai.png",
                "12_test_vi_pham_rang_buoc_check.png"
            };

            for (int i = 0; i < fileNames.Length; i++)
            {
                string fileName = fileNames[i];
                string projPath = Path.Combine(projectImagesDir, fileName);
                string parentPath = Path.Combine(parentLabImagesDir, fileName);

                form.ExecuteAndCapture(i, projPath);
                File.Copy(projPath, parentPath, true);

                Console.WriteLine($"[Captured {i + 1}/12]: {fileName}");
                Thread.Sleep(100);
            }

            Console.WriteLine("[EduManagerStudio] All 12 screenshots generated successfully!");
            Environment.Exit(0);
        }
    }
}
