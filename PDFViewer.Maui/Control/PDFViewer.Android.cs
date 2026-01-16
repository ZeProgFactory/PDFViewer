#if ANDROID

using Android.Graphics.Pdf;
using Android.Graphics;
using Android.OS;
using AndroidX.RecyclerView.Widget;
using Microsoft.Maui.Handlers;
using Android.Widget;
using Android.Views;
using ZPF.PDFViewer.DataSources;

namespace ZPF.PDFViewer.Maui;

// Droid-specific implementation
partial class PDFViewer
{
   PdfRenderer _PdfRenderer = null;

   public async Task LoadPDF(string pdfPath)
   {
      UnloadPDF();

      // Open the PDF file
      //StorageFile pdfFile = await StorageFile.GetFileFromPathAsync(pdfPath);

      //using (IRandomAccessStream pdfStream = await pdfFile.OpenAsync(FileAccessMode.Read))
      //{
      //   // Load the PDF document
      //   _PdfDocument = await PdfDocument.LoadFromStreamAsync(pdfStream);
      //}

      _PdfRenderer = new PdfRenderer(ParcelFileDescriptor.Open(new Java.IO.File(pdfPath), ParcelFileMode.ReadOnly));

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


   private async Task<PDFInfos> NewPDFInfos(string pdfPath, string url)
   {
      if (_PdfRenderer == null)
      {
         await LoadPDF(pdfPath);
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

      // create bitmap at appropriate size
      var bitmap = Bitmap.CreateBitmap(page.Width, page.Height, Bitmap.Config.Argb8888);

      //  If you need to apply a color to the page
      bitmap.EraseColor(Android.Graphics.Color.White);

      // Crop page
      var matrix = GetCropMatrix(page, bitmap, Thickness.Zero);

      // render PDF page to bitmap
      page.Render(bitmap, null, matrix, PdfRenderMode.ForDisplay);

      string savedPath = SaveBitmapToFile(bitmap, outputImagePath, Bitmap.CompressFormat.Png);

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
    public static string SaveBitmapToFile(Bitmap bitmap, string fileName, Bitmap.CompressFormat format, int quality = 100)
    {
        if (bitmap == null)
            throw new ArgumentNullException(nameof(bitmap));

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name cannot be empty.", nameof(fileName));

        // Ensure file name has correct extension
        //string extension = format == Bitmap.CompressFormat.Png ? ".png" : ".jpg";
        //if (!fileName.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
        //   fileName += extension;

        //// Save to app's cache directory
        //string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
        string filePath = fileName;

        try
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
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
        }

        return filePath;
    }


    public void RenderPages()
   {
      //if (_PdfDocument == null)
      //{
      //   return;
      //}

      //if (Handler is IPlatformViewHandler platformHandler)
      //{
      //   (platformHandler as Maui.PDFView.PdfViewHandler)?.RenderPages(_PdfDocument);
      //}
   }
}

#endif
