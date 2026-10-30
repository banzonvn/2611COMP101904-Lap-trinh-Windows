using Lab15.Models;

namespace Lab15.Views;

/// <summary>
/// Giao diện View định nghĩa hợp đồng hiển thị và nhận tương tác cho module Quản lý Sinh viên.
/// Tuân thủ nguyên lý MVP: Presenter chỉ tương tác với View qua interface này.
/// </summary>
public interface IStudentView
{
    // === Dữ liệu nhập liệu từ người dùng ===
    string StudentId { get; set; }
    string FullName { get; set; }
    string ClassName { get; set; }
    string AverageScoreText { get; set; }
    string SearchKeyword { get; set; }

    // === Các phương thức cập nhật giao diện người dùng ===
    void ShowStudents(IEnumerable<Student> students);
    void ShowMessage(string message, bool isSuccess = true);
    void ClearInput();
    void SetInputFields(Student student);
    void UpdateStatistics(int totalCount, double avgScore, int passCount, int failCount);
}
