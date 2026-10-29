-- ============================================================================
-- TRƯỜNG ĐẠI HỌC SƯ PHẠM TP. HỒ CHÍ MINH - KHOA CÔNG NGHỆ THÔNG TIN
-- COMP1019 - LẬP TRÌNH TRÊN WINDOWS
-- BUỔI 10 - LAB 10: THIẾT KẾ VÀ CÀI ĐẶT CƠ SỞ DỮ LIỆU EduManagerDB
-- Sinh viên: Lê Hoàng Quân | MSSV: 51.01.104.082 | Lớp: 51.CNTT.A
-- File 02: Chèn dữ liệu mẫu vào 6 bảng dữ liệu (KHOA, LOP, SINHVIEN, MONHOC, KETQUA, TAIKHOAN)
-- ============================================================================

USE EduManagerDB;
GO

-- Xóa dữ liệu cũ theo thứ tự ngược lại của khóa ngoại để tránh vi phạm tham chiếu
DELETE FROM KETQUA;
DELETE FROM SINHVIEN;
DELETE FROM MONHOC;
DELETE FROM LOP;
DELETE FROM KHOA;
DELETE FROM TAIKHOAN;
GO

-- ============================================================================
-- 1. CHÈN DỮ LIỆU BẢNG KHOA (Tối thiểu 2 khoa -> chèn 3 khoa)
-- ============================================================================
INSERT INTO KHOA (MaKhoa, TenKhoa, DienThoai) VALUES
('CNTT', N'Khoa Công nghệ Thông tin', '02838352020'),
('TOAN', N'Khoa Toán - Tin học', '02838352021'),
('NGOAINGU', N'Khoa Ngoại ngữ', '02838352022');
GO

-- ============================================================================
-- 2. CHÈN DỮ LIỆU BẢNG LOP (Tối thiểu 3 lớp -> chèn 4 lớp)
-- ============================================================================
INSERT INTO LOP (MaLop, TenLop, MaKhoa) VALUES
('51.CNTT.A', N'Cử nhân Công nghệ Thông tin Khóa 51A', 'CNTT'),
('51.CNTT.B', N'Cử nhân Công nghệ Thông tin Khóa 51B', 'CNTT'),
('51.SPTIN',   N'Sư phạm Tin học Khóa 51', 'CNTT'),
('51.SPTOAN',  N'Sư phạm Toán học Khóa 51', 'TOAN');
GO

-- ============================================================================
-- 3. CHÈN DỮ LIỆU BẢNG SINHVIEN (Tối thiểu 6 SV -> chèn 8 SV)
-- Trong đó có SV001 là Lê Hoàng Quân (MSSV: 51.01.104.082)
-- SV008 là sinh viên chưa có điểm (để test câu 6: DELETE sinh viên chưa có điểm)
-- ============================================================================
INSERT INTO SINHVIEN (MaSV, HoTen, NgaySinh, GioiTinh, MaLop) VALUES
('51.01.104.082', N'Lê Hoàng Quân',    '2003-05-18', N'Nam', '51.CNTT.A'),
('SV002',         N'Trần Thị Mai',     '2003-09-22', N'Nữ',  '51.CNTT.A'),
('SV003',         N'Nguyễn Văn An',    '2003-01-15', N'Nam', '51.CNTT.A'),
('SV004',         N'Phạm Minh Tuấn',   '2003-11-03', N'Nam', '51.CNTT.B'),
('SV005',         N'Đặng Thu Hà',      '2003-07-30', N'Nữ',  '51.CNTT.B'),
('SV006',         N'Võ Quốc Bảo',      '2003-03-12', N'Nam', '51.SPTIN'),
('SV007',         N'Lê Hồng Phúc',     '2003-12-25', N'Nam', '51.SPTOAN'),
('SV008',         N'Hoàng Ngọc Ánh',   '2003-08-19', N'Nữ',  '51.SPTIN');
GO

