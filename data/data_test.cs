using Dapper;
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

            Application a = Application.Create(new Application(1, 1));
            /*a.Address = "Nová Adresa 420, Ostrava 111 11";
            a.Refresh();
            a.Commit();*/ 
            // create DB
            /*
            using (var connection = new SqliteConnection(connection_string))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = File.ReadAllText("..\\..\\..\\data\\database-create.sql");
                cmd.ExecuteNonQuery();
            }
            */

        }

        static private void initDB()
        {
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.SQLite);
            Student.SetConnectionString(connection_string);
            School.SetConnectionString(connection_string);
            Program.SetConnectionString(connection_string);
            Application.SetConnectionString(connection_string);
        }

    }
}
