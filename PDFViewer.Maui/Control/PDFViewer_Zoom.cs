using System.Diagnostics;

namespace ZPF.PDFViewer.Maui;

public partial class PDFViewer
{
   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 

   List<double> _Zooms = new List<double>() { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9, 2.0 };
   //List<double> _Zooms = new List<double>() { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 };

   public double ZoomFactor { get => _ZoomFactor; internal set => _ZoomFactor = value; }
   double _ZoomFactor = 1.0;

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 

   double _Scale = 1.0;


   public async Task DoZoom(double zoom)
   {
      zoomStack.IsVisible = true;
      navStack.IsVisible = true;

      _ZoomFactor = zoom;
      lbZoom.Text = $"{(int)(_ZoomFactor * 100)} %";

      foreach (var p in Pages)
      {
         var scale = (scrollView.Width - 60) / p.RealWidth;
         p.Scale = scale * zoom;
      }
   }

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 

   private async void btnZoomIn_Clicked(object sender, EventArgs e)
   {
      (sender as Button).IsEnabled = false;

      var ind = _Zooms.IndexOf(ZoomFactor);

      if (ind >= 0 && ind - 1 >= 0)
      {
         await DoZoom(_Zooms[ind - 1]);
      }

      (sender as Button).IsEnabled = true;
   }

   private async void btnZoomOut_Clicked(object sender, EventArgs e)
   {
      (sender as Button).IsEnabled = false;

      var ind = _Zooms.IndexOf(ZoomFactor);

      if (ind >= 0 && ind + 1 < _Zooms.Count)
      {
         await DoZoom(_Zooms[ind + 1]);
      }

      (sender as Button).IsEnabled = true;
   }

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 
}
