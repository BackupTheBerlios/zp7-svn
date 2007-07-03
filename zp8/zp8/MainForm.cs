using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;

namespace zp8
{
    public partial class MainForm : Form
    {
        Dictionary<int, SongDatabase> m_loaded_dbs = new Dictionary<int, SongDatabase>();
        Dictionary<string, int> m_loaded_dbs_name_to_index = new Dictionary<string, int>();
        bool m_updating_state = false;
        //static MainForm m_form;

        public MainForm()
        {
            //m_form = this;
            InitializeComponent();
            rbdatabase.Checked = true;
        }

        /*
        public static IntPtr HDC
        {
            get
            {
                using (Graphics g = Graphics.FromHwnd(m_form.Handle))
                    return g.GetHdc();
            }
        }
        */

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDbList();
            //SongDatabase sc = new SongDatabase(@"e:\dev\zp8\songs.db");
            ////songDb1.song.AddsongRow("ahoj peggy", "Kamelot", "je mlynu", "1");
            ////songDb1.WriteXml(@"e:\dev\zp8\songs2.xml");
            //SQLiteDataAdapter ada = new SQLiteDataAdapter("SELECT * FROM song", sc.m_conn);
            
            //SQLiteCommandBuilder cb = new SQLiteCommandBuilder(ada);
            ////ada.InsertCommand = (SQLiteCommand)cb.GetInsertCommand();
            ////cb.GetDeleteCommand();
            //ada.Fill(songDb1.song);
            //SongDb xmldb = new SongDb();
            ////xmldb.ReadXml(@"e:\dev\zp8\songs.xml");
            ////songDb1.Merge((DataRow[])(new ArrayList(xmldb.song.Rows)).ToArray(typeof(DataRow)), true, MissingSchemaAction.Add);
            ////songDb1.song.ReadXml(@"e:\dev\zp8\songs.xml");
            //SQLiteTransaction t = sc.m_conn.BeginTransaction();
            ////songDb1.song.AddsongRow(3, "ahoj peggy", "b", "c", "d", "e");
            ////songDb1.AcceptChanges();
            ////ada.Update(songDb1.song);
            ////ada.Update(songDb1, "song");
            ////songDb1.AcceptChanges();
            //ada.Update(songDb1.song);
            //t.Commit();

            ////ada.Update(songDb1, "song");
            ////ada.TableMappings.Add("songs", "songs");
            ////ada.Update(songDb1, "song");
            //int cnt = songDb1.song.Rows.Count;
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 0) e.Cancel = true;
        }

        private static string GetDbTitle(SongDatabase db)
        {
            return (db.Modified ? "*" : "") + db.Name;
        }

        private void LoadDbList()
        {
            SongDatabase lastdb = SelectedDatabase;
            DbManager.Manager.Refresh();
            cbdatabase.Items.Clear();
            m_loaded_dbs.Clear();
            m_loaded_dbs_name_to_index.Clear();
            foreach (SongDatabase db in DbManager.Manager.GetDatabases())
            {
                m_loaded_dbs[cbdatabase.Items.Count] = db;
                m_loaded_dbs_name_to_index[db.Name] = cbdatabase.Items.Count;
                cbdatabase.Items.Add(GetDbTitle(db));
            }
            if (lastdb != null) cbdatabase.SelectedIndex = m_loaded_dbs_name_to_index[lastdb.Name];
        }

        private void mnuNewDb_Click(object sender, EventArgs e)
        {
            string dbname;
            if (NewDbForm.Run(out dbname))
            {
                DbManager.Manager.CreateDatabase(dbname);
                LoadDbList();
            }
        }
        public SongDatabase SelectedDatabase
        {
            get
            {
                try { return m_loaded_dbs[cbdatabase.SelectedIndex]; }
                catch (Exception) { return null; }
            }
        }

        public SongBook SelectedSongBook
        {
            get
            {
                try { return SongBook.Manager.SongBooks[cbsongbook.SelectedIndex]; }
                catch (Exception) { return null; }
            }
        }

        public AbstractSongDatabase SelectedDbOrSb
        {
            get
            {
                if (rbdatabase.Checked) return SelectedDatabase;
                else return SelectedSongBook;
            }
        }

        private void dblist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_updating_state) return;
            //songTable1.Bind(SelectedDb);
            //serversFrame1.Bind(SelectedDb);
            rbdatabase.Checked = true;
            LoadCurrentDbOrSb();
            //UpdateDbState();
        }

        private void LoadCurrentDbOrSb()
        {
            songDatabaseWrapper1.Database = SelectedDbOrSb;
            UpdateDbState();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string dbs = "";
            foreach (SongDatabase db in DbManager.Manager.GetDatabases())
            {
                if (db.Modified)
                {
                    if (dbs != "") dbs += ",";
                    dbs += db.Name;
                }
            }
            if (dbs != "")
            {
                DialogResult res= MessageBox.Show("Databáze " + dbs + " zmìnìny, uložit?", "Zpìvníkátor", MessageBoxButtons.YesNoCancel);
                if (res == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                if (res == DialogResult.No) return;
                if (res == DialogResult.Yes)
                {
                    foreach (SongDatabase db in DbManager.Manager.GetDatabases())
                        if (db.Modified)
                            db.Commit();
                }
            }
            
        }

        private void mnuSaveDb_Click(object sender, EventArgs e)
        {
            SelectedDatabase.Commit();
            UpdateDbState();
        }

        private void UpdateDbState()
        {
            try
            {
                m_updating_state = true;
                cbdatabase.Items[cbdatabase.SelectedIndex] = GetDbTitle(SelectedDatabase);
                dbstatus.Text = SelectedDatabase.Modified ? "Zmìnìno" : "";
                dbsize.Text = String.Format("{0} písní", SelectedDatabase.DataSet.song.Rows.Count);
                dbname.Text = SelectedDatabase.Name;
            }
            finally
            {
                m_updating_state = false;
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                songDatabaseWrapper1.Database = null;
                ImportForm.Run(SelectedDbOrSb);
                UpdateDbState();
            }
            finally
            {
                songDatabaseWrapper1.Database = SelectedDbOrSb;
            }
        }

        private void novýToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SongBook.Manager.CreateNew();
            LoadSbList();
        }

        private void LoadSbList()
        {
            SongBook lastsb = SelectedSongBook;
            cbsongbook.Items.Clear();
            foreach (SongBook sb in SongBook.Manager.SongBooks)
            {
                cbsongbook.Items.Add(sb.Title);
            }
            if (lastsb != null) cbsongbook.SelectedIndex = cbsongbook.Items.IndexOf(lastsb);
        }

        private void cbsongbook_SelectedIndexChanged(object sender, EventArgs e)
        {
            rbsongbook.Checked = true;
            LoadCurrentDbOrSb();
        }

        private void rbsongbook_CheckedChanged(object sender, EventArgs e)
        {
            LoadCurrentDbOrSb();
        }
    }
}
