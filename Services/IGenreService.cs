using WebApplication1.Models;
namespace WebApplication1.Services;
public interface IGenreService
{
    Task<List<Genre>> GetAllAsync();
    Task<Genre?> GetByIdAsync(int id);
    Task<Genre> CreateAsync(string name);
    Task<Genre?> UpdateAsync(int id, string name);
    Task<bool> DeleteAsync(int id);
}