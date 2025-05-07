using Microsoft.AspNetCore.Mvc;

namespace web.Controllers
{
    public class ProductController : Controller
    {
        [HttpGet]
        public IActionResult GetAll() 
        { 
            return Ok();
        }
    }
}
