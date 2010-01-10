using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace zp8
{
    public partial class TemplateTextForm : zp8.DialogBase
    {
        public TemplateTextForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.SelectedText = "$[TITLE]";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.SelectedText = "$[AUTHOR]";
        }

        public static string Run(string text)
        {
            TemplateTextForm win = new TemplateTextForm();
            win.textBox1.Text = text;
            if (win.ShowDialog() == DialogResult.OK) return win.textBox1.Text;
            return text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.SelectedText = "$[GROUP]";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.SelectedText = "$[NL]";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.SelectedText = "$[SONGINDEX]";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.SelectedText = "$[GROUPINDEX]";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.SelectedText = "$[REMARK]";
        }
    }
}

