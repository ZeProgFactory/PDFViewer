using ZPF.PDFViewer.DataSources;

namespace ZPF.PDFViewer.Maui;

public partial class PDFViewer : ContentView
{
   public PDFViewer()
   {
      BindingContext = this;

      InitializeComponent();

      zoomStack.IsVisible = false;
      navStack.IsVisible = false;

      lbZoom.Text = "";

      PDFPageInfo.OnNeedData += PDFPageInfo_OnNeedData;
   }

   private async void PDFPageInfo_OnNeedData(object sender)
   {
      if (sender is not PDFPageInfo p)
         return;

      p.SetValues(await UpdatePageInfo(p, ""));
   }

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -

   public event EventHandler<SelectedItemChangedEventArgs> ClickOnPage;
   public event EventHandler<SelectedItemChangedEventArgs> DoubleClickOnPage;
   //public event EventHandler<SelectedItemChangedEventArgs> RightClickOnPage;

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -

   private void scrollView_Scrolled(object sender, ScrolledEventArgs e)
   {
      double scrollY = e.ScrollY;

      // Use visual children API to avoid obsolete ScrollView.Children
      var visualChildren = (scrollView.Content as IVisualTreeElement)?.GetVisualChildren();
      if (visualChildren == null)
         return;

      for (int i = 0; i < visualChildren.Count; i++)
      {
         if (visualChildren[i] is VisualElement child)
         {
            // Get the absolute position of the child
            var location = child.GetBoundingBox();

            // If the bottom of the item is below the scroll offset,
            // it means this is the first visible item.
            if (location.Bottom >= scrollY)
            {
               CurrentPageNumber = i+1;
               break;
            }
         }
      }
   }

   private async void btnFirst_Clicked(object sender, EventArgs e)
   {
      await scrollView.ScrollToAsync(0, 0, true);
   }

   private async void btnLast_Clicked(object sender, EventArgs e)
   {
      await scrollView.ScrollToAsync(0, scrollView.ContentSize.Height, true);
   }

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -
}
