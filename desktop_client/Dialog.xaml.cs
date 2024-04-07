﻿using System;
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
    /// Interaction logic for Dialog.xaml
    /// </summary>
    public partial class Dialog : Window
    {
        public string Message { get; set; } = "Are you sure?";

        public bool? Response { get; set; } = false;

        public Dialog()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void YesCallback(object sender, RoutedEventArgs e)
        {
            Response = true;
            Close();
        }

        private void NoCallback(object sender, RoutedEventArgs e)
        {
            Response = false;
            Close();
        }
    }
}
