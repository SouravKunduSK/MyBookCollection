using MyBookCollection.Models.Data;

namespace MyBookCollection.Services
{
    public interface IUserService
    {
        Task<List<AppUser>> GetUsersAsync();
        Task<string> GetUsernameAsync(string userId);
        //Task<string> GetUserImageLinkAsync(string userId);
    }
}
