using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using StudentCardPrinter.Models;

namespace StudentCardPrinter.Services
{
    /// <summary>
    /// Lớp chuyên trách vẽ thẻ sinh viên (Single Responsibility Principle).
    /// Dùng chung duy nhất một logic vẽ cho cả màn hình Preview trên Form,
    /// in ra giấy A4/máy in thẻ qua PrintDocument, và xuất file ảnh PNG chất lượng cao.
    /// </summary>
    public static class StudentCardRenderer
    {
        public static void Render(Graphics g, Rectangle card, StudentCardInfo s, CardTheme theme)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            int cornerRadius = Math.Max(8, card.Height / 24);

            // 1. Vẽ nền thẻ với góc bo tròn mềm mại chuẩn thẻ nhựa CR80
            using (GraphicsPath cardPath = GetRoundedPath(card, cornerRadius))
            {
                using (Brush bgBrush = new SolidBrush(theme.CardBgColor))
                {
                    g.FillPath(bgBrush, cardPath);
                }

                // 2. Vẽ dải Header phía trên
                int headerHeight = (int)(card.Height * 0.22f);
                Rectangle headerRect = new Rectangle(card.X, card.Y, card.Width, headerHeight);
                using (GraphicsPath headerPath = GetTopRoundedPath(headerRect, cornerRadius))
                using (Brush headerBrush = new SolidBrush(theme.HeaderBgColor))
                {
                    g.FillPath(headerBrush, headerPath);
                }

                // 3. Vẽ text trong Header
                using (Font schoolFont = new Font("Arial", Math.Max(7.5f, card.Height * 0.034f), FontStyle.Bold))
                using (Font subHeaderFont = new Font("Arial", Math.Max(6.5f, card.Height * 0.028f), FontStyle.Bold))
                using (Font cardTitleFont = new Font("Arial", Math.Max(10f, card.Height * 0.052f), FontStyle.Bold))
                using (Brush headerTextBrush = new SolidBrush(theme.HeaderTextColor))
                using (Brush accentBrush = new SolidBrush(Color.FromArgb(240, 200, 80))) // Vàng kim accent
                {
                    int textLeft = card.X + 15;
                    g.DrawString("TRƯỜNG ĐẠI HỌC SƯ PHẠM TP. HỒ CHÍ MINH", schoolFont, headerTextBrush, textLeft, card.Y + 8);
                    
                    string facText = !string.IsNullOrWhiteSpace(s.Faculty) ? $"KHOA {s.Faculty.ToUpper()}" : "KHOA CÔNG NGHỆ THÔNG TIN";
                    g.DrawString(facText, subHeaderFont, accentBrush, textLeft, card.Y + 8 + (int)(card.Height * 0.045f));

                    g.DrawString("THẺ SINH VIÊN / STUDENT CARD", cardTitleFont, headerTextBrush, textLeft, card.Y + 8 + (int)(card.Height * 0.095f));
                }

                // 4. Vẽ khung ảnh đại diện (bên trái thẻ)
                int photoX = card.X + (int)(card.Width * 0.04f);
                int photoY = card.Y + headerHeight + (int)(card.Height * 0.04f);
                int photoW = (int)(card.Width * 0.26f);
                int photoH = (int)(card.Height * 0.58f);
                Rectangle photoBox = new Rectangle(photoX, photoY, photoW, photoH);

                // Nền và viền ảnh
                using (Brush whiteBrush = new SolidBrush(Color.White))
                using (Pen photoBorderPen = new Pen(theme.BorderColor, 1.5f))
                {
                    g.FillRectangle(whiteBrush, photoBox);
                    if (s.Photo != null)
                    {
                        DrawImageZoom(g, s.Photo, photoBox);
                    }
                    else
                    {
                        // Hiển thị khung placeholder nếu chưa có ảnh
                        using Font placeholderFont = new Font("Arial", Math.Max(8f, card.Height * 0.035f), FontStyle.Regular);
                        using Brush grayBrush = new SolidBrush(Color.Gray);
                        using StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                        g.DrawString("[ Chưa chọn\nảnh đại diện ]", placeholderFont, grayBrush, photoBox, sf);
                    }
                    g.DrawRectangle(photoBorderPen, photoBox);
                }

                // 5. Vẽ thông tin chi tiết sinh viên (bên phải ảnh)
                int infoX = photoX + photoW + (int)(card.Width * 0.04f);
                int infoY = photoY + 2;
                int lineSpacing = Math.Max(15, (int)(card.Height * 0.075f));

                using (Font labelFont = new Font("Arial", Math.Max(7.5f, card.Height * 0.033f), FontStyle.Bold))
                using (Font valueFont = new Font("Arial", Math.Max(7.5f, card.Height * 0.033f), FontStyle.Regular))
                using (Font idFont = new Font("Arial", Math.Max(8.5f, card.Height * 0.038f), FontStyle.Bold))
                using (Brush labelBrush = new SolidBrush(theme.TextLabelColor))
                using (Brush valueBrush = new SolidBrush(theme.TextValueColor))
                using (Brush idBrush = new SolidBrush(theme.TextPrimaryColor))
                {
                    int labelWidth = (int)(card.Width * 0.17f);

                    // MSSV nổi bật
                    g.DrawString("Mã số SV:", labelFont, labelBrush, infoX, infoY);
                    g.DrawString(!string.IsNullOrWhiteSpace(s.StudentId) ? s.StudentId : "---", idFont, idBrush, infoX + labelWidth, infoY - 1);
                    infoY += lineSpacing + 2;

                    // Họ và tên
                    g.DrawString("Họ và tên:", labelFont, labelBrush, infoX, infoY);
                    g.DrawString(!string.IsNullOrWhiteSpace(s.FullName) ? s.FullName : "---", labelFont, valueBrush, infoX + labelWidth, infoY);
                    infoY += lineSpacing;

                    // Lớp
                    g.DrawString("Lớp:", labelFont, labelBrush, infoX, infoY);
                    g.DrawString(!string.IsNullOrWhiteSpace(s.ClassName) ? s.ClassName : "---", valueFont, valueBrush, infoX + labelWidth, infoY);
                    infoY += lineSpacing;

                    // Khoa
                    g.DrawString("Khoa:", labelFont, labelBrush, infoX, infoY);
                    g.DrawString(!string.IsNullOrWhiteSpace(s.Faculty) ? s.Faculty : "---", valueFont, valueBrush, infoX + labelWidth, infoY);
                    infoY += lineSpacing;

                    // Khóa học & Giới tính
                    g.DrawString("Khóa học:", labelFont, labelBrush, infoX, infoY);
                    string courseGender = $"{s.Course}  |  Giới tính: {s.Gender}";
                    g.DrawString(courseGender, valueFont, valueBrush, infoX + labelWidth, infoY);
                    infoY += lineSpacing;

                    // Ngày sinh
                    g.DrawString("Ngày sinh:", labelFont, labelBrush, infoX, infoY);
                    g.DrawString(s.BirthDate.ToString("dd/MM/yyyy"), valueFont, valueBrush, infoX + labelWidth, infoY);
                }

                // 6. Vẽ Chip thông minh giả lập (tính năng hiện đại)
                if (theme.ShowSmartChip)
                {
                    int chipW = (int)(card.Width * 0.08f);
                    int chipH = (int)(card.Height * 0.11f);
                    int chipX = card.Right - chipW - (int)(card.Width * 0.04f);
                    int chipY = card.Y + headerHeight + (int)(card.Height * 0.035f);
                    DrawSmartChip(g, new Rectangle(chipX, chipY, chipW, chipH));
                }

                // 7. Vẽ Mã QR Code giả lập (tính năng hiện đại)
                if (theme.ShowQrCode)
                {
                    int qrSize = (int)(card.Height * 0.22f);
                    int qrX = card.Right - qrSize - (int)(card.Width * 0.035f);
                    int qrY = card.Bottom - qrSize - (int)(card.Height * 0.10f);
                    string qrData = string.IsNullOrWhiteSpace(s.StudentId) ? "HCMUE-STUDENT" : $"{s.StudentId}|{s.FullName}";
                    DrawQrCode(g, new Rectangle(qrX, qrY, qrSize, qrSize), qrData);
                }

                // 8. Vẽ Dải Footer bảo an phía dưới
                int footerHeight = (int)(card.Height * 0.075f);
                Rectangle footerRect = new Rectangle(card.X, card.Bottom - footerHeight, card.Width, footerHeight);
                using (GraphicsPath footerPath = GetBottomRoundedPath(footerRect, cornerRadius))
                using (Brush footerBrush = new SolidBrush(theme.FooterBgColor))
                using (Font footerFont = new Font("Arial", Math.Max(5.8f, card.Height * 0.025f), FontStyle.Italic))
                using (Brush footerTextBrush = new SolidBrush(Color.FromArgb(80, 90, 110)))
                using (StringFormat sfFooter = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    g.FillPath(footerBrush, footerPath);
                    g.DrawString("Thẻ có giá trị trong suốt thời gian theo học tại trường • HCMUE Smart Card", footerFont, footerTextBrush, footerRect, sfFooter);
                }

                // 9. Vẽ đường viền bao quanh toàn bộ thẻ
                using (Pen borderPen = new Pen(theme.BorderColor, 2f))
                {
                    g.DrawPath(borderPen, cardPath);
                }
            }
        }

