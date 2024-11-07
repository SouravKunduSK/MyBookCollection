using System.ComponentModel.DataAnnotations;

namespace MyBookCollection.Helpers.Validators
{
    public class PasswordComplexityAttribute:ValidationAttribute
    {
        public PasswordComplexityAttribute()
        {
            // Set a default error message
            this.ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.";
        }

        public override bool IsValid(object value)
        {
            var password = value as string;
            if (string.IsNullOrEmpty(password))
            {
                return false; // Ensure password is not null or empty
            }

            // Check for uppercase letter
            if (!password.Any(char.IsUpper))
            {
               // this.ErrorMessage = "Password must contain at least one uppercase letter.";
                return false;
            }

            // Check for lowercase letter
            if (!password.Any(char.IsLower))
            {
                //this.ErrorMessage = "Password must contain at least one lowercase letter.";
                return false;
            }

            // Check for a digit
            if (!password.Any(char.IsDigit))
            {
                //this.ErrorMessage = "Password must contain at least one digit.";
                return false;
            }

            // Check for a special character
            if (!password.Any(ch => "!@#$%^&*(),.?\":{}|<>".Contains(ch)))
            {
                //this.ErrorMessage = "Password must contain at least one special character.";
                return false;
            }

            return true;
        }
    }
}
