namespace MyBookCollection.Models.Data
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? UserId { get; set; }

        public AppUser? User { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
