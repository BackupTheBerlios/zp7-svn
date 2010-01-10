using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace zp8
{
    public partial class ChangeBookStyleForm : DialogBase
    {
        public ChangeBookStyleForm()
        {
            InitializeComponent();
        }

        private void ChangeBookStyleForm_Load(object sender, EventArgs e)
        {
            foreach (string name in BookStyle.GetSbTypes())
                lbstyle.Items.Add(name);
        }
        public static string Run()
        {
            ChangeBookStyleForm win = new ChangeBookStyleForm();
            if (win.ShowDialog() == DialogResult.OK && win.lbstyle.SelectedIndex >= 0)
            {
                return (string)win.lbstyle.Items[win.lbstyle.SelectedIndex];
            }
            else
            {
                return null;
            }
        }
    }
}

