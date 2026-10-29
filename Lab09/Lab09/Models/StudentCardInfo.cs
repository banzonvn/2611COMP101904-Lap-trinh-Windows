using System;
using System.Drawing;

namespace StudentCardPrinter.Models
{
    /// <summary>
    /// Model lưu trữ toàn bộ dữ liệu của một thẻ sinh viên.
    /// Giúp tách biệt dữ liệu khỏi giao diện điều khiển (Separation of Concerns).
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
        public DateTime IssueDate { get; set; } = DateTime.Today;
        public DateTime ExpiryDate { get; set; } = DateTime.Today.AddYears(4);

        /// <summary>
        /// Kiểm tra tính hợp lệ của dữ liệu thẻ sinh viên theo yêu cầu đề bài.
        /// Bắt buộc: Mã sinh viên, Họ tên, Ảnh đại diện không được để trống.
        /// </summary>
        public bool Validate(out string errorField, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(StudentId))
            {
                errorField = nameof(StudentId);
                errorMessage = "Vui lòng nhập Mã số sinh viên (MSSV không được để trống).";
                return false;
            }

            if (string.IsNullOrWhiteSpace(FullName))
            {
                errorField = nameof(FullName);
                errorMessage = "Vui lòng nhập Họ và tên sinh viên (Họ tên không được để trống).";
                return false;
            }

            if (Photo == null)
            {
                errorField = nameof(Photo);
                errorMessage = "Vui lòng chọn ảnh đại diện sinh viên.";
                return false;
            }

            errorField = string.Empty;
            errorMessage = string.Empty;
            return true;
        }

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
                Photo = this.Photo != null ? new Bitmap(this.Photo) : null,
                IssueDate = this.IssueDate,
                ExpiryDate = this.ExpiryDate
            };
        }
    }
}
