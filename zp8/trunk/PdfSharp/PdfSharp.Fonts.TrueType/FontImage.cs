#region PDFsharp - A .NET library for processing PDF
//
// Authors:
//   Stefan Lange (mailto:Stefan.Lange@pdfsharp.com)
//
// Copyright (c) 2005-2007 empira Software GmbH, Cologne (Germany)
//
// http://www.pdfsharp.com
// http://sourceforge.net/projects/pdfsharp
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
#endregion

#define VERBOSE_

using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using PdfSharp.Drawing;
using PdfSharp.Internal;

using Fixed = System.Int32;
using FWord = System.Int16;
using UFWord = System.UInt16;

namespace PdfSharp.Fonts.TrueType
{
  /// <summary>
  /// Represents a TrueType font in memory.
  /// </summary>
  class FontImage
  {
    public FontImage(FontImage fontImage)
    {
      this.offsetTable = fontImage.offsetTable;
    }

#if Gdip
    /// <summary>
    /// Initializes a new instance of the <see cref="FontImage"/> class.
    /// </summary>
    public FontImage(System.Drawing.Font font, XPdfFontOptions options)
    {
#if DEBUG_
      NativeMethods.LOGFONT logFont = new NativeMethods.LOGFONT();
      font.ToLogFont(logFont);
#endif
      int error;
      IntPtr hfont = font.ToHfont();
      IntPtr hdc = NativeMethods.GetDC(IntPtr.Zero);
      error = Marshal.GetLastWin32Error();
      IntPtr oldFont = NativeMethods.SelectObject(hdc, hfont);
      error = Marshal.GetLastWin32Error();
      // size is exactly the size of the font file.
      int size = NativeMethods.GetFontData(hdc, 0, 0, null, 0);
      error = Marshal.GetLastWin32Error();
      this.bytes = new byte[size];
      int xx = NativeMethods.GetFontData(hdc, 0, 0, this.bytes, this.bytes.Length);
      NativeMethods.SelectObject(hdc, oldFont);
      NativeMethods.ReleaseDC(IntPtr.Zero, hdc);
      error.GetType();

      Read();
    }
#endif

    /// <summary>
    /// Gets the bytes that represents the font image.
    /// </summary>
    public byte[] Bytes
    {
      get { return this.bytes; }
    }
    byte[] bytes;

    internal OffsetTable offsetTable;

    /// <summary>
    /// The dictionary of all font tables.
    /// </summary>
    internal Dictionary<string, TableDirectoryEntry> tableDictionary = new Dictionary<string, TableDirectoryEntry>();

    internal CMapTable cmap;
    internal ControlValueTable cvt;
    internal FontProgram fpgm;
    internal MaximumProfileTable maxp;
    internal NameTable name;
    internal ControlValueProgram prep;
    internal FontHeaderTable head;
    internal HorizontalHeaderTable hhea;
    internal HorizontalMetricsTable hmtx;
    internal OS2Table os2;
    internal PostScriptTable post;
    internal GlyphDataTable glyf;
    internal IndexToLocationTable loca;
    internal GlyphSubstitutionTable gsub;

    public bool CanRead
    {
      get { return this.bytes != null; }
    }

    public bool CanWrite
    {
      get { return this.bytes == null; }
    }

    /// <summary>
    /// Adds the specified table to this font image.
    /// </summary>
    public void AddTable(TrueTypeFontTable fontTable)
    {
      if (!CanWrite)
        throw new InvalidOperationException("Font image cannot be modified.");

      if (fontTable == null)
        throw new ArgumentNullException("fontTable");

      if (fontTable.FontImage == null)
      {
        fontTable.fontImage = this;
      }
      else
      {
        Debug.Assert(fontTable.FontImage.CanRead);
        // Create a reference to this font table
        fontTable = new IRefFontTable(this, fontTable);
      }

      //Debug.Assert(fontTable.FontImage == null);
      //fontTable.fontImage = this;

      this.tableDictionary[fontTable.DirectoryEntry.Tag] = fontTable.DirectoryEntry;
      switch (fontTable.DirectoryEntry.Tag)
      {
        case TableTagNames.CMap:
          this.cmap = fontTable as CMapTable;
          break;

        case TableTagNames.Cvt:
          this.cvt = fontTable as ControlValueTable;
          break;

        case TableTagNames.Fpgm:
          this.fpgm = fontTable as FontProgram;
          break;

        case TableTagNames.MaxP:
          this.maxp = fontTable as MaximumProfileTable;
          break;

        case TableTagNames.Name:
          this.name = fontTable as NameTable;
          break;

        case TableTagNames.Head:
          this.head = fontTable as FontHeaderTable;
          break;

        case TableTagNames.HHea:
          this.hhea = fontTable as HorizontalHeaderTable;
          break;

        case TableTagNames.HMtx:
          this.hmtx = fontTable as HorizontalMetricsTable;
          break;

        case TableTagNames.OS2:
          this.os2 = fontTable as OS2Table;
          break;

        case TableTagNames.Post:
          this.post = fontTable as PostScriptTable;
          break;

        case TableTagNames.Glyf:
          this.glyf = fontTable as GlyphDataTable;
          break;

        case TableTagNames.Loca:
          this.loca = fontTable as IndexToLocationTable;
          break;

        case TableTagNames.GSUB:
          this.gsub = fontTable as GlyphSubstitutionTable;
          break;

        case TableTagNames.Prep:
          this.prep = fontTable as ControlValueProgram;
          break;
      }
    }

