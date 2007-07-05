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
        //SongDatabase m_dataset;
        SongDatabaseWrapper m_dbwrap;
        public ServersFrame()
        {
            InitializeComponent();
        }

        /*
        public void Bind(SongDatabase db)
        {
            m_dataset = db;
            serverBindingSource.DataSource = db.DataSet.server;
        }
        */

        private void button1_Click(object sender, EventArgs e)
        {
            ISongServer[] servers = AddServerWizard.Run();
            if (servers == null) return;
            foreach(ISongServer srv in servers)
            {
                SongDb.serverRow row = m_dbwrap.SongDb.server.NewserverRow();
                //m_dataset.DataSet.server.IDColumn.
                row.url = srv.URL;
                row.servertype = srv.Type;
                row.config = srv.Config;
                m_dbwrap.SongDb.server.AddserverRow(row);
            }
        }

        public SongDatabaseWrapper SongDb
        {
            get { return m_dbwrap; }
            set
            {
                m_dbwrap = value;
                if (m_dbwrap != null)
                {
                    dataGridView1.DataSource = m_dbwrap.ServerBindingSource;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SongDb.serverRow row = m_dbwrap.SelectedServer;
            ISongServer srv = SongServer.LoadSongServer(row.servertype, row.url, row.config);
            srv.UploadChanges(m_dbwrap.Database, row.ID);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SongDb.serverRow row = m_dbwrap.SelectedServer;
            ISongServer srv = SongServer.LoadSongServer(row.servertype, row.url, row.IsconfigNull() ? null : row.config);
            srv.DownloadNew(m_dbwrap.Database, row.ID);
        }
    }
}
