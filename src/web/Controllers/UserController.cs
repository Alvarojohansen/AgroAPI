using Microsoft.AspNetCore.Mvc;

namespace web.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
