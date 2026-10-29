using System;
using Microsoft.Data.SqlClient;

namespace QLSinhVienLab12.DAL
{
    public static class Database
    {
        public static string ConnectionString { get; set; } =
            "Data Source=.;Initial Catalog=QLSinhVienLab12;Integrated Security=True;TrustServerCertificate=True";

        public static bool TestConnection(out string message)
        {
            try
            {
                using var conn = new SqlConnection(ConnectionString);
                conn.Open();
                message = "Kết nối SQL Server thành công (Database: QLSinhVienLab12).";
                return true;
            }
            catch (Exception ex)
            {
                message = "Không thể kết nối SQL Server: " + ex.Message;
                return false;
            }
        }
    }
}
