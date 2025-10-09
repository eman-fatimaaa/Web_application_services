using WebApplication1.Models;
namespace WebApplication1.Services;
public interface IMemberService
{
    Task<List<Member>> GetAllAsync();
    Task<Member?> GetByIdAsync(int id);
    Task<Member> CreateAsync(string fullName, string email);
    Task<Member?> UpdateAsync(int id, string fullName, string email);
    Task<bool> DeleteAsync(int id);
}