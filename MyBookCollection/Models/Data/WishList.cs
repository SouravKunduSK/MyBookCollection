namespace MyBookCollection.Models.Data
{
    public class WishList
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string Name { get; set; }
        public string AuthorName { get; set; }
        public string? Photo { get; set; }
        public decimal? Price { get; set; }

        public AppUser? User { get; set; }
    }
}
