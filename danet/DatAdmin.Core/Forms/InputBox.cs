using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DatAdmin
{
    public partial class InputBox : DialogBase
    {
        public InputBox()
        {
            InitializeComponent();
        }
        public static string Run(string label, string defvalue)
        {
            InputBox dlg = new InputBox();
            dlg.label1.Text = label;
            dlg.textBox1.Text = defvalue;
            dlg.textBox1.Select();
            if (DialogResult.OK == dlg.ShowDialog())
            {
                return dlg.textBox1.Text;
            }
            return null;            
        }
    }
}

