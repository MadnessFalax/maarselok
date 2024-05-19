using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS_projekt.data.table_attributes;
using CS_projekt.data.view_attributes;

namespace CS_projekt.data
{
    [Table("ProgramTable"), ViewName("Programs")]
    public class ProgramTable : ITable
    {
        [Column("Id", true), PrimaryKey, ViewName("ID")]
        public int? Id { get; set; }
        [Column("SchoolId"), ForeignKey("School")]
        public int? SchoolId { get; set; }
        [ForeignTable("School"), ViewName("School")]
        public SchoolTable? School { get; set; }
        [Column("Name"), ViewName("Name")]
        public string? Name { get; set; }
        [Column("Description"), ViewName("Description")]
        public string? Description { get; set; }
        [Column("Capacity"), ViewName("Capacity")]
        public int? Capacity { get; set; }
        [Column("ApplicationCount"), ViewName("Application Count")]
        public int? ApplicationCount { get; set; }
        [Column("Created", true), ViewName("Created")]
        public DateTime? Created { get; set; }
        [Column("LastUpdated", true), ViewName("Last Updated")]
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
