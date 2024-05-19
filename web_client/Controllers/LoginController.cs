using CS_projekt.data;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using web_client.Models;

namespace web_client.Controllers
{
    public class LoginController : Controller
    {
        private readonly DBAccess dbService;

        private Regex name_regex = new Regex(@"^[A-Z][a-zA-Z '-]+$");
        private Regex address_regex = new Regex(@"^[a-zA-Z ,'0-9-\.]+$");
        private Regex email_regex = new Regex(@"^[a-zA-Z0-9.]+[@][a-zA-Z0-9_]+[\.][a-z]{2,5}$");
        private Regex password_regex = new Regex(@"^[a-zA-Z0-9\./?!,%=+*'_@-]+$");

        public LoginController(DBAccess dbService)
        {
            this.dbService = dbService;
        }

        public IActionResult Index(string message)
        {
            return View(new LoginModel() { Message = message });
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

        public IActionResult Registration()
        {
            return View(new RegistrationModel() { NameOk = true, AddressOk = true, EmailOk = true, PasswordOk = true });
        }

        [HttpPost]
        public IActionResult Registration(string Name, string Address, string Email, string Password)
        {
            bool nameOk = name_regex.IsMatch(Name);
            bool addressOk = address_regex.IsMatch(Address);
            bool emailOk = email_regex.IsMatch(Email);
            bool passwordOk = password_regex.IsMatch(Password);

            if (nameOk && addressOk && emailOk && passwordOk)
            {
                TableOperation<StudentTable>.Create(new StudentTable(Name, Address, Email, Password));
                TableOperation<StudentTable>.ForceRefreshAll();
                TableOperation<ApplicationTable>.ForceRefreshAll();
                return RedirectToAction("Index", "Login", new {message = "You can login now!"});
            }
            else
            {
                return View(new RegistrationModel() { NameOk = nameOk, AddressOk = addressOk, EmailOk = emailOk, PasswordOk = passwordOk});
            }

        }
    }
}
