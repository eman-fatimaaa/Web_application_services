using System.ComponentModel.DataAnnotations;
namespace WebApplication1.DTOs;

public class CreatePublisherDto { [Required, StringLength(200, MinimumLength = 2)] public string Name { get; set; } = default!; }
public class UpdatePublisherDto { [Required, StringLength(200, MinimumLength = 2)] public string Name { get; set; } = default!; }

public record PublisherItemDto(int Id, string Name, DateTime CreatedAt);