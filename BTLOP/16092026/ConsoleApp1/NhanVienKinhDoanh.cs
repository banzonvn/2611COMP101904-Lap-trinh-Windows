using System;

namespace QuanLyNhanVien
{
    public class NhanVienKinhDoanh : NhanVien
    {
        private double doanhSo;

        public double DoanhSo
        {
            get => doanhSo;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Doanh số phải lớn hơn hoặc bằng 0.");
                doanhSo = value;
            }
        }

        public NhanVienKinhDoanh(string maNV, string hoTen, double luongCoBan, double doanhSo)
            : base(maNV, hoTen, luongCoBan)
        {
            DoanhSo = doanhSo;
        }

        public override double TinhLuong()
        {
            return LuongCoBan + (0.05 * DoanhSo);
        }

        public override void HienThiThongTin()
        {
            string chiTiet = $"DS: {DoanhSo:N0} đ";
            Console.WriteLine($"| {"Kinh Doanh",-12} | {MaNV,-6} | {HoTen,-18} | {chiTiet,-20} | {LuongCoBan,11:N0} đ | {TinhLuong(),11:N0} đ |");
        }

    }
}