using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace desktop_client
{
    /// <summary>
    /// Interaction logic for SelectionEntry.xaml
    /// </summary>
    /// 


    public class SelectionEntryModel : INotifyPropertyChanged
    {
        public List<(int, string)> Selection;
        private (int, string) selected = (-1, "");
        public int SelectedId { 
            get { return selected.Item1; } 
            set
            {
                if (value != selected.Item1)
                {
                    selected = Selection.Where(x => x.Item1 == value).First();
                    if (Selection.Any(x => x.Item1 == SelectedId))
                    {
                        val_ref.Model.IsValid = true;
                    }
                    else
                    {
                        val_ref.Model.IsValid = false;
                    }
                    OnPropertyChanged(nameof(SelectedId));
                    OnPropertyChanged(nameof(SelectedName));
                }
            }
        }

        public string SelectedName
        {
            get { return selected.Item2; }
            set { }
        }

        private string label;
        private Validation val_ref;

        public string Label
        {
            get { return label; }
            set
            {
                if (label != value)
                {
                    label = value;
                    OnPropertyChanged(nameof(Label));
                }
            }
        }

        public SelectionEntryModel(Validation validation)
        {
            Selection = new List<(int, string)>();
            label = "Label Placeholder";
            val_ref = validation;
        }

        public bool IsValid()
        {
            return val_ref.Model.IsValid;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public partial class SelectionEntry : UserControl
    {
        public SelectionEntryModel Model { get; set; }

        public SelectionEntry()
        {
            InitializeComponent();
            DataContext = this;
            Model = new SelectionEntryModel(TextValidation);
        }

        public void SetLabel(string labelText)
        {
            Model.Label = labelText;
        }

        // tuples of ID and Name
        public void SetSelection(List<(int, string)> sel)
        {
            Model.Selection = sel;
        }

        public void SetInitialSelected(int? id)
        {
            if (id != null)
            {
                Model.SelectedId = id.Value;
            }
        }

        private void ChooseCallback(object sender, RoutedEventArgs e)
        {
            var dialog = new ChooseWindow(Model.Selection);
            dialog.ShowDialog();
            if (dialog.Model.Ok)
            {
                Model.SelectedId = dialog.Model.SelectedId;
            }
        }
    }
}
