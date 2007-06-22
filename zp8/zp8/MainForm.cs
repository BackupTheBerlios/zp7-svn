using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Finisar.SQLite;

namespace zp8
{
    public partial class MainForm : Form
    {
        Dictionary<int, SongDatabase> m_loaded_dbs = new Dictionary<int, SongDatabase>();
        Dictionary<string, int> m_loaded_dbs_name_to_index = new Dictionary<string, int>();
        bool m_updating_state = false;
        public MainForm()
        {
            InitializeComponent();
        }

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
            SongDatabase lastdb = SelectedDb;
            DbManager.Manager.Refresh();
            dblist.Items.Clear();
            m_loaded_dbs.Clear();
            m_loaded_dbs_name_to_index.Clear();
            foreach (SongDatabase db in DbManager.Manager.GetDatabases())
            {
                m_loaded_dbs[dblist.Items.Count] = db;
                m_loaded_dbs_name_to_index[db.Name] = dblist.Items.Count;
                dblist.Items.Add(GetDbTitle(db));
            }
            if (lastdb != null) dblist.SelectedIndex = m_loaded_dbs_name_to_index[lastdb.Name];
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
        public SongDatabase SelectedDb
        {
            get
            {
                try { return m_loaded_dbs[dblist.SelectedIndex]; }
                catch (Exception) { return null; }
            }
        }

        private void dblist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_updating_state) return;
            songTable1.Bind(SelectedDb);
            UpdateDbState();
        }

        private void zeStaréhoZpìvníkátoruToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                SongDatabase db = SelectedDb;
                foreach (string file in openFileDialog1.FileNames)
                {
                    db.ImportZp6File(file);
                }
                LoadDbList();
            }
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
            SelectedDb.Commit();
            UpdateDbState();
        }

        private void UpdateDbState()
        {
            try
            {
                m_updating_state = true;
                dblist.Items[dblist.SelectedIndex] = GetDbTitle(SelectedDb);
                dbstatus.Text = SelectedDb.Modified ? "Zmìnìno" : "";
                dbsize.Text = String.Format("{0} písní", SelectedDb.DataSet.song.Rows.Count);
                dbname.Text = SelectedDb.Name;
            }
            finally
            {
                m_updating_state = false;
            }
        }
    }
}
