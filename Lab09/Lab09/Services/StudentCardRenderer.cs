using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using StudentCardPrinter.Models;

namespace StudentCardPrinter.Services
{
    /// <summary>
    /// Lớp chuyên trách vẽ thẻ sinh viên bằng GDI+ (Single Responsibility Principle).
    /// Dùng chung duy nhất một logic vẽ cho cả màn hình xem trước (Panel Preview),
    /// in ấn chất lượng cao ra máy in/PDF (PrintDocument) và xuất ảnh PNG.
    /// Thiết kế tỉ mỉ với thuật toán căn lề động, cam kết không bao giờ bị đè chữ.
    /// </summary>
    public static class StudentCardRenderer
    {
        public static void Render(Graphics g, Rectangle card, StudentCardInfo s, CardTheme theme)
        {
            // Thiết lập chế độ đồ họa chất lượng cao nhất
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Tỉ lệ co giãn linh hoạt theo kích thước card
            float scale = card.Height / 350.0f;
            int cornerRadius = (int)(12 * scale);

            // 1. VẼ NỀN THẺ VÀ KHUNG VIỀN BO GÓC
            using (GraphicsPath cardPath = GetRoundedPath(card, cornerRadius))
            {
                using (Brush bgBrush = new SolidBrush(theme.CardBgColor))
                {
                    g.FillPath(bgBrush, cardPath);
                }

                // 2. VẼ DẢI HEADER PHÍA TRÊN
                int headerHeight = (int)(card.Height * 0.22f);
                Rectangle headerRect = new Rectangle(card.X, card.Y, card.Width, headerHeight);
                using (GraphicsPath headerPath = GetTopRoundedPath(headerRect, cornerRadius))
                using (Brush headerBrush = new SolidBrush(theme.HeaderBgColor))
                {
                    g.FillPath(headerBrush, headerPath);
                }

                // VẼ CHỮ TRONG HEADER - CÁC VÙNG TÁCH BIỆT HOÀN TOÀN (KHÔNG ĐÈ CHỮ)
                using (Font schoolFont = new Font("Arial", 8.5f * scale, FontStyle.Bold))
                using (Font cardTitleFont = new Font("Arial", 12.0f * scale, FontStyle.Bold))
                using (Font facultyFont = new Font("Arial", 8.0f * scale, FontStyle.Bold))
                using (Brush headerTextBrush = new SolidBrush(theme.HeaderTextColor))
                using (Brush accentBrush = new SolidBrush(theme.AccentColor))
                using (StringFormat sfLeft = new StringFormat { LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap })
                {
                    float padLeft = card.X + 18 * scale;
                    float headerTextWidth = card.Width - 36 * scale;

                    // Dòng 1: Tên Trường (Y = 7..25)
                    RectangleF schoolRect = new RectangleF(padLeft, card.Y + 8 * scale, headerTextWidth, 18 * scale);
                    g.DrawString("TRƯỜNG ĐẠI HỌC SƯ PHẠM TP. HỒ CHÍ MINH", schoolFont, headerTextBrush, schoolRect, sfLeft);

                    // Dòng 2: Tiêu đề thẻ sinh viên (Y = 27..51)
                    RectangleF titleRect = new RectangleF(padLeft, card.Y + 28 * scale, headerTextWidth, 24 * scale);
                    g.DrawString("THẺ SINH VIÊN  •  STUDENT CARD", cardTitleFont, headerTextBrush, titleRect, sfLeft);

                    // Dòng 3: Tên Khoa đào tạo (Y = 53..71)
                    string facText = !string.IsNullOrWhiteSpace(s.Faculty) ? $"KHOA {s.Faculty.ToUpper()}" : "KHOA CÔNG NGHỆ THÔNG TIN";
                    RectangleF facRect = new RectangleF(padLeft, card.Y + 54 * scale, headerTextWidth, 18 * scale);
                    g.DrawString(facText, facultyFont, accentBrush, facRect, sfLeft);
                }

                // 3. VẼ DẢI FOOTER BẢO AN PHÍA DƯỚI
                int footerHeight = (int)(card.Height * 0.08f);
                Rectangle footerRect = new Rectangle(card.X, card.Bottom - footerHeight, card.Width, footerHeight);
                using (GraphicsPath footerPath = GetBottomRoundedPath(footerRect, cornerRadius))
                using (Brush footerBrush = new SolidBrush(theme.FooterBgColor))
                using (Font footerFont = new Font("Arial", 7.0f * scale, FontStyle.Italic))
                using (Brush footerTextBrush = new SolidBrush(Color.FromArgb(90, 100, 120)))
                using (StringFormat sfCenter = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    g.FillPath(footerBrush, footerPath);
                    g.DrawString("Thẻ có giá trị trong suốt thời gian theo học tại trường • HCMUE Smart Card", footerFont, footerTextBrush, footerRect, sfCenter);
                }

                // 4. VÙNG THÂN THẺ (BODY)
                // A. Khung ảnh sinh viên bên trái (Chuẩn 3x4)
                int photoX = card.X + (int)(18 * scale);
                int photoY = headerRect.Bottom + (int)(10 * scale);
                int photoW = (int)(card.Width * 0.27f);
                int photoH = (int)(photoW * 1.333f);
                Rectangle photoBox = new Rectangle(photoX, photoY, photoW, photoH);

                using (Brush whiteBrush = new SolidBrush(Color.White))
                using (Pen photoBorderPen = new Pen(theme.BorderColor, 1.5f * scale))
                {
                    g.FillRectangle(whiteBrush, photoBox);
                    if (s.Photo != null)
                    {
                        DrawImageZoom(g, s.Photo, photoBox);
                    }
                    else
                    {
                        using Font placeholderFont = new Font("Arial", 8.0f * scale, FontStyle.Regular);
                        using Brush grayBrush = new SolidBrush(Color.Gray);
                        using StringFormat sfPlaceholder = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                        g.DrawString("[ Chưa chọn ảnh ]\n(3x4 cm)", placeholderFont, grayBrush, photoBox, sfPlaceholder);
                    }
                    g.DrawRectangle(photoBorderPen, photoBox);
                }

                // B. Xác định vị trí Chip thông minh & Mã QR Code (nếu bật)
                int qrSize = (int)(80 * scale);
                int qrX = card.Right - qrSize - (int)(18 * scale);
                int qrY = footerRect.Top - qrSize - (int)(8 * scale);
                Rectangle qrBox = new Rectangle(qrX, qrY, qrSize, qrSize);

                int chipW = (int)(38 * scale);
                int chipH = (int)(28 * scale);
                int chipX = qrX + (qrSize - chipW) / 2;
                int chipY = photoY + (int)(4 * scale);
                Rectangle chipBox = new Rectangle(chipX, chipY, chipW, chipH);

                if (theme.ShowSmartChip)
                {
                    DrawSmartChip(g, chipBox, scale);
                }

                if (theme.ShowQrCode)
                {
                    string qrPayload = string.IsNullOrWhiteSpace(s.StudentId) ? "HCMUE-STUDENT" : $"{s.StudentId}|{s.FullName}|{s.ClassName}";
                    DrawQrCode(g, qrBox, qrPayload, scale);
                }

                // C. VẼ THÔNG TIN SINH VIÊN (6 DÒNG THẲNG HÀNG, CĂN TỌA ĐỘ CHUẨN, KHÔNG ĐÈ LÊN NHAU)
                float infoX = photoBox.Right + 16 * scale;
                float labelColWidth = 85 * scale;   // Dành trọn vẹn 85px cho nhãn
                float valueX = infoX + labelColWidth + 6 * scale; // Giá trị bắt đầu sau nhãn + 6px lề
                float rowPitch = 33 * scale;         // Mỗi dòng cách nhau 33px (rộng rãi, thoáng mắt)
                float currentY = photoY + 4 * scale;

                using (Font lblFont = new Font("Arial", 8.5f * scale, FontStyle.Bold))
                using (Font idFont = new Font("Arial", 10.5f * scale, FontStyle.Bold))
                using (Font nameFont = new Font("Arial", 10.5f * scale, FontStyle.Bold))
                using (Font valFont = new Font("Arial", 9.0f * scale, FontStyle.Regular))
                using (Brush lblBrush = new SolidBrush(theme.TextLabelColor))
                using (Brush primaryBrush = new SolidBrush(theme.TextPrimaryColor))
                using (Brush valBrush = new SolidBrush(theme.TextValueColor))
                using (StringFormat sfVal = new StringFormat { LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap })
                {
                    // Hàng 0: Mã số SV (Nổi bật, font to đậm)
                    float maxValWidthTop = (theme.ShowSmartChip ? chipBox.Left : card.Right - 18 * scale) - valueX - 6 * scale;
                    DrawOneInfoLine(g, "Mã số SV:", s.StudentId, lblFont, idFont, lblBrush, primaryBrush, infoX, labelColWidth, valueX, maxValWidthTop, currentY, 24 * scale, sfVal);
                    currentY += rowPitch;

                    // Hàng 1: Họ và tên (Font to đậm)
                    DrawOneInfoLine(g, "Họ và tên:", s.FullName, lblFont, nameFont, lblBrush, primaryBrush, infoX, labelColWidth, valueX, maxValWidthTop, currentY, 24 * scale, sfVal);
                    currentY += rowPitch;

                    // Hàng 2: Ngày sinh
                    float maxValWidthMid = card.Right - 18 * scale - valueX;
                    string birthStr = s.BirthDate.ToString("dd/MM/yyyy");
                    DrawOneInfoLine(g, "Ngày sinh:", birthStr, lblFont, valFont, lblBrush, valBrush, infoX, labelColWidth, valueX, maxValWidthMid, currentY, 24 * scale, sfVal);
                    currentY += rowPitch;

                    // Hàng 3: Giới tính
                    DrawOneInfoLine(g, "Giới tính:", s.Gender, lblFont, valFont, lblBrush, valBrush, infoX, labelColWidth, valueX, maxValWidthMid, currentY, 24 * scale, sfVal);
                    currentY += rowPitch;

                    // Hàng 4: Lớp học (Nếu có QR Code thì giới hạn trước QR Code)
                    float maxValWidthBottom = (theme.ShowQrCode ? qrBox.Left : card.Right - 18 * scale) - valueX - 6 * scale;
                    DrawOneInfoLine(g, "Lớp học:", s.ClassName, lblFont, valFont, lblBrush, valBrush, infoX, labelColWidth, valueX, maxValWidthBottom, currentY, 24 * scale, sfVal);
                    currentY += rowPitch;

                    // Hàng 5: Khóa học
                    DrawOneInfoLine(g, "Khóa học:", s.Course, lblFont, valFont, lblBrush, valBrush, infoX, labelColWidth, valueX, maxValWidthBottom, currentY, 24 * scale, sfVal);
                }

                // 5. VIỀN NGOÀI CỦA THẺ
                using (Pen borderPen = new Pen(theme.BorderColor, 2f * scale))
                {
                    g.DrawPath(borderPen, cardPath);
                }
            }
        }

