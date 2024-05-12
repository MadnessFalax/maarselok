using web_client.Models;

namespace web_client.Controllers
{
    public class LoginMiddleware : IMiddleware
    {
        private readonly DBAccess dBAccess;
        public LoginMiddleware(DBAccess dBAccess) 
        { 
            this.dBAccess = dBAccess;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var email = context.Request.Cookies["Email"];
            var password = context.Request.Cookies["Password"];
            var usedId = context.Request.Cookies["UserId"];

            var logged = IsUserLoggedIn(ref context, email, password);

            context.Items["IsAuthorized"] = logged;
            await next(context);
        }

        private bool IsUserLoggedIn(ref HttpContext context, string email, string password)
        {
            foreach(var user in dBAccess.StudentList)
            {
                if (user.Email == email)
                {
                    if (user.Password == password)
                    {
                        context.Items["UserId"] = user.Id;
                        return true;
                    }
                }
            }
            context.Items["UserId"] = null;
            return false;
        }
    }
}
