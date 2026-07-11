using Backend_session_10_SoC.Models;
using Backend_session_10_SoC.Repositories;

namespace Backend_session_10_SoC.Services
{
    public class CategoryService
    {
        private CategoryRepository categoryRepo;

        public CategoryService(CategoryRepository categoryRepo)
        {
            this.categoryRepo = categoryRepo;
        }

        public List<Category> GetAll()
        {
            return categoryRepo.GetAll();
        }

        public Category GetById(int categoryId)
        {
            return categoryRepo.GetById(categoryId);
        }

        public Category GetWithProducts(int categoryId)
        {
            return categoryRepo.GetWithProducts(categoryId);
        }
    }
}
