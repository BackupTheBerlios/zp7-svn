using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace zp8
{
    public partial class ImportForm : Form
    {
        List<IDbImportType> m_types = new List<IDbImportType>();
        SongDatabase m_db;
        public ImportForm(SongDatabase db)
        {
            InitializeComponent();
            m_db = db;
            serverBindingSource.DataSource = m_db.DataSet.server;
            foreach (IDbImportType type in DbImport.Types)
            {
                m_types.Add(type);
                imptype.Items.Add(type.Title);
            }
            imptype.SelectedIndex = 0;
        }

        private void imptype_SelectedIndexChanged(object sender, EventArgs e)
        {
            description.Text = m_types[imptype.SelectedIndex].Description;
        }

        private void Work()
        {
            IDbImportType type = m_types[imptype.SelectedIndex];
            foreach (string item in filelist.Items)
            {
                int? serverid = null;
                if (cbserver.Enabled) serverid = (int)lbserver.SelectedValue;

                type.Run(m_db, item, serverid);
            }
        }

        public static bool Run(SongDatabase db)
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
    }
}