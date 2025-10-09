using WebApplication1.Models;

namespace WebApplication1.Services;

public interface IBookService
{
    Task<List<Book>> GetAllWithAuthorsAsync();
    Task<Book?> GetByIdWithAuthorAsync(int id);
    Task<Book> CreateAsync(string title, int authorId);             // throws if bad authorId
    Task<Book?> UpdateAsync(int id, string title, int authorId);    // null if not found, throws if bad authorId
    Task<bool> DeleteAsync(int id);                                 // false if not found
}