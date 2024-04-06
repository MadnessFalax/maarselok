using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_projekt.data
{
    internal class School : ITable
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastUpdated { get; set; }
        public Dictionary<int, Program> Programs = new Dictionary<int, Program>();

        public School() 
        {
            LastUpdated = DateTime.Now;
        }

        public School(string name, string? address)
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

            if (type == typeof(Program))
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
                program.RemoveRelative(Id.Value, typeof(School));
            }
        }
    }
}
