using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace zp8
{
    public partial class AddServerWizard : Form
    {
        List<ISongServer> m_servers = new List<ISongServer>();
        List<ISongServerFactoryType> m_ftypes = new List<ISongServerFactoryType>();
        ISongServerFactory m_factory;
        public AddServerWizard()
        {
            InitializeComponent();
            foreach (ISongServerFactoryType type in SongServer.GetFactoryTypes())
            {
                m_ftypes.Add(type);
                servertype.Items.Add(type.Name);
            }
        }

        public static ISongServer[] Run()
        {
            AddServerWizard win = new AddServerWizard();
            if (win.ShowDialog() == DialogResult.OK) return win.m_servers.ToArray();
            return null;
        }

        private void servertype_SelectedIndexChanged(object sender, EventArgs e)
        {
            ISongServerFactoryType type = m_ftypes[servertype.SelectedIndex];
            description.Text = type.Description;
        }

        private void wizardPage2_ShowFromNext(object sender, EventArgs e)
        {
            ISongServerFactoryType type = m_ftypes[servertype.SelectedIndex];
            m_factory = type.CreateFactory();
            propertyGrid1.SelectedObject = m_factory;
        }
    }
}