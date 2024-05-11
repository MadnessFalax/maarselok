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

    public class SelectionEntryControl
    {
        private string text;
        private int selected_index;
        public int SelectedId { get; set; }
        private string label;
        private Validation val_ref;
        private List<(int, string)> selection;

        public List<(int, string)> Selection { 
            get { return selection; }
            set
            {
                selection = value;
                SelectedIndex = 0;
                OnPropertyChanged(nameof(Selection));
            }
        }

        public int SelectedIndex { 
            get { return selected_index; }
            set
            {
                selected_index = value;
                if (value >= selection.Count)
                {
                    text = "";
                    val_ref.Control.IsValid = false;
                }
                else
                {
                    text = selection[value].Item2;
                    val_ref.Control.IsValid = true;
                    SelectedId = selection[value].Item1;
                }
            }
        }

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

        public string Text
        {
            get { return text; }
            set
            {
                if (text != value)
                {
                    text = value;;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }

        public SelectionEntryControl(Validation validation)
        {
            selected_index = 0;
            selection = new List<(int, string)>();
            text = "";
            label = "Label Placeholder";
            val_ref = validation;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public partial class SelectionEntry : UserControl
    {
        public SelectionEntryControl Control { get; set; }

        public SelectionEntry()
        {
            InitializeComponent();
            DataContext = this;
            Control = new SelectionEntryControl(TextValidation);
        }

        public void SetLabel(string labelText)
        {
            Control.Label = labelText;
        }

        // tuples of ID and Name
        public void SetSelection(List<(int, string)> sel)
        {
            Control.Selection = sel;
        }

        private void ChooseCallback(object sender, RoutedEventArgs e)
        {

        }
    }
}
