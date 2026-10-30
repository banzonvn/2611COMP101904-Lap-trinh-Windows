========================================================================
COMP1019 - LẬP TRÌNH TRÊN WINDOWS | BÁO CÁO BÀN GIAO LAB 15
HỌC PHẦN: LẬP TRÌNH TRÊN WINDOWS (COMP1019) - THẦY LÊ THANH THOẠI
KHOA CÔNG NGHỆ THÔNG TIN - TRƯỜNG ĐẠI HỌC SƯ PHẠM TP. HỒ CHÍ MINH
========================================================================

THÔNG TIN SINH VIÊN THỰC HIỆN:
- Họ và tên: Lê Hoàng Quân
- Mã số sinh viên: 51.01.104.082
- Lớp sinh hoạt: 51.CNTT.A
- Nhóm thực hành: Nhóm 04
- Bài thực hành: Lab 15 - Mô hình MVP, Kiểm thử và Bàn giao đồ án

------------------------------------------------------------------------
1. MÔI TRƯỜNG VÀ CÔNG CỤ PHÁT TRIỂN:
------------------------------------------------------------------------
- Hệ điều hành: Windows 10 / Windows 11 (64-bit)
- Nền tảng: .NET 10.0 SDK (net10.0-windows)
- Môi trường phát triển: Visual Studio 2026 / Visual Studio Code / Rider
- Loại ứng dụng: Windows Forms Application (C# 13 / C# 14)
- Kiến trúc phần mềm: Model - View - Presenter (MVP) kết hợp Repository Pattern

------------------------------------------------------------------------
2. CÁCH KHỞI CHẠY CHƯƠNG TRÌNH:
------------------------------------------------------------------------
Cách 1: Khởi chạy bằng .NET CLI (Khuyên dùng)
  1. Mở PowerShell hoặc Terminal tại thư mục chứa mã nguồn:
     cd "C:\Users\Hoang Quan\Documents\Lập trình window Quan\Lab15\Lab15"
  2. Biên dịch dự án:
     dotnet build
  3. Khởi chạy ứng dụng:
     dotnet run

Cách 2: Chế độ kiểm thử tự động và xuất ảnh minh chứng (Headless Test Runner)
  1. Chạy lệnh:
     dotnet run -- --capture-all
  2. Toàn bộ 12 ca kiểm thử sẽ được thực thi tự động và lưu ảnh chụp màn hình
     vào thư mục: Lab15/images/ và Lab15/Lab15/images/

Cách 3: Mở bằng Visual Studio
  1. Nhấp đúp vào file solution "Lab15.slnx" hoặc mở Visual Studio.
  2. Chọn cấu hình "Debug" hoặc "Release" với Target Framework .NET 10.0.
  3. Nhấn F5 (hoặc Ctrl + F5) để bắt đầu chạy.

------------------------------------------------------------------------
3. TÀI KHOẢN VÀ DỮ LIỆU DEMO BÀN GIAO:
------------------------------------------------------------------------
- Tài khoản quản trị viên demo (phục vụ đồ án):
  + Tên đăng nhập: admin
  + Mật khẩu: admin123
  + Quyền hạn: Toàn quyền quản trị (Quản lý sinh viên, điểm số, tra cứu)
- Tài khoản người dùng (giảng viên/sinh viên tra cứu):
  + Tên đăng nhập: user
  + Mật khẩu: user123
  + Quyền hạn: Xem danh sách, tra cứu điểm và xuất báo cáo

- Dữ liệu mẫu khởi tạo sẵn trong hệ thống:
  Hệ thống được nạp sẵn 12 sinh viên mẫu thuộc 3 lớp (51.CNTT.A, CTK47A, CTK47B):
  * 51.01.104.082 | Lê Hoàng Quân       | 51.CNTT.A | 9.8 (Xuất sắc - Đạt)
  * 51.01.104.001 | Nguyễn Văn An       | 51.CNTT.A | 8.5 (Giỏi - Đạt)
  * 51.01.104.015 | Trần Thị Mai        | 51.CNTT.A | 7.6 (Khá - Đạt)
  * 51.01.104.022 | Phạm Đức Minh       | 51.CNTT.A | 4.5 (Yếu - Không đạt)
  * 51.01.104.035 | Lê Thị Hương Giang  | 51.CNTT.A | 8.9 (Giỏi - Đạt)
  * 47.01.104.012 | Đặng Hoàng Nam      | CTK47A    | 6.8 (Khá - Đạt)
  * 47.01.104.019 | Vũ Quỳnh Nga        | CTK47A    | 9.2 (Xuất sắc - Đạt)
  * 47.01.104.045 | Bùi Trọng Khang     | CTK47A    | 5.4 (Trung bình - Đạt)
  * 47.01.104.067 | Hoàng Lan Chi       | CTK47B    | 7.9 (Khá - Đạt)
  * 47.01.104.088 | Ngô Kiến Huy        | CTK47B    | 3.8 (Yếu - Không đạt)
  * 47.01.104.099 | Đỗ Kim Ngân         | CTK47B    | 8.2 (Giỏi - Đạt)
  * 51.01.104.105 | Đinh Tuấn Kiệt      | 51.CNTT.A | 6.5 (Khá - Đạt)

========================================================================
