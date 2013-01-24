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
using System.Collections;
using Microsoft.Win32;
using System.IO;

namespace EventDatabase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OpenFileDialog openDialog;
        SaveFileDialog saveDialog;
        const string FileFilter = "Comma-separated values (*.csv)|*.csv|All files (*.*)|*.*";
        const string TitleText = "Event Database";

        public MainWindow()
        {
            InitializeComponent();

            openDialog = new OpenFileDialog { Filter = FileFilter };
            saveDialog = new SaveFileDialog { Filter = FileFilter };
        }

        public IEnumerable ItemsSource
        {
            get { return dataGrid.ItemsSource; }
            set { dataGrid.ItemsSource = value; }
        }

        private void Open(string fileName)
        {
            dataGrid.ItemsSource = File.ReadAllLines(fileName).Select(line => Event.Parse(line)).ToList();
            saveDialog.FileName = fileName;
            SetTitle(fileName);
        }

        public void SetTitle(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) Title = TitleText;
            else Title = string.Format("{0} ({1})", TitleText, System.IO.Path.GetFileName(fileName));
        }

        private void newMenuItem_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = new List<Event>();
            saveDialog.FileName = null;
            SetTitle(saveDialog.FileName);
        }

        private void exitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void openMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (openDialog.ShowDialog() == true)
            {
                var fileName = openDialog.FileName;
                Open(fileName);
            }
        }

        private void saveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(saveDialog.FileName)) saveAsMenuItem_Click(sender, e);
            else
            {
                var fileName = saveDialog.FileName;
                var contents = ((IEnumerable<Event>)dataGrid.ItemsSource).Select(evt => evt.ToString()).ToArray();
                File.WriteAllLines(fileName, contents);
                SetTitle(fileName);
            }
        }

        private void saveAsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (saveDialog.ShowDialog() == true)
            {
                saveMenuItem_Click(sender, e);
            }
        }

        private void dataGrid_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void dataGrid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var fileName = ((string[])e.Data.GetData(DataFormats.FileDrop)).FirstOrDefault();
                if (!string.IsNullOrEmpty(fileName))
                {
                    Open(fileName);
                }
            }
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
    }
}
