using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace zp8
{
    public class DirectorySongExporterProperties : PropertyPageBase
    {
        string m_folderName;

        [DisplayName("Výstupní složka")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string FolderName
        {
            get { return m_folderName; }
            set { m_folderName = value; }
        }
    }

    public class GroupOfSongs
    {
        public List<ISongRow> Songs = new List<ISongRow>();
        public int Index;
        public string Name;
    }

    public class DirectorySongHolder
    {
        public DirectorySongHolder(IEnumerable songs)
        {
            int songindex = 0;
            foreach (ISongRow song in songs)
            {
                if (!Groups.ContainsKey(song.groupname))
                {
                    Groups[song.groupname] = new GroupOfSongs();
                    Groups[song.groupname].Index = Groups.Count;
                    Groups[song.groupname].Name = song.groupname;
                }
                Groups[song.groupname].Songs.Add(song);
                Songs.Add(song);
                SongIndexes[song] = ++songindex;
                SongGroups[song] = Groups[song.groupname];
            }
            foreach (GroupOfSongs grp in Groups.Values) grp.Songs.Sort(Sorting.GetComparison(SongOrder.GroupTitle));
            Songs.Sort(Sorting.GetComparison(SongOrder.GroupTitle));
            SortedGroups.AddRange(Groups.Values);
            SortedGroups.Sort(delegate(GroupOfSongs a, GroupOfSongs b) { return String.Compare(a.Name, b.Name); });
        }

        public Dictionary<string, GroupOfSongs> Groups = new Dictionary<string,GroupOfSongs>();
        public List<ISongRow> Songs = new List<ISongRow>();
        public Dictionary<ISongRow, int> SongIndexes = new Dictionary<ISongRow, int>();
        public Dictionary<ISongRow, GroupOfSongs> SongGroups = new Dictionary<ISongRow, GroupOfSongs>();
        public List<GroupOfSongs> SortedGroups = new List<GroupOfSongs>();
    }

    [ConfigurableSongFilter(Name = "Výstup do složky")]
    public class DirectorySongExporter : PropertyPageBase, ISongFormatter, ICustomSongFilter
    {
        string m_name;
        [Browsable(false)]
        [XmlIgnore]
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        Encoding m_encoding = System.Text.Encoding.UTF8;
        [DisplayName("Kódování")]
        public string Encoding
        {
            get { return m_encoding.WebName; }
            set { m_encoding = System.Text.Encoding.GetEncoding(value); }
        }

        bool m_writeIndex;
        [Category("Index")]
        [DisplayName("Zapisovat index")]
        public bool WriteIndex
        {
            get { return m_writeIndex; }
            set { m_writeIndex = value; }
        }

        string m_indexFileName = "";
        [Category("Index")]
        [DisplayName("Jméno souboru")]
        public string IndexFileName
        {
            get { return m_indexFileName; }
            set { m_indexFileName = value; }
        }

        string m_indexHeader = "";
        [Category("Index")]
        [DisplayName("Hlavièka souboru")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string IndexHeader
        {
            get { return m_indexHeader; }
            set { m_indexHeader = value; }
        }

        string m_indexGroupRepeat = "";
        [Category("Index")]
        [DisplayName("Šablona pro skupinu")]
        [Description("Opakuje se pro každou skupinu, mùže obsahovat makro $[GROUPINDEX] pro èíslo skupiny")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string IndexGroupRepeat
        {
            get { return m_indexGroupRepeat; }
            set { m_indexGroupRepeat = value; }
        }

        string m_indexBetweenGroupsAndSongs = "";
        [Category("Index")]
        [DisplayName("Skupiny-písnì")]
        [Description("Text mezi seznamem skupin a seznamem písní")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string IndexBetweenGroupsAndSongs
        {
            get { return m_indexBetweenGroupsAndSongs; }
            set { m_indexBetweenGroupsAndSongs = value; }
        }

        string m_indexSongRepeat = "";
        [Category("Index")]
        [DisplayName("Šablona pro píseò")]
        [Description("Opakuje se pro každou píseò, mùže obsahovat makro $[SONGINDEX] pro èíslo písnì")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string IndexSongRepeat
        {
            get { return m_indexSongRepeat; }
            set { m_indexSongRepeat = value; }
        }

        string m_indexFooter = "";
        [Category("Index")]
        [DisplayName("Patièka souboru")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string IndexFooter
        {
            get { return m_indexFooter; }
            set { m_indexFooter = value; }
        }

        bool m_writeGroups;
        [Category("Skupiny")]
        [DisplayName("Zapisovat skupiny")]
        public bool WriteGroups
        {
            get { return m_writeGroups; }
            set { m_writeGroups = value; }
        }

        string m_groupFileMask = "";
        [Category("Skupiny")]
        [DisplayName("Maska souboru")]
        [Description("Maska souboru pro skupinu, mùže obsahovat $[GROUPINDEX] pro èíslo skupiny")]
        public string GroupFileMask
        {
            get { return m_groupFileMask; }
            set { m_groupFileMask = value; }
        }

        string m_groupHeader = "";
        [Category("Skupiny")]
        [DisplayName("Hlavièka souboru")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string GroupHeader
        {
            get { return m_groupHeader; }
            set { m_groupHeader = value; }
        }

        string m_groupSongRepeat = "";
        [Category("Skupiny")]
        [DisplayName("Šablona pro píseò")]
        [Description("Opakuje se pro každou píseò, mùže obsahovat makro $[SONGINDEX] pro èíslo písnì")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string GroupSongRepeat
        {
            get { return m_groupSongRepeat; }
            set { m_groupSongRepeat = value; }
        }

        string m_groupFooter = "";
        [Category("Skupiny")]
        [DisplayName("Patièka souboru")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string GroupFooter
        {
            get { return m_groupFooter; }
            set { m_groupFooter = value; }
        }

        bool m_writeSeparateSongs;
        [Category("Písnì")]
        [DisplayName("Zapisovat jednotlivì")]
        [Description("Zda zapisovat písnì po jedné do jednoho souboru")]
        public bool WriteSeparateSongs
        {
            get { return m_writeSeparateSongs; }
            set { m_writeSeparateSongs = value; }
        }

        bool m_writeGroupedSongs;
        [Category("Písnì")]
        [DisplayName("Zapisovat seskupenì")]
        [Description("Zda zapisovat písnì po skupinách do jednoho souboru")]
        public bool WriteGroupedSongs
        {
            get { return m_writeGroupedSongs; }
            set { m_writeGroupedSongs = value; }
        }

        string m_groupedSongsFileMask = "";
        [Category("Písnì")]
        [DisplayName("Maska souboru seskupenì")]
        [Description("Maska souboru pro zápis seskupených písní do souboru, mùže obsahovat $[GROUINDEX]")]
        public string GroupedSongsFileMask
        {
            get { return m_groupedSongsFileMask; }
            set { m_groupedSongsFileMask = value; }
        }

        string m_songFileMask = "";
        [Category("Písnì")]
        [DisplayName("Maska souboru jednotlivì")]
        [Description("Maska souboru pro zápis jednotlivých písní do souboru, mùže obsahovat $[SONGINDEX] a $[GROUINDEX]")]
        public string SongFileMask
        {
            get { return m_songFileMask; }
            set { m_songFileMask = value; }
        }

        string m_streamFormatter;
        [Category("Písnì")]
        [DisplayName("Styl formátování písnì")]
        [Editor(typeof(DependedFilterEditor<IStreamSongFormatter>), typeof(UITypeEditor))]
        public string StreamFormatter
        {
            get { return m_streamFormatter; }
            set { m_streamFormatter = value; }
        }

        private IStreamSongFormatter GetStreamFormatter()
        {
            return (IStreamSongFormatter)SongFilters.FilterByName(m_streamFormatter);
        }

        private string MakeTemplate(string tpl, GroupOfSongs grp)
        {
            tpl = tpl.Replace("$[GROUPINDEX]", grp.Index.ToString());
            tpl = tpl.Replace("$[GROUP]", grp.Name);
            return tpl;
        }

        private string MakeTemplate(string tpl, ISongRow song, DirectorySongHolder dsh)
        {
            tpl = tpl.Replace("$[SONGINDEX]", dsh.SongIndexes[song].ToString());
            tpl = Templates.MakeTemplate(tpl, song);
            tpl = MakeTemplate(tpl, dsh.SongGroups[song]);
            return tpl;
        }

        private void WriteIndexFile(TextWriter fw, DirectorySongHolder dsh)
        {
            fw.Write(m_indexHeader);
            foreach (GroupOfSongs grp in dsh.SortedGroups)
            {
                fw.Write(MakeTemplate(m_indexGroupRepeat, grp));
            }
            fw.Write(m_indexBetweenGroupsAndSongs);
            foreach (ISongRow song in dsh.Songs)
            {
                fw.Write(MakeTemplate(m_indexSongRepeat, song, dsh));
            }
            fw.Write(m_indexFooter);
        }

        private void WriteGroupFile(StreamWriter fw, GroupOfSongs grp, DirectorySongHolder dsh)
        {
            fw.Write(MakeTemplate(m_groupHeader, grp));
            foreach (ISongRow song in grp.Songs)
            {
                fw.Write(MakeTemplate(m_groupSongRepeat, song, dsh));
            }
            fw.Write(MakeTemplate(m_groupFooter, grp));
        }

        #region ISongFormatter Members

        public void Format(InetSongDb db, object props, IWaitDialog wait)
        {
            DirectorySongExporterProperties p = (DirectorySongExporterProperties)props;
            IStreamSongFormatter songfmt = GetStreamFormatter();
            string directory = p.FolderName;
            DirectorySongHolder dsh = new DirectorySongHolder(db.song.Rows);
            // nejdrive zapiseme index
            wait.Message("Zapisuji " + m_indexFileName);
            if (m_writeIndex)
            {
                using (FileStream fsw = new FileStream(Path.Combine(directory, m_indexFileName), FileMode.Create))
                {
                    using (StreamWriter fw = new StreamWriter(fsw, m_encoding))
                    {
                        WriteIndexFile(fw, dsh);
                    }
                }
            }

            // pak skupinove soubory
            if (m_writeGroups)
            {
                foreach (GroupOfSongs grp in dsh.Groups.Values)
                {
                    string path = Path.Combine(directory, MakeTemplate(m_groupFileMask, grp));
                    try { Directory.CreateDirectory(Path.GetDirectoryName(path)); }
                    catch (Exception) { }

                    wait.Message("Zapisuji " + path);
                    if (wait.Canceled) return;

                    using (FileStream fsw = new FileStream(path, FileMode.Create))
                    {
                        using (StreamWriter fw = new StreamWriter(fsw, m_encoding))
                        {
                            WriteGroupFile(fw, grp, dsh);
                        }
                    }
                }
            }

            // pisne - nejdrive zgrupovane
            if (m_writeGroupedSongs)
            {
                foreach (GroupOfSongs grp in dsh.Groups.Values)
                {
                    string path = Path.Combine(directory, MakeTemplate(m_groupedSongsFileMask, grp));
                    try { Directory.CreateDirectory(Path.GetDirectoryName(path)); }
                    catch (Exception) { }

                    wait.Message("Zapisuji " + path);
                    if (wait.Canceled) return;

                    InetSongDb tmp = new InetSongDb();
                    foreach (ISongRow song in grp.Songs)
                    {
                        DbTools.AddSongRow(song, tmp);
                    }
                    using (FileStream fw = new FileStream(path, FileMode.Create))
                    {
                        songfmt.Format(tmp, fw, wait);
                    }
                }
            }

            // pisne - po jedne
            if (m_writeSeparateSongs)
            {
                foreach (ISongRow song in db.song.Rows)
                {
                    string path = Path.Combine(directory, MakeTemplate(m_songFileMask, song, dsh));
                    try { Directory.CreateDirectory(Path.GetDirectoryName(path)); }
                    catch (Exception) { }

                    wait.Message("Zapisuji " + path);
                    if (wait.Canceled) return;

                    InetSongDb tmp = new InetSongDb();
                    DbTools.AddSongRow(song, tmp);
                    using (FileStream fw = new FileStream(path, FileMode.Create))
                    {
                        songfmt.Format(tmp, fw, wait);
                    }
                }
            }
        }

        #endregion

        #region ISongFilter Members

        public string Title
        {
            get { return m_name; }
        }

        public string Description
        {
            get { return "Výstup do adresáøe"; }
        }

        public object CreateDynamicProperties()
        {
            return new DirectorySongExporterProperties();
        }

        #endregion

    }
}
