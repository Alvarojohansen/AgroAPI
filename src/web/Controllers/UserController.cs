using Application.Dtos.User;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace web.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserService _service;
        public UserController(UserService service )
        {
            _service = service;
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetAllUser() 
        {
            return Ok(_service.GetAllUser());
        }
        [Authorize]
        [HttpGet("{email}")]
        public IActionResult GetByEmail(string email)
        {
            return Ok(_service.GetUserbyEmail(email));
        }
        [Authorize]
        [HttpGet("{Id}")]
        public IActionResult Get( int Id)
        {
            return Ok(_service.Get(Id));
        }

        [HttpPost("postUser/")]
        public IActionResult AddNewUser([FromBody] UserRequest body) 
        {
            return Ok(_service.AddUser(body));
        }
        [Authorize]
        [HttpPut("updateUser/{id}")]
        public IActionResult UpdateUser([FromRoute]int id, [FromBody] UserUpdateRequest user) 
        {
            var updated = _service.UpdateUser(id, user);
            if (!updated)
                return NotFound($"No se encontró un usuario con ID {id}");
            return Ok("User updated successfully.");

        }
        [Authorize]
        [HttpDelete("deleteUser/{id}")]
        public IActionResult DeleteUser([FromRoute] int id) 
        {
            var user = _service.Get(id);
            if (user == null) 
            {
                return NotFound();

            }
            _service.DeleteUser(id);
            return Ok();
        }

        
    }
}
