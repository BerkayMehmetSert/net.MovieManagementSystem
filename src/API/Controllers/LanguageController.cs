using Application.Contracts.Requests.Language;
using Application.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class LanguageController : ControllerBase
{
    private readonly ILanguageService _service;

    public LanguageController(ILanguageService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult CreateLanguage([FromBody] CreateLanguageRequest request)
    {
        _service.CreateLanguage(request);
        return Ok();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateLanguage(Guid id, [FromBody] UpdateLanguageRequest request)
    {
        _service.UpdateLanguage(id, request);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteLanguage(Guid id)
    {
        _service.DeleteLanguage(id);
        return Ok();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetLanguageById(Guid id)
    {
        var response = _service.GetLanguageById(id);
        return Ok(response);
    }

    [HttpGet("name/{name}")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetLanguageByName(string name)
    {
        var response = _service.GetLanguageByName(name);
        return Ok(response);
    }

    [HttpGet]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetAllLanguages()
    {
        var response = _service.GetAllLanguages();
        return Ok(response);
    }
}