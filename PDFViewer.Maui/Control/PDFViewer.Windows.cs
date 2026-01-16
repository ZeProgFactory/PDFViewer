#if WINDOWS

using System.Diagnostics;
using Windows.Data.Pdf;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using ZPF.PDFViewer.DataSources;

namespace ZPF.PDFViewer.Maui;

// Windows-specific implementation 
partial class PDFViewer
{
   PdfDocument _PdfDocument = null;

   public async Task LoadPDF(string pdfPath)
   {
      UnloadPDF();

      // Open the PDF file
      StorageFile pdfFile = await StorageFile.GetFileFromPathAsync(pdfPath);

      using (IRandomAccessStream pdfStream = await pdfFile.OpenAsync(FileAccessMode.Read))
      {
         // Load the PDF document
         _PdfDocument = await PdfDocument.LoadFromStreamAsync(pdfStream);
      }
   }


   public void UnloadPDF()
   {
      if (_PdfDocument != null)
      {
         _PdfDocument = null;
         _PDFInfos = new PDFInfos();
      }

      ClearPages();
   }


   private async Task<PDFInfos> NewPDFInfos(string pdfPath, string url)
   {
      if (_PdfDocument == null)
      {
         await LoadPDF(pdfPath);
      }

      if (_PdfDocument != null)
      {
         _PDFInfos = new PDFInfos()
         {
            PageCount = (int)_PdfDocument.PageCount,
            IsPasswordProtected = _PdfDocument.IsPasswordProtected,
            FileName = url,
            FileSizeInBytes = new System.IO.FileInfo(pdfPath).Length,
         };
      }
      else
      {
         _PDFInfos = new PDFInfos();
      }

      return _PDFInfos;
   }


   public async System.Threading.Tasks.Task<object> SavePageAsImageAsync(string outputImagePath, uint pageNumber = 0)
   {
      if (_PdfDocument == null)
      {
         return;
      }

      Debug.WriteLine($"In {pageNumber} {outputImagePath} \n" );

      using (PdfPage page = _PdfDocument.GetPage(pageNumber))
      {
         // Render page to an in-memory stream
         InMemoryRandomAccessStream imageStream = new InMemoryRandomAccessStream();
         await page.RenderToStreamAsync(imageStream);

         // Create a BitmapDecoder from the rendered stream
         imageStream.Seek(0);
         BitmapDecoder decoder = await BitmapDecoder.CreateAsync(imageStream);

         try
         {
            // Create output file
            // StorageFile outputFile = await StorageFile.GetFileFromPathAsync(outputImagePath);
            // If you want to create/overwrite instead:
            StorageFolder outputFolder = await StorageFolder.GetFolderFromPathAsync(System.IO.Path.GetDirectoryName(outputImagePath));
            StorageFile outputFile = await outputFolder.CreateFileAsync(
               System.IO.Path.GetFileName(outputImagePath),
               CreationCollisionOption.ReplaceExisting);

            using (IRandomAccessStream fileStream =
                   await outputFile.OpenAsync(FileAccessMode.ReadWrite))
            {
               // Encode as PNG (you can choose JPEG or others)
               BitmapEncoder encoder = await BitmapEncoder.CreateAsync(
                   BitmapEncoder.PngEncoderId,
                   fileStream);

               // Copy pixel data from decoder to encoder
               PixelDataProvider pixelData = await decoder.GetPixelDataAsync();
               byte[] pixels = pixelData.DetachPixelData();

               encoder.SetPixelData(
                   decoder.BitmapPixelFormat,
                   decoder.BitmapAlphaMode,
                   decoder.PixelWidth,
                   decoder.PixelHeight,
                   decoder.DpiX,
                   decoder.DpiY,
                   pixels);

               await encoder.FlushAsync();
            }

            return page;
         }
         catch (Exception ex)
         {
            Debug.WriteLine( $"{pageNumber} {outputImagePath} \n" + ex.ToString());
         }
      }

      Debug.WriteLine($"Out {pageNumber} {outputImagePath} \n"  );
   }


   public void RenderPages()
   {
      if (_PdfDocument == null)
      {
         return;
      }

      if (Handler is IPlatformViewHandler platformHandler)
      {
         Debugger.Break();
         // (platformHandler as Maui.PDFView.PdfViewHandler)?.RenderPages(_PdfDocument);
      }
   }
}

#endif
