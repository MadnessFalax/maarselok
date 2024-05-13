using CS_projekt.data;
using Microsoft.AspNetCore.Mvc;
using web_client.Models;

namespace web_client.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly DBAccess dBAccess;

        public ApplicationsController(DBAccess dBAccess)
        {
            this.dBAccess = dBAccess;
        }

        public IActionResult Index()
        {
            bool IsAuthorized = (bool)HttpContext.Items["IsAuthorized"];
            int UserId = (int)HttpContext.Items["UserId"];

            StudentTable? stud = null;

            foreach (var user in dBAccess.StudentList)
            {
                if (user.Id == UserId)
                {
                    stud = user;
                }
            }

            return View(new ApplicationsModel() { IsAuthorized = true, Student = stud });
        }

        public IActionResult Delete(int id) 
        {
            bool IsAuthorized = (bool)HttpContext.Items["IsAuthorized"];
            int UserId = (int)HttpContext.Items["UserId"];

            StudentTable? stud = null;

            foreach (var user in dBAccess.StudentList)
            {
                if (user.Id == UserId)
                {
                    stud = user;

                    ApplicationTable toDelete = null;

                    foreach (var app in stud.Applications)
                    {

                        if (app.Value.Id == id)
                        {
                            toDelete = app.Value;
                        }
                    }

                    if (toDelete != null)
                    {
                        TableOperation<ApplicationTable>.Delete(ref toDelete);
                        TableOperation<StudentTable>.ForceRefreshAll();
                        TableOperation<ProgramTable>.ForceRefreshAll();
                        TableOperation<ApplicationTable>.ForceRefreshAll();
                    }
                }
            }

            return RedirectToAction("Index", "Applications");
        }
    }
}
