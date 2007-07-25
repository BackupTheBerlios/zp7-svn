using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace zp8
{
    public partial class FiltersForm : Form
    {
        public FiltersForm()
        {
            InitializeComponent();

            lbfiltertype.Items.Clear();
            foreach (ConfigurableSongFilterStruct f in SongFilters.FilterTypes) lbfiltertype.Items.Add(f.Name);
            lbfiltertype.SelectedIndex = 0;

            LoadBsList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!BookStyle.Exists(tbnewname.Text))
            {
                BookStyle bs = new BookStyle(tbnewname.Text);
                bs.Save();
                LoadBsList();
            }
        }

        private void LoadBsList()
        {
            lbfilters.Items.Clear();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (lbfilters.SelectedIndex < 0) return;
            string name = (string)lbfilters.Items[lbfilters.SelectedIndex];

            if (MessageBox.Show("Opravdu vymazat styl zpìvníku " + name + "?", "Zpìvníkátor", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                BookStyle bs = new BookStyle(name);
                File.Delete(bs.FileName);
                LoadBsList();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (lbfilters.SelectedIndex < 0) return;
            string name = (string)lbfilters.Items[lbfilters.SelectedIndex];
            BookStyle bs = BookStyle.LoadBookStyle(name);
            OptionsForm.Run(bs);
            bs.Save();
        }

        public static void Run()
        {
            FiltersForm frm = new FiltersForm();
            frm.ShowDialog();
        }
    }
}