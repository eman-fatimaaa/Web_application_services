using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class BookService : IBookService
{
    private readonly AppDbContext _db;
    public BookService(AppDbContext db) => _db = db;

    public Task<List<Book>> GetAllWithAuthorsAsync() =>
        _db.Books.Include(b => b.Author)
            .OrderByDescending(b => b.Id)
            .ToListAsync();

    public Task<Book?> GetByIdWithAuthorAsync(int id) =>
        _db.Books.Include(b => b.Author)
            .SingleOrDefaultAsync(b => b.Id == id);

    public async Task<Book> CreateAsync(string title, int authorId)
    {
        var exists = await _db.Authors.AnyAsync(a => a.Id == authorId);
        if (!exists) throw new ArgumentException("AuthorId not found", nameof(authorId));

        var entity = new Book { Title = title, AuthorId = authorId, CreatedAt = DateTime.UtcNow };
        _db.Books.Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task<Book?> UpdateAsync(int id, string title, int authorId)
    {
        var entity = await _db.Books.FindAsync(id);
        if (entity is null) return null;

        var exists = await _db.Authors.AnyAsync(a => a.Id == authorId);
        if (!exists) throw new ArgumentException("AuthorId not found", nameof(authorId));

        entity.Title = title;
        entity.AuthorId = authorId;
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Books.FindAsync(id);
        if (entity is null) return false;
        _db.Books.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}