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
                row.isreadonly = SongServer.ServerType(srv.Type).Readonly;
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

        private delegate void ServerActionDelegate(AbstractSongDatabase db, ISongServer srv, int serverid);

        private void DoServerAction(ServerActionDelegate callback)
        {
            try
            {
                SongDb.serverRow row = m_dbwrap.SelectedServer;
                ISongServer srv = SongServer.LoadSongServer(row.servertype, row.url, row.IsconfigNull() ? null : row.config);
                callback(m_dbwrap.Database, srv, row.ID);
                MessageBox.Show("Akce probìhla úspìšnì");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DoServerAction(delegate(AbstractSongDatabase db, ISongServer srv, int serverid)
                { srv.UploadChanges(db, serverid); }
            );
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DoServerAction(delegate(AbstractSongDatabase db, ISongServer srv, int serverid)
                { srv.DownloadNew(db, serverid); }
            );
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DoServerAction(delegate(AbstractSongDatabase db, ISongServer srv, int serverid)
                { srv.UploadWhole(db, serverid); }
            );
        }
    }
}
