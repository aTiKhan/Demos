using System.Diagnostics;
using Coderr.Client.AspNetCore.Mvc.Page;
using Microsoft.AspNetCore.Mvc;
using MvcDemo.WebSite.Models;

namespace MvcDemo.WebSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(PostModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Title == "fail")
            {
                throw new InvalidDataException("This failed!");
            }

            return Redirect("/");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorPageViewModel {  });
        }
    }
}