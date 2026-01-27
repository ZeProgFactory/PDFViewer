using System.Diagnostics;

namespace ZPF.PDFViewer.Maui;

public partial class PDFViewer
{
   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 

   List<double> _Zooms = new List<double>() { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9, 2.0 };
   //List<double> _Zooms = new List<double>() { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 };

   public double CalculatedZoom { get; set; } = -1;


   public double ZoomFactor { get => _ZoomFactor; internal set => _ZoomFactor = value; }
   double _ZoomFactor = 1.0;


   int RealWidth = 0;
   int RealHeight = 0;

   public void CalcZoom(PDFPageInfo pageInfo)
   {
      //Debug.WriteLine($" {svImage.Width}  {imagePage.Width}  {dropImage.Width}");

      //RealWidth = (int)PDFHelper.ToPT(pageInfo.Width) * 3;
      //RealHeight = (int)PDFHelper.ToPT(pageInfo.Height) * 3;

      //MainThread.BeginInvokeOnMainThread(() =>
      //{
      //   // Code to run on the main thread
      //   pageInfo.WidthRequest = RealWidth + 20;
      //   pageInfo.HeightRequest = RealHeight + 20;
      //});

      //CalculatedZoom = (decimal)((int)(svImage.Width / RealWidth * 10) / 10.0);

      //if (CalculatedZoom < 0.1m)
      //{
      //   CalculatedZoom = 0.1m;
      //};

      //if (CalculatedZoom > 1.0m)
      //{
      //   CalculatedZoom = 1.0m;
      //};

      //ZoomFactor = CalculatedZoom;
   }

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 

   public static int FirstTimeZoomDelay = 100;
   bool First_DoZoom = true;

   double _Scale = 1.0;


   public async Task DoZoom(double zoom)
   {
      zoomStack.IsVisible = true;

      _ZoomFactor = zoom;
      lbZoom.Text = $"{(int)(_ZoomFactor * 100)} %";

      foreach (var p in Pages)
      {
         var scale = (scrollView.Width - 60) / p.RealWidth;
         p.Scale = scale * zoom;
      }

      //try
      //{
      //   ZoomFactor = scale;
      //   lbZoom.Text = $"{(int)(100.0m * scale)} %";

      //   absoluteLayout.Children.Clear();

      //   imagePage.Scale = (double)scale;
      //   dropImage.Source = PDFViewModel.Current.ImageFileName;

      //   var sX = (1 - imagePage.Scale) * RealWidth / 2;
      //   var sY = (1 - imagePage.Scale) * RealHeight / 2;

      //   int delay = (First_DoZoom ? FirstTimeZoomDelay : 100);

      //   // workaround to scroll the first page into viewport ...
      //   await DoIt.Delay(delay, () =>
      //   {
      //      MainThread.InvokeOnMainThreadAsync(async () =>
      //      {
      //         // do ui stuff
      //         await svImage.ScrollToAsync(sX, sY, true);
      //      });
      //   });
      //}
      //catch { }

      //First_DoZoom = false;
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
