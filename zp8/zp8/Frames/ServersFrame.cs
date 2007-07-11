using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

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
            ISongServer server = AddServerWizard.Run();
            if (server == null) return;
            SongDb.serverRow row = m_dbwrap.SongDb.server.NewserverRow();
            row.url = server.ToString();
            SongServerType st = SongServer.GetServerName(server);
            row.servertype = st.Name;
            row.config = SongServer.Save(server);
            row.isreadonly =  st.ReadOnly;
            m_dbwrap.SongDb.server.AddserverRow(row);
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
            //try
            //{
                SongDb.serverRow row = m_dbwrap.SelectedServer;
                ISongServer srv = SongServer.Load(row.servertype, row.config);
                callback(m_dbwrap.Database, srv, row.ID);
                MessageBox.Show("Akce probìhla úspìšnì");
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show(e.ToString(), "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
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

        private void button6_Click(object sender, EventArgs e)
        {
            if (saveXML.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fw = new FileStream(saveXML.FileName, FileMode.Create))
                {
                    m_dbwrap.Database.CreateInternetXml(m_dbwrap.SelectedServer.ID, fw);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (openXML.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fr = new FileStream(openXML.FileName, FileMode.Open))
                {
                    m_dbwrap.Database.MergeInternetXml(m_dbwrap.SelectedServer.ID, fr);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SongDb.serverRow row = m_dbwrap.SelectedServer;
            ISongServer srv = SongServer.Load(row.servertype, row.config);
            PropertiesForm.Run(srv);
            row.config = SongServer.Save(srv);
            row.url = srv.ToString();
        }
    }
}
