using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace zp8
{
    public static class IOTools
    {
        public static void CopyStream(Stream fr, Stream fw)
        {
            byte[] buffer = new byte[0x100];
            do
            {
                int bytes = fr.Read(buffer, 0, 0x100);
                fw.Write(buffer, 0, bytes);
                if (bytes <= 0) return;

            } while (true);
        }
    }
}
