using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;
using System.Data.SQLite;

namespace zp8.Frames
{
    public partial class ServerListFrame : UserControl
    {
        SongDatabaseWrapper m_dbwrap;
        DataTable m_table;
        SQLiteDataAdapter m_adapter;

        public SongDatabaseWrapper SongDb
        {
            get { return m_dbwrap; }
            set
            {
                if (m_dbwrap != null) m_dbwrap.ChangedSongDatabase -= new EventHandler(m_dbwrap_ChangedSongDatabase);
                m_dbwrap = value;
                if (m_dbwrap != null) m_dbwrap.ChangedSongDatabase += new EventHandler(m_dbwrap_ChangedSongDatabase);
                try { ChangedDatabase(); }
                catch { }
            }
        }

        void m_dbwrap_ChangedSongDatabase(object sender, EventArgs e)
        {
            ChangedDatabase();
        }

        private void ChangedDatabase()
        {
            m_adapter = new SQLiteDataAdapter("select * from serverlist", m_dbwrap.Database.Connection);
            SQLiteCommandBuilder cb = new SQLiteCommandBuilder(m_adapter);
            m_adapter.InsertCommand = cb.GetInsertCommand();
            m_adapter.DeleteCommand = cb.GetDeleteCommand();
            m_adapter.UpdateCommand = cb.GetUpdateCommand();
            Reload();
        }

        private void Reload()
        {
            m_table = new DataTable();
            m_adapter.Fill(m_table);
            dataGridView1.DataSource = m_table;
        }


        public ServerListFrame()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Reload();
        }

        private void SaveChanges()
        {
            m_adapter.Update(m_table);
            Reload();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var dlg = MessageLogForm.Show("Importuji servery", false))
            {
                m_dbwrap.Database.LoadServers(dlg);
                dlg.FinishAndWait();
            }
        }
    }
}
