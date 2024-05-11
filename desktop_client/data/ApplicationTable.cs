using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS_projekt.data.table_attributes;

namespace CS_projekt.data
{
    [Table("ApplicationTable")]
    public class ApplicationTable : ITable
    {
        [Column("Id", true), PrimaryKey]
        public int? Id { get; set; }
        [Column("StudentId"), ForeignKey("Student")]
        public int? StudentId { get; set; }
        [ForeignTable("Student")]
        public StudentTable? Student { get; set; }
        [Column("ProgramId"), ForeignKey("Program")]
        public int? ProgramId { get; set; }
        [ForeignTable("Program")]
        public ProgramTable? Program { get; set; }
        [Column("Created", true)]
        public DateTime? Created { get; set; }
        [Column("LastUpdated", true)]
        public DateTime? LastUpdated { get; set; }

        public ApplicationTable()
        {
            Created = DateTime.Now;
        }

        public ApplicationTable(int studentId, int programId)
        {
            StudentId = studentId;
            ProgramId = programId;
            Created = DateTime.Now;
        }

        public void MapRelations()
        {
            if (this.StudentId.HasValue)
            {
                Student = TableOperation<StudentTable>.GetRelated(StudentId.Value);
                Student.Applications[Id.Value] = this;
            }
            if (this.ProgramId.HasValue)
            {
                Program = TableOperation<ProgramTable>.GetRelated(ProgramId.Value);
                Program.Applications[Id.Value] = this;
            }
        }

        public void RemoveRelative(int id, Type type)
        {
            if (type == typeof(ProgramTable))
            {
                if (Program.Id.Value == id)
                {
                    Program = null;
                }
            }
            if (type == typeof(StudentTable))
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
                Student.RemoveRelative(Id.Value, typeof(ApplicationTable));
            }
            if (Program != null)
            {
                Program.RemoveRelative(Id.Value, typeof(ProgramTable));
            }
        }
    }
}
