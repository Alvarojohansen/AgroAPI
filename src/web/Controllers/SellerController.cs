using Microsoft.AspNetCore.Mvc;

namespace web.Controllers
{
    public class SellerController : Controller
    {
        [HttpPost]
        public IActionResult CreateNewMensagge()
        {
            return Ok("mensagge created");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMensagge(int id)
        {
            return Ok($"mensagge {id} updated");
        }
    }
}
