using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MembersController : ControllerBase
{
    private readonly IMemberService _svc;
    public MembersController(IMemberService svc) => _svc = svc;

    [HttpPost]
    [ProducesResponseType(typeof(MemberItemDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<MemberItemDto>> Create([FromBody] CreateMemberDto body)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var e = await _svc.CreateAsync(body.FullName, body.Email);
        var dto = new MemberItemDto(e.Id, e.FullName, e.Email, e.CreatedAt);
        return Created($"/api/members/{dto.Id}", dto);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MemberItemDto>), 200)]
    public async Task<ActionResult<IEnumerable<MemberItemDto>>> GetAll()
    {
        var list = await _svc.GetAllAsync();
        return Ok(list.Select(x => new MemberItemDto(x.Id, x.FullName, x.Email, x.CreatedAt)));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MemberItemDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<MemberItemDto>> GetOne(int id)
    {
        var e = await _svc.GetByIdAsync(id);
        return e is null ? NotFound() : Ok(new MemberItemDto(e.Id, e.FullName, e.Email, e.CreatedAt));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(MemberItemDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<MemberItemDto>> Update(int id, [FromBody] UpdateMemberDto body)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var e = await _svc.UpdateAsync(id, body.FullName, body.Email);
        return e is null ? NotFound() : Ok(new MemberItemDto(e.Id, e.FullName, e.Email, e.CreatedAt));
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
