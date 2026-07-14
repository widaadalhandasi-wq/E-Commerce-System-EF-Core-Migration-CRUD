using FirstWebApp.Models;
using FirstWebApp.Repositories;

namespace FirstWebApp.Services
{
    public class ProductService
    {

        //ProductRepo repo = new ProductRepo();

        private ProductRepo repo;

        public ProductService(ProductRepo _repo)
        {
            repo = _repo;
        }





        public List<Product> GetAllProducts()
        {
            return repo.GetAllProducts();
        }

        public Product GetProductById(int id)
        {
            return repo.GetProductById(id);
        }


        public int Create(Product product)
        {

            repo.Add(product);
            return product.Id;
        }

        public bool UpdatePrice(int productId, int newPrice)
        {
            Product product = repo.GetProductById(productId);
            if (product == null)
            {
                return false;
            }

            product.Price = newPrice;
            repo.Update();
            return true;
         
        }


        public bool Delete(int productId)
        {
            Product product = repo.GetProductById(productId);
            if (product == null)
            {
                return false;
            }

            repo.Delete(product);
            return true;
        }
    }
}
