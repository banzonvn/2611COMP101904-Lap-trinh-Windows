-- ============================================================================
-- TRƯỜNG ĐẠI HỌC SƯ PHẠM TP. HỒ CHÍ MINH - KHOA CÔNG NGHỆ THÔNG TIN
-- COMP1019 - LẬP TRÌNH TRÊN WINDOWS
-- BUỔI 11 - LAB 11: ADO.NET VÀ CRUD VỚI SQL SERVER
-- Sinh viên: Lê Hoàng Quân | MSSV: 51.01.104.082 | Lớp: 51.CNTT.A | Nhóm: 4
-- File Script CSDL: EduManagerDB.sql
-- ============================================================================

USE master;
GO

-- 1. Tạo database nếu chưa tồn tại
IF DB_ID('EduManagerDB') IS NULL
BEGIN
    CREATE DATABASE EduManagerDB;
    PRINT N'>> Đã tạo mới CSDL EduManagerDB thành công.';
END
ELSE
BEGIN
    PRINT N'>> CSDL EduManagerDB đã tồn tại sẵn sàng sử dụng.';
END
GO

USE EduManagerDB;
GO

-- 2. Xóa các bảng cũ theo đúng thứ tự phụ thuộc khóa ngoại nếu đã tồn tại
IF OBJECT_ID('SINHVIEN', 'U') IS NOT NULL 
    DROP TABLE SINHVIEN;
IF OBJECT_ID('LOP', 'U') IS NOT NULL 
    DROP TABLE LOP;
GO

-- 3. Tạo bảng LOP (Bảng cha quản lý danh mục lớp học)
CREATE TABLE LOP (
    MaLop   VARCHAR(20)     NOT NULL,
    TenLop  NVARCHAR(100)   NOT NULL,
    CONSTRAINT PK_LOP PRIMARY KEY (MaLop)
);
GO

-- 4. Tạo bảng SINHVIEN (Bảng con tham chiếu đến bảng LOP)
-- Ràng buộc khóa ngoại FK_SINHVIEN_LOP ngăn chặn xóa lớp đang có sinh viên
CREATE TABLE SINHVIEN (
    MaSV    VARCHAR(20)     NOT NULL,
    HoTen   NVARCHAR(100)   NOT NULL,
    MaLop   VARCHAR(20)     NOT NULL,
    CONSTRAINT PK_SINHVIEN PRIMARY KEY (MaSV),
    CONSTRAINT FK_SINHVIEN_LOP FOREIGN KEY (MaLop) 
        REFERENCES LOP(MaLop)
        ON UPDATE CASCADE
        ON DELETE NO ACTION
);
GO

-- 5. Chèn dữ liệu mẫu cho bảng LOP
INSERT INTO LOP (MaLop, TenLop) VALUES
('CNTT01',  N'Công nghệ thông tin 01'),
('CNTT02',  N'Công nghệ thông tin 02'),
('SPTIN01', N'Sư phạm Tin học 01'),
('SPTOAN',  N'Sư phạm Toán học 01'); -- Lớp trống chưa có sinh viên để kiểm thử Xóa hợp lệ
GO

-- 6. Chèn dữ liệu mẫu cho bảng SINHVIEN
INSERT INTO SINHVIEN (MaSV, HoTen, MaLop) VALUES
('51.01.104.082', N'Lê Hoàng Quân',  'CNTT01'),
('SV001',         N'Nguyễn Văn An',   'CNTT01'),
('SV002',         N'Trần Thị Bình',   'CNTT01'),
('SV003',         N'Phạm Minh Tuấn',  'CNTT02'),
('SV004',         N'Đặng Thu Hà',     'CNTT02'),
('SV005',         N'Võ Quốc Bảo',     'SPTIN01');
GO

-- 7. Kiểm tra dữ liệu và thống kê số sinh viên theo từng lớp (Query mẫu cho Form)
SELECT 
    l.MaLop, 
    l.TenLop, 
    COUNT(sv.MaSV) AS SoSinhVien
FROM LOP l
LEFT JOIN SINHVIEN sv ON l.MaLop = sv.MaLop
GROUP BY l.MaLop, l.TenLop
ORDER BY l.MaLop ASC;
GO
