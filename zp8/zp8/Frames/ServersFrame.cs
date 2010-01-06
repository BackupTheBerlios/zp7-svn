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


        public SongDatabaseWrapper SongDb
        {
            get { return m_dbwrap; }
            set
            {
                if (m_dbwrap != null) m_dbwrap.ChangedSongDatabase -= new EventHandler(m_dbwrap_ChangedSongDatabase);
                m_dbwrap = value;
                if (m_dbwrap != null) m_dbwrap.ChangedSongDatabase += new EventHandler(m_dbwrap_ChangedSongDatabase);
                try { Reload(); }
                catch { }
            }
        }

        void m_dbwrap_ChangedSongDatabase(object sender, EventArgs e)
        {
            Reload();
        }

        private void Reload()
        {
            if (m_dbwrap != null)
            {
                var tbl = m_dbwrap.GetServerTable();
                dataGridView1.DataSource = tbl;
            }
            else
            {
                dataGridView1.DataSource = null;
            }
        }

        private int GetServerID(int rowindex)
        {
            if (rowindex < 0 || rowindex >= dataGridView1.Rows.Count) return 0;
            DataGridViewRow gridrow = dataGridView1.Rows[rowindex];
            DataRow row = ((DataRowView)gridrow.DataBoundItem).Row;
            return row.SafeInt(0);
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
            m_dbwrap.Database.InsertServer(server);
            Reload();
        }

        //public SongDatabaseWrapper SongDb
        //{
        //    get { return m_dbwrap; }
        //    set
        //    {
        //        m_dbwrap = value;
        //        if (m_dbwrap != null)
        //        {
        //            dataGridView1.DataSource = m_dbwrap.ServerBindingSource;
        //        }
        //    }
        //}

        //private delegate void ServerActionDelegate(SongDatabase db, ISongServer srv, int serverid);

//        private void DoServerAction(ServerActionDelegate callback)
//        {
//#if (DEBUG)
//            DoServerAction2(callback);
//#else
//            try
//            {
//                DoServerAction2(callback);
//            }
//            catch (Exception e)
//            {
//                MessageBox.Show("Pøi kominukaci se severem nastala chyba:\n" + e.Message, "Zpìvníkátor", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//#endif
//        }

        //private void DoServerAction2(ServerActionDelegate callback)
        //{
        //    using (IWaitDialog dlg = WaitForm.Show("Probíhá komunikace se serverem", false))
        //    {
        //        SongDb.serverRow row = m_dbwrap.SelectedServer;
        //        ISongServer srv = SongServer.Load(row.servertype, row.config);
        //        callback(m_dbwrap.Database, srv, row.ID);
        //    }
        //    MessageBox.Show("Akce probìhla úspìšnì");
        //}

        private void button3_Click(object sender, EventArgs e)
        {
            //DoServerAction(delegate(SongDatabase db, ISongServer srv, int serverid)
            //    { srv.UploadChanges(db, serverid); }
            //);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //DoServerAction(delegate(SongDatabase db, ISongServer srv, int serverid)
            //    { srv.DownloadNew(db, serverid); }
            //);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //DoServerAction(delegate(SongDatabase db, ISongServer srv, int serverid)
            //    { srv.UploadWhole(db, serverid); }
            //);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (saveXML.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fw = new FileStream(saveXML.FileName, FileMode.Create))
                {
                    //m_dbwrap.Database.CreateInternetXml(m_dbwrap.SelectedServer.ID, fw);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (openXML.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fr = new FileStream(openXML.FileName, FileMode.Open))
                {
                    //m_dbwrap.Database.MergeInternetXml(m_dbwrap.SelectedServer.ID, fr);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                int id = GetServerID(dataGridView1.CurrentCell.RowIndex);
                ISongServer srv = m_dbwrap.Database.LoadServer(id);
                if (srv != null)
                {
                    PropertiesForm.Run(srv);
                    m_dbwrap.Database.SaveServer(srv, id);
                    Reload();
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                int id = GetServerID(dataGridView1.CurrentCell.RowIndex);
                m_dbwrap.Database.DeleteServer(id);
                Reload();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Reload();
        }
    }
}
