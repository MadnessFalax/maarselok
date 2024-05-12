namespace web_client.Models
{
    public class LoginModel
    {
        public bool InvalidLoginData { get; set; } = false;
        public bool IsAuthorized { get; set; }
    }
}
