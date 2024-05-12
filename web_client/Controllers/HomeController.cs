using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using web_client.Models;

namespace web_client.Controllers
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
            bool IsLoggedIn = (bool)HttpContext.Items["IsAuthorized"];
            return View(new HomeModel() { IsAuthorized = IsLoggedIn });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}