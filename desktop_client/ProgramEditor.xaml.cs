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
using desktop_client;
using CS_projekt.data;

namespace desktop_client
{
    /// <summary>
    /// Interaction logic for ProgramEditor.xaml
    /// </summary>
    public class ProgramEditorModel
    {
        public bool Ok = false;
        public TextEntry nameEntry = new TextEntry();
        public TextEntry descriptionEntry = new TextEntry();
        public TextEntry capacityEntry = new TextEntry(true);
        public SelectionEntry schoolEntry = new SelectionEntry();
    }

    public partial class ProgramEditor : Window
    {

        public ProgramEditorModel Model = new ProgramEditorModel();

        public ProgramEditor(List<SchoolTable> selection, string? nameInitialText = null, string? descriptionInitialText = null, string? capacityInitialText = null, int? schoolInitialSelection = null)
        {
            InitializeComponent();
            DataContext = Model;

            Model.nameEntry.SetLabel("Name");
            Model.nameEntry.SetText(nameInitialText == null ? "" : nameInitialText);
            ControlPanel.Children.Add(Model.nameEntry);

            Model.descriptionEntry.SetLabel("Description");
            Model.descriptionEntry.SetText(descriptionInitialText == null ? "" : descriptionInitialText);
            ControlPanel.Children.Add(Model.descriptionEntry);

            Model.capacityEntry.SetLabel("Capacity");
            Model.capacityEntry.SetText(capacityInitialText == null ? "" : capacityInitialText);
            ControlPanel.Children.Add(Model.capacityEntry);

            Model.schoolEntry.SetLabel("School");
            Model.schoolEntry.SetSelection(selection.Select(x => (x.Id.Value, x.Name)).ToList());
            Model.schoolEntry.SetInitialSelected(schoolInitialSelection);
            ControlPanel.Children.Add(Model.schoolEntry);
        }

        private void OkCallback(object sender, RoutedEventArgs e)
        {
            if (Model.nameEntry.Model.IsValid() && Model.descriptionEntry.Model.IsValid() && Model.schoolEntry.Model.IsValid() && Model.capacityEntry.Model.IsValid())
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
