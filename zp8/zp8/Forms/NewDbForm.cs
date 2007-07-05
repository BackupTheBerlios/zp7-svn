using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace zp8
{
    public partial class NewDbForm : zp8.DialogBase
    {
        public NewDbForm()
        {
            InitializeComponent();
        }

        public static bool Run(out string dbname)
        {
            NewDbForm dlg = new NewDbForm();
            DialogResult res = dlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                dbname = dlg.textBox1.Text;
                return true;
            }
            else
            {
                dbname = null;
                return false;
            }
        }
    }
}

