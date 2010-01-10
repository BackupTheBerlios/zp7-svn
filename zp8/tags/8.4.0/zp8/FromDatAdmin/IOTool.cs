using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections.Specialized;

namespace DatAdmin
{
    public static class IOTool
    {
        public static void CopyStream(Stream fr, Stream fw)
        {
            byte[] buffer = new byte[0x100];
            for (; ; )
            {
                int len = fr.Read(buffer, 0, buffer.Length);
                fw.Write(buffer, 0, len);
                if (len <= 0) break;
            }
        }
        public static byte[] ReadToEnd(Stream fr)
        {
            MemoryStream fw = new MemoryStream();
            CopyStream(fr, fw);
            return fw.ToArray();
        }

        /// <summary>
        /// Creates a relative path from one file
        /// or folder to another.
        /// </summary>
        /// <param name="fromDirectory">
        /// Contains the directory that defines the
        /// start of the relative path.
        /// </param>
        /// <param name="toPath">
        /// Contains the path that defines the
        /// endpoint of the relative path.
        /// </param>
        /// <returns>
        /// The relative path from the start
        /// directory to the end path.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string RelativePathTo(
            string fromDirectory, string toPath)
        {
            if (fromDirectory == null)
                throw new ArgumentNullException("fromDirectory");

            if (toPath == null)
                throw new ArgumentNullException("toPath");

            bool isRooted = Path.IsPathRooted(fromDirectory)
                && Path.IsPathRooted(toPath);

            if (isRooted)
            {
                bool isDifferentRoot = string.Compare(
                    Path.GetPathRoot(fromDirectory),
                    Path.GetPathRoot(toPath), true) != 0;

                if (isDifferentRoot)
                    return toPath;
            }

            StringCollection relativePath = new StringCollection();
            string[] fromDirectories = fromDirectory.Split(
                Path.DirectorySeparatorChar);

            string[] toDirectories = toPath.Split(
                Path.DirectorySeparatorChar);

            int length = Math.Min(
                fromDirectories.Length,
                toDirectories.Length);

            int lastCommonRoot = -1;

            // find common root
            for (int x = 0; x < length; x++)
            {
                if (string.Compare(fromDirectories[x],
                    toDirectories[x], true) != 0)
                    break;

                lastCommonRoot = x;
            }
            if (lastCommonRoot == -1)
                return toPath;

            // add relative folders in from path
            for (int x = lastCommonRoot + 1; x < fromDirectories.Length; x++)
                if (fromDirectories[x].Length > 0)
                    relativePath.Add("..");

            // add to folders to path
            for (int x = lastCommonRoot + 1; x < toDirectories.Length; x++)
                relativePath.Add(toDirectories[x]);

            // create relative path
            string[] relativeParts = new string[relativePath.Count];
            relativePath.CopyTo(relativeParts, 0);

            string newPath = string.Join(
                Path.DirectorySeparatorChar.ToString(),
                relativeParts);

            return newPath;
        }

        public static string LoadTextFile(string file)
        {
            using (StreamReader sr = new StreamReader(file)) return sr.ReadToEnd();
        }

        public static bool FileIsLink(string file)
        {
            return file.ToLower().EndsWith(".lnk");
        }

        public static void CreateLink(string origfile, string linkpath)
        {
            string fn = origfile;
            fn = Path.ChangeExtension(fn, ".lnk");
            fn = Path.GetFileName(fn);
            fn = Path.Combine(linkpath, fn);
            fn = GetUniqueFileName(fn);
            using (StreamWriter sw = new StreamWriter(fn)) sw.Write(origfile);
        }

        public static string GetLinkContent(string file)
        {
            if (FileIsLink(file))
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    return sr.ReadToEnd().Trim();
                }
            }
            return file;
        }

        public static string GetUniqueFileName(string fn)
        {
            string fn0 = Path.Combine(Path.GetDirectoryName(fn), Path.GetFileNameWithoutExtension(fn));
            string ext = Path.GetExtension(fn);
            fn = fn0;
            int dindex = 1;
            while (File.Exists(fn + ext))
            {
                fn = fn0 + dindex;
                dindex++;
            }
            return fn + ext;
        }

        public static bool IsVersioningDirectory(string dir)
        {
            dir = Path.GetFileName(dir).ToLower();
            return dir == ".svn" || dir == "cvs";
        }


        public static void CopyDirectory(string origpath, string newpath)
        {
            CopyDirectory(new DirectoryInfo(origpath), new DirectoryInfo(newpath));
        }

        public static void CopyDirectory(DirectoryInfo source, DirectoryInfo target)
        {
            // Check if the target directory exists, if not, create it.
            if (!Directory.Exists(target.FullName))
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it’s new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyDirectory(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}

