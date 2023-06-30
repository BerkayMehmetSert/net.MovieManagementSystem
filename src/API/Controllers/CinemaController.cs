using Application.Contracts.Requests.Cinema;
using Application.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CinemaController : ControllerBase
{
    private readonly ICinemaService _service;

    public CinemaController(ICinemaService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult CreateCinema([FromBody] CreateCinemaRequest request)
    {
        _service.CreateCinema(request);
        return Ok();
    }

    [HttpPut("{id}/address")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateCinemaAddress(Guid id, [FromBody] UpdateCinemaAddressRequest request)
    {
        _service.UpdateCinemaAddress(id, request);
        return Ok();
    }

    [HttpPut("{id}/name")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateCinemaName(Guid id, [FromBody] UpdateCinemaNameRequest request)
    {
        _service.UpdateCinemaName(id, request);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteCinema(Guid id)
    {
        _service.DeleteCinema(id);
        return Ok();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetCinemaById(Guid id)
    {
        var response = _service.GetCinemaById(id);
        return Ok(response);
    }

    [HttpGet("name/{name}")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetCinemaByName(string name)
    {
        var response = _service.GetCinemaByName(name);
        return Ok(response);
    }

    [HttpGet]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetAllCinemas()
    {
        var response = _service.GetAllCinemas();
        return Ok(response);
    }
}