using System.ComponentModel;
using Lab07.Models;

namespace Lab07.Data;

/// <summary>
/// Quản lý kho dữ liệu tạm thời trong bộ nhớ (In-memory Data Store)
/// Dùng chung xuyên suốt các Form của ứng dụng ShopManagerMini.
/// </summary>
public static class ShopDataStore
{
    // Danh sách tài khoản người dùng mẫu
    public static List<User> Users { get; } = new()
    {
        new User("admin", "123", "Quản trị viên", "Admin"),
        new User("user", "123", "Nhân viên bán hàng", "User"),
        new User("hoangquan", "123", "Lê Hoàng Quân", "Admin")
    };

    // Danh sách sản phẩm (BindingList giúp DataGridView tự động nhận biết thay đổi)
    public static BindingList<Product> Products { get; } = new()
    {
        new Product("SP001", "Bánh Cosy mè giòn 288g", "Bánh kẹo", 35000m, 45),
        new Product("SP002", "Bánh ChocoPie hộp 12 cái", "Bánh kẹo", 62000m, 30),
        new Product("SP003", "Nước ngọt Coca-Cola lon 320ml", "Đồ uống", 11000m, 120),
        new Product("SP004", "Trà xanh Không Độ 455ml", "Đồ uống", 10000m, 85),
        new Product("SP005", "Cà phê sữa Highlands lon 185ml", "Đồ uống", 16000m, 60),
        new Product("SP006", "Sữa tươi chua TH True Milk 180ml", "Đồ uống", 9500m, 8),
        new Product("SP007", "Dầu gội Clear men mát lạnh 630g", "Hóa mỹ phẩm", 185000m, 25),
        new Product("SP008", "Kem đánh răng P/S than hoạt tính", "Hóa mỹ phẩm", 42000m, 50),
        new Product("SP009", "Cá hộp Ba Cô Gái 155g", "Thực phẩm đóng hộp", 24000m, 40),
        new Product("SP010", "Khăn ướt Nano Bạc gói 80 tờ", "Gia dụng", 28000m, 0)
    };

    // Danh sách khách hàng thân thiết
    public static BindingList<Customer> Customers { get; } = new()
    {
        new Customer("KH001", "Lê Hoàng Quân", "0912345678", "VIP", 850),
        new Customer("KH002", "Nguyễn Văn An", "0901234567", "Thân thiết", 420),
        new Customer("KH003", "Trần Thị Mai Loan", "0987654321", "VIP", 1250),
        new Customer("KH004", "Phạm Quốc Tuấn", "0933112233", "Thường", 80),
        new Customer("KH005", "Vũ Hoàng My", "0977889900", "Thân thiết", 360),
        new Customer("KH006", "Đỗ Minh Đức", "0918273645", "Thường", 50),
        new Customer("KH007", "Ngô Kiến Hùng", "0945678123", "Thường", 20),
        new Customer("KH008", "Hoàng Bích Ngọc", "0966554433", "VIP", 990)
    };

    /// <summary>
    /// Xác thực thông tin đăng nhập
    /// </summary>
    public static User? Authenticate(string username, string password)
    {
        return Users.FirstOrDefault(u =>
            u.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase) &&
            u.Password == password.Trim());
    }

    /// <summary>
    /// Kiểm tra trùng mã sản phẩm
    /// </summary>
    public static bool ProductExists(string productId)
    {
        return Products.Any(p => p.ProductId.Equals(productId.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Kiểm tra trùng mã khách hàng
    /// </summary>
    public static bool CustomerExists(string customerId)
    {
        return Customers.Any(c => c.CustomerId.Equals(customerId.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}
