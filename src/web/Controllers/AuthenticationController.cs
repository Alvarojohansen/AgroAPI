using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] CredentialsRequest credentials)
        {
            var userLogged = _userService.ValidationCredentials(credentials);
            if (userLogged == null)
            {
                return Unauthorized();
            }
            return Ok(userLogged);

        }
    }
}
