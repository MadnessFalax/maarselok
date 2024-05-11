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
    /// Interaction logic for UserControl1.xaml
    /// </summary>

    public class TextEntryControl : INotifyPropertyChanged
    {
        private string text;
        private string label;
        public Regex regex { get; set; }

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

        public string Text {
            get { return text; }
            set { 
                if (text != value) 
                {
                    text = value;
                    val_ref.Control.IsValid = regex.IsMatch(text);
                    OnPropertyChanged(nameof(Text));
                }
            }
        }

        public TextEntryControl(Validation validation) 
        {
            text = "";
            label = "Label Placeholder";
            regex = new Regex(".+");
            val_ref = validation;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public partial class TextEntry : UserControl
    {
        public TextEntryControl Control { get; set; }

        public TextEntry()
        {
            InitializeComponent();
            DataContext = this;
            Control = new TextEntryControl(TextValidation);
        }

        public void SetLabel(string labelText)
        {
            Control.Label = labelText;
        }

        public void SetRegex(string regexPattern)
        {
            Control.regex = new Regex(regexPattern);
        }

        public void SetRegex(Regex regex)
        {
            Control.regex = regex;
        }
    }
}
