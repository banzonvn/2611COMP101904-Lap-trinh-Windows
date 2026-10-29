-- ============================================================================
-- TRƯỜNG ĐẠI HỌC SƯ PHẠM TP. HỒ CHÍ MINH - KHOA CÔNG NGHỆ THÔNG TIN
-- COMP1019 - LẬP TRÌNH TRÊN WINDOWS
-- BÀI LAB 12: DATA BINDING, STORED PROCEDURE VÀ TRANSACTION
-- Sinh viên thực hiện: Lê Hoàng Quân | MSSV: 51.01.104.082 | Lớp: 51.CNTT.A | Nhóm: 4
-- File CSDL & Stored Procedure: QLSinhVienLab12.sql
-- ============================================================================

USE master;
GO

-- 1. Tạo Database QLSinhVienLab12 nếu chưa tồn tại
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

-- 2. Xóa các bảng cũ theo thứ tự ngược của khóa ngoại nếu đã tồn tại
IF OBJECT_ID('KETQUA', 'U') IS NOT NULL DROP TABLE KETQUA;
IF OBJECT_ID('SINHVIEN', 'U') IS NOT NULL DROP TABLE SINHVIEN;
IF OBJECT_ID('MONHOC', 'U') IS NOT NULL DROP TABLE MONHOC;
IF OBJECT_ID('LOP', 'U') IS NOT NULL DROP TABLE LOP;
GO

-- 3. Tạo bảng LOP (Quản lý các lớp học)
CREATE TABLE LOP (
    MaLop   NVARCHAR(20)    NOT NULL,
    TenLop  NVARCHAR(100)   NOT NULL,
    CONSTRAINT PK_LOP PRIMARY KEY (MaLop)
);
GO

-- 4. Tạo bảng SINHVIEN (Mỗi sinh viên thuộc một lớp học)
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

-- 5. Tạo bảng MONHOC (Danh mục môn học)
CREATE TABLE MONHOC (
    MaMon       NVARCHAR(20)    NOT NULL,
    TenMon      NVARCHAR(100)   NOT NULL,
    SoTinChi    INT             NOT NULL,
    CONSTRAINT PK_MONHOC PRIMARY KEY (MaMon),
    CONSTRAINT CK_MONHOC_SoTinChi CHECK (SoTinChi > 0)
);
GO

-- 6. Tạo bảng KETQUA (Lưu điểm của sinh viên theo từng môn học)
-- Khóa chính kép (MaSV, MaMon) đảm bảo mỗi sinh viên chỉ có 1 kết quả cho mỗi môn
CREATE TABLE KETQUA (
    MaSV    NVARCHAR(20)    NOT NULL,
    MaMon   NVARCHAR(20)    NOT NULL,
    Diem    FLOAT           NOT NULL,
    GhiChu  NVARCHAR(255)   NULL,
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
-- 7. CHÈN DỮ LIỆU MẪU ĐẦY ĐỦ VÀ HỢP LỆ
-- ============================================================================

-- Chèn dữ liệu bảng LOP
INSERT INTO LOP (MaLop, TenLop) VALUES
(N'51.CNTT.A', N'Cử nhân Công nghệ Thông tin Khóa 51A'),
(N'CTK47A',    N'Công nghệ thông tin K47A'),
(N'CTK47B',    N'Công nghệ thông tin K47B');
GO

-- Chèn dữ liệu bảng SINHVIEN (Có sinh viên Lê Hoàng Quân MSSV: 51.01.104.082)
INSERT INTO SINHVIEN (MaSV, HoTen, NgaySinh, MaLop) VALUES
(N'51.01.104.082', N'Lê Hoàng Quân',   '2003-05-18', N'51.CNTT.A'),
(N'SV001',         N'Nguyễn Văn An',   '2005-01-15', N'51.CNTT.A'),
(N'SV002',         N'Trần Thị Bình',   '2005-05-20', N'51.CNTT.A'),
(N'SV003',         N'Lê Minh Cường',   '2005-03-11', N'CTK47A'),
(N'SV004',         N'Phạm Thanh Dung', '2005-07-09', N'CTK47A'),
(N'SV005',         N'Hoàng Ngọc Ánh',  '2005-11-25', N'CTK47B');
GO

-- Chèn dữ liệu bảng MONHOC
INSERT INTO MONHOC (MaMon, TenMon, SoTinChi) VALUES
(N'COMP1019', N'Lập trình trên Windows', 3),
(N'COMP1017', N'Cơ sở dữ liệu & ADO.NET', 3),
(N'COMP1020', N'Cấu trúc dữ liệu & Giải thuật', 4);
GO

-- Chèn dữ liệu bảng KETQUA mẫu
INSERT INTO KETQUA (MaSV, MaMon, Diem, GhiChu) VALUES
(N'51.01.104.082', N'COMP1019', 9.5, N'Xuất sắc'),
(N'SV001',         N'COMP1019', 8.5, N'Giỏi'),
(N'SV002',         N'COMP1019', 7.0, N'Khá'),
(N'SV003',         N'COMP1017', 8.0, N'Giỏi');
GO

-- ============================================================================
-- 8. TẠO CÁC STORED PROCEDURE BẮT BUỘC THEO ĐỀ BÀI
-- ============================================================================

-- Stored Procedure 1: sp_Lop_GetAll (Lấy toàn bộ danh sách lớp học)
CREATE OR ALTER PROCEDURE sp_Lop_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT MaLop, TenLop
    FROM LOP
    ORDER BY TenLop ASC;
END;
GO

-- Stored Procedure 2: sp_MonHoc_GetAll (Lấy toàn bộ danh sách môn học)
CREATE OR ALTER PROCEDURE sp_MonHoc_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT MaMon, TenMon, SoTinChi
    FROM MONHOC
    ORDER BY TenMon ASC;
END;
GO

-- Stored Procedure 3: sp_KetQua_GetByLopMon
-- Lấy danh sách sinh viên theo lớp và môn học (dùng LEFT JOIN để hiển thị cả sinh viên chưa có điểm)
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

-- Stored Procedure 4: sp_KetQua_Save
-- Upsert điểm của sinh viên: Nếu đã có điểm thì UPDATE, chưa có thì INSERT
CREATE OR ALTER PROCEDURE sp_KetQua_Save
    @MaSV   NVARCHAR(20),
    @MaMon  NVARCHAR(20),
    @Diem   FLOAT,
    @GhiChu NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Kiểm tra ràng buộc điểm trước khi lưu
    IF @Diem < 0.0 OR @Diem > 10.0
    BEGIN
        RAISERROR(N'Điểm số phải nằm trong khoảng từ 0.0 đến 10.0!', 16, 1);
        RETURN;
    END

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

PRINT N'>> Khởi tạo toàn bộ CSDL và Stored Procedure QLSinhVienLab12 hoàn tất thành công!';
GO
