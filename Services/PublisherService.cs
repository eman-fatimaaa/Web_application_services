using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Services;
public class PublisherService : IPublisherService
{
    private readonly AppDbContext _db;
    public PublisherService(AppDbContext db) => _db = db;

    public Task<List<Publisher>> GetAllAsync() =>
        _db.Publishers.OrderByDescending(x => x.Id).ToListAsync();

    public Task<Publisher?> GetByIdAsync(int id) =>
        _db.Publishers.SingleOrDefaultAsync(x => x.Id == id);

    public async Task<Publisher> CreateAsync(string name)
    {
        var e = new Publisher { Name = name, CreatedAt = DateTime.UtcNow };
        _db.Publishers.Add(e); await _db.SaveChangesAsync(); return e;
    }

    public async Task<Publisher?> UpdateAsync(int id, string name)
    {
        var e = await _db.Publishers.FindAsync(id); if (e is null) return null;
        e.Name = name; await _db.SaveChangesAsync(); return e;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.Publishers.FindAsync(id); if (e is null) return false;
        _db.Publishers.Remove(e); await _db.SaveChangesAsync(); return true;
    }
}