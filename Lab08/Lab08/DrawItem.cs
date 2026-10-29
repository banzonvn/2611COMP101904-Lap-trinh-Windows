using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace COMP1019_Lab08_SimplePaint
{
    /// <summary>
    /// Lớp cơ sở trừu tượng đại diện cho một đối tượng vẽ (DrawItem).
    /// Áp dụng tính đa hình (Polymorphism) thay vì switch-case thông thường,
    /// giúp mở rộng thêm các loại hình vẽ mới dễ dàng theo nguyên lý Open/Closed (SOLID).
    /// </summary>
    public abstract class DrawItem
    {
        public DrawMode Mode { get; set; }
        public Point Start { get; set; }
        public Point End { get; set; }
        public Color Color { get; set; } = Color.Black;
        public float Width { get; set; } = 3.0f;
        public bool IsFilled { get; set; } = false;
        public List<Point> Points { get; set; } = new List<Point>();

        /// <summary>
        /// Phương thức vẽ trừu tượng, mỗi hình con sẽ tự thực thi cách vẽ riêng của mình.
        /// </summary>
        public abstract void Draw(Graphics g);

        /// <summary>
        /// Chuẩn hóa tọa độ hình chữ nhật bao quanh điểm Start và End.
        /// Xử lý triệt để trường hợp người dùng kéo chuột ngược hướng (phải sang trái, dưới lên trên).
        /// </summary>
        protected Rectangle GetNormalizedRectangle()
        {
            int x = Math.Min(Start.X, End.X);
            int y = Math.Min(Start.Y, End.Y);
            int width = Math.Abs(Start.X - End.X);
            int height = Math.Abs(Start.Y - End.Y);
            return new Rectangle(x, y, Math.Max(1, width), Math.Max(1, height));
        }

        /// <summary>
        /// Factory Method để khởi tạo đối tượng hình vẽ tương ứng với chế độ được chọn.
        /// </summary>
        public static DrawItem Create(DrawMode mode, Point startPoint, Color color, float width, bool isFilled)
        {
            DrawItem item = mode switch
            {
                DrawMode.Pen => new PenItem(),
                DrawMode.Line => new LineItem(),
                DrawMode.Rectangle => new RectangleItem(),
                DrawMode.Ellipse => new EllipseItem(),
                DrawMode.Eraser => new EraserItem(),
                _ => new PenItem()
            };

            item.Start = startPoint;
            item.End = startPoint;
            item.Color = color;
            item.Width = width;
            item.IsFilled = isFilled;

            if (mode == DrawMode.Pen || mode == DrawMode.Eraser)
            {
                item.Points.Add(startPoint);
            }

            return item;
        }
    }

    /// <summary>
    /// Nét bút vẽ tự do (Pen)
    /// </summary>
    public class PenItem : DrawItem
    {
        public PenItem()
        {
            Mode = DrawMode.Pen;
        }

        public override void Draw(Graphics g)
        {
            if (Points == null || Points.Count == 0) return;

            // Dùng using để đảm bảo giải phóng Pen ngay sau khi vẽ
            using Pen pen = new Pen(Color, Width)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round,
                LineJoin = LineJoin.Round
            };

            if (Points.Count == 1)
            {
                // Khi vừa chấm chuột một điểm
                using Brush brush = new SolidBrush(Color);
                float radius = Width / 2f;
                g.FillEllipse(brush, Points[0].X - radius, Points[0].Y - radius, Width, Width);
                return;
            }

            // Nối liên tiếp các điểm để tạo nét vẽ mượt mà, không đứt đoạn
            for (int i = 1; i < Points.Count; i++)
            {
                g.DrawLine(pen, Points[i - 1], Points[i]);
            }
        }
    }

    /// <summary>
    /// Đường thẳng (Line)
    /// </summary>
    public class LineItem : DrawItem
    {
        public LineItem()
        {
            Mode = DrawMode.Line;
        }

        public override void Draw(Graphics g)
        {
            using Pen pen = new Pen(Color, Width)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            };
            g.DrawLine(pen, Start, End);
        }
    }

    /// <summary>
    /// Hình chữ nhật (Rectangle), hỗ trợ cả chế độ viền và tô màu
    /// </summary>
    public class RectangleItem : DrawItem
    {
        public RectangleItem()
        {
            Mode = DrawMode.Rectangle;
        }

        public override void Draw(Graphics g)
        {
            Rectangle rect = GetNormalizedRectangle();

            // Nếu bật chế độ tô màu (chức năng nâng cao)
            if (IsFilled)
            {
                using Brush brush = new SolidBrush(Color);
                g.FillRectangle(brush, rect);
            }

            using Pen pen = new Pen(Color, Width)
            {
                LineJoin = LineJoin.Miter
            };
            g.DrawRectangle(pen, rect);
        }
    }

    /// <summary>
    /// Hình Elip / Hình tròn (Ellipse), hỗ trợ cả chế độ viền và tô màu
    /// </summary>
    public class EllipseItem : DrawItem
    {
        public EllipseItem()
        {
            Mode = DrawMode.Ellipse;
        }

        public override void Draw(Graphics g)
        {
            Rectangle rect = GetNormalizedRectangle();

            // Nếu bật chế độ tô màu (chức năng nâng cao)
            if (IsFilled)
            {
                using Brush brush = new SolidBrush(Color);
                g.FillEllipse(brush, rect);
            }

            using Pen pen = new Pen(Color, Width);
            g.DrawEllipse(pen, rect);
        }
    }

    /// <summary>
    /// Cục tẩy (Eraser) - vẽ bằng màu nền hiện tại của Canvas
    /// </summary>
    public class EraserItem : DrawItem
    {
        public EraserItem()
        {
            Mode = DrawMode.Eraser;
        }

        public override void Draw(Graphics g)
        {
            if (Points == null || Points.Count == 0) return;

            using Pen pen = new Pen(Color, Width * 2.5f) // Nét tẩy to gấp 2.5 lần nét bút cho tiện tẩy
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round,
                LineJoin = LineJoin.Round
            };

            if (Points.Count == 1)
            {
                using Brush brush = new SolidBrush(Color);
                float radius = (Width * 2.5f) / 2f;
                g.FillEllipse(brush, Points[0].X - radius, Points[0].Y - radius, Width * 2.5f, Width * 2.5f);
                return;
            }

            for (int i = 1; i < Points.Count; i++)
            {
                g.DrawLine(pen, Points[i - 1], Points[i]);
            }
        }
    }
}
