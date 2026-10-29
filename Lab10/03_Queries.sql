-- ============================================================================
-- TRƯỜNG ĐẠI HỌC SƯ PHẠM TP. HỒ CHÍ MINH - KHOA CÔNG NGHỆ THÔNG TIN
-- COMP1019 - LẬP TRÌNH TRÊN WINDOWS
-- BUỔI 10 - LAB 10: THIẾT KẾ VÀ CÀI ĐẶT CƠ SỞ DỮ LIỆU EduManagerDB
-- Sinh viên: Lê Hoàng Quân | MSSV: 51.01.104.082 | Lớp: 51.CNTT.A
-- File 03: 12 Câu truy vấn T-SQL theo đúng yêu cầu đề bài
-- ============================================================================

USE EduManagerDB;
GO

-- ============================================================================
-- TRUY VẤN 1: Liệt kê tất cả sinh viên
-- Mục đích: Kiểm tra toàn bộ danh sách sinh viên hiện có trong hệ thống
-- ============================================================================
PRINT N'--- CÂU 1: DANH SÁCH TẤT CẢ SINH VIÊN ---';
SELECT 
    MaSV        AS [Mã Sinh Viên],
    HoTen       AS [Họ Và Tên],
    CONVERT(VARCHAR(10), NgaySinh, 103) AS [Ngày Sinh (dd/MM/yyyy)],
    GioiTinh    AS [Giới Tính],
    MaLop       AS [Mã Lớp]
FROM SINHVIEN
ORDER BY MaSV ASC;
GO

-- ============================================================================
-- TRUY VẤN 2: Liệt kê sinh viên kèm tên lớp và tên khoa
-- Kỹ thuật: INNER JOIN 3 bảng SINHVIEN, LOP, KHOA
-- ============================================================================
PRINT N'--- CÂU 2: SINH VIÊN KÈM TÊN LỚP VÀ TÊN KHOA ---';
SELECT 
    sv.MaSV         AS [Mã Sinh Viên],
    sv.HoTen        AS [Họ Tên Sinh Viên],
    sv.GioiTinh     AS [Giới Tính],
    l.TenLop        AS [Tên Lớp],
    k.TenKhoa       AS [Khoa Quản Lý]
FROM SINHVIEN sv
INNER JOIN LOP l ON sv.MaLop = l.MaLop
INNER JOIN KHOA k ON l.MaKhoa = k.MaKhoa
ORDER BY k.TenKhoa ASC, l.TenLop ASC, sv.HoTen ASC;
GO

-- ============================================================================
-- TRUY VẤN 3: Tìm kiếm sinh viên theo tên (hoặc từ khóa họ tên)
-- Kỹ thuật: WHERE HoTen LIKE N'%...%' và ORDER BY họ tên tăng dần
-- Ví dụ: Tìm các sinh viên có họ hoặc tên chứa chữ 'Quân' hoặc 'Mai'
-- ============================================================================
PRINT N'--- CÂU 3: TÌM KIẾM SINH VIÊN THEO TÊN (VD: Chứa chữ "Quân") ---';
DECLARE @TuKhoa NVARCHAR(50) = N'Quân';

SELECT 
    MaSV        AS [Mã Sinh Viên],
    HoTen       AS [Họ Và Tên],
    NgaySinh    AS [Ngày Sinh],
    GioiTinh    AS [Giới Tính],
    MaLop       AS [Mã Lớp]
FROM SINHVIEN
WHERE HoTen LIKE N'%' + @TuKhoa + N'%'
ORDER BY HoTen ASC;
GO

-- ============================================================================
-- TRUY VẤN 4: Liệt kê sinh viên thuộc một lớp cụ thể
-- Kỹ thuật: Lọc điều kiện WHERE MaLop = '51.CNTT.A'
-- ============================================================================
PRINT N'--- CÂU 4: DANH SÁCH SINH VIÊN LỚP 51.CNTT.A ---';
SELECT 
    sv.MaSV     AS [Mã SV],
    sv.HoTen    AS [Họ và Tên],
    sv.NgaySinh AS [Ngày Sinh],
    sv.GioiTinh AS [Giới Tính],
    l.TenLop    AS [Tên Lớp]
FROM SINHVIEN sv
INNER JOIN LOP l ON sv.MaLop = l.MaLop
WHERE sv.MaLop = '51.CNTT.A'
ORDER BY sv.MaSV ASC;
GO

