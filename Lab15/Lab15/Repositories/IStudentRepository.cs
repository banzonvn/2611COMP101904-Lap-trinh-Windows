using Lab15.Models;

namespace Lab15.Repositories;

/// <summary>
/// Giao diện tầng truy xuất dữ liệu (Data Access Layer / Repository Pattern)
/// cho đối tượng Sinh viên.
/// </summary>
public interface IStudentRepository
{
    IEnumerable<Student> GetAll();
    Student? GetById(string studentId);
    bool Exists(string studentId);
    void Add(Student student);
    bool Update(Student student);
    bool Delete(string studentId);
    IEnumerable<Student> Search(string keyword);
}
