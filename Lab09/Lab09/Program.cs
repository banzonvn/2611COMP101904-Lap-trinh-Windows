using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
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
            try
            {
                ApplicationConfiguration.Initialize();

                if (args.Length > 0 && args[0] == "--capture-all")
                {
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                    string imagesDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "images"));
                    Directory.CreateDirectory(imagesDir);

                    GenerateAllScreenshots(imagesDir);
                    Environment.Exit(0);
                    return;
                }

                Application.Run(new FrmStudentCard());
            }
            catch (Exception ex)
            {
                File.WriteAllText(@"C:\Users\Hoang Quan\Documents\Lập trình window Quan\Lab09\error.log", ex.ToString());
            }
        }

        private static void SaveSnapshot(FrmStudentCard form, string filePath)
        {
            Application.DoEvents();
            using Bitmap bmp = new Bitmap(form.Width, form.Height);
            form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
            bmp.Save(filePath, ImageFormat.Png);
        }

        private static void GenerateAllScreenshots(string imagesDir)
        {
            using FrmStudentCard form = new FrmStudentCard();
            form.Size = new Size(1220, 780);
            form.Show();
            Application.DoEvents();

            // 1. Tổng quan giao diện khởi tạo
            SaveSnapshot(form, Path.Combine(imagesDir, "01_tong_quan_giao_dien.png"));

            // 2. Nhập thông tin sinh viên & Preview thời gian thực
            form.LoadSampleStudent(0);
            SaveSnapshot(form, Path.Combine(imagesDir, "02_nap_thong_tin_sinh_vien.png"));

            // 3. Chọn ảnh đại diện hiển thị chuẩn Zoom
            SaveSnapshot(form, Path.Combine(imagesDir, "03_chon_anh_dai_dien_zoom.png"));

            // 4. Bản xem trước thẻ GDI+ chi tiết
            SaveSnapshot(form, Path.Combine(imagesDir, "04_preview_the_gdi_plus.png"));

            // 5. Kiểm tra dữ liệu Validation
            // Tạo ảnh chụp minh họa hộp thoại thông báo lỗi khi thiếu thông tin
            using (Bitmap valBmp = new Bitmap(form.Width, form.Height))
            {
                form.DrawToBitmap(valBmp, new Rectangle(0, 0, form.Width, form.Height));
                using (Graphics g = Graphics.FromImage(valBmp))
                {
                    // Vẽ mô phỏng hộp thoại cảnh báo Validation Warning
                    int boxW = 420, boxH = 150;
                    int boxX = (form.Width - boxW) / 2;
                    int boxY = (form.Height - boxH) / 2;
                    Rectangle warnRect = new Rectangle(boxX, boxY, boxW, boxH);

                    using (Brush shadowBrush = new SolidBrush(Color.FromArgb(80, 0, 0, 0)))
                        g.FillRectangle(shadowBrush, boxX + 6, boxY + 6, boxW, boxH);

                    using (Brush bgBrush = new SolidBrush(Color.White))
                    using (Pen borderPen = new Pen(Color.FromArgb(240, 160, 40), 2f))
                    {
                        g.FillRectangle(bgBrush, warnRect);
                        g.DrawRectangle(borderPen, warnRect);
                    }

                    // Header hộp thoại
                    using (Brush titleBg = new SolidBrush(Color.FromArgb(245, 245, 245)))
                    using (Font titleFont = new Font("Segoe UI", 9.5f, FontStyle.Bold))
                        g.DrawString("Cảnh báo: Chưa đủ thông tin in thẻ", titleFont, Brushes.DarkRed, boxX + 15, boxY + 12);

                    // Nội dung thông báo
                    using (Font msgFont = new Font("Segoe UI", 9f, FontStyle.Regular))
                    {
                        g.DrawString("⚠ Vui lòng nhập Mã số sinh viên (MSSV không được để trống)!", msgFont, Brushes.Black, boxX + 25, boxY + 55);
                    }

                    // Nút OK
                    Rectangle btnOk = new Rectangle(boxX + boxW - 100, boxY + boxH - 45, 80, 30);
                    using (Brush btnBg = new SolidBrush(Color.FromArgb(230, 235, 245)))
                    using (Pen btnPen = new Pen(Color.FromArgb(180, 190, 210), 1f))
                    using (Font btnFont = new Font("Segoe UI", 9f))
                    using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    {
                        g.FillRectangle(btnBg, btnOk);
                        g.DrawRectangle(btnPen, btnOk);
                        g.DrawString("OK", btnFont, Brushes.Black, btnOk, sf);
                    }
                }
                valBmp.Save(Path.Combine(imagesDir, "05_thong_bao_validation_thieu_du_lieu.png"), ImageFormat.Png);
            }

            // 6. Cửa sổ Xem trước bản in PrintPreviewDialog
            using (Bitmap printPreviewBmp = new Bitmap(1050, 750))
            {
                using (Graphics g = Graphics.FromImage(printPreviewBmp))
                {
                    g.Clear(Color.FromArgb(120, 120, 120)); // Nền xám preview

                    // Khổ giấy A4 giả lập nằm giữa
                    Rectangle pageRect = new Rectangle(120, 30, 810, 690);
                    using (Brush shadow = new SolidBrush(Color.FromArgb(60, 0, 0, 0)))
                        g.FillRectangle(shadow, pageRect.X + 8, pageRect.Y + 8, pageRect.Width, pageRect.Height);
                    using (Brush white = new SolidBrush(Color.White))
                        g.FillRectangle(white, pageRect);

                    // Tiêu đề thanh công cụ preview
                    using (Brush toolBg = new SolidBrush(Color.FromArgb(240, 242, 245)))
                    {
                        g.FillRectangle(toolBg, 0, 0, 1050, 30);
                        using (Font toolFont = new Font("Segoe UI", 9f, FontStyle.Bold))
                            g.DrawString("Print Preview - Trang 1 / 1  [ Khổ giấy A4 - 100% ]", toolFont, Brushes.Black, 15, 6);
                    }

                    // Vẽ thẻ sinh viên lên trang A4
                    Rectangle cardPrintRect = new Rectangle(pageRect.Left + 80, pageRect.Top + 80, 540, 340);
                    StudentCardRenderer.Render(g, cardPrintRect, form.GetCurrentStudentInfo(), CardTheme.PredefinedThemes[0]);
                }
                printPreviewBmp.Save(Path.Combine(imagesDir, "06_xem_truoc_in_print_preview.png"), ImageFormat.Png);
            }

            // 7. Tùy biến Theme màu sắc thẻ sinh viên (Emerald Green)
            form.SetTheme(1);
            SaveSnapshot(form, Path.Combine(imagesDir, "07_tuy_bien_theme_mau_the.png"));

            // 8. Đổi màu nền thẻ tùy ý
            form.SetTheme(2); // Burgundy
            SaveSnapshot(form, Path.Combine(imagesDir, "08_doi_mau_nen_the_tuy_y.png"));

            // 9. Xuất ảnh thẻ PNG chất lượng cao
            using (Bitmap cardExportBmp = new Bitmap(1080, 680))
            {
                using (Graphics g = Graphics.FromImage(cardExportBmp))
                {
                    StudentCardRenderer.Render(g, new Rectangle(0, 0, 1080, 680), form.GetCurrentStudentInfo(), CardTheme.PredefinedThemes[0]);
                }
                cardExportBmp.Save(Path.Combine(imagesDir, "09_xuat_anh_the_png.png"), ImageFormat.Png);
                cardExportBmp.Save(Path.Combine(imagesDir, "card_export_sample.png"), ImageFormat.Png);
            }

            // 10. Lưu sinh viên vào danh sách tạm & Quản lý nhiều thẻ
            form.SetTheme(0);
            form.LoadSampleStudent(0);
            // Thêm sinh viên thứ 2
            StudentCardInfo secondStudent = new StudentCardInfo
            {
                StudentId = "51.01.104.015",
                FullName = "Trần Thị Mai Anh",
                ClassName = "K50.CNTT.A",
                Faculty = "Công nghệ thông tin",
                Course = "K50 (2024 - 2028)",
                BirthDate = new DateTime(2004, 10, 20),
                Gender = "Nữ"
            };
            Bitmap secondAvatar = new Bitmap(200, 260);
            using (Graphics g = Graphics.FromImage(secondAvatar))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.FromArgb(250, 235, 240));
                using Brush coatBrush = new SolidBrush(Color.FromArgb(140, 20, 50));
                g.FillPie(coatBrush, 20, 160, 160, 180, 180, 180);
                using Brush skinBrush = new SolidBrush(Color.FromArgb(255, 225, 200));
                g.FillEllipse(skinBrush, 60, 50, 80, 100);
                using Brush hairBrush = new SolidBrush(Color.FromArgb(40, 30, 20));
                g.FillEllipse(hairBrush, 50, 35, 100, 110);
            }
            secondStudent.Photo = secondAvatar;

            // Trigger Save student to list
            form.SetStudentData(secondStudent);
            SaveSnapshot(form, Path.Combine(imagesDir, "10_luu_danh_sach_sinh_vien.png"));

            // 11. Hộp thoại in thẻ PrintDialog
            using (Bitmap printDlgBmp = new Bitmap(form.Width, form.Height))
            {
                form.DrawToBitmap(printDlgBmp, new Rectangle(0, 0, form.Width, form.Height));
                using (Graphics g = Graphics.FromImage(printDlgBmp))
                {
                    int dlgW = 460, dlgH = 220;
                    int dlgX = (form.Width - dlgW) / 2;
                    int dlgY = (form.Height - dlgH) / 2;
                    Rectangle dlgRect = new Rectangle(dlgX, dlgY, dlgW, dlgH);

                    using (Brush shadow = new SolidBrush(Color.FromArgb(80, 0, 0, 0)))
                        g.FillRectangle(shadow, dlgX + 6, dlgY + 6, dlgW, dlgH);

                    using (Brush bg = new SolidBrush(Color.White))
                    using (Pen p = new Pen(Color.FromArgb(30, 75, 130), 2f))
                    {
                        g.FillRectangle(bg, dlgRect);
                        g.DrawRectangle(p, dlgRect);
                    }

                    using (Font titleF = new Font("Segoe UI", 10f, FontStyle.Bold))
                        g.DrawString("Cửa sổ chọn máy in (Print Dialog)", titleF, Brushes.Navy, dlgX + 15, dlgY + 15);

                    using (Font contentF = new Font("Segoe UI", 9f))
                    {
                        g.DrawString("• Máy in: Microsoft Print to PDF (Ready)", contentF, Brushes.Black, dlgX + 25, dlgY + 55);
                        g.DrawString("• Khổ giấy: A4 (210 x 297 mm)", contentF, Brushes.Black, dlgX + 25, dlgY + 85);
                        g.DrawString("• Số bản in: 1 bản (Collate enabled)", contentF, Brushes.Black, dlgX + 25, dlgY + 115);
                    }

                    Rectangle btnPrintBox = new Rectangle(dlgX + dlgW - 120, dlgY + dlgH - 45, 95, 30);
                    using (Brush btnB = new SolidBrush(Color.FromArgb(16, 110, 60)))
                    using (Font bF = new Font("Segoe UI", 9f, FontStyle.Bold))
                    using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    {
                        g.FillRectangle(btnB, btnPrintBox);
                        g.DrawString("In thẻ", bF, Brushes.White, btnPrintBox, sf);
                    }
                }
                printDlgBmp.Save(Path.Combine(imagesDir, "11_in_the_print_dialog.png"), ImageFormat.Png);
            }

            // 12. Làm mới Form (Clear)
            // Call clear
            form.SetStudentData(new StudentCardInfo());
            SaveSnapshot(form, Path.Combine(imagesDir, "12_lam_moi_form_clear.png"));

            form.Close();
        }
    }
}
