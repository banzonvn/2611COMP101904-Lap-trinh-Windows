using System;

namespace QLSinhVienLab12.Models
{
    public class LopHoc
    {
        public string MaLop { get; set; } = string.Empty;
        public string TenLop { get; set; } = string.Empty;

        public override string ToString() => TenLop;
    }

    public class MonHoc
    {
        public string MaMon { get; set; } = string.Empty;
        public string TenMon { get; set; } = string.Empty;
        public int SoTinChi { get; set; }

        public override string ToString() => $"{TenMon} ({SoTinChi} TC)";
    }

    public class KetQuaItem
    {
        public string MaSV { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public double Diem { get; set; }
        public string GhiChu { get; set; } = string.Empty;
    }

    public class TransactionLogEntry
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string ActionType { get; set; } = string.Empty; // BEGIN, EXEC_SP, COMMIT, ROLLBACK, ERROR
        public string Step { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
        public bool IsError { get; set; }
    }
}
