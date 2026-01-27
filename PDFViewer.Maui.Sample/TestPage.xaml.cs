using System.Collections.ObjectModel;

namespace PDFViewer.Maui.Sample;

public partial class TestPage : ContentPage
{
   public TestPage()
   {
      InitializeComponent();

      {
         var i = ImageSource.FromFile("image.jpeg");

         Pages.Add(new ZPF.PDFViewer.Page
         {
            ImageSource = i,
            RealWidth = 1588,
            RealHeight = 1598,
         });
      }

      {
         var i = ImageSource.FromFile("image2.jpeg");

         Pages.Add(new ZPF.PDFViewer.Page
         {
            ImageSource = i,
            RealWidth = 1586,
            RealHeight = 2244,
         });
      }

      listView.ItemsSource = Pages;
      collectionView.ItemsSource = Pages;

      DoubleClickOnPage += TestPage_DoubleClickOnPage;
   }

   double zoom = 1.0;

   private void TestPage_DoubleClickOnPage(object? sender, SelectedItemChangedEventArgs e)
   {
      if (zoom == 1.0)
      {
         zoom = 2.0;
      }
      else
      {
         zoom = 1.0;
      }

      foreach (var p in Pages)
      {
         var scale = (scrollView.Width - 60) / p.RealWidth;
         p.Scale = scale * zoom;
      }
   }

   ObservableCollection<ZPF.PDFViewer.Page> Pages = new ObservableCollection<ZPF.PDFViewer.Page>();


   public event EventHandler<SelectedItemChangedEventArgs> ClickOnPage;
   public event EventHandler<SelectedItemChangedEventArgs> DoubleClickOnPage;


   protected override void OnAppearing()
   {
      base.OnAppearing();

      // Retrieve DataTemplate from StaticResource
      if (Resources.TryGetValue("PDFTemplate", out var templateObj) && templateObj is DataTemplate PDFTemplate)
      {
         foreach (var p in Pages)
         {
            View view = (View)PDFTemplate.CreateContent();
            view.BindingContext = p;
            stack.Children.Add(view);

            var singleTap = new TapGestureRecognizer { NumberOfTapsRequired = 1 };
            singleTap.Tapped += (s, e) =>
            {
               var item = ((BindableObject)s).BindingContext;

               if (item != null)
               {
                  if (ClickOnPage != null)
                  {
                     ClickOnPage(view, new SelectedItemChangedEventArgs(item, -1));
                  }
               }
            };

            var doubleTap = new TapGestureRecognizer { NumberOfTapsRequired = 2 };
            doubleTap.Tapped += (s, e) =>
            {
               var item = ((BindableObject)s).BindingContext;

               if (item != null)
               {
                  if (DoubleClickOnPage != null)
                  {
                     DoubleClickOnPage(view, new SelectedItemChangedEventArgs(item, -1));
                  }
               }
            };

            view.GestureRecognizers.Add(singleTap);
            view.GestureRecognizers.Add(doubleTap);
         }
      }
   }

   override protected void OnSizeAllocated(double width, double height)
   {
      base.OnSizeAllocated(width, height);
      foreach (var p in Pages)
      {
         p.Scale = (width - 60) / p.RealWidth;
      }
   }

}
