using Microsoft.AspNetCore.Mvc;
using web_client.Models;

namespace web_client.Controllers
{
    public class LoginController : Controller
    {
        private readonly DBAccess dbService;

        public LoginController(DBAccess dbService)
        {
            this.dbService = dbService;
        }

        public IActionResult Index()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public IActionResult Index(string Email, string Password) 
        { 
            foreach (var user in dbService.StudentList)
            {
                if (user.Email.Equals(Email))
                {
                    if (user.Password.Equals(Password))
                    {
                        Response.Cookies.Append("Email", Email, new CookieOptions { Expires = DateTimeOffset.UtcNow.AddHours(168) });
                        Response.Cookies.Append("Password", Password, new CookieOptions { Expires = DateTimeOffset.UtcNow.AddHours(168) });
                        Response.Cookies.Append("UserId", user.Id.ToString(), new CookieOptions { Expires = DateTimeOffset.UtcNow.AddHours(168) });
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View(new LoginModel() { InvalidLoginData = true });
        }
    }
}
