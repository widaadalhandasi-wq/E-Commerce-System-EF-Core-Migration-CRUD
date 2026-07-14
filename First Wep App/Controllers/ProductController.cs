using FirstWebApp.Models;
using FirstWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebApp.Controllers
{
    [ApiController]
    [Route("product")]
    public class ProductController : ControllerBase
    {

        //ProductService productService = new ProductService();

        private ProductService productService;

        public ProductController(ProductService _productService)
        {
            productService = _productService;
        }




        [HttpGet("GetAllProducts")]        
        public IActionResult GetAllProducts()
        {
            return Ok(  productService.GetAllProducts()  )  ;
        }

        [HttpGet("GetProductById")]
        public IActionResult GetProductById(int id)
        {
            return Ok ( productService.GetProductById(id) );   
        }



        [HttpPost("Add")]
        public IActionResult Add(Product product)
        {
            int productId = productService.Create(product);
            return Ok(new { ProductId = productId });
        }

        [HttpPut("UpdatePrice")]
        public IActionResult UpdatePrice(int productId, int newPrice)
        {
            bool updated = productService.UpdatePrice(productId, newPrice);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int productId)
        {
            bool deleted = productService.Delete(productId);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
