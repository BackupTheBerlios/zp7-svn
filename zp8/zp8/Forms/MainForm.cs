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
        Dictionary<SongBook, int> m_loaded_songbooks = new Dictionary<SongBook, int>();
        bool m_updating_state = false;
        int? m_activeDbPage, m_activeSbPage;

        static MainForm m_form;
        static Graphics m_mainGraphics;

        public MainForm()
        {
            m_form = this;
            m_mainGraphics = Graphics.FromHwnd(Handle);

            InitializeComponent();
            Text = "Zpìvníkátor " + VersionInfo.Version;
            rbdatabase.Checked = true;

            WindowState = global::zp8.Properties.Settings.Default.MainWindowState;
        }

        public static IntPtr HDC
        {
            get { return m_mainGraphics.GetHdc(); }
        }
        public static Graphics MainGraphics
        {
            get { return m_mainGraphics; }
        }
        public static MainForm Form { get { return m_form; } }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDbList();

            string db = GlobalCfg.Default.currentdb;
            if (m_loaded_dbs_name_to_index.ContainsKey(db))
            {
                cbdatabase.SelectedIndex = m_loaded_dbs_name_to_index[db];
            }
            LoadCurrentDbOrSb();
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
                SongDatabase newdb = DbManager.Manager.CreateDatabase(dbname);
                LoadDbList();
                SelectedDatabase = newdb;
            }
        }
        public SongDatabase SelectedDatabase
        {
            get
            {
                try { return m_loaded_dbs[cbdatabase.SelectedIndex]; }
                catch (Exception) { return null; }
            }
            set
            {
                if (m_loaded_dbs_name_to_index.ContainsKey(value.Name))
                {
                    cbdatabase.SelectedIndex = m_loaded_dbs_name_to_index[value.Name];
                }
            }
        }

        public SongBook SelectedSongBook
        {
            get
            {
                try { return SongBook.Manager.SongBooks[cbsongbook.SelectedIndex]; }
                catch (Exception) { return null; }
            }
            set
            {
                if (m_loaded_songbooks.ContainsKey(value))
                {
                    cbsongbook.SelectedIndex = m_loaded_songbooks[value];
                }
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
            if (SelectedDatabase != null) GlobalCfg.Default.currentdb = SelectedDatabase.Name;
            //UpdateDbState();
        }

        private void LoadCurrentDbOrSb()
        {
            if (rbsongbook.Checked)
            {
                if (!TabControl1.TabPages.Contains(tbsongbook))
                {
                    m_activeDbPage = TabControl1.SelectedIndex;
                    TabControl1.TabPages.Add(tbsongbook);
                    if (m_activeSbPage.HasValue) TabControl1.SelectedIndex = m_activeSbPage.Value;
                }
                cbdatabase.BackColor = SystemColors.Window;
                cbsongbook.BackColor = Color.Yellow;
            }
            else
            {
                if (TabControl1.TabPages.Contains(tbsongbook))
                {
                    m_activeSbPage = TabControl1.SelectedIndex;
                    TabControl1.TabPages.Remove(tbsongbook);
                    if (m_activeDbPage.HasValue) TabControl1.SelectedIndex = m_activeDbPage.Value;
                }
                cbsongbook.BackColor = SystemColors.Window;
                cbdatabase.BackColor = Color.Yellow;
            }
            songDatabaseWrapper1.Database = SelectedDbOrSb;
            UpdateDbState();
            songView1.LoadSong();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (SongBook book in SongBook.Manager.SongBooks)
            {
                if (book.Modified)
                {
                    DialogResult res = MessageBox.Show("Zpìvník " + book.Title + " zmìnìn, uložit?", "Zpìvníkátor", MessageBoxButtons.YesNoCancel);
                    if (res == DialogResult.No) continue;
                    if (res == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        return;
                    }
                    if (book.FileName != null && book.FileName != "")
                    {
                        book.Save();
                    }
                    else
                    {
                        if (saveZP.ShowDialog() == DialogResult.OK)
                        {
                            book.FileName = saveZP.FileName;
                        }
                        else
                        {
                            e.Cancel = true;
                            return;
                        }
                        book.Save();
                    }
                }
            }

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
                if (SelectedDatabase != null)
                {
                    m_updating_state = true;
                    cbdatabase.Items[cbdatabase.SelectedIndex] = GetDbTitle(SelectedDatabase);
                    dbstatus.Text = SelectedDatabase.Modified ? "Zmìnìno" : "";
                    dbsize.Text = String.Format("{0} písní", SelectedDatabase.DataSet.song.Rows.Count);
                    dbname.Text = SelectedDatabase.Name;
                }
                else
                {
                    dbstatus.Text = dbsize.Text = dbname.Text = "";
                }
            }
            finally
            {
                m_updating_state = false;
            }
        }

        private void novýToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SongBook newsb = SongBook.Manager.CreateNew();
            LoadSbList();
            SelectedSongBook = newsb;
        }

        private void LoadSbList()
        {
            SongBook lastsb = SelectedSongBook;
            cbsongbook.Items.Clear();
            m_loaded_songbooks.Clear();
            foreach (SongBook sb in SongBook.Manager.SongBooks)
            {
                m_loaded_songbooks[sb] = cbsongbook.Items.Count;
                cbsongbook.Items.Add(sb.Title);
            }
            if (lastsb != null && m_loaded_songbooks.ContainsKey(lastsb)) cbsongbook.SelectedIndex = m_loaded_songbooks[lastsb];
        }

        private void cbsongbook_SelectedIndexChanged(object sender, EventArgs e)
        {
            rbsongbook.Checked = true;
            LoadCurrentDbOrSb();
            songBookFrame1.SongBook = SelectedSongBook;
        }

        private void rbsongbook_CheckedChanged(object sender, EventArgs e)
        {
            LoadCurrentDbOrSb();
        }

        /*
        private void pøidatDoZpìvníkuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SongBook sb = SelectedSongBook;
            SongDatabase db = SelectedDatabase;
            if (sb != null && db != null)
            {
                foreach (SongDb.songRow row in songTable1.GetSelectedSongs())
                {
                    SongDb.songRow newrow = sb.DataSet.song.NewsongRow();
                    foreach (DataColumn col in sb.DataSet.song.Columns)
                    {
                        if (col.ColumnName != "ID") newrow[col.ColumnName] = row[col.ColumnName];
                    }

                    sb.DataSet.song.AddsongRow(newrow);
                }
            }
        }
        */

        private void uložitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SongBook sb = SelectedSongBook;
            if (sb == null) return;
            if (sb.FileName == null) uložitNaToolStripMenuItem_Click(sender, e);
            sb.Save();
            LoadSbList();
        }

        private void uložitNaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SongBook sb = SelectedSongBook;
            if (sb == null) return;
            if (saveZP.ShowDialog() == DialogResult.OK)
            {
                sb.FileName = saveZP.FileName;
                sb.Save();
                LoadSbList();
            }
        }

        private void obecnéToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm.Run(GlobalOpts.Default);
            GlobalOpts.Default.Save();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            global::zp8.Properties.Settings.Default.MainWindowState = WindowState;
            GlobalCfg.Default.Save();
            zp8.Properties.Settings.Default.Save();
        }

        private void konecToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void importPísníToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void vlastnostiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedSongBook != null) songBookFrame1.PropertiesDialog();
        }

        private void naèístToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string ext = Path.GetExtension(openFileDialog1.FileName).ToLower();
                if (ext == ".zp")
                {
                    SongBook newsb = SongBook.Manager.LoadExisting(openFileDialog1.FileName);
                    LoadSbList();
                    SelectedSongBook = newsb;
                }
            }
        }

        private void pdfExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            songBookFrame1.ExportAsPDF();
        }

        private void stylyZpìvníkuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BookStylesForm.Run();
        }

        private void zmìnitStylToolStripMenuItem_Click(object sender, EventArgs e)
        {
            songBookFrame1.ChangeBookStyle();
        }

        private void tisknoutAktuálníPíseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SongDb.songRow song = songView1.Song;
            if (song != null && printDialog1.ShowDialog() == DialogResult.OK)
            {
                SongPrinter sp = new SongPrinter(song, printDialog1.PrinterSettings);
                sp.Run();
            }
        }

        private void exportPísnìDoPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SongDb.songRow song = songView1.Song;
            if (song != null && savePDF.ShowDialog() == DialogResult.OK)
            {
                SongPDFPrinter.Print(song, savePDF.FileName);
            }
        }

        private void vytisknoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            songBookFrame1.PrintSongBook();
        }

        private void songBookFrame1_ChangedPageInfo(object sender, EventArgs e)
        {
            tspages.Text = songBookFrame1.PageInfo;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tbfilter.Text = "";
            ApplyFilter();
        }

        private void upravitPíseòToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (songView1.Song != null) EditSongForm.Run(songView1.Song, SelectedDbOrSb.DataSet);
        }

        private void nováPíseòToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedDbOrSb != null)
            {
                SongDb.songRow row = SelectedDbOrSb.CreateSong();
                EditSongForm.Run(row, SelectedDbOrSb.DataSet);
            }
        }

        private void pøidatVybranouPíseòDoZpìvníkuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSelectedSongToDb(SelectedSongBook);
        }

        private void AddSelectedSongToDb(AbstractSongDatabase db)
        {
            if (db == null) return;
            foreach (SongDb.songRow song in songTable1.GetSelectedSongsOrFocused())
            {
                DbTools.AddSongRow(song, db);
            }
        }

        private void pøidatVybranouPíseòDoDatabázeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSelectedSongToDb(SelectedDatabase);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (tbfilter.Text != "")
            {
                songDatabaseWrapper1.SongBindingSource.Filter = String.Format("searchtext LIKE '%{0}%'", Searching.MakeSearchText(tbfilter.Text));
            }
            else
            {
                songDatabaseWrapper1.SongBindingSource.Filter = null;
            }
            songsByGroupFrame1.Reload();
            songView1.LoadSong();
        }

        private void tbfilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ApplyFilter();
            }
            if (e.KeyCode == Keys.Escape)
            {
                tbfilter.Text = "";
                ApplyFilter();
            }
        }

        private void songDatabaseWrapper1_SongChanged(object sender, EventArgs e)
        {
            UpdateDbState();
        }

        private void exportPísníToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportForm.Run(songDatabaseWrapper1, songTable1.GetSelectedSongs());
        }

        private void filtryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FiltersForm.Run();
        }

        private void wWWStránkaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://zpevnik.net/");
        }

        private void oProgramuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = "";
            text += "Zpìvníkátor " + VersionInfo.Version + "\r\n";
            text += "(c) JenaSoft 1998-2007\r\n";
            text += "WWW: http://zpevnik.net";
            MessageBox.Show(text, "O programu...");
        }

        private void smazatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<SongDb.songRow> songs = new List<SongDb.songRow>();
            songs.AddRange(songTable1.GetSelectedSongsOrFocused());
            if (MessageBox.Show(String.Format("Opravdu vymazat {0} písní ?", songs.Count), "Zpìvníkátor", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (SongDb.songRow song in songs) song.Delete();
            }
        }

        private void obsahToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "zp8.chm"));
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (TabControl1.SelectedTab == tbsongbook)
            {
                if (e.KeyCode == Keys.PageUp) songBookFrame1.ScrollPage(-1);
                if (e.KeyCode == Keys.PageDown) songBookFrame1.ScrollPage(1);
            }
        }

        private void kinoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (songView1.Song != null) ViewSongForm.ShowSong(songView1.Song);
        }
    }
}
