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
using System.ComponentModel;

namespace EventDatabase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly DependencyPropertyDescriptor TextPropertyChangedDescriptor = DependencyPropertyDescriptor.FromProperty(ComboBox.TextProperty, typeof(ComboBox));

        const string FileFilter = "Comma-separated values (*.csv)|*.csv|All files (*.*)|*.*";
        const string TitleText = "Event Database";
        const string EventTypesFile = "EventTypes.txt";
        OpenFileDialog openDialog;
        SaveFileDialog saveDialog;
        List<string> eventTypes;
        bool modifiedEventTypes;

        public MainWindow()
        {
            InitializeComponent();

            openDialog = new OpenFileDialog { Filter = FileFilter };
            saveDialog = new SaveFileDialog { Filter = FileFilter };
            eventTypes = new List<string>();
        }

        public IEnumerable EventTypesSource
        {
            get { return eventTypes; }
        }

        public IEnumerable ItemsSource
        {
            get { return dataGrid.ItemsSource; }
            set { dataGrid.ItemsSource = value; }
        }

        public void NewFile()
        {
            eventTypes.Clear();
            modifiedEventTypes = false;
            dataGrid.ItemsSource = new List<Event>();
            saveDialog.FileName = null;
            SetTitle(saveDialog.FileName);
        }

        public void OpenFile(string fileName)
        {
            eventTypes.Clear();
            modifiedEventTypes = false;
            var directory = System.IO.Path.GetDirectoryName(fileName);
            var tagsPath = System.IO.Path.Combine(directory, EventTypesFile);
            if (File.Exists(tagsPath))
            {
                var tags = File.ReadAllLines(tagsPath);
                eventTypes.AddRange(tags);
            }

            dataGrid.ItemsSource = File.ReadAllLines(fileName).Select(line => Event.Parse(line)).ToList();
            saveDialog.FileName = fileName;
            SetTitle(fileName);
        }

        void SaveFile(string fileName)
        {
            if (modifiedEventTypes)
            {
                var directory = System.IO.Path.GetDirectoryName(fileName);
                var tagsPath = System.IO.Path.Combine(directory, EventTypesFile);
                modifiedEventTypes = false;
                File.WriteAllLines(tagsPath, eventTypes.ToArray());
            }

            var contents = ((IEnumerable<Event>)dataGrid.ItemsSource).Select(evt => evt.ToString()).ToArray();
            File.WriteAllLines(fileName, contents);
            SetTitle(fileName);
        }

        void SetTitle(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) Title = TitleText;
            else Title = string.Format("{0} ({1})", TitleText, System.IO.Path.GetFileName(fileName));
        }

        private void newMenuItem_Click(object sender, RoutedEventArgs e)
        {
            NewFile();
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
                OpenFile(fileName);
            }
        }

        private void saveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(saveDialog.FileName)) saveAsMenuItem_Click(sender, e);
            else
            {
                var fileName = saveDialog.FileName;
                SaveFile(fileName);
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
                    OpenFile(fileName);
                }
            }
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var header = e.Column.Header as string;
            if (header == "Timestamp")
            {
                var columnTemplate = new DataGridTemplateColumn();
                columnTemplate.Header = header;
                columnTemplate.CellTemplate = (DataTemplate)Resources["DateTimeOffsetCellTemplate"];
                columnTemplate.CellEditingTemplate = (DataTemplate)Resources["DateTimeOffsetCellEditingTemplate"];
                e.Column = columnTemplate;
            }

            if (header == "EventType")
            {
                var columnTemplate = new DataGridTemplateColumn();
                columnTemplate.Header = header;
                columnTemplate.CellTemplate = (DataTemplate)Resources["EditableComboBoxCellTemplate"];
                columnTemplate.CellEditingTemplate = (DataTemplate)Resources["EditableComboBoxCellEditingTemplate"];
                e.Column = columnTemplate;
            }
        }

        private void ComboBox_LostFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var text = ((ComboBox)sender).Text;
            if (!string.IsNullOrWhiteSpace(text) && !eventTypes.Contains(text))
            {
                eventTypes.Add(text);
                modifiedEventTypes = true;
            }
        }
    }
}
