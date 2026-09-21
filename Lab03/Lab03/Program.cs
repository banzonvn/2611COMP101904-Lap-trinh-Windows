using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Lab03_QuanLySinhVienOOP
{
    internal class Program
    {
        private static readonly QuanLySinhVien qlsv = new QuanLySinhVien();

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            // Khởi tạo một số dữ liệu kiểm thử ban đầu
            KhoiTaoDuLieuMau();

            int luaChon = -1;
            do
            {
                HienThiMenu();
                Console.Write("Chon chuc nang: ");

                if (!int.TryParse(Console.ReadLine(), out luaChon))
                {
                    Console.WriteLine("Lỗi: Vui lòng nhập số hợp lệ!");
                    continue;
                }

                switch (luaChon)
                {
                    case 1:
                        ChucNangThemSinhVien();
                        break;
                    case 2:
                        InBangDanhSach(qlsv.LayDanhSach(), "DANH SÁCH TẤT CẢ SINH VIÊN");
                        break;
                    case 3:
                        ChucNangTimTheoMa();
                        break;
                    case 4:
                        ChucNangTimTheoTen();
                        break;
                    case 5:
                        ChucNangSuaDiem();
                        break;
                    case 6:
                        ChucNangXoaSinhVien();
                        break;
                    case 7:
                        InBangDanhSach(qlsv.SapXepTheoDiem(), "DANH SÁCH SẮP XẾP THEO ĐIỂM GIẢM DẦN");
                        break;
                    case 8:
                        InBangDanhSach(qlsv.LocSinhVienDat(), "DANH SÁCH SINH VIÊN ĐẠT (ĐIỂM >= 5.0)");
                        break;
                    case 0:
                        Console.WriteLine("Chương trình kết thúc. Tạm biệt!");
                        break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ, vui lòng chọn lại!");
                        break;
                }

            } while (luaChon != 0);
        }

        static void HienThiMenu()
        {
            Console.WriteLine("\n===== QUAN LY SINH VIEN =====");
            Console.WriteLine("1. Them sinh vien");
            Console.WriteLine("2. Xuat danh sach");
            Console.WriteLine("3. Tim sinh vien theo ma");
            Console.WriteLine("4. Tim sinh vien theo ten");
            Console.WriteLine("5. Sua diem trung binh");
            Console.WriteLine("6. Xoa sinh vien");
            Console.WriteLine("7. Sap xep theo diem giam dan");
            Console.WriteLine("8. Loc sinh vien dat");
            Console.WriteLine("0. Thoat");
        }

        static void ChucNangThemSinhVien()
        {
            Console.WriteLine("\n--- THÊM SINH VIÊN MỚI ---");

            string ma;
            while (true)
            {
                Console.Write("Nhập mã sinh viên: ");
                ma = Console.ReadLine()?.Trim() ?? "";
                if (string.IsNullOrEmpty(ma))
                {
                    Console.WriteLine("Mã sinh viên không được để trống!");
                    continue;
                }
                if (qlsv.TimTheoMa(ma) != null)
                {
                    Console.WriteLine($"Mã sinh viên '{ma}' đã tồn tại! Vui lòng nhập mã khác.");
                    continue;
                }
                break;
            }

            string ten;
            while (true)
            {
                Console.Write("Nhập họ tên sinh viên: ");
                ten = Console.ReadLine()?.Trim() ?? "";
                if (!string.IsNullOrEmpty(ten)) break;
                Console.WriteLine("Họ tên không được để trống!");
            }

            DateTime ngaySinh;
            while (true)
            {
                Console.Write("Nhập ngày sinh (dd/MM/yyyy): ");
                string strDate = Console.ReadLine()?.Trim() ?? "";
                if (DateTime.TryParseExact(strDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out ngaySinh))
                {
                    break;
                }
                Console.WriteLine("Định dạng ngày sinh không hợp lệ! Vui lòng nhập đúng dạng dd/MM/yyyy (ví dụ: 15/08/2005).");
            }

            Console.Write("Nhập mã lớp: ");
            string lop = Console.ReadLine()?.Trim() ?? "";

            double diem;
            while (true)
            {
                Console.Write("Nhập điểm trung bình (0 - 10): ");
                if (double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out diem) ||
                    double.TryParse(Console.ReadLine(), out diem))
                {
                    if (diem >= 0.0 && diem <= 10.0) break;
                }
                Console.WriteLine("Điểm không hợp lệ! Điểm trung bình phải là số thực từ 0 đến 10.");
            }

            var sv = new SinhVien(ma, ten, ngaySinh, lop, diem);
            if (qlsv.Them(sv))
            {
                Console.WriteLine("=> Thêm sinh viên thành công!");
            }
        }

        static void ChucNangTimTheoMa()
        {
            Console.Write("\nNhập mã sinh viên cần tìm: ");
            string ma = Console.ReadLine()?.Trim() ?? "";

            var sv = qlsv.TimTheoMa(ma);
            if (sv != null)
            {
                InBangDanhSach(new List<SinhVien> { sv }, "KẾT QUẢ TÌM KIẾM THEO MÃ");
            }
            else
            {
                Console.WriteLine($"Không tìm thấy sinh viên có mã: {ma}");
            }
        }

        static void ChucNangTimTheoTen()
        {
            Console.Write("\nNhập từ khóa họ tên cần tìm: ");
            string tuKhoa = Console.ReadLine()?.Trim() ?? "";

            var ketQua = qlsv.TimTheoTen(tuKhoa);
            InBangDanhSach(ketQua, $"KẾT QUẢ TÌM KIẾM VỚI TỪ KHÓA \"{tuKhoa}\"");
        }

        static void ChucNangSuaDiem()
        {
            Console.Write("\nNhập mã sinh viên cần sửa điểm: ");
            string ma = Console.ReadLine()?.Trim() ?? "";

            var sv = qlsv.TimTheoMa(ma);
            if (sv == null)
            {
                Console.WriteLine($"Không tìm thấy sinh viên có mã: {ma}");
                return;
            }

            Console.WriteLine($"Sinh viên: {sv.HoTen} (Điểm hiện tại: {sv.DiemTrungBinh:F1})");
            double diemMoi;
            while (true)
            {
                Console.Write("Nhập điểm trung bình mới (0 - 10): ");
                if (double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out diemMoi) ||
                    double.TryParse(Console.ReadLine(), out diemMoi))
                {
                    if (diemMoi >= 0.0 && diemMoi <= 10.0) break;
                }
                Console.WriteLine("Điểm không hợp lệ! Điểm phải từ 0 đến 10.");
            }

            qlsv.SuaDiem(ma, diemMoi);
            Console.WriteLine("=> Cập nhật điểm thành công!");
        }

        static void ChucNangXoaSinhVien()
        {
            Console.Write("\nNhập mã sinh viên cần xóa: ");
            string ma = Console.ReadLine()?.Trim() ?? "";

            if (qlsv.Xoa(ma))
            {
                Console.WriteLine($"=> Đã xóa sinh viên có mã '{ma}' thành công!");
            }
            else
            {
                Console.WriteLine($"Không tìm thấy sinh viên có mã: {ma}");
            }
        }

        static void InBangDanhSach(List<SinhVien> danhSach, string tieuDe)
        {
            Console.WriteLine($"\n--- {tieuDe} ---");
            if (danhSach == null || danhSach.Count == 0)
            {
                Console.WriteLine("Danh sách trống!");
                return;
            }

            Console.WriteLine(new string('=', 77));
            Console.WriteLine($"| {"Mã SV",-8} | {"Họ và Tên",-20} | {"Ngày sinh",-10} | {"Mã lớp",-10} | {"ĐTB",5} | {"Xếp loại",-10} |");
            Console.WriteLine(new string('-', 77));

            foreach (var sv in danhSach)
            {
                Console.WriteLine(sv.LayThongTin());
            }

            Console.WriteLine(new string('=', 77));
            Console.WriteLine($"Tổng số lượng: {danhSach.Count} sinh viên.");
        }

        static void KhoiTaoDuLieuMau()
        {
            qlsv.Them(new SinhVien("SV001", "Nguyễn Văn A", new DateTime(2005, 3, 15), "51.CNTT.A", 8.2));
            qlsv.Them(new SinhVien("SV002", "Trần Thị B", new DateTime(2005, 11, 20), "51.CNTT.A", 4.5));
            qlsv.Them(new SinhVien("SV003", "Lê Hoàng C", new DateTime(2005, 7, 5), "51.CNTT.B", 9.0));
            qlsv.Them(new SinhVien("SV004", "Phạm Văn D", new DateTime(2004, 9, 12), "51.CNTT.B", 6.8));
        }
    }
}