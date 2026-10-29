-- ============================================================================
-- TRƯỜNG ĐẠI HỌC SƯ PHẠM TP. HỒ CHÍ MINH - KHOA CÔNG NGHỆ THÔNG TIN
-- COMP1019 - LẬP TRÌNH TRÊN WINDOWS
-- BUỔI 10 - LAB 10: THIẾT KẾ VÀ CÀI ĐẶT CƠ SỞ DỮ LIỆU EduManagerDB
-- Sinh viên: Lê Hoàng Quân | MSSV: 51.01.104.082 | Lớp: 51.CNTT.A
-- File 01: Tạo Database, 6 bảng dữ liệu, Khóa chính, Khóa ngoại và Ràng buộc
-- ============================================================================

USE master;
GO

-- 1. Xóa database cũ nếu đã tồn tại để đảm bảo tính làm sạch khi chạy lại script
IF EXISTS (SELECT 1 FROM sys.databases WHERE name = 'EduManagerDB')
BEGIN
    ALTER DATABASE EduManagerDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE EduManagerDB;
END
GO

-- 2. Tạo mới Database EduManagerDB
CREATE DATABASE EduManagerDB;
GO

USE EduManagerDB;
GO

-- ============================================================================
-- 3. Tạo các bảng dữ liệu theo đúng thứ tự phụ thuộc khóa ngoại
-- Thứ tự tạo: KHOA -> LOP -> SINHVIEN -> MONHOC -> KETQUA -> TAIKHOAN
-- ============================================================================

-- Bảng 1: KHOA (Quản lý các khoa đào tạo trong trường)
CREATE TABLE KHOA (
    MaKhoa      VARCHAR(20)     NOT NULL,
    TenKhoa     NVARCHAR(100)   NOT NULL,
    DienThoai   VARCHAR(15)     NULL,
    CONSTRAINT PK_KHOA PRIMARY KEY (MaKhoa)
);
GO

-- Bảng 2: LOP (Mỗi lớp học thuộc một khoa đào tạo)
CREATE TABLE LOP (
    MaLop       VARCHAR(20)     NOT NULL,
    TenLop      NVARCHAR(100)   NOT NULL,
    MaKhoa      VARCHAR(20)     NOT NULL,
    CONSTRAINT PK_LOP PRIMARY KEY (MaLop),
    CONSTRAINT FK_LOP_KHOA FOREIGN KEY (MaKhoa) 
        REFERENCES KHOA(MaKhoa)
        ON UPDATE CASCADE
        ON DELETE NO ACTION
);
GO

-- Bảng 3: SINHVIEN (Mỗi sinh viên thuộc một lớp học)
CREATE TABLE SINHVIEN (
    MaSV        VARCHAR(20)     NOT NULL,
    HoTen       NVARCHAR(100)   NOT NULL,
    NgaySinh    DATE            NULL,
    GioiTinh    NVARCHAR(10)    NOT NULL,
    MaLop       VARCHAR(20)     NOT NULL,
    CONSTRAINT PK_SINHVIEN PRIMARY KEY (MaSV),
    CONSTRAINT CK_SINHVIEN_GioiTinh CHECK (GioiTinh IN (N'Nam', N'Nữ')),
    CONSTRAINT FK_SINHVIEN_LOP FOREIGN KEY (MaLop) 
        REFERENCES LOP(MaLop)
        ON UPDATE CASCADE
        ON DELETE NO ACTION
);
GO

-- Bảng 4: MONHOC (Quản lý danh mục môn học đào tạo)
CREATE TABLE MONHOC (
    MaMH        VARCHAR(20)     NOT NULL,
    TenMH       NVARCHAR(100)   NOT NULL,
    SoTinChi    INT             NOT NULL,
    CONSTRAINT PK_MONHOC PRIMARY KEY (MaMH),
    CONSTRAINT CK_MONHOC_SoTinChi CHECK (SoTinChi > 0)
);
GO

-- Bảng 5: KETQUA (Lưu điểm của sinh viên theo từng môn học)
-- Áp dụng khóa chính ghép (MaSV, MaMH) để mỗi sinh viên chỉ có 1 kết quả cho 1 môn
CREATE TABLE KETQUA (
    MaSV        VARCHAR(20)     NOT NULL,
    MaMH        VARCHAR(20)     NOT NULL,
    Diem        DECIMAL(4,2)    NOT NULL,
    CONSTRAINT PK_KETQUA PRIMARY KEY (MaSV, MaMH),
    CONSTRAINT CK_KETQUA_Diem CHECK (Diem >= 0.0 AND Diem <= 10.0),
    CONSTRAINT FK_KETQUA_SINHVIEN FOREIGN KEY (MaSV) 
        REFERENCES SINHVIEN(MaSV)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    CONSTRAINT FK_KETQUA_MONHOC FOREIGN KEY (MaMH) 
        REFERENCES MONHOC(MaMH)
        ON UPDATE CASCADE
        ON DELETE NO ACTION
);
GO

-- Bảng 6: TAIKHOAN (Quản lý tài khoản đăng nhập hệ thống)
CREATE TABLE TAIKHOAN (
    TenDangNhap VARCHAR(50)     NOT NULL,
    MatKhau     VARCHAR(100)    NOT NULL,
    HoTen       NVARCHAR(100)   NOT NULL,
    VaiTro      VARCHAR(20)     NOT NULL CONSTRAINT DF_TAIKHOAN_VaiTro DEFAULT 'User',
    TrangThai   BIT             NOT NULL CONSTRAINT DF_TAIKHOAN_TrangThai DEFAULT 1,
    CONSTRAINT PK_TAIKHOAN PRIMARY KEY (TenDangNhap)
);
GO

-- ============================================================================
-- 4. BỔ SUNG NÂNG CAO: Views hữu ích chuẩn bị cho ADO.NET buổi 11
-- ============================================================================

-- View 1: Danh sách sinh viên đầy đủ thông tin kèm Lớp và Khoa
CREATE OR ALTER VIEW vw_DanhSachSinhVienDayDu
AS
SELECT 
    sv.MaSV,
    sv.HoTen,
    sv.NgaySinh,
    sv.GioiTinh,
    l.MaLop,
    l.TenLop,
    k.MaKhoa,
    k.TenKhoa
FROM SINHVIEN sv
INNER JOIN LOP l ON sv.MaLop = l.MaLop
INNER JOIN KHOA k ON l.MaKhoa = k.MaKhoa;
GO

-- View 2: Bảng điểm chi tiết có phân loại học lực
CREATE OR ALTER VIEW vw_BangDiemChiTiet
AS
SELECT 
    sv.MaSV,
    sv.HoTen,
    l.TenLop,
    mh.MaMH,
    mh.TenMH,
    mh.SoTinChi,
    kq.Diem,
    CASE 
        WHEN kq.Diem >= 8.5 THEN N'Giỏi'
        WHEN kq.Diem >= 7.0 THEN N'Khá'
        WHEN kq.Diem >= 5.0 THEN N'Trung bình'
        ELSE N'Yếu'
    END AS XepLoai
FROM KETQUA kq
INNER JOIN SINHVIEN sv ON kq.MaSV = sv.MaSV
INNER JOIN LOP l ON sv.MaLop = l.MaLop
INNER JOIN MONHOC mh ON kq.MaMH = mh.MaMH;
GO
