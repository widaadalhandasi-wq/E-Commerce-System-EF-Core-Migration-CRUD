using Backend_session_10_SoC.Models;
using Backend_session_10_SoC.Services;

namespace Backend_session_10_SoC.Presentations
{
    public class CategoryPresentation
    {
        private CategoryService categoryService;

        public CategoryPresentation(CategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public void ViewAllCategories()
        {
            Console.WriteLine("\n=== All Categories ===");

            List<Category> categories = categoryService.GetAll();

            foreach (Category c in categories)
                Console.WriteLine($"ID: {c.categoryId}  |  {c.categoryName}  |  {c.description}");
        }

        public void GetCategoryWithProducts()
        {
            Console.WriteLine("\n=== Category with Its Products ===");

            List<Category> categories = categoryService.GetAll();
            foreach (Category c in categories)
                Console.WriteLine($"  ID: {c.categoryId}  |  {c.categoryName}");

            Console.Write("Enter category ID: ");
            int categoryId = int.Parse(Console.ReadLine());

            Category category = categoryService.GetWithProducts(categoryId);

            Console.WriteLine($"\nCategory: {category.categoryName}");
            Console.WriteLine($"Description: {category.description}");
            Console.WriteLine($"\nProducts ({category.Products.Count}):");

            foreach (var p in category.Products)
                Console.WriteLine($"  ID: {p.productId}  |  {p.productName}  |  {p.price:C}  |  Stock: {p.stockQuantity}");
        }
    }
}
