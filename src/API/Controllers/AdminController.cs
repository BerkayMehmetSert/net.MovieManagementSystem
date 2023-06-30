using Application.Contracts.Requests.User;
using Application.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IUserService _service;

    public AdminController(IUserService service)
    {
        _service = service;
    }

    [HttpPost("login")]
    public IActionResult AdminLogin([FromBody] AdminLoginRequest request)
    {
        var response = _service.AdminLogin(request);
        return Ok(response);
    }

    [HttpPost("register")]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminRegister([FromBody] AdminRegisterRequest request)
    {
        _service.AdminRegister(request);
        return Ok();
    }

    [HttpPut("change-password")]
    [Authorize(Roles = "Admin")]
    public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
    {
        _service.ChangePassword(request);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteUser(Guid id)
    {
        _service.DeleteUser(id);
        return Ok();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetUserById(Guid id)
    {
        var response = _service.GetUserById(id);
        return Ok(response);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAllUsers()
    {
        var response = _service.GetAllUsers();
        return Ok(response);
    }
}