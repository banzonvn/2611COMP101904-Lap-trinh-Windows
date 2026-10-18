# 🛍️ Lab 06 – Quản lý sản phẩm (ProductManager)

> **COMP101904 – Lập trình Windows** | Khoa Công nghệ Thông tin – HCMUE  
> Chủ đề: MenuStrip, ToolStrip, StatusStrip, ContextMenuStrip, DataGridView, BindingSource, ErrorProvider

---

## 📋 Mục tiêu

- Thiết kế màn hình quản lý dữ liệu có bố cục rõ ràng
- Hiển thị danh sách sản phẩm bằng **DataGridView**
- Thực hiện **Thêm / Sửa / Xóa / Tìm kiếm** và làm mới dữ liệu trong bộ nhớ
- Sử dụng **ErrorProvider** để kiểm tra dữ liệu nhập
- Sử dụng **MenuStrip, ToolStrip, StatusStrip** và **ContextMenuStrip** để thao tác nhanh

---

## 🖥️ Giao diện ứng dụng

```
┌──────────────────────────────────────────────────────────────────┐
│  File   │  Quản lý  │  Trợ giúp                                  │ ← MenuStrip
├──────────────────────────────────────────────────────────────────┤
│  [Thêm] [Sửa] [Xóa] [Làm mới]  │  [Tìm kiếm ___________] [🔍]  │ ← ToolStrip
├──────────────────────────────────────────────────────────────────┤
│ ╔══ THÔNG TIN SẢN PHẨM ════════════════════════════════════════╗ │
│ ║  Mã SP:     [__________]   Đơn giá:   [___________]         ║ │
│ ║  Tên SP:    [__________]   Số lượng:  [___________]         ║ │
│ ║  Loại SP:   [▼ Điện tử]   Ngày nhập: [dd/MM/yyyy ]  [📷]   ║ │
│ ║  ☑ Còn kinh doanh          [Chọn ảnh...]                    ║ │
│ ╚══════════════════════════════════════════════════════════════╝ │
│                                                                  │
│  ┌─Mã SP──┬─Tên sản phẩm─────────┬─Loại──────┬─Đơn giá──┐      │
│  │ SP001  │ Laptop Lenovo ThinkPad│ Điện tử   │ 21,500,000│      │
│  │ SP002  │ Bút dạ quang Staedtler│ Văn phòng │    25,000 │      │
│  │ SP003  │ Bình đun siêu tốc     │ Gia dụng  │   890,000 │      │
│  │ SP004  │ Quần Jeans Casual Fit │ Thời trang│   550,000 │      │
│  └────────┴───────────────────────┴───────────┴───────────┘      │
├──────────────────────────────────────────────────────────────────┤
│  Sẵn sàng quản trị...    │ Tổng sản phẩm: 4 │ Giá trị kho: ...  │ ← StatusStrip
└──────────────────────────────────────────────────────────────────┘
```

---

## 🏗️ Kiến trúc dự án

```
Lab06/
├── Lab06.slnx               # Solution file
└── Lab06/
    ├── Product.cs            # Class dữ liệu sản phẩm
    ├── FrmProduct.cs         # Logic xử lý form chính
    ├── FrmProduct.Designer.cs# Thiết kế giao diện (auto-generated)
    ├── Program.cs            # Entry point
    └── Lab06.csproj          # Project configuration (.NET 6+)
```

---

## 📦 Class Product

| Thuộc tính | Kiểu | Mô tả | Ràng buộc |
|---|---|---|---|
| `MaSP` | `string` | Mã sản phẩm | Không rỗng, không trùng |
| `TenSP` | `string` | Tên sản phẩm | Không rỗng |
| `LoaiSP` | `string` | Loại sản phẩm | ComboBox: Điện tử / Văn phòng / Gia dụng / Thời trang |
| `DonGia` | `decimal` | Đơn giá | > 0 |
| `SoLuong` | `int` | Số lượng tồn kho | ≥ 0 |
| `NgayNhap` | `DateTime` | Ngày nhập kho | Không được sau ngày hôm nay |
| `ConKinhDoanh` | `bool` | Còn kinh doanh | Checkbox |
| `DuongDanAnh` | `string` | Đường dẫn ảnh | Có thể để trống |
| `ThanhTien` *(computed)* | `decimal` | `DonGia × SoLuong` | Tính tự động |

---

## ⚙️ Các chức năng chính

### 1. Thêm sản phẩm
- Kiểm tra dữ liệu hợp lệ qua `ValidateBusinessRules()`
- Kiểm tra **mã sản phẩm không trùng**
- Thêm vào `BindingList<Product>` → tự động cập nhật DataGridView

### 2. Sửa sản phẩm
- Chọn dòng → click **Sửa** lần 1 → dữ liệu nạp lên form
- Chỉnh sửa → click **Sửa** lần 2 → lưu thay đổi
- Mã sản phẩm bị khóa khi đang sửa

