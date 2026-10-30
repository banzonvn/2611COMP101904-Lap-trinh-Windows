using System.Globalization;
using Lab15.Models;
using Lab15.Repositories;
using Lab15.Views;

namespace Lab15.Presenters;

/// <summary>
/// Presenter đóng vai trò điều phối logic nghiệp vụ trong mô hình MVP.
/// Nhận thao tác từ Form (View), xử lý dữ liệu qua Repository, và cập nhật kết quả lên View.
/// Không truy cập trực tiếp vào các UI controls (TextBox, DataGridView).
/// </summary>
public class StudentPresenter
{
    private readonly IStudentView _view;
    private readonly IStudentRepository _repo;

    public StudentPresenter(IStudentView view, IStudentRepository repo)
    {
        _view = view;
        _repo = repo;
    }

    /// <summary>
    /// Tải toàn bộ danh sách sinh viên lên View và cập nhật thống kê.
    /// </summary>
    public void LoadStudents()
    {
        var students = _repo.GetAll().ToList();
        _view.ShowStudents(students);
        RefreshStatistics(students);
        _view.ShowMessage($"Đã tải thành công {students.Count} sinh viên vào hệ thống.", true);
    }

    /// <summary>
    /// Xử lý nghiệp vụ Thêm sinh viên mới kèm kiểm tra tính hợp lệ dữ liệu.
    /// </summary>
    public void AddStudent()
    {
        string id = _view.StudentId.Trim();
        string name = _view.FullName.Trim();
        string className = string.IsNullOrWhiteSpace(_view.ClassName) ? "51.CNTT.A" : _view.ClassName.Trim();
        string scoreText = _view.AverageScoreText.Trim();

        // 1. Kiểm tra dữ liệu rỗng
        if (string.IsNullOrWhiteSpace(id))
        {
            _view.ShowMessage("Lỗi nhập liệu: Mã sinh viên không được để trống!", false);
            return;
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            _view.ShowMessage("Lỗi nhập liệu: Họ và tên sinh viên không được để trống!", false);
            return;
        }

        // 2. Kiểm tra định dạng điểm số (chấp nhận cả dấu chấm và dấu phẩy)
        string normalizedScore = scoreText.Replace(',', '.');
        if (!double.TryParse(normalizedScore, NumberStyles.Float, CultureInfo.InvariantCulture, out double score) || score < 0.0 || score > 10.0)
        {
            _view.ShowMessage("Lỗi nhập liệu: Điểm trung bình phải là số thực từ 0.0 đến 10.0!", false);
            return;
        }

        // 3. Kiểm tra trùng mã sinh viên
        if (_repo.Exists(id))
        {
            _view.ShowMessage($"Lỗi ràng buộc: Mã sinh viên '{id}' đã tồn tại trong hệ thống!", false);
            return;
        }

        // 4. Thực hiện thêm vào kho dữ liệu
        var student = new Student(id, name, className, score);
        _repo.Add(student);

        // 5. Cập nhật giao diện View
        var allStudents = _repo.GetAll().ToList();
        _view.ShowStudents(allStudents);
        RefreshStatistics(allStudents);
        _view.ClearInput();
        _view.ShowMessage($"Thêm thành công sinh viên: {student.FullName} (Mã: {student.StudentId})", true);
    }

