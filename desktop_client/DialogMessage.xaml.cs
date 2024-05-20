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

namespace desktop_client.Models
{
    /// <summary>
    /// Interaction logic for DialogMessage.xaml
    /// </summary>
    public class DialogMessageModel
    {
        public string Message { get; set; } = "An error occured!";
    }

    public partial class DialogMessage : Window
    {
        public DialogMessageModel Model = new DialogMessageModel();

        public DialogMessage(string? message = null)
        {
            InitializeComponent();
            if (message != null)
            {
                Model.Message = message;
            }
        }

        private void OkCallback(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
