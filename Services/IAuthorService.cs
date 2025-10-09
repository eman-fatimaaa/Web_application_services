using WebApplication1.Models;

namespace WebApplication1.Services;

public interface IAuthorService
{
    Task<List<Author>> GetAllAsync();
    Task<Author?> GetByIdAsync(int id);              // with Books loaded if you want (we'll not need it for list)
    Task<Author> CreateAsync(string name);
    Task<Author?> UpdateAsync(int id, string name);  // null if not found
    Task<bool> DeleteAsync(int id);                  // false if not found
}