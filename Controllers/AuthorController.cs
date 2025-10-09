using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authors;
    public AuthorsController(IAuthorService authors) => _authors = authors;

    // CREATE
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(AuthorDetailDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<AuthorDetailDto>> Create([FromBody] CreateAuthorDto body)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var entity = await _authors.CreateAsync(body.Name);
        var dto = new AuthorDetailDto(entity.Id, entity.Name, entity.CreatedAt, 0);
        return Created($"/api/authors/{dto.Id}", dto);
    }

    // READ ALL
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AuthorListDto>), 200)]
    public async Task<ActionResult<IEnumerable<AuthorListDto>>> GetAll()
    {
        var list = await _authors.GetAllAsync();
        var dto = list.Select(a => new AuthorListDto(a.Id, a.Name, a.CreatedAt));
        return Ok(dto);
    }

    // READ ONE
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AuthorDetailDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<AuthorDetailDto>> GetOne(int id)
    {
        var entity = await _authors.GetByIdAsync(id);
        if (entity is null) return NotFound();

        var dto = new AuthorDetailDto(entity.Id, entity.Name, entity.CreatedAt, entity.Books.Count);
        return Ok(dto);
    }

    // UPDATE
    [HttpPut("{id:int}")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(AuthorDetailDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<AuthorDetailDto>> Update(int id, [FromBody] UpdateAuthorDto body)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var entity = await _authors.UpdateAsync(id, body.Name);
        if (entity is null) return NotFound();

        var dto = new AuthorDetailDto(entity.Id, entity.Name, entity.CreatedAt, entity.Books.Count);
        return Ok(dto);
    }

    // DELETE
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _authors.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }
}
