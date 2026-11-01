# COMP1019 - LẬP TRÌNH TRÊN WINDOWS

## BÀI LAB BUỔI 9: THIẾT KẾ VÀ IN THẺ SINH VIÊN BẰNG WINFORMS VÀ GDI+

- **Giảng viên hướng dẫn:** ThS. Lê Thanh Thoại - Khoa Công nghệ Thông tin, Trường Đại học Sư phạm TP. Hồ Chí Minh (HCMUE)
- **Sinh viên thực hiện:** Lê Hoàng Quân
- **Mã số sinh viên:** 51.01.104.082
- **Lớp:** 51.CNTT.A | **Nhóm:** 4
- **Môi trường phát triển:** .NET 10.0 / C# Windows Forms, Microsoft Visual Studio 2022/2026

---

## 1. Mục tiêu bài lab

1. Sử dụng thành thạo điều khiển `PictureBox` và hộp thoại `OpenFileDialog` để chọn và hiển thị ảnh chân dung sinh viên đúng tỉ lệ (chế độ Zoom), không bị méo.
2. Áp dụng kỹ thuật nạp ảnh an toàn (`LoadImageSafe`) bằng cách tạo bản sao Bitmap độc lập, giải phóng ảnh cũ bằng `Dispose()` và không khóa file ảnh gốc trên ổ đĩa.
3. Sử dụng `Graphics` và GDI+ trong sự kiện `Paint` của Panel để vẽ bản xem trước thẻ sinh viên trực quan, thời gian thực.
4. Tách biệt kiến trúc dữ liệu: Dùng chung một hàm vẽ duy nhất `StudentCardRenderer.Render(...)` cho cả xem trước trên Form, in ấn ra giấy qua `PrintDocument` và xuất file ảnh PNG, đảm bảo tính nhất quán 100%.
5. Sử dụng `PrintDocument`, `PrintPreviewDialog` và `PrintDialog` để xem trước và in ấn bản in thẻ chuẩn quy cách.
6. Xây dựng cơ chế kiểm tra tính hợp lệ của dữ liệu đầu vào (Validation) chặt chẽ và thực hiện đầy đủ tối thiểu 8 test case kiểm thử.

---

## 2. Điểm khác biệt & Kiến trúc Code nổi bật

Khác biệt hoàn toàn với cách viết thông thường (viết dồn toàn bộ mã vẽ lẫn lộn trong sự kiện Form, tọa độ cứng dễ gây đè chữ khi text dài), dự án được xây dựng với các tiêu chuẩn công nghệ cao cấp:

1. **Giải quyết triệt để lỗi đè chữ (No-Overlap Dynamic Layout Engine):**
   - Phân chia thẻ thành các phân vùng hình học độc lập: Dải Header (22%), Dải Footer (8%), Khung ảnh 3x4 (27%), Cột Nhãn (85px cố định) và Cột Giá trị riêng biệt.
   - Mỗi dòng thông tin (Mã số SV, Họ tên, Ngày sinh, Giới tính, Lớp, Khoa, Khóa học) được gán một hình chữ nhật bao quanh `RectangleF` độc lập với khoảng cách hàng 33px rộng rãi (trong khi chiều cao chữ chỉ ~16px), đảm bảo giữa các dòng luôn có 17px đệm an toàn.
   - Áp dụng `StringFormat` với cờ `NoWrap` và `Trimming = StringTrimming.EllipsisCharacter`. Nếu họ tên sinh viên hoặc tên khoa quá dài, hệ thống sẽ tự động thêm dấu ba chấm "..." ở cuối dòng một cách trang nhã, **tuyệt đối không bao giờ rớt dòng đè lên chữ khác hoặc đè vào mã QR Code**.

2. **Kiến trúc tách biệt nghiệp vụ (Separation of Concerns):**
   - `StudentCardInfo`: Model thuần túy chứa thông tin sinh viên, độc lập với giao diện.
   - `StudentCardRenderer`: Lớp tĩnh chuyên trách toàn bộ thuật toán đồ họa GDI+, tái sử dụng cho 3 mục đích (Preview, Print, Save PNG).
   - `CardTheme`: Áp dụng mẫu thiết kế Strategy/Theme cho phép người dùng chuyển đổi các bộ màu trường học cao cấp (HCMUE Classic, Emerald Tech, Ruby Graduate, Modern Slate) hoặc tự do đổi màu nền thẻ bằng `ColorDialog`.

