using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace zp8
{
    public partial class SongTable : UserControl, ISongSource
    {
        SongDatabase m_dataset;
        public SongTable()
        {
            InitializeComponent();
        }
        public void Bind(SongDatabase db)
        {
            songBindingSource.DataSource = db.DataSet.song;
            m_dataset = db;
            //songDbBindingSource.DataSource = db.DataSet;
            //songDbBindingSource.DataMember = "song";
        }
        public BindingSource GetBindingSource() { return songBindingSource; }
        public SongDatabase GetDataSet() { return m_dataset; }
    }
}
