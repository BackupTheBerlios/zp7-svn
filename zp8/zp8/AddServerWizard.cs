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
        List<ISongServerType> m_types = new List<ISongServerType>();
        public AddServerWizard()
        {
            InitializeComponent();
            foreach (ISongServerType type in SongServer.GetTypes())
            {
                m_types.Add(type);
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
            ISongServerType type = m_types[servertype.SelectedIndex];
            cbreadonly.Checked = type.Readonly;
            description.Text = type.Description;
        }
    }
}