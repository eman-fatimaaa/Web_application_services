using System.ComponentModel.DataAnnotations;
namespace WebApplication1.DTOs;

public class CreateMemberDto
{
    [Required, StringLength(200, MinimumLength = 2)] public string FullName { get; set; } = default!;
    [Required, EmailAddress, StringLength(255)]     public string Email { get; set; } = default!;
}
public class UpdateMemberDto
{
    [Required, StringLength(200, MinimumLength = 2)] public string FullName { get; set; } = default!;
    [Required, EmailAddress, StringLength(255)]     public string Email { get; set; } = default!;
}

public record MemberItemDto(int Id, string FullName, string Email, DateTime CreatedAt);