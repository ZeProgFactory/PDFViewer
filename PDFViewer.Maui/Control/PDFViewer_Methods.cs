using ZPF.PDFViewer.DataSources;

namespace ZPF.PDFViewer.Maui;

public partial class PDFViewer
{
   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 

   public async Task<bool> LoadPDF(string filename, bool genPages = true)
   {
      return await LoadPDF(filename, "", genPages);
   }

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 

   public async Task<bool> LoadPDF(string filename, string password, bool genPages = true)
   {
      return await LoadPDF(new FilePdfSource(), filename, password, genPages);
   }

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 

   /// <summary>
   /// Load PDF from URL wo rendering it.
   /// The rendering is done when pages (ImageFileName) are requested.
   /// </summary>
   /// <param name="pdfSource"></param>
   /// <param name="url"></param>
   /// <param name="password"></param>
   /// <returns></returns>
   public async Task<bool> LoadPDF(IPdfSource pdfSource, string url, string password = "", bool genPages = true)
   {
      bool Result = false;

      IsBusy = true;
      LastMessage = "";

      UnloadPDF();

      // - - - Load PDF File - - -

      // delete previously created temp files
      PdfTempFileHelper.DeleteTempFiles();

      _PDFInfos = new PDFInfos();
      _PDFInfos.FileName = await pdfSource.LoadPDF(url);

      if (!string.IsNullOrEmpty(pdfSource.LastMessage))
      {
         _PDFInfos = new PDFInfos();
         IsBusy = false;

         zoomStack.IsVisible = false;
         navStack.IsVisible = false;

         LastMessage = pdfSource.LastMessage;
         return false;
      }

      if (System.IO.File.Exists(_PDFInfos.FileName))
      {
         _PDFInfos = await NewPDFInfos(_PDFInfos.FileName, url, password);

         if (string.IsNullOrEmpty(_PDFInfos.FileName))
         {
            IsBusy = false;
            return false;
         }

         Result = true;
      }
      else
      {
         _PDFInfos = new PDFInfos();
         IsBusy = false;

         zoomStack.IsVisible = false;
         navStack.IsVisible = false;

         LastMessage = (string.IsNullOrEmpty(LastMessage) ? "File not found" : LastMessage);
         return false;
      }

      // generating pages wo rendering them
      if (genPages)
      {
         GeneratePages(_PDFInfos.PageCount);

         await DoZoom(_Scale);
      }

      // - - -  - - -

      IsBusy = false;

      return Result;
   }

   async void GeneratePages(int numberOfPages)
   {
      Pages.Clear();
      GC.Collect();

      for (var i = 0; i < numberOfPages; i++)
      {
         var p = new PDFPageInfo() { PageNumber = i + 1 };
         p.DisplayRotation = PageRotation;
         Pages.Add(p);

         // Retrieve DataTemplate from StaticResource
         if (Resources.TryGetValue("PDFTemplate", out var templateObj) && templateObj is DataTemplate PDFTemplate)
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

      CurrentPageNumber = 1;

      OnPropertyChanged("Pages");
   }

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 

   /// <summary>
   /// Asynchronously saves the first page of the document as an image file at the specified path.
   /// </summary>
   /// <remarks>Use this method to capture and export the first page of the document as an image. Ensure that
   /// the output path has the appropriate permissions and file extension for the desired image format.</remarks>
   /// <param name="outputImagePath">The file path where the image of the first page will be saved. The path must be valid and writable, and should
   /// include a supported image file extension.</param>
   /// <returns>A task that represents the asynchronous save operation.</returns>
   public async System.Threading.Tasks.Task SaveFirstPageAsImageAsync(string outputImagePath)
   {
      await SavePageAsImageAsync(outputImagePath, 0);
   }

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 

   /// <summary>
   /// Removes all pages from the collection and updates the associated view to reflect the cleared state.
   /// </summary>
   /// <remarks>After calling this method, the collection view is reset to an empty state and then repopulated
   /// with the current set of pages. Any data bindings or UI elements dependent on the pages collection will be
   /// refreshed accordingly.</remarks>
   public void ClearPages()
   {
      stack.Children.Clear();
      Pages.Clear();
      GC.Collect();
      OnPropertyChanged("Pages");
      CurrentPageNumber = 0;
   }

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -

   /// <summary>
   /// Scrolls to first page
   /// </summary>
   /// <returns></returns>
   public async Task ScrollToFirstAsync()
   {
      await scrollView.ScrollToAsync(0, 0, true);
   }

   /// <summary>
   /// Scrolls to 'pageNumber'
   /// </summary>
   /// <returns></returns>
   public async Task ScrollToAsync(uint pageNumber)
   {
      if (pageNumber == 0)
      {
         await ScrollToFirstAsync();
         return;
      }

      int index = (int)pageNumber - 1;

      if (index >= stack.Children.Count)
      {
         await ScrollToLastAsync();
         return;
      }

      View view = (View)stack.Children[index];

      // The page views are added and measured asynchronously. When this is called right after
      // LoadPDF (e.g. restoring the last read page at startup) the target page is not laid out yet:
      // its Y is still 0 and ScrollToAsync would simply scroll back to the top.
      await WaitForLayoutAsync(view);

      await scrollView.ScrollToAsync(view, ScrollToPosition.Start, true);
   }

   /// <summary>
   /// Waits until <paramref name="view"/> has been measured/arranged, or until the timeout elapses.
   /// </summary>
   private static async Task WaitForLayoutAsync(View view, int timeoutMilliseconds = 5000)
   {
      var startedAt = Environment.TickCount64;

      while (Environment.TickCount64 - startedAt < timeoutMilliseconds)
      {
         if (view.Height > 0 && view.Y > 0)
         {
            // give the layout pass a chance to settle before scrolling
            await Task.Delay(50);
            return;
         }

         await Task.Delay(50);
      }
   }

   /// <summary>
   /// Scrolls to last page
   /// </summary>
   /// <returns></returns>
   public async Task ScrollToLastAsync()
   {
      await scrollView.ScrollToAsync(0, scrollView.ContentSize.Height, true);
   }

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 
}
