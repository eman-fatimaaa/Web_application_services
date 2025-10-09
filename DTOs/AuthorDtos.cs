using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs;

// Requests
public class CreateAuthorDto
{
    [Required, StringLength(200, MinimumLength = 2)]
    public string Name { get; set; } = default!;
}

public class UpdateAuthorDto
{
    [Required, StringLength(200, MinimumLength = 2)]
    public string Name { get; set; } = default!;
}

// Responses
public record AuthorListDto(int Id, string Name, DateTime CreatedAt);
public record AuthorDetailDto(int Id, string Name, DateTime CreatedAt, int BooksCount);