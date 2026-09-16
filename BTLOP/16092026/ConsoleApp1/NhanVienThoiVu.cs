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

        public NhanVienThoiVu() : base()
        {
            LuongCoBan = 1.0;
        }

        public NhanVienThoiVu(string maNV, string hoTen, double soGioLam, double luongTheoGio)
            : base(maNV, hoTen, 1.0)
        {
            SoGioLam = soGioLam;
            LuongTheoGio = luongTheoGio;
        }

        public override void NhapThongTin()
        {
            Console.Write("Nhập mã nhân viên: ");
            MaNV = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Nhập họ và tên: ");
            HoTen = Console.ReadLine()?.Trim() ?? "";

            LuongCoBan = 1.0;

            while (true)
            {
                Console.Write("Nhập số giờ làm (>= 0): ");
                if (double.TryParse(Console.ReadLine(), out double gio) && gio >= 0)
                {
                    SoGioLam = gio;
                    break;
                }
                Console.WriteLine("Số giờ làm không hợp lệ! Vui lòng nhập lại.");
            }

            while (true)
            {
                Console.Write("Nhập lương theo giờ (> 0): ");
                if (double.TryParse(Console.ReadLine(), out double ltg) && ltg > 0)
                {
                    LuongTheoGio = ltg;
                    break;
                }
                Console.WriteLine("Lương theo giờ không hợp lệ! Vui lòng nhập lại.");
            }
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