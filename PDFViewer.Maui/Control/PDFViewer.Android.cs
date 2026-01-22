#if ANDROID

using System.Diagnostics;
using Android.Graphics;
using Android.Graphics.Pdf;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Kotlin.Jvm.Functions;
using Microsoft.Maui.Handlers;
using ZPF.PDFViewer.DataSources;
using static Android.Graphics.Pdf.PdfDocument;

namespace ZPF.PDFViewer.Maui;

// Droid-specific implementation
partial class PDFViewer
{
   PdfRenderer _PdfRenderer = null;

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
            var fileDescriptor = ParcelFileDescriptor.Open(
                new Java.IO.File(pdfPath),
                ParcelFileMode.ReadOnly
            );

            // Build LoadParams with password
            // !!! only starting from API 35 (Android 14) !!!
            var loadParams = new Android.Graphics.Pdf.LoadParams.Builder()
                .SetPassword(password)
                .Build();

            // Create renderer with password
            _PdfRenderer = new PdfRenderer(fileDescriptor, loadParams);
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

      await RenderPage(page, outputImagePath);

      // close the page
      page.Close();
   }


   private Matrix? GetCropMatrix(PdfRenderer.Page page, Bitmap bitmap, Thickness bounds)
   {
      if (bounds.IsEmpty)
         return null;

      int pageWidth = page.Width;
      int pageHeight = page.Height;

      var cropLeft = (int)bounds.Left;
      int cropTop = (int)bounds.Top;
      int cropRight = pageWidth - (int)bounds.Right;
      int cropBottom = pageHeight - (int)bounds.Bottom;

      // Create a matrix for shifting and scaling
      Matrix matrix = new Matrix();

      // Scale the cut area to the entire bitmap
      float scaleX = (float)bitmap.Width / (cropRight - cropLeft);
      float scaleY = (float)bitmap.Height / (cropBottom - cropTop);

      matrix.SetScale(scaleX, scaleY);

      // Shift the rendering area so that only the necessary part of the PDF is drawn
      matrix.PostTranslate(-cropLeft * scaleX, -cropTop * scaleY);

      return matrix;
   }


   /// <summary>
   /// Saves an Android.Graphics.Bitmap to a file in PNG or JPEG format.
   /// </summary>
   /// <param name="bitmap">The bitmap to save.</param>
   /// <param name="fileName">The file name (without path).</param>
   /// <param name="format">The image format (PNG or JPEG).</param>
   /// <param name="quality">JPEG quality (0-100). Ignored for PNG.</param>
   /// <returns>The full file path where the bitmap was saved.</returns>
   public string SaveBitmapToFile(Bitmap bitmap, string fileName, Bitmap.CompressFormat format, int quality = 100)
   {
      if (bitmap == null)
         throw new ArgumentNullException(nameof(bitmap));

      if (string.IsNullOrWhiteSpace(fileName))
         throw new ArgumentException("File name cannot be empty.", nameof(fileName));

      try
      {
         using (var stream = new FileStream(fileName, FileMode.Create))
         {
            // Compress and write bitmap to file
            bool success = bitmap.Compress(format, quality, stream);
            if (!success)
               throw new IOException("Failed to compress bitmap.");
         }
      }
      catch (Exception ex)
      {
         throw new IOException($"Error saving bitmap to file: {ex.Message}", ex);
         Debugger.Break();
      }

      return fileName;
   }


   private async Task<string> RenderPage(PdfRenderer.Page page, string outputImagePath)
   {
      try
      {
         // create bitmap at appropriate size
         var bitmap = Bitmap.CreateBitmap(page.Width, page.Height, Bitmap.Config.Argb8888);

         //  If you need to apply a color to the page
         bitmap.EraseColor(Android.Graphics.Color.White);

         // Crop page
         var matrix = GetCropMatrix(page, bitmap, Thickness.Zero);

         // render PDF page to bitmap
         page.Render(bitmap, null, matrix, PdfRenderMode.ForDisplay);

         string savedPath = SaveBitmapToFile(bitmap, outputImagePath, Bitmap.CompressFormat.Png);

         System.Diagnostics.Debug.WriteLine($"LoadPDF {savedPath}");

         if( savedPath != outputImagePath)
         {
            Debugger.Break();
         }

         return savedPath;
      }
      catch (Exception ex)
      {
         Debugger.Break();
         return "";
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

      pageInfo.Rotation = (page.Width > page.Height) ? PDFHelper.PDFPageOrientations.Landscape : PDFHelper.PDFPageOrientations.Portrait;

      pageInfo.Width = PDFHelper.ToCM(page.Width);
      pageInfo.Height = PDFHelper.ToCM(page.Height);

      var rect = PDFHelper.GetPageSizeWithRotation(pageInfo);
      pageInfo.WidthRequest = rect.Width * pageInfo.Scale;
      pageInfo.HeightRequest = rect.Height * pageInfo.Scale;

      if (string.IsNullOrEmpty(outputImagePath))
      {
         // close the page
         page.Close();

         System.Diagnostics.Debug.WriteLine($"Out bug {pageInfo.PageNumber} {outputImagePath} \n");

         return pageInfo;
      }

      #endregion

      #region - - - image - - - 

      pageInfo.ImageFileName = await RenderPage(page, outputImagePath);
      System.Diagnostics.Debug.WriteLine($"Out {pageInfo.PageNumber} {outputImagePath} \n");

      #endregion

      // close the page
      page.Close();

      return pageInfo;
   }
}

#endif
