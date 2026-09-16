using System;

namespace QuanLyNhanVien
{
    public class NhanVienThoiVu : NhanVien
    {
        private double soGioLam;
        private double luongTheoGio;

        public double SoGioLam
        {
            get => soGioLam;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Số giờ làm phải lớn hơn hoặc bằng 0.");
                soGioLam = value;
            }
        }

        public double LuongTheoGio
        {
            get => luongTheoGio;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Lương theo giờ phải lớn hơn 0.");
                luongTheoGio = value;
            }
        }

        // Lương cơ bản gán 1.0 để thỏa mãn ràng buộc > 0 của lớp cha
        public NhanVienThoiVu(string maNV, string hoTen, double soGioLam, double luongTheoGio)
            : base(maNV, hoTen, 1.0)
        {
            SoGioLam = soGioLam;
            LuongTheoGio = luongTheoGio;
        }

        public override double TinhLuong()
        {
            return SoGioLam * LuongTheoGio;
        }

        public override void HienThiThongTin()
        {
            string chiTiet = $"{SoGioLam}h x {LuongTheoGio:N0}đ";
            Console.WriteLine($"| {"Thời Vụ",-12} | {MaNV,-6} | {HoTen,-18} | {chiTiet,-20} | {"-",11}   | {TinhLuong(),11:N0} đ |");
        }

    }
}