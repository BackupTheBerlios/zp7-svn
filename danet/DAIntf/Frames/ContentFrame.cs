using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DAIntf
{
    public partial class ContentFrame : UserControl
    {
        public ContentFrame()
        {
            InitializeComponent();
        }
        public virtual string PageTitle { get { return "content"; } }
    }
}
