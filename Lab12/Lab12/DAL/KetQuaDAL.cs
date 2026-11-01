using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using QLSinhVienLab12.Models;

namespace QLSinhVienLab12.DAL
{
    public class KetQuaDAL
    {
        public event Action<TransactionLogEntry>? OnTransactionLog;

        public bool IsSqlServerConnected { get; private set; } = false;

        // In-Memory mock storage to guarantee seamless offline testing & automated capture
        private readonly DataTable _mockLop = new("LOP");
        private readonly DataTable _mockMonHoc = new("MONHOC");
        private readonly DataTable _mockSinhVien = new("SINHVIEN");
        private readonly DataTable _mockKetQua = new("KETQUA");

        public KetQuaDAL()
        {
            InitializeMockDatabase();
            CheckConnection();
        }

        public bool CheckConnection()
        {
            if (Database.CheckConnection(Database.ConnectionString))
            {
                IsSqlServerConnected = true;
                return true;
            }
            if (Database.CheckConnection(Database.ExpressConnectionString))
            {
                Database.ConnectionString = Database.ExpressConnectionString;
                IsSqlServerConnected = true;
                return true;
            }

            IsSqlServerConnected = false;
            return false;
        }

        private void InitializeMockDatabase()
        {
            _mockLop.Reset();
            _mockLop.Columns.Add("MaLop", typeof(string));
            _mockLop.Columns.Add("TenLop", typeof(string));
            _mockLop.PrimaryKey = new[] { _mockLop.Columns["MaLop"]! };

            _mockMonHoc.Reset();
            _mockMonHoc.Columns.Add("MaMon", typeof(string));
            _mockMonHoc.Columns.Add("TenMon", typeof(string));
            _mockMonHoc.Columns.Add("SoTinChi", typeof(int));
            _mockMonHoc.PrimaryKey = new[] { _mockMonHoc.Columns["MaMon"]! };

            _mockSinhVien.Reset();
            _mockSinhVien.Columns.Add("MaSV", typeof(string));
            _mockSinhVien.Columns.Add("HoTen", typeof(string));
            _mockSinhVien.Columns.Add("NgaySinh", typeof(DateTime));
            _mockSinhVien.Columns.Add("MaLop", typeof(string));
            _mockSinhVien.PrimaryKey = new[] { _mockSinhVien.Columns["MaSV"]! };

            _mockKetQua.Reset();
            _mockKetQua.Columns.Add("MaSV", typeof(string));
            _mockKetQua.Columns.Add("MaMon", typeof(string));
            _mockKetQua.Columns.Add("Diem", typeof(double));
            _mockKetQua.Columns.Add("GhiChu", typeof(string));
            _mockKetQua.PrimaryKey = new[] { _mockKetQua.Columns["MaSV"]!, _mockKetQua.Columns["MaMon"]! };

            // Seed sample data matching QLSinhVienLab12.sql
            _mockLop.Rows.Add("51.CNTT.A", "Cử nhân Công nghệ Thông tin Khóa 51A");
            _mockLop.Rows.Add("CTK47A", "Công nghệ thông tin K47A");
            _mockLop.Rows.Add("CTK47B", "Công nghệ thông tin K47B");

            _mockSinhVien.Rows.Add("51.01.104.082", "Lê Hoàng Quân", new DateTime(2003, 5, 18), "51.CNTT.A");
            _mockSinhVien.Rows.Add("SV005", "Võ Quốc Bảo", new DateTime(2003, 11, 22), "51.CNTT.A");
            _mockSinhVien.Rows.Add("SV006", "Đặng Thu Hà", new DateTime(2003, 8, 14), "51.CNTT.A");

            _mockSinhVien.Rows.Add("SV001", "Nguyễn Văn An", new DateTime(2005, 1, 15), "CTK47A");
            _mockSinhVien.Rows.Add("SV002", "Trần Thị Bình", new DateTime(2005, 5, 20), "CTK47A");
            _mockSinhVien.Rows.Add("SV003", "Lê Minh Cường", new DateTime(2005, 3, 11), "CTK47A");
            _mockSinhVien.Rows.Add("SV004", "Phạm Thanh Dung", new DateTime(2005, 7, 9), "CTK47B");

            _mockMonHoc.Rows.Add("COMP1019", "Lập trình trên Windows", 3);
            _mockMonHoc.Rows.Add("COMP1017", "Cơ sở lập trình", 3);
            _mockMonHoc.Rows.Add("COMP1020", "Cấu trúc dữ liệu và giải thuật", 4);

            _mockKetQua.Rows.Add("51.01.104.082", "COMP1019", 9.5, "Xuất sắc - Hoàn thành bài sớm");
            _mockKetQua.Rows.Add("51.01.104.082", "COMP1017", 9.0, "Giỏi");
            _mockKetQua.Rows.Add("SV001", "COMP1019", 8.5, "Khá giỏi");
            _mockKetQua.Rows.Add("SV002", "COMP1019", 7.0, "Đạt yêu cầu");
            _mockKetQua.Rows.Add("SV004", "COMP1017", 8.0, "Tốt");

            _mockLop.AcceptChanges();
            _mockMonHoc.AcceptChanges();
            _mockSinhVien.AcceptChanges();
            _mockKetQua.AcceptChanges();
        }

