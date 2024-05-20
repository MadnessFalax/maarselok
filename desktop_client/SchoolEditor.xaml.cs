using CS_projekt.data;
using desktop_client.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace desktop_client
{
    /// <summary>
    /// Interaction logic for SchoolEditor.xaml
    /// </summary>

    public class SchoolEditorModel
    {
        public bool Ok = false;
        public TextEntry nameEntry = new TextEntry();
        public TextEntry addressEntry = new TextEntry();
    }

    public partial class SchoolEditor : Window
    {
        
        public SchoolEditorModel Model = new SchoolEditorModel();

        public SchoolEditor(string? nameInitialText = null, string? addressInitialText = null)
        {
            InitializeComponent();
            DataContext = Model;

            Model.nameEntry.SetLabel("Name");
            Model.nameEntry.SetText(nameInitialText == null ? "" : nameInitialText);
            Model.nameEntry.SetRegex(".+");
            ControlPanel.Children.Add(Model.nameEntry);

            Model.addressEntry.SetLabel("Address");
            Model.addressEntry.SetText(addressInitialText == null ? "" : addressInitialText);
            Model.addressEntry.SetRegex(".+");
            ControlPanel.Children.Add(Model.addressEntry);
        }

        private void OkCallback(object sender, RoutedEventArgs e)
        {
            if (Model.nameEntry.Model.IsValid() && Model.addressEntry.Model.IsValid())
            {
                Model.Ok = true;
                Close();
            }
            else
            {
                var dialog = new DialogMessage("All text entries must contain valid data!");
                dialog.ShowDialog();
            }
        }

        private void CancelCallback(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
