using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_projekt.data
{
    public class SchoolTable : ITable
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastUpdated { get; set; }
        public Dictionary<int, ProgramTable> Programs = new Dictionary<int, ProgramTable>();

        public SchoolTable() 
        {
            LastUpdated = DateTime.Now;
        }

        public SchoolTable(string name, string? address)
        {
            Name = name;
            Address = address;
            Created = DateTime.Now;
        }

        public void MapRelations()
        {

        }

        public void RemoveRelative(int id, Type type) 
        {

            if (type == typeof(ProgramTable))
            {
                if (Programs.ContainsKey(id))
                {
                    Programs.Remove(id);
                }
            }
        }

        public void RemoveSelfFromRelatives()
        {
            foreach(var program in Programs.Values) 
            {
                program.RemoveRelative(Id.Value, typeof(SchoolTable));
            }
        }
    }
}
