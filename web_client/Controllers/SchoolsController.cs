using Microsoft.AspNetCore.Mvc;

namespace web_client.Controllers
{
    public class SchoolsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
