namespace MyBookCollection.Models.Data
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? UserId { get; set; }

        public AppUser? User { get; set; }
        public virtual ICollection<SubCategory> SubCategories { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
