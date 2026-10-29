using System.Text;
using StudentReportDemo.Models;

namespace StudentReportDemo.Services
{
    /// <summary>
    /// BLL / LINQ Engine chuyên trách xử lý toàn bộ các thao tác nghiệp vụ,
    /// truy vấn lọc, sắp xếp, gom nhóm, thống kê và kết xuất tệp tin báo cáo.
    /// Tách rời hoàn toàn khỏi Form để đảm bảo code sạch (Clean Architecture).
    /// </summary>
    public class ReportEngine
    {
        /// <summary>
        /// Khởi tạo dữ liệu mẫu phong phú với 16 sinh viên thuộc 4 lớp khác nhau.
        /// Bao gồm sinh viên Lê Hoàng Quân (MSSV: 51.01.104.082) với thành tích xuất sắc.
        /// </summary>
        public static List<SinhVien> TaoDuLieuMau()
        {
            return new List<SinhVien>
            {
                // Lớp CTK47A (CNTT K47A)
                new SinhVien { MaSV = "51.01.104.082", HoTen = "Lê Hoàng Quân", MaLop = "CTK47A", TenLop = "CNTT K47A", Diem = 9.8 },
                new SinhVien { MaSV = "SV001", HoTen = "Nguyễn Văn An", MaLop = "CTK47A", TenLop = "CNTT K47A", Diem = 8.5 },
                new SinhVien { MaSV = "SV002", HoTen = "Trần Thị Bình", MaLop = "CTK47A", TenLop = "CNTT K47A", Diem = 6.8 },
                new SinhVien { MaSV = "SV003", HoTen = "Lê Minh Cường", MaLop = "CTK47A", TenLop = "CNTT K47A", Diem = 4.2 },
                new SinhVien { MaSV = "SV004", HoTen = "Phạm Thanh Dung", MaLop = "CTK47A", TenLop = "CNTT K47A", Diem = 9.0 },

                // Lớp CTK47B (CNTT K47B)
                new SinhVien { MaSV = "SV005", HoTen = "Hoàng Quốc Huy", MaLop = "CTK47B", TenLop = "CNTT K47B", Diem = 7.4 },
                new SinhVien { MaSV = "SV006", HoTen = "Võ Gia Khang", MaLop = "CTK47B", TenLop = "CNTT K47B", Diem = 3.8 },
                new SinhVien { MaSV = "SV007", HoTen = "Đặng Minh Khôi", MaLop = "CTK47B", TenLop = "CNTT K47B", Diem = 5.6 },
                new SinhVien { MaSV = "SV008", HoTen = "Bùi Thảo Linh", MaLop = "CTK47B", TenLop = "CNTT K47B", Diem = 8.2 },

                // Lớp CTK48A (CNTT K48A)
                new SinhVien { MaSV = "SV009", HoTen = "Ngô Tuấn Minh", MaLop = "CTK48A", TenLop = "CNTT K48A", Diem = 6.2 },
                new SinhVien { MaSV = "SV010", HoTen = "Đỗ Bảo Ngọc", MaLop = "CTK48A", TenLop = "CNTT K48A", Diem = 9.4 },
                new SinhVien { MaSV = "SV011", HoTen = "Huỳnh Gia Phúc", MaLop = "CTK48A", TenLop = "CNTT K48A", Diem = 4.5 },
                new SinhVien { MaSV = "SV012", HoTen = "Mai Anh Thư", MaLop = "CTK48A", TenLop = "CNTT K48A", Diem = 7.9 },

                // Lớp 51.CNTT.A (Sư phạm Tin học K51A)
                new SinhVien { MaSV = "SV013", HoTen = "Vũ Đình Trọng", MaLop = "51.CNTT.A", TenLop = "SP Tin K51A", Diem = 8.7 },
                new SinhVien { MaSV = "SV014", HoTen = "Nguyễn Thùy Trang", MaLop = "51.CNTT.A", TenLop = "SP Tin K51A", Diem = 4.8 },
                new SinhVien { MaSV = "SV015", HoTen = "Dương Nhật Nam", MaLop = "51.CNTT.A", TenLop = "SP Tin K51A", Diem = 7.1 }
            };
        }

