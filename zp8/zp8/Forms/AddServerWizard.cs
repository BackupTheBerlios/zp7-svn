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
        //List<ISongServer> m_servers = new List<ISongServer>();
        //List<ISongServerFactoryType> m_ftypes = new List<ISongServerFactoryType>();
        List<SongServerType> m_ftypes = new List<SongServerType>();
        ISongServer m_server;

        public AddServerWizard()
        {
            InitializeComponent();
            foreach (SongServerType type in SongServer.GetServerTypes())
            {
                m_ftypes.Add(type);
                servertype.Items.Add(type.Title);
            }
        }

        public static ISongServer Run()
        {
            AddServerWizard win = new AddServerWizard();
            if (win.ShowDialog() == DialogResult.OK) return win.m_server;
            return null;
        }

        private void servertype_SelectedIndexChanged(object sender, EventArgs e)
        {
            SongServerType type = m_ftypes[servertype.SelectedIndex];
            description.Text = type.Description;
        }

        private void wizardPage2_ShowFromNext(object sender, EventArgs e)
        {
            SongServerType type = m_ftypes[servertype.SelectedIndex];
            m_server = type.CreateServer();
            //m_factory = type.CreateFactory();
            propertyGrid1.SelectedObject = m_server;
        }

        private void wizardPage3_ShowFromNext(object sender, EventArgs e)
        {
            if (m_server != null)
            {
                wizardPage3.IsFinishPage = true;
                endpage.PageText = "Vytvoøen server " + m_server.ToString();
            }
            else
            {
                wizardPage3.IsFinishPage = false;
                endpage.PageText = "";
            }
        }
    }
}