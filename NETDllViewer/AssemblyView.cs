using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Windows.Data;
using System.ComponentModel;

namespace NETDllViewer
{
    public class AssemblyView : INotifyPropertyChanged
    {
        public AssemblyView()
        {
            UsingAssembly = Assembly.GetExecutingAssembly();            
        }

        public static AssemblyView Default
        {
            get
            {
                return new AssemblyView();
            }
        }

        private Assembly _assembly;
        public Assembly UsingAssembly
        {
            get
            {
                return _assembly;
            }
            set
            {
                _assembly = value;                
                NotifyPropertyChanged("UsingAssembly");
                Classes = new List<Type>(UsingAssembly.GetTypes());
            }
        }

        private List<Type> _classes;
        public List<Type> Classes
        {
            get
            {
                /*if (UsingAssembly != null)
                {
                    List<Type> lst = new List<Type>(UsingAssembly.GetTypes());
                    return lst;
                }
                return null;*/
                return _classes;
            }
            set
            {
                _classes = value;
                NotifyPropertyChanged("Classes");
            }
        }

        public List<Type> Interfaces
        {
            get;
            set;
        }

        public List<MethodInfo> Methods
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

    }

    [ValueConversion(typeof(Type), typeof(IEnumerable<Type>))]
    public class ClassToInterfacesConverter : IValueConverter
    {
        public static ClassToInterfacesConverter Default
        {
            get
            {
                return new ClassToInterfacesConverter();
            }
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            string FilterStr = parameter as string;
            if (string.IsNullOrEmpty(FilterStr))
            {
                return ((Type)value).GetInterfaces();
            }
            else
            {
                return ((Type)value).GetInterfaces().Where(type => type.ToString().Contains(FilterStr));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(Type), typeof(IEnumerable<MethodInfo>))]
    public class ClassToMethodsConverter : IValueConverter
    {
        public static ClassToMethodsConverter Default
        {
            get
            {
                return new ClassToMethodsConverter();
            }
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            return ((Type)value).GetMethods();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
