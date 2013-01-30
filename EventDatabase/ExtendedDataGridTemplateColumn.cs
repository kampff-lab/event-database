using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Reflection;
using System.Windows.Data;
using System.Globalization;

namespace EventDatabase
{
    public class ExtendedDataGridTemplateColumn : DataGridTemplateColumn
    {
        public override void OnPastingCellClipboardContent(object item, object cellContent)
        {
            if (item != null)
            {
                var binding = ClipboardContentBinding as Binding;
                if (binding != null)
                {
                    PropertyInfo propertyInfo = item.GetType().GetProperty(binding.Path.Path);
                    if (propertyInfo != null)
                    {
                        if (binding.Converter != null)
                        {
                            cellContent = binding.Converter.ConvertBack(cellContent, propertyInfo.PropertyType, null, CultureInfo.CurrentUICulture);
                        }
                        
                        propertyInfo.SetValue(item, cellContent, null);
                    }
                }
            }
        }
    }
}
