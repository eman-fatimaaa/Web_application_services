using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Services;
public class GenreService : IGenreService
{
    private readonly AppDbContext _db;
    public GenreService(AppDbContext db) => _db = db;

    public Task<List<Genre>> GetAllAsync() =>
        _db.Genres.OrderByDescending(x => x.Id).ToListAsync();

    public Task<Genre?> GetByIdAsync(int id) =>
        _db.Genres.SingleOrDefaultAsync(x => x.Id == id);

    public async Task<Genre> CreateAsync(string name)
    {
        var e = new Genre { Name = name, CreatedAt = DateTime.UtcNow };
        _db.Genres.Add(e); await _db.SaveChangesAsync(); return e;
    }

    public async Task<Genre?> UpdateAsync(int id, string name)
    {
        var e = await _db.Genres.FindAsync(id); if (e is null) return null;
        e.Name = name; await _db.SaveChangesAsync(); return e;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.Genres.FindAsync(id); if (e is null) return false;
        _db.Genres.Remove(e); await _db.SaveChangesAsync(); return true;
    }
}