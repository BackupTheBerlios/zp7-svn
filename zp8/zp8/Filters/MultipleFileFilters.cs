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
    public class OpenFileNamesEditor : UITypeEditor
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
            string filter = ((MultipleStreamImporterProperties)context.Instance).Importer.FileDialogFilter;
            return FileCollectionEditorForm.Run((string[])value, filter);
        }
    }

    public class MultipleStreamImporterProperties : PropertyPageBase
    {
        string[] m_fileNames = new string[] { };

        [Editor(typeof(OpenFileNamesEditor), typeof(UITypeEditor))]
        [DisplayName("Importované soubory")]
        public string[] FileNames
        {
            get { return m_fileNames; }
            set { m_fileNames = value; }
        }
        public readonly MultipleStreamImporter Importer;
        public MultipleStreamImporterProperties(MultipleStreamImporter importer)
        {
            Importer = importer;
        }
    }

    public abstract class MultipleStreamImporter : PropertyPageBase, ISongParser
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
            return new MultipleStreamImporterProperties(this);
        }

        #endregion

        #region ISongParser Members

        public void Parse(object props, InetSongDb db)
        {
            MultipleStreamImporterProperties p = (MultipleStreamImporterProperties)props;
            foreach (string filename in p.FileNames)
            {
                using (FileStream fr = new FileStream(filename, FileMode.Open))
                {
                    Parse(fr, db);
                }
            }
        }

        #endregion

        protected abstract void Parse(Stream fr, InetSongDb db);
    }

}