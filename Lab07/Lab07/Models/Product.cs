using System;

namespace Lab07.Models
{
    public class Product
    {
        public string MaSP { get; set; } = string.Empty;
        public string TenSP { get; set; } = string.Empty;
        public string LoaiSP { get; set; } = "Điện tử";
        public decimal DonGia { get; set; }
        public int SoLuong { get; set; }

        public Product() { }

        public Product(string maSP, string tenSP, string loaiSP, decimal donGia, int soLuong)
        {
            MaSP = maSP.Trim();
            TenSP = tenSP.Trim();
            LoaiSP = loaiSP;
            DonGia = donGia;
            SoLuong = soLuong;
        }
    }
}
