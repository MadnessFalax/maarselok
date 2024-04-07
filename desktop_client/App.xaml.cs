using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CS_projekt.data;


namespace desktop_client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void mock_method()
        {
            var a = DataEntryPoint.StudentMap;
            TableOperation<StudentTable>.RefreshAll();
            
        }
    }
}
