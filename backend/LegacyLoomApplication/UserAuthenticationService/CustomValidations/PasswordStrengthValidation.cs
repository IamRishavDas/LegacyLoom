using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UserAuthenticationService.CustomValidations
{
    public class PasswordStrengthValidation: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var password = value as string;

            if (string.IsNullOrEmpty(password))
            {
                return new ValidationResult("Password is required.");
            }

            // Password must be at least 8 characters, include an uppercase letter, a number, and a special character.
            var regex = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
            if (!regex.IsMatch(password))
            {
                return new ValidationResult("Password must be at least 8 characters long, include an uppercase letter, a number, and a special character.");
            }

            return ValidationResult.Success;
        }
    }
}
