namespace StudentReportDemo.Models
{
    /// <summary>
    /// Data Transfer Object (DTO) lưu trữ kết quả thống kê theo từng lớp được xử lý bằng LINQ GroupBy.
    /// </summary>
    public class ThongKeLopDTO
    {
        public string MaLop { get; set; } = string.Empty;
        public string TenLop { get; set; } = string.Empty;
        public int TongSoSV { get; set; }
        public int SoDat { get; set; }
        public int SoKhongDat { get; set; }
        public double TiLeDatPercent { get; set; }
        public string TiLeDatHienThi => $"{TiLeDatPercent:0.0}%";
        public double DiemTrungBinh { get; set; }
        public double DiemCaoNhat { get; set; }
        public double DiemThapNhat { get; set; }
    }

    /// <summary>
    /// Thống kê tổng quan KPI cho bộ lọc hiện tại
    /// </summary>
    public class ThongKeTongQuanDTO
    {
        public int TongSo { get; set; }
        public int SoDat { get; set; }
        public int SoKhongDat { get; set; }
        public double DiemTB { get; set; }
        public double DiemMax { get; set; }
        public double DiemMin { get; set; }
        public double TiLeDat => TongSo > 0 ? (double)SoDat / TongSo * 100.0 : 0.0;
    }
}
