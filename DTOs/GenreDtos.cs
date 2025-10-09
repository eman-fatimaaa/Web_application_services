using System.ComponentModel.DataAnnotations;
namespace WebApplication1.DTOs;

public class CreateGenreDto { [Required, StringLength(200, MinimumLength = 2)] public string Name { get; set; } = default!; }
public class UpdateGenreDto { [Required, StringLength(200, MinimumLength = 2)] public string Name { get; set; } = default!; }

public record GenreItemDto(int Id, string Name, DateTime CreatedAt);