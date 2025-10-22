using Application.Dtos.User;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace web.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _service;
        public UserController(IUserService service )
        {
            _service = service;
        }
        
        [HttpGet]
        public IActionResult GetAllUser() 
        {
            return Ok(_service.GetAllUser());
        }
        
        [HttpGet("{email}")]
        public IActionResult GetByEmail(string email)
        {
            var user = _service.GetUserbyEmail(email);
            return Ok(user);
        }
        
        [HttpGet("user/{Id}")]
        public IActionResult Get( int Id)
        {
            return Ok(_service.Get(Id));
        }
        [AllowAnonymous]
        [HttpPost("postUser/")]
        public IActionResult AddNewUser([FromBody] UserRequest body) 
        {
            
            if (body == null) 
            {
                return BadRequest("User vacio");
            }
            else 
            {
                var newUser = _service.AddUser(body);
                return Ok(newUser);
            }
                
        }
        
        [HttpPut("updateUser/{id}")]
        public IActionResult UpdateUser([FromRoute]int id, [FromBody] UserUpdateRequest user) 
        {
            var updated = _service.UpdateUser(id, user);
            if (!updated)
                return NotFound($"No se encontró un usuario con ID {id}");
            return Ok("Usuario actualizado con exito.");

        }
       
        [HttpPut("updateRoleUser/{id}")]
        public IActionResult UpdateRoleUser([FromRoute] int id, [FromBody] UserUpdateRoleRequest role)
        {
            var updated = _service.UpdateRoleUser(id, role);
            if (!updated)
                return NotFound($"No se encontró un usuario con ID {id}");
            return Ok("Role actualizado con exito.");
        }


        [HttpDelete("deleteUser/{id}")]
        public IActionResult DeleteUser([FromRoute] int id) 
        {
            var user = _service.Get(id);
            if (user == null) 
            {
                return NotFound("No se encontró un usuario con ese ID");

            }
            _service.DeleteUser(id);
            return Ok("Usuario eliminado con exito.");
        }

        
    }
}
