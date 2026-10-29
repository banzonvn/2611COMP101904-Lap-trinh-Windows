using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Microsoft.Data.SqlClient;
using QLSinhVienLab12.Models;

namespace QLSinhVienLab12.DAL
{
    public class KetQuaDAL
    {
        public bool IsConnectedToSqlServer { get; private set; } = false;

        public event Action<TransactionLogEntry>? OnTransactionLog;

        // In-Memory dataset backup for fallback & testing
        private readonly DataTable _tLop = new("LOP");
        private readonly DataTable _tMonHoc = new("MONHOC");
        private readonly DataTable _tSinhVien = new("SINHVIEN");
        private readonly DataTable _tKetQua = new("KETQUA");

        public KetQuaDAL()
        {
            InitializeMockDatabase();
            CheckConnection();
        }

        public bool CheckConnection()
        {
            try
            {
                using var conn = new SqlConnection(Database.ConnectionString);
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

        public void InitializeMockDatabase()
        {
            _tLop.Reset();
            _tLop.Columns.Add("MaLop", typeof(string));
            _tLop.Columns.Add("TenLop", typeof(string));
            _tLop.PrimaryKey = new[] { _tLop.Columns["MaLop"]! };

            _tMonHoc.Reset();
            _tMonHoc.Columns.Add("MaMon", typeof(string));
            _tMonHoc.Columns.Add("TenMon", typeof(string));
            _tMonHoc.Columns.Add("SoTinChi", typeof(int));
            _tMonHoc.PrimaryKey = new[] { _tMonHoc.Columns["MaMon"]! };

            _tSinhVien.Reset();
            _tSinhVien.Columns.Add("MaSV", typeof(string));
            _tSinhVien.Columns.Add("HoTen", typeof(string));
            _tSinhVien.Columns.Add("NgaySinh", typeof(DateTime));
            _tSinhVien.Columns.Add("MaLop", typeof(string));
            _tSinhVien.PrimaryKey = new[] { _tSinhVien.Columns["MaSV"]! };

            _tKetQua.Reset();
            _tKetQua.Columns.Add("MaSV", typeof(string));
            _tKetQua.Columns.Add("MaMon", typeof(string));
            _tKetQua.Columns.Add("Diem", typeof(double));
            _tKetQua.Columns.Add("GhiChu", typeof(string));
            _tKetQua.PrimaryKey = new[] { _tKetQua.Columns["MaSV"]!, _tKetQua.Columns["MaMon"]! };

            // Seed LOP
            _tLop.Rows.Add("51.CNTT.A", "Cử nhân Công nghệ Thông tin Khóa 51A");
            _tLop.Rows.Add("CTK47A", "Công nghệ thông tin K47A");
            _tLop.Rows.Add("CTK47B", "Công nghệ thông tin K47B");

            // Seed MONHOC
            _tMonHoc.Rows.Add("COMP1019", "Lập trình trên Windows", 3);
            _tMonHoc.Rows.Add("COMP1017", "Cơ sở dữ liệu & ADO.NET", 3);
            _tMonHoc.Rows.Add("COMP1020", "Cấu trúc dữ liệu & Giải thuật", 4);

            // Seed SINHVIEN
            _tSinhVien.Rows.Add("51.01.104.082", "Lê Hoàng Quân", new DateTime(2003, 5, 18), "51.CNTT.A");
            _tSinhVien.Rows.Add("SV001", "Nguyễn Văn An", new DateTime(2005, 1, 15), "51.CNTT.A");
            _tSinhVien.Rows.Add("SV002", "Trần Thị Bình", new DateTime(2005, 5, 20), "51.CNTT.A");
            _tSinhVien.Rows.Add("SV003", "Lê Minh Cường", new DateTime(2005, 3, 11), "CTK47A");
            _tSinhVien.Rows.Add("SV004", "Phạm Thanh Dung", new DateTime(2005, 7, 9), "CTK47A");
            _tSinhVien.Rows.Add("SV005", "Hoàng Ngọc Ánh", new DateTime(2005, 11, 25), "CTK47B");

            // Seed KETQUA
            _tKetQua.Rows.Add("51.01.104.082", "COMP1019", 9.5, "Xuất sắc");
            _tKetQua.Rows.Add("SV001", "COMP1019", 8.5, "Giỏi");
            _tKetQua.Rows.Add("SV002", "COMP1019", 7.0, "Khá");
            _tKetQua.Rows.Add("SV003", "COMP1017", 8.0, "Giỏi");

            _tLop.AcceptChanges();
            _tMonHoc.AcceptChanges();
            _tSinhVien.AcceptChanges();
            _tKetQua.AcceptChanges();
        }

        public DataTable GetLop()
        {
            if (IsConnectedToSqlServer)
            {
                try
                {
                    using SqlConnection conn = new SqlConnection(Database.ConnectionString);
                    using SqlCommand cmd = new SqlCommand("sp_Lop_GetAll", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    Log("sp_Lop_GetAll", $"Tải thành công {table.Rows.Count} lớp từ SQL Server.", false);
                    return table;
                }
                catch (Exception ex)
                {
                    Log("sp_Lop_GetAll (Lỗi)", ex.Message, true);
                }
            }

            // Fallback
            DataTable dt = _tLop.Copy();
            Log("sp_Lop_GetAll (Mock)", $"Tải {dt.Rows.Count} lớp học từ bộ nhớ.", false);
            return dt;
        }

        public DataTable GetMonHoc()
        {
            if (IsConnectedToSqlServer)
            {
                try
                {
                    using SqlConnection conn = new SqlConnection(Database.ConnectionString);
                    using SqlCommand cmd = new SqlCommand("sp_MonHoc_GetAll", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    Log("sp_MonHoc_GetAll", $"Tải thành công {table.Rows.Count} môn học từ SQL Server.", false);
                    return table;
                }
                catch (Exception ex)
                {
                    Log("sp_MonHoc_GetAll (Lỗi)", ex.Message, true);
                }
            }

            // Fallback
            DataTable dt = _tMonHoc.Copy();
            Log("sp_MonHoc_GetAll (Mock)", $"Tải {dt.Rows.Count} môn học từ bộ nhớ.", false);
            return dt;
        }

        public DataTable GetKetQuaByLopMon(string maLop, string maMon)
        {
            if (IsConnectedToSqlServer)
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
                    Log("sp_KetQua_GetByLopMon", $"Tải kết quả học tập lớp '{maLop}', môn '{maMon}': {table.Rows.Count} sinh viên.", false);
                    return table;
                }
                catch (Exception ex)
                {
                    Log("sp_KetQua_GetByLopMon (Lỗi)", ex.Message, true);
                }
            }

            // Fallback: Simulate sp_KetQua_GetByLopMon using LEFT JOIN
            DataTable dt = new DataTable();
            dt.Columns.Add("MaSV", typeof(string));
            dt.Columns.Add("HoTen", typeof(string));
            dt.Columns.Add("Diem", typeof(double));
            dt.Columns.Add("GhiChu", typeof(string));

            var svList = _tSinhVien.AsEnumerable().Where(r => r.Field<string>("MaLop") == maLop).OrderBy(r => r.Field<string>("HoTen"));
            foreach (var sv in svList)
            {
                string maSV = sv.Field<string>("MaSV")!;
                string hoTen = sv.Field<string>("HoTen")!;
                var kq = _tKetQua.AsEnumerable().FirstOrDefault(r => r.Field<string>("MaSV") == maSV && r.Field<string>("MaMon") == maMon);
                double diem = kq != null ? Convert.ToDouble(kq["Diem"]) : 0.0;
                string ghiChu = kq != null ? (kq["GhiChu"]?.ToString() ?? "") : "";

                dt.Rows.Add(maSV, hoTen, diem, ghiChu);
            }

            Log("sp_KetQua_GetByLopMon (Mock)", $"Nạp danh sách {dt.Rows.Count} sinh viên lớp '{maLop}' cho môn '{maMon}'.", false);
            return dt;
        }

        public int SaveAll(string maMon, DataTable table)
        {
            Log("TRANSACTION", ">>> BẮT ĐẦU GIAO DỊCH (BEGIN TRANSACTION)...", false);

            if (IsConnectedToSqlServer)
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

                        // Business rule verification
                        if (diem < 0.0 || diem > 10.0)
                        {
                            throw new ArgumentOutOfRangeException(nameof(diem),
                                $"Điểm của sinh viên '{maSV}' không hợp lệ ({diem}). Quy định: từ 0.0 đến 10.0!");
                        }

                        using SqlCommand cmd = new SqlCommand("sp_KetQua_Save", conn, trans);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaSV", maSV);
                        cmd.Parameters.AddWithValue("@MaMon", maMon);
                        cmd.Parameters.AddWithValue("@Diem", diem);
                        cmd.Parameters.AddWithValue("@GhiChu", ghiChu);

                        cmd.ExecuteNonQuery();
                        count++;
                        Log("sp_KetQua_Save", $"Đã lưu SV '{maSV}' - Điểm: {diem:F1} (Ghi chú: {ghiChu}) vào Transaction.", false);
                    }

                    trans.Commit();
                    Log("TRANSACTION", $">>> XÁC NHẬN GIAO DỊCH THÀNH CÔNG (COMMIT TRANSACTION)! Tổng số bản ghi đã lưu: {count}.", false);
                    return count;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    Log("TRANSACTION (ROLLBACK)", $">>> ĐÃ HỦY BỎ TOÀN BỘ GIAO DỊCH (ROLLBACK TRANSACTION)! Lý do: {ex.Message}", true);
                    throw;
                }
            }

