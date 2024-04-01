using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_projekt.data
{
    [Table("program")]
    internal class Program
    {
        static private string connectionString = "";
        static public void SetConnectionString(string connectionString)
        {
            Program.connectionString = connectionString;
        }
        static public IDictionary<int, Program> identity_map = new Dictionary<int, Program>();
        [Key]
        [Column("id")]
        public int? Id { get; set; }
        [Column("school_id")]
        public int? SchoolId { get; set; }
        [Column("name")]
        public string? Name { get; set; }
        [Column("description")]
        public string? Description { get; set; }
        [Column("capacity")]
        public int? Capacity { get; set; }
        [Column("application_count")]
        public int? ApplicationCount { get; set; }
        [Column("created")]
        public DateTime? Created { get; set; }

        // dapper
        private Program(Int64 Id, Int64 SchoolId, String Name, String Description, Int64 Capacity, Int64 ApplicationCount, String Created)
        {
            this.Id = (int)Id;
            this.SchoolId = (int)SchoolId;
            this.Name = Name;
            this.Description = Description;
            this.Capacity = (int)Capacity;
            this.ApplicationCount = (int)ApplicationCount;
            this.Created = DateTime.Parse(Created);
        }

        public Program(int schoolId, string name, string? description, int capacity)
        {
            SchoolId = schoolId;
            Name = name;
            Description = description;
            Capacity = capacity;
            ApplicationCount = 0;
            Created = DateTime.Now;
        }

        public Program(int id)
        {
            Id = id;
        }

        static public Program Read(int id)
        {
            Program? program = null;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                program = connection.Get<Program>(id);
            }

            if (program != null)
            {
                return program;
            }
            else
            {
                throw new Exception("Invalid Id");
            }

        }

        static public Program Read(Program entity)
        {
            if (!entity.Id.HasValue)
            {
                return Program.Create(entity);
            }

            Program prog;

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                prog = connection.Get<Program>(entity.Id.Value);
            }

            return prog;
        }

        static public ICollection<Program> ReadAll()
        {
            IEnumerable<Program> tmp;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                tmp = connection.GetList<Program>();
            }

            List<Program> list = new();

            foreach (Program prog in tmp)
            {
                if (!prog.Id.HasValue)
                {
                    // should not occur
                    throw new Exception("Entity missing Id");
                }
                identity_map[prog.Id.Value] = prog;
                list.Add(prog);
            }

            return list;
        }

        // ex: new {Name = "Joe Mamma", Address: "17. listopadu 6969, 420 69 Ostrava"};
        static public ICollection<Program> ReadWithQuery(object whereConds)
        {
            IEnumerable<Program> tmp;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                tmp = connection.GetList<Program>(whereConds);
            }

            List<Program> list = new();

            foreach (Program prog in tmp)
            {
                if (!prog.Id.HasValue)
                {
                    // should not occur
                    throw new Exception("Entity missing Id");
                }
                identity_map[prog.Id.Value] = prog;
                list.Add(prog);
            }

            return list;
        }

        static public Program Create(Program entity)
        {
            if (entity.Id.HasValue)
            {
                return Program.Update(entity);
            }
            int? id = null;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                id = connection.Insert<Program>(entity);
            }

            if (id.HasValue)
            {
                var tmp = new Program(id.Value);
                tmp.Refresh();
                identity_map[id.Value] = tmp;
                return tmp;
            }
            else
            {
                throw new Exception("Program could not have been inserted");
            }

        }

        static public Program Update(Program entity)
        {
            if (!entity.Id.HasValue)
            {
                return Program.Create(entity);
            }

            var updatedCount = 0;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                updatedCount = connection.Update<Program>(entity);
            }

            if (updatedCount == 0)
            {
                throw new Exception("Record with this Id not found.");
            }

            return entity;
        }

        static public Program? Delete(Program entity)
        {
            using var connection = new SqliteConnection(connectionString);

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var deletedCount = connection.Delete<Program>(entity);

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
                var tmp = Program.Read(this.Id.Value);
                this.Name = tmp.Name;
                this.ApplicationCount = tmp.ApplicationCount;
                this.Created = tmp.Created;
                this.Capacity = tmp.Capacity;
                this.SchoolId = tmp.SchoolId;
                this.Description = tmp.Description;
            }
        }

        public void Commit()
        {
            if (this.Id.HasValue)
            {
                var tmp = Program.Update(this);
                this.Name = tmp.Name;
                this.ApplicationCount = tmp.ApplicationCount;
                this.Created = tmp.Created;
                this.Capacity = tmp.Capacity;
                this.SchoolId = tmp.SchoolId;
                this.Description = tmp.Description;
            }
        }
    }
}
