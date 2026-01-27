#if ANDROID

using System.Diagnostics;
using Android.Graphics;
using Android.Graphics.Pdf;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Java.IO;
using Kotlin.Jvm.Functions;
using Microsoft.Maui.Handlers;
using ZPF.PDFViewer.DataSources;
using static Android.Graphics.Pdf.PdfDocument;
using static ZPF.PDFViewer.PDFHelper;

namespace ZPF.PDFViewer.Maui;

// Droid-specific implementation
partial class PDFViewer
{
   PdfRenderer _PdfRenderer = null;
   ParcelFileDescriptor _FileDescriptor = null;

   public async Task LoadPDF(string pdfPath, string password = "")
   {
      // Open the PDF file
      if (!System.IO.File.Exists(pdfPath))
      {
         Debugger.Break();
      }

      System.Diagnostics.Debug.WriteLine($"LoadPDF {pdfPath} \n");

      try
      {
         if (string.IsNullOrEmpty(password))
         {
            _PdfRenderer = new PdfRenderer(ParcelFileDescriptor.Open(new Java.IO.File(pdfPath), ParcelFileMode.ReadOnly));
         }
         else
         {
            // Open the file
            _FileDescriptor = ParcelFileDescriptor.Open(new Java.IO.File(pdfPath), ParcelFileMode.ReadOnly);

            // Build LoadParams with password
            // !!! only starting from API 35 (Android 14) !!!
            var loadParams = new Android.Graphics.Pdf.LoadParams.Builder()
                .SetPassword(password)
                .Build();

            // Create renderer with password
            _PdfRenderer = new PdfRenderer(_FileDescriptor, loadParams);
         }
      }
      catch (Exception ex)
      {
         _PdfRenderer = null;
         LastMessage = ex.Message.ToString();
      }
   }


   public void UnloadPDF()
   {
      if (_PdfRenderer != null)
      {
         // close the renderer
         _PdfRenderer.Close();
         _PdfRenderer = null;

         _FileDescriptor?.Close();
         _FileDescriptor = null;

         _PDFInfos = new PDFInfos();
      }

      ClearPages();

      // delete previously created temp files
      PdfTempFileHelper.DeleteTempFiles();
   }


   private async Task<PDFInfos> NewPDFInfos(string pdfPath, string url, string password = "")
   {
      if (_PdfRenderer == null)
      {
         await LoadPDF(pdfPath, password);
      }

      if (_PdfRenderer != null)
      {
         _PDFInfos = new PDFInfos()
         {
            PageCount = _PdfRenderer.PageCount,
            // IsPasswordProtected = _PdfRenderer.,
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
      if (_PdfRenderer == null)
      {
         return;
      }

      var page = _PdfRenderer.OpenPage((int)pageNumber);

      await RenderPageToFile(page, outputImagePath);

      // close the page
      page.Close();
   }


   private async Task<string> RenderPageToFile(PdfRenderer.Page page, string outputImagePath)
   {
      try
      {
         // Create bitmap with page size 
         var bitmap = Bitmap.CreateBitmap(page.Width, page.Height, Bitmap.Config.Argb8888);

         //  If you need to apply a color to the page
         bitmap.EraseColor(Android.Graphics.Color.White);

         // render PDF page to bitmap
         page.Render(bitmap, null, null, PdfRenderMode.ForDisplay);
         //page.Close();

         // Save bitmap to file 
         using (var fs = new FileStream(outputImagePath, FileMode.Create))
         {
            bitmap.Compress(Bitmap.CompressFormat.Png, 100, fs);
         }
         bitmap.Dispose();
         //renderer.Close(); 
         //fileDescriptor.Close();

         System.Diagnostics.Debug.WriteLine($"LoadPDF {outputImagePath}");

         return outputImagePath;
      }
      catch (Exception ex)
      {
         Debugger.Break();
         return "";
      }
   }


   private async Task<ImageSource> RenderPageToImageSource(PdfRenderer.Page page)
   {
      try
      {
         // Create bitmap with page size 
         var bitmap = Bitmap.CreateBitmap(page.Width, page.Height, Bitmap.Config.Argb8888);

         //  If you need to apply a color to the page
         bitmap.EraseColor(Android.Graphics.Color.White);

         // render PDF page to bitmap
         page.Render(bitmap, null, null, PdfRenderMode.ForDisplay);
         //page.Close();

         // Convert bitmap to stream 
         var ms = new MemoryStream();
         bitmap.Compress(Bitmap.CompressFormat.Png, 100, ms);
         ms.Position = 0;

         bitmap.Dispose();

         return ImageSource.FromStream(() => ms);
      }
      catch (Exception ex)
      {
         Debugger.Break();
         return null;
      }
   }


   public async System.Threading.Tasks.Task<PDFPageInfo> UpdatePageInfo(PDFPageInfo pageInfo, string outputImagePath)
   {
      if (_PdfRenderer == null || pageInfo == null)
      {
         return null;
      }

      System.Diagnostics.Debug.WriteLine($"UpdatePageInfo {pageInfo.PageNumber} {outputImagePath} \n");


      var page = _PdfRenderer.OpenPage((int)pageInfo.PageNumber - 1);

      #region - - - size, ... - - - 

      //ToDo: write converter
      pageInfo.Rotation = (page.Width > page.Height) ? PDFHelper.PDFPageOrientations.Landscape : PDFHelper.PDFPageOrientations.Portrait;

      pageInfo.Width = PDFHelper.ToCM(page.Width);
      pageInfo.Height = PDFHelper.ToCM(page.Height);

      pageInfo.RealWidth = (int)page.Width;
      pageInfo.RealHeight = (int)page.Height;

      pageInfo.ImageSource = await RenderPageToImageSource(page);

      System.Diagnostics.Debug.WriteLine($"Out {pageInfo.PageNumber} {outputImagePath} \n");

      #endregion

      // close the page
      page.Close();

      return pageInfo;
   }
}

#endif
