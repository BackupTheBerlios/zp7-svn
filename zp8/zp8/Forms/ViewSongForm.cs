using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace zp8
{
    public partial class ViewSongForm : Form
    {
        static ViewSongForm Instance;

        ISongRow m_song;

        public ViewSongForm()
        {
            InitializeComponent();
        }

        public static void ShowSong(ISongRow song)
        {
            if (Instance == null)
            {
                Instance = new ViewSongForm();
            }
            Instance.Song = song;
            Instance.Show();
            Instance.BringToFront();
        }

        public ISongRow Song
        {
            get { return m_song; }
            set
            {
                m_song = value;
                songView1.SongText = m_song.SongText;
                Text = String.Format("{0} - {1}", m_song.Title, m_song.GroupName);
            }

        }

        private void ViewSongForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}