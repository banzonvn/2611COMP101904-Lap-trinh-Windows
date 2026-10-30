namespace Lab07.Models;

/// <summary>
/// Model thực thể khách hàng của cửa hàng tiện lợi mini.
/// </summary>
public class Customer
{
    public string CustomerId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string CustomerType { get; set; } = "Thường"; // "Thường", "Thân thiết", "VIP"
    public int Points { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Customer() { }

    public Customer(string id, string name, string phone, string type, int points = 0)
    {
        CustomerId = id;
        FullName = name;
        PhoneNumber = phone;
        CustomerType = type;
        Points = points;
        CreatedAt = DateTime.Now;
    }
}
