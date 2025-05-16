using Microsoft.AspNetCore.Mvc;

namespace web.Controllers
{
    public class SellerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