    /// <summary>
    /// Xử lý nghiệp vụ Cập nhật thông tin sinh viên hiện có.
    /// </summary>
    public void UpdateStudent()
    {
        string id = _view.StudentId.Trim();
        string name = _view.FullName.Trim();
        string className = string.IsNullOrWhiteSpace(_view.ClassName) ? "51.CNTT.A" : _view.ClassName.Trim();
        string scoreText = _view.AverageScoreText.Trim();

        if (string.IsNullOrWhiteSpace(id))
        {
            _view.ShowMessage("Vui lòng chọn hoặc nhập mã sinh viên cần cập nhật!", false);
            return;
        }

        if (!_repo.Exists(id))
        {
            _view.ShowMessage($"Không tìm thấy sinh viên có mã '{id}' để cập nhật!", false);
            return;
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            _view.ShowMessage("Lỗi nhập liệu: Họ và tên sinh viên không được để trống!", false);
            return;
        }

        string normalizedScore = scoreText.Replace(',', '.');
        if (!double.TryParse(normalizedScore, NumberStyles.Float, CultureInfo.InvariantCulture, out double score) || score < 0.0 || score > 10.0)
        {
            _view.ShowMessage("Lỗi nhập liệu: Điểm trung bình phải là số thực từ 0.0 đến 10.0!", false);
            return;
        }

        var updatedStudent = new Student(id, name, className, score);
        bool success = _repo.Update(updatedStudent);

        if (success)
        {
            var allStudents = _repo.GetAll().ToList();
            _view.ShowStudents(allStudents);
            RefreshStatistics(allStudents);
            _view.ShowMessage($"Cập nhật thông tin sinh viên '{id}' thành công!", true);
        }
        else
        {
            _view.ShowMessage($"Cập nhật thất bại cho sinh viên '{id}'!", false);
        }
    }

    /// <summary>
    /// Xử lý nghiệp vụ Xóa sinh viên theo mã.
    /// </summary>
    public void DeleteStudent()
    {
        string id = _view.StudentId.Trim();

        if (string.IsNullOrWhiteSpace(id))
        {
            _view.ShowMessage("Vui lòng chọn hoặc nhập mã sinh viên cần xóa!", false);
            return;
        }

        if (!_repo.Exists(id))
        {
            _view.ShowMessage($"Không tìm thấy sinh viên có mã '{id}' để xóa!", false);
            return;
        }

        bool removed = _repo.Delete(id);
        if (removed)
        {
            var allStudents = _repo.GetAll().ToList();
            _view.ShowStudents(allStudents);
            RefreshStatistics(allStudents);
            _view.ClearInput();
            _view.ShowMessage($"Đã xóa thành công sinh viên có mã '{id}' khỏi hệ thống!", true);
        }
        else
        {
            _view.ShowMessage($"Xóa sinh viên '{id}' thất bại!", false);
        }
    }

    /// <summary>
    /// Xử lý nghiệp vụ Tìm kiếm sinh viên theo tên hoặc mã.
    /// </summary>
    public void SearchStudent()
    {
        string keyword = _view.SearchKeyword.Trim();

        if (string.IsNullOrWhiteSpace(keyword))
        {
            LoadStudents();
            return;
        }

        var results = _repo.Search(keyword).ToList();
        _view.ShowStudents(results);
        RefreshStatistics(results);
        _view.ShowMessage($"Tìm thấy {results.Count} sinh viên phù hợp với từ khóa '{keyword}'.", true);
    }

    /// <summary>
    /// Xử lý khi người dùng chọn dòng trên danh sách (hiển thị lên form nhập liệu).
    /// </summary>
    public void SelectStudent(string studentId)
    {
        var student = _repo.GetById(studentId);
        if (student != null)
        {
            _view.SetInputFields(student);
            _view.ShowMessage($"Đã nạp thông tin sinh viên '{student.FullName}' lên form nhập liệu.", true);
        }
    }

    /// <summary>
    /// Làm mới giao diện và nạp lại danh sách gốc.
    /// </summary>
    public void ClearInput()
    {
        _view.ClearInput();
        LoadStudents();
    }

    /// <summary>
    /// Tính toán và gửi chỉ số thống kê sang View.
    /// </summary>
    private void RefreshStatistics(List<Student> students)
    {
        int count = students.Count;
        double avg = count > 0 ? students.Average(s => s.AverageScore) : 0.0;
        int pass = students.Count(s => s.AverageScore >= 5.0);
        int fail = students.Count(s => s.AverageScore < 5.0);

        _view.UpdateStatistics(count, avg, pass, fail);
    }
}
