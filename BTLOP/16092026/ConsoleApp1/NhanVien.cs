using System;

namespace QuanLyNhanVien
{
    public class NhanVien
    {
        private string maNV;
        private string hoTen;
        private double luongCoBan;

        public string MaNV
        {
            get => maNV;
            set => maNV = value;
        }

        public string HoTen
        {
            get => hoTen;
            set => hoTen = value;
        }

        public double LuongCoBan
        {
            get => luongCoBan;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Lương cơ bản phải lớn hơn 0.");
                luongCoBan = value;
            }
        }

        public NhanVien() { }

        public NhanVien(string maNV, string hoTen, double luongCoBan)
        {
            MaNV = maNV;
            HoTen = hoTen;
            LuongCoBan = luongCoBan;
        }

        public virtual void NhapThongTin()
        {
            Console.Write("Nhập mã nhân viên: ");
            MaNV = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Nhập họ và tên: ");
            HoTen = Console.ReadLine()?.Trim() ?? "";

            while (true)
            {
                Console.Write("Nhập lương cơ bản (> 0): ");
                if (double.TryParse(Console.ReadLine(), out double lcb) && lcb > 0)
                {
                    LuongCoBan = lcb;
                    break;
                }
                Console.WriteLine("Lương cơ bản không hợp lệ! Vui lòng nhập lại.");
            }
        }

        public virtual double TinhLuong()
        {
            return LuongCoBan;
        }

        public virtual void HienThiThongTin()
        {
            Console.WriteLine($"| {"Cơ Bản",-12} | {MaNV,-6} | {HoTen,-18} | {"-",-20} | {LuongCoBan,11:N0} đ | {TinhLuong(),11:N0} đ |");
        }
    }
}