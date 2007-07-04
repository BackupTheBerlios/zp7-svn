using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace zp8
{
    public partial class OptionsForm : Form
    {
        Options m_options;
        public OptionsForm(Options options)
        {
            InitializeComponent();
            m_options = options;
            foreach (PropertyPage page in m_options.Pages)
            {
                lbobjects.Items.Add(page.m_title);
            }
            if (lbobjects.Items.Count > 0) lbobjects.SelectedIndex = 0;
        }

        public static void Run(Options options)
        {
            OptionsForm win = new OptionsForm(options);
            win.ShowDialog();
        }

        private void lbobjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = m_options.Pages[lbobjects.SelectedIndex];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
