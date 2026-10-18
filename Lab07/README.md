# Lab 07 – ShopManagerMini

Bài lab số 7 môn Lập trình Windows. Ứng dụng này xây dựng một cửa hàng mini dùng giao diện MDI (Multiple Document Interface) với nhiều Form lồng nhau trong C# WinForms.

---

## Ứng dụng gồm những gì

Khi chạy lên, đầu tiên sẽ hiện form đăng nhập. Nhập đúng tài khoản thì vào được màn hình chính (FrmMain). Từ đó có thể mở các màn hình quản lý sản phẩm hoặc khách hàng bên trong.

**Tài khoản thử:**
| Username | Password | Vai trò |
|---|---|---|
| admin | 123 | Admin |
| user | 123 | Nhân viên bán hàng |

---

## Các Form trong dự án

**FrmLogin** – Màn hình đăng nhập

Có 2 ô nhập username và password. Nhấn Enter ở ô username thì nhảy sang password, nhấn Enter ở ô password thì đăng nhập luôn. Nếu sai tài khoản hoặc sai mật khẩu thì hiện thông báo lỗi riêng biệt.

**FrmMain** – Màn hình chính (MDI Parent)

Đây là cửa sổ cha chứa tất cả màn hình con bên trong. Có menu ngang gồm:
- **Hệ thống**: Đăng xuất (F5), Thoát (Alt+F4)
- **Danh mục**: Quản lý sản phẩm (Ctrl+P), Quản lý khách hàng (Ctrl+K)  
- **Trợ giúp**: Giới thiệu

Thanh trạng thái bên dưới hiện tên người dùng, vai trò và đồng hồ cập nhật từng giây.

Nếu mở cùng loại form hai lần thì không tạo form mới mà chỉ kích hoạt cái đang mở lên.

**FrmProduct** – Quản lý sản phẩm

Chức năng đầy đủ thêm/sửa/xóa/tìm kiếm. Có validation kiểm tra mã không được trùng, giá phải lớn hơn 0, số lượng không được âm. Dùng ErrorProvider để hiện lỗi ngay cạnh ô nhập.

**FrmCustomer** – Quản lý khách hàng

Tương tự FrmProduct nhưng cho khách hàng. Có thêm/sửa/làm mới, DataGridView hiện danh sách. Khách hàng gồm các loại: Thường, Thân thiết, VIP.

**FrmAbout** – Giới thiệu

Hiện thông tin ứng dụng và môn học. Mở lên bằng menu Trợ giúp → Giới thiệu.

---

## Cấu trúc thư mục

```
Lab07/
└── Lab07/
    ├── Models/
    │   ├── User.cs          ← thông tin tài khoản người dùng
    │   ├── Product.cs       ← model sản phẩm
    │   └── Customer.cs      ← model khách hàng
    ├── Forms/
    │   ├── FrmLogin.cs / .Designer.cs
    │   ├── FrmMain.cs / .Designer.cs
    │   ├── FrmProduct.cs / .Designer.cs
    │   ├── FrmCustomer.cs / .Designer.cs
    │   └── FrmAbout.cs / .Designer.cs
    ├── Program.cs
    └── Lab07.csproj
```

---

## Yêu cầu để chạy

- Visual Studio 2022
- .NET 6.0 SDK trở lên
- Windows 10/11

Mở file `Lab07.slnx` rồi nhấn F5 là chạy được.

---

## Câu hỏi ôn tập (phần 10 trong đề)

**1. SDI và MDI khác nhau như thế nào?**

SDI (Single Document Interface) là mỗi cửa sổ mở độc lập, không có quan hệ cha-con. MDI (Multiple Document Interface) có một cửa sổ cha (Parent) chứa nhiều cửa sổ con (Child) bên trong. Trong bài này FrmMain là MDI Parent, còn FrmProduct và FrmCustomer là MDI Child.

**2. Vì sao Form đăng nhập nên dùng ShowDialog()?**

Vì `ShowDialog()` mở form ở dạng modal – tức là người dùng phải xử lý form đó xong mới được làm gì tiếp. Còn `Show()` thì không block, form mở song song và người dùng có thể bỏ qua. Với form đăng nhập thì cần chặn lại, phải xác thực xong mới mở FrmMain nên phải dùng `ShowDialog()`.

**3. Trong bài lab, dữ liệu user được truyền từ FrmLogin sang FrmMain bằng cách nào?**

Dùng property public. FrmLogin có property `LoggedInUser` kiểu `User`. Khi đăng nhập thành công thì gán user vào property đó rồi đóng form với `DialogResult.OK`. Bên Program.cs nhận về qua `loginForm.LoggedInUser` rồi truyền vào constructor của FrmMain.

**4. Nếu người dùng bấm menu sản phẩm nhiều lần, chương trình xử lý như thế nào để không mở trùng Form?**

Dùng vòng lặp kiểm tra `this.MdiChildren`. Trước khi tạo form mới, duyệt qua danh sách các form con đang mở, nếu thấy có form cùng kiểu (dùng `is T`) thì gọi `.Activate()` để kích hoạt form đó lên thay vì tạo thêm. Đây là pattern khá hay, tránh bị mở trùng gây rối.

**5. Khi nào nên dùng constructor, khi nào nên dùng property để truyền dữ liệu giữa các Form?**

Constructor thì dùng khi dữ liệu cần thiết ngay từ đầu để form khởi tạo đúng, ví dụ như truyền thông tin user vào FrmMain – cần biết user ngay khi load. Property thì linh hoạt hơn, có thể set sau khi form đã tạo rồi, phù hợp khi muốn nhận dữ liệu trả về từ form con như kiểu FrmLogin trả về `LoggedInUser`.

---

## Tiêu chí chấm điểm

| Tiêu chí | Điểm |
|---|---|
| Thiết kế đủ Form, giao diện rõ ràng, đặt tên control hợp lý | 2.0 |
| Đăng nhập đúng, truyền được thông tin user sang FrmMain | 2.0 |
| FrmMain dạng MDI, mở Form con đúng và không mở trùng | 2.0 |
| FrmProduct có CRUD trong bộ nhớ và DataGridView | 2.0 |
| Validation, MessageBox, xác nhận xóa/thoát | 1.0 |
| Code rõ ràng, có class Model, có comment cần thiết | 1.0 |
| **Tổng** | **10.0** |

---

*COMP1019 – Lập trình trên Windows | ThS. Lê Thanh Thoại*