-- ============================================================================
-- TRUY VẤN 5: Cập nhật thông tin sinh viên
-- Kỹ thuật: UPDATE ... WHERE MaSV = ...
-- Cập nhật ngày sinh và họ tên cho sinh viên SV003
-- ============================================================================
PRINT N'--- CÂU 5: CẬP NHẬT THÔNG TIN SINH VIÊN SV003 ---';
UPDATE SINHVIEN
SET HoTen = N'Nguyễn Văn An (Đã cập nhật)',
    NgaySinh = '2003-01-20'
WHERE MaSV = 'SV003';

-- Kiểm tra lại kết quả sau cập nhật
SELECT MaSV, HoTen, NgaySinh, GioiTinh, MaLop 
FROM SINHVIEN 
WHERE MaSV = 'SV003';
GO

-- ============================================================================
-- TRUY VẤN 6: Xóa sinh viên chưa có điểm trong bảng KETQUA
-- Kỹ thuật: DELETE ... WHERE NOT EXISTS (...)
-- Thực hiện an toàn: SV008 là sinh viên chưa có điểm, sẽ bị xóa
-- ============================================================================
PRINT N'--- CÂU 6: XÓA SINH VIÊN CHƯA CÓ BẤT KỲ ĐIỂM SỐ NÀO ---';
-- Hiển thị sinh viên sẽ bị xóa trước khi thực hiện
SELECT MaSV, HoTen, MaLop 
FROM SINHVIEN sv
WHERE NOT EXISTS (
    SELECT 1 FROM KETQUA kq WHERE kq.MaSV = sv.MaSV
);

-- Tiến hành xóa
DELETE FROM SINHVIEN
WHERE NOT EXISTS (
    SELECT 1 FROM KETQUA kq WHERE kq.MaSV = SINHVIEN.MaSV
);

PRINT N'>> Đã xóa thành công các sinh viên chưa có điểm trong bảng KETQUA.';
GO

-- ============================================================================
-- TRUY VẤN 7: Liệt kê điểm của từng sinh viên theo môn học
-- Kỹ thuật: INNER JOIN SINHVIEN, KETQUA, MONHOC
-- ============================================================================
PRINT N'--- CÂU 7: BẢNG ĐIỂM CHI TIẾT THEO TỪNG SINH VIÊN VÀ MÔN HỌC ---';
SELECT 
    sv.MaSV         AS [Mã SV],
    sv.HoTen        AS [Họ Tên Sinh Viên],
    mh.MaMH         AS [Mã Môn],
    mh.TenMH        AS [Tên Môn Học],
    mh.SoTinChi     AS [Số Tín Chỉ],
    kq.Diem         AS [Điểm Số]
FROM KETQUA kq
INNER JOIN SINHVIEN sv ON kq.MaSV = sv.MaSV
INNER JOIN MONHOC mh   ON kq.MaMH = mh.MaMH
ORDER BY sv.MaSV ASC, mh.MaMH ASC;
GO

-- ============================================================================
-- TRUY VẤN 8: Tính điểm trung bình của từng sinh viên
-- Kỹ thuật: AVG(Diem), ROUND, GROUP BY MaSV, HoTen
-- Đi kèm xếp loại học tập
-- ============================================================================
PRINT N'--- CÂU 8: ĐIỂM TRUNG BÌNH VÀ XẾP LOẠI CỦA TỪNG SINH VIÊN ---';
SELECT 
    sv.MaSV                             AS [Mã SV],
    sv.HoTen                            AS [Họ Tên Sinh Viên],
    COUNT(kq.MaMH)                      AS [Số Môn Đã Học],
    CAST(ROUND(AVG(kq.Diem), 2) AS DECIMAL(4,2)) AS [Điểm Trung Bình],
    CASE 
        WHEN AVG(kq.Diem) >= 8.5 THEN N'Giỏi / Xuất sắc'
        WHEN AVG(kq.Diem) >= 7.0 THEN N'Khá'
        WHEN AVG(kq.Diem) >= 5.0 THEN N'Trung bình'
        ELSE N'Yếu / Cần cố gắng'
    END AS [Xếp Loại Học Tập]
FROM SINHVIEN sv
INNER JOIN KETQUA kq ON sv.MaSV = kq.MaSV
GROUP BY sv.MaSV, sv.HoTen
ORDER BY [Điểm Trung Bình] DESC;
GO

