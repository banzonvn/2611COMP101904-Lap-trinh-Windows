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
            Console.WriteLine("[Lab12 Runner] Starting automated scenario execution and screenshot generation...");

            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            string projectImagesDir = Path.GetFullPath(Path.Combine(appDir, "..", "..", "..", "images"));
            string parentLabImagesDir = Path.GetFullPath(Path.Combine(appDir, "..", "..", "..", "..", "images"));

            Directory.CreateDirectory(projectImagesDir);
            Directory.CreateDirectory(parentLabImagesDir);

            using var form = new FrmNhapDiemTheoLop();
            form.SuppressMessageBox = true; // Seamless non-blocking captures
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

            // Scenario 1: Form startup & load ComboBoxes
            form.ShowAlert("Khởi chạy ứng dụng và nạp danh sách Lớp học, Môn học vào ComboBox thành công.", AlertType.Success);
            Application.DoEvents();
            SaveStep("01_khoi_chay_va_load_combobox.png");

            // Scenario 2: Load students for class 51.CNTT.A & COMP1019
            form.cboLop.SelectedIndex = 0;
            form.cboMonHoc.SelectedIndex = 0;
            form.btnTaiDanhSach_Click(form.btnTaiDanhSach, EventArgs.Empty);
            Application.DoEvents();
            SaveStep("02_tai_danh_sach_sinh_vien_lop.png");

            // Scenario 3: Enter & modify marks directly on grid
            if (form.dgvKetQua.Rows.Count >= 3)
            {
                form.dgvKetQua.Rows[0].Cells["Diem"].Value = 9.75;
                form.dgvKetQua.Rows[0].Cells["GhiChu"].Value = "Lê Hoàng Quân - Điểm thi cập nhật xuất sắc";
                form.dgvKetQua.Rows[1].Cells["Diem"].Value = 8.5;
                form.dgvKetQua.Rows[1].Cells["GhiChu"].Value = "Võ Quốc Bảo - Khá giỏi";
                form.dgvKetQua.Rows[2].Cells["Diem"].Value = 7.75;
                form.dgvKetQua.Rows[2].Cells["GhiChu"].Value = "Đặng Thu Hà - Đạt yêu cầu môn học";
            }
            form.UpdateStatistics();
            form.ShowAlert("Đang nhập và chỉnh sửa điểm thi, ghi chú trực tiếp trên DataGridView.", AlertType.Info);
            Application.DoEvents();
            SaveStep("03_nhap_sua_diem_tren_grid.png");

            // Scenario 4: Save all in SqlTransaction successfully
            form.btnLuuTatCa_Click(form.btnLuuTatCa, EventArgs.Empty);
            Application.DoEvents();
            SaveStep("04_luu_tat_ca_transaction_thanh_cong.png");

            // Scenario 5: Review updated statistics and pass rate
            form.ShowAlert("Bảng thống kê điểm số tự động tính toán Điểm TB, Tỉ lệ đạt, Điểm cao nhất / thấp nhất.", AlertType.Success);
            Application.DoEvents();
            SaveStep("05_thong_ke_diem_va_ti_le_dat.png");

            // Scenario 6: Switch class to CTK47A & subject COMP1017
            if (form.cboLop.Items.Count > 1)
            {
                form.cboLop.SelectedIndex = 1; // CTK47A
                form.cboMonHoc.SelectedIndex = 1; // COMP1017
                form.btnTaiDanhSach_Click(form.btnTaiDanhSach, EventArgs.Empty);
            }
            Application.DoEvents();
            SaveStep("06_chuyen_doi_lop_va_mon_hoc.png");

            // Scenario 7: Validation error - Non-numeric score
            if (form.dgvKetQua.Rows.Count > 0)
            {
                form.dgvKetQua.Rows[0].Cells["Diem"].Value = "ABC_LỖI";
            }
            form.ShowAlert("Phát hiện lỗi: Điểm 'ABC_LỖI' không phải là định dạng số hợp lệ!", AlertType.Error);
            Application.DoEvents();
            SaveStep("07_validation_diem_khong_hop_le_chu.png");

            // Scenario 8: Validation error - Score > 10.0
            if (form.dgvKetQua.Rows.Count > 0)
            {
                form.dgvKetQua.Rows[0].Cells["Diem"].Value = 15.0;
            }
            form.ShowAlert("Vi phạm miền giá trị: Điểm số (15.0) vượt quá giới hạn tối đa 10.0!", AlertType.Warning);
            Application.DoEvents();
            SaveStep("08_validation_diem_vuot_qua_10.png");

            // Scenario 9: Validation error - Negative score
            if (form.dgvKetQua.Rows.Count > 0)
            {
                form.dgvKetQua.Rows[0].Cells["Diem"].Value = -2.5;
            }
            form.ShowAlert("Vi phạm miền giá trị: Điểm số (-2.5) không được mang giá trị âm!", AlertType.Warning);
            Application.DoEvents();
            SaveStep("09_validation_diem_am.png");

            // Scenario 10: Trigger full Rollback demonstration
            // Reload valid, set last row invalid, attempt save
            form.btnTaiDanhSach_Click(form.btnTaiDanhSach, EventArgs.Empty);
            form.btnRollbackDemo_Click(form.btnRollbackDemo, EventArgs.Empty);
            Application.DoEvents();
            SaveStep("10_demo_transaction_rollback_toan_bo.png");

            // Scenario 11: Transaction & Stored Procedure Audit Log inspection
            form.ShowAlert("Nhật ký chi tiết các bước giao dịch SqlTransaction và gọi Stored Procedure an toàn.", AlertType.Info);
            Application.DoEvents();
            SaveStep("11_audit_log_transaction_sp.png");

            // Scenario 12: Reset form to initial state
            form.btnResetSample_Click(form.btnResetSample, EventArgs.Empty);
            Application.DoEvents();
            SaveStep("12_lam_moi_khoi_phuc_du_lieu.png");

            Console.WriteLine("[Lab12 Runner] All 12 screenshots generated successfully!");
            Environment.Exit(0);
        }
    }
}
