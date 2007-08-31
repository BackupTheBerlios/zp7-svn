using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;
using DAIntf;

namespace DatAdmin
{
    public partial class SchemaFrame : DAIntf.ContentFrame, IInvoker
    {
        DbConnection m_conn;
        DataTable m_table = null;

        public SchemaFrame(DbConnection conn)
        {
            InitializeComponent();
            m_conn = conn;
            Async.InvokeVoid(GetSchemas, this, ShowTableSchemas);
        }
        private void GetSchemas()
        {
            Logging.Info("Getting list of schema collections");
            m_table = m_conn.GetSchema();
        }
        private void ShowTable()
        {
            dataGridView1.DataSource = m_table;
        }
        private void ShowTableSchemas()
        {
            DataTable tbl = m_table;
            dataGridView1.DataSource = tbl;
            lbcolname.Items.Clear();
            foreach (DataRow row in tbl.Rows)
            {
                lbcolname.Items.Add(row[0].ToString());
            }
            lbcolname.SelectedIndex = 0;
        }
        public override string PageTitle
        {
            get { return "Schema"; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string colname = lbcolname.Items[lbcolname.SelectedIndex].ToString();
            Async.InvokeVoid(delegate() { GetSchema(colname); }, this, ShowTable);
        }

        private void GetSchema(string colname)
        {
            Logging.Info("Getting schema {0}", colname);
            m_table = m_conn.GetSchema(colname);
        }

        #region IInvoker Members

        public void DoInvoke(SimpleCallback callback)
        {
            Invoke(callback);
        }

        #endregion
    }
}

