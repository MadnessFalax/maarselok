﻿using System;
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

    public class TextEntryModel : INotifyPropertyChanged
    {
        private string text;
        private string label;
        public Regex regex { get; set; }

        private Validation validation;

        public bool IsValid()
        {
            return validation.Model.IsValid;
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

        public string Text {
            get { return text; }
            set { 
                if (text != value) 
                {
                    text = value;
                    validation.Model.IsValid = regex.IsMatch(text);
                    OnPropertyChanged(nameof(Text));
                }
            }
        }

        public TextEntryModel(Validation validation, bool expectNumber = false) 
        {
            text = "";
            label = "Label Placeholder";
            regex = expectNumber ? new Regex(@"^[0-9]+$") : new Regex(".+");
            this.validation = validation;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public partial class TextEntry : UserControl
    {
        public TextEntryModel Model { get; set; }

        public bool ExpectNumber { get; set; } = false;
        public TextEntry(bool expectNumber = false)
        {
            InitializeComponent();
            ExpectNumber = expectNumber; 
            Model = new TextEntryModel(TextValidation, ExpectNumber);
            DataContext = Model;
        }

        public void SetLabel(string labelText)
        {
            Model.Label = labelText;
        }

        public void SetText(string entryText)
        {
            Model.Text = entryText;
        }

        public void SetRegex(string regexPattern)
        {
            Model.regex = new Regex(regexPattern);
        }

        public void SetRegex(Regex regex)
        {
            Model.regex = regex;
        }
    }
}
