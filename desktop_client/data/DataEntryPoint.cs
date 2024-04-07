using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CS_projekt.data
{

    

    public class DataEntryPoint
    {
        static string connection_string = "Data Source=mydb.db;";

        static public Dictionary<int, StudentTable> StudentMap { get; set; }
        static public Dictionary<int, SchoolTable> SchoolMap { get; set; }
        static public Dictionary<int, ProgramTable> ProgramMap { get; set; }
        static public Dictionary<int, ApplicationTable> ApplicationMap { get; set; }

        static void Main()
        {
            initDB();
            loadDB();

            // create DB schema
            /*using (var connection = new SqliteConnection(connection_string))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = File.ReadAllText("..\\..\\..\\data\\database-create.sql");
                cmd.ExecuteNonQuery();
            }
            */


        }

        static public void initDB()
        {
            DBGateway<StudentTable>.setConnectionString(connection_string);
            DBGateway<SchoolTable>.setConnectionString(connection_string);
            DBGateway<ApplicationTable>.setConnectionString(connection_string);
            DBGateway<ProgramTable>.setConnectionString(connection_string);
        }

        static public void loadDB()
        {
            TableOperation<StudentTable>.RefreshAll();
            TableOperation<SchoolTable>.RefreshAll();
            TableOperation<ProgramTable>.RefreshAll();
            TableOperation<ApplicationTable>.RefreshAll();

            StudentMap = TableOperation<StudentTable>.identity_map;
            SchoolMap = TableOperation<SchoolTable>.identity_map;
            ProgramMap  = TableOperation<ProgramTable>.identity_map;
            ApplicationMap = TableOperation<ApplicationTable>.identity_map;
        }

    }
}