3. **Tích hợp Multimedia & Timer theo đúng bài học:**
   - Phát âm thanh phản hồi `SystemSounds.Asterisk` khi mở Print Preview hoặc in thành công.
   - Đồng hồ thời gian thực cập nhật từng giây trên `StatusStrip` bằng `System.Windows.Forms.Timer`.

4. **Tối ưu hóa hiệu năng & Chống nhấp nháy (Zero-Flickering):**
   - Vùng vẽ sử dụng custom control `DoubleBufferedPanel` kế thừa từ `Panel` với cờ `DoubleBuffered = true`, `AllPaintingInWmPaint`, `OptimizedDoubleBuffer`, mang lại trải nghiệm xem trước mượt mà khi người dùng gõ phím.

---

## 3. Danh sách Điều khiển (Controls) trên Giao diện

| Nhóm chức năng | Loại Control | Tên Control (`Name`) | Chức năng / Mô tả chi tiết |
| :--- | :---: | :---: | :--- |
| **Thông tin SV** | `TextBox` | `txtStudentId` | Nhập mã số sinh viên (bắt buộc, không được để trống) |
| | `TextBox` | `txtFullName` | Nhập họ và tên sinh viên (bắt buộc) |
| | `ComboBox` | `cboClass` | Chọn lớp học từ danh sách mẫu (K50.CNTT.A, K49.CNTT.A, ...) |
| | `ComboBox` | `cboFaculty` | Chọn khoa đào tạo (Công nghệ thông tin, Toán - Tin học, ...) |
| | `TextBox` | `txtCourse` | Nhập khóa học / niên khóa (K50 (2024 - 2028)) |
| | `DateTimePicker` | `dtpBirthDate` | Chọn ngày tháng năm sinh (định dạng `dd/MM/yyyy`) |
| | `ComboBox` | `cboGender` | Chọn giới tính (Nam / Nữ / Khác) |
| **Ảnh đại diện** | `PictureBox` | `picAvatar` | Hiển thị ảnh chân dung sinh viên theo chế độ `Zoom` |
| | `Button` | `btnChoosePhoto` | Mở `OpenFileDialog` lọc file ảnh `.jpg`, `.png`, `.bmp` |
| | `Button` | `btnSampleData` | Nạp nhanh dữ liệu sinh viên mẫu phục vụ kiểm thử |
| | `Button` | `btnClear` | Làm mới form, xóa dữ liệu và giải phóng ảnh đại diện |
| **Tùy biến thẻ** | `ComboBox` | `cboTheme` | Chọn bộ màu thẻ sinh viên (HCMUE Classic, Emerald, Ruby, Slate) |
| | `Button` | `btnCustomBg` | Mở `ColorDialog` đổi màu nền thẻ tùy biến |
| | `CheckBox` | `chkShowQr` | Bật/tắt hiển thị mã QR Code sinh viên |
| | `CheckBox` | `chkShowChip` | Bật/tắt hiển thị vi mạch Chip bảo mật giả lập |
| | `Button` | `btnSaveStudent`| Lưu thẻ hiện tại vào danh sách bộ nhớ tạm |
| **Xem trước & In** | `DoubleBufferedPanel` | `pnlCardPreview` | Vùng vẽ xem trước thẻ sinh viên bằng GDI+ |
| | `Button` | `btnPreviewPrint` | Mở `PrintPreviewDialog` xem trước trang in khổ giấy |
| | `Button` | `btnPrint` | Mở `PrintDialog` chọn máy in và thực hiện in thẻ |
| | `Button` | `btnSavePng` | Mở `SaveFileDialog` xuất thẻ ra file ảnh PNG chất lượng cao |
| **Danh sách lưu tạm** | `DataGridView` | `dgvStudents` | Bảng quản lý danh sách sinh viên đã lưu trong phiên làm việc |
| **In ấn & Hệ thống** | `PrintDocument` | `printDocument1` | Định nghĩa trang in thẻ qua sự kiện `PrintPage` |
| | `PrintPreviewDialog` | `printPreviewDialog1` | Cửa sổ xem trước trang in |
| | `PrintDialog` | `printDialog1` | Hộp thoại chọn máy in |
| | `StatusStrip` | `statusStrip1` | Hiển thị thông báo trạng thái và đồng hồ thời gian thực |

