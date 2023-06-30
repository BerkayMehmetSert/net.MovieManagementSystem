using Application.Contracts.Requests.User;
using Application.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }
    
    [HttpPost("login")]
    public IActionResult UserLogin([FromBody] UserLoginRequest request)
    {
        var response = _service.UserLogin(request);
        return Ok(response);
    }
    
    [HttpPost("register")]
    public IActionResult UserRegister([FromBody] UserRegisterRequest request)
    {
        _service.UserRegister(request);
        return Ok();
    }
    
    [HttpPut("change-password")]
    [Authorize(Roles = "User")]
    public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
    {
        _service.ChangePassword(request);
        return Ok();
    }
}