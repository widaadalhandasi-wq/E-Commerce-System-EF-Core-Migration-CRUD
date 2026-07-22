using FirstWebApp.DTOs;
using FirstWebApp.Models;
using FirstWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebApp.Controllers
{
    [ApiController]
    [Route("product")]
    [Authorize]
    public class ProductController : ControllerBase
    {

        //ProductService productService = new ProductService(); 
        //apply dependency inversion concept 

        private ProductService productService;
        public ProductController(ProductService _productService)//dependency injection
        {
            productService = _productService;
        }



        [AllowAnonymous]
        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            List<ProductOutputDTO> result = productService.GetAllProducts();

            if (result.Count > 0)
            {
                return Ok(result);
            }

            return NoContent(); //204 no data
        }

        [HttpGet("GetProductById/{id}")]
        public IActionResult GetProductById([FromRoute] int id)
        {
            ProductAllOutputDTO product = productService.GetProductById(id);

            if (product == null)
            {
                return NotFound(); // 404 notfound
            }

            return Ok(product);   //200 succeeded
        }




        [Authorize(Roles = "Admin")]
        [HttpPost("AddDTO")]
        public IActionResult AddDTO([FromBody] ProductInputDTO product)
        {
            int productId = productService.Create(product);

            return Ok(new { ProductId = productId }); //200, ProductId=1
            return Ok("product added successfully");  /// 200, product added successfully

        }


        //http://localhost:5153/product/UpdatePrice/3?newCount=200

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateCount/{productId}")]
        public IActionResult UpdateCount([FromRoute] int productId, [FromQuery] int newCount)
        {
            bool updated = productService.UpdateCount(productId, newCount);

            if (!updated)
                return NotFound();

            return Ok("Updated successfully");
            // return NoContent();
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{productId}")]
        public IActionResult Delete([FromRoute] int productId)
        {
            bool deleted = productService.Delete(productId);

            if (!deleted)
                return NotFound();

            return Ok("deleted successfully");
            //return NoContent();
        }
    }
}