        public void ResetMockData()
        {
            InitializeMockDatabase();
            LogEvent("RESET", "Khôi phục dữ liệu", "Đã tái thiết lập dữ liệu mẫu ban đầu của QLSinhVienLab12.", false);
        }

        public DataTable GetLop()
        {
            if (IsSqlServerConnected)
            {
                try
                {
                    using SqlConnection conn = new SqlConnection(Database.ConnectionString);
                    using SqlCommand cmd = new SqlCommand("sp_Lop_GetAll", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    LogEvent("EXEC_SP", "sp_Lop_GetAll", $"Nạp thành công {table.Rows.Count} lớp học từ SQL Server.", false);
                    return table;
                }
                catch (Exception ex)
                {
                    LogEvent("ERROR", "sp_Lop_GetAll", "Lỗi SQL: " + ex.Message, true);
                    IsSqlServerConnected = false;
                }
            }

            // Fallback in-memory
            var dt = new DataTable();
            dt.Columns.Add("MaLop", typeof(string));
            dt.Columns.Add("TenLop", typeof(string));
            foreach (var r in _mockLop.AsEnumerable().OrderBy(r => r.Field<string>("TenLop")))
            {
                dt.Rows.Add(r["MaLop"], r["TenLop"]);
            }
            LogEvent("EXEC_SP", "sp_Lop_GetAll (Mock)", $"Nạp {dt.Rows.Count} lớp học từ bộ nhớ giả lập.", false);
            return dt;
        }

        public DataTable GetMonHoc()
        {
            if (IsSqlServerConnected)
            {
                try
                {
                    using SqlConnection conn = new SqlConnection(Database.ConnectionString);
                    using SqlCommand cmd = new SqlCommand("sp_MonHoc_GetAll", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    LogEvent("EXEC_SP", "sp_MonHoc_GetAll", $"Nạp thành công {table.Rows.Count} môn học từ SQL Server.", false);
                    return table;
                }
                catch (Exception ex)
                {
                    LogEvent("ERROR", "sp_MonHoc_GetAll", "Lỗi SQL: " + ex.Message, true);
                    IsSqlServerConnected = false;
                }
            }

            var dt = new DataTable();
            dt.Columns.Add("MaMon", typeof(string));
            dt.Columns.Add("TenMon", typeof(string));
            dt.Columns.Add("SoTinChi", typeof(int));
            foreach (var r in _mockMonHoc.AsEnumerable().OrderBy(r => r.Field<string>("TenMon")))
            {
                dt.Rows.Add(r["MaMon"], r["TenMon"], r["SoTinChi"]);
            }
            LogEvent("EXEC_SP", "sp_MonHoc_GetAll (Mock)", $"Nạp {dt.Rows.Count} môn học từ bộ nhớ giả lập.", false);
            return dt;
        }

        public DataTable GetKetQuaByLopMon(string maLop, string maMon)
        {
            if (IsSqlServerConnected)
            {
                try
                {
                    using SqlConnection conn = new SqlConnection(Database.ConnectionString);
                    using SqlCommand cmd = new SqlCommand("sp_KetQua_GetByLopMon", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaLop", maLop);
                    cmd.Parameters.AddWithValue("@MaMon", maMon);
                    using SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    LogEvent("EXEC_SP", "sp_KetQua_GetByLopMon",
                        $"Truy vấn sinh viên Lớp='{maLop}', Môn='{maMon}': {table.Rows.Count} dòng kết quả.", false);
                    return table;
                }
                catch (Exception ex)
                {
                    LogEvent("ERROR", "sp_KetQua_GetByLopMon", "Lỗi SQL: " + ex.Message, true);
                    IsSqlServerConnected = false;
                }
            }

            // In-Memory LEFT JOIN simulation as in sp_KetQua_GetByLopMon
            var dt = new DataTable();
            dt.Columns.Add("MaSV", typeof(string));
            dt.Columns.Add("HoTen", typeof(string));
            dt.Columns.Add("Diem", typeof(object));
            dt.Columns.Add("GhiChu", typeof(string));

            var svList = _mockSinhVien.AsEnumerable()
                .Where(s => string.Equals(s.Field<string>("MaLop"), maLop, StringComparison.OrdinalIgnoreCase))
                .OrderBy(s => s.Field<string>("HoTen"));

            foreach (var sv in svList)
            {
                string msv = sv.Field<string>("MaSV")!;
                string hoTen = sv.Field<string>("HoTen")!;

                var kqRow = _mockKetQua.AsEnumerable().FirstOrDefault(k =>
                    string.Equals(k.Field<string>("MaSV"), msv, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(k.Field<string>("MaMon"), maMon, StringComparison.OrdinalIgnoreCase));

                double diem = kqRow != null ? kqRow.Field<double>("Diem") : 0.0;
                string ghiChu = kqRow != null ? (kqRow.Field<string>("GhiChu") ?? "") : "";

                dt.Rows.Add(msv, hoTen, diem, ghiChu);
            }

            LogEvent("EXEC_SP", "sp_KetQua_GetByLopMon (Mock)",
                $"Truy vấn LEFT JOIN Lớp='{maLop}', Môn='{maMon}': {dt.Rows.Count} sinh viên.", false);
            return dt;
        }

        public int SaveAll(string maMon, DataTable table)
        {
            LogEvent("BEGIN", "SqlTransaction", "Bắt đầu giao dịch SqlTransaction trên kết nối cơ sở dữ liệu...", false);

            if (IsSqlServerConnected)
            {
                using SqlConnection conn = new SqlConnection(Database.ConnectionString);
                conn.Open();
                using SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    int count = 0;
                    foreach (DataRow row in table.Rows)
                    {
                        if (row.RowState == DataRowState.Deleted) continue;

                        string maSV = row["MaSV"].ToString() ?? "";
                        double diem = Convert.ToDouble(row["Diem"]);
                        string ghiChu = row["GhiChu"]?.ToString() ?? "";

                        using SqlCommand cmd = new SqlCommand("sp_KetQua_Save", conn, trans);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaSV", maSV);
                        cmd.Parameters.AddWithValue("@MaMon", maMon);
                        cmd.Parameters.AddWithValue("@Diem", diem);
                        cmd.Parameters.AddWithValue("@GhiChu", string.IsNullOrWhiteSpace(ghiChu) ? (object)DBNull.Value : ghiChu);
                        cmd.ExecuteNonQuery();

                        count++;
                        LogEvent("EXEC_SP", "sp_KetQua_Save", $"[TRANS-ENLISTED] Lưu điểm MaSV='{maSV}', Diem={diem:F1}", false);
                    }

                    trans.Commit();
                    LogEvent("COMMIT", "SqlTransaction", $"Giao dịch COMMIT thành công! Đã lưu vĩnh viễn {count} dòng kết quả.", false);
                    return count;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    LogEvent("ROLLBACK", "SqlTransaction", $"Giao dịch bị ROLLBACK toàn bộ do lỗi: {ex.Message}", true);
                    throw;
                }
            }

            // In-Memory atomic transaction simulation
            var backupList = new List<(string MaSV, string MaMon, double Diem, string GhiChu)>();
            foreach (DataRow r in _mockKetQua.Rows)
            {
                backupList.Add((r.Field<string>("MaSV")!, r.Field<string>("MaMon")!, r.Field<double>("Diem"), r.Field<string>("GhiChu") ?? ""));
            }

            try
            {
                int count = 0;
                foreach (DataRow row in table.Rows)
                {
                    if (row.RowState == DataRowState.Deleted) continue;

                    string maSV = row["MaSV"].ToString() ?? "";
                    double diem = Convert.ToDouble(row["Diem"]);
                    string ghiChu = row["GhiChu"]?.ToString() ?? "";

                    // Validate constraint Diem >= 0 and <= 10
                    if (diem < 0.0 || diem > 10.0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(diem),
                            $"Vi phạm ràng buộc CK_KETQUA_Diem: Điểm số ({diem}) phải nằm trong miền [0.0 - 10.0]!");
                    }

                    var existing = _mockKetQua.AsEnumerable().FirstOrDefault(k =>
                        string.Equals(k.Field<string>("MaSV"), maSV, StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(k.Field<string>("MaMon"), maMon, StringComparison.OrdinalIgnoreCase));

                    if (existing != null)
                    {
                        existing["Diem"] = diem;
                        existing["GhiChu"] = ghiChu;
                    }
                    else
                    {
                        _mockKetQua.Rows.Add(maSV, maMon, diem, ghiChu);
                    }

                    count++;
                    LogEvent("EXEC_SP", "sp_KetQua_Save (Mock)", $"[TRANS-STEP] Đã nạp điểm MaSV='{maSV}', Diem={diem:F1}", false);
                }

                _mockKetQua.AcceptChanges();
                LogEvent("COMMIT", "SqlTransaction (Mock)", $"Giao dịch COMMIT thành công! Đã ghi nhận {count} dòng.", false);
                return count;
            }
            catch (Exception ex)
            {
                // Rollback in-memory
                _mockKetQua.Rows.Clear();
                foreach (var b in backupList)
                {
                    _mockKetQua.Rows.Add(b.MaSV, b.MaMon, b.Diem, b.GhiChu);
                }
                _mockKetQua.AcceptChanges();

                LogEvent("ROLLBACK", "SqlTransaction (Mock)", $"Đã ROLLBACK toàn bộ thay đổi do lỗi: {ex.Message}", true);
                throw;
            }
        }

        private void LogEvent(string action, string step, string detail, bool isError)
        {
            OnTransactionLog?.Invoke(new TransactionLogEntry
            {
                Timestamp = DateTime.Now,
                ActionType = action,
                Step = step,
                Detail = detail,
                IsError = isError
            });
        }
    }
}
