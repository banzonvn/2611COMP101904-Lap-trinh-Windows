using System;

namespace Lab03_QuanLySinhVienOOP
{
    public class SinhVien : Nguoi
    {
        private string maSinhVien;
        private string maLop;
        private double diemTrungBinh;

        public string MaSinhVien
        {
            get => maSinhVien;
            set => maSinhVien = value;
        }

        public string MaLop
        {
            get => maLop;
            set => maLop = value;
        }

        public double DiemTrungBinh
        {
            get => diemTrungBinh;
            set
            {
                if (value < 0.0 || value > 10.0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Điểm trung bình phải nằm trong khoảng từ 0 đến 10.");
                diemTrungBinh = value;
            }
        }

        public SinhVien() : base() { }

        public SinhVien(string maSinhVien, string hoTen, DateTime ngaySinh, string maLop, double diemTrungBinh)
            : base(hoTen, ngaySinh)
        {
            MaSinhVien = maSinhVien;
            MaLop = maLop;
            DiemTrungBinh = diemTrungBinh;
        }

        public string XepLoai()
        {
            if (DiemTrungBinh >= 8.5) return "Giỏi";
            if (DiemTrungBinh >= 7.0) return "Khá";
            if (DiemTrungBinh >= 5.0) return "Trung bình";
            return "Yếu";
        }

        public override string LayThongTin()
        {
            return $"| {MaSinhVien,-8} | {HoTen,-20} | {NgaySinh:dd/MM/yyyy} | {MaLop,-10} | {DiemTrungBinh,5:F1} | {XepLoai(),-10} |";
        }
    }
}