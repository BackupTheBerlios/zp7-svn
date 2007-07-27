using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace zp8
{
    public partial class ImportForm : Form
    {
        List<ISongParser> m_types = new List<ISongParser>();
        AbstractSongDatabase m_db;
        public ImportForm(AbstractSongDatabase db)
        {
            InitializeComponent();
            m_db = db;
            serverBindingSource.DataSource = m_db.DataSet.server;
            foreach (ISongParser type in SongFilters.EnumFilters<ISongParser>())
            {
                m_types.Add(type);
                imptype.Items.Add(type.Title);
            }
            imptype.SelectedIndex = 0;
        }

        private void imptype_SelectedIndexChanged(object sender, EventArgs e)
        {
            description.Text = m_types[imptype.SelectedIndex].Description;
            openFileDialog1.Filter = m_types[imptype.SelectedIndex].FileDialogFilter;
        }

        private void Work()
        {
            ISongParser type = m_types[imptype.SelectedIndex];
            InetSongDb xmldb = new InetSongDb();
            int? serverid = null;
            if (cbserver.Checked) serverid = (int)lbserver.SelectedValue;
            foreach (string item in filelist.Items)
            {
                using (FileStream fr = new FileStream(item, FileMode.Open))
                {
                    type.Parse(fr, xmldb);
                }
            }
            m_db.ImportSongs(xmldb, serverid);
        }

        public static bool Run(AbstractSongDatabase db)
        {
            ImportForm frm = new ImportForm(db);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                frm.Work();
                return true;
            }
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filelist.Items.AddRange(openFileDialog1.FileNames);
            }
        }

        private void cbserver_CheckedChanged(object sender, EventArgs e)
        {
            lbserver.Enabled = cbserver.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (filelist.SelectedIndex >= 0) filelist.Items.RemoveAt(filelist.SelectedIndex);
        }
    }
}