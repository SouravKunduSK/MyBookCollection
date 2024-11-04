namespace MyBookCollection.Models.Data
{
    public class Lending
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string? UserId { get; set; }
        public string BorrowerName { get; set; }
        public DateTime LendDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int BookStatusId { get; set; }

        // Navigation Properties
        public virtual Book Book { get; set; }
        public virtual BookStatus BookStatus { get; set; }
    }
}