---

## 4. Chi tiết các chức năng & Ảnh chụp màn hình minh họa

### 4.1. Khởi tạo & Giao diện tổng quan FrmStudentCard

Khi khởi động ứng dụng, Form chính mở ra ở trạng thái sẵn sàng với layout 2 cột rõ ràng: Cột trái gồm các nhóm nhập liệu thông tin, ảnh và tùy biến theme; Cột phải là vùng preview thẻ kích thước lớn cùng bảng danh sách sinh viên.

![Tổng quan giao diện](images/01_tong_quan_giao_dien.png)

_Mô tả: Giao diện tổng quan ứng dụng thiết kế và in thẻ sinh viên khi vừa khởi chạy._

---

### 4.2. Nhập thông tin sinh viên đầy đủ

Người dùng nhập đầy đủ thông tin: Mã sinh viên, Họ tên, chọn Lớp, Khoa, Khóa học, Ngày sinh từ DateTimePicker và Giới tính.

![Nhập thông tin sinh viên](images/02_nap_thong_tin_sinh_vien.png)

_Mô tả: Form sau khi nhập đầy đủ thông tin sinh viên hợp lệ._

---

### 4.3. Chọn ảnh đại diện qua OpenFileDialog (Chế độ Zoom không méo)

Bấm nút **Chọn ảnh...** để mở `OpenFileDialog` với bộ lọc chỉ cho phép chọn các định dạng ảnh `.jpg`, `.jpeg`, `.png`, `.bmp`. Ảnh sau khi chọn được hiển thị vừa vặn trong PictureBox theo chế độ `Zoom`, giữ nguyên tỉ lệ khuôn mặt sinh viên.

![Chọn ảnh đại diện](images/03_chon_anh_dai_dien_zoom.png)

_Mô tả: Ảnh chân dung sinh viên được tải và hiển thị chuẩn tỉ lệ 3x4 trong PictureBox._

---

### 4.4. Xem trước thẻ sinh viên thời gian thực trên Panel bằng GDI+

Ngay khi người dùng gõ ký tự hoặc chọn giá trị mới, sự kiện `Paint` của Panel tự động vẽ lại thẻ sinh viên bằng GDI+ sắc nét, các dòng chữ thẳng hàng, khoảng cách đều đặn và không bị đè chữ.

![Xem trước thẻ GDI+](images/04_preview_the_gdi_plus.png)

_Mô tả: Thẻ sinh viên được kết xuất trực tiếp trên Panel xem trước bằng thư viện GDI+._

---

### 4.5. Kiểm tra dữ liệu bắt buộc (Validation Warning)

Khi người dùng nhấn nút xem trước in hoặc in thẻ mà chưa nhập Mã sinh viên, Họ tên hoặc chưa chọn ảnh đại diện, hệ thống sẽ ngăn chặn thao tác và hiển thị hộp thoại cảnh báo `MessageBox`, đồng thời tự động đưa con trỏ chuột (Focus) đến ô nhập còn thiếu.

![Thông báo Validation](images/05_thong_bao_validation_thieu_du_lieu.png)

_Mô tả: Hệ thống cảnh báo người dùng bổ sung các thông tin bắt buộc trước khi in ấn._

---

### 4.6. Xem trước bản in bằng PrintPreviewDialog

Bấm nút **Xem trước khi in (Print Preview)** kích hoạt cửa sổ chuẩn của Windows `PrintPreviewDialog`. Người dùng có thể phóng to, thu nhỏ và kiểm tra bố cục thẻ sinh viên trên trang in trước khi in vật lý.

![Xem trước bản in](images/06_xem_truoc_in_print_preview.png)

_Mô tả: Cửa sổ PrintPreviewDialog hiển thị trang in thẻ sinh viên ở kích thước chuẩn._

---

### 4.7. Tùy biến bộ màu phong cách thẻ (Card Themes)

Người dùng có thể chọn các bộ theme thiết kế chuyên nghiệp từ ComboBox (như Emerald Tech xanh ngọc, Ruby Graduate đỏ đô, Modern Slate xám titan). Màu nền, màu dải header, màu khung viền và màu chữ sẽ tự động phối màu hài hòa.

