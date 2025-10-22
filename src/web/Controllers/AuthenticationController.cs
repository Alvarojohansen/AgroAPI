using Application.Dtos.Authorize;
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
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
        private readonly IConfiguration _config;
        private readonly IAuthenticationService _customAuthenticationService;

        public AuthenticationController(IConfiguration config, IAuthenticationService authenticateService)
        {
            _config = config;
            _customAuthenticationService = authenticateService;
        }

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate([FromBody] CredentialsRequest credentials)
        {
            try
            {
                string token = _customAuthenticationService.Authenticate(credentials);
                return Ok(token);
            }
            catch (InvalidOperationException)
            {
                return Unauthorized(new { message = "Credenciales inválidas. Por favor, verifica tu email y contraseña." });
            }
        }
    }
}
