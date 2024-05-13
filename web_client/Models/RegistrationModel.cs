namespace web_client.Models
{
    public class RegistrationModel : GlobalModel
    {
        public bool NameOk { get; set; }
        public bool AddressOk { get; set; }
        public bool EmailOk { get; set; }
        public bool PasswordOk { get; set; }

    }
}
