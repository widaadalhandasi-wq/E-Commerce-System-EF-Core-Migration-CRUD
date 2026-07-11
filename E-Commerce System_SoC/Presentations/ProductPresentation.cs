using Backend_session_10_SoC.Models;
using Backend_session_10_SoC.Services;

namespace Backend_session_10_SoC.Presentations
{
    public class ProductPresentation
    {
        private ProductService productService;
        private CategoryService categoryService;

        public ProductPresentation(ProductService productService, CategoryService categoryService)
        {
            this.productService  = productService;
            this.categoryService = categoryService;
        }

        public void AddProduct()
        {
            Console.WriteLine("\n=== Add New Product ===");

            List<Category> categories = categoryService.GetAll();
            Console.WriteLine("Available categories:");
            foreach (Category c in categories)
                Console.WriteLine($"  ID: {c.categoryId}  |  {c.categoryName}");

            Console.Write("Enter category ID: ");
            int categoryId = int.Parse(Console.ReadLine());

            Console.Write("Enter product name: ");
            string name = Console.ReadLine();

            Console.Write("Enter description (optional): ");
            string desc = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(desc)) desc = null;

            Console.Write("Enter price: ");
            double price = double.Parse(Console.ReadLine());

            Console.Write("Enter stock quantity: ");
            int stock = int.Parse(Console.ReadLine());

            productService.AddProduct(name, desc, price, stock, categoryId);

            int newId = productService.GetLastProductId();
            Console.WriteLine($"Product added successfully. Assigned ID: {newId}");
        }

        public void ViewAllProducts()
        {
            Console.WriteLine("\n=== All Products ===");

            List<Product> products = productService.GetAll();

            foreach (Product p in products)
                Console.WriteLine($"ID: {p.productId}  |  {p.productName}  |  {p.price:C}" +
                                  $"  |  Category: {p.c.categoryName}" +
                                  $"  |  Stock: {p.stockQuantity}  |  Available: {p.isAvailable}");
        }

        public void FilterProducts()
        {
            Console.WriteLine("\n=== Filter Products by Category & Price ===");

            List<Category> categories = categoryService.GetAll();
            Console.WriteLine("Categories:");
            foreach (Category c in categories)
                Console.WriteLine($"  ID: {c.categoryId}  |  {c.categoryName}");

            Console.Write("Enter category ID: ");
            int categoryId = int.Parse(Console.ReadLine());

            Console.Write("Enter minimum price: ");
            double minPrice = double.Parse(Console.ReadLine());

            Console.Write("Enter maximum price: ");
            double maxPrice = double.Parse(Console.ReadLine());

            List<Product> results = productService.GetFiltered(categoryId, minPrice, maxPrice);

            Console.WriteLine($"\nFound {results.Count} product(s):");
            foreach (Product p in results)
                Console.WriteLine($"  ID: {p.productId}  |  {p.productName}  |  {p.price:C}  |  Stock: {p.stockQuantity}");
        }

        public void UpdateProduct()
        {
            Console.WriteLine("\n=== Update Product Price & Availability ===");

            List<Product> products = productService.GetAll();
            foreach (Product p in products)
                Console.WriteLine($"  ID: {p.productId}  |  {p.productName}  |  {p.price:C}  |  Available: {p.isAvailable}");

            Console.Write("Enter product ID to update: ");
            int productId = int.Parse(Console.ReadLine());

            Product current = productService.GetById(productId);

            Console.Write($"Enter new price (current: {current.price:C}): ");
            double newPrice = double.Parse(Console.ReadLine());

            Console.Write($"Is available? (y/n, current: {current.isAvailable}): ");
            bool isAvailable = Console.ReadLine().Trim().ToLower() == "y";

            productService.UpdatePriceAndAvailability(productId, newPrice, isAvailable);

            Console.WriteLine("Product updated successfully.");
        }

        public void ProductSummaryReport()
        {
            Console.WriteLine("\n=== Product Summary Report ===");

            List<object> summary = productService.GetSummaryReport();

            Console.WriteLine($"\n  {"Product",-25} {"Category",-18} {"Price",-10} {"Stock",-7} {"Reviews",-9} {"Avg"}");
            Console.WriteLine("  " + new string('-', 78));

            foreach (var item in summary)
            {
                dynamic d = item;
                Console.WriteLine($"  {d.productName,-25} {d.CategoryName,-18} {d.price,-10:C}" +
                                  $" {d.stockQuantity,-7} {d.ReviewCount,-9} {d.AvgRating:F1}");
            }
        }
    }
}
