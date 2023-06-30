using Application.Contracts.Requests.Director;
using Application.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DirectorController : ControllerBase
{
    private readonly IDirectorService _service;

    public DirectorController(IDirectorService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult CreateDirector([FromBody] CreateDirectorRequest request)
    {
        _service.CreateDirector(request);
        return Ok();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateDirector(Guid id, [FromBody] UpdateDirectorRequest request)
    {
        _service.UpdateDirector(id, request);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteDirector(Guid id)
    {
        _service.DeleteDirector(id);
        return Ok();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetDirectorById(Guid id)
    {
        var response = _service.GetDirectorById(id);
        return Ok(response);
    }

    [HttpGet]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetAllDirectors()
    {
        var response = _service.GetAllDirectors();
        return Ok(response);
    }
}