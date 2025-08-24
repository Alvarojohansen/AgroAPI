using Microsoft.AspNetCore.Mvc;

namespace web.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
