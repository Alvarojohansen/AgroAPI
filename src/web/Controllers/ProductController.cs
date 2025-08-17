using Application.Dtos.Product;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        private bool IsUserInRole(string role)
        {
            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role); // Obtener el claim de rol, si existe
            return roleClaim != null && roleClaim.Value == role; //Verificar si el claim existe y su valor es "role"
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            if (IsUserInRole("Admin") || (IsUserInRole("Client")))
            {
                var products = _productService.GetAllProducts();
                return Ok(products);
            }
            return Forbid();

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

            return Ok(updated);
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
