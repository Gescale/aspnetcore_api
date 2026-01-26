using DemoAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoAPI.Models.Filters.ActionFilters
{
    public class Shirt_ValidateCreateShirtFilterAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext _db;

        public Shirt_ValidateCreateShirtFilterAttribute(ApplicationDbContext db)
        {
            this._db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var shirt = context.ActionArguments["shirt"] as Shirt;
            
            if (shirt == null)
            {
                context.ModelState.AddModelError("Shirt", "Shirt object is null.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else if(shirt != null)
            {
                var existingShirt = _db.Shirts.FirstOrDefault(x =>
                    !string.IsNullOrWhiteSpace(shirt.BrandName) &&
                    !string.IsNullOrWhiteSpace(x.BrandName) &&
                    x.BrandName.ToLower() == shirt.BrandName.ToLower() &&
                    !string.IsNullOrWhiteSpace(shirt.Gender) &&
                    !string.IsNullOrWhiteSpace(x.Gender) &&
                    x.Gender.ToLower() == shirt.Gender.ToLower() &&
                    !string.IsNullOrWhiteSpace(shirt.Colour) &&
                    !string.IsNullOrWhiteSpace(x.Colour) &&
                    x.Colour.ToLower() == shirt.Colour.ToLower() &&
                    shirt.Size.HasValue &&
                    shirt.Size.Value == x.Size.Value);

                if (existingShirt != null)
                {
                    context.ModelState.AddModelError("Shirt", "Shirt with the same values already exist.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
            }
        }
    }
}
