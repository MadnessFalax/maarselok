using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_projekt.data
{
    internal class Application : ITable
    {
        public int? Id { get; set; }
        public int? StudentId { get; set; }
        public Student? Student { get; set; }
        public int? ProgramId { get; set; }
        public Program? Program { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastUpdated { get; set; }

        public Application()
        {
            Created = DateTime.Now;
        }

        public Application(int studentId, int programId)
        {
            StudentId = studentId;
            ProgramId = programId;
            Created = DateTime.Now;
        }

        public void MapRelations()
        {
            if (this.StudentId.HasValue)
            {
                Student = TableOperation<Student>.GetRelated(StudentId.Value);
                Student.Applications[Id.Value] = this;
            }
            if (this.ProgramId.HasValue)
            {
                Program = TableOperation<Program>.GetRelated(ProgramId.Value);
                Program.Applications[Id.Value] = this;
            }
        }

        public void RemoveRelative(int id, Type type)
        {
            if (type == typeof(Program))
            {
                if (Program.Id.Value == id)
                {
                    Program = null;
                }
            }
            if (type == typeof(Student))
            {
                if (Student.Id.Value == id)
                {
                    Student = null;
                }
            }
        }

        public void RemoveSelfFromRelatives()
        {
            if (Student != null)
            {
                Student.RemoveRelative(Id.Value, typeof(Application));
            }
            if (Program != null)
            {
                Program.RemoveRelative(Id.Value, typeof(Program));
            }
        }
    }
}
