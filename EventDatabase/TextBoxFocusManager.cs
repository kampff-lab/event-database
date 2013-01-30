using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace EventDatabase
{
    public static class TextBoxFocusManager
    {
        public static readonly DependencyProperty FocusProperty = DependencyProperty.RegisterAttached("Focus", typeof(bool), typeof(TextBoxFocusManager), new PropertyMetadata(false, FocusChanged));

        public static bool GetFocus(DependencyObject d)
        {
            return (bool)d.GetValue(FocusProperty);
        }

        public static void SetFocus(DependencyObject d, bool value)
        {
            d.SetValue(FocusProperty, value);
        }

        private static void FocusChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                var textBox = sender as TextBox;
                if (textBox != null)
                {
                    textBox.Focus();
                    textBox.Dispatcher.BeginInvoke((Action)textBox.SelectAll);
                }
            }
        }

    }

}
