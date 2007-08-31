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
    public partial class SchemaFrame : DAIntf.ContentFrame
    {
        DbConnection m_conn;
        public SchemaFrame(DbConnection conn)
        {
            InitializeComponent();
            m_conn = conn;
            DataTable table = conn.GetSchema();
            dataGridView1.DataSource = table;
        }
        public override string PageTitle
        {
            get { return "Schema"; }
        }
    }
}

