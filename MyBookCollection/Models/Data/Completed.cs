namespace MyBookCollection.Models.Data
{
    public class Completed
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string? UserId { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public DateTime BuyingDate { get; set; }
        public DateTime? ReadingStartDate { get; set; }
        public DateTime? ReadingEndDate { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }

        // Navigation Property
        public virtual Book Book { get; set; }
    }
}
