using Microsoft.EntityFrameworkCore;
using MyBookCollection.Models;
using MyBookCollection.Models.Data;

namespace MyBookCollection.Services
{
    public class UserService : IUserService
    {
        private MyBookCollectionDbContext _context;
        public UserService(MyBookCollectionDbContext context)
        {
            _context = context;
        }

        public async Task<List<AppUser>> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }
    }
}
