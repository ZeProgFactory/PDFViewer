using System;
using System.Collections.Generic;
using System.Text;

namespace ZPF.PDFViewer.Maui;

public static class ViewExtensions
{
   public static Rect GetBoundingBox(this VisualElement view)
   {
      var x = view.X;
      var y = view.Y;
      var width = view.Width;
      var height = view.Height;

      return new Rect(x, y, width, height);
   }
}
