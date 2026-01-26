using DemoAPI.Data;
using DemoAPI.Models;
using DemoAPI.Models.Filters.ActionFilters;
using DemoAPI.Models.Filters.ExceptionFilters;
using DemoAPI.Models.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DemoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShirtsController : ControllerBase
    {
        private readonly ApplicationDbContext db;

        public ShirtsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        //There are two ways to define routes in ASP.NET Core:
        //  - Using [HttpGet] and [Route("/my-route")] on every method
        //  - Using [HttpGet("/my-route")] on method level and placing [Route("api/[controller]")]
        //      on the class level, the route name will be derived from the class name. eg : ShirtsController -> shirts


        [HttpGet]
        public IActionResult GetShirts()
        {
            return Ok(db.Shirts.ToList());
        }

        // 1. We can use automatic model binding to bind route parameters to method parameters
        //[HttpGet("{id}/{colour}")]
        //public string GetShirtById(int id, string colour)
        //{
        //    return ($"Shirt with ID: {id} returned");
        //}

        // 2. We can also use [FromRoute] attribute to bind route parameters to method parameters
        //[HttpGet("{id}/{colour}")]
        //public string GetShirtById(int id, [FromRoute] string colour)
        //{
        //    return ($"Shirt with ID: {id} returned");
        //}

        // 3. [FromHeader(Name = "colour")] or [FromBody] and [FromForm]
        // This is another option, the colour can be passed in the header as shown above

        // 4. We can also use [FromQuery] attribute to bind query parameters to method parameters
        // The colour parameter will be passed as a query string parameter eg: /api/shirts/34?colour=red
        //[HttpGet("{id}")]
        //public string GetShirtById(int id, [FromQuery] string colour)
        //{
        //    return ($"Shirt with ID: {id}, colour : {colour} returned");
        //}

        // Get shirt by id without mentioning the colour
        // We use IActionResult when the method returns varying types
        // Whenever we want to return IActionResult as a string we must return the string in the Ok method eg: return Ok("The return string");
        [HttpGet("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        public IActionResult GetShirtById(int id)
        {
            return Ok(HttpContext.Items["shirt"]);
        }

        //To update
        [HttpPut("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        [Shirt_ValidateUpdateShirtFilter]
        [Shirt_HandleUpdateExceptionsFilter]
        public IActionResult UpdateShirt(int id, Shirt shirt)
        {
            ShirtRepository.UpdateShirt(shirt);
            
            return NoContent();
        }


        // Binding using the body we use the post or put
        [HttpPost]
        [Shirt_ValidateShirtExistanceFilter]
        public IActionResult CreateShirt([FromBody]Shirt shirt)
        {
            ShirtRepository.AddShirt(shirt);

            return CreatedAtAction(nameof(GetShirtById),
                new { id = shirt.ShirtId },
                shirt);
        }


        // To delete
        //[HttpDelete]
        //[Route("/shirts/{id}")]
        //public IActionResult DeleteShirt(int id)
        //{
        //    return Ok($"Deleting shirt {id}");
        //}

        [HttpDelete]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        public IActionResult DeleteShirt(int id)
        {
            var shirt = ShirtRepository.GetShirtById(id);
            ShirtRepository.DeleteShirt(id);

            return Ok(shirt);
        }

    }
}
