using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace zp8
{
    public partial class FileCollectionEditorForm : DialogBase
    {
        public FileCollectionEditorForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (filelist.SelectedIndex >= 0) filelist.Items.RemoveAt(filelist.SelectedIndex);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filelist.Items.AddRange(openFileDialog1.FileNames);
            }
        }

        public static FileCollection Run(FileCollection files, string filter)
        {
            FileCollectionEditorForm win = new FileCollectionEditorForm();
            win.openFileDialog1.Filter = filter;
            foreach (string file in files.Files) win.filelist.Items.Add(file);
            if (win.ShowDialog() == DialogResult.OK)
            {
                List<string> res = new List<string>();
                foreach (string item in win.filelist.Items)
                {
                    res.Add(item);
                }
                return new FileCollection(res.ToArray());
            }
            return files;
        }
    }
}