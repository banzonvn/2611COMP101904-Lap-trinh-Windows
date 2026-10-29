using System;

namespace QLSinhVienLab12.Models
{
    public class LopItem
    {
        public string MaLop { get; set; } = string.Empty;
        public string TenLop { get; set; } = string.Empty;
    }

    public class MonHocItem
    {
        public string MaMon { get; set; } = string.Empty;
        public string TenMon { get; set; } = string.Empty;
        public int SoTinChi { get; set; }
    }

    public class TransactionLogEntry
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Step { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
        public bool IsError { get; set; }
    }
}
