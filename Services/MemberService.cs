using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Services;
public class MemberService : IMemberService
{
    private readonly AppDbContext _db;
    public MemberService(AppDbContext db) => _db = db;

    public Task<List<Member>> GetAllAsync() =>
        _db.Members.OrderByDescending(x => x.Id).ToListAsync();

    public Task<Member?> GetByIdAsync(int id) =>
        _db.Members.SingleOrDefaultAsync(x => x.Id == id);

    public async Task<Member> CreateAsync(string fullName, string email)
    {
        var e = new Member { FullName = fullName, Email = email, CreatedAt = DateTime.UtcNow };
        _db.Members.Add(e); await _db.SaveChangesAsync(); return e;
    }

    public async Task<Member?> UpdateAsync(int id, string fullName, string email)
    {
        var e = await _db.Members.FindAsync(id); if (e is null) return null;
        e.FullName = fullName; e.Email = email; await _db.SaveChangesAsync(); return e;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.Members.FindAsync(id); if (e is null) return false;
        _db.Members.Remove(e); await _db.SaveChangesAsync(); return true;
    }
}