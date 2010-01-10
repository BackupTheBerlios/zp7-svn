using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace zp8
{
    public partial class WaitForm : Form, IWaitDialog
    {
        bool m_canceled = false;
        public WaitForm()
        {
            InitializeComponent();
        }

        public static WaitForm Show(string msg, bool cancelable)
        {
            WaitForm win = new WaitForm();
            win.Message(msg);
            win.button1.Visible = cancelable;
            win.Show();
            Application.DoEvents();
            return win;
        }

        #region IWaitDialog Members

        public bool Canceled
        {
            get
            {
                Application.DoEvents();
                return m_canceled;
            }
        }

        public void Message(string msg)
        {
            labCurWork.Text = msg;
            Update();
            Application.DoEvents();
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            m_canceled = true;
        }
    }
}