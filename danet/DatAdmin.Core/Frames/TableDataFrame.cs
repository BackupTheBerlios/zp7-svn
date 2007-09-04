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
    public partial class TableDataFrame : ContentFrame
    {
        ITableSource m_conn;
        DataTable m_table;

        public TableDataFrame(ITableSource conn)
        {
            InitializeComponent();
            m_conn = conn;
            LoadData();
        }
        public override string PageTitle
        {
            get { return m_conn.TableName; }
        }
        public void LoadData()
        {
            ConnTools.InvokeVoid(m_conn, DoLoadData, m_invoker, LoadedData);
        }
        void DoLoadData()
        {
            m_table = m_conn.GetTabularData(new TableDataProperties());
        }
        void LoadedData()
        {
            dataGridView1.DataSource = m_table;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Logging.Warning("Data error, row={0}, col={1}, error={2}", e.RowIndex, e.ColumnIndex, e.Exception.Message);
        }
    }
}