        /// <summary>
        /// Sử dụng LINQ pipeline thực hiện lọc, tìm kiếm và sắp xếp kết quả báo cáo.
        /// </summary>
        public static List<SinhVien> LocBaoCao(
            IEnumerable<SinhVien> nguon,
            string? maLop,
            string? ketQua,
            string? tuKhoa)
        {
            var query = nguon.AsQueryable();

            // 1. Lọc theo lớp học (Nếu chọn lớp cụ thể)
            if (!string.IsNullOrWhiteSpace(maLop) && maLop != "ALL")
            {
                query = query.Where(sv => sv.MaLop == maLop);
            }

            // 2. Lọc theo điều kiện kết quả học tập
            if (string.Equals(ketQua, "Đạt", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(sv => sv.Diem >= 5.0);
            }
            else if (string.Equals(ketQua, "Không đạt", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(sv => sv.Diem < 5.0);
            }

            // 3. Tìm kiếm theo từ khóa (Mã sinh viên hoặc Họ và tên không phân biệt hoa thường)
            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                string normKey = tuKhoa.Trim().ToLower();
                query = query.Where(sv =>
                    sv.MaSV.ToLower().Contains(normKey) ||
                    sv.HoTen.ToLower().Contains(normKey));
            }

            // 4. Sắp xếp giảm dần theo điểm trung bình, sau đó sắp xếp theo họ tên
            return query
                .OrderByDescending(sv => sv.Diem)
                .ThenBy(sv => sv.HoTen)
                .ToList();
        }

        /// <summary>
        /// Sử dụng LINQ GroupBy để tổng hợp chỉ số thống kê chi tiết theo từng lớp.
        /// </summary>
        public static List<ThongKeLopDTO> ThongKeTheoLop(IEnumerable<SinhVien> nguon)
        {
            return nguon
                .GroupBy(sv => new { sv.MaLop, sv.TenLop })
                .Select(g =>
                {
                    int tong = g.Count();
                    int dat = g.Count(sv => sv.Diem >= 5.0);
                    int khongDat = tong - dat;
                    double tile = tong > 0 ? (double)dat / tong * 100.0 : 0.0;
                    double dtb = tong > 0 ? g.Average(sv => sv.Diem) : 0.0;
                    double dmax = tong > 0 ? g.Max(sv => sv.Diem) : 0.0;
                    double dmin = tong > 0 ? g.Min(sv => sv.Diem) : 0.0;

                    return new ThongKeLopDTO
                    {
                        MaLop = g.Key.MaLop,
                        TenLop = g.Key.TenLop,
                        TongSoSV = tong,
                        SoDat = dat,
                        SoKhongDat = khongDat,
                        TiLeDatPercent = Math.Round(tile, 1),
                        DiemTrungBinh = Math.Round(dtb, 2),
                        DiemCaoNhat = Math.Round(dmax, 2),
                        DiemThapNhat = Math.Round(dmin, 2)
                    };
                })
                .OrderBy(x => x.MaLop)
                .ToList();
        }

        /// <summary>
        /// Tính toán thống kê nhanh KPI cho tập dữ liệu đang lọc.
        /// </summary>
        public static ThongKeTongQuanDTO ThongKeTongQuan(IEnumerable<SinhVien> danhSach)
        {
            var list = danhSach.ToList();
            int count = list.Count;
            if (count == 0)
            {
                return new ThongKeTongQuanDTO();
            }

            int dat = list.Count(sv => sv.Diem >= 5.0);
            return new ThongKeTongQuanDTO
            {
                TongSo = count,
                SoDat = dat,
                SoKhongDat = count - dat,
                DiemTB = Math.Round(list.Average(sv => sv.Diem), 2),
                DiemMax = Math.Round(list.Max(sv => sv.Diem), 2),
                DiemMin = Math.Round(list.Min(sv => sv.Diem), 2)
            };
        }

        /// <summary>
        /// Xuất danh sách sinh viên ra định dạng CSV chuẩn RFC-4180 với UTF-8 BOM.
        /// Tương thích 100% với Microsoft Excel tiếng Việt.
        /// </summary>
        public static void XuatFileCsv(IEnumerable<SinhVien> data, string filePath)
        {
            var sb = new StringBuilder();
            sb.AppendLine("STT,Mã SV,Họ và Tên,Mã Lớp,Tên Lớp,Điểm TB,Kết Quả,Xếp Loại");

            int stt = 1;
            foreach (var sv in data)
            {
                string safeName = EscapeCsvCell(sv.HoTen);
                string safeClass = EscapeCsvCell(sv.TenLop);
                sb.AppendLine($"{stt++},{sv.MaSV},{safeName},{sv.MaLop},{safeClass},{sv.Diem:0.0},{sv.KetQua},{sv.XepLoai}");
            }

            File.WriteAllText(filePath, sb.ToString(), new UTF8Encoding(true));
        }

        /// <summary>
        /// Xuất bảng thống kê theo lớp ra file CSV.
        /// </summary>
        public static void XuatFileCsvThongKe(IEnumerable<ThongKeLopDTO> data, string filePath)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Mã Lớp,Tên Lớp,Tổng Sinh Viên,Số Lượng Đạt,Số Không Đạt,Tỉ Lệ Đạt (%),Điểm TB,Điểm Cao Nhất,Điểm Thấp Nhất");

            foreach (var tk in data)
            {
                string safeName = EscapeCsvCell(tk.TenLop);
                sb.AppendLine($"{tk.MaLop},{safeName},{tk.TongSoSV},{tk.SoDat},{tk.SoKhongDat},{tk.TiLeDatPercent:0.0}%,{tk.DiemTrungBinh:0.00},{tk.DiemCaoNhat:0.0},{tk.DiemThapNhat:0.0}");
            }

            File.WriteAllText(filePath, sb.ToString(), new UTF8Encoding(true));
        }

