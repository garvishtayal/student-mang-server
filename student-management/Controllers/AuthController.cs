using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using student_management.Models;
using student_management.Services;
using System.Threading.Tasks;

namespace student_management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> SignIn([FromBody] LoginModel model)
        {
            var result = await _authService.SignInAsync(model);
            if (result.Success)
            {
                return Ok(result);
            }

            return Unauthorized(result);
        }

        //Only admins will be able to access this route
        
        [HttpPost("admin/addUser"), Authorize(Roles = "admin")]
        public async Task<IActionResult> AddUser([FromBody] RegisterModel model)
        {
            var result = await _authService.AddUserAsync(model);
            if (result.Succeeded)
            {
                return Ok(new { Message = "User added successfully" });
            }

            return BadRequest(result.Errors);
        }
    }
}


public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}

public class RegisterModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string StudentMail { get; set; }
}
public class AuthResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string Token { get; set; }
}