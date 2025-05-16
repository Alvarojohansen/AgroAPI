using Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        [HttpPost]
        public IActionResult Authenticate([FromBody] CredentialsRequest credentials)
        {
            return Ok("jwt");
        }
    }
}
