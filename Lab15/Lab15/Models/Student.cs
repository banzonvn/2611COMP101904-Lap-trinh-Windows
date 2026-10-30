namespace Lab15.Models;

/// <summary>
/// Entity biểu diễn thông tin sinh viên trong hệ thống quản lý.
/// </summary>
public class Student
{
    public string StudentId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public double AverageScore { get; set; }

    public Student() { }

    public Student(string studentId, string fullName, string className, double averageScore)
    {
        StudentId = studentId;
        FullName = fullName;
        ClassName = className;
        AverageScore = averageScore;
    }

    /// <summary>
    /// Đánh giá kết quả học tập (Đạt nếu >= 5.0).
    /// </summary>
    public string Result => AverageScore >= 5.0 ? "Đạt" : "Không đạt";

    /// <summary>
    /// Xếp loại học lực theo thang điểm chuẩn.
    /// </summary>
    public string Classification
    {
        get
        {
            if (AverageScore >= 9.0) return "Xuất sắc";
            if (AverageScore >= 8.0) return "Giỏi";
            if (AverageScore >= 6.5) return "Khá";
            if (AverageScore >= 5.0) return "Trung bình";
            return "Yếu";
        }
    }
}
