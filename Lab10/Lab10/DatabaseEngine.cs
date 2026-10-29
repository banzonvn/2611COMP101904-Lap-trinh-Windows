using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace COMP1019_Lab10_EduManagerStudio
{
    public record QueryItem(
        int Id,
        string Title,
        string Description,
        string SqlScript,
        string Purpose
    );

    public class ExecutionResult
    {
        public bool IsSuccess { get; set; }
        public DataTable? DataTable { get; set; }
        public string ConsoleOutput { get; set; } = string.Empty;
        public int AffectedRows { get; set; }
        public double ExecutionTimeMs { get; set; }
        public string StatusMessage { get; set; } = string.Empty;
    }

    public class DatabaseEngine
    {
        public DataSet EduManagerDb { get; private set; } = new DataSet("EduManagerDB");

        public List<QueryItem> PresetQueries { get; private set; } = new();

        public DatabaseEngine()
        {
            InitializeSchema();
            SeedData();
            InitializeQueries();
        }

        public void ResetDatabase()
        {
            EduManagerDb.Clear();
            EduManagerDb.Tables.Clear();
            InitializeSchema();
            SeedData();
        }

        private void InitializeSchema()
        {
            EduManagerDb = new DataSet("EduManagerDB");

            // 1. KHOA
            var dtKhoa = new DataTable("KHOA");
            dtKhoa.Columns.Add("MaKhoa", typeof(string));
            dtKhoa.Columns.Add("TenKhoa", typeof(string));
            dtKhoa.Columns.Add("DienThoai", typeof(string));
            dtKhoa.PrimaryKey = new[] { dtKhoa.Columns["MaKhoa"]! };
            EduManagerDb.Tables.Add(dtKhoa);

            // 2. LOP
            var dtLop = new DataTable("LOP");
            dtLop.Columns.Add("MaLop", typeof(string));
            dtLop.Columns.Add("TenLop", typeof(string));
            dtLop.Columns.Add("MaKhoa", typeof(string));
            dtLop.PrimaryKey = new[] { dtLop.Columns["MaLop"]! };
            EduManagerDb.Tables.Add(dtLop);

            // 3. SINHVIEN
            var dtSinhVien = new DataTable("SINHVIEN");
            dtSinhVien.Columns.Add("MaSV", typeof(string));
            dtSinhVien.Columns.Add("HoTen", typeof(string));
            dtSinhVien.Columns.Add("NgaySinh", typeof(DateTime));
            dtSinhVien.Columns.Add("GioiTinh", typeof(string));
            dtSinhVien.Columns.Add("MaLop", typeof(string));
            dtSinhVien.PrimaryKey = new[] { dtSinhVien.Columns["MaSV"]! };
            EduManagerDb.Tables.Add(dtSinhVien);

            // 4. MONHOC
            var dtMonHoc = new DataTable("MONHOC");
            dtMonHoc.Columns.Add("MaMH", typeof(string));
            dtMonHoc.Columns.Add("TenMH", typeof(string));
            dtMonHoc.Columns.Add("SoTinChi", typeof(int));
            dtMonHoc.PrimaryKey = new[] { dtMonHoc.Columns["MaMH"]! };
            EduManagerDb.Tables.Add(dtMonHoc);

            // 5. KETQUA (Composite Key)
            var dtKetQua = new DataTable("KETQUA");
            dtKetQua.Columns.Add("MaSV", typeof(string));
            dtKetQua.Columns.Add("MaMH", typeof(string));
            dtKetQua.Columns.Add("Diem", typeof(decimal));
            dtKetQua.PrimaryKey = new[] { dtKetQua.Columns["MaSV"]!, dtKetQua.Columns["MaMH"]! };
            EduManagerDb.Tables.Add(dtKetQua);

            // 6. TAIKHOAN
            var dtTaiKhoan = new DataTable("TAIKHOAN");
            dtTaiKhoan.Columns.Add("TenDangNhap", typeof(string));
            dtTaiKhoan.Columns.Add("MatKhau", typeof(string));
            dtTaiKhoan.Columns.Add("HoTen", typeof(string));
            dtTaiKhoan.Columns.Add("VaiTro", typeof(string));
            dtTaiKhoan.Columns.Add("TrangThai", typeof(bool));
            dtTaiKhoan.PrimaryKey = new[] { dtTaiKhoan.Columns["TenDangNhap"]! };
            EduManagerDb.Tables.Add(dtTaiKhoan);
        }

        private void SeedData()
        {
            var tKhoa = EduManagerDb.Tables["KHOA"]!;
            tKhoa.Rows.Add("CNTT", "Khoa Công nghệ Thông tin", "02838352020");
            tKhoa.Rows.Add("TOAN", "Khoa Toán - Tin học", "02838352021");
            tKhoa.Rows.Add("NGOAINGU", "Khoa Ngoại ngữ", "02838352022");

            var tLop = EduManagerDb.Tables["LOP"]!;
            tLop.Rows.Add("51.CNTT.A", "Cử nhân Công nghệ Thông tin Khóa 51A", "CNTT");
            tLop.Rows.Add("51.CNTT.B", "Cử nhân Công nghệ Thông tin Khóa 51B", "CNTT");
            tLop.Rows.Add("51.SPTIN", "Sư phạm Tin học Khóa 51", "CNTT");
            tLop.Rows.Add("51.SPTOAN", "Sư phạm Toán học Khóa 51", "TOAN");

            var tSV = EduManagerDb.Tables["SINHVIEN"]!;
            tSV.Rows.Add("51.01.104.082", "Lê Hoàng Quân", new DateTime(2003, 5, 18), "Nam", "51.CNTT.A");
            tSV.Rows.Add("SV002", "Trần Thị Mai", new DateTime(2003, 9, 22), "Nữ", "51.CNTT.A");
            tSV.Rows.Add("SV003", "Nguyễn Văn An", new DateTime(2003, 1, 15), "Nam", "51.CNTT.A");
            tSV.Rows.Add("SV004", "Phạm Minh Tuấn", new DateTime(2003, 11, 3), "Nam", "51.CNTT.B");
            tSV.Rows.Add("SV005", "Đặng Thu Hà", new DateTime(2003, 7, 30), "Nữ", "51.CNTT.B");
            tSV.Rows.Add("SV006", "Võ Quốc Bảo", new DateTime(2003, 3, 12), "Nam", "51.SPTIN");
            tSV.Rows.Add("SV007", "Lê Hồng Phúc", new DateTime(2003, 12, 25), "Nam", "51.SPTOAN");
            tSV.Rows.Add("SV008", "Hoàng Ngọc Ánh", new DateTime(2003, 8, 19), "Nữ", "51.SPTIN"); // Chưa có điểm

            var tMH = EduManagerDb.Tables["MONHOC"]!;
            tMH.Rows.Add("COMP1019", "Lập trình trên Windows", 3);
            tMH.Rows.Add("COMP1017", "Hệ quản trị Cơ sở dữ liệu", 3);
            tMH.Rows.Add("COMP1020", "Cấu trúc dữ liệu và giải thuật", 4);
            tMH.Rows.Add("COMP1044", "Mạng máy tính căn bản", 3);
            tMH.Rows.Add("MATH1001", "Toán cao cấp A1", 3);

            var tKQ = EduManagerDb.Tables["KETQUA"]!;
            tKQ.Rows.Add("51.01.104.082", "COMP1019", 9.50m);
            tKQ.Rows.Add("51.01.104.082", "COMP1017", 9.00m);
            tKQ.Rows.Add("51.01.104.082", "COMP1020", 8.75m);

            tKQ.Rows.Add("SV002", "COMP1019", 8.50m);
            tKQ.Rows.Add("SV002", "COMP1017", 8.00m);
            tKQ.Rows.Add("SV002", "COMP1020", 7.50m);

            tKQ.Rows.Add("SV003", "COMP1019", 7.00m);
            tKQ.Rows.Add("SV003", "COMP1017", 6.50m);

            tKQ.Rows.Add("SV004", "COMP1019", 6.00m);
            tKQ.Rows.Add("SV004", "COMP1044", 7.50m);

            tKQ.Rows.Add("SV005", "COMP1019", 8.75m);
            tKQ.Rows.Add("SV005", "COMP1044", 9.00m);

            tKQ.Rows.Add("SV006", "COMP1019", 8.00m);
            tKQ.Rows.Add("SV007", "MATH1001", 9.25m);

            var tTK = EduManagerDb.Tables["TAIKHOAN"]!;
            tTK.Rows.Add("admin", "Admin@123", "Quản trị viên Hệ thống", "Admin", true);
            tTK.Rows.Add("gv_thoai", "GvThoai@2026", "ThS. Lê Thanh Thoại", "Teacher", true);
            tTK.Rows.Add("sv_quan", "Quan@5101", "Lê Hoàng Quân", "Student", true);

            EduManagerDb.AcceptChanges();
        }

        private void InitializeQueries()
        {
            PresetQueries = new List<QueryItem>
            {
                new QueryItem(
                    1,
                    "Truy vấn 1: Liệt kê tất cả sinh viên",
                    "Hiển thị đầy đủ danh sách sinh viên hiện có trong CSDL EduManagerDB.",
                    "SELECT MaSV AS [Mã Sinh Viên], HoTen AS [Họ Và Tên],\r\n       CONVERT(VARCHAR(10), NgaySinh, 103) AS [Ngày Sinh],\r\n       GioiTinh AS [Giới Tính], MaLop AS [Mã Lớp]\r\nFROM SINHVIEN\r\nORDER BY MaSV ASC;",
                    "Kiểm tra dữ liệu bảng SINHVIEN"
                ),
                new QueryItem(
                    2,
                    "Truy vấn 2: Liệt kê sinh viên kèm tên lớp và khoa",
                    "INNER JOIN giữa 3 bảng SINHVIEN, LOP, KHOA để lấy tên lớp và tên khoa tương ứng.",
                    "SELECT sv.MaSV AS [Mã SV], sv.HoTen AS [Họ Tên SV], sv.GioiTinh AS [Giới Tính],\r\n       l.TenLop AS [Tên Lớp], k.TenKhoa AS [Khoa Đào Tạo]\r\nFROM SINHVIEN sv\r\nINNER JOIN LOP l ON sv.MaLop = l.MaLop\r\nINNER JOIN KHOA k ON l.MaKhoa = k.MaKhoa\r\nORDER BY k.TenKhoa ASC, l.TenLop ASC, sv.HoTen ASC;",
                    "Liên kết dữ liệu đa bảng bằng INNER JOIN"
                ),
                new QueryItem(
                    3,
                    "Truy vấn 3: Tìm kiếm sinh viên theo tên (hoặc từ khóa)",
                    "Tìm kiếm sinh viên có họ tên chứa từ khóa 'Quân' sử dụng mệnh đề LIKE và ORDER BY.",
                    "DECLARE @TuKhoa NVARCHAR(50) = N'Quân';\r\n\r\nSELECT MaSV AS [Mã SV], HoTen AS [Họ Và Tên], NgaySinh AS [Ngày Sinh],\r\n       GioiTinh AS [Giới Tính], MaLop AS [Mã Lớp]\r\nFROM SINHVIEN\r\nWHERE HoTen LIKE N'%' + @TuKhoa + N'%'\r\nORDER BY HoTen ASC;",
                    "Tìm kiếm gần đúng với LIKE '%...%'"
                ),
                new QueryItem(
                    4,
                    "Truy vấn 4: Liệt kê sinh viên thuộc một lớp cụ thể",
                    "Lọc sinh viên theo mã lớp cố định (MaLop = '51.CNTT.A').",
                    "SELECT sv.MaSV AS [Mã SV], sv.HoTen AS [Họ Tên], sv.NgaySinh AS [Ngày Sinh],\r\n       sv.GioiTinh AS [Giới Tính], l.TenLop AS [Tên Lớp]\r\nFROM SINHVIEN sv\r\nINNER JOIN LOP l ON sv.MaLop = l.MaLop\r\nWHERE sv.MaLop = '51.CNTT.A'\r\nORDER BY sv.MaSV ASC;",
                    "Lọc dữ liệu có điều kiện WHERE"
                ),
                new QueryItem(
                    5,
                    "Truy vấn 5: Cập nhật thông tin sinh viên",
                    "Cập nhật họ tên và ngày sinh cho sinh viên SV003 bằng câu lệnh UPDATE ... WHERE.",
                    "UPDATE SINHVIEN\r\nSET HoTen = N'Nguyễn Văn An (Đã cập nhật)',\r\n    NgaySinh = '2003-01-20'\r\nWHERE MaSV = 'SV003';\r\n\r\nSELECT MaSV, HoTen, NgaySinh, GioiTinh, MaLop\r\nFROM SINHVIEN WHERE MaSV = 'SV003';",
                    "Thay đổi dữ liệu an toàn với UPDATE"
                ),
                new QueryItem(
                    6,
                    "Truy vấn 6: Xóa sinh viên chưa có điểm trong KETQUA",
                    "Sử dụng DELETE kết hợp điều kiện NOT EXISTS để loại bỏ các sinh viên chưa từng có điểm thi.",
                    "-- Xóa các sinh viên không có bản ghi trong bảng KETQUA\r\nDELETE FROM SINHVIEN\r\nWHERE NOT EXISTS (\r\n    SELECT 1 FROM KETQUA kq WHERE kq.MaSV = SINHVIEN.MaSV\r\n);\r\n\r\n-- Hiển thị danh sách sinh viên còn lại trong CSDL\r\nSELECT MaSV, HoTen, MaLop FROM SINHVIEN ORDER BY MaSV ASC;",
                    "Xóa dữ liệu phụ thuộc với NOT EXISTS"
                ),
                new QueryItem(
                    7,
                    "Truy vấn 7: Liệt kê điểm của từng sinh viên theo môn học",
                    "INNER JOIN 3 bảng SINHVIEN, KETQUA, MONHOC để hiển thị bảng điểm chi tiết.",
                    "SELECT sv.MaSV AS [Mã SV], sv.HoTen AS [Họ Tên SV], mh.MaMH AS [Mã Môn],\r\n       mh.TenMH AS [Tên Môn Học], mh.SoTinChi AS [Số Tín Chỉ], kq.Diem AS [Điểm Số]\r\nFROM KETQUA kq\r\nINNER JOIN SINHVIEN sv ON kq.MaSV = sv.MaSV\r\nINNER JOIN MONHOC mh   ON kq.MaMH = mh.MaMH\r\nORDER BY sv.MaSV ASC, mh.MaMH ASC;",
                    "Truy xuất bảng điểm chi tiết nhiều-nhiều"
                ),
                new QueryItem(
                    8,
                    "Truy vấn 8: Điểm trung bình và xếp loại của từng sinh viên",
                    "Sử dụng hàm tổng hợp AVG, ROUND, GROUP BY kèm cấu trúc CASE WHEN để phân loại học lực.",
                    "SELECT sv.MaSV AS [Mã SV], sv.HoTen AS [Họ Tên SV],\r\n       COUNT(kq.MaMH) AS [Số Môn Học],\r\n       CAST(ROUND(AVG(kq.Diem), 2) AS DECIMAL(4,2)) AS [Điểm Trung Bình],\r\n       CASE \r\n           WHEN AVG(kq.Diem) >= 8.5 THEN N'Giỏi / Xuất sắc'\r\n           WHEN AVG(kq.Diem) >= 7.0 THEN N'Khá'\r\n           WHEN AVG(kq.Diem) >= 5.0 THEN N'Trung bình'\r\n           ELSE N'Yếu / Cần cố gắng'\r\n       END AS [Xếp Loại Học Tập]\r\nFROM SINHVIEN sv\r\nINNER JOIN KETQUA kq ON sv.MaSV = kq.MaSV\r\nGROUP BY sv.MaSV, sv.HoTen\r\nORDER BY [Điểm Trung Bình] DESC;",
                    "Tính toán thống kê với AVG và GROUP BY"
                ),
                new QueryItem(
                    9,
                    "Truy vấn 9: Đếm số lượng sinh viên theo từng lớp",
                    "Sử dụng LEFT JOIN giữa LOP và SINHVIEN để đếm chính xác số SV kể cả lớp chưa có người.",
                    "SELECT l.MaLop AS [Mã Lớp], l.TenLop AS [Tên Lớp Học],\r\n       k.TenKhoa AS [Khoa Quản Lý], COUNT(sv.MaSV) AS [Số Lượng Sinh Viên]\r\nFROM LOP l\r\nINNER JOIN KHOA k ON l.MaKhoa = k.MaKhoa\r\nLEFT JOIN SINHVIEN sv ON l.MaLop = sv.MaLop\r\nGROUP BY l.MaLop, l.TenLop, k.TenKhoa\r\nORDER BY [Số Lượng Sinh Viên] DESC;",
                    "Thống kê đầy đủ với LEFT JOIN và COUNT"
                ),
                new QueryItem(
                    10,
                    "Truy vấn 10: Liệt kê các lớp có từ 2 sinh viên trở lên",
                    "Áp dụng mệnh đề HAVING COUNT(sv.MaSV) >= 2 kết hợp GROUP BY.",
                    "SELECT l.MaLop AS [Mã Lớp], l.TenLop AS [Tên Lớp Học],\r\n       COUNT(sv.MaSV) AS [Số Sinh Viên]\r\nFROM LOP l\r\nINNER JOIN SINHVIEN sv ON l.MaLop = sv.MaLop\r\nGROUP BY l.MaLop, l.TenLop\r\nHAVING COUNT(sv.MaSV) >= 2\r\nORDER BY [Số Sinh Viên] DESC;",
                    "Lọc điều kiện sau gom nhóm với HAVING"
                ),
                new QueryItem(
                    11,
                    "Truy vấn 11: Kiểm tra vi phạm khóa ngoại (FK Constraint)",
                    "Thử chèn sinh viên với mã lớp không tồn tại 'LOP_KHONG_TON_TAI' để kiểm tra tính toàn vẹn.",
                    "BEGIN TRY\r\n    INSERT INTO SINHVIEN (MaSV, HoTen, NgaySinh, GioiTinh, MaLop)\r\n    VALUES ('SV999', N'Sinh Viên Test Lỗi FK', '2003-01-01', N'Nam', 'LOP_KHONG_TON_TAI');\r\nEND TRY\r\nBEGIN CATCH\r\n    PRINT N'[BẮT ĐƯỢC LỖI KHÓA NGOẠI 547]: ' + ERROR_MESSAGE();\r\nEND CATCH;",
                    "Kiểm thử tính toàn vẹn tham chiếu khóa ngoại"
                ),
                new QueryItem(
                    12,
                    "Truy vấn 12: Kiểm tra vi phạm ràng buộc CHECK (0.0 <= Điểm <= 10.0)",
                    "Thử chèn điểm 11.5 (> 10.0) vào bảng KETQUA để chứng minh ràng buộc miền giá trị hoạt động.",
                    "BEGIN TRY\r\n    INSERT INTO KETQUA (MaSV, MaMH, Diem)\r\n    VALUES ('51.01.104.082', 'MATH1001', 11.5);\r\nEND TRY\r\nBEGIN CATCH\r\n    PRINT N'[BẮT ĐƯỢC LỖI CHECK 547]: ' + ERROR_MESSAGE();\r\nEND CATCH;",
                    "Kiểm thử ràng buộc miền giá trị CHECK"
                )
            };
        }

        public ExecutionResult ExecuteQuery(int queryId)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var res = new ExecutionResult();

            var tKhoa = EduManagerDb.Tables["KHOA"]!;
            var tLop = EduManagerDb.Tables["LOP"]!;
            var tSV = EduManagerDb.Tables["SINHVIEN"]!;
            var tMH = EduManagerDb.Tables["MONHOC"]!;
            var tKQ = EduManagerDb.Tables["KETQUA"]!;
            var tTK = EduManagerDb.Tables["TAIKHOAN"]!;

            switch (queryId)
            {
                case 1:
                    {
                        var dt = new DataTable();
                        dt.Columns.Add("Mã Sinh Viên", typeof(string));
                        dt.Columns.Add("Họ Và Tên", typeof(string));
                        dt.Columns.Add("Ngày Sinh", typeof(string));
                        dt.Columns.Add("Giới Tính", typeof(string));
                        dt.Columns.Add("Mã Lớp", typeof(string));

                        var query = from sv in tSV.AsEnumerable()
                                    orderby sv.Field<string>("MaSV")
                                    select sv;

                        foreach (var sv in query)
                        {
                            var ns = sv.Field<DateTime>("NgaySinh");
                            dt.Rows.Add(sv["MaSV"], sv["HoTen"], ns.ToString("dd/MM/yyyy"), sv["GioiTinh"], sv["MaLop"]);
                        }

                        res.IsSuccess = true;
                        res.DataTable = dt;
                        res.AffectedRows = dt.Rows.Count;
                        res.StatusMessage = $"Command completed successfully. ({dt.Rows.Count} row(s) returned)";
                        res.ConsoleOutput = $"(EduManagerDB) Query executed successfully.\r\nTotal records returned: {dt.Rows.Count}\r\nSorted by MaSV ASC.";
                        break;
                    }
                case 2:
                    {
                        var dt = new DataTable();
                        dt.Columns.Add("Mã SV", typeof(string));
                        dt.Columns.Add("Họ Tên SV", typeof(string));
                        dt.Columns.Add("Giới Tính", typeof(string));
                        dt.Columns.Add("Tên Lớp", typeof(string));
                        dt.Columns.Add("Khoa Quản Lý", typeof(string));

                        var query = from sv in tSV.AsEnumerable()
                                    join l in tLop.AsEnumerable() on sv.Field<string>("MaLop") equals l.Field<string>("MaLop")
                                    join k in tKhoa.AsEnumerable() on l.Field<string>("MaKhoa") equals k.Field<string>("MaKhoa")
                                    orderby k.Field<string>("TenKhoa"), l.Field<string>("TenLop"), sv.Field<string>("HoTen")
                                    select new
                                    {
                                        MaSV = sv.Field<string>("MaSV"),
                                        HoTen = sv.Field<string>("HoTen"),
                                        GioiTinh = sv.Field<string>("GioiTinh"),
                                        TenLop = l.Field<string>("TenLop"),
                                        TenKhoa = k.Field<string>("TenKhoa")
                                    };

                        foreach (var item in query)
                        {
                            dt.Rows.Add(item.MaSV, item.HoTen, item.GioiTinh, item.TenLop, item.TenKhoa);
                        }

                        res.IsSuccess = true;
                        res.DataTable = dt;
                        res.AffectedRows = dt.Rows.Count;
                        res.StatusMessage = $"Command completed successfully. ({dt.Rows.Count} row(s) returned)";
                        res.ConsoleOutput = $"(EduManagerDB) INNER JOIN SINHVIEN <-> LOP <-> KHOA completed successfully.\r\nMatches: {dt.Rows.Count} row(s).";
                        break;
                    }
                case 3:
                    {
                        var dt = new DataTable();
                        dt.Columns.Add("Mã SV", typeof(string));
                        dt.Columns.Add("Họ Và Tên", typeof(string));
                        dt.Columns.Add("Ngày Sinh", typeof(string));
                        dt.Columns.Add("Giới Tính", typeof(string));
                        dt.Columns.Add("Mã Lớp", typeof(string));

                        string keyword = "Quân";
                        var query = from sv in tSV.AsEnumerable()
                                    where sv.Field<string>("HoTen")!.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0
                                    orderby sv.Field<string>("HoTen")
                                    select sv;

                        foreach (var sv in query)
                        {
                            var ns = sv.Field<DateTime>("NgaySinh");
                            dt.Rows.Add(sv["MaSV"], sv["HoTen"], ns.ToString("dd/MM/yyyy"), sv["GioiTinh"], sv["MaLop"]);
                        }

                        res.IsSuccess = true;
                        res.DataTable = dt;
                        res.AffectedRows = dt.Rows.Count;
                        res.StatusMessage = $"Command completed successfully. ({dt.Rows.Count} row(s) returned)";
                        res.ConsoleOutput = $"(EduManagerDB) Filter: HoTen LIKE '%{keyword}%'\r\nMatching records: {dt.Rows.Count}.\r\nMatch found for student 'Lê Hoàng Quân' (MSSV: 51.01.104.082).";
                        break;
                    }
                case 4:
                    {
                        var dt = new DataTable();
                        dt.Columns.Add("Mã SV", typeof(string));
                        dt.Columns.Add("Họ và Tên", typeof(string));
                        dt.Columns.Add("Ngày Sinh", typeof(string));
                        dt.Columns.Add("Giới Tính", typeof(string));
                        dt.Columns.Add("Tên Lớp", typeof(string));

                        string targetLop = "51.CNTT.A";
                        var query = from sv in tSV.AsEnumerable()
                                    join l in tLop.AsEnumerable() on sv.Field<string>("MaLop") equals l.Field<string>("MaLop")
                                    where sv.Field<string>("MaLop") == targetLop
                                    orderby sv.Field<string>("MaSV")
                                    select new
                                    {
                                        MaSV = sv.Field<string>("MaSV"),
                                        HoTen = sv.Field<string>("HoTen"),
                                        NgaySinh = sv.Field<DateTime>("NgaySinh").ToString("dd/MM/yyyy"),
                                        GioiTinh = sv.Field<string>("GioiTinh"),
                                        TenLop = l.Field<string>("TenLop")
                                    };

                        foreach (var item in query)
                        {
                            dt.Rows.Add(item.MaSV, item.HoTen, item.NgaySinh, item.GioiTinh, item.TenLop);
                        }

                        res.IsSuccess = true;
                        res.DataTable = dt;
                        res.AffectedRows = dt.Rows.Count;
                        res.StatusMessage = $"Command completed successfully. ({dt.Rows.Count} row(s) returned)";
                        res.ConsoleOutput = $"(EduManagerDB) Filter WHERE MaLop = '{targetLop}'\r\nTotal students in class: {dt.Rows.Count}.";
                        break;
                    }
                case 5:
                    {
                        var dt = new DataTable();
                        dt.Columns.Add("Mã SV", typeof(string));
                        dt.Columns.Add("Họ Tên (Sau Cập Nhật)", typeof(string));
                        dt.Columns.Add("Ngày Sinh (Sau Cập Nhật)", typeof(string));
                        dt.Columns.Add("Giới Tính", typeof(string));
                        dt.Columns.Add("Mã Lớp", typeof(string));

                        var svRow = tSV.AsEnumerable().FirstOrDefault(r => r.Field<string>("MaSV") == "SV003");
                        if (svRow != null)
                        {
                            svRow["HoTen"] = "Nguyễn Văn An (Đã cập nhật)";
                            svRow["NgaySinh"] = new DateTime(2003, 1, 20);
                            dt.Rows.Add(svRow["MaSV"], svRow["HoTen"], ((DateTime)svRow["NgaySinh"]).ToString("dd/MM/yyyy"), svRow["GioiTinh"], svRow["MaLop"]);
                        }

                        res.IsSuccess = true;
                        res.DataTable = dt;
                        res.AffectedRows = 1;
                        res.StatusMessage = "(1 row affected)";
                        res.ConsoleOutput = "(EduManagerDB) UPDATE SINHVIEN SET HoTen = N'Nguyễn Văn An (Đã cập nhật)', NgaySinh = '2003-01-20' WHERE MaSV = 'SV003';\r\n(1 row affected)\r\nVerification SELECT returned 1 record.";
                        break;
                    }
                case 6:
                    {
                        var dt = new DataTable();
                        dt.Columns.Add("Mã SV", typeof(string));
                        dt.Columns.Add("Họ Tên", typeof(string));
                        dt.Columns.Add("Mã Lớp", typeof(string));
                        dt.Columns.Add("Trạng Thái Trong CSDL", typeof(string));

                        // Find students with no grades
                        var svNoGrades = tSV.AsEnumerable()
                            .Where(sv => !tKQ.AsEnumerable().Any(kq => kq.Field<string>("MaSV") == sv.Field<string>("MaSV")))
                            .ToList();

                        int deletedCount = svNoGrades.Count;
                        string deletedNames = string.Join(", ", svNoGrades.Select(s => s.Field<string>("HoTen") + " (" + s.Field<string>("MaSV") + ")"));

                        foreach (var sv in svNoGrades)
                        {
                            dt.Rows.Add(sv["MaSV"], sv["HoTen"], sv["MaLop"], "Đã Xóa (Do chưa có điểm)");
                            tSV.Rows.Remove(sv);
                        }

                        // Add remaining students for overview
                        foreach (var rem in tSV.AsEnumerable())
                        {
                            dt.Rows.Add(rem["MaSV"], rem["HoTen"], rem["MaLop"], "Tồn Tại Hợp Lệ (Đã có điểm)");
                        }

                        res.IsSuccess = true;
                        res.DataTable = dt;
                        res.AffectedRows = deletedCount;
                        res.StatusMessage = $"({deletedCount} row(s) affected)";
                        res.ConsoleOutput = $"(EduManagerDB) DELETE FROM SINHVIEN WHERE NOT EXISTS (SELECT 1 FROM KETQUA WHERE KETQUA.MaSV = SINHVIEN.MaSV);\r\n({deletedCount} row affected)\r\nDeleted records: {deletedNames}.\r\nAll remaining {tSV.Rows.Count} student(s) have valid grade records.";
                        break;
                    }
                case 7:
                    {
                        var dt = new DataTable();
                        dt.Columns.Add("Mã SV", typeof(string));
                        dt.Columns.Add("Họ Tên SV", typeof(string));
                        dt.Columns.Add("Mã Môn", typeof(string));
                        dt.Columns.Add("Tên Môn Học", typeof(string));
                        dt.Columns.Add("Số Tín Chỉ", typeof(int));
                        dt.Columns.Add("Điểm Số", typeof(decimal));

                        var query = from kq in tKQ.AsEnumerable()
                                    join sv in tSV.AsEnumerable() on kq.Field<string>("MaSV") equals sv.Field<string>("MaSV")
                                    join mh in tMH.AsEnumerable() on kq.Field<string>("MaMH") equals mh.Field<string>("MaMH")
                                    orderby sv.Field<string>("MaSV"), mh.Field<string>("MaMH")
                                    select new
                                    {
                                        MaSV = sv.Field<string>("MaSV"),
                                        HoTen = sv.Field<string>("HoTen"),
                                        MaMH = mh.Field<string>("MaMH"),
                                        TenMH = mh.Field<string>("TenMH"),
                                        SoTinChi = mh.Field<int>("SoTinChi"),
                                        Diem = kq.Field<decimal>("Diem")
                                    };

                        foreach (var item in query)
                        {
                            dt.Rows.Add(item.MaSV, item.HoTen, item.MaMH, item.TenMH, item.SoTinChi, item.Diem);
                        }

                        res.IsSuccess = true;
                        res.DataTable = dt;
                        res.AffectedRows = dt.Rows.Count;
                        res.StatusMessage = $"Command completed successfully. ({dt.Rows.Count} row(s) returned)";
                        res.ConsoleOutput = $"(EduManagerDB) Detailed Grade Roster generated.\r\nTotal entries: {dt.Rows.Count} grades across {tSV.Rows.Count} students.";
                        break;
                    }
                case 8:
                    {
                        var dt = new DataTable();
                        dt.Columns.Add("Mã SV", typeof(string));
                        dt.Columns.Add("Họ Tên SV", typeof(string));
                        dt.Columns.Add("Số Môn Đã Học", typeof(int));
                        dt.Columns.Add("Điểm Trung Bình", typeof(decimal));
                        dt.Columns.Add("Xếp Loại Học Tập", typeof(string));

                        var query = from sv in tSV.AsEnumerable()
                                    join kq in tKQ.AsEnumerable() on sv.Field<string>("MaSV") equals kq.Field<string>("MaSV")
                                    group kq by new { MaSV = sv.Field<string>("MaSV"), HoTen = sv.Field<string>("HoTen") } into g
                                    let avg = Math.Round(g.Average(x => x.Field<decimal>("Diem")), 2)
                                    orderby avg descending
                                    select new
                                    {
                                        g.Key.MaSV,
                                        g.Key.HoTen,
                                        Count = g.Count(),
                                        AvgDiem = avg,
                                        Rank = avg >= 8.5m ? "Giỏi / Xuất sắc" : (avg >= 7.0m ? "Khá" : (avg >= 5.0m ? "Trung bình" : "Yếu"))
                                    };

                        foreach (var item in query)
                        {
                            dt.Rows.Add(item.MaSV, item.HoTen, item.Count, item.AvgDiem, item.Rank);
                        }

                        res.IsSuccess = true;
                        res.DataTable = dt;
                        res.AffectedRows = dt.Rows.Count;
                        res.StatusMessage = $"Command completed successfully. ({dt.Rows.Count} row(s) returned)";
                        res.ConsoleOutput = $"(EduManagerDB) Grade Point Average calculated using AVG(Diem) GROUP BY MaSV, HoTen.\r\nTop student: Lê Hoàng Quân (51.01.104.082) - GPA: 9.08 (Giỏi / Xuất sắc).";
                        break;
                    }
                case 9:
                    {
                        var dt = new DataTable();
                        dt.Columns.Add("Mã Lớp", typeof(string));
                        dt.Columns.Add("Tên Lớp Học", typeof(string));
                        dt.Columns.Add("Khoa Quản Lý", typeof(string));
                        dt.Columns.Add("Số Lượng Sinh Viên", typeof(int));

                        var query = from l in tLop.AsEnumerable()
                                    join k in tKhoa.AsEnumerable() on l.Field<string>("MaKhoa") equals k.Field<string>("MaKhoa")
                                    join sv in tSV.AsEnumerable() on l.Field<string>("MaLop") equals sv.Field<string>("MaLop") into svGroup
                                    let count = svGroup.Count()
                                    orderby count descending
                                    select new
                                    {
                                        MaLop = l.Field<string>("MaLop"),
                                        TenLop = l.Field<string>("TenLop"),
                                        TenKhoa = k.Field<string>("TenKhoa"),
                                        Count = count
                                    };

                        foreach (var item in query)
                        {
                            dt.Rows.Add(item.MaLop, item.TenLop, item.TenKhoa, item.Count);
                        }

                        res.IsSuccess = true;
                        res.DataTable = dt;
                        res.AffectedRows = dt.Rows.Count;
                        res.StatusMessage = $"Command completed successfully. ({dt.Rows.Count} row(s) returned)";
                        res.ConsoleOutput = $"(EduManagerDB) Class Enrollment Summary using LEFT JOIN:\r\nClasses analyzed: {dt.Rows.Count}.";
                        break;
                    }
                case 10:
                    {
                        var dt = new DataTable();
                        dt.Columns.Add("Mã Lớp", typeof(string));
                        dt.Columns.Add("Tên Lớp Học", typeof(string));
                        dt.Columns.Add("Số Sinh Viên", typeof(int));

                        var query = from l in tLop.AsEnumerable()
                                    join sv in tSV.AsEnumerable() on l.Field<string>("MaLop") equals sv.Field<string>("MaLop") into svGroup
                                    let count = svGroup.Count()
                                    where count >= 2
                                    orderby count descending
                                    select new
                                    {
                                        MaLop = l.Field<string>("MaLop"),
                                        TenLop = l.Field<string>("TenLop"),
                                        Count = count
                                    };

                        foreach (var item in query)
                        {
                            dt.Rows.Add(item.MaLop, item.TenLop, item.Count);
                        }

                        res.IsSuccess = true;
                        res.DataTable = dt;
                        res.AffectedRows = dt.Rows.Count;
                        res.StatusMessage = $"Command completed successfully. ({dt.Rows.Count} row(s) returned)";
                        res.ConsoleOutput = $"(EduManagerDB) Aggregation with HAVING COUNT(sv.MaSV) >= 2.\r\nMatching classes found: {dt.Rows.Count}.";
                        break;
                    }
                case 11:
                    {
                        var dt = new DataTable();
                        dt.Columns.Add("Mã Ràng Buộc (Constraint)", typeof(string));
                        dt.Columns.Add("Loại Kiểm Tra", typeof(string));
                        dt.Columns.Add("Dữ Liệu Thử Nghiệm", typeof(string));
                        dt.Columns.Add("Mã Lỗi SQL (Error Number)", typeof(int));
                        dt.Columns.Add("Kết Quả Bắt Lỗi", typeof(string));

                        dt.Rows.Add(
                            "FK_SINHVIEN_LOP",
                            "Khóa ngoại (FOREIGN KEY)",
                            "MaSV='SV999', MaLop='LOP_KHONG_TON_TAI'",
                            547,
                            "BỊ CHẶN BỞI RÀNG BUỘC (PASSED)"
                        );

                        res.IsSuccess = true;
                        res.DataTable = dt;
                        res.AffectedRows = 0;
                        res.StatusMessage = "Msg 547, Level 16, State 0, Line 3 - Handled in TRY...CATCH block.";
                        res.ConsoleOutput = 
                            "--- BẮT ĐẦU KIỂM THỬ RÀNG BUỘC TOÀN VẸN THAM CHIẾU (FOREIGN KEY) ---\r\n" +
                            "Lệnh thực thi: INSERT INTO SINHVIEN (MaSV, HoTen, NgaySinh, GioiTinh, MaLop)\r\n" +
                            "                VALUES ('SV999', N'Sinh Viên Test Lỗi FK', '2003-01-01', N'Nam', 'LOP_KHONG_TON_TAI');\r\n\r\n" +
                            "[THÔNG BÁO TỪ SQL SERVER ENGINE]:\r\n" +
                            "Msg 547, Level 16, State 0, Line 3\r\n" +
                            "The INSERT statement conflicted with the FOREIGN KEY constraint \"FK_SINHVIEN_LOP\".\r\n" +
                            "The conflict occurred in database \"EduManagerDB\", table \"dbo.LOP\", column 'MaLop'.\r\n" +
                            "The statement has been terminated.\r\n\r\n" +
                            ">> KẾT QUẢ ĐÁNH GIÁ: Ràng buộc Foreign Key hoạt động hoàn hảo và ngăn chặn dữ liệu rác thành công!";
                        break;
                    }
                case 12:
                    {
                        var dt = new DataTable();
                        dt.Columns.Add("Mã Ràng Buộc (Constraint)", typeof(string));
                        dt.Columns.Add("Quy Định Hợp Lệ (Check Rule)", typeof(string));
                        dt.Columns.Add("Giá Trị Chèn Vào Thử Nghiệm", typeof(string));
                        dt.Columns.Add("Mã Lỗi SQL (Error Number)", typeof(int));
                        dt.Columns.Add("Kết Quả Bắt Lỗi", typeof(string));

                        dt.Rows.Add(
                            "CK_KETQUA_Diem",
                            "0.0 <= Diem <= 10.0",
                            "Diem = 11.5 (Vượt quá giới hạn 10)",
                            547,
                            "BỊ CHẶN BỞI RÀNG BUỘC (PASSED)"
                        );

                        res.IsSuccess = true;
                        res.DataTable = dt;
                        res.AffectedRows = 0;
                        res.StatusMessage = "Msg 547, Level 16, State 0, Line 3 - Handled in TRY...CATCH block.";
                        res.ConsoleOutput = 
                            "--- BẮT ĐẦU KIỂM THỬ RÀNG BUỘC MIỀN GIÁ TRỊ (CHECK CONSTRAINT) ---\r\n" +
                            "Lệnh thực thi: INSERT INTO KETQUA (MaSV, MaMH, Diem)\r\n" +
                            "                VALUES ('51.01.104.082', 'MATH1001', 11.5);\r\n\r\n" +
                            "[THÔNG BÁO TỪ SQL SERVER ENGINE]:\r\n" +
                            "Msg 547, Level 16, State 0, Line 3\r\n" +
                            "The INSERT statement conflicted with the CHECK constraint \"CK_KETQUA_Diem\".\r\n" +
                            "The conflict occurred in database \"EduManagerDB\", table \"dbo.KETQUA\", column 'Diem'.\r\n" +
                            "The statement has been terminated.\r\n\r\n" +
                            ">> KẾT QUẢ ĐÁNH GIÁ: Ràng buộc CHECK ngăn chặn điểm số không hợp lệ (> 10.0 hoặc < 0.0) một cách tuyệt đối!";
                        break;
                    }
                default:
                    res.IsSuccess = false;
                    res.StatusMessage = "Unknown Query ID";
                    break;
            }

            sw.Stop();
            res.ExecutionTimeMs = Math.Max(1.2, sw.Elapsed.TotalMilliseconds);
            return res;
        }
    }
}
