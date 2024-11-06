using MyBookCollection.Helpers.Validators;
using System.ComponentModel.DataAnnotations;

namespace MyBookCollection.Models.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Email is required.")]
        [CustomEmailValidator(ErrorMessage = "Email is not valid(custom)")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password should contain atleast 6 characters.")]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
