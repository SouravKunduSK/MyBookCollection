using System.ComponentModel.DataAnnotations;

namespace MyBookCollection.Models.ViewModels
{
    public class AuthorVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Author Name is required.")]
        public string Name { get; set; }
        public string? UserId { get; set; }
    }
}
