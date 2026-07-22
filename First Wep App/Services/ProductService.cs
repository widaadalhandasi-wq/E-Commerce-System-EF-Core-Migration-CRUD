using FirstWebApp.DTOs;
using FirstWebApp.Models;
using FirstWebApp.Repositories;
using System.Security.Cryptography.Pkcs;

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

        public List<ProductOutputDTO> GetAllProducts()
        {
            return repo.GetAllProducts()
                       .Select(product => new ProductOutputDTO
                       {
                           Name = product.Name,
                           Price = product.Price
                       })
                       .ToList();
        }


        public ProductAllOutputDTO GetProductById(int id)
        {
            Product p = repo.GetProductById(id);

            ProductAllOutputDTO output = new ProductAllOutputDTO();
            output.Price = p.Price;
            output.Name = p.Name;
            output.Description = p.Description;

            return output;
        }


        public int Create(Product product)
        {

            repo.Add(product);
            return product.Id;
        }


        public int Create(ProductInputDTO product)
        {

            Product p = new Product();
            p.Name = product.Name;
            p.Price = product.Price;
            p.Description = product.Description;
            p.createdDate = DateTime.Now;
            p.Count = 0;

            repo.Add(p);
            return p.Id;
        }

        public bool UpdateCount(int productId, int newCount)
        {
            Product product = repo.GetProductById(productId);
            if (product == null)
            {
                return false;
            }

            product.Count = newCount;
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
