using MyBookCollection.Helpers.Validators;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBookCollection.Models.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [RegularExpression(@"^\S+@\S+\.\S+$", ErrorMessage = "Invalid email address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [PasswordComplexity(ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter and one digit.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "Passwords don't match, try another.")]
        public string ConfirmPassword { get; set; }
    }
}
