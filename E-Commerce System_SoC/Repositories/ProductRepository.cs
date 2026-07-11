using Backend_session_10_SoC.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_session_10_SoC.Repositories
{
    public class ProductRepository
    {
        private EcommerceContext context;

        public ProductRepository(EcommerceContext context)
        {
            this.context = context;
        }

        public List<Product> GetAll()
        {
            return context.Products.ToList();
        }

        public List<Product> GetAllWithCategory()
        {
            return context.Products
                .Include(p => p.c)
                .ToList();
        }

        public List<Product> GetAvailable()
        {
            return context.Products
                .Where(p => p.isAvailable && p.stockQuantity > 0)
                .ToList();
        }

        public List<Product> GetFilteredByCategoryAndPrice(int categoryId, double minPrice, double maxPrice)
        {
            return context.Products
                .Where(p => p.categoryId == categoryId && p.price >= minPrice && p.price <= maxPrice)
                .OrderBy(p => p.price)
                .ToList();
        }

        public List<object> GetSummaryProjection()
        {
            var result = context.Products
                .Select(p => new
                {
                    p.productId,
                    p.productName,
                    p.price,
                    p.stockQuantity,
                    CategoryName = p.c.categoryName,
                    ReviewCount  = p.Reviews.Count(),
                    AvgRating    = p.Reviews.Count() == 0
                                    ? 0.0
                                    : p.Reviews.Average(r => (double)r.rating)
                })
                .OrderBy(p => p.productName)
                .ToList();

            List<object> list = new List<object>();
            foreach (var item in result)
            {
                list.Add(item);
            }
            return list;
        }

        public Product GetById(int productId)
        {
            return context.Products.FirstOrDefault(p => p.productId == productId);
        }

        public void Add(Product product)
        {
            context.Products.Add(product);
            context.SaveChanges();
        }

        public void Update()
        {
            context.SaveChanges();
        }
    }
}
