#if MACCATALYST || IOS

using System;
using System.Diagnostics;
using CoreGraphics;
using Foundation;
using ImageIO;
using MobileCoreServices;
using PdfKit;
using ZPF.PDFViewer.DataSources;

namespace ZPF.PDFViewer.Maui;

// iOS-specific implementation 
partial class PDFViewer
{
   PdfDocument _PdfDocument = null;

   public async Task LoadPDF(string pdfPath)
   {
      UnloadPDF();

      // Open the PDF file
      _PdfDocument = new PdfDocument(NSUrl.FromFilename(pdfPath));
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

   private async Task<PDFInfos> NewPDFInfos(string pdfPath, string url)
   {
      if (_PdfDocument == null)
      {
         await LoadPDF(pdfPath);
      }

      //_PdfDocument.Description;

      _PDFInfos = new PDFInfos()
      {
         PageCount = (int)_PdfDocument.PageCount,
         FileName = url,
         FileSizeInBytes = new System.IO.FileInfo(pdfPath).Length,
      };

      return _PDFInfos;
   }


   public async System.Threading.Tasks.Task SavePageAsImageAsync(string outputImagePath, uint pageNumber = 0)
   {
      float scale = 2.0f; // ⭐ scale factor: 1 = native, 2 = 2×, 3 = 3×, etc.

      if (_PdfDocument == null)
      {
         // ("No PDF loaded.");
         return;
      }

      if (_PdfDocument.PageCount == 0)
         throw new InvalidOperationException("PDF has no pages.");

      // Get page
      var page = _PdfDocument.GetPage((nint)pageNumber);
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


   public async System.Threading.Tasks.Task<PDFPageInfo> UpdatePageInfo(PDFPageInfo pageInfo, string outputImagePath)
   {
      throw new NotImplementedException();
   }
}

#endif
