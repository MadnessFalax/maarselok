namespace web_client.Models
{
    public class LoginModel : GlobalModel
    {
        public bool InvalidLoginData { get; set; } = false;
        public string Message { get; set; } = "";
    }
}
