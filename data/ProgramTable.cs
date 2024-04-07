using CS_projekt.application;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_projekt.data
{
    public class ProgramTable : ITable
    {

        public int? Id { get; set; }
        public int? SchoolId { get; set; }
        public SchoolTable? School { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Capacity { get; set; }
        public int? ApplicationCount { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastUpdated { get; set; }
        public Dictionary<int, ApplicationTable> Applications = new Dictionary<int, ApplicationTable>();

        public ProgramTable()
        {
            LastUpdated = DateTime.Now;
        }

        public ProgramTable(int schoolId, string name, string? description, int capacity)
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
                School = TableOperation<SchoolTable>.GetRelated(SchoolId.Value);
                School.Programs[Id.Value] = this;
            }
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
            if (type == typeof(SchoolTable))
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
                School.RemoveRelative(Id.Value, typeof(ProgramTable));
            }
            foreach (var application in Applications.Values)
            {
                application.RemoveRelative(Id.Value, typeof(ProgramTable));
            }
        }
    }
}
