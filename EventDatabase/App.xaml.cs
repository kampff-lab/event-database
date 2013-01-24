using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.IO;

namespace EventDatabase
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            string fileName = null;
            IEnumerable<Event> events;
            if (e.Args.Length > 0)
            {
                fileName = e.Args[0];
                events = File.ReadAllLines(fileName).Select(line => Event.Parse(line)).ToList();
            }
            else events = new List<Event>();
            Dispatcher.BeginInvoke((Action)(() =>
            {
                var window = (MainWindow)MainWindow;
                window.ItemsSource = events;
                window.SetTitle(fileName);
            }));
            base.OnStartup(e);
        }
    }
}
