using System.Drawing;

namespace StudentCardPrinter.Models
{
    /// <summary>
    /// Lớp cấu hình màu sắc và phong cách của Thẻ Sinh Viên (Design Pattern: Strategy/Theme).
    /// Giúp người dùng tùy biến giao diện thẻ sinh động, độc đáo mà không làm xáo trộn bố cục vẽ.
    /// </summary>
    public class CardTheme
    {
        public string Name { get; set; } = "Mặc định";
        public Color HeaderBgColor { get; set; }
        public Color HeaderTextColor { get; set; }
        public Color AccentColor { get; set; }
        public Color CardBgColor { get; set; }
        public Color BorderColor { get; set; }
        public Color TextPrimaryColor { get; set; }
        public Color TextLabelColor { get; set; }
        public Color TextValueColor { get; set; }
        public Color FooterBgColor { get; set; }
        public bool ShowQrCode { get; set; } = true;
        public bool ShowSmartChip { get; set; } = true;

        public static readonly CardTheme[] PredefinedThemes = new[]
        {
            new CardTheme
            {
                Name = "HCMUE Classic (Xanh Dương Sư Phạm)",
                HeaderBgColor = Color.FromArgb(20, 50, 105),       // Xanh Navy đại học
                HeaderTextColor = Color.White,
                AccentColor = Color.FromArgb(255, 215, 0),         // Vàng ánh kim
                CardBgColor = Color.FromArgb(248, 250, 254),       // Trắng xanh thanh nhã
                BorderColor = Color.FromArgb(20, 50, 105),
                TextPrimaryColor = Color.FromArgb(20, 50, 105),
                TextLabelColor = Color.FromArgb(85, 95, 110),
                TextValueColor = Color.FromArgb(25, 30, 40),
                FooterBgColor = Color.FromArgb(232, 238, 248),
                ShowQrCode = true,
                ShowSmartChip = true
            },
            new CardTheme
            {
                Name = "Emerald Tech (Xanh Ngọc Công Nghệ)",
                HeaderBgColor = Color.FromArgb(15, 80, 60),        // Xanh ngọc lục bảo
                HeaderTextColor = Color.White,
                AccentColor = Color.FromArgb(110, 230, 180),       // Xanh mint sáng
                CardBgColor = Color.FromArgb(246, 253, 250),
                BorderColor = Color.FromArgb(15, 80, 60),
                TextPrimaryColor = Color.FromArgb(15, 80, 60),
                TextLabelColor = Color.FromArgb(80, 100, 90),
                TextValueColor = Color.FromArgb(20, 35, 30),
                FooterBgColor = Color.FromArgb(228, 246, 238),
                ShowQrCode = true,
                ShowSmartChip = true
            },
            new CardTheme
            {
                Name = "Ruby Graduate (Đỏ Đô Sang Trọng)",
                HeaderBgColor = Color.FromArgb(130, 25, 35),       // Đỏ ruby truyền thống
                HeaderTextColor = Color.White,
                AccentColor = Color.FromArgb(255, 210, 120),       // Vàng kem
                CardBgColor = Color.FromArgb(255, 250, 250),
                BorderColor = Color.FromArgb(130, 25, 35),
                TextPrimaryColor = Color.FromArgb(130, 25, 35),
                TextLabelColor = Color.FromArgb(110, 80, 85),
                TextValueColor = Color.FromArgb(35, 20, 25),
                FooterBgColor = Color.FromArgb(252, 235, 238),
                ShowQrCode = true,
                ShowSmartChip = true
            },
            new CardTheme
            {
                Name = "Modern Slate (Xám Titan Tối Giản)",
                HeaderBgColor = Color.FromArgb(45, 55, 72),        // Xám slate hiện đại
                HeaderTextColor = Color.White,
                AccentColor = Color.FromArgb(144, 205, 244),       // Xanh cyan nhạt
                CardBgColor = Color.FromArgb(250, 250, 252),
                BorderColor = Color.FromArgb(45, 55, 72),
                TextPrimaryColor = Color.FromArgb(45, 55, 72),
                TextLabelColor = Color.FromArgb(90, 100, 115),
                TextValueColor = Color.FromArgb(20, 25, 35),
                FooterBgColor = Color.FromArgb(235, 238, 242),
                ShowQrCode = true,
                ShowSmartChip = true
            }
        };
    }
}