    /// <summary>
    /// Reads all required table from the font prgram.
    /// </summary>
    internal void Read()
    {
      try
      {
        // Read offset table
        this.offsetTable.Version = ReadULong();
        this.offsetTable.TableCount = ReadUShort();
        this.offsetTable.SearchRange = ReadUShort();
        this.offsetTable.EntrySelector = ReadUShort();
        this.offsetTable.RangeShift = ReadUShort();

        // Move to table dictionary at position 12
        Debug.Assert(this.pos == 12);
        //this.tableDictionary = (this.offsetTable.TableCount);
        for (int idx = 0; idx < this.offsetTable.TableCount; idx++)
        {
          TableDirectoryEntry entry = TableDirectoryEntry.ReadFrom(this);
          this.tableDictionary.Add(entry.Tag, entry);
#if VERBOSE
          Debug.WriteLine(String.Format("Font table: {0}", entry.Tag));
#endif
        }

        // PDFlib checks this, but it is not part of the OpenType spec anymore
        if (this.tableDictionary.ContainsKey("bhed"))
          throw new NotSupportedException("Bitmap fonts are not supported by PDFsharp.");

        // Read required tables
        if (Seek(CMapTable.Tag) != -1)
          this.cmap = new CMapTable(this);

        if (Seek(ControlValueTable.Tag) != -1)
          this.cvt = new ControlValueTable(this);

        if (Seek(FontProgram.Tag) != -1)
          this.fpgm = new FontProgram(this);

        if (Seek(MaximumProfileTable.Tag) != -1)
          this.maxp = new MaximumProfileTable(this);

        if (Seek(NameTable.Tag) != -1)
          this.name = new NameTable(this);

        if (Seek(FontHeaderTable.Tag) != -1)
          this.head = new FontHeaderTable(this);

        if (Seek(HorizontalHeaderTable.Tag) != -1)
          this.hhea = new HorizontalHeaderTable(this);

        if (Seek(HorizontalMetricsTable.Tag) != -1)
          this.hmtx = new HorizontalMetricsTable(this);

        if (Seek(OS2Table.Tag) != -1)
          this.os2 = new OS2Table(this);

        if (Seek(PostScriptTable.Tag) != -1)
          this.post = new PostScriptTable(this);

        if (Seek(GlyphDataTable.Tag) != -1)
          this.glyf = new GlyphDataTable(this);

        if (Seek(IndexToLocationTable.Tag) != -1)
          this.loca = new IndexToLocationTable(this);

        if (Seek(GlyphSubstitutionTable.Tag) != -1)
          this.gsub = new GlyphSubstitutionTable(this);

        if (Seek(ControlValueProgram.Tag) != -1)
          this.prep = new ControlValueProgram(this);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    /// <summary>
    /// Creates a new font image that is a subset of this font image containing only the specified glyphs.
    /// </summary>
    public FontImage CreateFontSubSet(Dictionary<int, object> glyphs, bool cidFont)
    {
      // Create new font image
      FontImage fontImage = new FontImage(this);

      // Create new loca and glyf table
      IndexToLocationTable loca = new IndexToLocationTable();
      loca.ShortIndex = this.loca.ShortIndex;
      GlyphDataTable glyf = new GlyphDataTable();

      // Add all required tables
      //fontImage.AddTable(this.os2);
      if (!cidFont)
        fontImage.AddTable(this.cmap);
      if (this.cvt != null)
        fontImage.AddTable(this.cvt);
      if (this.fpgm != null)
        fontImage.AddTable(this.fpgm);
      fontImage.AddTable(glyf);
      fontImage.AddTable(this.head);
      fontImage.AddTable(this.hhea);
      fontImage.AddTable(this.hmtx);
      fontImage.AddTable(loca);
      if (this.maxp != null)
        fontImage.AddTable(this.maxp);
      //fontImage.AddTable(this.name);
      if (this.prep != null)
        fontImage.AddTable(this.prep);

      // Get closure of used glyphs
      this.glyf.CompleteGlyphClosure(glyphs);

      // Create a sorted array of all used glyphs
      int glyphCount = glyphs.Count;
      int[] glyphArray = new int[glyphCount];
      glyphs.Keys.CopyTo(glyphArray, 0);
      Array.Sort<int>(glyphArray);

      // Calculate new size of glyph table.
      int size = 0;
      for (int idx = 0; idx < glyphCount; idx++)
        size += this.glyf.GetGlyphSize(glyphArray[idx]);
      glyf.DirectoryEntry.Length = size;

      // Create new loca table
      int numGlyphs = this.maxp.numGlyphs;
      loca.locaTable = new int[numGlyphs + 1];

      // Create new glyf table
      glyf.glyphTable = new byte[glyf.DirectoryEntry.PaddedLength];

      // Fill new glyf and loca table
      int glyphOffset = 0;
      int glyphIndex = 0;
      for (int idx = 0; idx < numGlyphs; idx++)
      {
        loca.locaTable[idx] = glyphOffset;
        if (glyphIndex < glyphCount && glyphArray[glyphIndex] == idx)
        {
          glyphIndex++;
          byte[] bytes = this.glyf.GetGlyphData(idx);
          int length = bytes.Length;
          if (length > 0)
          {
            Buffer.BlockCopy(bytes, 0, glyf.glyphTable, glyphOffset, length);
            glyphOffset += length;
          }
        }
      }
      loca.locaTable[numGlyphs] = glyphOffset;

      // Compile font tables into byte array
      fontImage.Compile();

      return fontImage;
    }

    /// <summary>
    /// Compiles the font to its binary representation.
    /// </summary>
    void Compile()
    {
      MemoryStream stream = new MemoryStream();
      TrueTypeFontWriter writer = new TrueTypeFontWriter(stream);

      int tableCount = this.tableDictionary.Count;
      int selector = entrySelectors[tableCount];

      this.offsetTable.Version = 0x00010000;
      this.offsetTable.TableCount = tableCount;
      this.offsetTable.SearchRange = (ushort)((1 << selector) * 16);
      this.offsetTable.EntrySelector = (ushort)selector;
      this.offsetTable.RangeShift = (ushort)((tableCount - (1 << selector)) * 16);
      this.offsetTable.Write(writer);

      // Sort tables by tag name
      string[] tags = new string[tableCount];
      this.tableDictionary.Keys.CopyTo(tags, 0);
      Array.Sort<string>(tags, StringComparer.Ordinal);

#if VERBOSE
      Debug.WriteLine("Start Compile");
#endif
      // Write tables in alphabetical order
      int tablePosition = 12 + 16 * tableCount;
      for (int idx = 0; idx < tableCount; idx++)
      {
        TableDirectoryEntry entry = this.tableDictionary[tags[idx]];
#if DEBUG
        if (entry.Tag == "glyf" || entry.Tag == "loca")
          GetType();
#endif
        entry.FontTable.PrepareForCompilation();
        entry.Offset = tablePosition;
        writer.Position = tablePosition;
        entry.FontTable.Write(writer);
        int endPosition = writer.Position;
        tablePosition = endPosition;
        writer.Position = 12 + 16 * idx;
        entry.Write(writer);
#if VERBOSE
        Debug.WriteLine(String.Format("  Write Table '{0}', offset={1}, length={2}, checksum={3}, ", entry.Tag, entry.Offset, entry.Length, entry.CheckSum));
#endif
      }
#if VERBOSE
      Debug.WriteLine("End Compile");
#endif
      writer.Stream.Flush();
      int l = (int)writer.Stream.Length;
      l.GetType();
      this.bytes = stream.ToArray();
    }
    // 2^entrySelector[n] <= n
    static int[] entrySelectors = { 0, 0, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 };

    public int Position
    {
      get { return this.pos; }
      set { this.pos = value; }
    }
    int pos;

    //public int Seek(int position)
    //{
    //  this.pos = position;
    //  return this.pos;
    //}

    public int Seek(string tag)
    {
      if (this.tableDictionary.ContainsKey(tag))
      {
        this.pos = this.tableDictionary[tag].Offset;
        return this.pos;
      }
      return -1;
    }

    public int SeekOffset(int offset)
    {
      this.pos += offset;
      return this.pos;
    }

    /// <summary>
    /// Reads a System.Byte.
    /// </summary>
    public byte ReadByte()
    {
      return this.bytes[this.pos++];
    }

    /// <summary>
    /// Reads a System.Int16.
    /// </summary>
    public short ReadShort()
    {
      int pos = this.pos;
      this.pos += 2;
      return (short)((this.bytes[pos] << 8) | (this.bytes[pos + 1]));
    }

    /// <summary>
    /// Reads a System.UInt16.
    /// </summary>
    public ushort ReadUShort()
    {
      int pos = this.pos;
      this.pos += 2;
      return (ushort)((this.bytes[pos] << 8) | (this.bytes[pos + 1]));
    }

    /// <summary>
    /// Reads a System.Int32.
    /// </summary>
    public int ReadLong()
    {
      int pos = this.pos;
      this.pos += 4;
      return (int)((this.bytes[pos] << 24) | (this.bytes[pos + 1] << 16) | (this.bytes[pos + 2] << 8) | (this.bytes[pos + 3]));
    }

    /// <summary>
    /// Reads a System.UInt32.
    /// </summary>
    public uint ReadULong()
    {
      int pos = this.pos;
      this.pos += 4;
      return (uint)((this.bytes[pos] << 24) | (this.bytes[pos + 1] << 16) | (this.bytes[pos + 2] << 8) | (this.bytes[pos + 3]));
    }

    /// <summary>
    /// Reads a System.Int32.
    /// </summary>
    public Fixed ReadFixed()
    {
      int pos = this.pos;
      this.pos += 4;
      return (int)((this.bytes[pos] << 24) | (this.bytes[pos + 1] << 16) | (this.bytes[pos + 2] << 8) | (this.bytes[pos + 3]));
    }

    /// <summary>
    /// Reads a System.Int16.
    /// </summary>
    public short ReadFWord()
    {
      int pos = this.pos;
      this.pos += 2;
      return (short)((this.bytes[pos] << 8) | (this.bytes[pos + 1]));
    }

    /// <summary>
    /// Reads a System.UInt16.
    /// </summary>
    public ushort ReadUFWord()
    {
      int pos = this.pos;
      this.pos += 2;
      return (ushort)((this.bytes[pos] << 8) | (this.bytes[pos + 1]));
    }

    /// <summary>
    /// Reads a System.Int64.
    /// </summary>
    public long ReadLongDate()
    {
      int pos = this.pos;
      this.pos += 8;
      return (int)((this.bytes[pos] << 56) | (this.bytes[pos + 1] << 48) | (this.bytes[pos + 2] << 40) | (this.bytes[pos + 32]) |
                   (this.bytes[pos + 4] << 24) | (this.bytes[pos + 5] << 16) | (this.bytes[pos + 5] << 8) | (this.bytes[pos + 7]));
    }

    /// <summary>
    /// Reads a System.String with the specified size.
    /// </summary>
    public string ReadString(int size)
    {
      char[] chars = new char[size];
      for (int idx = 0; idx < size; idx++)
        chars[idx] = (char)this.bytes[this.pos++];
      return new string(chars);
    }

    /// <summary>
    /// Reads a System.Byte[] with the specified size.
    /// </summary>
    public byte[] ReadBytes(int size)
    {
      byte[] bytes = new byte[size];
      for (int idx = 0; idx < size; idx++)
        bytes[idx] = this.bytes[this.pos++];
      return bytes;
    }

    /// <summary>
    /// Reads the specified buffer.
    /// </summary>
    public void Read(byte[] buffer)
    {
      Read(buffer, 0, buffer.Length);
    }

    /// <summary>
    /// Reads the specified buffer.
    /// </summary>
    public void Read(byte[] buffer, int offset, int length)
    {
      Buffer.BlockCopy(this.bytes, this.pos, buffer, offset, length);
      this.pos += length;
    }

    /// <summary>
    /// Reads a System.Char[4] as System.String.
    /// </summary>
    public string ReadTag()
    {
      return ReadString(4);
    }

    /// <summary>
    /// Represents the font offset table.
    /// </summary>
    internal struct OffsetTable
    {
      /// <summary>
      /// 0x00010000 for version 1.0.
      /// </summary>
      public uint Version;

      /// <summary>
      /// Number of tables.
      /// </summary>
      public int TableCount;

      /// <summary>
      /// (Maximum power of 2 ≤ numTables) x 16.
      /// </summary>
      public ushort SearchRange;

      /// <summary>
      /// Log2(maximum power of 2 ≤ numTables).
      /// </summary>
      public ushort EntrySelector;

      /// <summary>
      /// NumTables x 16-searchRange.
      /// </summary>
      public ushort RangeShift;

      /// <summary>
      /// Writes the offset table.
      /// </summary>
      public void Write(TrueTypeFontWriter writer)
      {
        writer.WriteUInt(Version);
        writer.WriteShort(TableCount);
        writer.WriteUShort(SearchRange);
        writer.WriteUShort(EntrySelector);
        writer.WriteUShort(RangeShift);
      }
    }
  }
}
