using Microsoft.AspNetCore.Mvc;

namespace web.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
