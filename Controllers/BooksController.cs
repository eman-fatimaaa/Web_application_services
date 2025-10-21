using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Services;
using WebApplication1.Exceptions; // <— add
using WebApplication1.Filters;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BooksController : ControllerBase
{
    private readonly IBookService _books;
    public BooksController(IBookService books) => _books = books;

    // CREATE
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(BookItemDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<BookItemDto>> Create([FromBody] CreateBookDto body)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        try
        {
            var entity = await _books.CreateAsync(body.Title, body.AuthorId);
            var withAuthor = await _books.GetByIdWithAuthorAsync(entity.Id)!;

            var dto = new BookItemDto(withAuthor.Id, withAuthor.Title, withAuthor.AuthorId, withAuthor.Author.Name, withAuthor.CreatedAt);
            return Created($"/api/books/{dto.Id}", dto);
        }
        catch (InvalidForeignKeyException ex) // <— changed
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    [HttpGet("secure-ping")]
    [RequireHeader("X-Client")]
    public IActionResult SecurePing() => Ok(new { ok = true, serverTime = DateTime.UtcNow });

    // READ ALL
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BookItemDto>), 200)]
    public async Task<ActionResult<IEnumerable<BookItemDto>>> GetAll()
    {
        var list = await _books.GetAllWithAuthorsAsync();
        var dto = list.Select(b => new BookItemDto(b.Id, b.Title, b.AuthorId, b.Author.Name, b.CreatedAt));
        return Ok(dto);
    }

    // READ ONE
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BookItemDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<BookItemDto>> GetOne(int id)
    {
        var entity = await _books.GetByIdWithAuthorAsync(id);
        if (entity is null) return NotFound();

        var dto = new BookItemDto(entity.Id, entity.Title, entity.AuthorId, entity.Author.Name, entity.CreatedAt);
        return Ok(dto);
    }

    // UPDATE
    [HttpPut("{id:int}")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(BookItemDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<BookItemDto>> Update(int id, [FromBody] UpdateBookDto body)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        try
        {
            var entity = await _books.UpdateAsync(id, body.Title, body.AuthorId);
            if (entity is null) return NotFound();

            var withAuthor = await _books.GetByIdWithAuthorAsync(entity.Id)!;
            var dto = new BookItemDto(withAuthor.Id, withAuthor.Title, withAuthor.AuthorId, withAuthor.Author.Name, withAuthor.CreatedAt);
            return Ok(dto);
        }
        catch (InvalidForeignKeyException ex) // <— changed
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // DELETE
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _books.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }
}
