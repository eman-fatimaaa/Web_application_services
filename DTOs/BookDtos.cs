using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs;

// Requests
public class CreateBookDto
{
    [Required, StringLength(300, MinimumLength = 1)]
    public string Title { get; set; } = default!;

    [Range(1, int.MaxValue)]
    public int AuthorId { get; set; }
}

public class UpdateBookDto
{
    [Required, StringLength(300, MinimumLength = 1)]
    public string Title { get; set; } = default!;

    [Range(1, int.MaxValue)]
    public int AuthorId { get; set; }
}

// Responses
public record BookItemDto(int Id, string Title, int AuthorId, string AuthorName, DateTime CreatedAt);