        /// <summary>
        /// Vẽ ảnh đại diện theo chế độ Zoom, giữ nguyên tỷ lệ khung hình và không bị méo.
        /// </summary>
        public static void DrawImageZoom(Graphics g, Image image, Rectangle box)
        {
            float ratio = Math.Min((float)box.Width / image.Width, (float)box.Height / image.Height);
            int w = (int)(image.Width * ratio);
            int h = (int)(image.Height * ratio);
            int x = box.Left + (box.Width - w) / 2;
            int y = box.Top + (box.Height - h) / 2;
            g.DrawImage(image, new Rectangle(x, y, w, h));
        }

        /// <summary>
        /// Vẽ mô phỏng vi mạch chip điện tử thông minh màu vàng hoàng kim.
        /// </summary>
        private static void DrawSmartChip(Graphics g, Rectangle box)
        {
            using (Brush chipBrush = new LinearGradientBrush(box, Color.FromArgb(238, 195, 75), Color.FromArgb(198, 150, 40), 45f))
            using (Pen chipBorderPen = new Pen(Color.FromArgb(145, 105, 20), 1f))
            {
                g.FillRectangle(chipBrush, box);
                g.DrawRectangle(chipBorderPen, box);

                // Đường rãnh phân cực chip
                int midY = box.Top + box.Height / 2;
                g.DrawLine(chipBorderPen, box.Left, midY, box.Right, midY);
                int midX = box.Left + box.Width / 2;
                g.DrawLine(chipBorderPen, midX, box.Top, midX, box.Bottom);
            }
        }

