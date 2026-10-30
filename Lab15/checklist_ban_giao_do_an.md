# BẢNG KIỂM TRA BÀN GIAO ĐỒ ÁN CUỐI KỲ (PROJECT DELIVERY CHECKLIST)

**Học phần:** Lập trình trên Windows (COMP1019)  
**Giảng viên:** ThS. Lê Thanh Thoại  
**Sinh viên thực hiện:** Lê Hoàng Quân - **MSSV:** 51.01.104.082 - **Lớp:** 51.CNTT.A - **Nhóm:** 04  

---

## 1. Hạng mục Kỹ thuật (Technical Deliverables)

| STT | Tiêu chí kiểm tra | Tình trạng | Chi tiết thực hiện / Ghi chú |
| :---: | :--- | :---: | :--- |
| 1 | **Source code biên dịch và chạy thành công** | [x] Đạt | Dự án `COMP1019_Buoi15_Lab15` biên dịch với 0 Error, 0 Warning trên .NET 10.0 SDK. Chạy độc lập hoặc qua lệnh `dotnet run`. |
| 2 | **Cơ sở dữ liệu / Bộ nhớ mẫu** | [x] Đạt | Repository tích hợp sẵn 12 sinh viên mẫu đại diện cho 3 lớp, có đầy đủ các dải điểm (Xuất sắc, Giỏi, Khá, Trung bình, Yếu) để kiểm thử ngay lập tức. |
| 3 | **Quản lý thư viện phụ thuộc (Dependencies)** | [x] Đạt | File cấu hình `Lab15.csproj` sạch sẽ, khai báo rõ ràng target `net10.0-windows`, không phát sinh lỗi thiếu NuGet package. |
| 4 | **Không hard-code đường dẫn tuyệt đối cá nhân** | [x] Đạt | Toàn bộ đường dẫn lưu file/ảnh, thư mục tài nguyên đều sử dụng `AppDomain.CurrentDomain.BaseDirectory` và relative path linh động. |
| 5 | **Tài khoản và thông tin demo có sẵn** | [x] Đạt | Có tài khoản demo `admin/admin123` và `user/user123` được ghi rõ trong `README.txt`. |
| 6 | **Kiểm tra ở chế độ Release** | [x] Đạt | Ứng dụng đã được build và chạy thử thành công ở chế độ `dotnet build -c Release` trước khi đóng gói nộp bài. |

---

## 2. Hạng mục Học thuật & Báo cáo (Academic & Presentation Deliverables)

| STT | Tiêu chí kiểm tra | Tình trạng | Chi tiết thực hiện / Ghi chú |
| :---: | :--- | :---: | :--- |
| 1 | **Bố cục báo cáo & Tài liệu README** | [x] Đạt | Tài liệu `README.md` trình bày chuyên nghiệp, phân mục logic, trực quan với bảng biểu, mã nguồn minh họa và ảnh chụp chi tiết. |
| 2 | **Kiến trúc phần mềm đúng chuẩn MVP** | [x] Đạt | Tách rời 100% trách nhiệm: `FrmStudent` (View) $\leftrightarrow$ `IStudentView` $\leftrightarrow$ `StudentPresenter` (Presenter) $\leftrightarrow$ `IStudentRepository` (Model/Data). Button click chỉ gọi 1 dòng phương thức Presenter. |
| 3 | **Đầy đủ test case và ảnh minh chứng** | [x] Đạt | Xây dựng 12 test case từ TC01 đến TC12 bao phủ kiểm tra luồng nạp, thêm hợp lệ, validation lỗi rỗng, trùng mã, lỗi điểm, sửa, xóa, tìm kiếm, làm mới. Kèm 12 ảnh chụp màn hình tương ứng. |
| 4 | **Trả lời 5 câu hỏi lý thuyết cốt lõi** | [x] Đạt | Đã giải thích chi tiết, khoa học toàn bộ 5 câu hỏi ôn tập cuối học phần từ Slide 36 của bài giảng. |
| 5 | **Kịch bản thuyết trình & demo 7 phút** | [x] Đạt | Chuẩn bị kịch bản thuyết trình mạch lạc theo đúng hướng dẫn của thầy: Giới thiệu bài toán $\rightarrow$ Đăng nhập $\rightarrow$ Nghiệp vụ chính $\rightarrow$ CRUD $\rightarrow$ Thống kê $\rightarrow$ Kiến trúc $\rightarrow$ Hạn chế & Hướng phát triển. |

---

## 3. Xác nhận bàn giao

- **Người lập checklist:** Lê Hoàng Quân
- **Trạng thái sẵn sàng nghiệm thu:** 100% SẴN SÀNG BÀN GIAO
