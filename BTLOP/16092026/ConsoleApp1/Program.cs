using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLyNhanVien
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            List<NhanVien> danhSach = new List<NhanVien>();

            // Khởi tạo sẵn dữ liệu mẫu ban đầu
            danhSach.Add(new NhanVienVanPhong("VP01", "Trần Văn A", 6000000, 22));
            danhSach.Add(new NhanVienVanPhong("VP02", "Lê Thị B", 5500000, 26));
            danhSach.Add(new NhanVienKinhDoanh("KD01", "Nguyễn Văn C", 5000000, 150000000));
            danhSach.Add(new NhanVienKinhDoanh("KD02", "Phạm Thị D", 4500000, 80000000));
            danhSach.Add(new NhanVienThoiVu("TV01", "Hoàng Văn E", 120, 50000));

            int luaChon = -1;
            do
            {
                Console.WriteLine("\n========== MENU ==========");
                Console.WriteLine("1. Nhập thêm nhân viên từ bàn phím");
                Console.WriteLine("2. Xuất danh sách nhân viên");
                Console.WriteLine("3. Tìm nhân viên theo mã");
                Console.WriteLine("4. Tìm nhân viên có lương cao nhất");
                Console.WriteLine("5. Tính tổng lương công ty phải trả");
                Console.WriteLine("0. Thoát");
                Console.Write("Chọn chức năng (0-5): ");

                if (!int.TryParse(Console.ReadLine(), out luaChon))
                {
                    Console.WriteLine("Vui lòng nhập số!");
                    continue;
                }

                switch (luaChon)
                {
                    case 1:
                        NhapThemNhanVien(danhSach);
                        break;
                    case 2:
                        XuatDanhSach(danhSach);
                        break;
                    case 3:
                        TimTheoMa(danhSach);
                        break;
                    case 4:
                        TimLuongCaoNhat(danhSach);
                        break;
                    case 5:
                        TinhTongLuong(danhSach);
                        break;
                    case 0:
                        Console.WriteLine("Đã thoát chương trình.");
                        break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ!");
                        break;
                }

            } while (luaChon != 0);
        }

        static void NhapThemNhanVien(List<NhanVien> danhSach)
        {
            Console.WriteLine("\n--- CHỌN LOẠI NHÂN VIÊN CẦN NHẬP ---");
            Console.WriteLine("1. Nhân viên văn phòng");
            Console.WriteLine("2. Nhân viên kinh doanh");
            Console.WriteLine("3. Nhân viên thời vụ");
            Console.Write("Chọn (1-3): ");

            if (!int.TryParse(Console.ReadLine(), out int loai) || loai < 1 || loai > 3)
            {
                Console.WriteLine("Loại nhân viên không hợp lệ!");
                return;
            }

            NhanVien nv = null;
            if (loai == 1) nv = new NhanVienVanPhong();
            else if (loai == 2) nv = new NhanVienKinhDoanh();
            else if (loai == 3) nv = new NhanVienThoiVu();

            // Tính đa hình: gọi NhapThongTin() tương ứng với từng đối tượng
            nv.NhapThongTin();
            danhSach.Add(nv);

            Console.WriteLine("=> Thêm nhân viên thành công!");
        }

        static void XuatDanhSach(List<NhanVien> danhSach)
        {
            if (danhSach.Count == 0)
            {
                Console.WriteLine("Danh sách trống!");
                return;
            }

            Console.WriteLine("\n" + new string('=', 95));
            Console.WriteLine($"| {"Loại NV",-12} | {"Mã NV",-6} | {"Họ và Tên",-18} | {"Chi tiết công việc",-20} | {"Lương CB",13} | {"Thực Lĩnh",13} |");
            Console.WriteLine(new string('-', 95));

            foreach (var nv in danhSach)
            {
                nv.HienThiThongTin();
            }

            Console.WriteLine(new string('=', 95));
        }

        static void TimTheoMa(List<NhanVien> danhSach)
        {
            Console.Write("Nhập mã nhân viên cần tìm: ");
            string ma = Console.ReadLine()?.Trim() ?? "";

            NhanVien timThay = danhSach.Find(nv => nv.MaNV.Equals(ma, StringComparison.OrdinalIgnoreCase));

            if (timThay != null)
            {
                Console.WriteLine("\nKết quả tìm kiếm:");
                timThay.HienThiThongTin();
            }
            else
            {
                Console.WriteLine($"\nKhông tìm thấy nhân viên có mã: {ma}");
            }
        }

        static void TimLuongCaoNhat(List<NhanVien> danhSach)
        {
            if (danhSach.Count == 0)
            {
                Console.WriteLine("Danh sách trống!");
                return;
            }

            NhanVien maxNV = danhSach[0];
            double maxLuong = danhSach[0].TinhLuong();

            for (int i = 1; i < danhSach.Count; i++)
            {
                double luongHienTai = danhSach[i].TinhLuong();
                if (luongHienTai > maxLuong)
                {
                    maxLuong = luongHienTai;
                    maxNV = danhSach[i];
                }
            }

            Console.WriteLine("\n--- NHÂN VIÊN CÓ LƯƠNG CAO NHẤT ---");
            maxNV.HienThiThongTin();
        }

        static void TinhTongLuong(List<NhanVien> danhSach)
        {
            double tongLuong = 0;
            foreach (var nv in danhSach)
            {
                tongLuong += nv.TinhLuong();
            }

            Console.WriteLine($"\nTổng quỹ lương công ty phải trả: {tongLuong:N0} VNĐ");
        }
    }
}