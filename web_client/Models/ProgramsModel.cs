using CS_projekt.data;

namespace web_client.Models
{
    public class ProgramsModel : GlobalModel
    {
        public ActionStatusModel ActionStatus { get; set; }
        public SchoolTable School { get; set; }
        public StudentTable Student { get; set; }
    }
}
