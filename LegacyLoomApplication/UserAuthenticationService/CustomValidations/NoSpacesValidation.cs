using System.ComponentModel.DataAnnotations;

namespace UserAuthenticationService.CustomValidations
{
    public class NoSpacesValidation: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string || value == null) return new ValidationResult($"{value} is not of string type");
            var username = value.ToString();
            if (username is not null && username.Contains(" "))
            {
                return new ValidationResult($"Username: {username} must not contain any spaces in between");
            }
            return ValidationResult.Success;
        }
    }
}
