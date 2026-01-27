using ZPF.PDFViewer.DataSources;

namespace ZPF.PDFViewer.Maui;

public partial class PDFViewer : ContentView
{
   public PDFViewer()
   {
      BindingContext = this;

      InitializeComponent();

      zoomStack.IsVisible = false;
      lbZoom.Text = "";

      PDFPageInfo.OnNeedData += PDFPageInfo_OnNeedData;
   }

   private async void PDFPageInfo_OnNeedData(object sender)
   {
      if (sender is not PDFPageInfo p)
         return;

      p.SetValues(await UpdatePageInfo(p,""));
   }

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -

   public event EventHandler<SelectedItemChangedEventArgs> ClickOnPage;
   public event EventHandler<SelectedItemChangedEventArgs> DoubleClickOnPage;
   //public event EventHandler<SelectedItemChangedEventArgs> RightClickOnPage;

   private ViewCell WrapViewCell(ViewCell view)
   {
      // Add gestures to the view
      var singleTap = new TapGestureRecognizer { NumberOfTapsRequired = 1 };
      singleTap.Tapped += (s, e) =>
      {
         var item = ((BindableObject)s).BindingContext;

         if (item != null)
         {
            if (ClickOnPage != null)
            {
               ClickOnPage(view.View, new SelectedItemChangedEventArgs(item, -1));
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
               DoubleClickOnPage(view.View, new SelectedItemChangedEventArgs(item, -1));
            }
         }
      };

      view.View.GestureRecognizers.Add(singleTap);
      view.View.GestureRecognizers.Add(doubleTap);


#if WINDOWS || MACCATALYST

      var rightClickRecognizer = new TapGestureRecognizer
      {
         Buttons = ButtonsMask.Secondary
      };

      rightClickRecognizer.Tapped += (s, e) =>
      {
         var item = ((BindableObject)s).BindingContext;

         if (item is PDFPageInfo)
         {
            //   // ContextMenu((FileItem)item);
         }
      };

      view.View.GestureRecognizers.Add(rightClickRecognizer);
#endif

#if ANDROID || IOS || WINDOWS || MACCATALYST
      // CommunityToolkit.Maui.Behaviors;
      //var touchBehavior = new TouchBehavior
      //{
      //   LongPressCommand = new Command(async () =>
      //   {
      //      // Simulate a context menu with a popup
      //      var item = wrapper.Content.BindingContext;
      //      // ContextMenu((PDFPageInfo)item);
      //   })
      //};

      //wrapper.Content.Behaviors.Add(touchBehavior);
#endif

      return view;
   }

   private ContentView WrapView(View view)
   {
      // --- Wrap in a container you control ---
      var wrapper = new ContentView
      {
         Content = view,
         //BackgroundColor = Colors.Transparent,
      };

      // Add gestures to the wrapper
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

      wrapper.Content.GestureRecognizers.Add(singleTap);
      wrapper.Content.GestureRecognizers.Add(doubleTap);


#if WINDOWS || MACCATALYST

      var rightClickRecognizer = new TapGestureRecognizer
      {
         Buttons = ButtonsMask.Secondary
      };

      rightClickRecognizer.Tapped += (s, e) =>
      {
         var item = ((BindableObject)s).BindingContext;

         if (item is PDFPageInfo)
         {
            //   // ContextMenu((FileItem)item);
         }
      };

      wrapper.Content.GestureRecognizers.Add(rightClickRecognizer);
#endif

#if ANDROID || IOS || WINDOWS || MACCATALYST
      // CommunityToolkit.Maui.Behaviors;
      //var touchBehavior = new TouchBehavior
      //{
      //   LongPressCommand = new Command(async () =>
      //   {
      //      // Simulate a context menu with a popup
      //      var item = wrapper.Content.BindingContext;
      //      // ContextMenu((PDFPageInfo)item);
      //   })
      //};

      //wrapper.Content.Behaviors.Add(touchBehavior);
#endif

      return wrapper;
   }

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -

   private void collectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
   {
      if (e.CurrentSelection.Count > 0)
      {
         var selectedItem = e.CurrentSelection[0];

         CurrentPageNumber = (selectedItem as PDFPageInfo).PageNumber;
         //int index = Items.IndexOf(selectedItem as string);

         //DisplayAlert("Item Selected",
         //    $"Item: {selectedItem}\nIndex: {index}",
         //    "OK");
      }
   }

   private void collectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
   {
      // e.FirstVisibleItemIndex gives the first visible item index
      int firstIndex = Math.Max(e.FirstVisibleItemIndex + 1, 1);
      int lastIndex = e.LastVisibleItemIndex + 1;

      if (CurrentPageNumber < firstIndex || CurrentPageNumber > lastIndex)
      {
         CurrentPageNumber = firstIndex;
      }
   }

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -

   private void collectionView_SizeChanged(object sender, EventArgs e)
   {
      DoZoom(_Scale);
   }

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -
}