-- ============================================================================
-- 4. CHÈN DỮ LIỆU BẢNG MONHOC (Tối thiểu 4 môn -> chèn 5 môn)
-- ============================================================================
INSERT INTO MONHOC (MaMH, TenMH, SoTinChi) VALUES
('COMP1019', N'Lập trình trên Windows', 3),
('COMP1017', N'Hệ quản trị Cơ sở dữ liệu', 3),
('COMP1020', N'Cấu trúc dữ liệu và giải thuật', 4),
('COMP1044', N'Mạng máy tính căn bản', 3),
('MATH1001', N'Toán cao cấp A1', 3);
GO

-- ============================================================================
-- 5. CHÈN DỮ LIỆU BẢNG KETQUA (Tối thiểu 8 kết quả -> chèn 14 kết quả)
-- Lưu ý: SV008 cố ý không chèn điểm nào để phục vụ kiểm thử truy vấn 6
-- ============================================================================
INSERT INTO KETQUA (MaSV, MaMH, Diem) VALUES
-- Sinh viên Lê Hoàng Quân (51.01.104.082)
('51.01.104.082', 'COMP1019', 9.50),
('51.01.104.082', 'COMP1017', 9.00),
('51.01.104.082', 'COMP1020', 8.75),

-- Sinh viên Trần Thị Mai (SV002)
('SV002', 'COMP1019', 8.50),
('SV002', 'COMP1017', 8.00),
('SV002', 'COMP1020', 7.50),

-- Sinh viên Nguyễn Văn An (SV003)
('SV003', 'COMP1019', 7.00),
('SV003', 'COMP1017', 6.50),

-- Sinh viên Phạm Minh Tuấn (SV004)
('SV004', 'COMP1019', 6.00),
('SV004', 'COMP1044', 7.50),

-- Sinh viên Đặng Thu Hà (SV005)
('SV005', 'COMP1019', 8.75),
('SV005', 'COMP1044', 9.00),

-- Sinh viên Võ Quốc Bảo (SV006)
('SV006', 'COMP1019', 8.00),

-- Sinh viên Lê Hồng Phúc (SV007)
('SV007', 'MATH1001', 9.25);
GO

-- ============================================================================
-- 6. CHÈN DỮ LIỆU BẢNG TAIKHOAN (Tối thiểu 2 tài khoản -> chèn 3 tài khoản)
-- Mật khẩu được mã hóa SHA-256 hoặc hash trong ứng dụng thực tế
-- ============================================================================
INSERT INTO TAIKHOAN (TenDangNhap, MatKhau, HoTen, VaiTro, TrangThai) VALUES
('admin',     'Admin@123',   N'Quản trị viên Hệ thống', 'Admin',   1),
('gv_thoai',  'GvThoai@2026', N'ThS. Lê Thanh Thoại',    'Teacher', 1),
('sv_quan',   'Quan@5101',   N'Lê Hoàng Quân',          'Student', 1);
GO

-- ============================================================================
-- 7. KIỂM TRA SỐ LƯỢNG BẢN GHI ĐÃ ĐƯỢC CHÈN THÀNH CÔNG
-- ============================================================================
SELECT 'KHOA'     AS TenBang, COUNT(*) AS TongSoDong FROM KHOA UNION ALL
SELECT 'LOP'      AS TenBang, COUNT(*) AS TongSoDong FROM LOP UNION ALL
SELECT 'SINHVIEN' AS TenBang, COUNT(*) AS TongSoDong FROM SINHVIEN UNION ALL
SELECT 'MONHOC'   AS TenBang, COUNT(*) AS TongSoDong FROM MONHOC UNION ALL
SELECT 'KETQUA'   AS TenBang, COUNT(*) AS TongSoDong FROM KETQUA UNION ALL
SELECT 'TAIKHOAN' AS TenBang, COUNT(*) AS TongSoDong FROM TAIKHOAN;
GO
