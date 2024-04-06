using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_projekt.data
{
    internal interface ITable
    {
        public int? Id { get; set; }
        public DateTime? LastUpdated { get; set; }
        public void MapRelations();
        public void RemoveRelative(int id, Type type);
        public void RemoveSelfFromRelatives();
    }
}
