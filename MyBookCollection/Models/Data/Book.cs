namespace MyBookCollection.Models.Data
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string? UserId { get; set; }
        public int AuthorId { get; set; }
        public int TranslatorId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public DateTime BuyingDate { get; set; }
        public DateTime? ReadingStartDate { get; set; }
        public DateTime? ReadingEndDate { get; set; }
        public int ReadingStatusId { get; set; }
        public int BookStatusId { get; set; }

        public virtual Author Author { get; set; }
        public virtual Translator Translator { get; set; }
        public virtual Category Category { get; set; }
        public virtual SubCategory SubCategory { get; set; }
        public virtual ReadingStatus ReadingStatus { get; set; }
        public virtual BookStatus BookStatus { get; set; }
    }
}
