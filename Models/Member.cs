namespace WebApplication1.Models;
public class Member
{
    public int Id { get; set; }
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}