### 3. Xóa sản phẩm
- Yêu cầu **xác nhận** qua `MessageBox` trước khi xóa
- Xóa khỏi `BindingList` → Grid tự cập nhật

### 4. Tìm kiếm
- Tìm theo **Mã SP** hoặc **Tên SP** (không phân biệt chữ hoa/thường)
- Hỗ trợ nhấn `Enter` trong ô tìm kiếm

### 5. Làm mới
- Xóa toàn bộ dữ liệu nhập trên form
- Khôi phục hiển thị toàn bộ danh sách

### 6. Chọn ảnh
- Dùng `OpenFileDialog` với filter: JPG, PNG, BMP
- Hiển thị ảnh trong `PictureBox` (SizeMode: Zoom)

### 7. Xuất file .txt
- Menu **File → Xuất file .txt...** (Ctrl+E)
- Dùng `SaveFileDialog` → lưu báo cáo danh sách sản phẩm

### 8. ContextMenuStrip (chuột phải trên Grid)
| Menu | Chức năng |
|---|---|
| Sửa | Nạp dữ liệu lên form để chỉnh sửa |
| Xóa | Xác nhận và xóa sản phẩm |
| Xem chi tiết | Hiển thị thông tin đầy đủ qua MessageBox |

---

## 🔧 Kiểm tra dữ liệu (ErrorProvider)

| Trường | Điều kiện lỗi | Thông báo |
|---|---|---|
| Mã sản phẩm | Rỗng | "Mã sản phẩm không được rỗng." |
| Mã sản phẩm | Trùng | "Mã sản phẩm này đã tồn tại trong hệ thống." |
| Tên sản phẩm | Rỗng | "Tên sản phẩm không được rỗng." |
| Đơn giá | ≤ 0 | "Đơn giá phải lớn hơn 0 VNĐ." |
| Số lượng | < 0 | "Số lượng không được âm." |
| Ngày nhập | > Hôm nay | "Ngày nhập không được lớn hơn ngày hiện tại." |

---

## 🎯 Control đặt tên theo yêu cầu kỹ thuật

| Control | Tên | Loại |
|---|---|---|
| Ô nhập mã SP | `txtMaSP` | TextBox |
| Ô nhập tên SP | `txtTenSP` | TextBox |
| Dropdown loại | `cboLoaiSP` | ComboBox |
| Đơn giá | `numDonGia` | NumericUpDown |
| Số lượng | `numSoLuong` | NumericUpDown |
| Ngày nhập | `dtpNgayNhap` | DateTimePicker |
| Còn KD | `chkConKinhDoanh` | CheckBox |
| Danh sách SP | `dgvProducts` | DataGridView |
| Trạng thái | `lblStatus` | ToolStripStatusLabel |

---

## 🚀 Cách chạy dự án

### Yêu cầu
- **Visual Studio 2022** (Community trở lên)
- **.NET 6.0 SDK** trở lên
- Windows 10/11

### Bước thực hiện

```bash
# 1. Clone repository
git clone https://github.com/banzonvn/2611COMP101904-Lap-trinh-Windows.git

# 2. Mở thư mục Lab06
cd "2611COMP101904-Lap-trinh-Windows/Lab06/Lab06"

# 3. Mở file solution
start Lab06.slnx
```

Hoặc mở trực tiếp file `Lab06.slnx` bằng Visual Studio, rồi nhấn **F5** để chạy.

---

## 📊 Tiêu chí chấm điểm

| Tiêu chí | Mô tả | Điểm |
|---|---|---|
| **Giao diện** | MenuStrip, ToolStrip, StatusStrip, GroupBox nhập liệu, DataGridView | 2.0 |
| **Dữ liệu & Hiển thị** | Class Product, BindingSource/BindingList, DataGridView đúng cột | 2.0 |
| **CRUD** | Thêm, Sửa, Xóa, Tìm kiếm hoạt động đúng | 3.0 |
| **Validation** | Kiểm tra rỗng, mã trùng, số âm, ngày nhập; dùng ErrorProvider | 1.5 |
| **Thao tác nâng cao** | ContextMenuStrip, OpenFileDialog, phím tắt, keyboard/mouse | 1.0 |
| **Chất lượng code** | Đặt tên rõ ràng, tách hàm, code dễ đọc | 0.5 |
| **Tổng** | | **10.0** |

---

## 💡 Gợi ý mở rộng

- [ ] Sắp xếp theo đơn giá hoặc số lượng
- [ ] Tính tổng giá trị tồn kho và hiển thị trên StatusStrip ✅ *(đã có)*
- [ ] Xuất danh sách ra file `.txt` bằng `SaveFileDialog` ✅ *(đã có)*

---

## 👨‍💻 Thông tin

| | |
|---|---|
| **Môn học** | COMP101904 – Lập trình Windows |
| **Giảng viên** | ThS. Lê Thanh Thoại |
| **Trường** | Trường Đại học Sư phạm TP.HCM (HCMUE) |
| **Bài thực hành** | Lab 06 – Buổi 6 |
