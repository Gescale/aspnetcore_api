namespace DemoAPI.Models.Repositories
{
    public class ShirtRepository
    {
        private static List<Shirt> shirts = new List<Shirt>
        {
            new Shirt { ShirtId = 1, BrandName = "Country Road", Colour = "Red", Size = 34, Price = 199.99M, Gender = "men" },
            new Shirt { ShirtId = 2, BrandName = "Zara", Colour = "Blue", Size = 28, Price = 249.99M, Gender = "women" },
            new Shirt { ShirtId = 3, BrandName = "Woolworths", Colour = "Green", Size = 30, Price = 149.99M, Gender = "men" },
            new Shirt { ShirtId = 4, BrandName = "Polo", Colour = "Black", Size = 32, Price = 299.3m, Gender = "women" },
            new Shirt { ShirtId = 5, BrandName = "Levis", Colour = "White", Size = 36, Price = 179.5m, Gender = "men" },
            new Shirt { ShirtId = 6, BrandName = "Guess", Colour = "Yellow", Size = 26, Price = 219.99m, Gender = "men" }
        };

        public static List<Shirt> GetShirts()
        {
            return shirts;
        }

        public static bool ShirtExists(int id)
        {
            return shirts.Any(s => s.ShirtId == id);
        }

        public static Shirt? GetShirtById(int id)
        {
            return shirts.FirstOrDefault(s => s.ShirtId == id);
        }

        public static Shirt? GetShirtByProperties(string? brandName, string? gender, string? colour, int? size)
        {
            return shirts.FirstOrDefault(x =>
            !string.IsNullOrWhiteSpace(brandName) &&
            !string.IsNullOrWhiteSpace(x.BrandName) &&
            x.BrandName.Equals(brandName, StringComparison.OrdinalIgnoreCase) &&
            !string.IsNullOrWhiteSpace(gender) &&
            !string.IsNullOrWhiteSpace(x.Gender) &&
            x.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase) &&
            !string.IsNullOrWhiteSpace(colour) &&
            !string.IsNullOrWhiteSpace(x.Colour) &&
            x.Colour.Equals(colour, StringComparison.OrdinalIgnoreCase) &&
            size.HasValue &&
            x.Size.HasValue &&
            size.Value == x.Size.Value);
        }

        public static void AddShirt(Shirt shirt)
        {
            // In a real-world application, you would typically generate the ID using a database auto-increment feature
            // Here, we simply set it to one more than the current maximum ID for demonstration purposes
            int maxId = shirts.Max(s => s.ShirtId);
            shirt.ShirtId = maxId + 1;
            shirts.Add(shirt);
        }

        public static void UpdateShirt(Shirt shirt)
        {
            var existingShirt = GetShirtById(shirt.ShirtId);
            if (existingShirt != null)
            {
                existingShirt.BrandName = shirt.BrandName;
                existingShirt.Colour = shirt.Colour;
                existingShirt.Size = shirt.Size;
                existingShirt.Price = shirt.Price;
                existingShirt.Gender = shirt.Gender;
            }
        }

        public static void DeleteShirt(int id)
        {
            var shirt = GetShirtById(id);
            if (shirt != null)
            {
                shirts.Remove(shirt);
            }
        }
    }
}
