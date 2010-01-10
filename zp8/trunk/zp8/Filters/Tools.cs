using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace zp8
{
    public class DependedFilterEditor<T> : UITypeEditor where T : class
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                if (!context.PropertyDescriptor.IsReadOnly)
                {
                    return UITypeEditorEditStyle.DropDown;
                }
            }
            return UITypeEditorEditStyle.None;
        }

        public override object EditValue(ITypeDescriptorContext context,
        IServiceProvider provider, object value)
        {
            if (context != null
                && context.Instance != null
                && provider != null)
            {
                // Get an instance of the IWindowsFormsEditorService. 
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (edSvc != null)
                {
                    ListBox ctrl = new ListBox();
                    foreach (string name in SongFilters.EnumFilterNames<T>())
                    {
                        ctrl.Items.Add(name);
                    }

                    if (value != null) ctrl.SelectedIndex = ctrl.Items.IndexOf((string)value);

                    edSvc.DropDownControl(ctrl);

                    if (ctrl.SelectedIndex >= 0) return (string)ctrl.Items[ctrl.SelectedIndex];
                    return null;
                }
            }

            return value;
        }
    }
}
