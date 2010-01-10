using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace zp8
{
    public partial class InputBox : Form
    {
        public InputBox()
        {
            InitializeComponent();
        }
        public static string Run(string label, string defvalue, IEnumerable<string> values)
        {
            InputBox dlg = new InputBox();
            dlg.label1.Text = label;
            dlg.tbxValue.Text = defvalue;
            dlg.tbxValue.Select();
            if (values != null)
            {
                foreach (string val in values) dlg.tbxValue.Items.Add(val);
            }
            if (DialogResult.OK == dlg.ShowDialog())
            {
                return dlg.tbxValue.Text;
            }
            return null;
        }
        public static string Run(string label, string defvalue)
        {
            return Run(label, defvalue, null);
        }
    }
}

