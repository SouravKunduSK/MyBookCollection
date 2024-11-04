namespace MyBookCollection.Models.Data
{
    public class BookStatus
    {
        public int Id { get; set; }
        public string Status { get; set; }

        // Navigation Property
        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<Lending> Lendings { get; set; }
    }
}
