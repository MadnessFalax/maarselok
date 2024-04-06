using CS_projekt.application;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_projekt.data
{
    internal class Program : ITable
    {

        public int? Id { get; set; }
        public int? SchoolId { get; set; }
        public School? School { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Capacity { get; set; }
        public int? ApplicationCount { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastUpdated { get; set; }
        public Dictionary<int, Application> Applications = new Dictionary<int, Application>();

        public Program()
        {
            LastUpdated = DateTime.Now;
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

        public void MapRelations()
        {
            if (SchoolId.HasValue)
            {
                School = TableOperation<School>.GetRelated(SchoolId.Value);
                School.Programs[Id.Value] = this;
            }
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
            if (type == typeof(School))
            {
                if (School.Id.Value == id)
                {
                    School = null;
                }
            }
        }

        public void RemoveSelfFromRelatives()
        {
            if (School != null)
            {
                School.RemoveRelative(Id.Value, typeof(Program));
            }
            foreach (var application in Applications.Values)
            {
                application.RemoveRelative(Id.Value, typeof(Program));
            }
        }
    }
}
