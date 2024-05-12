using Microsoft.AspNetCore.Mvc;
using web_client.Models;
using CS_projekt.data;

namespace web_client.Controllers
{
    public class ProfileController : Controller
    {
        private readonly DBAccess dBAccess;

        public ProfileController(DBAccess dBAccess)
        {
            this.dBAccess = dBAccess;
        }

        public IActionResult Index()
        {
            bool IsAuthorized = (bool)HttpContext.Items["IsAuthorized"];
            int UserId = (int)HttpContext.Items["UserId"];

            StudentTable? stud = null;

            foreach(var user in dBAccess.StudentList)
            {
                if (user.Id == UserId)
                {
                    stud = user;
                }
            }

            return View(new ProfileModel() { IsAuthorized = true, Student=stud });
        }
    }
}
