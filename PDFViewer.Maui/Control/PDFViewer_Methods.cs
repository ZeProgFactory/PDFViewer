using System.Diagnostics;
using ZPF.PDFViewer.DataSources;

namespace ZPF.PDFViewer.Maui;

public partial class PDFViewer
{
   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 

   /// <summary>
   /// Load PDF from URL wo rendering it.
   /// The rendering is done when pages (ImageFileName) are requested.
   /// </summary>
   /// <param name="pdfSource"></param>
   /// <param name="url"></param>
   /// <returns></returns>
   public async Task<bool> LoadPDF(IPdfSource pdfSource, string url, string password = "")
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
      }

      // generating pages wo rendering them
      GeneratePages(_PDFInfos.PageCount);

      collectionView.ItemsSource = null;
      collectionView.ItemsSource = Pages;

      // - - -  - - -

      IsBusy = false;

      return Result;
   }

   async void GeneratePages(int numberOfPages)
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

      CurrentPageNumber = 1;

      //Pages.Add(p);

      for (var i = 1; i <= numberOfPages; i++)
      {
         Pages.Add(new PDFPageInfo() { PageNumber = i, Scale = scale });
      }

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
      Pages.Clear();
      GC.Collect();
      OnPropertyChanged("Pages");
      CurrentPageNumber = 0;

      collectionView.ItemsSource = null;
      collectionView.ItemsSource = Pages;
   }

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 
}
