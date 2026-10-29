using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace COMP1019_Lab08_SimplePaint
{
    public class FrmPaint : Form
    {
        // Các control bắt buộc theo yêu cầu đề bài
        private readonly DrawSurface canvas = new DrawSurface();
        private readonly ComboBox cboMode = new ComboBox();
        private readonly NumericUpDown nudWidth = new NumericUpDown();
        private readonly Button btnColor = new Button();
        private readonly Button btnUndo = new Button();
        private readonly Button btnClear = new Button();
        private readonly Label lblStatus = new Label();

        // Các control nâng cao & bổ sung giao diện hiện đại
        private readonly Button btnRedo = new Button();
        private readonly Button btnSave = new Button();
        private readonly Button btnOpen = new Button();
        private readonly Button btnBgColor = new Button();
        private readonly CheckBox chkFill = new CheckBox();
        private readonly Panel pnlColorPreview = new Panel();
        private readonly FlowLayoutPanel flpPalette = new FlowLayoutPanel();

        // Danh sách lưu trữ nét vẽ và ngăn xếp Undo/Redo
        private readonly List<DrawItem> items = new List<DrawItem>();
        private readonly Stack<DrawItem> redoStack = new Stack<DrawItem>();
        private DrawItem? currentItem;
        private bool isDrawing = false;
        private Color currentColor = Color.Black;

        public FrmPaint()
        {
            // Thiết lập thông tin Form chính
            Text = "COMP1019 - Simple Paint Studio [Lê Hoàng Quân - MSSV: 51.01.104.082]";
            Width = 1150;
            Height = 720;
            MinimumSize = new Size(950, 600);
            StartPosition = FormStartPosition.CenterScreen;
            KeyPreview = true; // Bật để nhận diện phím tắt Ctrl+Z, Ctrl+Y, Ctrl+S

            BuildUserInterface();
            RegisterEvents();
            UpdateStatus("Sẵn sàng");
        }

        /// <summary>
        /// Khởi tạo và bố cục giao diện người dùng theo phong cách Studio chuyên nghiệp
        /// </summary>
        private void BuildUserInterface()
        {
            // Toolbar trên cùng chứa các cụm chức năng
            Panel topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = Color.FromArgb(245, 247, 250),
                Padding = new Padding(10, 6, 10, 6)
            };

            // Đường viền ngăn cách toolbar và canvas
            Panel divider = new Panel
            {
                Dock = DockStyle.Top,
                Height = 1,
                BackColor = Color.FromArgb(215, 220, 228)
            };

            // 1. Nhóm Chế độ vẽ (cboMode)
            Label lblMode = new Label
            {
                Text = "Công cụ:",
                AutoSize = true,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Location = new Point(12, 10)
            };

            cboMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMode.Font = new Font("Segoe UI", 9f);
            cboMode.Items.AddRange(new object[] { "Pen", "Line", "Rectangle", "Ellipse", "Eraser" });
            cboMode.SelectedIndex = 0;
            cboMode.Width = 105;
            cboMode.Location = new Point(12, 30);

            // 2. Nhóm Độ dày nét (nudWidth) & Tô màu (chkFill)
            Label lblWidth = new Label
            {
                Text = "Độ dày:",
                AutoSize = true,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Location = new Point(128, 10)
            };

            nudWidth.Minimum = 1;
            nudWidth.Maximum = 20;
            nudWidth.Value = 3;
            nudWidth.Width = 55;
            nudWidth.Font = new Font("Segoe UI", 9f);
            nudWidth.Location = new Point(128, 30);

            chkFill.Text = "Tô màu (Fill)";
            chkFill.AutoSize = true;
            chkFill.Font = new Font("Segoe UI", 9f);
            chkFill.Location = new Point(190, 32);

            // 3. Nhóm Màu sắc (btnColor, Preview & Bảng màu nhanh)
            btnColor.Text = "Chọn màu...";
            btnColor.Font = new Font("Segoe UI", 9f);
            btnColor.Size = new Size(88, 27);
            btnColor.Location = new Point(298, 28);
            btnColor.Cursor = Cursors.Hand;

            pnlColorPreview.Size = new Size(26, 26);
            pnlColorPreview.Location = new Point(392, 29);
            pnlColorPreview.BackColor = currentColor;
            pnlColorPreview.BorderStyle = BorderStyle.FixedSingle;

            // Bảng 10 màu nhanh tiện dụng (Quick Palette)
            flpPalette.Size = new Size(160, 52);
            flpPalette.Location = new Point(425, 6);
            flpPalette.Margin = new Padding(0);
            flpPalette.Padding = new Padding(0);

            Color[] quickColors = new[]
            {
                Color.Black, Color.White, Color.Red, Color.Orange, Color.Yellow,
                Color.Green, Color.DeepSkyBlue, Color.Blue, Color.Purple, Color.Gray
            };

            foreach (Color col in quickColors)
            {
                Panel pnlBox = new Panel
                {
                    Size = new Size(24, 22),
                    BackColor = col,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(2),
                    Cursor = Cursors.Hand
                };
                pnlBox.Click += (s, e) =>
                {
                    currentColor = col;
                    pnlColorPreview.BackColor = currentColor;
                    UpdateStatus($"Đã chọn màu nhanh: {ColorTranslator.ToHtml(col)}");
                };
                flpPalette.Controls.Add(pnlBox);
            }

            // 4. Nhóm Tệp & Nền Canvas (Open, Save, BgColor)
            btnOpen.Text = "Mở ảnh";
            btnOpen.Font = new Font("Segoe UI", 8.5f);
            btnOpen.Size = new Size(68, 27);
            btnOpen.Location = new Point(595, 28);
            btnOpen.Cursor = Cursors.Hand;

            btnSave.Text = "Lưu PNG";
            btnSave.Font = new Font("Segoe UI", 8.5f);
            btnSave.Size = new Size(72, 27);
            btnSave.Location = new Point(668, 28);
            btnSave.Cursor = Cursors.Hand;

            btnBgColor.Text = "Màu nền";
            btnBgColor.Font = new Font("Segoe UI", 8.5f);
            btnBgColor.Size = new Size(72, 27);
            btnBgColor.Location = new Point(745, 28);
            btnBgColor.Cursor = Cursors.Hand;

            // 5. Nhóm Lịch sử nét vẽ (Undo, Redo, Clear)
            btnUndo.Text = "Undo (Ctrl+Z)";
            btnUndo.Font = new Font("Segoe UI", 8.5f);
            btnUndo.Size = new Size(92, 27);
            btnUndo.Location = new Point(827, 28);
            btnUndo.Cursor = Cursors.Hand;

            btnRedo.Text = "Redo (Ctrl+Y)";
            btnRedo.Font = new Font("Segoe UI", 8.5f);
            btnRedo.Size = new Size(92, 27);
            btnRedo.Location = new Point(924, 28);
            btnRedo.Cursor = Cursors.Hand;

            btnClear.Text = "Clear All";
            btnClear.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            btnClear.ForeColor = Color.DarkRed;
            btnClear.Size = new Size(75, 27);
            btnClear.Location = new Point(1021, 28);
            btnClear.Cursor = Cursors.Hand;

            // Đưa các control vào topPanel
            topPanel.Controls.Add(lblMode);
            topPanel.Controls.Add(cboMode);
            topPanel.Controls.Add(lblWidth);
            topPanel.Controls.Add(nudWidth);
            topPanel.Controls.Add(chkFill);
            topPanel.Controls.Add(btnColor);
            topPanel.Controls.Add(pnlColorPreview);
            topPanel.Controls.Add(flpPalette);
            topPanel.Controls.Add(btnOpen);
            topPanel.Controls.Add(btnSave);
            topPanel.Controls.Add(btnBgColor);
            topPanel.Controls.Add(btnUndo);
            topPanel.Controls.Add(btnRedo);
            topPanel.Controls.Add(btnClear);

            // Status bar phía dưới
            lblStatus.Dock = DockStyle.Bottom;
            lblStatus.Height = 28;
            lblStatus.BackColor = Color.FromArgb(238, 240, 244);
            lblStatus.Font = new Font("Segoe UI", 9f);
            lblStatus.Padding = new Padding(12, 4, 12, 4);
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;

            // Thêm các control vào Form chính (Thứ tự thêm Dock để canvas chiếm toàn bộ vùng giữa)
            Controls.Add(canvas);
            Controls.Add(divider);
            Controls.Add(topPanel);
            Controls.Add(lblStatus);
        }

        /// <summary>
        /// Đăng ký các sự kiện chuột, nút bấm và phím tắt
        /// </summary>
        private void RegisterEvents()
        {
            // Sự kiện Canvas
            canvas.Paint += Canvas_Paint;
            canvas.MouseDown += Canvas_MouseDown;
            canvas.MouseMove += Canvas_MouseMove;
            canvas.MouseUp += Canvas_MouseUp;
            canvas.MouseLeave += Canvas_MouseLeave;

            // Sự kiện các nút bấm
            btnColor.Click += BtnColor_Click;
            btnBgColor.Click += BtnBgColor_Click;
            btnUndo.Click += BtnUndo_Click;
            btnRedo.Click += BtnRedo_Click;
            btnClear.Click += BtnClear_Click;
            btnSave.Click += BtnSave_Click;
            btnOpen.Click += BtnOpen_Click;

            // Sự kiện phím tắt toàn Form
            KeyDown += FrmPaint_KeyDown;
        }

        private DrawMode CurrentMode
        {
            get
            {
                string modeStr = cboMode.SelectedItem?.ToString() ?? "Pen";
                return Enum.TryParse(modeStr, out DrawMode mode) ? mode : DrawMode.Pen;
            }
        }

        #region Xử lý Sự kiện Vẽ trên Canvas

        private void Canvas_MouseDown(object? sender, MouseEventArgs e)
        {
            // Chỉ chấp nhận vẽ bằng chuột trái, bỏ qua chuột phải theo rubric
            if (e.Button != MouseButtons.Left) return;

            isDrawing = true;
            redoStack.Clear(); // Bắt đầu nét vẽ mới thì làm rỗng ngăn xếp Redo

            DrawMode mode = CurrentMode;
            Color drawColor = (mode == DrawMode.Eraser) ? canvas.BackColor : currentColor;
            float strokeWidth = (float)nudWidth.Value;
            bool fill = chkFill.Checked;

            // Sử dụng Factory Pattern đa hình để tạo đúng hình vẽ
            currentItem = DrawItem.Create(mode, e.Location, drawColor, strokeWidth, fill);

            UpdateStatus($"Đang vẽ {mode} tại ({e.X}, {e.Y})");
        }

        private void Canvas_MouseMove(object? sender, MouseEventArgs e)
        {
            // Cập nhật tọa độ chuột liên tục trên Status Label
            UpdateMouseCoordinates(e.X, e.Y);

            if (!isDrawing || currentItem == null) return;

            currentItem.End = e.Location;

            // Nếu là Pen hoặc Eraser thì bổ sung điểm vào danh sách tọa độ
            if (currentItem.Mode == DrawMode.Pen || currentItem.Mode == DrawMode.Eraser)
            {
                currentItem.Points.Add(e.Location);
            }

            // Yêu cầu Windows vẽ lại vùng canvas qua Invalidate()
            canvas.Invalidate();
        }

        private void Canvas_MouseUp(object? sender, MouseEventArgs e)
        {
            if (!isDrawing || currentItem == null) return;

            currentItem.End = e.Location;

            if (currentItem.Mode == DrawMode.Pen || currentItem.Mode == DrawMode.Eraser)
            {
                currentItem.Points.Add(e.Location);
            }

            // Lưu nét vẽ hoàn chỉnh vào danh sách gốc
            items.Add(currentItem);
            currentItem = null;
            isDrawing = false;

            canvas.Invalidate();
            UpdateStatus($"Đã thêm nét vẽ ({CurrentMode}). Tổng số đối tượng: {items.Count}");
        }

        private void Canvas_MouseLeave(object? sender, EventArgs e)
        {
            UpdateStatus("Sẵn sàng");
        }

        private void Canvas_Paint(object? sender, PaintEventArgs e)
        {
            // Bật Anti-Aliasing để nét vẽ mịn đẹp, không răng cưa
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // 1. Vẽ tất cả các đối tượng đã lưu trong danh sách gốc (Polymorphism - không cần switch-case)
            foreach (DrawItem item in items)
            {
                item.Draw(e.Graphics);
            }

            // 2. Vẽ đối tượng tạm thời đang kéo chuột (preview trực tiếp)
            if (currentItem != null)
            {
                currentItem.Draw(e.Graphics);
            }
        }

        #endregion

        #region Xử lý Thao tác nút bấm & Chức năng nâng cao

        private void BtnColor_Click(object? sender, EventArgs e)
        {
            using ColorDialog dialog = new ColorDialog
            {
                Color = currentColor,
                FullOpen = true
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                currentColor = dialog.Color;
                pnlColorPreview.BackColor = currentColor;
                UpdateStatus($"Đã chọn màu mới: RGB({currentColor.R}, {currentColor.G}, {currentColor.B})");
            }
        }

        private void BtnBgColor_Click(object? sender, EventArgs e)
        {
            using ColorDialog dialog = new ColorDialog
            {
                Color = canvas.BackColor,
                FullOpen = true
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                canvas.BackColor = dialog.Color;
                canvas.Invalidate();
                UpdateStatus($"Đã đổi màu nền Canvas thành: RGB({dialog.Color.R}, {dialog.Color.G}, {dialog.Color.B})");
            }
        }

        private void BtnUndo_Click(object? sender, EventArgs e)
        {
            PerformUndo();
        }

        private void BtnRedo_Click(object? sender, EventArgs e)
        {
            PerformRedo();
        }

        private void PerformUndo()
        {
            if (items.Count == 0)
            {
                UpdateStatus("Không có nét vẽ nào để Undo!");
                return;
            }

            DrawItem lastItem = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);
            redoStack.Push(lastItem);

            canvas.Invalidate();
            UpdateStatus($"Đã Undo nét vẽ gần nhất. Còn lại {items.Count} đối tượng.");
        }

        private void PerformRedo()
        {
            if (redoStack.Count == 0)
            {
                UpdateStatus("Không có thao tác nào để Redo!");
                return;
            }

            DrawItem item = redoStack.Pop();
            items.Add(item);

            canvas.Invalidate();
            UpdateStatus($"Đã Redo nét vẽ ({item.Mode}). Tổng số đối tượng: {items.Count}.");
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            if (items.Count == 0 && canvas.BackgroundImageBitmap == null)
            {
                UpdateStatus("Vùng vẽ đang trống!");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa toàn bộ vùng vẽ và ảnh nền không?",
                "Xác nhận xóa vùng vẽ",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                items.Clear();
                redoStack.Clear();
                canvas.BackgroundImageBitmap?.Dispose();
                canvas.BackgroundImageBitmap = null;

                canvas.Invalidate();
                UpdateStatus("Đã xóa sạch toàn bộ vùng vẽ.");
            }
        }

        /// <summary>
        /// Chức năng nâng cao: Xuất toàn bộ vùng vẽ ra file ảnh PNG chất lượng cao
        /// </summary>
        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (canvas.Width <= 0 || canvas.Height <= 0) return;

            using SaveFileDialog sfd = new SaveFileDialog
            {
                Title = "Lưu bản vẽ ra file PNG",
                Filter = "PNG Image (*.png)|*.png|JPEG Image (*.jpg)|*.jpg|Bitmap Image (*.bmp)|*.bmp",
                FileName = $"Drawing_{DateTime.Now:yyyyMMdd_HHmmss}.png"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Tạo một Bitmap ngoài bộ nhớ đệm đúng kích thước Canvas
                    using Bitmap bmp = new Bitmap(canvas.Width, canvas.Height);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;

                        // 1. Vẽ nền Canvas
                        using (Brush bgBrush = new SolidBrush(canvas.BackColor))
                        {
                            g.FillRectangle(bgBrush, 0, 0, canvas.Width, canvas.Height);
                        }

                        // 2. Vẽ ảnh nền (nếu có)
                        if (canvas.BackgroundImageBitmap != null)
                        {
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.DrawImage(canvas.BackgroundImageBitmap, 0, 0, canvas.Width, canvas.Height);
                        }

                        // 3. Vẽ tất cả các nét vẽ
                        foreach (DrawItem item in items)
                        {
                            item.Draw(g);
                        }
                    }

                    // Lưu theo đúng định dạng file người dùng chọn
                    ImageFormat format = Path.GetExtension(sfd.FileName).ToLower() switch
                    {
                        ".jpg" or ".jpeg" => ImageFormat.Jpeg,
                        ".bmp" => ImageFormat.Bmp,
                        _ => ImageFormat.Png
                    };

                    bmp.Save(sfd.FileName, format);
                    UpdateStatus($"Đã lưu ảnh thành công: {Path.GetFileName(sfd.FileName)}");
                    MessageBox.Show($"Lưu ảnh thành công tại:\n{sfd.FileName}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lưu ảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Chức năng nâng cao: Mở ảnh từ máy tính để làm nền và vẽ đè lên
        /// </summary>
        private void BtnOpen_Click(object? sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Mở ảnh làm nền vẽ",
                Filter = "Hình ảnh (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|All Files (*.*)|*.*"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Giải phóng ảnh nền cũ nếu có
                    canvas.BackgroundImageBitmap?.Dispose();

                    // Đọc ảnh và clone vào Bitmap mới để không bị khóa file (File Lock)
                    using Image loadedImg = Image.FromFile(ofd.FileName);
                    canvas.BackgroundImageBitmap = new Bitmap(loadedImg);

                    canvas.Invalidate();
                    UpdateStatus($"Đã mở ảnh nền: {Path.GetFileName(ofd.FileName)} ({canvas.BackgroundImageBitmap.Width}x{canvas.BackgroundImageBitmap.Height})");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không thể mở file ảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Bắt các phím tắt phổ biến: Ctrl+Z (Undo), Ctrl+Y (Redo), Ctrl+S (Save), Ctrl+O (Open)
        /// </summary>
        private void FrmPaint_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z)
            {
                PerformUndo();
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.Y)
            {
                PerformRedo();
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                BtnSave_Click(sender, EventArgs.Empty);
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.O)
            {
                BtnOpen_Click(sender, EventArgs.Empty);
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.N)
            {
                BtnClear_Click(sender, EventArgs.Empty);
                e.SuppressKeyPress = true;
            }
        }

        #endregion

        #region Dữ liệu mẫu kiểm thử

        /// <summary>
        /// Nạp các hình vẽ mẫu đại diện cho 4 loại hình bắt buộc (Pen, Line, Rectangle, Ellipse)
        /// và các hình tô màu nâng cao để kiểm thử và chụp ảnh màn hình nộp bài.
        /// </summary>
        public void LoadSampleDrawing()
        {
            // 1. Nét vẽ tự do (Pen) uốn lượn mềm mại
            PenItem pen = new PenItem
            {
                Color = Color.DarkSlateBlue,
                Width = 4.0f,
                Points = new List<Point>
                {
                    new Point(80, 240),
                    new Point(120, 180),
                    new Point(170, 150),
                    new Point(230, 200),
                    new Point(290, 270),
                    new Point(350, 210),
                    new Point(410, 160),
                    new Point(470, 200)
                }
            };
            items.Add(pen);

            // 2. Đoạn thẳng (Line)
            LineItem line = new LineItem
            {
                Start = new Point(540, 130),
                End = new Point(860, 220),
                Color = Color.Crimson,
                Width = 5.0f
            };
            items.Add(line);

            // 3. Hình chữ nhật (Rectangle) viền xanh lá
            RectangleItem rect = new RectangleItem
            {
                Start = new Point(90, 310),
                End = new Point(340, 480),
                Color = Color.SeaGreen,
                Width = 4.0f,
                IsFilled = false
            };
            items.Add(rect);

            // 4. Hình elip (Ellipse) viền màu cam
            EllipseItem ellipse = new EllipseItem
            {
                Start = new Point(390, 300),
                End = new Point(620, 500),
                Color = Color.DarkOrange,
                Width = 4.0f,
                IsFilled = false
            };
            items.Add(ellipse);

            // 5. Hình chữ nhật tô màu (Filled Rectangle - Tính năng nâng cao)
            RectangleItem filledRect = new RectangleItem
            {
                Start = new Point(670, 310),
                End = new Point(860, 420),
                Color = Color.DodgerBlue,
                Width = 2.0f,
                IsFilled = true
            };
            items.Add(filledRect);

            // 6. Hình elip tô màu (Filled Ellipse - Tính năng nâng cao)
            EllipseItem filledEllipse = new EllipseItem
            {
                Start = new Point(720, 390),
                End = new Point(930, 520),
                Color = Color.MediumVioletRed,
                Width = 2.0f,
                IsFilled = true
            };
            items.Add(filledEllipse);

            canvas.Invalidate();
            UpdateStatus("Đã tải dữ liệu kiểm thử (Pen, Line, Rectangle, Ellipse, Filled Shapes)");
        }

        #endregion

        #region Hỗ trợ hiển thị Trạng thái

        private void UpdateMouseCoordinates(int x, int y)
        {
            lblStatus.Text = $"X = {x}, Y = {y} | Chế độ: {CurrentMode} | Độ dày: {nudWidth.Value}px | Đối tượng: {items.Count} | (Ctrl+Z: Undo, Ctrl+Y: Redo, Ctrl+S: Lưu)";
        }

        private void UpdateStatus(string message)
        {
            lblStatus.Text = $"{message} | Chế độ: {CurrentMode} | Độ dày: {nudWidth.Value}px | Đối tượng: {items.Count} | (Ctrl+Z: Undo, Ctrl+Y: Redo, Ctrl+S: Lưu)";
        }

        #endregion
    }
}
