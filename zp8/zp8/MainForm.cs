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

        private void LoadDbList()
        {
            dblist.Items.Clear();
            m_loaded_dbs.Clear();
            foreach (SongDatabase db in DbManager.Manager.GetDatabases())
            {
                m_loaded_dbs[dblist.Items.Count] = db;
                dblist.Items.Add(db.Name);
            }
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

        private void dblist_SelectedIndexChanged(object sender, EventArgs e)
        {
            SongDatabase db = m_loaded_dbs[dblist.SelectedIndex];
        }
    }

}