        /// <summary>
        /// Xuất báo cáo văn bản chuyên nghiệp định dạng TXT có cấu trúc khung bảng ASCII,
        /// tiêu đề trường học, tham số lọc và chữ ký xác nhận.
        /// </summary>
        public static void XuatFileTxt(
            IEnumerable<SinhVien> data,
            string tenLop,
            string dieuKien,
            string tuKhoa,
            string filePath)
        {
            var list = data.ToList();
            var stats = ThongKeTongQuan(list);

            var sb = new StringBuilder();
            sb.AppendLine("==========================================================================================");
            sb.AppendLine("                 TRƯỜNG ĐẠI HỌC SƯ PHẠM THÀNH PHỐ HỒ CHÍ MINH");
            sb.AppendLine("                             KHOA CÔNG NGHỆ THÔNG TIN");
            sb.AppendLine("==========================================================================================");
            sb.AppendLine();
            sb.AppendLine("                         BÁO CÁO KẾT QUẢ HỌC TẬP SINH VIÊN");
            sb.AppendLine($"                    Lớp: {tenLop.ToUpper()} - Điều kiện: {dieuKien}");
            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                sb.AppendLine($"                    Từ khóa tìm kiếm: \"{tuKhoa}\"");
            }
            sb.AppendLine($"                    Thời gian lập báo cáo: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            sb.AppendLine();
            sb.AppendLine("+-----+---------------+---------------------------+------------+-------+-----------+------------+");
            sb.AppendLine("| STT | Mã SV         | Họ và Tên                 | Tên Lớp    | Điểm  | Kết Quả   | Xếp Loại   |");
            sb.AppendLine("+-----+---------------+---------------------------+------------+-------+-----------+------------+");

            int stt = 1;
            foreach (var sv in list)
            {
                sb.AppendLine($"| {stt++,3} | {sv.MaSV,-13} | {sv.HoTen,-25} | {sv.TenLop,-10} | {sv.Diem,5:0.0} | {sv.KetQua,-9} | {sv.XepLoai,-10} |");
            }

            sb.AppendLine("+-----+---------------+---------------------------+------------+-------+-----------+------------+");
            sb.AppendLine();
            sb.AppendLine("--- THỐNG KÊ TỔNG QUAN ---");
            sb.AppendLine($"* Tổng số sinh viên hiển thị : {stats.TongSo} sinh viên");
            sb.AppendLine($"* Số lượng sinh viên ĐẠT     : {stats.SoDat} sinh viên ({stats.TiLeDat:0.0}%)");
            sb.AppendLine($"* Số sinh viên KHÔNG ĐẠT     : {stats.SoKhongDat} sinh viên");
            sb.AppendLine($"* Điểm trung bình nhóm lọc   : {stats.DiemTB:0.00} điểm");
            if (stats.TongSo > 0)
            {
                sb.AppendLine($"* Điểm cao nhất / Thấp nhất  : {stats.DiemMax:0.0} / {stats.DiemMin:0.0} điểm");
            }
            sb.AppendLine();
            sb.AppendLine("                                                TP. Hồ Chí Minh, Ngày .... Tháng .... Năm 2026");
            sb.AppendLine("        NGƯỜI LẬP BÁO CÁO                                    TRƯỞNG KHOA / GIẢNG VIÊN");
            sb.AppendLine("          (Ký và ghi rõ họ tên)                                 (Ký và ghi rõ họ tên)");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("           Lê Hoàng Quân                                          ThS. Lê Thanh Thoại");
            sb.AppendLine("         MSSV: 51.01.104.082");
            sb.AppendLine("==========================================================================================");

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        private static string EscapeCsvCell(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            if (text.Contains(',') || text.Contains('"') || text.Contains('\n') || text.Contains('\r'))
            {
                return $"\"{text.Replace("\"", "\"\"")}\"";
            }
            return text;
        }
    }
}
