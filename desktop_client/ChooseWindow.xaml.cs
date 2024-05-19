using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace desktop_client
{
    /// <summary>
    /// Interaction logic for ChooseWindow.xaml
    /// </summary>
    /// 
    public class ChooseWindowModel
    {
        public ObservableCollection<(int, string)> Selection { get; set; }

        public ChooseWindowModel(List<(int, string)> selection)
        {
            Selection = new ObservableCollection<(int, string)>(selection);
        }
    }
    public partial class ChooseWindow : Window
    {
        public ChooseWindowModel Model { get; set; }

        public ChooseWindow(List<(int, string)> selection)
        {
            Model = new ChooseWindowModel(selection);
            DataContext = Model;
            InitializeComponent();
        }
    }
}
