using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduManager.Models
{
    /// <summary>
    /// Thực thể Entity đại diện cho bảng dữ liệu MONHOC trong SQL Server.
    /// Đáp ứng tiêu chuẩn EF Core Code First / Database Mapping cho Lab 14.
    /// </summary>
    [Table("MONHOC")]
    public class MonHoc
    {
        [Key]
        [Column("MaMonHoc")]
        [StringLength(10)]
        public string MaMonHoc { get; set; } = string.Empty;

        [Required]
        [Column("TenMonHoc")]
        [StringLength(100)]
        public string TenMonHoc { get; set; } = string.Empty;

        [Column("SoTinChi")]
        [Range(1, 6)]
        public int SoTinChi { get; set; } = 3;

        [Column("TrangThai")]
        public bool TrangThai { get; set; } = true;

        /// <summary>
        /// Thuộc tính dẫn xuất phục vụ hiển thị trực quan trạng thái trên giao diện.
        /// </summary>
        [NotMapped]
        public string TinhTrangHienThi => TrangThai ? "Còn sử dụng" : "Ngừng sử dụng";

        public override string ToString()
        {
            return $"[{MaMonHoc}] {TenMonHoc} ({SoTinChi} TC - {TinhTrangHienThi})";
        }
    }
}
