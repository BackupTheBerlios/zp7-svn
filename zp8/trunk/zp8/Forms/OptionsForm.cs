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
        object m_options;
        List<object> m_pages = new List<object>();
        public OptionsForm(object options)
        {
            InitializeComponent();
            m_options = options;
            foreach (PropertyPageReference page in Options.GetPropertyPages(m_options))
            {
                lbobjects.Items.Add(page.Title);
                m_pages.Add(page.PageObject);
            }
            if (lbobjects.Items.Count > 0) lbobjects.SelectedIndex = 0;
        }

        public static void Run(object options)
        {
            OptionsForm win = new OptionsForm(options);
            win.ShowDialog();
        }

        private void lbobjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            //propertyGrid1.Item.Clear();

            //foreach(
            propertyGrid1.SelectedObject = m_pages[lbobjects.SelectedIndex];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
