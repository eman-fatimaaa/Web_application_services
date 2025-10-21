namespace WebApplication1.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public DateTime CreatedAt { get; set; }

    public int AuthorId { get; set; }
    public Author Author { get; set; } = default!;

    public int PublisherId { get; set; }
    public Publisher Publisher { get; set; } = default!;

    public ICollection<Genre> Genres { get; set; } = new List<Genre>();
}