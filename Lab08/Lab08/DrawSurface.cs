using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace COMP1019_Lab08_SimplePaint
{
    /// <summary>
    /// Vùng vẽ Canvas tùy biến kế thừa từ Panel, bật DoubleBuffered để triệt tiêu hiện tượng nhấp nháy (flickering).
    /// </summary>
    public class DrawSurface : Panel
    {
        public DrawSurface()
        {
            // Bật DoubleBuffered và các style vẽ tối ưu
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            UpdateStyles();

            BackColor = Color.White;
            Dock = DockStyle.Fill;
            Cursor = Cursors.Cross;
        }

        /// <summary>
        /// Ảnh nền được tải vào để vẽ đè (chức năng nâng cao).
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Bitmap? BackgroundImageBitmap { get; set; }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            // Nếu có ảnh nền, vẽ ảnh nền lên trước khi các nét vẽ đè lên
            if (BackgroundImageBitmap != null)
            {
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                e.Graphics.DrawImage(BackgroundImageBitmap, 0, 0, Width, Height);
            }
        }
    }
}
