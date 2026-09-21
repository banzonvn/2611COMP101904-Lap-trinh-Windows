using System;

namespace Lab03_QuanLySinhVienOOP
{
    public class Nguoi
    {
        private string hoTen;
        private DateTime ngaySinh;

        public string HoTen
        {
            get => hoTen;
            set => hoTen = value;
        }

        public DateTime NgaySinh
        {
            get => ngaySinh;
            set => ngaySinh = value;
        }

        public Nguoi() { }

        public Nguoi(string hoTen, DateTime ngaySinh)
        {
            HoTen = hoTen;
            NgaySinh = ngaySinh;
        }

        public virtual string LayThongTin()
        {
            return $"Họ tên: {HoTen,-20} | Ngày sinh: {NgaySinh:dd/MM/yyyy}";
        }
    }
}