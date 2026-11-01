-- ============================================================================
-- TRƯỜNG ĐẠI HỌC SƯ PHẠM TP. HỒ CHÍ MINH - KHOA CÔNG NGHỆ THÔNG TIN
-- COMP1019 - LẬP TRÌNH TRÊN WINDOWS
-- BÀI LAB 12: DATA BINDING, STORED PROCEDURE VÀ TRANSACTION
-- Sinh viên: Lê Hoàng Quân | MSSV: 51.01.104.082 | Lớp: 51.CNTT.A | Nhóm: 4
-- File Script CSDL: QLSinhVienLab12.sql
-- ============================================================================

USE master;
GO

-- 1. Tạo database QLSinhVienLab12 nếu chưa tồn tại
IF DB_ID('QLSinhVienLab12') IS NULL
BEGIN
    CREATE DATABASE QLSinhVienLab12;
    PRINT N'>> Đã tạo mới CSDL QLSinhVienLab12 thành công.';
END
ELSE
BEGIN
    PRINT N'>> CSDL QLSinhVienLab12 đã tồn tại sẵn sàng sử dụng.';
END
GO

USE QLSinhVienLab12;
GO

-- 2. Xóa các bảng cũ theo đúng thứ tự phụ thuộc khóa ngoại nếu đã tồn tại
IF OBJECT_ID('KETQUA', 'U') IS NOT NULL DROP TABLE KETQUA;
IF OBJECT_ID('SINHVIEN', 'U') IS NOT NULL DROP TABLE SINHVIEN;
IF OBJECT_ID('MONHOC', 'U') IS NOT NULL DROP TABLE MONHOC;
IF OBJECT_ID('LOP', 'U') IS NOT NULL DROP TABLE LOP;
GO

-- ============================================================================
-- 3. Tạo các bảng dữ liệu
-- ============================================================================

-- Bảng 1: LOP (Quản lý các lớp học)
CREATE TABLE LOP (
    MaLop   NVARCHAR(20)    NOT NULL,
    TenLop  NVARCHAR(100)   NOT NULL,
    CONSTRAINT PK_LOP PRIMARY KEY (MaLop)
);
GO

-- Bảng 2: SINHVIEN (Quản lý hồ sơ sinh viên)
CREATE TABLE SINHVIEN (
    MaSV        NVARCHAR(20)    NOT NULL,
    HoTen       NVARCHAR(100)   NOT NULL,
    NgaySinh    DATE            NULL,
    MaLop       NVARCHAR(20)    NOT NULL,
    CONSTRAINT PK_SINHVIEN PRIMARY KEY (MaSV),
    CONSTRAINT FK_SINHVIEN_LOP FOREIGN KEY (MaLop) 
        REFERENCES LOP(MaLop)
        ON UPDATE CASCADE
        ON DELETE NO ACTION
);
GO

-- Bảng 3: MONHOC (Quản lý danh mục môn học đào tạo)
CREATE TABLE MONHOC (
    MaMon       NVARCHAR(20)    NOT NULL,
    TenMon      NVARCHAR(100)   NOT NULL,
    SoTinChi    INT             NOT NULL,
    CONSTRAINT PK_MONHOC PRIMARY KEY (MaMon),
    CONSTRAINT CK_MONHOC_SoTinChi CHECK (SoTinChi > 0)
);
GO

-- Bảng 4: KETQUA (Quản lý kết quả học tập - Khóa chính ghép MaSV, MaMon)
CREATE TABLE KETQUA (
    MaSV        NVARCHAR(20)    NOT NULL,
    MaMon       NVARCHAR(20)    NOT NULL,
    Diem        FLOAT           NOT NULL,
    GhiChu      NVARCHAR(255)   NULL,
    CONSTRAINT PK_KETQUA PRIMARY KEY (MaSV, MaMon),
    CONSTRAINT CK_KETQUA_Diem CHECK (Diem >= 0.0 AND Diem <= 10.0),
    CONSTRAINT FK_KETQUA_SINHVIEN FOREIGN KEY (MaSV) 
        REFERENCES SINHVIEN(MaSV)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    CONSTRAINT FK_KETQUA_MONHOC FOREIGN KEY (MaMon) 
        REFERENCES MONHOC(MaMon)
        ON UPDATE CASCADE
        ON DELETE NO ACTION
);
GO

-- ============================================================================
-- 4. Chèn dữ liệu mẫu thực tế
-- ============================================================================

-- Dữ liệu bảng LOP
INSERT INTO LOP (MaLop, TenLop) VALUES
(N'51.CNTT.A', N'Cử nhân Công nghệ Thông tin Khóa 51A'),
(N'CTK47A',    N'Công nghệ thông tin K47A'),
(N'CTK47B',    N'Công nghệ thông tin K47B');
GO

