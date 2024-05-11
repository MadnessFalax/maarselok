using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CS_projekt.data;

namespace desktop_client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public struct SchoolControl
        {
            public bool Checked { get; set; }
            public SchoolTable Entry { get; set; }

            public SchoolControl(bool c, SchoolTable t)
            {
                Checked = c;
                Entry = t;
            }
        }

        public struct StudentControl
        {
            public bool Checked { get; set; }
            public StudentTable Entry { get; set; }

            public StudentControl(bool c, StudentTable t)
            {
                Checked = c;
                Entry = t;
            }
        }

        public struct ProgramControl
        {
            public bool Checked { get; set; }
            public ProgramTable Entry { get; set; }

            public ProgramControl(bool c, ProgramTable t)
            {
                Checked = c;
                Entry = t;
            }
        }

        public struct ApplicationControl
        {
            public bool Checked { get; set; }
            public ApplicationTable Entry { get; set; }

            public ApplicationControl(bool c, ApplicationTable t)
            {
                Checked = c;
                Entry = t;
            }
        }


        public ObservableCollection<SchoolControl> schools { get; set; } = new ObservableCollection<SchoolControl>();
        public ObservableCollection<StudentControl> students { get; set; } = new ObservableCollection<StudentControl>();
        public ObservableCollection<ApplicationControl> applications { get; set; } = new ObservableCollection<ApplicationControl>();
        public ObservableCollection<ProgramControl> programs { get; set; } = new ObservableCollection<ProgramControl>();

        public string SchoolSearch { get; set; } = "Vyplňte";
        public string StudentSearch { get; set; } = "Vyplňte";
        public string ApplicationSearch { get; set; } = "Vyplňte";
        public string ProgramSearch { get; set; } = "Vyplňte";

        public MainWindow()
        {
            LoadSchools();
            LoadStudents();
            LoadPrograms();
            LoadApplications();
            InitializeComponent();
            DataContext = this;
        }

        private void LoadSchools()
        {
            schools = new ObservableCollection<SchoolControl>();
            var sch_map = DataEntryPoint.SchoolMap;
            foreach(var item in sch_map)
            {
                schools.Add(new SchoolControl(false, item.Value));
            }
        }

        private void LoadStudents()
        {
            students = new ObservableCollection<StudentControl>();
            var stu_map = DataEntryPoint.StudentMap;
            foreach (var item in stu_map)
            {
                students.Add(new StudentControl(false, item.Value));
            }
        }

        private void LoadPrograms()
        {
            programs = new ObservableCollection<ProgramControl>();
            var pro_map = DataEntryPoint.ProgramMap;
            foreach (var item in pro_map)
            {
                programs.Add(new ProgramControl(false, item.Value));
            }
        }

        private void LoadApplications()
        {
            applications = new ObservableCollection<ApplicationControl>();
            var app_map = DataEntryPoint.ApplicationMap;
            foreach (var item in app_map)
            {
                applications.Add(new ApplicationControl(false, item.Value));
            }
        }

        private void SchoolDeleteCallback(object sender, RoutedEventArgs e)
        {
            var marked_for_deletion = new List<SchoolControl>();

            foreach(var school in schools)
            {
                if (school.Checked)
                {
                    SchoolTable entry = school.Entry;
                    TableOperation<SchoolTable>.Delete(ref entry);
                    marked_for_deletion.Add(school);
                }
            }

            foreach(var school in marked_for_deletion)
            {
                schools.Remove(school);
            }

            LoadSchools();
        }

        private void SchoolEditCallback(object sender, RoutedEventArgs e)
        {
            var dialog = new Dialog();
            dialog.Message = "A nejsi ty tak trošku retard? Proč mi jako dáváš tak dlouhou otázku? Co s ní mám jako dělat?";
            dialog.ShowDialog();
        }

        private void SchoolCreateCallback(object sender, RoutedEventArgs e)
        {

        }

        private void SchoolSearchCallback(object sender, RoutedEventArgs e)
        {

        }

        private void SchoolRefreshCallback(object sender, RoutedEventArgs e)
        {

        }
        private void StudentDeleteCallback(object sender, RoutedEventArgs e)
        {
            var marked_for_deletion = new List<StudentControl>();

            foreach (var student in students)
            {
                if (student.Checked)
                {
                    StudentTable entry = student.Entry;
                    TableOperation<StudentTable>.Delete(ref entry);
                    marked_for_deletion.Add(student);
                }
            }

            foreach (var student in marked_for_deletion)
            {
                students.Remove(student);
            }
        }

        private void StudentEditCallback(object sender, RoutedEventArgs e)
        {

        }

        private void StudentCreateCallback(object sender, RoutedEventArgs e)
        {

        }

        private void StudentSearchCallback(object sender, RoutedEventArgs e)
        {

        }

        private void StudentRefreshCallback(object sender, RoutedEventArgs e)
        {

        }
        private void ApplicationDeleteCallback(object sender, RoutedEventArgs e)
        {
            var marked_for_deletion = new List<ApplicationControl>();

            foreach (var application in applications)
            {
                if (application.Checked)
                {
                    ApplicationTable entry = application.Entry;
                    TableOperation<ApplicationTable>.Delete(ref entry);
                    marked_for_deletion.Add(application);
                }
            }

            foreach (var application in marked_for_deletion)
            {
                applications.Remove(application);
            }
        }

        private void ApplicationEditCallback(object sender, RoutedEventArgs e)
        {

        }

        private void ApplicationCreateCallback(object sender, RoutedEventArgs e)
        {

        }

        private void ApplicationSearchCallback(object sender, RoutedEventArgs e)
        {

        }

        private void ApplicationRefreshCallback(object sender, RoutedEventArgs e)
        {

        }
        private void ProgramDeleteCallback(object sender, RoutedEventArgs e)
        {
            var marked_for_deletion = new List<ProgramControl>();

            foreach (var program in programs)
            {
                if (program.Checked)
                {
                    ProgramTable entry = program.Entry;
                    TableOperation<ProgramTable>.Delete(ref entry);
                    marked_for_deletion.Add(program);
                }
            }

            foreach (var program in marked_for_deletion)
            {
                programs.Remove(program);
            }

            LoadPrograms();
        }

        private void ProgramEditCallback(object sender, RoutedEventArgs e)
        {
            var dialog = new ProgramEditor();
            dialog.ShowDialog();
        }

        private void ProgramCreateCallback(object sender, RoutedEventArgs e)
        {

        }

        private void ProgramSearchCallback(object sender, RoutedEventArgs e)
        {

        }

        private void ProgramRefreshCallback(object sender, RoutedEventArgs e)
        {

        }
    }
}
