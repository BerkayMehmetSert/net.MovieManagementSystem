using Application.Contracts.Requests.Rating;
using Application.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class RatingController : ControllerBase
{
    private readonly IRatingService _service;

    public RatingController(IRatingService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult CreateRating([FromBody] CreateRatingRequest request)
    {
        _service.CreateRating(request);
        return Ok();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateRating(Guid id, [FromBody] UpdateRatingRequest request)
    {
        _service.UpdateRating(id, request);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteRating(Guid id)
    {
        _service.DeleteRating(id);
        return Ok();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetRatingById(Guid id)
    {
        var response = _service.GetRatingById(id);
        return Ok(response);
    }

    [HttpGet]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetAllRatings()
    {
        var response = _service.GetAllRatings();
        return Ok(response);
    }
}