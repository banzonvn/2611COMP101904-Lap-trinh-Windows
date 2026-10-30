========================================================================
COMP1019 - LẬP TRÌNH TRÊN WINDOWS | BÁO CÁO BÀN GIAO LAB 07
HỌC PHẦN: LẬP TRÌNH TRÊN WINDOWS (COMP1019) - THẦY LÊ THANH THOẠI
KHOA CÔNG NGHỆ THÔNG TIN - TRƯỜNG ĐẠI HỌC SƯ PHẠM TP. HỒ CHÍ MINH
========================================================================

THÔNG TIN SINH VIÊN THỰC HIỆN:
- Họ và tên: Lê Hoàng Quân
- Mã số sinh viên: 51.01.104.082
- Lớp sinh hoạt: 51.CNTT.A
- Nhóm thực hành: Nhóm 04
- Bài thực hành: Lab 07 - SDI, MDI và ứng dụng nhiều Form trong C# WinForms
- Ứng dụng: ShopManagerMini (Quản lý cửa hàng tiện lợi mini đa màn hình MDI)

------------------------------------------------------------------------
1. MÔI TRƯỜNG VÀ CÔNG CỤ PHÁT TRIỂN:
------------------------------------------------------------------------
- Hệ điều hành: Windows 10 / Windows 11 (64-bit)
- Nền tảng: .NET 10.0 SDK (net10.0-windows)
- Môi trường phát triển: Visual Studio 2026 / Visual Studio Code / Rider
- Loại ứng dụng: Windows Forms Application (C# 13 / C# 14)
- Kiến trúc giao diện: MDI (Multiple Document Interface) Parent-Child

------------------------------------------------------------------------
2. CÁCH KHỞI CHẠY CHƯƠNG TRÌNH:
------------------------------------------------------------------------
Cách 1: Khởi chạy bằng .NET CLI (Khuyên dùng)
  1. Mở PowerShell hoặc Terminal tại thư mục chứa mã nguồn:
     cd "C:\Users\Hoang Quan\Documents\Lập trình window Quan\Lab07\Lab07"
  2. Biên dịch dự án:
     dotnet build
  3. Khởi chạy ứng dụng:
     dotnet run

Cách 2: Chế độ kiểm thử tự động và xuất ảnh minh chứng (Headless Test Runner)
  1. Chạy lệnh:
     dotnet run -- --capture-all
  2. Toàn bộ 17 ca kiểm thử sẽ được thực thi tự động và lưu ảnh chụp màn hình
     vào thư mục: Lab07/images/ và Lab07/Lab07/images/

Cách 3: Mở bằng Visual Studio
  1. Nhấp đúp vào file solution "Lab07.slnx" hoặc mở Visual Studio.
  2. Chọn cấu hình "Debug" hoặc "Release" với Target Framework .NET 10.0.
  3. Nhấn F5 (hoặc Ctrl + F5) để bắt đầu chạy.

------------------------------------------------------------------------
3. TÀI KHOẢN VÀ DỮ LIỆU DEMO BÀN GIAO:
------------------------------------------------------------------------
- Tài khoản quản trị viên demo:
  + Tên đăng nhập: admin
  + Mật khẩu: 123
  + Họ và tên: Quản trị viên
  + Vai trò: Admin (Toàn quyền quản lý kho hàng và khách hàng)

- Tài khoản nhân viên bán hàng:
  + Tên đăng nhập: user
  + Mật khẩu: 123
  + Họ và tên: Nhân viên bán hàng
  + Vai trò: User

- Tài khoản sinh viên thực hiện:
  + Tên đăng nhập: hoangquan
  + Mật khẩu: 123
  + Họ và tên: Lê Hoàng Quân
  + Vai trò: Admin

- Dữ liệu mẫu khởi tạo sẵn trong bộ nhớ (ShopDataStore):
  + 10 sản phẩm mẫu đa dạng loại hàng (Bánh kẹo, Đồ uống, Hóa mỹ phẩm, Hộp, Gia dụng)
  + 8 khách hàng thân thiết mẫu (VIP, Thân thiết, Thường)
========================================================================
