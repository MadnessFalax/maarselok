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

namespace desktop_client
{
    /// <summary>
    /// Interaction logic for StudentEditor.xaml
    /// </summary>
    
    public class StudentEditorModel
    {
        public bool Ok = false;
        public TextEntry nameEntry { get; set; } = new TextEntry();
        public TextEntry addressEntry { get; set; } = new TextEntry();
        public TextEntry emailEntry { get; set; } = new TextEntry();
        public TextEntry passwordEntry { get; set; } = new TextEntry();
    }

    public partial class StudentEditor : Window
    {
        public StudentEditorModel Model = new StudentEditorModel();

        public StudentEditor(string? nameInitialText = null, string? addressInitialText = null, string? emailInitialText = null, string? passwordInitialText = null)
        {
            InitializeComponent();
            DataContext = Model;

            Model.nameEntry.SetLabel("Name");
            Model.nameEntry.SetText(nameInitialText == null ? "" : nameInitialText);
            ControlPanel.Children.Add(Model.nameEntry);

            Model.addressEntry.SetLabel("Address");
            Model.addressEntry.SetText(addressInitialText == null ? "" : addressInitialText);
            ControlPanel.Children.Add(Model.addressEntry);

            Model.emailEntry.SetLabel("Email");
            Model.emailEntry.SetText(emailInitialText == null ? "" : emailInitialText);
            Model.emailEntry.SetRegex(@"^.+@.+[\.]{1}.{2,5}$");
            ControlPanel.Children.Add(Model.emailEntry);

            Model.passwordEntry.SetLabel("Password");
            Model.passwordEntry.SetText(passwordInitialText == null ? "" : passwordInitialText);
            ControlPanel.Children.Add(Model.passwordEntry);
        }

        private void OkCallback(object sender, RoutedEventArgs e)
        {
            if (Model.nameEntry.Model.IsValid() && Model.addressEntry.Model.IsValid() && Model.emailEntry.Model.IsValid() && Model.passwordEntry.Model.IsValid())
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
