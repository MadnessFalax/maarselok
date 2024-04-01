using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_projekt.data
{
    [Table("student")]
    internal class Student
    {
        static private string connectionString = "";
        static public void SetConnectionString(string connectionString)
        {
            Student.connectionString = connectionString;
        }
        static public IDictionary<int, Student> identity_map = new Dictionary<int, Student>();
        [Key]
        [Column("id")]
        public int? Id { get; set; }
        [Column("name")]
        public string? Name { get; set; }
        [Column("address")]
        public string? Address { get; set; }
        [Column("email")]
        public string? Email { get; set; }
        [Column("application_count")]
        public int? ApplicationCount { get; set; }
        [Column("created")]
        public DateTime? Created { get; set; }

        // for dapper
        private Student(Int64 Id, String Name, String Address, String Email, Int64 ApplicationCount, String Created)
        {
            this.Id = (int)Id;
            this.Name = Name;
            this.Address = Address;
            this.Email = Email;  
            this.ApplicationCount = (int)ApplicationCount;
            this.Created = DateTime.Parse(Created);  
        }

        public Student(int id) 
        {
            Id = id;
        }

        public Student(string name, string address, string email)
        {
            Name = name;
            Address = address;
            Email = email;
            ApplicationCount = 0;
            Created = DateTime.Now;
        }

        static public Student Read(int id)
        {
            Student? student = null;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                student = connection.Get<Student>(id);
            }
            
            if(student != null)
            {
                return student;
            }
            else
            {
                throw new Exception("Invalid Id");
            }

        }

        static public Student Read(Student entity)
        {
            if (!entity.Id.HasValue)
            {
                return Student.Create(entity);
            }

            Student stud;

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                stud = connection.Get<Student>(entity.Id.Value);
            }

            return stud;
        }

        static public ICollection<Student> ReadAll()
        {
            IEnumerable<Student> tmp;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                tmp = connection.GetList<Student>();
            }

            List<Student> list = new();

            foreach(Student stud in tmp)
            {
                if (!stud.Id.HasValue)
                {
                    // should not occur
                    throw new Exception("Entity missing Id");
                }
                identity_map[stud.Id.Value] = stud;
                list.Add(stud);
            }

            return list;
        }

        // ex: new {Name = "Joe Mamma", Address: "17. listopadu 6969, 420 69 Ostrava"};
        static public ICollection<Student> ReadWithQuery(object whereConds)
        {
            IEnumerable<Student> tmp;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                tmp = connection.GetList<Student>(whereConds);
            }

            List<Student> list = new();

            foreach (Student stud in tmp)
            {
                if (!stud.Id.HasValue)
                {
                    // should not occur
                    throw new Exception("Entity missing Id");
                }
                identity_map[stud.Id.Value] = stud;
                list.Add(stud);
            }

            return list;
        }

        static public Student Create(Student entity)
        {
            if (entity.Id.HasValue)
            {
                return Student.Update(entity);
            }
            int? id = null;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                id = connection.Insert<Student>(entity);
            }

            if (id.HasValue)
            {
                var tmp = new Student(id.Value);
                tmp.Refresh();
                identity_map[id.Value] = tmp;
                return tmp;
            }
            else
            {
                throw new Exception("Student could not have been inserted");
            }

        }

        static public Student Update(Student entity)
        {
            if (!entity.Id.HasValue)
            {
                return Student.Create(entity);
            }

            var updatedCount = 0;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                updatedCount = connection.Update<Student>(entity);
            }

            if (updatedCount == 0)
            {
                throw new Exception("Record with this Id not found.");
            }

            return entity;
        }

        static public Student? Delete(Student entity)
        {
            using var connection = new SqliteConnection(connectionString);

            connection.Open();
            
            using var transaction = connection.BeginTransaction();
            
            var deletedCount = connection.Delete<Student>(entity);

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
                var tmp = Student.Read(this.Id.Value);
                this.Name = tmp.Name;
                this.Address = tmp.Address;
                this.Created = tmp.Created;
                this.Email = tmp.Email;
                this.ApplicationCount = tmp.ApplicationCount;
            }
        }

        public void Commit()
        {
            if (this.Id.HasValue)
            {
                var tmp = Student.Update(this);
                this.Name = tmp.Name;
                this.Address = tmp.Address;
                this.Created = tmp.Created;
                this.Email = tmp.Email;
                this.ApplicationCount = tmp.ApplicationCount;
            }
        }
    }
}
