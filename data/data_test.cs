using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CS_projekt.data
{

    internal class data_test
    {
        static string connection_string = "Data Source=mydb.db;";
        
        static void Main()
        {
            initDB();

            TableOperation<Student>.RefreshAll();
            TableOperation<School>.RefreshAll();
            // create DB schema
            /*using (var connection = new SqliteConnection(connection_string))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = File.ReadAllText("..\\..\\..\\data\\database-create.sql");
                cmd.ExecuteNonQuery();
            }
            */

            var stud_map = TableOperation<Student>.identity_map;
            var school_map = TableOperation<School>.identity_map;

            var school = school_map[1];
            school.Address = "Upravená adresa";

            TableOperation<School>.Update(ref school);

        }

        static private void initDB()
        {
            DBGateway<Student>.setConnectionString(connection_string);
            DBGateway<School>.setConnectionString(connection_string);
            DBGateway<Application>.setConnectionString(connection_string);
            DBGateway<Program>.setConnectionString(connection_string);
        }

    }
}
