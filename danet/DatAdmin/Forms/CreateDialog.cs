using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DAIntf;

namespace DatAdmin
{
    public partial class CreateDialog : Form
    {
        ITreeNode m_parent;
        Dictionary<string, List<ICreateFactoryItem>> m_items = new Dictionary<string, List<ICreateFactoryItem>>();
        Dictionary<ICreateFactoryItem, int> m_imageIndexes = new Dictionary<ICreateFactoryItem, int>();

        public CreateDialog(ITreeNode parent)
        {
            m_parent = parent;
            InitializeComponent();
        }

        private void CreateDialog_Load(object sender, EventArgs e)
        {
            foreach (ICreateFactoryItem item in CreateFactory.GetItems(m_parent))
            {
                if (!m_items.ContainsKey(item.Group))
                {
                    m_items[item.Group] = new List<ICreateFactoryItem>();
                    ListViewItem grp = listView1.Items.Add(item.Group);
                    grp.ImageIndex = 0;
                }
                m_items[item.Group].Add(item);
                Bitmap bmp = item.Bitmap;
                if (bmp != null)
                {
                    m_imageIndexes[item] = imageList2.Images.Count;
                    imageList2.Images.Add(item.Bitmap);
                }
                else
                {
                    m_imageIndexes[item] = -1;
                }
            }
            if (listView1.Items.Count>0)
            {
                listView1.Items[0].Focused = true;
                listView1.Items[0].Selected = true;
                listView1_SelectedIndexChanged(sender, e);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            string grp = listView1.SelectedItems[0].Text;
            listView2.Items.Clear();
            foreach (ICreateFactoryItem item in m_items[grp])
            {
                ListViewItem it = listView2.Items.Add(item.Title);
                it.ImageIndex = m_imageIndexes[item];
                it.Tag = item;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0 && newname.Text != "")
            {
                ICreateFactoryItem item = (ICreateFactoryItem)listView2.SelectedItems[0].Tag;
                if (item.Create(m_parent, newname.Text)) Close();
            }
        }

    }
}