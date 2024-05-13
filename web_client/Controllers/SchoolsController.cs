using CS_projekt.data;
using Microsoft.AspNetCore.Mvc;
using web_client.Models;


namespace web_client.Controllers
{
    public class SchoolsController : Controller
    {
        private readonly DBAccess _access;

        public SchoolsController(DBAccess access)
        {
            _access = access;
        }

        public IActionResult Index()
        {
            bool IsLoggedIn = (bool)HttpContext.Items["IsAuthorized"];

            

            return View(new SchoolsModel() { IsAuthorized = IsLoggedIn, Schools = _access.SchoolList });
        }

        public IActionResult Programs(int id, bool successful = true)
        {
            bool IsLoggedIn = (bool)HttpContext.Items["IsAuthorized"];
            int? UserId = null;
            if (IsLoggedIn) 
            { 
                UserId = (int)HttpContext.Items["UserId"];
            }
            
            return View(new ProgramsModel() { IsAuthorized = IsLoggedIn, School = _access.SchoolMap[id], Student = IsLoggedIn ? _access.StudentMap[UserId.Value] : null, ActionStatus = new ActionStatusModel() { Success = successful, Message = "You are at maximum number of applications!" } });
        }

        public IActionResult Apply(int id)
        {
            bool IsLoggedIn = (bool)HttpContext.Items["IsAuthorized"];
            int UserId = (int)HttpContext.Items["UserId"];

            bool success = false;

            if (_access.StudentMap[UserId].ApplicationCount < 3)
            {
                success = true;
                TableOperation<ApplicationTable>.Create(new ApplicationTable(UserId, id));
                TableOperation<StudentTable>.ForceRefreshAll();
                TableOperation<ProgramTable>.ForceRefreshAll();
                TableOperation<ApplicationTable>.ForceRefreshAll();
            }

            return RedirectToAction("Programs", "Schools", new { id = _access.ProgramMap[id].SchoolId.Value, successful = success });
        }
    }
}
