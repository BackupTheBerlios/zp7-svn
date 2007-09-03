using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DatAdmin
{
    public partial class WaitDialog : Form, IWaitDialog
    {
        bool m_canceled;
        public WaitDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_canceled = true;
        }


        #region IWaitDialog Members

        void IWaitDialog.Show()
        {
            m_canceled = false;
            Show();
        }

        void IWaitDialog.Hide()
        {
            m_canceled = false;
            Hide();
        }

        bool IWaitDialog.Canceled
        {
            get { return m_canceled; }
        }

        #endregion
    }
}