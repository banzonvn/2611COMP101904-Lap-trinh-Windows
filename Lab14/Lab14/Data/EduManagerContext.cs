using Microsoft.EntityFrameworkCore;
using EduManager.Models;

namespace EduManager.Data
{
    /// <summary>
    /// DbContext đóng vai trò là cầu nối trung tâm giữa ứng dụng WinForms và cơ sở dữ liệu.
    /// Quản lý theo dõi trạng thái thực thể (Change Tracker) và thực thi lưu trữ (SaveChanges).
    /// </summary>
    public class EduManagerContext : DbContext
    {
        public static string ConnectionString { get; set; } =
            "Server=.;Database=EduManagerDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public static bool UseInMemoryFallback { get; set; } = false;

        public DbSet<MonHoc> MonHocs { get; set; } = null!;

        public EduManagerContext()
        {
        }

        public EduManagerContext(DbContextOptions<EduManagerContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (UseInMemoryFallback)
                {
                    optionsBuilder.UseInMemoryDatabase("EduManagerDB_InMemory");
                }
                else
                {
                    try
                    {
                        optionsBuilder.UseSqlServer(ConnectionString);
                    }
                    catch
                    {
                        optionsBuilder.UseInMemoryDatabase("EduManagerDB_InMemory");
                    }
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình bảng và các ràng buộc dữ liệu bằng Fluent API
            modelBuilder.Entity<MonHoc>(entity =>
            {
                entity.ToTable("MONHOC");
                entity.HasKey(e => e.MaMonHoc);

                entity.Property(e => e.MaMonHoc)
                      .HasColumnName("MaMonHoc")
                      .HasMaxLength(10)
                      .IsRequired();

                entity.Property(e => e.TenMonHoc)
                      .HasColumnName("TenMonHoc")
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.SoTinChi)
                      .HasColumnName("SoTinChi")
                      .IsRequired();

                entity.Property(e => e.TrangThai)
                      .HasColumnName("TrangThai")
                      .HasDefaultValue(true)
                      .IsRequired();
            });
        }

        /// <summary>
        /// Nạp sẵn dữ liệu mẫu ban đầu nếu cơ sở dữ liệu chưa có dữ liệu.
        /// </summary>
        public void SeedSampleData()
        {
            if (!MonHocs.Any())
            {
                MonHocs.AddRange(new List<MonHoc>
                {
                    new MonHoc { MaMonHoc = "COMP1019", TenMonHoc = "Lập trình trên Windows", SoTinChi = 3, TrangThai = true },
                    new MonHoc { MaMonHoc = "COMP1002", TenMonHoc = "Cấu trúc dữ liệu và giải thuật", SoTinChi = 4, TrangThai = true },
                    new MonHoc { MaMonHoc = "COMP1011", TenMonHoc = "Cơ sở dữ liệu", SoTinChi = 3, TrangThai = true },
                    new MonHoc { MaMonHoc = "COMP1024", TenMonHoc = "Lập trình hướng đối tượng", SoTinChi = 3, TrangThai = true },
                    new MonHoc { MaMonHoc = "COMP1035", TenMonHoc = "Mạng máy tính cơ bản", SoTinChi = 3, TrangThai = true },
                    new MonHoc { MaMonHoc = "COMP1042", TenMonHoc = "Phát triển ứng dụng Web", SoTinChi = 4, TrangThai = true },
                    new MonHoc { MaMonHoc = "SOFT1005", TenMonHoc = "Kỹ năng mềm và làm việc nhóm", SoTinChi = 2, TrangThai = true },
                    new MonHoc { MaMonHoc = "MATH1001", TenMonHoc = "Giải tích cơ bản", SoTinChi = 3, TrangThai = false }
                });
                SaveChanges();
            }
        }
    }
}
