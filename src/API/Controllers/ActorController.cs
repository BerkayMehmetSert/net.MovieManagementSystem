using Application.Contracts.Requests.Actor;
using Application.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ActorController : ControllerBase
{
    private readonly IActorService _service;

    public ActorController(IActorService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult CreateActor([FromBody] CreateActorRequest request)
    {
        _service.CreateActor(request);
        return Ok();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateActor(Guid id, [FromBody] UpdateActorRequest request)
    {
        _service.UpdateActor(id, request);
        return Ok();
    }

    [HttpPut("{id}/award")]
    [Authorize(Roles = "Admin")]
    public IActionResult AddActorAward(Guid id, [FromBody] AddActorAwardRequest request)
    {
        _service.AddActorAward(id, request);
        return Ok();
    }

    [HttpDelete("{id}/award")]
    [Authorize(Roles = "Admin")]
    public IActionResult RemoveActorAward(Guid id, [FromBody] RemoveActorAwardRequest request)
    {
        _service.RemoveActorAward(id, request);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteActor(Guid id)
    {
        _service.DeleteActor(id);
        return Ok();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetActorById(Guid id)
    {
        var response = _service.GetActorById(id);
        return Ok(response);
    }

    [HttpGet]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetAllActors()
    {
        var response = _service.GetAllActors();
        return Ok(response);
    }
}