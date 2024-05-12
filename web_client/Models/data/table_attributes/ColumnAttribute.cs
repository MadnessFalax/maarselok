using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_projekt.data.table_attributes
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public string Name { get; set; }
        public bool Unmanaged { get; set; }

        public ColumnAttribute(string column_name)
        {
            Name = column_name; 
            Unmanaged = false;
        }

        public ColumnAttribute(string column_name, bool unmanaged)
        {
            Name = column_name;
            Unmanaged = unmanaged;
        }
    }
}
