using System.ComponentModel.DataAnnotations;

namespace DemoAPI.Models.Validations
{
    public class Shirt_EnsureCorrectSizingAttribute : ValidationAttribute
    {
        // Ensures that the shirt size is within acceptable limits
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // var shirt = (Shirt)validationContext.ObjectInstance;
            var shirt = validationContext.ObjectInstance as Shirt;

            if (shirt != null && !string.IsNullOrWhiteSpace(shirt.Gender))
            {
                if(shirt.Gender.Equals("men", StringComparison.OrdinalIgnoreCase) && shirt.Size < 8)
                {
                    return new ValidationResult("For men's shirts, the size has to be greater or equal to 8.");
                }

                else if(shirt.Gender.Equals("women", StringComparison.OrdinalIgnoreCase) && shirt.Size < 6)
                {
                    return new ValidationResult("For women's shirts, the size has to be greater or equal to 6.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
