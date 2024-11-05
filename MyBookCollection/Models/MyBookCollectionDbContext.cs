using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyBookCollection.Models.Data;

namespace MyBookCollection.Models
{
    public class MyBookCollectionDbContext : IdentityDbContext<AppUser>
    {
        public MyBookCollectionDbContext(DbContextOptions<MyBookCollectionDbContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Translator> Translators { get; set; }
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Completed> CompletedBooks { get; set; }
        public DbSet<Lending> Lendings { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<BookStatus> BookStatuses { get; set; }
        public DbSet<ReadingStatus> ReadingStatuses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define relationships and constraints

            // SubCategory - Category (Many-to-One)
            modelBuilder.Entity<SubCategory>()
                .HasOne(sc => sc.Category)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(sc => sc.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Book - Author (Many-to-One)
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            //Book - Translator (Many-to-one)
            modelBuilder.Entity<Book>()
                .HasOne(b=>b.Translator)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.TranslatorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Book - Category (Many-to-One)
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Book - SubCategory (Many-to-One)
            modelBuilder.Entity<Book>()
                .HasOne(b => b.SubCategory)
                .WithMany(sc => sc.Books)
                .HasForeignKey(b => b.SubCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Book - ReadingStatus (Many-to-One)
            modelBuilder.Entity<Book>()
                .HasOne(b => b.ReadingStatus)
                .WithMany(rs => rs.Books)
                .HasForeignKey(b => b.ReadingStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // Book - BookStatus (Many-to-One)
            modelBuilder.Entity<Book>()
                .HasOne(b => b.BookStatus)
                .WithMany(bs => bs.Books)
                .HasForeignKey(b => b.BookStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // Completed - Book (Many-to-One)
            modelBuilder.Entity<Completed>()
                .HasOne(c => c.Book)
                .WithMany()
                .HasForeignKey(c => c.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            // Lending - Book (Many-to-One)
            modelBuilder.Entity<Lending>()
                .HasOne(l => l.Book)
                .WithMany()
                .HasForeignKey(l => l.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            // Lending - BookStatus (Many-to-One)
            modelBuilder.Entity<Lending>()
                .HasOne(l => l.BookStatus)
                .WithMany(bs => bs.Lendings)
                .HasForeignKey(l => l.BookStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // BookStatus - Book (One-to-Many)
            modelBuilder.Entity<BookStatus>()
                .HasMany(bs => bs.Books)
                .WithOne(b => b.BookStatus)
                .HasForeignKey(b => b.BookStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // ReadingStatus - Book (One-to-Many)
            modelBuilder.Entity<ReadingStatus>()
                .HasMany(rs => rs.Books)
                .WithOne(b => b.ReadingStatus)
                .HasForeignKey(b => b.ReadingStatusId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
