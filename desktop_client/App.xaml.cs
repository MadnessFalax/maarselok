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
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DataEntryPoint.connection_string = "Data Source=../../../../Bin/Debug/net6.0/mydb.db;";
            DataEntryPoint.initDB();
            DataEntryPoint.loadDB();

        }
    }
}