![Tùy biến Theme](images/07_tuy_bien_theme_mau_the.png)

_Mô tả: Thẻ sinh viên được chuyển đổi sang phong cách màu Emerald Tech hiện đại._

---

### 4.8. Đổi màu nền thẻ tùy ý bằng ColorDialog

Nút **Đổi màu nền thẻ...** mở hộp thoại `ColorDialog` cho phép người dùng tùy chọn bất kỳ gam màu yêu thích nào cho phần thân thẻ (ví dụ màu vàng kem sang trọng, màu xanh pastel...).

![Đổi màu nền thẻ](images/08_doi_mau_nen_the_tuy_y.png)

_Mô tả: Màu thân thẻ được chuyển sang màu vàng kem tùy biến theo sở thích người dùng._

---

### 4.9. Tùy chọn hiển thị Mã QR Code và Vi mạch Chip bảo mật

Người dùng có thể bật hoặc tắt checkbox **Mã QR Code** và **Vi mạch Chip**. GDI+ sẽ linh hoạt căn chỉnh lại chiều rộng của các dòng thông tin sinh viên mà không làm lệch lạc bố cục tổng thể.

![Bật tắt QR và Chip](images/09_bat_tat_qr_va_chip.png)

_Mô tả: Thẻ sinh viên với mã QR Code ma trận và vi mạch Chip thông minh bảo mật._

---

### 4.10. Xuất thẻ sinh viên thành file ảnh PNG chất lượng cao

Nút **Xuất file ảnh PNG** mở `SaveFileDialog` cho phép kết xuất thẻ sinh viên thành file ảnh chất lượng cao 560x350 pixel với khử răng cưa `SmoothingMode.AntiAlias` để gửi qua email hoặc in ấn điện tử.

![Xuất file ảnh PNG](images/10_xuat_anh_the_png.png)

_Mô tả: Thông báo xuất thẻ thành công ra file ảnh PNG chất lượng cao._

---

### 4.11. Quản lý danh sách thẻ đã tạo bằng DataGridView

Tính năng mở rộng: Bấm nút **Lưu thẻ vào danh sách** sẽ nạp đối tượng `StudentCardInfo` vào `BindingList<T>` hiển thị trên DataGridView. Khi click chọn sinh viên bất kỳ trên bảng, thông tin và thẻ tương ứng sẽ tự động hiển thị lại.

![Lưu danh sách sinh viên](images/11_luu_danh_sach_sinh_vien.png)

_Mô tả: Danh sách sinh viên đã lưu hiển thị trực quan trên DataGridView._

---

### 4.12. In thẻ sinh viên bằng PrintDialog & PrintDocument

Khi bấm nút **In thẻ sinh viên**, hộp thoại hệ thống `PrintDialog` xuất hiện cho phép lựa chọn máy in (máy in thẻ chuyên dụng, máy in văn phòng, hoặc máy in ảo Microsoft Print to PDF). Thao tác in thành công kèm theo âm thanh thông báo.

![Hộp thoại in PrintDialog](images/12_in_the_print_dialog.png)

_Mô tả: Hộp thoại PrintDialog lựa chọn máy in để xuất bản in thẻ sinh viên._

---

### 4.13. Làm mới toàn bộ Form về trạng thái ban đầu (Clear All)

Nút **Làm mới (Clear)** xóa sạch các ô nhập văn bản, đưa các ComboBox về giá trị mặc định, giải phóng ảnh đại diện cũ khỏi bộ nhớ RAM và đặt lại vùng xem trước thẻ về trạng thái khởi tạo.

![Làm mới Form](images/13_lam_moi_form_clear.png)

_Mô tả: Giao diện sau khi bấm Làm mới Form, sẵn sàng cho sinh viên tiếp theo._

---

## 5. Danh mục 8 Test Case Kiểm thử thực tế

Dưới đây là 8 trường hợp kiểm thử thực tế (Test Cases) theo đúng yêu cầu trong đề bài và thang điểm chấm của Giảng viên:

### 5.1. Test Case 1: Kiểm tra bỏ trống Mã số sinh viên

