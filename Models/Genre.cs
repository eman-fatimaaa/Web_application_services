namespace WebApplication1.Models;
public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    
    public ICollection<Book> Books { get; set; } = new List<Book>();

}