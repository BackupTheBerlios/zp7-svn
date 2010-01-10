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

            LoadFilterList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tbnewname.Text != "" && !SongFilters.ExistsCustomFilter(tbnewname.Text))
            {
                Type t = SongFilters.FilterTypes[lbfiltertype.SelectedIndex].Type;
                ISongFilter flt = (ISongFilter)t.GetConstructor(new Type[] { }).Invoke(new object[] { });
                ((ICustomSongFilter)flt).Name = tbnewname.Text;
                SongFilters.SaveCustomFilter(tbnewname.Text, flt);
                LoadFilterList();
            }
        }

        private void LoadFilterList()
        {
            lbfilters.Items.Clear();
            foreach (string flt in SongFilters.GetCustomFilters())
            {
                lbfilters.Items.Add(flt);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (lbfilters.SelectedIndex < 0) return;
            string name = (string)lbfilters.Items[lbfilters.SelectedIndex];

            if (MessageBox.Show("Opravdu vymazat filtr " + name + "?", "Zpìvníkátor", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SongFilters.DeleteFilter(name);
                LoadFilterList();
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
            ISongFilter flt = SongFilters.LoadCustomFilter(name);
            PropertiesForm.Run(flt);
            SongFilters.SaveCustomFilter(name, flt);
        }

        public static void Run()
        {
            FiltersForm frm = new FiltersForm();
            frm.ShowDialog();
        }
    }
}