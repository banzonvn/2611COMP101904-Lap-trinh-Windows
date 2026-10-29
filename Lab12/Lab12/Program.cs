using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using QLSinhVienLab12.Forms;

namespace QLSinhVienLab12
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

            Application.Run(new FrmNhapDiemTheoLop());
        }

        private static void RunCaptureMode()
        {
            Console.WriteLine("[Lab12 Runner] Starting automatic screenshot generation for all 12 scenarios...");

            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            string projectImagesDir = Path.GetFullPath(Path.Combine(appDir, "..", "..", "..", "images"));
            string parentLabImagesDir = Path.GetFullPath(Path.Combine(appDir, "..", "..", "..", "..", "images"));

            Directory.CreateDirectory(projectImagesDir);
            Directory.CreateDirectory(parentLabImagesDir);

            using var form = new FrmNhapDiemTheoLop();
            form.SuppressMessageBox = true; // Prevent modal popups from blocking capture
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

            // Step 1: Form initial load & ComboBoxes
            form.ShowAlert("Khởi chạy module Quản lý kết quả học tập thành công. Đã nạp danh mục Lớp và Môn học.", AlertType.Info);
            Application.DoEvents();
            SaveStep("01_khoi_chay_va_load_combobox.png");

            // Step 2: Load student list for class 51.CNTT.A, subject COMP1019
            form.cboLop.SelectedIndex = 0;
            form.cboMonHoc.SelectedIndex = 0;
            form.btnTaiDanhSach_Click(form.btnTaiDanhSach, EventArgs.Empty);
            Application.DoEvents();
            SaveStep("02_tai_danh_sach_sinh_vien_lop.png");

            // Step 3: Edit grades directly on grid
            if (form.dgvKetQua.Rows.Count >= 3)
            {
                form.dgvKetQua.Rows[0].Cells["Diem"].Value = 9.8;
                form.dgvKetQua.Rows[0].Cells["GhiChu"].Value = "Xuất sắc (Điểm cộng)";
                form.dgvKetQua.Rows[1].Cells["Diem"].Value = 8.5;
                form.dgvKetQua.Rows[1].Cells["GhiChu"].Value = "Giỏi";
                form.dgvKetQua.Rows[2].Cells["Diem"].Value = 7.5;
                form.dgvKetQua.Rows[2].Cells["GhiChu"].Value = "Khá tốt";
            }
            form.UpdateStatistics();
            form.ShowAlert("Đang chỉnh sửa điểm và ghi chú trực tiếp trên DataGridView. Điểm được tự động highlight màu xanh nhạt.", AlertType.Info);
            Application.DoEvents();
            SaveStep("03_nhap_sua_diem_tren_grid.png");

            // Step 4: Save all with Transaction (Commit success)
            form.btnLuuTatCa_Click(form.btnLuuTatCa, EventArgs.Empty);
            Application.DoEvents();
            SaveStep("04_luu_tat_ca_transaction_thanh_cong.png");

            // Step 5: Statistics card overview
            form.ShowAlert("Bảng phân tích điểm học phần tự động tính Điểm TB, Tỉ lệ đạt (%) và Max/Min theo thời gian thực.", AlertType.Success);
            Application.DoEvents();
            SaveStep("05_thong_ke_diem_va_ti_le_dat.png");

            // Step 6: Switch class and subject
            if (form.cboLop.Items.Count > 1) form.cboLop.SelectedIndex = 1; // CTK47A
            if (form.cboMonHoc.Items.Count > 1) form.cboMonHoc.SelectedIndex = 1; // COMP1017
            form.btnTaiDanhSach_Click(form.btnTaiDanhSach, EventArgs.Empty);
            Application.DoEvents();
            SaveStep("06_chuyen_doi_lop_va_mon_hoc.png");

            // Step 7: Validation non-numeric input
            if (form.dgvKetQua.Rows.Count > 0)
            {
                form.dgvKetQua.Rows[0].Cells["Diem"].Value = "abc";
                form.btnLuuTatCa_Click(form.btnLuuTatCa, EventArgs.Empty);
            }
            Application.DoEvents();
            SaveStep("07_validation_diem_khong_hop_le_chu.png");

            // Step 8: Validation grade > 10
            if (form.dgvKetQua.Rows.Count > 0)
            {
                form.dgvKetQua.Rows[0].Cells["Diem"].Value = 11.5;
                form.btnLuuTatCa_Click(form.btnLuuTatCa, EventArgs.Empty);
            }
            Application.DoEvents();
            SaveStep("08_validation_diem_vuot_qua_10.png");

            // Step 9: Validation negative grade < 0
            if (form.dgvKetQua.Rows.Count > 0)
            {
                form.dgvKetQua.Rows[0].Cells["Diem"].Value = -2.5;
                form.btnLuuTatCa_Click(form.btnLuuTatCa, EventArgs.Empty);
            }
            Application.DoEvents();
            SaveStep("09_validation_diem_am.png");

            // Step 10: Transaction Rollback Demo
            // Set valid grades for row 0, 1, but invalid for row 2
            if (form.dgvKetQua.Rows.Count >= 2)
            {
                form.dgvKetQua.Rows[0].Cells["Diem"].Value = 8.0;
                form.dgvKetQua.Rows[1].Cells["Diem"].Value = 15.0; // Trigger rollback
            }
            form.btnLuuTatCa_Click(form.btnLuuTatCa, EventArgs.Empty);
            Application.DoEvents();
            SaveStep("10_demo_transaction_rollback_toan_bo.png");

            // Step 11: Transaction & SP Audit Log
            form.ShowAlert("Nhật ký Transaction ghi nhận toàn bộ chu trình BEGIN -> EXEC sp_KetQua_Save -> ROLLBACK / COMMIT an toàn.", AlertType.Info);
            Application.DoEvents();
            SaveStep("11_audit_log_transaction_sp.png");

            // Step 12: Reset Form and sample data
            form.btnResetSample_Click(form.btnResetSample, EventArgs.Empty);
            Application.DoEvents();
            SaveStep("12_lam_moi_khoi_phuc_du_lieu.png");

            Console.WriteLine("[Lab12 Runner] All 12 screenshots generated successfully!");
            Environment.Exit(0);
        }
    }
}