        /// <summary>
        /// Thuật toán vẽ mã QR Code ma trận vuông GDI+ dựa trên dữ liệu thẻ.
        /// </summary>
        private static void DrawQrCode(Graphics g, Rectangle box, string payload)
        {
            using (Brush whiteBrush = new SolidBrush(Color.White))
            using (Brush blackBrush = new SolidBrush(Color.Black))
            using (Pen borderPen = new Pen(Color.LightGray, 1f))
            {
                g.FillRectangle(whiteBrush, box);
                g.DrawRectangle(borderPen, box);

                int matrixSize = 17; // Ma trận 17x17 pixel
                float cellSize = (float)box.Width / matrixSize;

                // 3 ô vuông định vị (Position Detection Patterns)
                DrawQrFinderPattern(g, box.Left, box.Top, cellSize);
                DrawQrFinderPattern(g, box.Right - (int)(cellSize * 5), box.Top, cellSize);
                DrawQrFinderPattern(g, box.Left, box.Bottom - (int)(cellSize * 5), cellSize);

                // Dữ liệu ma trận giả lập ngẫu nhiên có kiểm soát dựa trên chuỗi payload
                int hash = Math.Abs(payload.GetHashCode());
                for (int r = 0; r < matrixSize; r++)
                {
                    for (int c = 0; c < matrixSize; c++)
                    {
                        // Bỏ qua các góc định vị
                        if ((r < 6 && c < 6) || (r < 6 && c > matrixSize - 7) || (r > matrixSize - 7 && c < 6))
                            continue;

                        // Quyết định pixel đen dựa trên hàm băm
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
            using (Brush black = new SolidBrush(Color.Black))
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
