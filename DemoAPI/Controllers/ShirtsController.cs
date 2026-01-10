using Microsoft.AspNetCore.Mvc;
using DemoAPI.Models;

namespace DemoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShirtsController : ControllerBase
    {

        //There are two ways to define routes in ASP.NET Core:
        //  - Using [HttpGet] and [Route("/my-route")] on every method
        //  - Using [HttpGet("/my-route")] on method level and placing [Route("api/[controller]")]
        //      on the class level, the route name will be derived from the class name. eg : ShirtsController -> shirts
        

        [HttpGet]
        public string GetShirtSize()
        {
            return ("Many shirts returned");
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
        [HttpGet("{id}")]
        public string GetShirtById(int id, [FromQuery] string colour)
        {
            return ($"Shirt with ID: {id}, colour : {colour} returned");
        }

        //To update
        [HttpPut("{id}")]
        public string UpdateShirt(int id)
        {
            return $"Updating shirt {id}";
        }


        // Binding using the body we use the post or put
        // 
        [HttpPost]
        public string CreateShirt([FromBody]Shirt shirt)
        {
            
            return $"Created a shirt for : R{shirt.Price}";
        }


        // To delete
        //[HttpDelete]
        //[Route("/shirts/{id}")]
        //public string DeleteShirt(int id)
        //{
        //    return $"Deleting shirt {id}";
        //}
    }
}
