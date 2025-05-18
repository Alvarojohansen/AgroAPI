using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly UserService _userService;
        public AuthenticationController(UserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public IActionResult Authenticate([FromBody] CredentialsRequest credentials)
        {
            UserModel? userLogged = _userService.ValidationCredentials(credentials);
            if (userLogged == null) 
            {
                return Ok("jwt");
            }return Unauthorized();
            
        }
    }
}
