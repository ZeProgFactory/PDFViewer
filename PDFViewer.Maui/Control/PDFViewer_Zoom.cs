using System.Diagnostics;

namespace ZPF.PDFViewer.Maui;

public partial class PDFViewer
{
   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 

   //ToDo: List<decimal> _Zooms = new List<decimal>() { 0.1m, 0.2m, 0.3m, 0.4m, 0.5m, 0.6m, 0.7m, 0.8m, 0.9m, 1.0m, 1.1m, 1.2m, 1.3m, 1.4m, 1.5m, 1.6m, 1.7m, 1.8m, 1.9m, 2.0m };
   List<decimal> _Zooms = new List<decimal>() { 0.1m, 0.2m, 0.3m, 0.4m, 0.5m, 0.6m, 0.7m, 0.8m, 0.9m, 1.0m };

   public static decimal ZoomFactor { get => _ZoomFactor; set => _ZoomFactor = value; }
   public decimal CalculatedZoom { get; set; } = -1;

   static decimal _ZoomFactor = 0.5m;

   int RealWidth = 0;
   int RealHeight = 0;

   public void CalcZoom( PDFPageInfo pageInfo )
   {
      //Debug.WriteLine($" {svImage.Width}  {imagePage.Width}  {dropImage.Width}");

      RealWidth = (int)PDFHelper.ToPT(pageInfo.Width) * 3;
      RealHeight = (int)PDFHelper.ToPT(pageInfo.Height) * 3;

      MainThread.BeginInvokeOnMainThread(() =>
      {
         // Code to run on the main thread
         pageInfo.WidthRequest = RealWidth + 20;
         pageInfo.HeightRequest = RealHeight + 20;
      });

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

   public async Task DoZoom(decimal scale)
   {
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
