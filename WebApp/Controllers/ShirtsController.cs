using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models.Repositories;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ShirtsController : Controller
    {
        private readonly IWebApiExecuter webApiExecuter;

        public ShirtsController(IWebApiExecuter webApiExecuter)
        {
            this.webApiExecuter = webApiExecuter;
        }
        public async Task<IActionResult> Index()
        {
            Console.WriteLine("ShirtsController.Index called");
            return View(await webApiExecuter.InvokeGet<List<Shirt>>("shirts"));
        }

        public IActionResult CreateShirt()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateShirt(Shirt shirt)
        {
            var response = await webApiExecuter.InvokePost<Shirt>("shirts", shirt);
            if(response != null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(shirt);
        }

        public async Task<IActionResult> UpdateShirt(int shirtId)
        {
            var shirt = await webApiExecuter.InvokeGet<Shirt>($"shirts/{shirtId}");
            if(shirt != null)
            {
                return View(shirt);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateShirt(Shirt shirt)
        {
            var response = await webApiExecuter.InvokePut($"shirts/{shirt.ShirtId}", shirt);
            if (response != null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(shirt);
        }
    }
}
