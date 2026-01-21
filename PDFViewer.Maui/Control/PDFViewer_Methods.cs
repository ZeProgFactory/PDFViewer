using System.Diagnostics;
using ZPF.PDFViewer.DataSources;

namespace ZPF.PDFViewer.Maui;

public partial class PDFViewer
{
   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 

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

      //Pages.Add(p);

      for (var i = 1; i <= numberOfPages; i++)
      {
         Pages.Add(new PDFPageInfo() { PageNumber = i, Scale = scale });
      }

      OnPropertyChanged("Pages");
   }

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 

   public async System.Threading.Tasks.Task SaveFirstPageAsImageAsync(string outputImagePath)
   {
      await SavePageAsImageAsync(outputImagePath, 0);
   }

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 

   public void ClearPages()
   {
      Pages.Clear();
      GC.Collect();
      OnPropertyChanged("Pages");

      collectionView.ItemsSource = null;
      collectionView.ItemsSource = Pages;
   }

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 
}
