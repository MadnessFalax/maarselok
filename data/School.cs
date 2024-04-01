using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_projekt.data
{
    [Table("school")]
    internal class School
    {
        static private string connectionString = "";

        static public void SetConnectionString(string connectionString)
        {
            School.connectionString = connectionString;
        }
        
        static public IDictionary<int, School> identity_map = new Dictionary<int, School>();
        [Key]
        [Column("id")]
        public int? Id { get; set; }
        [Column("name")]
        public string? Name { get; set; }
        [Column("address")]
        public string? Address { get; set; }
        [Column("created")]
        public DateTime? Created { get; set; }

        // dapper
        private School(Int64 Id, String Name, String Address, String Created)
        {
            this.Id = (int)Id;
            this.Name = Name;
            this.Address = Address;
            this.Created = DateTime.Parse(Created);
        }

        public School(string name, string? address)
        {
            Name = name;
            Address = address;
            Created = DateTime.Now;
        }

        public School(int id)
        {
            Id = id;
        }

        static public School Read(int id)
        {
            School? school = null;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                school = connection.Get<School>(id);
            }
            
            if(school != null)
            {
                return school;
            }
            else
            {
                throw new Exception("Invalid Id");
            }

        }

        static public School Read(School entity)
        {
            if (!entity.Id.HasValue)
            {
                return School.Create(entity);
            }

            School sch;

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                sch = connection.Get<School>(entity.Id.Value);
            }

            return sch;
        }

        static public ICollection<School> ReadAll()
        {
            IEnumerable<School> tmp;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                tmp = connection.GetList<School>();
            }

            List<School> list = new();

            foreach(School sch in tmp)
            {
                if (!sch.Id.HasValue)
                {
                    // should not occur
                    throw new Exception("Entity missing Id");
                }
                identity_map[sch.Id.Value] = sch;
                list.Add(sch);
            }

            return list;
        }

        // ex: new {Name = "Joe Mamma", Address: "17. listopadu 6969, 420 69 Ostrava"};
        static public ICollection<School> ReadWithQuery(object whereConds)
        {
            IEnumerable<School> tmp;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                tmp = connection.GetList<School>(whereConds);
            }

            List<School> list = new();

            foreach (School sch in tmp)
            {
                if (!sch.Id.HasValue)
                {
                    // should not occur
                    throw new Exception("Entity missing Id");
                }
                identity_map[sch.Id.Value] = sch;
                list.Add(sch);
            }

            return list;
        }

        static public School Create(School entity)
        {
            if (entity.Id.HasValue)
            {
                return School.Update(entity);
            }
            int? id = null;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                id = connection.Insert<School>(entity);
            }

            if (id.HasValue)
            {
                var tmp = new School(id.Value);
                tmp.Refresh();
                identity_map[id.Value] = tmp;
                return tmp;
            }
            else
            {
                throw new Exception("School could not have been inserted");
            }

        }

        static public School Update(School entity)
        {
            if (!entity.Id.HasValue)
            {
                return School.Create(entity);
            }

            var updatedCount = 0;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                updatedCount = connection.Update<School>(entity);
            }

            if (updatedCount == 0)
            {
                throw new Exception("Record with this Id not found.");
            }

            return entity;
        }

        static public School? Delete(School entity)
        {
            using var connection = new SqliteConnection(connectionString);

            connection.Open();
            
            using var transaction = connection.BeginTransaction();
            
            var deletedCount = connection.Delete<School>(entity);

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
                var tmp = School.Read(this.Id.Value);
                this.Name = tmp.Name;
                this.Address = tmp.Address;
                this.Created = tmp.Created;
            }
        }

        public void Commit()
        {
            if (this.Id.HasValue)
            {
                var tmp = School.Update(this);
                this.Name = tmp.Name;
                this.Address = tmp.Address;
                this.Created = tmp.Created;
            }
        }
    }
}
