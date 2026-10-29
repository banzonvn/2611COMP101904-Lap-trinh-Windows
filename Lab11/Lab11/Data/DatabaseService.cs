using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using COMP1019_Buoi11_Lab11.Models;
using Microsoft.Data.SqlClient;

namespace COMP1019_Buoi11_Lab11.Data
{
    public class DatabaseService
    {
        public string ConnectionString { get; set; } =
            @"Data Source=.;Initial Catalog=EduManagerDB;Integrated Security=True;TrustServerCertificate=True";

        public bool IsConnectedToSqlServer { get; private set; } = false;

        public event Action<SqlLogEntry>? OnSqlExecuted;

        // In-Memory dataset replication for fallback/offline testing
        private readonly DataTable _tLop = new("LOP");
        private readonly DataTable _tSinhVien = new("SINHVIEN");

        public DatabaseService()
        {
            InitializeMockDatabase();
            CheckSqlServerConnection();
        }

        private void InitializeMockDatabase()
        {
            _tLop.Reset();
            _tLop.Columns.Add("MaLop", typeof(string));
            _tLop.Columns.Add("TenLop", typeof(string));
            _tLop.PrimaryKey = new[] { _tLop.Columns["MaLop"]! };

            _tSinhVien.Reset();
            _tSinhVien.Columns.Add("MaSV", typeof(string));
            _tSinhVien.Columns.Add("HoTen", typeof(string));
            _tSinhVien.Columns.Add("MaLop", typeof(string));
            _tSinhVien.PrimaryKey = new[] { _tSinhVien.Columns["MaSV"]! };

            // Seed initial data matching EduManagerDB.sql
            _tLop.Rows.Add("CNTT01", "Công nghệ thông tin 01");
            _tLop.Rows.Add("CNTT02", "Công nghệ thông tin 02");
            _tLop.Rows.Add("SPTIN01", "Sư phạm Tin học 01");
            _tLop.Rows.Add("SPTOAN", "Sư phạm Toán học 01");

            _tSinhVien.Rows.Add("51.01.104.082", "Lê Hoàng Quân", "CNTT01");
            _tSinhVien.Rows.Add("SV001", "Nguyễn Văn An", "CNTT01");
            _tSinhVien.Rows.Add("SV002", "Trần Thị Bình", "CNTT01");
            _tSinhVien.Rows.Add("SV003", "Phạm Minh Tuấn", "CNTT02");
            _tSinhVien.Rows.Add("SV004", "Đặng Thu Hà", "CNTT02");
            _tSinhVien.Rows.Add("SV005", "Võ Quốc Bảo", "SPTIN01");

            _tLop.AcceptChanges();
            _tSinhVien.AcceptChanges();
        }

        public void ResetMockData()
        {
            InitializeMockDatabase();
        }

        public bool CheckSqlServerConnection()
        {
            try
            {
                using var conn = new SqlConnection(ConnectionString);
                conn.Open();
                IsConnectedToSqlServer = true;
                return true;
            }
            catch
            {
                IsConnectedToSqlServer = false;
                return false;
            }
        }

        public DataTable LoadData(string keyword = "")
        {
            var sw = Stopwatch.StartNew();
            string sql;
            string paramText;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                sql = @"SELECT l.MaLop, l.TenLop, COUNT(sv.MaSV) AS SoSinhVien
FROM LOP l
LEFT JOIN SINHVIEN sv ON l.MaLop = sv.MaLop
GROUP BY l.MaLop, l.TenLop
ORDER BY l.MaLop ASC";
                paramText = "(none)";
            }
            else
            {
                sql = @"SELECT l.MaLop, l.TenLop, COUNT(sv.MaSV) AS SoSinhVien
FROM LOP l
LEFT JOIN SINHVIEN sv ON l.MaLop = sv.MaLop
WHERE l.MaLop LIKE @kw OR l.TenLop LIKE @kw
GROUP BY l.MaLop, l.TenLop
ORDER BY l.MaLop ASC";
                paramText = $"@kw = '%{keyword.Trim()}%'";
            }

            DataTable dt = new DataTable();

            if (IsConnectedToSqlServer)
            {
                try
                {
                    using var conn = new SqlConnection(ConnectionString);
                    using var cmd = new SqlCommand(sql, conn);
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        cmd.Parameters.AddWithValue("@kw", "%" + keyword.Trim() + "%");
                    }
                    using var da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    sw.Stop();

                    LogSql(sql, paramText, sw.Elapsed.TotalMilliseconds, dt.Rows.Count, true,
                        $"Truy vấn dữ liệu thành công từ SQL Server ({dt.Rows.Count} hàng).");
                    return dt;
                }
                catch (Exception ex)
                {
                    sw.Stop();
                    LogSql(sql, paramText, sw.Elapsed.TotalMilliseconds, 0, false, "Lỗi SQL Server: " + ex.Message);
                    // Fall back to mock
                }
            }

