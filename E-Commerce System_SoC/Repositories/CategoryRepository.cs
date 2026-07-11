using Backend_session_10_SoC.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_session_10_SoC.Repositories
{
    public class CategoryRepository
    {
        private EcommerceContext context;

        public CategoryRepository(EcommerceContext context)
        {
            this.context = context;
        }

        public List<Category> GetAll()
        {
            return context.Categories.ToList();
        }

        public Category GetById(int categoryId)
        {
            return context.Categories.FirstOrDefault(c => c.categoryId == categoryId);
        }

        public Category GetWithProducts(int categoryId)
        {
            return context.Categories
                .Include(c => c.Products)
                .FirstOrDefault(c => c.categoryId == categoryId);
        }

        public void Add(Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
        }

        public void Update()
        {
            context.SaveChanges();
        }
    }
}
