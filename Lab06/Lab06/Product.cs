using System;

namespace Lab06
{
    public class Product
    {
        public string MaSP { get; set; } = string.Empty;
        public string TenSP { get; set; } = string.Empty;
        public string LoaiSP { get; set; } = "Điện tử";
        public decimal DonGia { get; set; }
        public int SoLuong { get; set; }
        public DateTime NgayNhap { get; set; } = DateTime.Today;
        public bool ConKinhDoanh { get; set; } = true;
        public string DuongDanAnh { get; set; } = string.Empty;

        // Cột tính toán mở rộng theo Slide 23 của bài giảng
        public decimal ThanhTien => DonGia * SoLuong;

        public Product() { }

        public Product(string id, string name, string category, decimal price, int qty, DateTime importDate, bool active, string photo = "")
        {
            MaSP = id.Trim();
            TenSP = name.Trim();
            LoaiSP = category;
            DonGia = price;
            SoLuong = qty;
            NgayNhap = importDate;
            ConKinhDoanh = active;
            DuongDanAnh = photo;
        }

        public void MirrorFrom(Product source)
        {
            TenSP = source.TenSP;
            LoaiSP = source.LoaiSP;
            DonGia = source.DonGia;
            SoLuong = source.SoLuong;
            NgayNhap = source.NgayNhap;
            ConKinhDoanh = source.ConKinhDoanh;
            DuongDanAnh = source.DuongDanAnh;
        }
    }
}