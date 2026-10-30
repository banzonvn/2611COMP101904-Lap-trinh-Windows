namespace Lab07.Models;

/// <summary>
/// Model thực thể sản phẩm của cửa hàng tiện lợi mini.
/// </summary>
public class Product
{
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string Category { get; set; } = "Bánh kẹo";
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public Product() { }

    public Product(string id, string name, string category, decimal price, int quantity)
    {
        ProductId = id;
        ProductName = name;
        Category = category;
        Price = price;
        Quantity = quantity;
    }

    /// <summary>
    /// Tổng giá trị tồn kho của mặt hàng
    /// </summary>
    public decimal TotalAmount => Price * Quantity;

    /// <summary>
    /// Đánh giá tình trạng hàng tồn kho
    /// </summary>
    public string StockStatus
    {
        get
        {
            if (Quantity == 0) return "Hết hàng";
            if (Quantity < 10) return "Sắp hết";
            return "Còn hàng";
        }
    }
}
