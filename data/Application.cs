using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_projekt.data
{
    [Table("application")]
    internal class Application
    {
        static private string connectionString = "";
        
        static public void SetConnectionString(string connectionString)
        {
            Application.connectionString = connectionString;
        }
        
        static public IDictionary<int, Application> identity_map = new Dictionary<int, Application>();
        [Key]
        [Column("id")]
        public int? Id { get; set; }
        [Column("student_id")]
        public int? StudentId { get; set; }
        [Column("program_id")]
        public int? ProgramId { get; set; }
        [Column("created")]
        public DateTime? Created { get; set; }

        // dapper
        public Application(Int64 Id, Int64 StudentId, Int64 ProgramId, String Created)
        {
            this.Id = (int)Id;
            this.StudentId = (int)StudentId;
            this.ProgramId = (int)ProgramId;
            this.Created = DateTime.Parse(Created);
        }

        public Application (int studentId, int programId)
        {
            StudentId = studentId;
            ProgramId = programId;
            Created = DateTime.Now;
        }

        public Application (int id)
        {
            Id = id;
        }

        static public Application Read(int id)
        {
            Application? application = null;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                application = connection.Get<Application>(id);
            }

            if (application != null)
            {
                return application;
            }
            else
            {
                throw new Exception("Invalid Id");
            }

        }

        static public Application Read(Application entity)
        {
            if (!entity.Id.HasValue)
            {
                return Application.Create(entity);
            }

            Application app;

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                app = connection.Get<Application>(entity.Id.Value);
            }

            return app;
        }

        static public ICollection<Application> ReadAll()
        {
            IEnumerable<Application> tmp;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                tmp = connection.GetList<Application>();
            }

            List<Application> list = new();

            foreach (Application app in tmp)
            {
                if (!app.Id.HasValue)
                {
                    // should not occur
                    throw new Exception("Entity missing Id");
                }
                identity_map[app.Id.Value] = app;
                list.Add(app);
            }

            return list;
        }

        // ex: new {Name = "Joe Mamma", Address: "17. listopadu 6969, 420 69 Ostrava"};
        static public ICollection<Application> ReadWithQuery(object whereConds)
        {
            IEnumerable<Application> tmp;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                tmp = connection.GetList<Application>(whereConds);
            }

            List<Application> list = new();

            foreach (Application app in tmp)
            {
                if (!app.Id.HasValue)
                {
                    // should not occur
                    throw new Exception("Entity missing Id");
                }
                identity_map[app.Id.Value] = app;
                list.Add(app);
            }

            return list;
        }

        static public Application Create(Application entity)
        {
            if (entity.Id.HasValue)
            {
                return Application.Update(entity);
            }
            int? id = null;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                id = connection.Insert<Application>(entity);
            }

            if (id.HasValue)
            {
                var tmp = new Application(id.Value);
                tmp.Refresh();
                identity_map[id.Value] = tmp;
                return tmp;
            }
            else
            {
                throw new Exception("Application could not have been inserted");
            }

        }

        static public Application Update(Application entity)
        {
            if (!entity.Id.HasValue)
            {
                return Application.Create(entity);
            }

            var updatedCount = 0;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                updatedCount = connection.Update<Application>(entity);
            }

            if (updatedCount == 0)
            {
                throw new Exception("Record with this Id not found.");
            }

            return entity;
        }

        static public Application? Delete(Application entity)
        {
            using var connection = new SqliteConnection(connectionString);

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var deletedCount = connection.Delete<Application>(entity);

            if (deletedCount != 1)
            {
                transaction.Rollback();
                throw new Exception("Multiple records affected! ROLLING BACK!");
            }
            else
            {
                transaction.Commit();
            }
            return null;
        }

        public void Refresh()
        {
            if (this.Id.HasValue)
            {
                var tmp = Application.Read(this.Id.Value);
                this.StudentId = tmp.StudentId;
                this.ProgramId = tmp.ProgramId;
                this.Created = tmp.Created;
            }
        }

        public void Commit()
        {
            if (this.Id.HasValue)
            {
                var tmp = Application.Update(this);
            }
        }
    }
}