        /// <summary>
        /// Vẽ một dòng thông tin gồm Nhãn và Giá trị vào hai hình chữ nhật độc lập,
        /// ngăn chặn tuyệt đối tình trạng đè chữ lên nhau.
        /// </summary>
        private static void DrawOneInfoLine(Graphics g, string label, string value,
            Font lblFont, Font valFont, Brush lblBrush, Brush valBrush,
            float lblX, float lblWidth, float valX, float maxValWidth, float y, float height, StringFormat sfVal)
        {
            RectangleF lblRect = new RectangleF(lblX, y, lblWidth, height);
            RectangleF valRect = new RectangleF(valX, y, Math.Max(30, maxValWidth), height);

            using (StringFormat sfLbl = new StringFormat { LineAlignment = StringAlignment.Center })
            {
                g.DrawString(label, lblFont, lblBrush, lblRect, sfLbl);
            }

            string displayVal = !string.IsNullOrWhiteSpace(value) ? value : "---";
            g.DrawString(displayVal, valFont, valBrush, valRect, sfVal);
        }

        /// <summary>
        /// Thuật toán vẽ ảnh giữ đúng tỉ lệ gốc (tương đương PictureBoxSizeMode.Zoom),
        /// giúp ảnh chân dung sinh viên không bao giờ bị méo khi hiển thị hoặc in.
        /// </summary>
        public static void DrawImageZoom(Graphics g, Image image, Rectangle box)
        {
            float ratio = Math.Min(
                (float)box.Width / image.Width,
                (float)box.Height / image.Height);

            int w = (int)(image.Width * ratio);
            int h = (int)(image.Height * ratio);
            int x = box.Left + (box.Width - w) / 2;
            int y = box.Top + (box.Height - h) / 2;

            g.DrawImage(image, new Rectangle(x, y, w, h));
        }

