# COMP1019 - LẬP TRÌNH TRÊN WINDOWS

## BÀI TẬP: QUẢN LÝ VÀ TÍNH LƯƠNG NHÂN VIÊN (OOP C# CONSOLE)

- **Sinh viên thực hiện:** Lê Hoàng Quân
- **Mã số sinh viên:** 51.01.104.082
- **Lớp:** 51.CNTT.A
- **Môi trường:** .NET / C# Console App, Microsoft Visual Studio

---

## 1. Mục tiêu bài thực hành

- Vận dụng các đặc trưng cốt lõi của **Lập trình hướng đối tượng (OOP)** trong C#:
  - **Đóng gói (Encapsulation):** Bảo vệ dữ liệu thông qua các thuộc tính (Properties).
  - **Kế thừa (Inheritance):** Xây dựng lớp cơ sở trừu tượng `NhanVien` và các lớp dẫn xuất cho từng loại nhân viên.
  - **Đa hình (Polymorphism):** Override phương thức tính lương `TinhLuong()` và xuất thông tin `XuatThongTin()` linh hoạt theo từng loại hình nhân viên.
  - **Trừu tượng (Abstraction):** Thiết kế `abstract class` định nghĩa khung mẫu nghiệp vụ chung.
- Quản lý tập hợp đối tượng đa hình bằng danh sách động `List<NhanVien>`.
- Định dạng xuất dữ liệu dạng bảng biểu chuyên nghiệp trên Console.
- Kiểm tra tính hợp lệ của dữ liệu đầu vào và xử lý ngoại lệ chống dừng chương trình đột ngột.

---

## 2. Kiến trúc các lớp (Class Design) & Công thức tính lương

### 2.1. Lớp cơ sở trừu tượng: `NhanVien` (abstract)

- **Thuộc tính:** `MaNV`, `HoTen`, `LuongCoBan`
- **Phương thức trừu tượng:**
  - `public abstract decimal TinhLuong();`
  - `public abstract void XuatThongTin();`

### 2.2. Các lớp con kế thừa

1. **`NhanVienVanPhong` (Nhân viên văn phòng):**
   - Thuộc tính bổ sung: `SoNgayLamViec`
   - Công thức tính lương:
     $$\text{Thực lĩnh} = \text{Lương cơ bản} + (\text{Số ngày làm việc} \times 200.000\,\text{đ})$$

2. **`NhanVienKinhDoanh` (Nhân viên kinh doanh):**
   - Thuộc tính bổ sung: `DoanhSo`
   - Công thức tính lương:
     $$\text{Thực lĩnh} = \text{Lương cơ bản} + (\text{Doanh số} \times 5\%)$$

3. **`NhanVienThoiVu` (Nhân viên thời vụ):**
   - Thuộc tính bổ sung: `SoGioLamViec`, `DonGiaGio` ($50.000\,\text{đ/giờ}$)
   - Công thức tính lương:
     $$\text{Thực lĩnh} = \text{Số giờ làm việc} \times 50.000\,\text{đ}$$

---

## 3. Danh mục chức năng chương trình

|              Chức năng              | Mô tả chi tiết                                                                                                                           |
| :---------------------------------: | :--------------------------------------------------------------------------------------------------------------------------------------- |
|   **1. Xuất danh sách nhân viên**   | Hiển thị toàn bộ nhân viên dưới dạng bảng có phân loại: Văn Phòng, Kinh Doanh, Thời Vụ, kèm Lương cơ bản và Thực lĩnh đã format tiền tệ. |
|    **2. Tìm nhân viên theo mã**     | Cho phép nhập mã nhân viên (không phân biệt hoa/thường). Thông báo chi tiết nếu tìm thấy hoặc cảnh báo nếu không tồn tại.                |
| **3. Tìm nhân viên lương cao nhất** | Quét danh sách và in ra thông tin nhân viên có mức thực lĩnh cao nhất công ty.                                                           |
|     **4. Tính tổng quỹ lương**      | Cộng dồn thực lĩnh của toàn bộ nhân viên công ty phải chi trả trong tháng.                                                               |
|            **0. Thoát**             | Đóng ứng dụng an toàn kèm thông báo kết thúc.                                                                                            |

---

## 4. Kết quả thực nghiệm & Minh chứng chức năng

> _Ghi chú: Toàn bộ ảnh chụp minh chứng được lưu trong thư mục `images/`._

### 4.1. Menu chính của chương trình

Menu hiển thị rõ ràng 4 chức năng nghiệp vụ và tùy chọn thoát (0).

![Menu chính](images/01_menu.png)

_Mô tả: Giao diện Menu quản lý nhân viên._

---

### 4.2. Chức năng 1: Xuất danh sách nhân viên

Xuất danh sách nhân viên định dạng bảng ngay ngắn, rõ ràng từng cột loại nhân viên, thông tin làm việc và thực lĩnh.

![Xuất danh sách nhân viên](images/02_xuat_danh_sach.png)

_Mô tả: Bảng tổng hợp lương nhân viên đầy đủ các bộ phận._

---

### 4.3. Chức năng 2: Tìm nhân viên theo mã

- **Trường hợp 1: Tìm thấy nhân viên**
  Nhập mã `VP02`, hệ thống tìm và hiển thị chính xác thông tin nhân viên Lê Thị B (Văn Phòng - 26 ngày công).

  ![Tìm thấy nhân viên](images/03_tim_kiem_thanh_cong.png)

  _Mô tả: Tìm thấy nhân viên có mã VP02._

- **Trường hợp 2: Không tìm thấy nhân viên**
  Nhập mã `1`, hệ thống đưa ra thông báo rõ ràng: `Không tìm thấy nhân viên có mã: 1`.

  ![Không tìm thấy nhân viên](images/04_tim_kiem_that_bai.png)

  _Mô tả: Thông báo khi mã nhân viên không tồn tại._

---

### 4.4. Chức năng 3: Tìm nhân viên có lương cao nhất

Tìm ra nhân viên có mức lương thực lĩnh cao nhất là Nguyễn Văn C (Kinh Doanh - KD01) với mức lương $12.500.000\,\text{đ}$.

![Lương cao nhất](images/05_luong_cao_nhat.png)

_Mô tả: Nhân viên có thu nhập cao nhất công ty._

---

### 4.5. Chức năng 4: Tính tổng lương công ty phải trả

Tính tổng lương thực lĩnh của toàn bộ 5 nhân viên:
$$10.400.000 + 10.700.000 + 12.500.000 + 8.500.000 + 6.000.000 = 48.100.000\,\text{VNĐ}$$

![Tổng quỹ lương](images/06_tong_quy_luong.png)

_Mô tả: Tổng quỹ lương công ty phải chi trả là 48.100.000 VNĐ._

---

### 4.6. Kiểm tra lỗi nhập liệu Menu

Khi nhập số nằm ngoài phạm vi $0 - 4$ (ví dụ nhập `5`), chương trình xuất thông báo cảnh báo `Lựa chọn không hợp lệ!` và cho phép người dùng tiếp tục thao tác.

![Lỗi chọn Menu](images/07_loi_menu.png)

_Mô tả: Bắt lỗi khi người dùng chọn chức năng không hợp lệ._

---

### 4.7. Chức năng 0: Thoát chương trình

Kết thúc chương trình an toàn kèm thông báo xác nhận đã thoát.

![Thoát](images/08_thoat.png)

_Mô tả: Thoát ứng dụng thành công._

---
