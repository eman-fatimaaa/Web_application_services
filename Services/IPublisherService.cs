using WebApplication1.Models;
namespace WebApplication1.Services;
public interface IPublisherService
{
    Task<List<Publisher>> GetAllAsync();
    Task<Publisher?> GetByIdAsync(int id);
    Task<Publisher> CreateAsync(string name);
    Task<Publisher?> UpdateAsync(int id, string name);
    Task<bool> DeleteAsync(int id);
}