using Microsoft.AspNetCore.Mvc;

namespace web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
