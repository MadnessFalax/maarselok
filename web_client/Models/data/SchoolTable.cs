﻿using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS_projekt.data.table_attributes;
using CS_projekt.data.view_attributes;

namespace CS_projekt.data
{
    [Table("SchoolTable"), ViewName("Schools")]
    public class SchoolTable : ITable
    {
        [Column("Id", true), PrimaryKey, ViewName("ID")]
        public int? Id { get; set; }
        [Column("Name"), ViewName("Name")]
        public string? Name { get; set; }
        [Column("Address"), ViewName("Address")]
        public string? Address { get; set; }
        [Column("Created", true), ViewName("Created")]
        public DateTime? Created { get; set; }
        [Column("LastUpdated", true), ViewName("Last Updated")]
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
