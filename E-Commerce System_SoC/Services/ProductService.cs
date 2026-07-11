using Backend_session_10_SoC.Models;
using Backend_session_10_SoC.Repositories;

namespace Backend_session_10_SoC.Services
{
    public class ProductService
    {
        private ProductRepository productRepo;
        private CategoryRepository categoryRepo;

        public ProductService(ProductRepository productRepo, CategoryRepository categoryRepo)
        {
            this.productRepo  = productRepo;
            this.categoryRepo = categoryRepo;
        }

        public List<Product> GetAll()
        {
            return productRepo.GetAll();
        }

        public List<Product> GetAvailable()
        {
            return productRepo.GetAvailable();
        }

        public List<Product> GetFiltered(int categoryId, double minPrice, double maxPrice)
        {
            return productRepo.GetFilteredByCategoryAndPrice(categoryId, minPrice, maxPrice);
        }

        public List<object> GetSummaryReport()
        {
            return productRepo.GetSummaryProjection();
        }

        public Product GetById(int productId)
        {
            return productRepo.GetById(productId);
        }

        public void AddProduct(string name, string description, double price,
                               int stockQuantity, int categoryId)
        {
            Product product = new Product();
            product.productName   = name;
            product.description   = description;
            product.price         = price;
            product.stockQuantity = stockQuantity;
            product.categoryId    = categoryId;
            product.createdAt     = DateTime.Now;
            product.isAvailable   = true;

            productRepo.Add(product);
        }

        public int GetLastProductId()
        {
            List<Product> all = productRepo.GetAll();
            return all[all.Count - 1].productId;
        }

        public void UpdatePriceAndAvailability(int productId, double newPrice, bool isAvailable)
        {
            Product product = productRepo.GetById(productId);
            product.price       = newPrice;
            product.isAvailable = isAvailable;
            productRepo.Update();
        }

        public void DecrementStock(int productId, int quantity)
        {
            Product product = productRepo.GetById(productId);
            product.stockQuantity -= quantity;
            productRepo.Update();
        }

        public void RestoreStock(int productId, int quantity)
        {
            Product product = productRepo.GetById(productId);
            product.stockQuantity += quantity;
            productRepo.Update();
        }
    }
}
