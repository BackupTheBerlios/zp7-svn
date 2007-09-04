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
    public partial class SchemaFrame : ContentFrame
    {
        IPhysicalConnection m_pconn;
        DbConnection m_conn;
        DataTable m_table = null;

        public SchemaFrame(IPhysicalConnection conn)
        {
            InitializeComponent();
            m_pconn = conn;
            m_conn = conn.SystemConnection;
            ConnTools.InvokeVoid(m_pconn, GetSchemas, m_invoker, ShowTableSchemas);
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
            if (tbparams.Text != "")
            {
                string[] pars = tbparams.Text.Split(',');
                List<string> ps = new List<string>();
                foreach (string p in pars)
                {
                    if (p == "null") ps.Add(null);
                    else ps.Add(p);
                }
                ConnTools.InvokeVoid(m_pconn, delegate() { GetSchema(colname, ps.ToArray()); }, m_invoker, ShowTable);
            }
            else
            {
                ConnTools.InvokeVoid(m_pconn, delegate() { GetSchema(colname); }, m_invoker, ShowTable);
            }
        }

        private void GetSchema(string colname, string []pars)
        {
            Logging.Info("Getting schema {0}, parameters {1}", colname, pars.Length);
            m_table = m_conn.GetSchema(colname, pars);
        }
        private void GetSchema(string colname)
        {
            Logging.Info("Getting schema {0}", colname);
            m_table = m_conn.GetSchema(colname);
        }
    }
}
