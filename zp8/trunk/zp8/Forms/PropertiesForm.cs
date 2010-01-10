using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace zp8
{
    public partial class PropertiesForm : Form
    {
        public PropertiesForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
        public static void Run(object o)
        {
            PropertiesForm frm = new PropertiesForm();
            frm.propertyGrid1.SelectedObject = o;
            frm.ShowDialog();
        }
    }
}