namespace Lab07.Models;

/// <summary>
/// Model đại diện cho người dùng đăng nhập hệ thống ShopManagerMini.
/// </summary>
public class User
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = "User"; // "Admin" hoặc "User"

    public User() { }

    public User(string username, string password, string fullName, string role)
    {
        Username = username;
        Password = password;
        FullName = fullName;
        Role = role;
    }

    public bool IsAdmin => Role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
}
