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


        //what i have achieved named: dependency inversion (principle of clean code ) ==> goal ( why )
        // to be independent i have used a technique called : dependency injection => technique ( how )





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
