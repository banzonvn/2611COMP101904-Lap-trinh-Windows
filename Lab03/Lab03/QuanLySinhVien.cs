using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab03_QuanLySinhVienOOP
{
    public class QuanLySinhVien
    {
        private readonly List<SinhVien> danhSach;

        public QuanLySinhVien()
        {
            danhSach = new List<SinhVien>();
        }

        // Lấy toàn bộ danh sách sinh viên
        public List<SinhVien> LayDanhSach()
        {
            return danhSach;
        }

        // 1. Thêm sinh viên (kiểm tra trùng mã)
        public bool Them(SinhVien sv)
        {
            if (danhSach.Any(s => s.MaSinhVien.Equals(sv.MaSinhVien, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }
            danhSach.Add(sv);
            return true;
        }

        // 2. Tìm theo mã sinh viên
        public SinhVien TimTheoMa(string ma)
        {
            return danhSach.FirstOrDefault(s => s.MaSinhVien.Equals(ma.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        // 3. Tìm theo họ tên (chứa từ khóa)
        public List<SinhVien> TimTheoTen(string tuKhoa)
        {
            return danhSach
                .Where(s => s.HoTen.IndexOf(tuKhoa.Trim(), StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }

        // 4. Sửa điểm trung bình theo mã
        public bool SuaDiem(string ma, double diemMoi)
        {
            var sv = TimTheoMa(ma);
            if (sv == null) return false;

            sv.DiemTrungBinh = diemMoi;
            return true;
        }

        // 5. Xóa sinh viên theo mã
        public bool Xoa(string ma)
        {
            var sv = TimTheoMa(ma);
            if (sv == null) return false;

            return danhSach.Remove(sv);
        }

        // 6. Sắp xếp danh sách theo điểm giảm dần (LINQ)
        public List<SinhVien> SapXepTheoDiem()
        {
            return danhSach.OrderByDescending(s => s.DiemTrungBinh).ToList();
        }

        // 7. Lọc danh sách sinh viên đạt (Điểm >= 5.0) (LINQ)
        public List<SinhVien> LocSinhVienDat()
        {
            return danhSach.Where(s => s.DiemTrungBinh >= 5.0).ToList();
        }
    }
}