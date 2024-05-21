using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CS_projekt.data
{

    using TOStudent = CS_projekt.data.TableOperation<StudentTable>;
    using TOSchool = CS_projekt.data.TableOperation<SchoolTable>;
    using TOApplication = CS_projekt.data.TableOperation<ApplicationTable>;
    using TOProgram = CS_projekt.data.TableOperation<ProgramTable>;


    public class DataEntryPoint
    {
        static public string connection_string = "Data Source=mydb.db;";

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
            TOStudent.RefreshAll();
            TOSchool.RefreshAll();
            TOProgram.RefreshAll();
            TOApplication.RefreshAll();

            StudentMap = TOStudent.identity_map;
            SchoolMap = TOSchool.identity_map;
            ProgramMap  = TOProgram.identity_map;
            ApplicationMap = TOApplication.identity_map;
        }

    }
}
