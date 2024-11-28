using MyBookCollection.Models.Data;

namespace MyBookCollection.Services
{
    public interface IUserService
    {
        List<AppUser> GetUsers();
    }
}
