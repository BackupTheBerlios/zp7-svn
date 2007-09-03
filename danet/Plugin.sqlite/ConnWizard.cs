using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using DatAdmin;

namespace Plugin.sqlite
{
    public partial class ConnWizard : Form
    {
        string m_name;
        ITreeNode m_parent;

        public ConnWizard(ITreeNode parent, string name)
        {
            InitializeComponent();
            rbopenExisting.Checked = true;
            lbversion.SelectedIndex = 1;
            m_name = name;
            m_parent = parent;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            tbfilename.Enabled = btbrowse.Enabled = true;
            lbversion.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            tbfilename.Enabled = btbrowse.Enabled = false;
            lbversion.Enabled = true;
        }

        internal bool CreateNew { get { return rbcreateNew.Checked; } }

        internal string GenerateConnectionString()
        {
            if (CreateNew)
            {
                return String.Format("Data Source={0};New=True;Version={1}", Path.Combine(m_parent.FileSystemPath, m_name + ".db3"), lbversion.SelectedIndex + 2);
            }
            else
            {
                return String.Format("Data Source={0}", tbfilename.Text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string conns = GenerateConnectionString();
            SQLiteConnection conn = new SQLiteConnection(conns);
            IAsyncVoid res = Async.InvokeVoid(conn.Open);
            try
            {
                res.Wait();
                conn.Close();
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception err)
            {
                StdDialog.ShowError(err);
                return;
            }
        }

        private void btbrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = tbfilename.Text;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbfilename.Text = openFileDialog1.FileName;
            }
        }
    }


    [CreateFactoryItem]
    public class SQLiteCreateWizard : ICreateFactoryItem
    {
        #region ICreateFactoryItem Members

        public string Title
        {
            get { return "SQLite"; }
        }

        public string Group
        {
            get { return "s_connection"; }
        }

        public System.Drawing.Bitmap Bitmap
        {
            get { return StdIcons.img_database; }
        }

        public bool Create(ITreeNode parent, string name)
        {
            ConnWizard wiz = new ConnWizard(parent, name);
            wiz.ShowDialog();
            if (wiz.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                if (wiz.CreateNew)
                {
                    SQLiteConnection conn = new SQLiteConnection(wiz.GenerateConnectionString());
                    conn.Open();
                    conn.Close();
                }
                else
                {
                    SQLiteStoredConnection con = new SQLiteStoredConnection();
                    con.ConnectionString = wiz.GenerateConnectionString();
                    con.Save(Path.Combine(parent.FileSystemPath, name + ".con"));
                }
                //MsSqlConnection con = new MsSqlConnection();
                //con.ConnectionString = wiz.GenerateConnectionString(true);
                //con.OneDatabase = wiz.OneDatabase;
                //con.Save(Path.Combine(parent.FileSystemPath, name + ".con"));
                return true;
            }
            return false;
        }

        #endregion
    }
}