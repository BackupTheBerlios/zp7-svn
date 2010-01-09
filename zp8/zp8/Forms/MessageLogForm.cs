using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace zp8
{
    public partial class MessageLogForm : Form, IWaitDialog
    {
        bool m_canceled = false;
        bool m_dialog = false;
        public MessageLogForm()
        {
            InitializeComponent();
        }

        public static MessageLogForm Show(string msg, bool cancelable)
        {
            MessageLogForm win = new MessageLogForm();
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
            txaction.Text = msg;
            txlog.AppendText(String.Format("{0}: {1}\r\n", DateTime.Now, msg));
            Update();
            Application.DoEvents();
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            m_canceled = true;
            if (m_dialog) Close();
        }

        public void FinishAndWait()
        {
            Hide();
            Text = "Akce dokonèena";
            button1.Text = "OK";
            button1.Visible = true;
            m_dialog = true;
            UseWaitCursor = false;
            ShowDialog();
        }
    }
}