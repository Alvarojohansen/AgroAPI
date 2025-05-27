using Application.Dtos.Product;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        public ProductController(ProductService productService)
        {
            _productService = productService;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_productService.GetAllProducts());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(_productService.GetProductById(id));
        }

        [HttpPost("/addProduct")]
        public IActionResult AddProduct([FromBody] ProductRequest product)
        {
            if (product == null)
            {
                return BadRequest("Product cannot be null");
            }
            // Assuming you have a method to add a product in your service
            return Ok(_productService.AddProduct(product));

        }

        [HttpPut("updateProduct/{id}")]
        public IActionResult UpdateProduct([FromRoute] int id, [FromBody] ProductUpdateRequest product)
        {

            var updated = _productService.UpdateProduct(id, product);

            if (!updated)
                return NotFound($"No se encontró un producto con ID {id}");

            return Ok("Product updated successfully.");
        }

        [HttpDelete("deleteProduct/{id}")]
        public IActionResult DeleteProduct([FromRoute] int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound($"No se encontró un producto con ID {id}");
            }
            _productService.DeleteProduct(id);
            return Ok("Product deleted successfully.");
        }
    }
}
