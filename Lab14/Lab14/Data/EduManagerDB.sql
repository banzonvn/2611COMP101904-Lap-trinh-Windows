-- =====================================================================
-- HỌC PHẦN: LẬP TRÌNH TRÊN WINDOWS (COMP1019)
-- BÀI LAB 14: ENTITY FRAMEWORK CORE VÀ MÔ HÌNH BA LỚP (GUI - BLL - DAL)
-- GIẢNG VIÊN: ThS. Lê Thanh Thoại
-- SINH VIÊN: Lê Hoàng Quân - MSSV: 51.01.104.082 - Lớp: 51.CNTT.A
-- =====================================================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'EduManagerDB')
BEGIN
    CREATE DATABASE EduManagerDB;
END
GO

USE EduManagerDB;
GO

-- Xóa bảng MONHOC nếu đã tồn tại để tạo mới tinh gọn
IF OBJECT_ID(N'dbo.MONHOC', N'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.MONHOC;
END
GO

-- Tạo bảng MONHOC đúng chuẩn ràng buộc đề bài
CREATE TABLE dbo.MONHOC (
    MaMonHoc    NVARCHAR(10)  NOT NULL,
    TenMonHoc   NVARCHAR(100) NOT NULL,
    SoTinChi    INT           NOT NULL,
    TrangThai   BIT           NOT NULL CONSTRAINT DF_MONHOC_TrangThai DEFAULT 1,
    
    CONSTRAINT PK_MONHOC PRIMARY KEY CLUSTERED (MaMonHoc),
    CONSTRAINT CK_MONHOC_SoTinChi CHECK (SoTinChi >= 1 AND SoTinChi <= 6)
);
GO

-- Chèn dữ liệu mẫu phong phú đại diện cho chương trình đào tạo CNTT
INSERT INTO dbo.MONHOC (MaMonHoc, TenMonHoc, SoTinChi, TrangThai) VALUES
(N'COMP1019', N'Lập trình trên Windows', 3, 1),
(N'COMP1002', N'Cấu trúc dữ liệu và giải thuật', 4, 1),
(N'COMP1011', N'Cơ sở dữ liệu', 3, 1),
(N'COMP1024', N'Lập trình hướng đối tượng', 3, 1),
(N'COMP1035', N'Mạng máy tính cơ bản', 3, 1),
(N'COMP1042', N'Phát triển ứng dụng Web', 4, 1),
(N'SOFT1005', N'Kỹ năng mềm và làm việc nhóm', 2, 1),
(N'MATH1001', N'Giải tích cơ bản', 3, 0); -- Môn đã ngừng áp dụng để kiểm thử trạng thái
GO

-- Kiểm tra truy vấn
SELECT 
    MaMonHoc, 
    TenMonHoc, 
    SoTinChi, 
    CASE WHEN TrangThai = 1 THEN N'Còn sử dụng' ELSE N'Ngừng sử dụng' END AS TinhTrang
FROM dbo.MONHOC
ORDER BY MaMonHoc;
GO
