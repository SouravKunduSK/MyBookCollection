using MyBookCollection.Models.Data;

namespace MyBookCollection.Services
{
    public interface IUserService
    {
        Task<List<AppUser>> GetUsersAsync();
    }
}
