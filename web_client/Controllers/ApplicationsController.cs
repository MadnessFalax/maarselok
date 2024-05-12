using Microsoft.AspNetCore.Mvc;

namespace web_client.Controllers
{
    public class ApplicationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
