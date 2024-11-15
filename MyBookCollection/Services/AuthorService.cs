using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyBookCollection.Models;
using MyBookCollection.Models.Data;
using NuGet.Versioning;

namespace MyBookCollection.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly MyBookCollectionDbContext _context;
        public AuthorService(MyBookCollectionDbContext context)
        {
            _context = context;
        }
        public async Task AddAuthorAsync(Author author, string userId)
        {
            var authorExist = await _context.Authors.FirstOrDefaultAsync(a=>a.UserId == userId && a.Name == author.Name);
            if(authorExist!=null)
            {
                throw new InvalidOperationException("This author name already exists! Try another.");
            }
            author.UserId = userId;
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateAuthorAsync(Author author)
        {
            var existingAuthor = await _context.Authors.FindAsync(author.Id);
            if (existingAuthor == null)
            {
                throw new InvalidOperationException("This author name is not available! Try another.");
            }
            existingAuthor.Name = author.Name;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAuthorAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                throw new InvalidOperationException("This author name is not available! Try another.");
            }
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Author>> GetAllAuthorsAsync(string userId)
        {
            var authors = await _context.Authors
                .Where(a=>a.UserId == userId)
                .ToListAsync();
            return authors;
        }

        public async Task<Author> GetAuthorByIdAsync(int id)
        {
            var author = await _context.Authors
               .FindAsync(id);
            return author;
        }
    }
}