-- ============================================================================
-- TRUY VẤN 9: Đếm số lượng sinh viên theo từng lớp
-- Kỹ thuật: LEFT JOIN giữa LOP và SINHVIEN để lớp không có SV vẫn đếm được 0
-- ============================================================================
PRINT N'--- CÂU 9: THỐNG KÊ SỐ LƯỢNG SINH VIÊN THEO TỪNG LỚP HỌC ---';
SELECT 
    l.MaLop             AS [Mã Lớp],
    l.TenLop            AS [Tên Lớp Học],
    k.TenKhoa           AS [Khoa],
    COUNT(sv.MaSV)      AS [Số Lượng Sinh Viên]
FROM LOP l
INNER JOIN KHOA k ON l.MaKhoa = k.MaKhoa
LEFT JOIN SINHVIEN sv ON l.MaLop = sv.MaLop
GROUP BY l.MaLop, l.TenLop, k.TenKhoa
ORDER BY [Số Lượng Sinh Viên] DESC;
GO

-- ============================================================================
-- TRUY VẤN 10: Liệt kê các lớp có từ 2 sinh viên trở lên
-- Kỹ thuật: GROUP BY ... HAVING COUNT(sv.MaSV) >= 2
-- ============================================================================
PRINT N'--- CÂU 10: CÁC LỚP CÓ TỪ 2 SINH VIÊN TRỞ LÊN ---';
SELECT 
    l.MaLop             AS [Mã Lớp],
    l.TenLop            AS [Tên Lớp Học],
    COUNT(sv.MaSV)      AS [Số Sinh Viên]
FROM LOP l
INNER JOIN SINHVIEN sv ON l.MaLop = sv.MaLop
GROUP BY l.MaLop, l.TenLop
HAVING COUNT(sv.MaSV) >= 2
ORDER BY [Số Sinh Viên] DESC;
GO

-- ============================================================================
-- TRUY VẤN 11: Kiểm tra vi phạm khóa ngoại (Foreign Key Constraint)
-- Kỹ thuật: Chèn sinh viên với MaLop 'LOP_KHONG_TON_TAI'
-- Kỳ vọng: SQL Server chặn lại với lỗi 547 (FK constraint violation)
-- ============================================================================
PRINT N'--- CÂU 11: TEST VI PHẠM KHÓA NGOẠI (FK_SINHVIEN_LOP) ---';
BEGIN TRY
    INSERT INTO SINHVIEN (MaSV, HoTen, NgaySinh, GioiTinh, MaLop)
    VALUES ('SV999', N'Sinh Viên Test Lỗi FK', '2003-01-01', N'Nam', 'LOP_KHONG_TON_TAI');
    
    PRINT N'[THẤT BẠI] Lẽ ra phải bị chặn bởi khóa ngoại FK_SINHVIEN_LOP!';
END TRY
BEGIN CATCH
    PRINT N'[THÀNH CÔNG - ĐÃ BẮT LỖI KHÓA NGOẠI THEO ĐÚNG THIẾT KẾ]';
    PRINT N'Mã lỗi: ' + CAST(ERROR_NUMBER() AS NVARCHAR(10));
    PRINT N'Thông điệp lỗi SQL Server: ' + ERROR_MESSAGE();
END CATCH;
GO

-- ============================================================================
-- TRUY VẤN 12: Kiểm tra vi phạm ràng buộc CHECK (Check Constraint)
-- Kỹ thuật: Chèn điểm 11.5 hoặc -1.0 vào bảng KETQUA (Quy định: 0.0 <= Diem <= 10.0)
-- Kỳ vọng: SQL Server chặn lại với lỗi 547 (CK_KETQUA_Diem violation)
-- ============================================================================
PRINT N'--- CÂU 12: TEST VI PHẠM RÀNG BUỘC CHECK (CK_KETQUA_Diem) ---';
BEGIN TRY
    INSERT INTO KETQUA (MaSV, MaMH, Diem)
    VALUES ('51.01.104.082', 'MATH1001', 11.5); -- Điểm vượt quá 10.0
    
    PRINT N'[THẤT BẠI] Lẽ ra phải bị chặn bởi ràng buộc CK_KETQUA_Diem!';
END TRY
BEGIN CATCH
    PRINT N'[THÀNH CÔNG - ĐÃ BẮT LỖI CHECK THEO ĐÚNG THIẾT KẾ]';
    PRINT N'Mã lỗi: ' + CAST(ERROR_NUMBER() AS NVARCHAR(10));
    PRINT N'Thông điệp lỗi SQL Server: ' + ERROR_MESSAGE();
END CATCH;
GO
