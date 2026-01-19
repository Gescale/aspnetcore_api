using DemoAPI.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace DemoAPI.Models
{
    public class Shirt
    {
        // - By placing [Required] attribute on a property, we ensure that the property must have a value when creating or updating a Shirt object.
        // - By placing a question mark (?) after the type of a property, we indicate that the property is nullable and can have a null value.
        // - This is all part of data validation and ensuring data integrity in our application.
        [Required]
        public int ShirtId { get; set; }

        public string? BrandName { get; set; }

        [Required]
        public string? Colour { get; set; }

        [Shirt_EnsureCorrectSizing]
        public int? Size { get; set; }

        [Required]
        public int? Price { get; set; }

        [Required]
        public string? Gender { get; set; }
    }
}
