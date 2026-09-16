using System;

namespace QuanLyNhanVien
{
    public class NhanVienVanPhong : NhanVien
    {
        private int soNgayLamViec;

        public int SoNgayLamViec
        {
            get => soNgayLamViec;
            set
            {
                if (value < 0 || value > 31)
                    throw new ArgumentOutOfRangeException(nameof(value), "Số ngày làm việc phải từ 0 đến 31.");
                soNgayLamViec = value;
            }
        }

        public NhanVienVanPhong() : base() { }

        public NhanVienVanPhong(string maNV, string hoTen, double luongCoBan, int soNgayLamViec)
            : base(maNV, hoTen, luongCoBan)
        {
            SoNgayLamViec = soNgayLamViec;
        }

        public override void NhapThongTin()
        {
            base.NhapThongTin();
            while (true)
            {
                Console.Write("Nhập số ngày làm việc (0 - 31): ");
                if (int.TryParse(Console.ReadLine(), out int ngay) && ngay >= 0 && ngay <= 31)
                {
                    SoNgayLamViec = ngay;
                    break;
                }
                Console.WriteLine("Số ngày làm việc phải từ 0 đến 31! Vui lòng nhập lại.");
            }
        }

        public override double TinhLuong()
        {
            return LuongCoBan + (SoNgayLamViec * 200000.0);
        }

        public override void HienThiThongTin()
        {
            string chiTiet = $"{SoNgayLamViec} ngày";
            Console.WriteLine($"| {"Văn Phòng",-12} | {MaNV,-6} | {HoTen,-18} | {chiTiet,-20} | {LuongCoBan,11:N0} đ | {TinhLuong(),11:N0} đ |");
        }
    }
}