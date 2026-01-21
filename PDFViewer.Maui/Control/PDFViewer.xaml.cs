using ZPF.PDFViewer.DataSources;

namespace ZPF.PDFViewer.Maui;

public partial class PDFViewer : ContentView
{
   public PDFViewer()
   {
      BindingContext = this;

      InitializeComponent();

      PDFPageInfo.OnNeedData += PDFPageInfo_OnNeedData;

      // - - -  - - - 

      // Retrieve DataTemplate from StaticResource
      if (Resources.TryGetValue("PDFTemplate", out var templateObj) && templateObj is DataTemplate PDFTemplate)
      {
         // Use the DataTemplate in code
         collectionView.ItemTemplate = new DataTemplate(() =>
         {
            var view = (View)PDFTemplate.CreateContent();

            return WrapView(view);
         });
      }
      else
      {
         Console.WriteLine("DataTemplate 'MyItemTemplate' not found.");
      }
   }

   private PDFInfos _PDFInfos = new PDFInfos();


   /// <summary>
   /// Load PDF from URL wo rendering it.
   /// </summary>
   /// <param name="pdfSource"></param>
   /// <param name="url"></param>
   /// <returns></returns>
   public async Task<bool> LoadPDF(IPdfSource pdfSource, string url)
   {
      bool Result = false;

      IsBusy = true;

      UnloadPDF();

      // - - - Load PDF File - - -

      // delete previously created temp files
      PdfTempFileHelper.DeleteTempFiles();

      _PDFInfos = new PDFInfos();
      _PDFInfos.FileName = await pdfSource.LoadPDF(url);

      if (System.IO.File.Exists(_PDFInfos.FileName))
      {
         _PDFInfos = await NewPDFInfos(_PDFInfos.FileName, url);

         Result = true;
      }
      else
      {
         _PDFInfos = new PDFInfos();
      }

      GeneratePages(_PDFInfos.PageCount);

      collectionView.ItemsSource = null;
      collectionView.ItemsSource = Pages;

      // - - -  - - -

      IsBusy = false;

      return Result;
   }


   public PDFInfos GetInfos()
   {
      return _PDFInfos;
   }


   public async System.Threading.Tasks.Task SaveFirstPageAsImageAsync(string outputImagePath)
   {
      await SavePageAsImageAsync(outputImagePath, 0);
   }


   public void ClearPages()
   {
      Pages.Clear();
      GC.Collect();
      OnPropertyChanged("Pages");

      collectionView.ItemsSource = null;
      collectionView.ItemsSource = Pages;
   }


   public async void GeneratePages(int numberOfPages)
   {
      Pages.Clear();
      GC.Collect();

      var p = new PDFPageInfo() { PageNumber = 1 };

      //var tnFileName = PdfTempFileHelper.CreateTempPageFilePath("Cover.jpeg");
      p.SetValues(await UpdatePageInfo(p, ""));

      var scale = Math.Min(
         collectionView.Width / p.WidthRequest,
         (collectionView.Height - 30) / p.HeightRequest);

      //collectionView.Scale = scale;

      //Pages.Add(p);

      for (var i = 1; i <= numberOfPages; i++)
      {
         Pages.Add(new PDFPageInfo() { PageNumber = i, Scale = scale });
      }

      OnPropertyChanged("Pages");
   }


   private async void PDFPageInfo_OnNeedData(object sender)
   {
      if (sender is not PDFPageInfo p)
         return;

      string tnFileName = "";

      if (p.PageNumber == 1)
      {
         tnFileName = PdfTempFileHelper.CreateTempPageFilePath("_Thumbnail_.jpeg");
      }
      else
      {
         tnFileName = PdfTempFileHelper.CreateTempPageFilePath($"TNPage({p.PageNumber}).jpeg");
      }

      p.SetValues(await UpdatePageInfo(p, tnFileName));
   }

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -

   //public event EventHandler<SelectedItemChangedEventArgs> ClickOnPage;
   public event EventHandler<SelectedItemChangedEventArgs> DoubleClickOnPage;
   //public event EventHandler<SelectedItemChangedEventArgs> RightClickOnPage;

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
            //collectionView.SelectedItem = item;

            //if (item is FileItem)
            //{
            //   SelectItem((FileItem)item);
            //}
         }
      };

      var doubleTap = new TapGestureRecognizer { NumberOfTapsRequired = 2 };
      doubleTap.Tapped += (s, e) =>
      {
         var item = ((BindableObject)s).BindingContext;

         if (DoubleClickOnPage != null)
         {
            DoubleClickOnPage(this, new SelectedItemChangedEventArgs(item, -1));
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


}
