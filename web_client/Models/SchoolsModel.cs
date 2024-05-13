using CS_projekt.data;

namespace web_client.Models
{
    public class SchoolsModel : GlobalModel
    {
        public List<SchoolTable> Schools { get; set; } = new List<SchoolTable>();
    }
}
