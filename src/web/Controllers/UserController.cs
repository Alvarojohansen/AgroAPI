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

        [HttpPut("updateUser/{id}")]
        public IActionResult UpdateUser([FromRoute]int id, [FromBody] UserUpdateRequest user) 
        {
            _service.UpdateUser(id, user);
            return Ok("Usuario actualizado correctamente.");

        }

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
