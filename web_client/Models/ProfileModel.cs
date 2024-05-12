using CS_projekt.data;

namespace web_client.Models
{
    public class ProfileModel
    {
        public StudentTable? Student { get; set; }
        public bool IsAuthorized { get; set; }
    }
}
