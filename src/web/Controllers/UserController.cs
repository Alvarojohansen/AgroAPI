using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace web.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _service;
        public UserController(UserService service )
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult GetAllUser() 
        {
            return Ok(_service.GetAllUser());
        }
        [HttpGet("{name}")]
        public IActionResult Get( string name)
        {
            return Ok(_service.Get(name));
        }

        [HttpPost("postUser/")]
        
        public IActionResult AddNewUser([FromBody] UserDtosRequest body) 
        {
            return Ok(_service.AddUser(body));
        }
        
    }
}
