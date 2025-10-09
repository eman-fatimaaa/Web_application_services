using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class GenresController : ControllerBase
{
    private readonly IGenreService _svc;
    public GenresController(IGenreService svc) => _svc = svc;

    [HttpPost]
    [ProducesResponseType(typeof(GenreItemDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<GenreItemDto>> Create([FromBody] CreateGenreDto body)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var e = await _svc.CreateAsync(body.Name);
        var dto = new GenreItemDto(e.Id, e.Name, e.CreatedAt);
        return Created($"/api/genres/{dto.Id}", dto);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GenreItemDto>), 200)]
    public async Task<ActionResult<IEnumerable<GenreItemDto>>> GetAll()
    {
        var list = await _svc.GetAllAsync();
        return Ok(list.Select(x => new GenreItemDto(x.Id, x.Name, x.CreatedAt)));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GenreItemDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<GenreItemDto>> GetOne(int id)
    {
        var e = await _svc.GetByIdAsync(id);
        return e is null ? NotFound() : Ok(new GenreItemDto(e.Id, e.Name, e.CreatedAt));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(GenreItemDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<GenreItemDto>> Update(int id, [FromBody] UpdateGenreDto body)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var e = await _svc.UpdateAsync(id, body.Name);
        return e is null ? NotFound() : Ok(new GenreItemDto(e.Id, e.Name, e.CreatedAt));
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
