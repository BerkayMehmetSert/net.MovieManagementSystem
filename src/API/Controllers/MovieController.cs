using Application.Contracts.Requests.Movie;
using Application.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _service;

    public MovieController(IMovieService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult CreateMovie([FromBody] CreateMovieRequest request)
    {
        _service.CreateMovie(request);
        return Ok();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateMovie(Guid id, [FromBody] UpdateMovieRequest request)
    {
        _service.UpdateMovie(id, request);
        return Ok();
    }

    [HttpPatch("{id}/actor")]
    [Authorize(Roles = "Admin")]
    public IActionResult AddMovieActor(Guid id, [FromBody] AddMovieActorRequest request)
    {
        _service.AddMovieActor(id, request);
        return Ok();
    }

    [HttpPatch("{id}/cinema")]
    [Authorize(Roles = "Admin")]
    public IActionResult AddMovieCinema(Guid id, [FromBody] AddMovieCinemaRequest request)
    {
        _service.AddMovieCinema(id, request);
        return Ok();
    }

    [HttpPatch("{id}/director")]
    [Authorize(Roles = "Admin")]
    public IActionResult AddMovieDirector(Guid id, [FromBody] AddMovieDirectorRequest request)
    {
        _service.AddMovieDirector(id, request);
        return Ok();
    }

    [HttpPatch("{id}/genre")]
    [Authorize(Roles = "Admin")]
    public IActionResult AddMovieGenre(Guid id, [FromBody] AddMovieGenreRequest request)
    {
        _service.AddMovieGenre(id, request);
        return Ok();
    }

    [HttpPatch("{id}/language")]
    [Authorize(Roles = "Admin")]
    public IActionResult AddMovieLanguage(Guid id, [FromBody] AddMovieLanguageRequest request)
    {
        _service.AddMovieLanguage(id, request);
        return Ok();
    }

    [HttpPatch("{id}/rating")]
    [Authorize(Roles = "Admin")]
    public IActionResult AddMovieRating(Guid id, [FromBody] AddMovieRatingRequest request)
    {
        _service.AddMovieRating(id, request);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteMovie(Guid id)
    {
        _service.DeleteMovie(id);
        return Ok();
    }

    [HttpDelete("{id}/actor")]
    [Authorize(Roles = "Admin")]
    public IActionResult RemoveMovieActor(Guid id, [FromBody] RemoveMovieActorRequest request)
    {
        _service.RemoveMovieActor(id, request);
        return Ok();
    }

    [HttpDelete("{id}/cinema")]
    [Authorize(Roles = "Admin")]
    public IActionResult RemoveMovieCinema(Guid id, [FromBody] RemoveMovieCinemaRequest request)
    {
        _service.RemoveMovieCinema(id, request);
        return Ok();
    }

    [HttpDelete("{id}/director")]
    [Authorize(Roles = "Admin")]
    public IActionResult RemoveMovieDirector(Guid id, [FromBody] RemoveMovieDirectorRequest request)
    {
        _service.RemoveMovieDirector(id, request);
        return Ok();
    }

    [HttpDelete("{id}/genre")]
    [Authorize(Roles = "Admin")]
    public IActionResult RemoveMovieGenre(Guid id, [FromBody] RemoveMovieGenreRequest request)
    {
        _service.RemoveMovieGenre(id, request);
        return Ok();
    }

    [HttpDelete("{id}/language")]
    [Authorize(Roles = "Admin")]
    public IActionResult RemoveMovieLanguage(Guid id, [FromBody] RemoveMovieLanguageRequest request)
    {
        _service.RemoveMovieLanguage(id, request);
        return Ok();
    }

    [HttpDelete("{id}/rating")]
    [Authorize(Roles = "Admin")]
    public IActionResult RemoveMovieRating(Guid id, [FromBody] RemoveMovieRatingRequest request)
    {
        _service.RemoveMovieRating(id, request);
        return Ok();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetMovieById(Guid id)
    {
        var response = _service.GetMovieById(id);
        return Ok(response);
    }

    [HttpGet("title/{title}")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetMovieByTitle(string title)
    {
        var response = _service.GetMovieByTitle(title);
        return Ok(response);
    }

    [HttpGet]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetAllMovies()
    {
        var response = _service.GetAllMovies();
        return Ok(response);
    }
}