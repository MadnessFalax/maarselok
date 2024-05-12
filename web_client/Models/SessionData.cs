using CS_projekt.data;
using System.Runtime.Serialization;

namespace web_client.Models
{
    public class SessionData
    {
        public bool IsAuthorized { get; set; }
        public StudentTable User { get; set; }

    }
}
