using EduManager.DAL;
using EduManager.Models;

namespace EduManager.BLL
{
    /// <summary>
    /// Business Logic Layer (BLL) trung tâm tiếp nhận yêu cầu từ GUI,
    /// thực hiện chuẩn hóa dữ liệu, thẩm định các quy tắc nghiệp vụ (Validation),
    /// và điều phối dữ liệu xuống Data Access Layer (DAL).
    /// </summary>
    public class MonHocBLL
    {
        private readonly MonHocDAL _monHocDAL;

        public MonHocBLL()
        {
            _monHocDAL = new MonHocDAL();
        }

        public MonHocBLL(MonHocDAL dal)
        {
            _monHocDAL = dal;
        }

        /// <summary>
        /// Lấy toàn bộ danh sách môn học cho tầng GUI.
        /// </summary>
        public List<MonHoc> GetAll()
        {
            return _monHocDAL.GetAll();
        }

        /// <summary>
        /// Lấy thông tin một môn học theo mã.
        /// </summary>
        public MonHoc? GetById(string maMonHoc)
        {
            if (string.IsNullOrWhiteSpace(maMonHoc))
            {
                throw new ArgumentException("Mã môn học cần tra cứu không được để trống.");
            }
            return _monHocDAL.GetById(maMonHoc.Trim().ToUpper());
        }

        /// <summary>
        /// Thêm mới môn học kèm toàn bộ các quy tắc thẩm định nghiệp vụ bắt buộc.
        /// </summary>
        public void Add(MonHoc monHoc)
        {
            // 1. Chuẩn hóa dữ liệu đầu vào
            ChuanHoaMonHoc(monHoc);

            // 2. Thẩm định các trường bắt buộc
            KiemTraThongTinMonHoc(monHoc);

            // 3. Quy tắc nghiệp vụ cốt lõi: Không cho phép thêm mã môn học đã tồn tại
            if (_monHocDAL.Exists(monHoc.MaMonHoc))
            {
                throw new InvalidOperationException($"Mã môn học '{monHoc.MaMonHoc}' đã tồn tại trong hệ thống. Vui lòng chọn mã khác.");
            }

            // 4. Ủy quyền cho DAL lưu trữ xuống SQL Server bằng EF Core
            _monHocDAL.Insert(monHoc);
        }

        /// <summary>
        /// Cập nhật thông tin môn học đã có trong hệ thống.
        /// </summary>
        public void Update(MonHoc monHoc)
        {
            // 1. Chuẩn hóa dữ liệu
            ChuanHoaMonHoc(monHoc);

            // 2. Thẩm định thông tin
            KiemTraThongTinMonHoc(monHoc);

            // 3. Kiểm tra sự tồn tại của môn học
            if (!_monHocDAL.Exists(monHoc.MaMonHoc))
            {
                throw new KeyNotFoundException($"Không thể sửa: Môn học có mã '{monHoc.MaMonHoc}' không tồn tại trong hệ thống.");
            }

            // 4. Ủy quyền cho DAL cập nhật
            _monHocDAL.Update(monHoc);
        }

        /// <summary>
        /// Xóa môn học khỏi hệ thống sau khi đã kiểm tra ràng buộc.
        /// </summary>
        public void Delete(string maMonHoc)
        {
            if (string.IsNullOrWhiteSpace(maMonHoc))
            {
                throw new ArgumentException("Mã môn học cần xóa không được để trống.");
            }

            string cleanCode = maMonHoc.Trim().ToUpper();

            if (!_monHocDAL.Exists(cleanCode))
            {
                throw new KeyNotFoundException($"Không thể xóa: Môn học có mã '{cleanCode}' không tồn tại trong cơ sở dữ liệu.");
            }

            _monHocDAL.Delete(cleanCode);
        }

        /// <summary>
        /// Tìm kiếm môn học theo từ khóa mã môn hoặc tên môn.
        /// </summary>
        public List<MonHoc> Search(string? tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
            {
                return GetAll();
            }

            return _monHocDAL.Search(tuKhoa.Trim());
        }

        /// <summary>
        /// Khôi phục dữ liệu mẫu ban đầu.
        /// </summary>
        public void ResetSampleData()
        {
            _monHocDAL.ResetSampleData();
        }

        /// <summary>
        /// Thống kê chỉ số tổng quan (KPI) cho danh mục môn học.
        /// </summary>
        public (int TongMon, int ConDung, int NgungDung, double TinChiTB, int TongTinChi) GetStatistics(IEnumerable<MonHoc> danhSach)
        {
            var list = danhSach.ToList();
            int tong = list.Count;
            if (tong == 0) return (0, 0, 0, 0.0, 0);

            int conDung = list.Count(m => m.TrangThai);
            int ngungDung = tong - conDung;
            int tongTc = list.Sum(m => m.SoTinChi);
            double tcTb = Math.Round(list.Average(m => m.SoTinChi), 1);

            return (tong, conDung, ngungDung, tcTb, tongTc);
        }

        // ==========================================
        // CÁC HÀM VALIDATION & CHUẨN HÓA DỮ LIỆU NỘI BỘ
        // ==========================================
        private static void ChuanHoaMonHoc(MonHoc monHoc)
        {
            if (monHoc == null)
            {
                throw new ArgumentNullException(nameof(monHoc), "Dữ liệu môn học không được phép null.");
            }

            monHoc.MaMonHoc = monHoc.MaMonHoc?.Trim().ToUpper() ?? string.Empty;
            monHoc.TenMonHoc = monHoc.TenMonHoc?.Trim() ?? string.Empty;
        }

        private static void KiemTraThongTinMonHoc(MonHoc monHoc)
        {
            // 1. Kiểm tra Mã môn học
            if (string.IsNullOrWhiteSpace(monHoc.MaMonHoc))
            {
                throw new ArgumentException("Mã môn học không được để trống.");
            }

            if (monHoc.MaMonHoc.Length > 10)
            {
                throw new ArgumentException("Mã môn học không được vượt quá 10 ký tự.");
            }

            // 2. Kiểm tra Tên môn học
            if (string.IsNullOrWhiteSpace(monHoc.TenMonHoc))
            {
                throw new ArgumentException("Tên môn học không được để trống.");
            }

            if (monHoc.TenMonHoc.Length > 100)
            {
                throw new ArgumentException("Tên môn học không được vượt quá 100 ký tự.");
            }

            // 3. Kiểm tra Số tín chỉ (Ràng buộc 1 đến 6)
            if (monHoc.SoTinChi < 1 || monHoc.SoTinChi > 6)
            {
                throw new ArgumentOutOfRangeException(nameof(monHoc.SoTinChi), "Số tín chỉ phải nằm trong khoảng từ 1 đến 6.");
            }
        }
    }
}
