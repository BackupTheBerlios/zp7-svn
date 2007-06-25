using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace zp8
{
    public partial class ServersFrame : UserControl
    {
        SongDatabase m_dataset;
        public ServersFrame()
        {
            InitializeComponent();
        }
        public void Bind(SongDatabase db)
        {
            m_dataset = db;
            serverBindingSource.DataSource = db.DataSet.server;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ISongServer[] types = AddServerWizard.Run();
        }
    }
}
