using System;

namespace Lab02_QuanLyMang
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            int[] arr = null;
            bool daNhapMang = false;
            int luaChon = -1;

            do
            {
                HienThiMenu();
                luaChon = NhapSoNguyen("👉 Chọn chức năng: ");
                Console.WriteLine();

                switch (luaChon)
                {
                    case 1:
                        Console.WriteLine("--- 1. NHẬP MẢNG ---");
                        arr = NhapMang();
                        daNhapMang = true;
                        Console.WriteLine("=> Nhập mảng thành công!");
                        break;

                    case 2:
                        Console.WriteLine("--- 2. XUẤT MẢNG ---");
                        if (!KiemTraDaNhapMang(daNhapMang)) break;
                        XuatMang(arr);
                        break;

                    case 3:
                        Console.WriteLine("--- 3. TÍNH TỔNG CÁC PHẦN TỬ ---");
                        if (!KiemTraDaNhapMang(daNhapMang)) break;
                        XuatMang(arr);
                        int tong = TinhTong(arr);
                        Console.WriteLine($"=> Tổng các phần tử trong mảng: {tong}");
                        break;

                    case 4:
                        Console.WriteLine("--- 4. TÌM LỚN NHẤT VÀ NHỎ NHẤT ---");
                        if (!KiemTraDaNhapMang(daNhapMang)) break;
                        XuatMang(arr);
                        Console.WriteLine($"=> Giá trị lớn nhất (Max): {TimMax(arr)}");
                        Console.WriteLine($"=> Giá trị nhỏ nhất (Min): {TimMin(arr)}");
                        break;

                    case 5:
                        Console.WriteLine("--- 5. ĐẾM SỐ LƯỢNG CHẴN / LẺ ---");
                        if (!KiemTraDaNhapMang(daNhapMang)) break;
                        XuatMang(arr);
                        Console.WriteLine($"=> Số lượng phần tử chẵn: {DemChan(arr)}");
                        Console.WriteLine($"=> Số lượng phần tử lẻ:   {DemLe(arr)}");
                        break;

                    case 6:
                        Console.WriteLine("--- 6. SẮP XẾP TĂNG DẦN ---");
                        if (!KiemTraDaNhapMang(daNhapMang)) break;
                        Console.Write("Mảng trước khi sắp xếp: ");
                        XuatMang(arr);

                        SapXepTangDan(arr);

                        Console.Write("Mảng sau khi sắp xếp:   ");
                        XuatMang(arr);
                        break;

                    case 7:
                        Console.WriteLine("--- 7. TÌM KIẾM PHẦN TỬ ---");
                        if (!KiemTraDaNhapMang(daNhapMang)) break;
                        XuatMang(arr);
                        int x = NhapSoNguyen("Nhập giá trị x cần tìm: ");
                        int viTri = TimKiem(arr, x);
                        if (viTri != -1)
                        {
                            Console.WriteLine($"=> Tìm thấy {x} tại vị trí đầu tiên là: {viTri} (chỉ số index từ 0)");
                        }
                        else
                        {
                            Console.WriteLine($"=> Không tìm thấy giá trị {x} trong mảng!");
                        }
                        break;

                    case 0:
                        Console.WriteLine("Cảm ơn bạn đã sử dụng chương trình. Tạm biệt!");
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("❌ Lựa chọn không hợp lệ! Vui lòng chọn từ 0 đến 7.");
                        Console.ResetColor();
                        break;
                }

                if (luaChon != 0)
                {
                    Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
                    Console.ReadKey();
                }

            } while (luaChon != 0);
        }

        #region Các hàm hiển thị và kiểm tra nhập liệu

        static void HienThiMenu()
        {
            Console.Clear();
            Console.WriteLine("=====================================");
            Console.WriteLine("        BUỔI 2 - LAB 02: MENU        ");
            Console.WriteLine("=====================================");
            Console.WriteLine(" 1. Nhập mảng");
            Console.WriteLine(" 2. Xuất mảng");
            Console.WriteLine(" 3. Tính tổng");
            Console.WriteLine(" 4. Tìm max / min");
            Console.WriteLine(" 5. Đếm chẵn / lẻ");
            Console.WriteLine(" 6. Sắp xếp tăng dần");
            Console.WriteLine(" 7. Tìm kiếm");
            Console.WriteLine(" 0. Thoát");
            Console.WriteLine("=====================================");
        }

        static bool KiemTraDaNhapMang(bool daNhap)
        {
            if (!daNhap)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("⚠️ Cảnh báo: Bạn chưa nhập mảng! Vui lòng chọn chức năng 1 để nhập mảng trước.");
                Console.ResetColor();
                return false;
            }
            return true;
        }

        // Nhập số nguyên bất kỳ, có chống crash
        static int NhapSoNguyen(string message)
        {
            int giaTri;
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine();
                if (int.TryParse(input, out giaTri))
                {
                    return giaTri;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Lỗi: Dữ liệu phải là số nguyên! Vui lòng nhập lại.");
                Console.ResetColor();
            }
        }

        // Nhập số nguyên dương (n > 0), có chống crash
        static int NhapSoNguyenDuong(string message)
        {
            int n;
            while (true)
            {
                n = NhapSoNguyen(message);
                if (n > 0)
                {
                    return n;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Lỗi: Số lượng phần tử phải là số nguyên dương (> 0)! Vui lòng nhập lại.");
                Console.ResetColor();
            }
        }

        #endregion

        #region Các phương thức xử lý mảng theo yêu cầu đề bài

        // 1. Nhập mảng
        static int[] NhapMang()
        {
            int n = NhapSoNguyenDuong("Nhập số lượng phần tử n: ");
            int[] a = new int[n];
            for (int i = 0; i < n; i++)
            {
                a[i] = NhapSoNguyen($"a[{i}] = ");
            }
            return a;
        }

        // 2. Xuất mảng
        static void XuatMang(int[] a)
        {
            Console.WriteLine("Các phần tử trong mảng: " + string.Join("  ", a));
        }

        // 3. Tính tổng
        static int TinhTong(int[] a)
        {
            int tong = 0;
            for (int i = 0; i < a.Length; i++)
            {
                tong += a[i];
            }
            return tong;
        }

        // 4. Tìm Max
        static int TimMax(int[] a)
        {
            int max = a[0];
            for (int i = 1; i < a.Length; i++)
            {
                if (a[i] > max)
                {
                    max = a[i];
                }
            }
            return max;
        }

        // 4. Tìm Min
        static int TimMin(int[] a)
        {
            int min = a[0];
            for (int i = 1; i < a.Length; i++)
            {
                if (a[i] < min)
                {
                    min = a[i];
                }
            }
            return min;
        }

        // 5. Đếm chẵn
        static int DemChan(int[] a)
        {
            int dem = 0;
            foreach (int x in a)
            {
                if (x % 2 == 0) dem++;
            }
            return dem;
        }

        // 5. Đếm lẻ
        static int DemLe(int[] a)
        {
            int dem = 0;
            foreach (int x in a)
            {
                if (x % 2 != 0) dem++;
            }
            return dem;
        }

        // 6. Sắp xếp tăng dần (Bubble Sort)
        static void SapXepTangDan(int[] a)
        {
            for (int i = 0; i < a.Length - 1; i++)
            {
                for (int j = 0; j < a.Length - 1 - i; j++)
                {
                    if (a[j] > a[j + 1])
                    {
                        int temp = a[j];
                        a[j] = a[j + 1];
                        a[j + 1] = temp;
                    }
                }
            }
        }

        // 7. Tìm kiếm phần tử (trả về index đầu tiên, không có trả về -1)
        static int TimKiem(int[] a, int x)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == x)
                {
                    return i;
                }
            }
            return -1;
        }

        #endregion
    }
}