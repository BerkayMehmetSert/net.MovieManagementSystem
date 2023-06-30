using Application.Contracts.Requests.Genre;
using Application.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class GenreController : ControllerBase
{
    private readonly IGenreService _service;

    public GenreController(IGenreService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult CreateGenre([FromBody] CreateGenreRequest request)
    {
        _service.CreateGenre(request);
        return Ok();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateGenre(Guid id, [FromBody] UpdateGenreRequest request)
    {
        _service.UpdateGenre(id, request);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteGenre(Guid id)
    {
        _service.DeleteGenre(id);
        return Ok();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetGenreById(Guid id)
    {
        var response = _service.GetGenreById(id);
        return Ok(response);
    }

    [HttpGet("name/{name}")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetGenreByName(string name)
    {
        var response = _service.GetGenreByName(name);
        return Ok(response);
    }

    [HttpGet]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetAllGenres()
    {
        var response = _service.GetAllGenres();
        return Ok(response);
    }
}