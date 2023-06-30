using Application.Contracts.Requests.Award;
using Application.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AwardController : ControllerBase
{
    private readonly IAwardService _service;

    public AwardController(IAwardService service)
    {
        _service = service;
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult CreateAward([FromBody] CreateAwardRequest request)
    {
        _service.CreateAward(request);
        return Ok();
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateAward(Guid id, [FromBody] UpdateAwardRequest request)
    {
        _service.UpdateAward(id, request);
        return Ok();
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteAward(Guid id)
    {
        _service.DeleteAward(id);
        return Ok();
    }
    
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetAwardById(Guid id)
    {
        var response = _service.GetAwardById(id);
        return Ok(response);
    }
    
    [HttpGet("name/{name}")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetAwardByName(string name)
    {
        var response = _service.GetAwardByName(name);
        return Ok(response);
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetAllAwards()
    {
        var response = _service.GetAllAwards();
        return Ok(response);
    }
    
    [HttpGet("date/asc")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetAllAwardsOrderedByDateAsc()
    {
        var response = _service.GetAllAwardsOrderedByDateAsc();
        return Ok(response);
    }
    
    [HttpGet("date/desc")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetAllAwardsOrderedByDateDesc()
    {
        var response = _service.GetAllAwardsOrderedByDateDesc();
        return Ok(response);
    }
}