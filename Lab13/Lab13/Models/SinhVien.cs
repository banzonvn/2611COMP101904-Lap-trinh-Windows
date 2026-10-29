namespace StudentReportDemo.Models
{
    /// <summary>
    /// Model thực thể SinhVien đại diện cho đối tượng sinh viên trong báo cáo.
    /// Đáp ứng chuẩn yêu cầu đề bài Lab 13 - COMP1019.
    /// </summary>
    public class SinhVien
    {
        public string MaSV { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string MaLop { get; set; } = string.Empty;
        public string TenLop { get; set; } = string.Empty;
        public double Diem { get; set; }

        /// <summary>
        /// Thuộc tính chỉ đọc (Read-only property) suy diễn logic từ điểm trung bình.
        /// Đảm bảo tính nhất quán của dữ liệu (Single Source of Truth).
        /// </summary>
        public string KetQua => Diem >= 5.0 ? "Đạt" : "Không đạt";

        /// <summary>
        /// Xếp loại học lực học thuật mở rộng (Sử dụng biểu thức switch C# hiện đại).
        /// </summary>
        public string XepLoai => Diem switch
        {
            >= 9.0 => "Xuất sắc",
            >= 8.0 => "Giỏi",
            >= 6.5 => "Khá",
            >= 5.0 => "Trung bình",
            _ => "Yếu"
        };

        public override string ToString()
        {
            return $"[{MaSV}] {HoTen} - {TenLop} (Điểm: {Diem:0.0} | {KetQua})";
        }
    }
}
