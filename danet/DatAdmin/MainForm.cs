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
    public partial class MainForm : Form, IWindowToolkit
    {
        public MainForm()
        {
            InitializeComponent();
            Toolkit.WindowToolkit = this;

            daTreeView1.RootPath = "";
        }
        public void OpenSchemaWindow(DbConnection conn)
        {
            OpenContent(new SchemaFrame(conn));
        }

        private void OpenContent(ContentFrame frame)
        {
            TabPage page = new TabPage(frame.PageTitle);
            frame.Parent = page;
            frame.Dock = DockStyle.Fill;
            contentTabs.TabPages.Add(page);
        }
    }
}