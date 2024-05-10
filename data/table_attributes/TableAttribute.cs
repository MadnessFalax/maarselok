using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_projekt.data.table_attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public string Name { get; set; }

        public TableAttribute(string table_name)
        {
            Name = table_name;
        }
    }
}
