using Microsoft.AspNetCore.Mvc;

namespace web_client.Controllers
{
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            Response.Cookies.Delete("Password");
            Response.Cookies.Delete("Email");
            Response.Cookies.Delete("UsedId");
            return RedirectToAction("Index", "Home");
        }
    }
}
