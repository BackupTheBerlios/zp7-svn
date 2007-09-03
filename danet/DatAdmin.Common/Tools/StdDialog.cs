using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DatAdmin
{
    public static class StdDialog
    {
        public static void ShowError(object error)
        {
            MessageBox.Show(error.ToString(), "DatAdmin", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void ShowInfo(object info)
        {
            MessageBox.Show(info.ToString(), "DatAdmin", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
