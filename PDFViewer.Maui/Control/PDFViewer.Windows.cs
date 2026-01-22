#if WINDOWS

using System.Diagnostics;
using Microsoft.UI.Xaml.Controls;
using Windows.Data.Pdf;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using ZPF.PDFViewer.DataSources;
using static ZPF.PDFViewer.PDFHelper;

namespace ZPF.PDFViewer.Maui;

// Windows-specific implementation 
partial class PDFViewer
{
   PdfDocument _PdfDocument = null;

   public async Task LoadPDF(string pdfPath, string password = "")
   {
      // Open the PDF file
      if (!System.IO.File.Exists(pdfPath))
      {
         Debugger.Break();
      }

      StorageFile pdfFile = await StorageFile.GetFileFromPathAsync(pdfPath);

      using (IRandomAccessStream pdfStream = await pdfFile.OpenAsync(FileAccessMode.Read))
      {
         try
         {
            // Load the PDF document

            if (string.IsNullOrEmpty(password))
            {
               _PdfDocument = await PdfDocument.LoadFromStreamAsync(pdfStream);
            }
            else
            {
               _PdfDocument = await PdfDocument.LoadFromStreamAsync(pdfStream, password);
            }
         }
         catch (Exception ex)
         {
            _PdfDocument = null;
            LastMessage = ex.Message.ToString();
         }
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


   private async Task<PDFInfos> NewPDFInfos(string pdfPath, string url, string password = "")
   {
      if (_PdfDocument == null)
      {
         await LoadPDF(pdfPath, password);
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


   public async System.Threading.Tasks.Task SavePageAsImageAsync(string outputImagePath, uint pageNumber = 0)
   {
      if (_PdfDocument == null)
      {
         return;
      }

      Debug.WriteLine($"In {pageNumber} {outputImagePath} \n");

      using (PdfPage page = _PdfDocument.GetPage(pageNumber))
      {
         await RenderPage(page, outputImagePath);
      }

      Debug.WriteLine($"Out {pageNumber} {outputImagePath} \n");
   }

   private async Task<bool> RenderPage(PdfPage page, string outputImagePath)
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
      }
      catch (Exception ex)
      {
         Debug.WriteLine($"{page.Index + 1} {outputImagePath} \n" + ex.ToString());
         return false;
      }

      return true;
   }

   public async System.Threading.Tasks.Task<PDFPageInfo> UpdatePageInfo(PDFPageInfo pageInfo, string outputImagePath)
   {
      if (_PdfDocument == null || pageInfo == null)
      {
         return null;
      }

      Debug.WriteLine($"UpdatePageInfo {pageInfo.PageNumber} {outputImagePath} \n");

      using (PdfPage page = _PdfDocument.GetPage((uint)pageInfo.PageNumber - 1))
      {
         #region - - - size, ... - - - 

         //ToDo: write converter
         pageInfo.Rotation = (page.Rotation == PdfPageRotation.Normal ? PDFPageOrientations.Portrait : PDFPageOrientations.Landscape);

         pageInfo.Width = PDFHelper.ToCM(page.Size.Width);
         pageInfo.Height = PDFHelper.ToCM(page.Size.Height);

         var rect = PDFHelper.GetPageSizeWithRotation(pageInfo);
         pageInfo.WidthRequest = rect.Width * pageInfo.Scale;
         pageInfo.HeightRequest = rect.Height * pageInfo.Scale;

         if (string.IsNullOrEmpty(outputImagePath))
         {
            Debug.WriteLine($"Out {pageInfo.PageNumber} {outputImagePath} \n");

            return pageInfo;
         }

         #endregion

         #region - - - image - - - 

         if (await RenderPage(page, outputImagePath))
         {
            pageInfo.ImageFileName = outputImagePath;
         }

         #endregion
      }

      Debug.WriteLine($"Out {pageInfo.PageNumber} {outputImagePath} \n");

      return pageInfo;
   }
}

#endif
