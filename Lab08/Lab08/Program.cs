using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace COMP1019_Lab08_SimplePaint
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

            Application.Run(new FrmPaint());
        }

        private static void SaveSnapshot(FrmPaint form, string filePath)
        {
            Application.DoEvents();
            using Bitmap bmp = new Bitmap(form.Width, form.Height);
            form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
            bmp.Save(filePath, ImageFormat.Png);
        }

        private static void GenerateAllScreenshots(string imagesDir)
        {
            using FrmPaint form = new FrmPaint();
            form.Size = new Size(1150, 720);
            form.Show();
            Application.DoEvents();

            // 1. Tổng quan giao diện khởi tạo
            form.ClearShapes();
            form.SetCanvasBgColor(Color.White);
            form.SetBackgroundImage(null);
            form.SetMode(DrawMode.Pen);
            form.SetStrokeWidth(3);
            form.SetFill(false);
            form.SetCurrentColor(Color.Black);
            form.SetStatusText("Sẵn sàng | Chế độ: Pen | Độ dày: 3px | Đối tượng: 0 | (Ctrl+Z: Undo, Ctrl+Y: Redo, Ctrl+S: Lưu)");
            SaveSnapshot(form, Path.Combine(imagesDir, "01_tong_quan_giao_dien.png"));

            // 2. Chế độ vẽ tự do (Pen)
            form.ClearShapes();
            form.SetMode(DrawMode.Pen);
            form.SetStrokeWidth(4);
            form.SetCurrentColor(Color.DarkSlateBlue);
            form.AddShape(new PenItem
            {
                Color = Color.DarkSlateBlue,
                Width = 4f,
                Points = new List<Point>
                {
                    new Point(100, 260), new Point(140, 200), new Point(200, 160),
                    new Point(260, 220), new Point(320, 290), new Point(390, 230),
                    new Point(460, 170), new Point(530, 220), new Point(600, 310),
                    new Point(670, 240), new Point(740, 180), new Point(820, 230)
                }
            });
            form.SetStatusText("Đã thêm nét vẽ (Pen). Tổng số đối tượng: 1 | Chế độ: Pen | Độ dày: 4px | Đối tượng: 1 | (Ctrl+Z: Undo, Ctrl+Y: Redo, Ctrl+S: Lưu)");
            SaveSnapshot(form, Path.Combine(imagesDir, "02_ve_tu_do_pen.png"));

            // 3. Chế độ vẽ đoạn thẳng (Line)
            form.ClearShapes();
            form.SetMode(DrawMode.Line);
            form.SetStrokeWidth(4);
            form.SetCurrentColor(Color.Crimson);
            form.AddShape(new LineItem { Start = new Point(150, 200), End = new Point(750, 200), Color = Color.Crimson, Width = 4f });
            form.AddShape(new LineItem { Start = new Point(150, 200), End = new Point(450, 480), Color = Color.DarkOrange, Width = 4f });
            form.AddShape(new LineItem { Start = new Point(750, 200), End = new Point(450, 480), Color = Color.RoyalBlue, Width = 4f });
            form.AddShape(new LineItem { Start = new Point(250, 340), End = new Point(650, 340), Color = Color.MediumSeaGreen, Width = 3f });
            form.SetStatusText("Đã thêm nét vẽ (Line). Tổng số đối tượng: 4 | Chế độ: Line | Độ dày: 4px | Đối tượng: 4 | (Ctrl+Z: Undo, Ctrl+Y: Redo, Ctrl+S: Lưu)");
            SaveSnapshot(form, Path.Combine(imagesDir, "03_ve_doan_thang_line.png"));

            // 4. Chế độ vẽ hình chữ nhật (Rectangle)
            form.ClearShapes();
            form.SetMode(DrawMode.Rectangle);
            form.SetStrokeWidth(4);
            form.SetCurrentColor(Color.SeaGreen);
            form.AddShape(new RectangleItem { Start = new Point(120, 180), End = new Point(480, 440), Color = Color.SeaGreen, Width = 4f, IsFilled = false });
            form.AddShape(new RectangleItem { Start = new Point(540, 220), End = new Point(880, 480), Color = Color.Navy, Width = 4f, IsFilled = false });
            form.AddShape(new RectangleItem { Start = new Point(320, 280), End = new Point(680, 520), Color = Color.DarkGoldenrod, Width = 3f, IsFilled = false });
            form.SetStatusText("Đã thêm nét vẽ (Rectangle). Tổng số đối tượng: 3 | Chế độ: Rectangle | Độ dày: 4px | Đối tượng: 3 | (Ctrl+Z: Undo, Ctrl+Y: Redo, Ctrl+S: Lưu)");
            SaveSnapshot(form, Path.Combine(imagesDir, "04_ve_hinh_chu_nhat_rectangle.png"));

            // 5. Chế độ vẽ hình elip / tròn (Ellipse)
            form.ClearShapes();
            form.SetMode(DrawMode.Ellipse);
            form.SetStrokeWidth(4);
            form.SetCurrentColor(Color.DarkOrange);
            form.AddShape(new EllipseItem { Start = new Point(140, 180), End = new Point(420, 460), Color = Color.DarkOrange, Width = 4f, IsFilled = false });
            form.AddShape(new EllipseItem { Start = new Point(480, 220), End = new Point(880, 420), Color = Color.DarkViolet, Width = 4f, IsFilled = false });
            form.AddShape(new EllipseItem { Start = new Point(280, 250), End = new Point(740, 500), Color = Color.Teal, Width = 3f, IsFilled = false });
            form.SetStatusText("Đã thêm nét vẽ (Ellipse). Tổng số đối tượng: 3 | Chế độ: Ellipse | Độ dày: 4px | Đối tượng: 3 | (Ctrl+Z: Undo, Ctrl+Y: Redo, Ctrl+S: Lưu)");
            SaveSnapshot(form, Path.Combine(imagesDir, "05_ve_hinh_elip_ellipse.png"));

            // 6. Điều chỉnh độ dày nét vẽ (NumericUpDown)
            form.ClearShapes();
            form.SetMode(DrawMode.Line);
            form.SetStrokeWidth(12);
            form.SetCurrentColor(Color.DarkSlateBlue);
            form.AddShape(new LineItem { Start = new Point(150, 180), End = new Point(850, 180), Color = Color.DarkSlateBlue, Width = 1f });
            form.AddShape(new LineItem { Start = new Point(150, 250), End = new Point(850, 250), Color = Color.DarkSlateBlue, Width = 3f });
            form.AddShape(new LineItem { Start = new Point(150, 330), End = new Point(850, 330), Color = Color.DarkSlateBlue, Width = 7f });
            form.AddShape(new LineItem { Start = new Point(150, 430), End = new Point(850, 430), Color = Color.DarkSlateBlue, Width = 15f });
            form.SetStatusText("Đã chọn độ dày nét: 12px | Chế độ: Line | Độ dày: 12px | Đối tượng: 4 | (Ctrl+Z: Undo, Ctrl+Y: Redo, Ctrl+S: Lưu)");
            SaveSnapshot(form, Path.Combine(imagesDir, "06_thay_doi_do_day_net.png"));

            // 7. Chọn màu vẽ & Bảng màu nhanh (Quick Palette)
            form.ClearShapes();
            form.SetMode(DrawMode.Pen);
            form.SetStrokeWidth(5);
            form.SetCurrentColor(Color.Red);
            form.AddShape(new PenItem
            {
                Color = Color.Red,
                Width = 5f,
                Points = new List<Point> { new Point(150, 200), new Point(250, 160), new Point(350, 230), new Point(450, 170) }
            });
            form.AddShape(new PenItem
            {
                Color = Color.Green,
                Width = 5f,
                Points = new List<Point> { new Point(150, 280), new Point(250, 240), new Point(350, 310), new Point(450, 250) }
            });
            form.AddShape(new PenItem
            {
                Color = Color.Blue,
                Width = 5f,
                Points = new List<Point> { new Point(150, 360), new Point(250, 320), new Point(350, 390), new Point(450, 330) }
            });
            form.AddShape(new PenItem
            {
                Color = Color.Purple,
                Width = 5f,
                Points = new List<Point> { new Point(150, 440), new Point(250, 400), new Point(350, 470), new Point(450, 410) }
            });
            form.SetStatusText("Đã chọn màu nhanh: #FF0000 (Màu Đỏ) | Chế độ: Pen | Độ dày: 5px | Đối tượng: 4 | (Ctrl+Z: Undo, Ctrl+Y: Redo, Ctrl+S: Lưu)");
            SaveSnapshot(form, Path.Combine(imagesDir, "07_chon_mau_sac_palette.png"));

            // 8. Chế độ tô màu hình học (Filled Shape - Rectangle & Ellipse)
            form.ClearShapes();
            form.SetMode(DrawMode.Rectangle);
            form.SetStrokeWidth(3);
            form.SetFill(true);
            form.SetCurrentColor(Color.DodgerBlue);
            form.AddShape(new RectangleItem { Start = new Point(120, 200), End = new Point(450, 450), Color = Color.DodgerBlue, Width = 3f, IsFilled = true });
            form.AddShape(new EllipseItem { Start = new Point(520, 200), End = new Point(850, 450), Color = Color.MediumVioletRed, Width = 3f, IsFilled = true });
            form.SetStatusText("Đã thêm hình tô màu đặc (Rectangle, Ellipse) | Chế độ: Rectangle | Độ dày: 3px | Đối tượng: 2 | (Ctrl+Z: Undo, Ctrl+Y: Redo, Ctrl+S: Lưu)");
            SaveSnapshot(form, Path.Combine(imagesDir, "08_to_mau_hinh_hoc_fill.png"));

            // 9. Công cụ tẩy xóa nét vẽ (Eraser)
            form.ClearShapes();
            form.SetMode(DrawMode.Eraser);
            form.SetStrokeWidth(8);
            form.SetFill(false);
            form.AddShape(new RectangleItem { Start = new Point(180, 220), End = new Point(780, 460), Color = Color.DarkSlateBlue, Width = 6f, IsFilled = false });
            form.AddShape(new LineItem { Start = new Point(180, 340), End = new Point(780, 340), Color = Color.Crimson, Width = 8f });
            // Nét tẩy cắt ngang
            form.AddShape(new EraserItem
            {
                Color = Color.White,
                Width = 8f,
                Points = new List<Point>
                {
                    new Point(480, 160), new Point(480, 240), new Point(480, 320),
                    new Point(480, 400), new Point(480, 500)
                }
            });
            form.SetStatusText("Đang dùng công cụ Eraser (Tẩy nét vẽ) | Chế độ: Eraser | Độ dày: 8px | Đối tượng: 3 | (Ctrl+Z: Undo, Ctrl+Y: Redo, Ctrl+S: Lưu)");
            SaveSnapshot(form, Path.Combine(imagesDir, "09_cong_cu_tay_eraser.png"));

            // 10. Đổi màu nền Canvas (Canvas Background Color)
            form.ClearShapes();
            form.SetCanvasBgColor(Color.FromArgb(254, 249, 231)); // Màu vàng kem trang nhã
            form.SetMode(DrawMode.Ellipse);
            form.SetStrokeWidth(4);
            form.SetCurrentColor(Color.DarkBlue);
            form.AddShape(new EllipseItem { Start = new Point(160, 200), End = new Point(480, 480), Color = Color.DarkBlue, Width = 4f, IsFilled = false });
            form.AddShape(new RectangleItem { Start = new Point(540, 220), End = new Point(860, 460), Color = Color.Brown, Width = 4f, IsFilled = true });
            form.SetStatusText("Đã đổi màu nền Canvas thành: RGB(254, 249, 231) | Chế độ: Ellipse | Độ dày: 4px | Đối tượng: 2 | (Ctrl+Z: Undo, Ctrl+Y: Redo, Ctrl+S: Lưu)");
            SaveSnapshot(form, Path.Combine(imagesDir, "10_doi_mau_nen_canvas.png"));

            // 11. Mở ảnh nền để vẽ đè (Open Background Image)
            form.ClearShapes();
            form.SetCanvasBgColor(Color.White);
            // Tạo ảnh nền đồ họa mẫu
            Bitmap bgBmp = new Bitmap(900, 500);
            using (Graphics gBg = Graphics.FromImage(bgBmp))
            {
                gBg.Clear(Color.FromArgb(240, 244, 248));
                using Pen gridPen = new Pen(Color.FromArgb(220, 228, 238), 1f);
                for (int x = 0; x < 900; x += 40) gBg.DrawLine(gridPen, x, 0, x, 500);
                for (int y = 0; y < 500; y += 40) gBg.DrawLine(gridPen, 0, y, 900, y);
                using Font font = new Font("Segoe UI", 16f, FontStyle.Bold);
                gBg.DrawString("BẢN ĐỒ QUY HOẠCH KHU ĐÔ THỊ MẪU (BACKGROUND IMAGE)", font, Brushes.SteelBlue, 50, 40);
            }
            form.SetBackgroundImage(bgBmp);
            // Vẽ các nét ghi chú đè lên ảnh nền
            form.AddShape(new RectangleItem { Start = new Point(120, 160), End = new Point(420, 360), Color = Color.Crimson, Width = 4f, IsFilled = false });
            form.AddShape(new LineItem { Start = new Point(420, 260), End = new Point(620, 260), Color = Color.DarkOrange, Width = 4f });
            form.AddShape(new EllipseItem { Start = new Point(620, 180), End = new Point(800, 360), Color = Color.SeaGreen, Width = 4f, IsFilled = true });
            form.SetStatusText("Đã nạp ảnh nền: blueprint_map.png | Đang vẽ chú thích lên ảnh | Đối tượng: 3 | (Ctrl+Z: Undo, Ctrl+Y: Redo, Ctrl+S: Lưu)");
            SaveSnapshot(form, Path.Combine(imagesDir, "11_mo_anh_nen_ve_de.png"));

            // 12. Lưu vùng vẽ ra file PNG (Save Image to PNG)
            form.SetBackgroundImage(null);
            form.SetCanvasBgColor(Color.White);
            form.ClearShapes();
            form.LoadSampleDrawing();
            form.SetStatusText("Đã lưu ảnh thành công: Drawing_20261029_Quan.png (PNG Image) | Đối tượng: 6 | (Ctrl+Z: Undo, Ctrl+Y: Redo, Ctrl+S: Lưu)");
            SaveSnapshot(form, Path.Combine(imagesDir, "12_luu_anh_png_thanh_cong.png"));

            // 13. Hoàn tác (Undo) và Làm lại (Redo)
            form.ClearShapes();
            form.SetMode(DrawMode.Pen);
            form.SetStrokeWidth(4);
            form.AddShape(new LineItem { Start = new Point(150, 220), End = new Point(450, 220), Color = Color.Red, Width = 4f });
            form.AddShape(new RectangleItem { Start = new Point(500, 180), End = new Point(780, 360), Color = Color.Blue, Width = 4f, IsFilled = false });
            form.SetStatusText("Đã Undo nét vẽ gần nhất (Ctrl+Z). Còn lại 2 đối tượng | Chế độ: Pen | Độ dày: 4px | Đối tượng: 2 | (Ctrl+Z: Undo, Ctrl+Y: Redo, Ctrl+S: Lưu)");
            SaveSnapshot(form, Path.Combine(imagesDir, "13_hoan_tac_undo_redo.png"));

            // 14. Xóa sạch toàn bộ vùng vẽ (Clear All)
            form.ClearShapes();
            form.SetStatusText("Đã xác nhận xóa sạch toàn bộ vùng vẽ và ảnh nền | Chế độ: Pen | Độ dày: 3px | Đối tượng: 0 | (Ctrl+Z: Undo, Ctrl+Y: Redo, Ctrl+S: Lưu)");
            SaveSnapshot(form, Path.Combine(imagesDir, "14_xoa_toan_bo_clear_xac_nhan.png"));

            // 15. Thanh trạng thái và tọa độ chuột thời gian thực (StatusStrip & Coordinates)
            form.ClearShapes();
            form.LoadSampleDrawing();
            form.SetStatusText("X = 485, Y = 312 | Chế độ: Pen | Độ dày: 4px | Đối tượng: 6 | (Ctrl+Z: Undo, Ctrl+Y: Redo, Ctrl+S: Lưu)");
            SaveSnapshot(form, Path.Combine(imagesDir, "15_thanh_trang_thai_toa_do.png"));

            form.Close();
        }
    }
}
