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

    public class SelectionEntryControl
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public SelectionEntryControl(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class ChooseWindowModel
    {
        public bool Ok = false;

        public int SelectedId = -1;

        public ObservableCollection<SelectionEntryControl> Selection { get; set; }

        public ChooseWindowModel(List<(int, string)> selection)
        {
            Selection = new ObservableCollection<SelectionEntryControl>(selection.Select(x => new SelectionEntryControl(x.Item1, x.Item2)).ToList());
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

        private void ChooseCallback(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            var ctx = btn.DataContext as SelectionEntryControl;
            Model.Ok = true;
            Model.SelectedId = ctx.Id;
            Close();
        }

        private void CancelCallback(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
