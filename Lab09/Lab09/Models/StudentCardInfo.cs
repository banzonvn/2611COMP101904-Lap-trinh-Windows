using System;
using System.Drawing;

namespace StudentCardPrinter.Models
{
    /// <summary>
    /// Lớp đóng gói dữ liệu của Thẻ Sinh Viên.
    /// Giúp tách biệt dữ liệu nghiệp vụ khỏi các điều khiển trên giao diện (Separation of Concerns),
    /// cho phép dùng chung một đối tượng dữ liệu cho cả preview trên Panel và in ấn bằng PrintDocument.
    /// </summary>
    public class StudentCardInfo
    {
        public string StudentId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string Faculty { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; } = new DateTime(2004, 1, 1);
        public string Gender { get; set; } = "Nam";
        public Image? Photo { get; set; }

        public StudentCardInfo Clone()
        {
            return new StudentCardInfo
            {
                StudentId = this.StudentId,
                FullName = this.FullName,
                ClassName = this.ClassName,
                Faculty = this.Faculty,
                Course = this.Course,
                BirthDate = this.BirthDate,
                Gender = this.Gender,
                Photo = this.Photo != null ? new Bitmap(this.Photo) : null
            };
        }
    }
}
