using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PublishersController : ControllerBase
{
    private readonly IPublisherService _svc;
    public PublishersController(IPublisherService svc) => _svc = svc;

    [HttpPost]
    [ProducesResponseType(typeof(PublisherItemDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<PublisherItemDto>> Create([FromBody] CreatePublisherDto body)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var e = await _svc.CreateAsync(body.Name);
        var dto = new PublisherItemDto(e.Id, e.Name, e.CreatedAt);
        return Created($"/api/publishers/{dto.Id}", dto);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PublisherItemDto>), 200)]
    public async Task<ActionResult<IEnumerable<PublisherItemDto>>> GetAll()
    {
        var list = await _svc.GetAllAsync();
        return Ok(list.Select(x => new PublisherItemDto(x.Id, x.Name, x.CreatedAt)));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PublisherItemDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<PublisherItemDto>> GetOne(int id)
    {
        var e = await _svc.GetByIdAsync(id);
        return e is null ? NotFound() : Ok(new PublisherItemDto(e.Id, e.Name, e.CreatedAt));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(PublisherItemDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<PublisherItemDto>> Update(int id, [FromBody] UpdatePublisherDto body)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var e = await _svc.UpdateAsync(id, body.Name);
        return e is null ? NotFound() : Ok(new PublisherItemDto(e.Id, e.Name, e.CreatedAt));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _svc.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }
}
