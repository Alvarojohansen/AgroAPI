using Microsoft.AspNetCore.Mvc;

namespace web.Controllers
{
    public class SaleOrderLineController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