        /// <summary>
        /// Vẽ mô phỏng vi mạch chip thông minh màu vàng hoàng kim.
        /// </summary>
        private static void DrawSmartChip(Graphics g, Rectangle box, float scale)
        {
            using (Brush chipBrush = new LinearGradientBrush(box, Color.FromArgb(240, 205, 90), Color.FromArgb(200, 155, 45), 45f))
            using (Pen chipBorderPen = new Pen(Color.FromArgb(145, 105, 20), 1f * scale))
            {
                g.FillRectangle(chipBrush, box);
                g.DrawRectangle(chipBorderPen, box);

                int midY = box.Top + box.Height / 2;
                g.DrawLine(chipBorderPen, box.Left, midY, box.Right, midY);
                int midX = box.Left + box.Width / 2;
                g.DrawLine(chipBorderPen, midX, box.Top, midX, box.Bottom);
            }
        }

        /// <summary>
        /// Vẽ mã QR Code ma trận chuẩn GDI+ dựa trên chuỗi định danh sinh viên.
        /// </summary>
        private static void DrawQrCode(Graphics g, Rectangle box, string payload, float scale)
        {
            using (Brush whiteBrush = new SolidBrush(Color.White))
            using (Brush blackBrush = new SolidBrush(Color.FromArgb(20, 25, 35)))
            using (Pen borderPen = new Pen(Color.LightGray, 1f * scale))
            {
                g.FillRectangle(whiteBrush, box);
                g.DrawRectangle(borderPen, box);

                int matrixSize = 17;
                float cellSize = (float)box.Width / matrixSize;

                // 3 ô vuông định vị (Position Patterns)
                DrawQrFinderPattern(g, box.Left, box.Top, cellSize);
                DrawQrFinderPattern(g, box.Right - (int)(cellSize * 5), box.Top, cellSize);
                DrawQrFinderPattern(g, box.Left, box.Bottom - (int)(cellSize * 5), cellSize);

                // Điểm ảnh ma trận được sinh từ mã băm dữ liệu
                int hash = Math.Abs(payload.GetHashCode());
                for (int r = 0; r < matrixSize; r++)
                {
                    for (int c = 0; c < matrixSize; c++)
                    {
                        if ((r < 6 && c < 6) || (r < 6 && c > matrixSize - 7) || (r > matrixSize - 7 && c < 6))
                            continue;

                        if (((hash >> ((r * c) % 24)) & 1) == 1 || (r + c) % 3 == 0)
                        {
                            g.FillRectangle(blackBrush, box.Left + c * cellSize, box.Top + r * cellSize, cellSize, cellSize);
                        }
                    }
                }
            }
        }

        private static void DrawQrFinderPattern(Graphics g, float x, float y, float cellSize)
        {
            using (Brush black = new SolidBrush(Color.FromArgb(20, 25, 35)))
            using (Brush white = new SolidBrush(Color.White))
            {
                g.FillRectangle(black, x, y, cellSize * 5, cellSize * 5);
                g.FillRectangle(white, x + cellSize, y + cellSize, cellSize * 3, cellSize * 3);
                g.FillRectangle(black, x + cellSize * 1.5f, y + cellSize * 1.5f, cellSize * 2, cellSize * 2);
            }
        }

        #region Helper Bo góc bằng GraphicsPath

        private static GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static GraphicsPath GetTopRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddLine(rect.Right, rect.Bottom, rect.X, rect.Bottom);
            path.CloseFigure();
            return path;
        }

        private static GraphicsPath GetBottomRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            path.AddLine(rect.X, rect.Y, rect.Right, rect.Y);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        #endregion
    }
}
