using Microsoft.AspNetCore.Identity;
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
        public async Task<string> GetUsernameAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user?.FullName;
        }

        public async Task<string> GetUserImageLinkAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user?.ImageLink;
        }
    }
}
