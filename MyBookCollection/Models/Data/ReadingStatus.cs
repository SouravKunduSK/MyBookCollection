namespace MyBookCollection.Models.Data
{
    public class ReadingStatus
    {
        public int Id { get; set; }
        public string Status { get; set; }

        // Navigation Property
        public virtual ICollection<Book> Books { get; set; }
    }
}
