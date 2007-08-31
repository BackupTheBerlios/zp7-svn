using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DAIntf;
using System.Data.Common;

namespace DatAdmin
{
    public partial class MainForm : Form, IWindowToolkit, ILogToolkit
    {
        public MainForm()
        {
            InitializeComponent();
            Toolkit.WindowToolkit = this;
            Toolkit.LogToolkit = this;

            daTreeView1.RootPath = "";
        }

        private void OpenContent(ContentFrame frame)
        {
            TabPage page = new TabPage(frame.PageTitle);
            frame.Parent = page;
            frame.Dock = DockStyle.Fill;
            contentTabs.TabPages.Add(page);
        }

        #region ILogToolkit Members

        public void LogMessage(string type, string message)
        {
            DateTime now = DateTime.Now;
            SimpleCallback callback = delegate() { AddLogMessage(DateTime.Now, type, message); };
            Invoke(callback);
        }

        #endregion

        private void AddLogMessage(DateTime datetime, string type, string message)
        {
            ListViewItem item = logListView.Items.Add(type);
            item.SubItems.Add(datetime.ToString("d"));
            item.SubItems.Add(datetime.ToString("T"));
            item.SubItems.Add(message);
        }

        #region IWindowToolkit Members

        public void OpenSchemaWindow(DbConnection conn)
        {
            OpenContent(new SchemaFrame(conn));
        }

        public void OpenTable(ITableConnection conn)
        {
            OpenContent(new TableDataFrame(conn));
        }

        #endregion
    }
}