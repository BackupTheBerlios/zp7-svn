using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;

namespace DatAdmin
{
    public partial class ConnWizard : Form
    {
        DbProviderFactory m_factory;
        DataTable m_factoryClasses;
        DbConnectionStringBuilder m_builder;

        public ConnWizard()
        {
            InitializeComponent();
        }

        private void wpprovider_CloseFromNext(object sender, Gui.Wizard.PageEventArgs e)
        {
            string invname;
            if (provider.SelectedIndex > 0)
                invname = m_factoryClasses.Rows[provider.SelectedIndex]["InvariantName"].ToString();
            else
                invname = provider.Text;
            try
            {
                m_factory = DbProviderFactories.GetFactory(invname);
            }
            catch (Exception err)
            {
                StdDialog.ShowError(String.Format(
                    "{0}:{1}\n{2}", Texts.Get("s_cannot_create_provider"), invname, err.Message));
                e.Page = wpprovider;
                return;
            }
        }

        private void wpconnprops_ShowFromNext(object sender, EventArgs e)
        {
            try
            {
                m_builder = m_factory.CreateConnectionStringBuilder();
                propertyGrid1.SelectedObject = m_builder;
            }
            catch (Exception err)
            {
                StdDialog.ShowError(err.Message);
            }
        }

        private void wizard1_Load(object sender, EventArgs e)
        {
            m_factoryClasses = DbProviderFactories.GetFactoryClasses();
            foreach (DataRow row in m_factoryClasses.Rows)
            {
                provider.Items.Add(row["InvariantName"]);
            }
        }

        private void wpconnprops_CloseFromNext(object sender, Gui.Wizard.PageEventArgs e)
        {
            //string conns = m_builder.ConnectionString;
            //DbConnection conn = m_factory.CreateConnection();
            //conn.ConnectionString = conns;
            //IAsyncVoid res = Async.InvokeVoid(conn.Open);
            //try
            //{
            //    res.Wait();
            //}
            //catch (Exception err)
            //{
            //    StdDialog.ShowError(err);
            //    e.Page = wpconnprops;
            //    return;
            //}
            //e.Page = wpfinish;
        }

        private void wpfinish_CloseFromNext(object sender, Gui.Wizard.PageEventArgs e)
        {

        }
    }

    [CreateFactoryItem]
    public class ConnectionCreateWizard : ICreateFactoryItem
    {

        #region ICreateFactoryItem Members

        public string Title
        {
            get { return "Database Connection"; }
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
