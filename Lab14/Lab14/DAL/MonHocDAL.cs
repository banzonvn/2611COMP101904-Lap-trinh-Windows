using Microsoft.EntityFrameworkCore;
using EduManager.Data;
using EduManager.Models;

namespace EduManager.DAL
{
    /// <summary>
    /// Data Access Layer (DAL) chuyên trách toàn bộ thao tác giao tiếp trực tiếp
    /// với cơ sở dữ liệu SQL Server thông qua Entity Framework Core.
    /// Tuyệt đối không chứa logic kiểm tra nghiệp vụ hay giao diện người dùng.
    /// </summary>
    public class MonHocDAL
    {
        static MonHocDAL()
        {
            // Kiểm tra khả năng kết nối SQL Server, nếu không có sẵn thì kích hoạt In-Memory an toàn
            try
            {
                using var testDb = new EduManagerContext();
                if (!testDb.Database.CanConnect())
                {
                    EduManagerContext.UseInMemoryFallback = true;
                }
                testDb.Database.EnsureCreated();
                testDb.SeedSampleData();
            }
            catch
            {
                EduManagerContext.UseInMemoryFallback = true;
                using var fallbackDb = new EduManagerContext();
                fallbackDb.Database.EnsureCreated();
                fallbackDb.SeedSampleData();
            }
        }

        private EduManagerContext CreateContext() => new EduManagerContext();

        /// <summary>
        /// Lấy toàn bộ danh sách môn học sắp xếp theo mã môn.
        /// </summary>
        public List<MonHoc> GetAll()
        {
            using var db = CreateContext();
            return db.MonHocs
                     .AsNoTracking()
                     .OrderBy(m => m.MaMonHoc)
                     .ToList();
        }

        /// <summary>
        /// Tìm môn học theo khóa chính Mã Môn Học.
        /// </summary>
        public MonHoc? GetById(string maMonHoc)
        {
            using var db = CreateContext();
            return db.MonHocs
                     .AsNoTracking()
                     .FirstOrDefault(m => m.MaMonHoc == maMonHoc);
        }

        /// <summary>
        /// Kiểm tra sự tồn tại của Mã Môn Học trong database.
        /// </summary>
        public bool Exists(string maMonHoc)
        {
            using var db = CreateContext();
            return db.MonHocs.Any(m => m.MaMonHoc.ToLower() == maMonHoc.ToLower());
        }

        /// <summary>
        /// Thêm môn học mới vào cơ sở dữ liệu bằng EF Core Add & SaveChanges.
        /// </summary>
        public void Insert(MonHoc monHoc)
        {
            using var db = CreateContext();
            db.MonHocs.Add(monHoc);
            db.SaveChanges();
        }

        /// <summary>
        /// Cập nhật thông tin môn học đã tồn tại.
        /// </summary>
        public void Update(MonHoc monHoc)
        {
            using var db = CreateContext();
            var target = db.MonHocs.FirstOrDefault(m => m.MaMonHoc == monHoc.MaMonHoc);
            if (target != null)
            {
                target.TenMonHoc = monHoc.TenMonHoc;
                target.SoTinChi = monHoc.SoTinChi;
                target.TrangThai = monHoc.TrangThai;
                db.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException($"Không tìm thấy môn học có mã [{monHoc.MaMonHoc}] để cập nhật.");
            }
        }

        /// <summary>
        /// Xóa môn học khỏi cơ sở dữ liệu bằng EF Core Remove & SaveChanges.
        /// </summary>
        public void Delete(string maMonHoc)
        {
            using var db = CreateContext();
            var target = db.MonHocs.FirstOrDefault(m => m.MaMonHoc == maMonHoc);
            if (target != null)
            {
                db.MonHocs.Remove(target);
                db.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException($"Không tìm thấy môn học có mã [{maMonHoc}] để xóa.");
            }
        }

        /// <summary>
        /// Tìm kiếm môn học theo mã hoặc tên môn học bằng truy vấn LINQ.
        /// </summary>
        public List<MonHoc> Search(string tuKhoa)
        {
            using var db = CreateContext();
            string keyword = tuKhoa.Trim().ToLower();

            return db.MonHocs
                     .AsNoTracking()
                     .Where(m => m.MaMonHoc.ToLower().Contains(keyword) ||
                                 m.TenMonHoc.ToLower().Contains(keyword))
                     .OrderBy(m => m.MaMonHoc)
                     .ToList();
        }

        /// <summary>
        /// Khôi phục dữ liệu mẫu ban đầu cho mục đích kiểm thử.
        /// </summary>
        public void ResetSampleData()
        {
            using var db = CreateContext();
            db.MonHocs.RemoveRange(db.MonHocs);
            db.SaveChanges();
            db.SeedSampleData();
        }
    }
}
