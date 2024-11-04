namespace MyBookCollection.Models.Data
{
    public class Translator
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? UserId { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
