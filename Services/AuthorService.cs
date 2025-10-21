using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Exceptions;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class AuthorService : IAuthorService
{
    private readonly AppDbContext _db;
    public AuthorService(AppDbContext db) => _db = db;

    public Task<List<Author>> GetAllAsync() =>
        _db.Authors
            .Include(a => a.Books)
            .OrderByDescending(a => a.Id)
            .ToListAsync();

    public Task<Author?> GetByIdAsync(int id) =>
        _db.Authors
            .Include(a => a.Books)
            .SingleOrDefaultAsync(a => a.Id == id);

    public async Task<Author> CreateAsync(string name)
    {
        var entity = new Author
        {
            Name = name,
            CreatedAt = DateTime.UtcNow
        };
        _db.Authors.Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task<Author?> UpdateAsync(int id, string name)
    {
        var entity = await _db.Authors.FindAsync(id);
        if (entity is null) throw new AppNotFoundException("Author not found.");

        entity.Name = name;
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Authors.FindAsync(id);
        if (entity is null) throw new AppNotFoundException("Author not found.");

        _db.Authors.Remove(entity); // ON DELETE CASCADE handles related books if configured
        await _db.SaveChangesAsync();
        return true;
    }
}