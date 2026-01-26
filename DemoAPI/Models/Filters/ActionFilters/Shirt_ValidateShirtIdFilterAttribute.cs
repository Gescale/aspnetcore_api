using DemoAPI.Data;
using DemoAPI.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoAPI.Models.Filters.ActionFilters
{
    public class Shirt_ValidateShirtExistanceFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var shirt = context.ActionArguments["shirt"] as Shirt;
            if (shirt == null)
            {
                context.ModelState.AddModelError("Shirt", "Shirt can not be null.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }

            else
            {
                var existingShirt = ShirtRepository.GetShirtByProperties(shirt.BrandName, shirt.Gender, shirt.Colour, shirt.Size);
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

    public class Shirt_ValidateShirtIdFilterAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext db;

        public Shirt_ValidateShirtIdFilterAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var shirtId = context.ActionArguments["id"] as int?;
       
            if(shirtId.HasValue)
            {
                if (shirtId.Value < 0)
                {
                    context.ModelState.AddModelError("ShirtId", "ShirtId is invalid.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }

                else
                {
                    var shirt = db.Shirts.Find(shirtId.Value);

                    if (shirt == null)
                    {
                        context.ModelState.AddModelError("ShirtId", "Shirt doesnt exist.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new NotFoundObjectResult(problemDetails);
                    }

                    else
                    {
                        context.HttpContext.Items["shirt"] = shirt;
                    }
                }
            }
        }
    }
}
