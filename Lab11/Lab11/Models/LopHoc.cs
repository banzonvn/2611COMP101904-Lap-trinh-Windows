using System;

namespace COMP1019_Buoi11_Lab11.Models
{
    public class LopHocItem
    {
        public string MaLop { get; set; } = string.Empty;
        public string TenLop { get; set; } = string.Empty;
        public int SoSinhVien { get; set; }
    }

    public class SinhVienItem
    {
        public string MaSV { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string MaLop { get; set; } = string.Empty;
    }

    public class SqlLogEntry
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string CommandText { get; set; } = string.Empty;
        public string Parameters { get; set; } = string.Empty;
        public double ExecutionTimeMs { get; set; }
        public int RowsAffected { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