-- Dữ liệu bảng SINHVIEN (Có sinh viên Lê Hoàng Quân MSSV: 51.01.104.082)
INSERT INTO SINHVIEN (MaSV, HoTen, NgaySinh, MaLop) VALUES
(N'51.01.104.082', N'Lê Hoàng Quân',    '2003-05-18', N'51.CNTT.A'),
(N'SV005',         N'Võ Quốc Bảo',      '2003-11-22', N'51.CNTT.A'),
(N'SV006',         N'Đặng Thu Hà',      '2003-08-14', N'51.CNTT.A'),
(N'SV001',         N'Nguyễn Văn An',    '2005-01-15', N'CTK47A'),
(N'SV002',         N'Trần Thị Bình',    '2005-05-20', N'CTK47A'),
(N'SV003',         N'Lê Minh Cường',    '2005-03-11', N'CTK47A'),
(N'SV004',         N'Phạm Thanh Dung',  '2005-07-09', N'CTK47B');
GO

-- Dữ liệu bảng MONHOC
INSERT INTO MONHOC (MaMon, TenMon, SoTinChi) VALUES
(N'COMP1019', N'Lập trình trên Windows', 3),
(N'COMP1017', N'Cơ sở lập trình', 3),
(N'COMP1020', N'Cấu trúc dữ liệu và giải thuật', 4);
GO

-- Dữ liệu bảng KETQUA mẫu
INSERT INTO KETQUA (MaSV, MaMon, Diem, GhiChu) VALUES
(N'51.01.104.082', N'COMP1019', 9.5, N'Xuất sắc - Hoàn thành bài sớm'),
(N'51.01.104.082', N'COMP1017', 9.0, N'Giỏi'),
(N'SV001',         N'COMP1019', 8.5, N'Khá giỏi'),
(N'SV002',         N'COMP1019', 7.0, N'Đạt yêu cầu'),
(N'SV004',         N'COMP1017', 8.0, N'Tốt');
GO

-- ============================================================================
-- 5. Cài đặt 4 Stored Procedure bắt buộc theo yêu cầu đề bài
-- ============================================================================

-- Stored Procedure 1: sp_Lop_GetAll (Lấy danh sách tất cả các lớp)
CREATE OR ALTER PROCEDURE sp_Lop_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT MaLop, TenLop
    FROM LOP
    ORDER BY TenLop ASC;
END;
GO

-- Stored Procedure 2: sp_MonHoc_GetAll (Lấy danh sách tất cả các môn học)
CREATE OR ALTER PROCEDURE sp_MonHoc_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT MaMon, TenMon, SoTinChi
    FROM MONHOC
    ORDER BY TenMon ASC;
END;
GO

-- Stored Procedure 3: sp_KetQua_GetByLopMon (Lấy danh sách sinh viên theo lớp và môn học)
-- Kỹ thuật LEFT JOIN để sinh viên chưa có điểm vẫn xuất hiện với điểm mặc định là 0
CREATE OR ALTER PROCEDURE sp_KetQua_GetByLopMon
    @MaLop NVARCHAR(20),
    @MaMon NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        sv.MaSV,
        sv.HoTen,
        CAST(ISNULL(kq.Diem, 0.0) AS FLOAT) AS Diem,
        ISNULL(kq.GhiChu, N'') AS GhiChu
    FROM SINHVIEN sv
    LEFT JOIN KETQUA kq 
        ON sv.MaSV = kq.MaSV AND kq.MaMon = @MaMon
    WHERE sv.MaLop = @MaLop
    ORDER BY sv.HoTen ASC;
END;
GO

-- Stored Procedure 4: sp_KetQua_Save (Upsert: Cập nhật nếu đã có, Thêm mới nếu chưa có)
CREATE OR ALTER PROCEDURE sp_KetQua_Save
    @MaSV   NVARCHAR(20),
    @MaMon  NVARCHAR(20),
    @Diem   FLOAT,
    @GhiChu NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM KETQUA WHERE MaSV = @MaSV AND MaMon = @MaMon)
    BEGIN
        UPDATE KETQUA
        SET Diem = @Diem,
            GhiChu = @GhiChu
        WHERE MaSV = @MaSV AND MaMon = @MaMon;
    END
    ELSE
    BEGIN
        INSERT INTO KETQUA (MaSV, MaMon, Diem, GhiChu)
        VALUES (@MaSV, @MaMon, @Diem, @GhiChu);
    END
END;
GO

PRINT N'>> Toàn bộ CSDL QLSinhVienLab12 và 4 Stored Procedure đã được biên dịch thành công!';
GO
