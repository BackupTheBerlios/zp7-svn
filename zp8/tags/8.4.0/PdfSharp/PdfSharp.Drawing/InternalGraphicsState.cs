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

using System;
#if Gdip
using System.Drawing;
using System.Drawing.Drawing2D;
#endif
#if Wpf
using System.Windows.Media;
#endif

namespace PdfSharp.Drawing
{
  // In GDI+ the functions Save/Restore, BeginContainer/EndContainer, Transform, SetClip and ResetClip
  // can be combined in any order. E.g. you can set a clip region, save the graphics state, empty the
  // clip region and draw without clipping. Then you can restore to the previous clip region. With PDF
  // this behaviour is hard to implement. To solve this problem I first an automaton that keeps track
  // of all clipping paths and the current transformation when the clip path was set. The automation
  // manages a PDF graphics state stack to calculate the desired bahaviour. It also takes into consideration
  // not to multiply with inverse matrixes when the user sets a new transformation matrix.
  // After the design works on pager I decided not to implement it because it is much to large-scale.
  // Instead I lay down some rules how to use the XGraphics class.
  //
  // * Before you set a transformation matrix save the graphics state (Save) or begin a new container
  //   (BeginContainer).
  // 
  // * Instead of resetting the transformation matrix, call Restore or EndContainer. If you reset the
  //   transformation, in PDF must be multiplied with the inverse matrix. That leads to round off errors
  //   because in PDF file only 3 digits are used and Acrobat internally uses fixed point numbers (until
  //   versioin 6 or 7 I think).
  //
  // * When no clip path is defined, you can set or intersect a new path.
  //
  // * When a clip path is already defined, you can always intersect with a new one (wich leads in general
  //   to a smaller clip region).
  //
  // * When a clip path is already defined, you can only reset it to the empty region (ResetClip) when
  //   the graphics state stack is at the same position as it had when the clip path was defined. Otherwise
  //   an error occurs.
  //
  // Keeping these rules leads to easy to read code and best results in PDF output.

  /// <summary>
  /// Represents the internal state of an XGraphics object.
  /// </summary>
  internal class InternalGraphicsState
  {
    public InternalGraphicsState(XGraphicsState state)
    {
      GdipGraphicsState = state.GdipState;
      state.InternalState = this;
    }

    public InternalGraphicsState(XGraphicsContainer container)
    {
      //GdipGraphicsContainer = container.GdipContainer;
      GdipGraphicsState = container.GdipState;
      container.InternalState = this;
    }

    /// <summary>
    /// Gets or sets the current transformation matrix.
    /// </summary>
    public XMatrix Transform
    {
      get { return this.transform; }
      set { this.transform = value; }
    }
    XMatrix transform = XMatrix.Identity;

#if Gdip
    /// <summary>
    /// The GDI+ GraphicsState if contructed from XGraphicsState.
    /// </summary>
    public GraphicsState GdipGraphicsState;
#endif

    // /// <summary>
    // /// The GDI+ GraphicsContainer if contructed from XGraphicsContainer.
    // /// </summary>
    // public GraphicsContainer GdipGraphicsContainer;

    internal bool invalid;
  }
}