            // In-Memory Transaction Mock
            DataTable backupKetQua = _tKetQua.Copy();
            try
            {
                int count = 0;
                foreach (DataRow row in table.Rows)
                {
                    if (row.RowState == DataRowState.Deleted) continue;

                    string maSV = row["MaSV"].ToString() ?? "";
                    double diem = Convert.ToDouble(row["Diem"]);
                    string ghiChu = row["GhiChu"]?.ToString() ?? "";

                    if (diem < 0.0 || diem > 10.0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(diem),
                            $"Điểm của sinh viên '{maSV}' không hợp lệ ({diem}). Quy định: từ 0.0 đến 10.0!");
                    }

                    // Upsert in mock
                    var existing = _tKetQua.AsEnumerable().FirstOrDefault(r => r.Field<string>("MaSV") == maSV && r.Field<string>("MaMon") == maMon);
                    if (existing != null)
                    {
                        existing["Diem"] = diem;
                        existing["GhiChu"] = ghiChu;
                    }
                    else
                    {
                        _tKetQua.Rows.Add(maSV, maMon, diem, ghiChu);
                    }

                    count++;
                    Log("sp_KetQua_Save (Mock)", $"Đã lưu SV '{maSV}' - Điểm: {diem:F1} vào Transaction bộ nhớ.", false);
                }

                _tKetQua.AcceptChanges();
                Log("TRANSACTION (Mock)", $">>> XÁC NHẬN GIAO DỊCH THÀNH CÔNG (COMMIT TRANSACTION)! Tổng số: {count} bản ghi.", false);
                return count;
            }
            catch (Exception ex)
            {
                // Rollback mock state
                _tKetQua.Clear();
                foreach (DataRow r in backupKetQua.Rows)
                {
                    _tKetQua.ImportRow(r);
                }
                _tKetQua.AcceptChanges();

                Log("TRANSACTION (Mock Rollback)", $">>> ĐÃ HỦY BỎ TOÀN BỘ GIAO DỊCH (ROLLBACK)! Khôi phục lại trạng thái cũ. Lỗi: {ex.Message}", true);
                throw;
            }
        }

        private void Log(string step, string detail, bool isError)
        {
            OnTransactionLog?.Invoke(new TransactionLogEntry
            {
                Timestamp = DateTime.Now,
                Step = step,
                Detail = detail,
                IsError = isError
            });
        }
    }
}