- **Mục tiêu:** Kiểm tra cơ chế validation khi người dùng không nhập mã số sinh viên.
- **Thao tác:** Bỏ trống ô `txtStudentId`, nhập họ tên và ảnh, sau đó bấm *Xem trước khi in*.
- **Kết quả mong đợi:** Xuất hiện cảnh báo *"Vui lòng nhập Mã số sinh viên (bắt buộc)"*, không mở Print Preview và con trỏ tự động Focus vào ô `txtStudentId`.

![Test Case 1](images/14_testcase1_thieu_mssv.png)

_Mô tả: Test Case 1 - Cảnh báo và tự động Focus vào ô Mã sinh viên khi bị bỏ trống._

---

### 5.2. Test Case 2: Kiểm tra bỏ trống Họ và tên sinh viên

- **Mục tiêu:** Kiểm tra ràng buộc bắt buộc đối với trường họ tên sinh viên.
- **Thao tác:** Nhập mã số sinh viên hợp lệ nhưng xóa trắng ô `txtFullName`, sau đó bấm *In thẻ sinh viên*.
- **Kết quả mong đợi:** Xuất hiện cảnh báo *"Vui lòng nhập Họ và tên sinh viên (bắt buộc)"*, chặn lệnh in và Focus vào `txtFullName`.

![Test Case 2](images/15_testcase2_thieu_hoten.png)

_Mô tả: Test Case 2 - Bắt lỗi bỏ trống Họ tên sinh viên và đưa con trỏ về ô nhập liệu._

---

### 5.3. Test Case 3: Kiểm tra chưa chọn ảnh đại diện khi in

- **Mục tiêu:** Đảm bảo thẻ in ra không bị thiếu ảnh chân dung nhận diện.
- **Thao tác:** Nhập đầy đủ mã sinh viên và họ tên nhưng chưa bấm chọn ảnh (ảnh null), sau đó bấm *Xem trước khi in*.
- **Kết quả mong đợi:** Xuất hiện cảnh báo yêu cầu chọn ảnh chân dung sinh viên, vùng preview hiển thị khung placeholder nhắc nhở.

![Test Case 3](images/16_testcase3_thieu_anh_dai_dien.png)

_Mô tả: Test Case 3 - Cảnh báo thiếu ảnh đại diện và khung placeholder ảnh trên thẻ._

---

### 5.4. Test Case 4: Kiểm tra chọn file không phải định dạng ảnh

- **Mục tiêu:** Kiểm tra tính an toàn (Fault Tolerance) khi người dùng cố tình chọn file văn bản (.txt, .docx, .exe) hoặc file ảnh hỏng.
- **Thao tác:** Chọn một file text hoặc file không phải ảnh qua `OpenFileDialog`.
- **Kết quả mong đợi:** Khối `try-catch` trong hàm `LoadImageSafe` bắt lỗi ngoại lệ, thông báo *"File được chọn không phải là ảnh hợp lệ hoặc bị lỗi"*, chương trình không bị crash hay tắt đột ngột.

![Test Case 4](images/17_testcase4_file_anh_khong_hop_le.png)

_Mô tả: Test Case 4 - Bắt ngoại lệ và hiển thị thông báo an toàn khi file nạp vào không phải ảnh._

---

### 5.5. Test Case 5: Kiểm tra nạp và đổi ảnh nhiều lần không khóa file gốc

- **Mục tiêu:** Kiểm tra cơ chế giải phóng tài nguyên đồ họa và tránh lỗi Windows File Lock.
- **Thao tác:** Chọn một ảnh, sau đó đổi sang ảnh khác nhiều lần liên tục và thử xóa/đổi tên file ảnh gốc trên máy tính.
- **Kết quả mong đợi:** Ảnh cũ được tự động `Dispose()`, ảnh mới được tạo qua bản sao `new Bitmap(temp)`. File gốc trên ổ đĩa không bị tiến trình ứng dụng chiếm dụng/khóa.

![Test Case 5](images/18_testcase5_thay_doi_anh_lien_tuc.png)

_Mô tả: Test Case 5 - Đổi ảnh đại diện nhiều lần mượt mà, quản lý RAM chặt chẽ._

---

### 5.6. Test Case 6: Kiểm tra nhập họ tên và khoa rất dài (Không đè chữ)

