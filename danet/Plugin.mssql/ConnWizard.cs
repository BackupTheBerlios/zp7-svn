using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DAIntf;

namespace Plugin.mssql
{
    public partial class ConnWizard : Form
    {
        public ConnWizard()
        {
            InitializeComponent();
        }

        string GenerateConnectionString(bool includepwd)
        {
            if (comboBox1.SelectedIndex == 1)
            {
                return String.Format("Data Source={0};User ID={1};Password={2}", datasource.Text, login.Text, includepwd ? password.Text : "******");
            }
            else
            {
                return String.Format("Data Source={0};Integrated Security=SSPI", datasource.Text);
            }
        }

        private void pgcreadentials_CloseFromNext(object sender, Gui.Wizard.PageEventArgs e)
        {
            string conns = GenerateConnectionString(true);
            SqlConnection conn = new SqlConnection(conns);
            IAsyncVoid res = Async.InvokeVoid(conn.Open);
            try
            {
                res.Wait();
            }
            catch (Exception err)
            {
                StdDialog.ShowError(err);
                e.Page = pgcreadentials;
                return;
            }
        }

        private void datasource_TextChanged(object sender, EventArgs e)
        {
            connectionstring.Text = GenerateConnectionString(false);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool enabledcr = comboBox1.SelectedIndex == 1;
            login.Enabled = enabledcr;
            password.Enabled = enabledcr;
            datasource_TextChanged(sender, e);
        }

        private void wizard1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox1_SelectedIndexChanged(sender, e);
        }
    }

    public class MsSqlCreateWizard : ICreateFactoryItem
    {

        #region ICreateFactoryItem Members

        public string Title
        {
            get { return "Microsoft SQL"; }
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
            ConnWizard wiz = new ConnWizard();
            wiz.ShowDialog();
            return wiz.DialogResult == System.Windows.Forms.DialogResult.OK;
        }

        #endregion
    }
}