            // In-Memory Mock provider execution
            dt.Columns.Add("MaLop", typeof(string));
            dt.Columns.Add("TenLop", typeof(string));
            dt.Columns.Add("SoSinhVien", typeof(int));

            var kw = keyword?.Trim() ?? string.Empty;
            var query = from l in _tLop.AsEnumerable()
                        where string.IsNullOrWhiteSpace(kw) ||
                              l.Field<string>("MaLop")!.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0 ||
                              l.Field<string>("TenLop")!.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0
                        join sv in _tSinhVien.AsEnumerable() on l.Field<string>("MaLop") equals sv.Field<string>("MaLop") into svGroup
                        orderby l.Field<string>("MaLop")
                        select new
                        {
                            MaLop = l.Field<string>("MaLop"),
                            TenLop = l.Field<string>("TenLop"),
                            SoSinhVien = svGroup.Count()
                        };

            foreach (var item in query)
            {
                dt.Rows.Add(item.MaLop, item.TenLop, item.SoSinhVien);
            }

            sw.Stop();
            LogSql(sql, paramText, Math.Max(1.5, sw.Elapsed.TotalMilliseconds), dt.Rows.Count, true,
                $"Truy vấn hoàn tất (Bộ nhớ giả lập SQL Server): {dt.Rows.Count} hàng.");
            return dt;
        }

        public bool IsMaLopExists(string maLop)
        {
            var sw = Stopwatch.StartNew();
            string sql = "SELECT COUNT(*) FROM LOP WHERE MaLop = @ma";
            string paramText = $"@ma = '{maLop}'";

            if (IsConnectedToSqlServer)
            {
                try
                {
                    using var conn = new SqlConnection(ConnectionString);
                    using var cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ma", maLop);
                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    sw.Stop();
                    LogSql(sql, paramText, sw.Elapsed.TotalMilliseconds, count, true, $"Kiểm tra trùng mã lớp: count={count}");
                    return count > 0;
                }
                catch (Exception ex)
                {
                    sw.Stop();
                    LogSql(sql, paramText, sw.Elapsed.TotalMilliseconds, 0, false, "Lỗi SQL Server: " + ex.Message);
                }
            }

            bool exists = _tLop.AsEnumerable().Any(r => string.Equals(r.Field<string>("MaLop"), maLop, StringComparison.OrdinalIgnoreCase));
            sw.Stop();
            LogSql(sql, paramText, Math.Max(1.0, sw.Elapsed.TotalMilliseconds), exists ? 1 : 0, true,
                $"Kiểm tra trùng mã lớp: {(exists ? "ĐÃ TỒN TẠI" : "HỢP LỆ")}");
            return exists;
        }

        public int CountSinhVienByLop(string maLop)
        {
            var sw = Stopwatch.StartNew();
            string sql = "SELECT COUNT(*) FROM SINHVIEN WHERE MaLop = @ma";
            string paramText = $"@ma = '{maLop}'";

            if (IsConnectedToSqlServer)
            {
                try
                {
                    using var conn = new SqlConnection(ConnectionString);
                    using var cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ma", maLop);
                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    sw.Stop();
                    LogSql(sql, paramText, sw.Elapsed.TotalMilliseconds, count, true, $"Đếm sinh viên lớp {maLop}: {count} SV");
                    return count;
                }
                catch (Exception ex)
                {
                    sw.Stop();
                    LogSql(sql, paramText, sw.Elapsed.TotalMilliseconds, 0, false, "Lỗi SQL Server: " + ex.Message);
                }
            }

            int svCount = _tSinhVien.AsEnumerable().Count(r => string.Equals(r.Field<string>("MaLop"), maLop, StringComparison.OrdinalIgnoreCase));
            sw.Stop();
            LogSql(sql, paramText, Math.Max(1.0, sw.Elapsed.TotalMilliseconds), svCount, true,
                $"Kiểm tra sĩ số sinh viên lớp {maLop}: {svCount} sinh viên");
            return svCount;
        }

        public int InsertLop(string maLop, string tenLop)
        {
            var sw = Stopwatch.StartNew();
            string sql = "INSERT INTO LOP(MaLop, TenLop) VALUES(@ma, @ten)";
            string paramText = $"@ma = '{maLop}', @ten = N'{tenLop}'";

            if (IsConnectedToSqlServer)
            {
                try
                {
                    using var conn = new SqlConnection(ConnectionString);
                    using var cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ma", maLop);
                    cmd.Parameters.AddWithValue("@ten", tenLop);
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    sw.Stop();
                    LogSql(sql, paramText, sw.Elapsed.TotalMilliseconds, rows, true, $"Thêm lớp mới thành công ({rows} row affected).");
                    return rows;
                }
                catch (Exception ex)
                {
                    sw.Stop();
                    LogSql(sql, paramText, sw.Elapsed.TotalMilliseconds, 0, false, "Lỗi thêm lớp: " + ex.Message);
                    throw;
                }
            }

            _tLop.Rows.Add(maLop, tenLop);
            _tLop.AcceptChanges();
            sw.Stop();
            LogSql(sql, paramText, Math.Max(2.0, sw.Elapsed.TotalMilliseconds), 1, true,
                $"INSERT INTO LOP thực thi thành công (1 row affected).");
            return 1;
        }

        public int UpdateLop(string maLop, string tenLop)
        {
            var sw = Stopwatch.StartNew();
            string sql = "UPDATE LOP SET TenLop = @ten WHERE MaLop = @ma";
            string paramText = $"@ten = N'{tenLop}', @ma = '{maLop}'";

            if (IsConnectedToSqlServer)
            {
                try
                {
                    using var conn = new SqlConnection(ConnectionString);
                    using var cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ma", maLop);
                    cmd.Parameters.AddWithValue("@ten", tenLop);
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    sw.Stop();
                    LogSql(sql, paramText, sw.Elapsed.TotalMilliseconds, rows, true, $"Cập nhật lớp thành công ({rows} row affected).");
                    return rows;
                }
                catch (Exception ex)
                {
                    sw.Stop();
                    LogSql(sql, paramText, sw.Elapsed.TotalMilliseconds, 0, false, "Lỗi cập nhật: " + ex.Message);
                    throw;
                }
            }

            var row = _tLop.AsEnumerable().FirstOrDefault(r => string.Equals(r.Field<string>("MaLop"), maLop, StringComparison.OrdinalIgnoreCase));
            if (row != null)
            {
                row["TenLop"] = tenLop;
                _tLop.AcceptChanges();
                sw.Stop();
                LogSql(sql, paramText, Math.Max(2.0, sw.Elapsed.TotalMilliseconds), 1, true,
                    $"UPDATE LOP hoàn tất thành công (1 row affected).");
                return 1;
            }

            sw.Stop();
            LogSql(sql, paramText, Math.Max(1.0, sw.Elapsed.TotalMilliseconds), 0, false,
                "Không tìm thấy mã lớp cần sửa.");
            return 0;
        }

        public int DeleteLop(string maLop)
        {
            var sw = Stopwatch.StartNew();
            string sql = "DELETE FROM LOP WHERE MaLop = @ma";
            string paramText = $"@ma = '{maLop}'";

            if (IsConnectedToSqlServer)
            {
                try
                {
                    using var conn = new SqlConnection(ConnectionString);
                    using var cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ma", maLop);
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    sw.Stop();
                    LogSql(sql, paramText, sw.Elapsed.TotalMilliseconds, rows, true, $"Xóa lớp thành công ({rows} row affected).");
                    return rows;
                }
                catch (Exception ex)
                {
                    sw.Stop();
                    LogSql(sql, paramText, sw.Elapsed.TotalMilliseconds, 0, false, "Lỗi xóa lớp: " + ex.Message);
                    throw;
                }
            }

            var row = _tLop.AsEnumerable().FirstOrDefault(r => string.Equals(r.Field<string>("MaLop"), maLop, StringComparison.OrdinalIgnoreCase));
            if (row != null)
            {
                _tLop.Rows.Remove(row);
                _tLop.AcceptChanges();
                sw.Stop();
                LogSql(sql, paramText, Math.Max(2.0, sw.Elapsed.TotalMilliseconds), 1, true,
                    $"DELETE FROM LOP hoàn tất thành công (1 row affected).");
                return 1;
            }

            sw.Stop();
            LogSql(sql, paramText, Math.Max(1.0, sw.Elapsed.TotalMilliseconds), 0, false,
                "Không tìm thấy mã lớp cần xóa.");
            return 0;
        }

        public DataTable GetSinhVienByLop(string maLop)
        {
            var dt = new DataTable();
            dt.Columns.Add("Mã SV", typeof(string));
            dt.Columns.Add("Họ và Tên", typeof(string));
            dt.Columns.Add("Lớp", typeof(string));

            if (IsConnectedToSqlServer)
            {
                try
                {
                    string sql = "SELECT MaSV AS [Mã SV], HoTen AS [Họ và Tên], MaLop AS [Lớp] FROM SINHVIEN WHERE MaLop = @ma";
                    using var conn = new SqlConnection(ConnectionString);
                    using var cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ma", maLop);
                    using var da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                }
                catch
                {
                    // Fall back to mock
                }
            }

            var rows = _tSinhVien.AsEnumerable().Where(r => string.Equals(r.Field<string>("MaLop"), maLop, StringComparison.OrdinalIgnoreCase));
            foreach (var r in rows)
            {
                dt.Rows.Add(r["MaSV"], r["HoTen"], r["MaLop"]);
            }
            return dt;
        }

        private void LogSql(string command, string parameters, double ms, int affected, bool success, string msg)
        {
            OnSqlExecuted?.Invoke(new SqlLogEntry
            {
                Timestamp = DateTime.Now,
                CommandText = command,
                Parameters = parameters,
                ExecutionTimeMs = ms,
                RowsAffected = affected,
                IsSuccess = success,
                Message = msg
            });
        }
    }
}
