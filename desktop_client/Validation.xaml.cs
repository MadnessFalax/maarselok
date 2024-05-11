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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace desktop_client
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    
    public class ValidationControl : INotifyPropertyChanged
    {
        private bool isValid;
        public bool IsValid { 
            get { return isValid; }
            set { 
                if (isValid != value)
                {
                    isValid = value;  
                    OnPropertyChanged(nameof(IsValid));
                }
            } 
        }

        public ValidationControl()
        {
            IsValid = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public partial class Validation : UserControl
    {
        public ValidationControl Control { get; set; }
        public Validation()
        {
            InitializeComponent();
            DataContext = this;
            Control = new ValidationControl();
        }
    }
}
