using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_projekt.data.view_attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Property)]
    public class ViewNameAttribute : Attribute
    {
        public string Name { get; set; }

        public ViewNameAttribute(string view_name)
        {
            Name = view_name;
        }
    }
}
