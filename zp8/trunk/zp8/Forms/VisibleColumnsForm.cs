using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace zp8
{
    public partial class VisibleColumnsForm : DialogBase
    {
        public VisibleColumnsForm(DataGridView grid)
        {
            InitializeComponent();
            foreach(DataGridViewColumn col in grid.Columns)
            {
                lbcolumns.Items.Add(col.HeaderText);
                lbcolumns.SetItemChecked(lbcolumns.Items.Count - 1, col.Visible);
            }
        }
        public static bool Run(DataGridView grid)
        {
            VisibleColumnsForm win = new VisibleColumnsForm(grid);
            if (win.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < win.lbcolumns.Items.Count;i++ )
                {
                    grid.Columns[i].Visible = win.lbcolumns.GetItemChecked(i);
                }
                return true;
            }
            return false;
        }
    }
}