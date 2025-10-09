namespace WebApplication1.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public DateTime CreatedAt { get; set; }

    // FK + back-reference
    public int AuthorId { get; set; }
    public Author Author { get; set; } = default!;
}