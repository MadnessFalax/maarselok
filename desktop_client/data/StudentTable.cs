using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CS_projekt.data.table_attributes;
using CS_projekt.data.view_attributes;

namespace CS_projekt.data
{
    [Table("StudentTable"), ViewName("Students")]
    public class StudentTable : ITable
    {
        [Column("Id", true), PrimaryKey, ViewName("ID")]
        public int? Id { get; set; }
        [Column("Name"), ViewName("Name")]
        public string? Name { get; set; }
        [Column("Address"), ViewName("Address")]
        public string? Address { get; set; }
        [Column("Email"), ViewName("Email")]
        public string? Email { get; set; }
        [Column("Password"), ViewName("Password")]
        public string? Password { get; set; }
        [Column("ApplicationCount"), ViewName("Application Count")]
        public int? ApplicationCount { get; set; }
        [Column("Created", true), ViewName("Created")]
        public DateTime? Created { get; set; }
        [Column("LastUpdated", true), ViewName("Last Updated")]
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
