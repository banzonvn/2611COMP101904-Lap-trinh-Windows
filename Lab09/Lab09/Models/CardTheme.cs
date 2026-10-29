using System.Drawing;

namespace StudentCardPrinter.Models
{
    /// <summary>
    /// Định nghĩa cấu hình màu sắc và phong cách giao diện của thẻ sinh viên.
    /// Giúp người dùng tùy biến giao diện thẻ sinh động, độc đáo (tính năng mở rộng).
    /// </summary>
    public class CardTheme
    {
        public string Name { get; set; } = "HCMUE Chuẩn Xanh Navy";
        public Color HeaderBgColor { get; set; } = Color.FromArgb(16, 44, 87);      // Xanh Navy đậm truyền thống HCMUE
        public Color HeaderTextColor { get; set; } = Color.White;
        public Color CardBgColor { get; set; } = Color.FromArgb(247, 250, 255);       // Nền thẻ dịu mắt
        public Color BorderColor { get; set; } = Color.FromArgb(16, 44, 87);
        public Color TextPrimaryColor { get; set; } = Color.FromArgb(16, 44, 87);
        public Color TextLabelColor { get; set; } = Color.FromArgb(70, 80, 95);
        public Color TextValueColor { get; set; } = Color.Black;
        public Color FooterBgColor { get; set; } = Color.FromArgb(232, 238, 248);
        public bool ShowQrCode { get; set; } = true;
        public bool ShowSmartChip { get; set; } = true;

        public static CardTheme[] PredefinedThemes => new[]
        {
            new CardTheme
            {
                Name = "HCMUE Chuẩn Xanh Navy",
                HeaderBgColor = Color.FromArgb(16, 44, 87),
                BorderColor = Color.FromArgb(16, 44, 87),
                CardBgColor = Color.FromArgb(247, 250, 255),
                FooterBgColor = Color.FromArgb(232, 238, 248),
                TextPrimaryColor = Color.FromArgb(16, 44, 87)
            },
            new CardTheme
            {
                Name = "Xanh Ngọc Năng Động (Emerald)",
                HeaderBgColor = Color.FromArgb(10, 84, 56),
                BorderColor = Color.FromArgb(10, 84, 56),
                CardBgColor = Color.FromArgb(244, 252, 247),
                FooterBgColor = Color.FromArgb(228, 245, 234),
                TextPrimaryColor = Color.FromArgb(10, 84, 56)
            },
            new CardTheme
            {
                Name = "Đỏ Rượu Sang Trọng (Burgundy)",
                HeaderBgColor = Color.FromArgb(128, 24, 40),
                BorderColor = Color.FromArgb(128, 24, 40),
                CardBgColor = Color.FromArgb(254, 246, 247),
                FooterBgColor = Color.FromArgb(249, 230, 233),
                TextPrimaryColor = Color.FromArgb(128, 24, 40)
            },
            new CardTheme
            {
                Name = "Vàng Cam Nhiệt Huyết (Amber)",
                HeaderBgColor = Color.FromArgb(190, 85, 10),
                BorderColor = Color.FromArgb(190, 85, 10),
                CardBgColor = Color.FromArgb(255, 250, 242),
                FooterBgColor = Color.FromArgb(254, 240, 222),
                TextPrimaryColor = Color.FromArgb(190, 85, 10)
            }
        };
    }
}
