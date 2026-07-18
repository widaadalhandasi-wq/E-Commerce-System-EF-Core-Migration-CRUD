using First_Wep_App;
using FirstWebApp.Models;

namespace FirstWebApp.Repositories
{
    public class ProductRepo
    {
        //ProjectContext context = new ProjectContext();

        private ProjectContext context;

        public ProductRepo(ProjectContext _context)
        {
            context = _context;
        }




        public List<Product> GetAllProducts()
        {
            return context.Products.ToList();
        }

        public Product GetProductById(int id)
        {
            return context.Products.FirstOrDefault(p => p.Id == id);
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

        public void Delete(Product product)
        {
            context.Products.Remove(product);
            context.SaveChanges();
        }
    }
}