- **Mục tiêu:** Kiểm tra khả năng tự động xử lý chuỗi văn bản dài bằng GDI+, đảm bảo không bao giờ đè chữ lên nhau hoặc tràn ra khỏi thẻ.
- **Thao tác:** Nhập họ tên cực dài (*Nguyễn Hoàng Công Tằng Tôn Nữ Bích Thủy*) và khoa đào tạo dài (*Khoa Công Nghệ Thông Tin & Truyền Thông Đa Phương Tiện*).
- **Kết quả mong đợi:** Thuật toán căn lề động kết hợp cờ `StringTrimming.EllipsisCharacter` tự động cắt gọn chuỗi bằng dấu "..." trước khi chạm vào QR Code hoặc dòng kế tiếp; **các dòng thông tin hoàn toàn tách biệt, không hề bị đè chữ**.

![Test Case 6](images/19_testcase6_ho_ten_dai_khong_de_chu.png)

_Mô tả: Test Case 6 - Họ tên và Khoa rất dài được căn chỉnh hoàn hảo, chữ không đè lên nhau._

---

### 5.7. Test Case 7: Kiểm tra nút Làm mới (Clear dữ liệu & hủy ảnh trong RAM)

- **Mục tiêu:** Đảm bảo Form được đưa về trạng thái sạch sẽ hoàn toàn sau khi làm mới.
- **Thao tác:** Nhấn nút *Làm mới (Clear)* khi đang có đầy đủ dữ liệu và ảnh đại diện.
- **Kết quả mong đợi:** Toàn bộ textboxes được làm rỗng, comboboxes trở về mục đầu tiên, `picAvatar.Image` được giải phóng khỏi RAM (`Dispose`), vùng preview vẽ lại thẻ trống và focus đặt tại ô MSSV.

![Test Case 7](images/20_testcase7_chuc_nang_clear_form.png)

_Mô tả: Test Case 7 - Thao tác Clear dọn dẹp sạch sẽ toàn bộ trạng thái Form và giải phóng bộ nhớ._

---

### 5.8. Test Case 8: Kiểm tra xuất bản in ra máy in ảo Microsoft Print to PDF

- **Mục tiêu:** Kiểm tra khả năng kết xuất thẻ sinh viên thành tài liệu PDF chuẩn in ấn.
- **Thao tác:** Bấm nút *In thẻ sinh viên*, chọn máy in ảo *Microsoft Print to PDF* và lưu file PDF ra ổ đĩa.
- **Kết quả mong đợi:** File PDF được tạo thành công với kích thước thẻ CR80 sắc nét, ảnh chân dung không méo và màu sắc đồng nhất với bản xem trước.

![Test Case 8](images/21_testcase8_in_ra_microsoft_print_to_pdf.png)

_Mô tả: Test Case 8 - Xuất bản in thẻ sinh viên chất lượng cao ra máy in ảo PDF._

---

## 6. Cấu trúc thư mục mã nguồn

```
Lab09/
├── README.md                           ← Báo cáo tổng quan Lab 09
├── images/                             ← Chứa 21 ảnh chụp màn hình chức năng và test cases
│   ├── 01_tong_quan_giao_dien.png
│   ├── 02_nap_thong_tin_sinh_vien.png
│   ├── ...
│   ├── 14_testcase1_thieu_mssv.png
│   ├── ...
│   ├── 21_testcase8_in_ra_microsoft_print_to_pdf.png
│   └── card_export_sample.png
└── Lab09/
    ├── Models/
    │   ├── StudentCardInfo.cs          ← Model dữ liệu sinh viên
    │   └── CardTheme.cs                ← Cấu hình bộ màu và phong cách thẻ
    ├── Services/
    │   └── StudentCardRenderer.cs      ← Bộ vẽ GDI+ căn lề động, chống đè chữ
    ├── Forms/
    │   ├── FrmStudentCard.cs           ← Form chính: Điều phối sự kiện, preview & in ấn
    │   └── FrmStudentCard.Designer.cs  ← Thiết kế giao diện WinForms
    ├── Program.cs                      ← Điểm khởi chạy ứng dụng
    ├── Lab09.csproj                    ← Cấu hình project .NET 10.0 Windows Forms
    ├── Lab09.slnx                      ← File giải pháp hiện đại
    ├── images/                         ← Thư mục ảnh chụp màn hình trong project
    └── README.md                       ← Báo cáo chi tiết theo chuẩn môn học
```

---

*COMP1019 - Lập trình trên Windows | ThS. Lê Thanh Thoại - HCMUE*
