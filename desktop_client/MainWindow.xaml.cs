using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

    public class EntryControl<T> : INotifyPropertyChanged where T : ITable
    {
        private bool isChecked;
        public bool IsChecked { 
            get { return isChecked; } 
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }
        public T Entry { get; set; }

        public EntryControl(T entity)
        {
            IsChecked = false;
            Entry = entity;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName) 
        { 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class MainWindowModel : INotifyPropertyChanged
    {
        public ObservableCollection<EntryControl<SchoolTable>> schools { get; set; } = new ObservableCollection<EntryControl<SchoolTable>>();
        public ObservableCollection<EntryControl<StudentTable>> students { get; set; } = new ObservableCollection<EntryControl<StudentTable>>();
        public ObservableCollection<EntryControl<ApplicationTable>> applications { get; set; } = new ObservableCollection<EntryControl<ApplicationTable>>();
        public ObservableCollection<EntryControl<ProgramTable>> programs { get; set; } = new ObservableCollection<EntryControl<ProgramTable>>();

        public string SchoolSearch { get; set; } = "";
        public string StudentSearch { get; set; } = "";
        public string ApplicationSearch { get; set; } = "";
        public string ProgramSearch { get; set; } = "";

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public void LoadSchools(string? query = null)
        {
            var sch_map = DataEntryPoint.SchoolMap;
            if (query == null)
            {
                foreach (var item in sch_map)
                {
                    schools.Add(new EntryControl<SchoolTable>(item.Value));
                }
            }
            else
            {
                foreach(var item in sch_map)
                {
                    if (item.Value.Name.Contains(query))
                    {
                        schools.Add(new EntryControl<SchoolTable>(item.Value));
                    }
                }
            }
        }

        public void LoadStudents(string? query = null)
        {
            var stu_map = DataEntryPoint.StudentMap;
            if (query == null)
            {
                foreach (var item in stu_map)
                {
                    students.Add(new EntryControl<StudentTable>(item.Value));
                }
            }
            else
            {
                foreach (var item in stu_map)
                {
                    if (item.Value.Name.Contains(query))
                    {
                        students.Add(new EntryControl<StudentTable>(item.Value));
                    }
                }
            }
        }

        public void LoadPrograms(string? query = null)
        {
            var pro_map = DataEntryPoint.ProgramMap;
            if (query == null)
            {
                foreach (var item in pro_map)
                {
                    programs.Add(new EntryControl<ProgramTable>(item.Value));
                }
            }
            else
            {
                foreach (var item in pro_map)
                {
                    if (item.Value.Name.Contains(query))
                    {
                        programs.Add(new EntryControl<ProgramTable>(item.Value));
                    }
                }
            }
        }

        public void LoadApplications()
        {
            var app_map = DataEntryPoint.ApplicationMap;
            foreach (var item in app_map)
            {
                applications.Add(new EntryControl<ApplicationTable>(item.Value));
            }
        }
    }

    public partial class MainWindow : Window
    {
        public MainWindowModel Model = new MainWindowModel();
        public MainWindow()
        {
            Model.LoadSchools();
            Model.LoadStudents();
            Model.LoadPrograms();
            Model.LoadApplications();
            InitializeComponent();
            DataContext = Model;
        }


        private void SchoolDeleteCallback(object sender, RoutedEventArgs e)
        {
            var dialog = new DialogQuestion();
            dialog.Model.Message = "Are you sure you want to delete selected entries?";
            dialog.ShowDialog();
            var answer = dialog.Model.Response;
            if (answer.Value)
            {
                var for_deletion = Model.schools.Where(x => x.IsChecked).Select(x => x.Entry).ToList();
                var size = for_deletion.Count;

                for (int i = 0; i < size; i++)
                {
                    var tmp = for_deletion[i];

                    TableOperation<SchoolTable>.Delete(ref tmp);
                }

                TableOperation<SchoolTable>.ForceRefreshAll();
                Model.schools.Clear();
                Model.LoadSchools();
            }
        }

        private void SchoolEditCallback(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var ctx = btn.DataContext as EntryControl<SchoolTable>;
            var dialog = new SchoolEditor(ctx.Entry.Name, ctx.Entry.Address);
            dialog.ShowDialog();
            if (dialog.Model.Ok)
            {
                ctx.Entry.Name = dialog.Model.nameEntry.Model.Text;
                ctx.Entry.Address = dialog.Model.addressEntry.Model.Text;
                var tmp = ctx.Entry;
                TableOperation<SchoolTable>.Update(ref tmp);
                Model.schools.Clear();
                Model.LoadSchools();
            }
        }

        private void SchoolCreateCallback(object sender, RoutedEventArgs e)
        {
            var dialog = new SchoolEditor();
            dialog.ShowDialog();
            if (dialog.Model.Ok)
            {
                var entity = new SchoolTable(dialog.Model.nameEntry.Model.Text, dialog.Model.addressEntry.Model.Text);
                TableOperation<SchoolTable>.Create(entity);
                TableOperation<SchoolTable>.ForceRefreshAll();
                Model.schools.Clear();
                Model.LoadSchools();
            }
        }

        private void SchoolSearchCallback(object sender, RoutedEventArgs e)
        {
            if (Model.SchoolSearch.Equals(""))
            {
                Model.schools.Clear();
                Model.LoadSchools();
            }
            else
            {
                Model.schools.Clear();
                Model.LoadSchools(Model.SchoolSearch);
            }
        }

        private void SchoolRefreshCallback(object sender, RoutedEventArgs e)
        {
            TableOperation<SchoolTable>.RefreshAll();
            Model.schools.Clear();
            Model.LoadSchools();
        }
        private void StudentDeleteCallback(object sender, RoutedEventArgs e)
        {
            var dialog = new DialogQuestion();
            dialog.Model.Message = "Are you sure you want to delete selected entries?";
            dialog.ShowDialog();
            var answer = dialog.Model.Response;
            if (answer.Value)
            {
                var for_deletion = Model.students.Where(x => x.IsChecked).Select(x => x.Entry).ToList();
                var size = for_deletion.Count;

                for (int i = 0; i < size; i++)
                {
                    var tmp = for_deletion[i];

                    TableOperation<StudentTable>.Delete(ref tmp);
                }

                TableOperation<StudentTable>.ForceRefreshAll();
                Model.students.Clear();
                Model.LoadSchools();
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
            if (Model.StudentSearch.Equals(""))
            {
                Model.students.Clear();
                Model.LoadStudents();
            }
            else
            {
                Model.students.Clear();
                Model.LoadStudents(Model.StudentSearch);
            }

        }

        private void StudentRefreshCallback(object sender, RoutedEventArgs e)
        {
            TableOperation<StudentTable>.RefreshAll();
            Model.students.Clear();
            Model.LoadStudents();
        }
        private void ApplicationDeleteCallback(object sender, RoutedEventArgs e)
        {
            var dialog = new DialogQuestion();
            dialog.Model.Message = "Are you sure you want to delete selected entries?";
            dialog.ShowDialog();
            var answer = dialog.Model.Response;
            if (answer.Value)
            {
                var for_deletion = Model.applications.Where(x => x.IsChecked).Select(x => x.Entry).ToList();
                var size = for_deletion.Count;

                for (int i = 0; i < size; i++)
                {
                    var tmp = for_deletion[i];

                    TableOperation<ApplicationTable>.Delete(ref tmp);
                }

                TableOperation<ApplicationTable>.ForceRefreshAll();
                Model.applications.Clear();
                Model.LoadSchools();
            }
        }

        private void ApplicationEditCallback(object sender, RoutedEventArgs e)
        {

        }

        private void ApplicationCreateCallback(object sender, RoutedEventArgs e)
        {

        }

        private void ApplicationRefreshCallback(object sender, RoutedEventArgs e)
        {
            TableOperation<ApplicationTable>.RefreshAll();
            Model.applications.Clear();
            Model.LoadApplications();

        }
        private void ProgramDeleteCallback(object sender, RoutedEventArgs e)
        {
            var dialog = new DialogQuestion();
            dialog.Model.Message = "Are you sure you want to delete selected entries?";
            dialog.ShowDialog();
            var answer = dialog.Model.Response;
            if (answer.Value)
            {
                var for_deletion = Model.programs.Where(x => x.IsChecked).Select(x => x.Entry).ToList();
                var size = for_deletion.Count;

                for (int i = 0; i < size; i++)
                {
                    var tmp = for_deletion[i];

                    TableOperation<ProgramTable>.Delete(ref tmp);
                }

                TableOperation<ProgramTable>.ForceRefreshAll();
                Model.programs.Clear();
                Model.LoadSchools();
            }
        }

        private void ProgramEditCallback(object sender, RoutedEventArgs e)
        {
        }

        private void ProgramCreateCallback(object sender, RoutedEventArgs e)
        {

        }

        private void ProgramSearchCallback(object sender, RoutedEventArgs e)
        {
            if (Model.ProgramSearch.Equals(""))
            {
                Model.programs.Clear();
                Model.LoadPrograms();
            }
            else
            {
                Model.programs.Clear();
                Model.LoadPrograms(Model.ProgramSearch);
            }

        }

        private void ProgramRefreshCallback(object sender, RoutedEventArgs e)
        {
            TableOperation<ProgramTable>.RefreshAll();
            Model.programs.Clear();
            Model.LoadPrograms();

        }
    }
}
