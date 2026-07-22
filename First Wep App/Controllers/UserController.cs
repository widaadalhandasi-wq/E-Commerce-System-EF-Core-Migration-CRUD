using FirstWebApp.DTOs;
using FirstWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebApp.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private UserService userService;

        public UserController(UserService _userService)
        {
            userService = _userService;
        }

        // POST user/register
        // Public — no token required
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            UserResponseDto created = userService.Register(dto);

            if (created == null)
                return BadRequest(new { message = "Email is already registered." });

            return Ok(created);
        }

        // POST user/login
        // Public — returns the JWT token on success
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            LoginResponseDto result = userService.Login(dto);

            if (result == null)
                return Unauthorized(new { message = "Invalid email or password." });

            return Ok(result);
        }

        // GET user/GetUserData/3
        // Protected — any authenticated user (Buyer or Seller)
        [HttpGet("GetUserData/{id}")]
        [Authorize(Roles = "Buyer,Seller")]
        public IActionResult GetUserData(int id)
        {
            UserResponseDto user = userService.GetById(id);

            if (user == null)
                return NotFound(new { message = $"User with ID {id} was not found." });

            return Ok(user);
        }

        // PUT user/UpdateUserData/3
        // Protected — any authenticated user (Buyer or Seller)
        [HttpPut("UpdateUserData/{id}")]
        [Authorize(Roles = "Buyer,Seller")]
        public IActionResult UpdateUserData(int id, [FromBody] UpdateUserDto dto)
        {
            UserResponseDto updated = userService.Update(id, dto);

            if (updated == null)
                return NotFound(new { message = $"User with ID {id} was not found." });

            return Ok(updated);
        }

        // DELETE user/DeleteUser/3
        // Protected — any authenticated user (Buyer or Seller)
        [HttpDelete("DeleteUser/{id}")]
        [Authorize(Roles = "Buyer,Seller")]
        public IActionResult DeleteUser(int id)
        {
            bool deleted = userService.Delete(id);

            if (!deleted)
                return NotFound(new { message = $"User with ID {id} was not found." });

            return NoContent();
        }
    }
}
