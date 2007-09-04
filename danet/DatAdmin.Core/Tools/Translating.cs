using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DatAdmin
{
    public static class Translating
    {
        public static void TranslateControl(Control ctrl)
        {
            if (ShouldTranslate(ctrl.Text)) ctrl.Text = TranslateText(ctrl.Text);

            if (ctrl is ListView)
            {
                ListView obj = (ListView)ctrl;
                foreach (ColumnHeader hdr in obj.Columns)
                {
                    if (ShouldTranslate(hdr.Text)) hdr.Text = TranslateText(hdr.Text);
                }
            }

            foreach (Control child in ctrl.Controls)
            {
                TranslateControl(child);
            }
        }

        private static bool ShouldTranslate(string text)
        {
            return text.StartsWith("s_");
        }

        private static string TranslateText(string text)
        {
            return Texts.Get(text);
        }
    }
}
