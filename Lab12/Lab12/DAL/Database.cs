using System;
using Microsoft.Data.SqlClient;

namespace QLSinhVienLab12.DAL
{
    public static class Database
    {
        // Connection string mặc định cho máy nội bộ và phòng máy thực hành
        public static string ConnectionString =
            @"Data Source=.;Initial Catalog=QLSinhVienLab12;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=2";

        // Connection string dự phòng cho SQL Server Express
        public static string ExpressConnectionString =
            @"Data Source=.\SQLEXPRESS;Initial Catalog=QLSinhVienLab12;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=2";

        public static bool CheckConnection(string connStr)
        {
            try
            {
                using var conn = new SqlConnection(connStr);
                conn.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
