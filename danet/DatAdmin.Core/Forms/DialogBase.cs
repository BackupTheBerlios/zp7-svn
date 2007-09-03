using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DatAdmin
{
    public partial class DialogBase : Form
    {
        public DialogBase()
        {
            InitializeComponent();
        }

        private void DialogBase_Load(object sender, EventArgs e)
        {
            bt_ok.Left = ClientSize.Width - bt_ok.Width - bt_cancel.Width - 50;
            bt_cancel.Left = ClientSize.Width - bt_cancel.Width - 20;
            bt_ok.Top = ClientSize.Height - bt_ok.Height - 25;
            bt_cancel.Top = ClientSize.Height - bt_cancel.Height - 25;
        }
    }
}