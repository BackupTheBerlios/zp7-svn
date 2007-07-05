using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace zp8
{
    public partial class DialogBase : Form
    {
        public DialogBase()
        {
            InitializeComponent();
        }

        private void btok_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DialogBase_Shown(object sender, EventArgs e)
        {
            btok.Top = ClientSize.Height - btok.Height - 5;
            btcancel.Top = ClientSize.Height - btcancel.Height - 5;
            btok.Left = ClientSize.Width - btok.Width - btcancel.Width - 10;
            btcancel.Left = ClientSize.Width - btcancel.Width - 5;
        }
    }
}