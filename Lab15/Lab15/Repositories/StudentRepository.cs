using Lab15.Models;

namespace Lab15.Repositories;

/// <summary>
/// Cài đặt Repository lưu trữ dữ liệu sinh viên trong bộ nhớ (In-memory storage)
/// phục vụ kiểm thử đơn vị, demo và bàn giao đồ án.
/// </summary>
public class StudentRepository : IStudentRepository
{
    private readonly List<Student> _students;

    public StudentRepository()
    {
        // Khởi tạo bộ dữ liệu mẫu ban đầu để giảng viên kiểm tra ngay khi khởi chạy
        _students = new List<Student>
        {
            new Student("51.01.104.082", "Lê Hoàng Quân", "51.CNTT.A", 9.8),
            new Student("51.01.104.001", "Nguyễn Văn An", "51.CNTT.A", 8.5),
            new Student("51.01.104.015", "Trần Thị Mai", "51.CNTT.A", 7.6),
            new Student("51.01.104.022", "Phạm Đức Minh", "51.CNTT.A", 4.5),
            new Student("51.01.104.035", "Lê Thị Hương Giang", "51.CNTT.A", 8.9),
            new Student("47.01.104.012", "Đặng Hoàng Nam", "CTK47A", 6.8),
            new Student("47.01.104.019", "Vũ Quỳnh Nga", "CTK47A", 9.2),
            new Student("47.01.104.045", "Bùi Trọng Khang", "CTK47A", 5.4),
            new Student("47.01.104.067", "Hoàng Lan Chi", "CTK47B", 7.9),
            new Student("47.01.104.088", "Ngô Kiến Huy", "CTK47B", 3.8),
            new Student("47.01.104.099", "Đỗ Kim Ngân", "CTK47B", 8.2),
            new Student("51.01.104.105", "Đinh Tuấn Kiệt", "51.CNTT.A", 6.5)
        };
    }

    public IEnumerable<Student> GetAll()
    {
        // Trả về bản sao danh sách để bảo toàn tính toàn vẹn dữ liệu
        return _students.Select(s => new Student(s.StudentId, s.FullName, s.ClassName, s.AverageScore)).ToList();
    }

    public Student? GetById(string studentId)
    {
        return _students.FirstOrDefault(s => s.StudentId.Equals(studentId.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    public bool Exists(string studentId)
    {
        return _students.Any(s => s.StudentId.Equals(studentId.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    public void Add(Student student)
    {
        _students.Add(new Student(student.StudentId.Trim(), student.FullName.Trim(), student.ClassName.Trim(), student.AverageScore));
    }

    public bool Update(Student student)
    {
        var existing = GetById(student.StudentId);
        if (existing == null) return false;

        existing.FullName = student.FullName.Trim();
        existing.ClassName = student.ClassName.Trim();
        existing.AverageScore = student.AverageScore;
        return true;
    }

    public bool Delete(string studentId)
    {
        var existing = GetById(studentId);
        if (existing == null) return false;

        return _students.Remove(existing);
    }

    public IEnumerable<Student> Search(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return GetAll();
        }

        string kw = keyword.Trim();
        return _students.Where(s =>
            s.StudentId.Contains(kw, StringComparison.OrdinalIgnoreCase) ||
            s.FullName.Contains(kw, StringComparison.OrdinalIgnoreCase) ||
            s.ClassName.Contains(kw, StringComparison.OrdinalIgnoreCase)
        ).Select(s => new Student(s.StudentId, s.FullName, s.ClassName, s.AverageScore)).ToList();
    }
}
