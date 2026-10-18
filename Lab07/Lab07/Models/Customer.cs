using System;

namespace Lab07.Models
{
    public class Customer
    {
        public string MaKH { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string LoaiKH { get; set; } = "Thường";

        public Customer() { }

        public Customer(string maKH, string hoTen, string soDienThoai, string loaiKH)
        {
            MaKH = maKH.Trim();
            HoTen = hoTen.Trim();
            SoDienThoai = soDienThoai.Trim();
            LoaiKH = loaiKH;
        }
    }
}
