using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Reflection;
using Microsoft.Win32;

namespace NETDllViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        Assembly assembly
        {
            get
            {
                AssemblyView asm = (AssemblyView)FindResource("Viewer");
                return asm.UsingAssembly;
            }
            set
            {
                AssemblyView asm = (AssemblyView)FindResource("Viewer");
                asm.UsingAssembly = value;
            }
        }
        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Library (.dll) | *.dll|Executable (.exe) | *.exe|All files (*.*) | *.*";
            if (dialog.ShowDialog()  == true)
            {
                assembly = Assembly.LoadFrom(dialog.FileName);                
            }
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void mainWindow_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                try
                {
                    string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);          
                    assembly = Assembly.LoadFrom(droppedFiles[0]);
                }
                catch(Exception ex)
                {
                    //txtAssemblyName.Text = ex.Message;
                    assembly = null;
                }
            }
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {           
            assembly = Assembly.GetExecutingAssembly();
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            string filterStr = txtFilter.Text;
            if (string.IsNullOrEmpty(filterStr))
            {
                e.Accepted = true;
                return;
            }
            filterStr = filterStr.ToLowerInvariant();
            if (e.Item.ToString().ToLowerInvariant().Contains(filterStr))
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = false;
            }
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource collectionMethods = TryFindResource("collectionMethods") as CollectionViewSource;
            CollectionViewSource collectionInterfaces = TryFindResource("collectionInterfaces") as CollectionViewSource;
            if (collectionMethods != null)
            {
                collectionMethods.View.Refresh();
            }
            if (collectionInterfaces != null)
            {
                collectionInterfaces.View.Refresh();
            }
        }
    }
}
