#if MACCATALYST || IOS

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using CoreGraphics;
using Foundation;
using ImageIO;
using MobileCoreServices;
using PdfKit;
using UIKit;
using ZPF.PDFViewer.DataSources;
using static ZPF.PDFViewer.PDFHelper;

namespace ZPF.PDFViewer.Maui;

// iOS-specific implementation 
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

      try
      {
         _PdfDocument = new PdfDocument(NSUrl.FromFilename(pdfPath));

         if (!string.IsNullOrEmpty(password) && _PdfDocument.IsLocked)
         {
            Infos.IsPasswordProtected = true;
            _PdfDocument.Unlock(password);
         }
         else
         {
            if (_PdfDocument.IsLocked)
            {
               _PdfDocument = null;
               LastMessage = "PDF is locked.";
            }
         }
      }
      catch (Exception ex)
      {
         _PdfDocument = null;
         LastMessage = ex.Message.ToString();

         Debugger.Break();
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

      // delete previously created temp files
      PdfTempFileHelper.DeleteTempFiles();
   }

   private async Task<PDFInfos> NewPDFInfos(string pdfPath, string url, string password = "")
   {
      _PDFInfos = new PDFInfos();

      if (_PdfDocument == null)
      {
         await LoadPDF(pdfPath, password);
      }

      if (_PdfDocument != null)
      {
         //_PdfDocument.Description;

         _PDFInfos.PageCount = (int)_PdfDocument.PageCount;
         _PDFInfos.FileName = url;
         _PDFInfos.FileSizeInBytes = new System.IO.FileInfo(pdfPath).Length;
      }

      return _PDFInfos;
   }


   public async System.Threading.Tasks.Task SavePageAsImageAsync(string outputImagePath, uint pageNumber = 0)
   {
      if (_PdfDocument == null)
      {
         // ("No PDF loaded.");
         return;
      }

      if (_PdfDocument.PageCount == 0)
         throw new InvalidOperationException("PDF has no pages.");

      // Get page
      var page = _PdfDocument.GetPage((nint)pageNumber);

      await RenderPageToFile(page, outputImagePath);
   }

   private async Task<bool> RenderPageToFile(PdfPage page, string outputImagePath)
   {
      float scale = 2.0f; // ⭐ scale factor: 1 = native, 2 = 2×, 3 = 3×, etc.

      try
      {
         var bounds = page.GetBoundsForBox(PdfDisplayBox.Media);

         int width = (int)(bounds.Width * scale);
         int height = (int)(bounds.Height * scale);

         using var colorSpace = CGColorSpace.CreateDeviceRGB();
         using var context = new CGBitmapContext(
             IntPtr.Zero,
             width,
             height,
             8,
             width * 4,
             colorSpace,
             CGImageAlphaInfo.PremultipliedLast
         );

         // White background
         context.SetFillColor(1, 1, 1, 1);
         context.FillRect(new CGRect(0, 0, width, height));

         // Flip coordinate system for PDF drawing
         context.TranslateCTM(0, height);
         context.ScaleCTM(1, -1);

         // Apply scale factor to PDF content
         context.ScaleCTM(scale, scale);

         // Apply PDF page rotation
         int rotation = (int)page.Rotation;
         if (rotation != 0)
         {
            context.TranslateCTM(width / 2f, height / 2f);
            context.RotateCTM((float)(rotation * Math.PI / 180));
            context.TranslateCTM(-width / 2f, -height / 2f);
         }

         // Draw PDF page
         page.Draw(PdfDisplayBox.Media, context);

         // Create CGImage from the (flipped) context
         using var cgImage = context.ToImage();

         // ⭐ Flip the final CGImage so the PNG is upright
         using var finalContext = new CGBitmapContext(
             IntPtr.Zero,
             width,
             height,
             8,
             width * 4,
             colorSpace,
             CGImageAlphaInfo.PremultipliedLast
         );

         finalContext.SetFillColor(1, 1, 1, 1);
         finalContext.FillRect(new CGRect(0, 0, width, height));

         // Flip vertically
         finalContext.TranslateCTM(0, height);
         finalContext.ScaleCTM(1, -1);

         finalContext.DrawImage(new CGRect(0, 0, width, height), cgImage);

         using var finalImage = finalContext.ToImage();

         // Save PNG
         using var url = NSUrl.FromFilename(outputImagePath);
         using var dest = CGImageDestination.Create(url, UTType.PNG, 1);

         dest.AddImage(finalImage);
         dest.Close();

         return true;
      }
      catch (Exception ex)
      {
         Debug.WriteLine($"RenderPage Error: {ex.Message} \n");
         return false;
      }
   }


   private async Task<ImageSource> RenderPageToImageSource(PdfPage page)
   {
      nfloat scale = 2.0f; // ⭐ scale factor: 1 = native, 2 = 2×, 3 = 3×, etc.   

      // Get page bounds
      var bounds = page.GetBoundsForBox(PdfDisplayBox.Media);
      var targetSize = new CGSize(bounds.Width * scale, bounds.Height * scale);

      // Begin drawing
      UIGraphics.BeginImageContextWithOptions(targetSize, false, scale);
      var ctx = UIGraphics.GetCurrentContext();

      // Flip coordinate system
      ctx.SaveState();
      ctx.TranslateCTM(0, targetSize.Height);
      ctx.ScaleCTM(1, -1);

      // Scale for resolution
      ctx.ScaleCTM(scale, scale);

      // Correct Draw() call
      page.Draw(PdfDisplayBox.Media, ctx);

      ctx.RestoreState();

      // Get UIImage
      var uiImage = UIGraphics.GetImageFromCurrentImageContext();
      UIGraphics.EndImageContext();

      // Convert to PNG bytes
      var data = uiImage.AsPNG();
      var bytes = data.ToArray();

      // Return MAUI ImageSource
      return ImageSource.FromStream(() => new MemoryStream(bytes));
   }


   async System.Threading.Tasks.Task<PDFPageInfo> UpdatePageInfo(PDFPageInfo pageInfo, string outputImagePath, [CallerMemberName] string callerName = "")
   {
      if (_PdfDocument == null || pageInfo == null)
      {
         return null;
      }

      Debug.WriteLine($"UpdatePageInfo In {pageInfo.PageNumber} {callerName} \n");

      // Get page
      var page = _PdfDocument.GetPage((nint)pageInfo.PageNumber - 1);

      //ToDo: write converter
      pageInfo.Rotation = (page.Rotation == 0 || page.Rotation == 180) ? PDFHelper.PDFPageOrientations.Portrait : PDFHelper.PDFPageOrientations.Landscape;

      var b = page.GetBoundsForBox(PdfDisplayBox.Media);
      pageInfo.Width = PDFHelper.ToCM(b.Width);
      pageInfo.Height = PDFHelper.ToCM(b.Height);

      pageInfo.RealWidth = (int)b.Width;
      pageInfo.RealHeight = (int)b.Height;

      pageInfo.ImageSource = await RenderPageToImageSource(page);

      Debug.WriteLine($"UpdatePageInfo Out {pageInfo.PageNumber} {callerName} \n");

      return pageInfo;
   }
}

#endif
