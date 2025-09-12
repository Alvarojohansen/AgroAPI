using Application.Dtos.Product;
using Application.Interfaces;
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
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
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
                var products = _productService.GetAllProducts();
                return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(_productService.GetProductById(id));
        }

        [HttpPost("/addProduct")]
        public IActionResult AddProduct([FromBody] ProductRequest product)
        {
            if ( IsUserInRole("Seller"))
            {
                if (product == null)
                {
                    return BadRequest("Product cannot be null");
                }
                return Ok(_productService.AddProduct(product));
            }
            
            return Forbid();

        }

        [HttpPut("updateProduct/{id}")]
        public IActionResult UpdateProduct([FromRoute] int id, [FromBody] ProductUpdateRequest product)
        {
            if (IsUserInRole("Admin") || (IsUserInRole("Seller")))
            {
                var updated = _productService.UpdateProduct(id, product);

                if (!updated)
                    return NotFound($"No se encontró un producto con ID {id}");

                return Ok(updated);
            }
            return Forbid();
        }
        

        [HttpDelete("deleteProduct/{id}")]
        public IActionResult DeleteProduct([FromRoute] int id)
        {
            if (IsUserInRole("Admin") || (IsUserInRole("Seller")))
            {
                var product = _productService.GetProductById(id);
                if (product == null)
                {
                    return NotFound($"No se encontró un producto con ID {id}");
                }
                _productService.DeleteProduct(id);
                return Ok("Product deleted successfully.");
            }
            return Forbid();
        }
    }
}
