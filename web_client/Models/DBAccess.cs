using CS_projekt.data;

namespace web_client.Models
{
    public class DBAccess
    {
        public Dictionary<int, ApplicationTable> ApplicationMap { 
            get
            {
                return DataEntryPoint.ApplicationMap;
            }
            set
            {
                ApplicationMap = value;
            }
        }
        public List<ApplicationTable> ApplicationList { 
            get 
            {
                return new List<ApplicationTable>(ApplicationMap.ToList().Select(x => x.Value));    
            }
            set { }
        }

        public Dictionary<int, StudentTable> StudentMap {
            get 
            {
                return DataEntryPoint.StudentMap;
            }

            set 
            {
                StudentMap = value;
            }
        }
        public List<StudentTable> StudentList
        {
            get
            {
                return new List<StudentTable>(StudentMap.ToList().Select(x => x.Value));
            }
            set { }
        }

        public Dictionary<int, SchoolTable> SchoolMap
        {
            get
            {
                return DataEntryPoint.SchoolMap;
            }
            set
            {
                SchoolMap = value;
            }
        }
        public List<SchoolTable> SchoolList
        {
            get
            {
                return new List<SchoolTable>(SchoolMap.ToList().Select(x => x.Value));
            }
            set { }
        }

        public Dictionary<int, ProgramTable> ProgramMap
        {
            get
            {
                return DataEntryPoint.ProgramMap;
            }
            set
            {
                ProgramMap = value;
            }
        }
        public List<ProgramTable> ProgramList
        {
            get
            {
                return new List<ProgramTable>(ProgramMap.ToList().Select(x => x.Value));
            }
            set { }
        }

        public DBAccess()
        {
            DataEntryPoint.connection_string = "Data Source=../Bin/Debug/net6.0/mydb.db;";
            DataEntryPoint.initDB();
            DataEntryPoint.loadDB();

        }
    }
}
