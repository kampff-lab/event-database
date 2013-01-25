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
            if (e.Args.Length > 0)
            {
                var fileName = e.Args[0];
                Dispatcher.BeginInvoke((Action)(() =>
                {
                    var window = (MainWindow)MainWindow;
                    window.OpenFile(fileName);
                }));
            }
            else Dispatcher.BeginInvoke((Action)(() => ((MainWindow)MainWindow).NewFile()));
            base.OnStartup(e);
        }
    }
}
