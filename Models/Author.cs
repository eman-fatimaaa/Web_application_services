namespace WebApplication1.Models;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public DateTime CreatedAt { get; set; }

    // 1:N navigation
    public ICollection<Book> Books { get; set; } = new List<Book>();
}