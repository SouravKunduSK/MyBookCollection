using MyBookCollection.Models.Data;

namespace MyBookCollection.Services
{
    public interface IAuthorService
    {
        Task <List<Author>>GetAllAuthorsAsync(string userId);
        Task <Author> GetAuthorByIdAsync(int id);
        Task AddAuthorAsync(Author author, string userId);
        Task UpdateAuthorAsync(Author author);
        Task DeleteAuthorAsync(int id);
    }
}
