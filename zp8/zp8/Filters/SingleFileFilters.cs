using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using System.Xml;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace zp8
{
    public class SaveFileNameEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                if (!context.PropertyDescriptor.IsReadOnly)
                {
                    return UITypeEditorEditStyle.Modal;
                }
            }
            return UITypeEditorEditStyle.None;
        }

        [RefreshProperties(RefreshProperties.All)]
        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            if (context == null || provider == null || context.Instance == null)
            {
                return base.EditValue(provider, value);
            }
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = ((SingleFileDynamicProperties)context.Instance).Exporter.FileDialogFilter;
            dlg.FileName = (string)value;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                return dlg.FileName;
            }
            else
            {
                return value;
            }
        }
    }

    public class SingleFileDynamicProperties : PropertyPageBase
    {
        string m_fileName;

        [Editor(typeof(SaveFileNameEditor), typeof(UITypeEditor))]
        [DisplayName("Výstupní soubor")]
        public string FileName
        {
            get { return m_fileName; }
            set { m_fileName = value; }
        }
        public readonly SingleFileExporter Exporter;
        public SingleFileDynamicProperties(SingleFileExporter exporter)
        {
            Exporter = exporter;
        }
    }

    public abstract class SingleFileExporter : PropertyPageBase, ISongFormatter, IStreamSongFormatter
    {
        #region ISongFilter Members

        public virtual string FileDialogFilter
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public virtual string Title
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public virtual string Description
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public virtual object CreateDynamicProperties()
        {
            return new SingleFileDynamicProperties(this);
        }

        #endregion

        #region ISongFormatter Members

        public void Format(InetSongDb db, object props)
        {
            string filename = ((SingleFileDynamicProperties)props).FileName;
            using (FileStream fw = new FileStream(filename, FileMode.Create))
            {
                Format(db, fw);
            }
        }

        #endregion

        public abstract void Format(InetSongDb db, Stream fw);
    }

}