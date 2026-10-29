using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace COMP1019_Lab08_SimplePaint
{
    internal static class Program
    {
        /// <summary>
        /// Điểm bắt đầu chính của ứng dụng Windows Forms Lab 08 - Simple Paint.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();

            // Nếu truyền tham số --capture-screenshot, khởi tạo giao diện mẫu và lưu ảnh chụp màn hình
            if (args.Length > 0 && args[0] == "--capture-screenshot")
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string imagesDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "images"));
                Directory.CreateDirectory(imagesDir);

                using FrmPaint form = new FrmPaint();
                form.Size = new Size(1150, 720);
                form.LoadSampleDrawing();
                form.Show();
                Application.DoEvents();

                // 1. Chụp ảnh toàn bộ giao diện Form đang chạy
                string screenshotPath = Path.Combine(imagesDir, "screenshot_lab08.png");
                using (Bitmap formBmp = new Bitmap(form.Width, form.Height))
                {
                    form.DrawToBitmap(formBmp, new Rectangle(0, 0, form.Width, form.Height));
                    formBmp.Save(screenshotPath, ImageFormat.Png);
                }

                // 2. Chụp ảnh riêng phần vẽ Canvas xuất ra (chức năng Save PNG)
                string canvasExportPath = Path.Combine(imagesDir, "canvas_export_sample.png");
                using (Bitmap canvasBmp = new Bitmap(950, 550))
                {
                    using (Graphics g = Graphics.FromImage(canvasBmp))
                    {
                        g.Clear(Color.White);
                        g.SmoothingMode = SmoothingMode.AntiAlias;

                        // Vẽ tiêu đề demo lên ảnh
                        using Font titleFont = new Font("Segoe UI", 12f, FontStyle.Bold);
                        g.DrawString("COMP1019 - Lab 08 Simple Paint | Demo 4 loại hình + Tô màu", titleFont, Brushes.DarkSlateGray, 20, 20);

                        // Lấy các hình vẽ từ form
                        form.LoadSampleDrawing();
                        // Vẽ trực tiếp lên bitmap
                        foreach (Control c in form.Controls)
                        {
                            if (c is DrawSurface surface)
                            {
                                surface.DrawToBitmap(canvasBmp, new Rectangle(0, 0, surface.Width, surface.Height));
                                break;
                            }
                        }
                    }
                    canvasBmp.Save(canvasExportPath, ImageFormat.Png);
                }

                form.Close();
                return;
            }

            Application.Run(new FrmPaint());
        }
    }
}
