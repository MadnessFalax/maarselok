using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CS_projekt.data.table_attributes;

namespace CS_projekt.data
{
    [Table("StudentTable")]
    public class StudentTable : ITable
    {
        [Column("Id", true), PrimaryKey]
        public int? Id { get; set; }
        [Column("Name")]
        public string? Name { get; set; }
        [Column("Address")]
        public string? Address { get; set; }
        [Column("Email")]
        public string? Email { get; set; }
        [Column("Password")]
        public string? Password { get; set; }
        [Column("ApplicationCount")]
        public int? ApplicationCount { get; set; }
        [Column("Created", true)]
        public DateTime? Created { get; set; }
        [Column("LastUpdated", true)]
        public DateTime? LastUpdated { get; set; }
        public Dictionary<int, ApplicationTable> Applications = new Dictionary<int, ApplicationTable>();

        public StudentTable() 
        {
            LastUpdated = DateTime.Now;
        }

        public StudentTable(string name, string address, string email, string password)
        {
            Name = name;
            Address = address;
            Email = email;
            ApplicationCount = 0;
            Created = DateTime.Now;
            LastUpdated = DateTime.Now;
            Password = password;
        }

        public void MapRelations()
        {

        }

        public void RemoveRelative(int id, Type type)
        {
            if (type == typeof(ApplicationTable)) 
            {
                if (Applications.ContainsKey(id))
                {
                    Applications.Remove(id);
                }
            }
        }
        
        public void RemoveSelfFromRelatives()
        {
            foreach(var application in Applications.Values)
            {
                application.RemoveRelative(Id.Value, typeof(StudentTable));
            }
        }
    }
}
