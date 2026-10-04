using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab04
{
    #region 1. Interface IEntity & Generic Repository
    public interface IEntity
    {
        string Id { get; }
    }

    public class Repository<T> where T : class, IEntity
    {
        private readonly List<T> _items = new List<T>();

        public void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Dữ liệu không được để trống.");
            _items.Add(item);
        }

        public bool Remove(string id)
        {
            var item = FindById(id);
            if (item != null)
            {
                return _items.Remove(item);
            }
            return false;
        }

        public T? FindById(string id)
        {
            return _items.FirstOrDefault(x => string.Equals(x.Id, id, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return _items.Where(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return _items.ToList();
        }
    }
    #endregion

    #region 2. Custom Exceptions
    public class DuplicateProductException : Exception
    {
        public string ProductId { get; }

        public DuplicateProductException(string id)
            : base($"Lỗi: Sản phẩm với mã '{id}' đã tồn tại trong hệ thống.")
        {
            ProductId = id;
        }

        public DuplicateProductException(string id, string message) : base(message)
        {
            ProductId = id;
        }
    }

    public class ProductNotFoundException : Exception
    {
        public string ProductId { get; }

        public ProductNotFoundException(string id)
            : base($"Lỗi: Không tìm thấy sản phẩm có mã '{id}'.")
        {
            ProductId = id;
        }

        public ProductNotFoundException(string id, string message) : base(message)
        {
            ProductId = id;
        }
    }
    #endregion

    #region 3. Model Product
    public class Product : IEntity
    {
        private string _maSp = string.Empty;
        private string _tenSp = string.Empty;
        private decimal _price;
        private int _quantity;

        public string Id => MaSP;

        public string MaSP
        {
            get => _maSp;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Mã sản phẩm không được để trống.");
                _maSp = value.Trim();
            }
        }

        public string TenSP
        {
            get => _tenSp;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tên sản phẩm không được để trống.");
                _tenSp = value.Trim();
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Đơn giá không được âm.");
                _price = value;
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Số lượng không được âm.");
                _quantity = value;
            }
        }

        public Product(string maSp, string tenSp, decimal price, int quantity)
        {
            MaSP = maSp;
            TenSP = tenSp;
            Price = price;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return $"Mã SP: {MaSP,-8} | Tên SP: {TenSP,-20} | Đơn giá: {Price,12:N0} đ | Số lượng: {Quantity,5} | Thành tiền: {(Price * Quantity),14:N0} đ";
        }
    }
    #endregion

    #region 4. Service Layer (ProductService)
    public class ProductService
    {
        private readonly Repository<Product> _repository;

        public event Action<Product>? OnProductAdded;
        public event Action<string>? OnProductRemoved;

        public ProductService(Repository<Product> repository)
        {
            _repository = repository;
        }

        public void AddProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (_repository.FindById(product.MaSP) != null)
            {
                throw new DuplicateProductException(product.MaSP);
            }

            _repository.Add(product);
            OnProductAdded?.Invoke(product);
        }

        public void RemoveProduct(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Mã sản phẩm không hợp lệ.");

            var product = _repository.FindById(id);
            if (product == null)
            {
                throw new ProductNotFoundException(id);
            }

            _repository.Remove(id);
            OnProductRemoved?.Invoke(id);
        }

        public Product? FindById(string id)
        {
            return _repository.FindById(id);
        }

        public IEnumerable<Product> Filter(Func<Product, bool> predicate)
        {
            return _repository.Find(predicate);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _repository.GetAll();
        }

        public decimal CalculateTotalInventoryValue()
        {
            return _repository.GetAll().Sum(p => p.Price * p.Quantity);
        }
    }
    #endregion

    #region 5. Program & Menu
    internal class Program
    {
        private static ProductService? _service;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            var repository = new Repository<Product>();
            _service = new ProductService(repository);

            _service.OnProductAdded += product =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n[EVENT] Đã thêm thành công: {product.TenSP} ({product.MaSP})");
                Console.ResetColor();
            };

            _service.OnProductRemoved += id =>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n[EVENT] Đã xóa thành công sản phẩm: {id}");
                Console.ResetColor();
            };

            SeedSampleData();

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n===== PRODUCT MANAGER =====");
                Console.WriteLine("1. Them san pham");
                Console.WriteLine("2. Xuat danh sach");
                Console.WriteLine("3. Tim theo ma");
                Console.WriteLine("4. Tim theo ten");
                Console.WriteLine("5. Loc theo khoang gia");
                Console.WriteLine("6. Xoa san pham");
                Console.WriteLine("7. Tinh tong gia tri kho");
                Console.WriteLine("0. Thoat");
                Console.Write("Chon: ");

                string choice = Console.ReadLine()?.Trim() ?? string.Empty;
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            HandleAddProduct();
                            break;
                        case "2":
                            HandleDisplayAll();
                            break;
                        case "3":
                            HandleFindById();
                            break;
                        case "4":
                            HandleFindByName();
                            break;
                        case "5":
                            HandleFilterByPrice();
                            break;
                        case "6":
                            HandleRemoveProduct();
                            break;
                        case "7":
                            HandleCalculateTotalValue();
                            break;
                        case "0":
                            exit = true;
                            Console.WriteLine("Đã đóng chương trình.");
                            break;
                        default:
                            Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng chọn từ 0 đến 7.");
                            break;
                    }
                }
                catch (DuplicateProductException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[Lỗi nghiệp vụ]: {ex.Message}");
                    Console.ResetColor();
                }
                catch (ProductNotFoundException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[Lỗi nghiệp vụ]: {ex.Message}");
                    Console.ResetColor();
                }
                catch (ArgumentException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[Lỗi dữ liệu]: {ex.Message}");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"[Lỗi hệ thống]: {ex.Message}");
                    Console.ResetColor();
                }
            }
        }

        private static void SeedSampleData()
        {
            _service?.AddProduct(new Product("SP01", "Chuột Logitech G102", 400000, 15));
            _service?.AddProduct(new Product("SP02", "Bàn phím cơ Akko", 1250000, 8));
            _service?.AddProduct(new Product("SP03", "Tai nghe Sony WH-1000XM4", 5500000, 4));
        }

        private static void HandleAddProduct()
        {
            Console.WriteLine("--- THÊM SẢN PHẨM MỚI ---");
            Console.Write("Nhập mã sản phẩm: ");
            string id = Console.ReadLine()?.Trim() ?? string.Empty;

            Console.Write("Nhập tên sản phẩm: ");
            string name = Console.ReadLine()?.Trim() ?? string.Empty;

            Console.Write("Nhập đơn giá: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
                throw new FormatException("Đơn giá phải là số hợp lệ.");

            Console.Write("Nhập số lượng: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity))
                throw new FormatException("Số lượng phải là số nguyên hợp lệ.");

            _service?.AddProduct(new Product(id, name, price, quantity));
        }

        private static void HandleDisplayAll()
        {
            Console.WriteLine("--- DANH SÁCH SẢN PHẨM ---");
            var products = _service?.GetAllProducts().ToList() ?? new List<Product>();

            if (!products.Any())
            {
                Console.WriteLine("Danh sách sản phẩm hiện đang rỗng.");
                return;
            }

            foreach (var item in products)
            {
                Console.WriteLine(item);
            }
        }

        private static void HandleFindById()
        {
            Console.Write("Nhập mã sản phẩm cần tìm: ");
            string id = Console.ReadLine()?.Trim() ?? string.Empty;

            var product = _service?.FindById(id);
            if (product != null)
            {
                Console.WriteLine("Kết quả tìm kiếm:");
                Console.WriteLine(product);
            }
            else
            {
                throw new ProductNotFoundException(id);
            }
        }

        private static void HandleFindByName()
        {
            Console.Write("Nhập từ khóa tên sản phẩm: ");
            string keyword = Console.ReadLine()?.Trim() ?? string.Empty;

            Func<Product, bool> searchFilter = p => p.TenSP.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
            var results = _service?.Filter(searchFilter).ToList() ?? new List<Product>();

            if (!results.Any())
            {
                Console.WriteLine($"Không tìm thấy sản phẩm nào chứa từ khóa '{keyword}'.");
                return;
            }

            Console.WriteLine($"Tìm thấy {results.Count} sản phẩm phù hợp:");
            foreach (var item in results)
            {
                Console.WriteLine(item);
            }
        }

        private static void HandleFilterByPrice()
        {
            Console.Write("Nhập giá tối thiểu (Min): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal minPrice))
                throw new FormatException("Giá tối thiểu phải là số hợp lệ.");

            Console.Write("Nhập giá tối đa (Max): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal maxPrice))
                throw new FormatException("Giá tối đa phải là số hợp lệ.");

            if (minPrice > maxPrice)
                throw new ArgumentException("Giá tối thiểu không được lớn hơn giá tối đa.");

            Func<Product, bool> priceFilter = p => p.Price >= minPrice && p.Price <= maxPrice;
            var results = _service?.Filter(priceFilter).ToList() ?? new List<Product>();

            if (!results.Any())
            {
                Console.WriteLine($"Không có sản phẩm nào trong tầm giá từ {minPrice:N0} đ đến {maxPrice:N0} đ.");
                return;
            }

            Console.WriteLine($"--- KẾT QUẢ LỌC GIÁ ({minPrice:N0} đ - {maxPrice:N0} đ) ---");
            foreach (var item in results)
            {
                Console.WriteLine(item);
            }
        }

        private static void HandleRemoveProduct()
        {
            Console.Write("Nhập mã sản phẩm cần xóa: ");
            string id = Console.ReadLine()?.Trim() ?? string.Empty;

            _service?.RemoveProduct(id);
        }

        private static void HandleCalculateTotalValue()
        {
            decimal total = _service?.CalculateTotalInventoryValue() ?? 0;
            Console.WriteLine($"Tổng giá trị hàng tồn kho: {total:N0} VNĐ");
        }
    }
    #endregion
}