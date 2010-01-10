using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace zp8
{
    public partial class ZoomControl : UserControl
    {
        public ZoomControl()
        {
            InitializeComponent();
        }

        public float Zoom { get { return (float)Math.Pow(5, tbzoom.Value / 10.0); } }
        public event EventHandler ChangedZoom;

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            lbzoom.Text = String.Format("{0}%", (int)(Zoom * 100 + 0.5));
            if (ChangedZoom != null) ChangedZoom(sender, e);
        }
    }
}
