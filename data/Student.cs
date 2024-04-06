using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CS_projekt.data
{
    internal class Student : ITable
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public int? ApplicationCount { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastUpdated { get; set; }
        public Dictionary<int, Application> Applications = new Dictionary<int, Application>();

        public Student() 
        {
            LastUpdated = DateTime.Now;
        }

        public Student(string name, string address, string email)
        {
            Name = name;
            Address = address;
            Email = email;
            ApplicationCount = 0;
            Created = DateTime.Now;
            LastUpdated = DateTime.Now;

        }

        public void MapRelations()
        {

        }

        public void RemoveRelative(int id, Type type)
        {
            if (type == typeof(Application)) 
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
                application.RemoveRelative(Id.Value, typeof(Student));
            }
        }
    }
}
