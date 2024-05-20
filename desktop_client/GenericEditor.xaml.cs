using CS_projekt.data;
using CS_projekt.data.table_attributes;
using CS_projekt.data.view_attributes;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for GenericEditor.xaml
    /// </summary>
    /// 

    public class EditorModel : INotifyPropertyChanged
    {
        private bool valid = false;
        public bool Valid {
            get { return valid; } 
            set
            {
                valid = value;
                OnPropertyChanged(nameof(Valid));
            }
        }

        public EditorModel()
        {
            valid = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class TypeModel : INotifyPropertyChanged
    {
        public Type Type { get; set; }
        public object? EditorInstance { get; set; }
        public TypeModel(Type t) 
        {
            Type = t;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName) 
        {
            PropertyChanged?.Invoke(PropertyChanged, new PropertyChangedEventArgs(propertyName));
        }
    }

    public partial class GenericEditor : Window
    {
        public TypeModel TypeModel { get; set; }

        public EditorModel EditorModel { get; set; } = new EditorModel();
        public GenericEditor()
        {
            throw new NotImplementedException();
            //InitializeComponent();
        }

        public GenericEditor(Type t, object editorInstance)
        {
            InitializeComponent();
            TypeModel = new TypeModel(t) { EditorInstance = editorInstance };
            if (editorInstance.GetType() != t)
            {
                throw new Exception("editorInstance must be of type t!");
            }

            var entity = Activator.CreateInstance(t);
            foreach(var property in t.GetProperties())
            {
                var eIProperty = editorInstance.GetType().GetProperty(property.Name);

                if (eIProperty != null)
                {
                    object value = eIProperty.GetValue(editorInstance);

                    property.SetValue(entity, value);
                }
            }

            var cols = t.GetProperties().Where(x => x.IsDefined(typeof(ColumnAttribute))).Where(x => !x.GetCustomAttribute<ColumnAttribute>().Unmanaged);
            var other_properties = t.GetProperties().Where(x => !x.IsDefined(typeof(ColumnAttribute)));
            var text_cols = cols.Where(x => !(x.IsDefined(typeof(ForeignKeyAttribute)) || x.IsDefined(typeof(ForeignTableAttribute))));
            
            
            foreach(var col in text_cols)
            {
                var tmp = new TextEntry();
                tmp.SetLabel(col.GetCustomAttribute<ViewNameAttribute>().Name);
                tmp.SetRegex(@"........+");
                tmp.SetText(GetValue(entity, col));

                ControlPanel.Children.Add(tmp);
            }

            // unused
            var key_cols = cols.Where(x => x.IsDefined(typeof(ForeignKeyAttribute))).ToList();
            var entity_cols = other_properties.Where(x => x.IsDefined(typeof(ForeignTableAttribute))).ToList();
            
            if (key_cols.Count > 0)
            {
                var col_pairs = key_cols.Select(x => (x, entity_cols.Where(y => x.GetCustomAttribute<ForeignKeyAttribute>().Name.Equals(y.GetCustomAttribute<ForeignTableAttribute>().Name)).First()));
                foreach(var (key_col, entity_col) in col_pairs)
                {
                    var tmp = new SelectionEntry();
                    tmp.SetLabel(entity_col.GetCustomAttribute<ViewNameAttribute>().Name);
                    // alright... this part is not as generic as hoped for
                    var typeName = entity_col.GetCustomAttribute<ForeignTableAttribute>().Name;
                    if (typeName.Equals("School"))
                    {
                        tmp.SetSelection(DataEntryPoint.SchoolMap.Select(x => (x.Key, x.Value.Name)).ToList());
                    }
                    else if (typeName.Equals("Student"))
                    {
                        tmp.SetSelection(DataEntryPoint.StudentMap.Select(x => (x.Key, x.Value.Name)).ToList());
                    }
                    else if (typeName.Equals("Program"))
                    {
                        tmp.SetSelection(DataEntryPoint.ProgramMap.Select(x => (x.Key, x.Value.Name)).ToList());
                    }

                    ControlPanel.Children.Add(tmp);
                }

            }
        }
        static string GetValue(object obj, PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType.IsValueType && Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null)
            {
                // Property is nullable
                object value = propertyInfo.GetValue(obj);
                if (value == null)
                {
                    return string.Empty;
                }
                else
                {
                    return value.ToString();
                }
            }
            else
            {
                // Property is not nullable
                object value = propertyInfo.GetValue(obj);
                return value.ToString();
            }
        }

        private void OkCallback(object sender, RoutedEventArgs e)
        {
            EditorModel.Valid = true;
            Close();
        }

        private void CancelCallback(object sender, RoutedEventArgs e)
        {
            EditorModel.Valid = false;
            Close();
        }
    }
}
