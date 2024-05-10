using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_projekt.data.table_attributes
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ForeignKeyAttribute : Attribute
    {
        public string Name { get; set; }

        public ForeignKeyAttribute(string fk_name)
        {
            Name = fk_name;
        }
    }
}
