using desktop_client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CS_projekt.data;

namespace desktop_client
{
    /// <summary>
    /// Interaction logic for ApplicationEditor.xaml
    /// </summary>

    public class ApplicationEditorModel
    {
        public bool Ok = false;
        public SelectionEntry programEntry = new SelectionEntry();
        public SelectionEntry studentEntry = new SelectionEntry();
    }

    public partial class ApplicationEditor : Window
    {
        public ApplicationEditorModel Model = new ApplicationEditorModel();

        public ApplicationEditor(List<StudentTable> studentSelection, List<ProgramTable> programSelection, int? studentInitialSelection = null, int? programInitialSelected = null)
        {
            InitializeComponent();
            DataContext = Model;

            Model.studentEntry.SetLabel("Student");
            Model.studentEntry.SetSelection(studentSelection.Select(x => (x.Id.Value, x.Name)).ToList());
            Model.studentEntry.SetInitialSelected(studentInitialSelection);
            ControlPanel.Children.Add(Model.studentEntry);

            Model.programEntry.SetLabel("Program");
            Model.programEntry.SetSelection(programSelection.Select(x => (x.Id.Value, x.Name)).ToList());
            Model.programEntry.SetInitialSelected(programInitialSelected);
            ControlPanel.Children.Add(Model.programEntry);

        }

        private void OkCallback(object sender, RoutedEventArgs e)
        {
            if (Model.programEntry.Model.IsValid() && Model.studentEntry.Model.IsValid())
            {
                Model.Ok = true;
                Close();
            }
            else
            {
                var dialog = new DialogMessage("All entries must contain valid data!");
                dialog.ShowDialog();
            }
        }

        private void CancelCallback(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
