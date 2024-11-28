using Microsoft.AspNetCore.Identity;

namespace MyBookCollection.Models.Data
{
    public class AppUser:IdentityUser
    {
        public AppUser()
        {
            Authors = new List<Author>();
            Categories = new List<Category>();
            Books = new List<Book>();
            WishLists = new List<WishList>();
            CompletedBooks = new List<Completed>();
            Lendings = new List<Lending>();
        }

        public string FullName { get; set; }

        public virtual ICollection<Author> Authors { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<WishList> WishLists { get; set; }
        public virtual ICollection<Completed> CompletedBooks { get; set; }
        public virtual ICollection<Lending> Lendings { get; set; }
    }
}
