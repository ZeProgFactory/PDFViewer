
using System.Diagnostics;

namespace ZPF.PDFViewer.Maui;

public static class PDFToImageHelper
{
   /// <summary>
   /// Asynchronously saves the first page of a PDF document as an image file at the specified location.
   /// </summary>
   /// <remarks>This method does not block the calling thread while the image is being saved.</remarks>
   /// <param name="pdfPath">The file path of the PDF document from which the first page will be extracted.</param>
   /// <param name="outputImagePath">The file path where the image of the first page will be saved.</param>
   /// <returns>A task that represents the asynchronous save operation.</returns>
   public static async System.Threading.Tasks.Task SaveFirstPageAsImageAsync(string pdfPath, string outputImagePath)
   {
      await SavePageAsImageAsync(pdfPath, outputImagePath);
   }

   /// <summary>
   /// Asynchronously saves a specified page of a PDF document as an image file.
   /// </summary>
   /// <remarks>The PDF document must be successfully loaded before saving a page as an image. Ensure that the
   /// output path is valid and writable. If the specified page number is out of range, the operation may
   /// fail.</remarks>
   /// <param name="pdfPath">The file path of the PDF document to process. This path must refer to an existing and accessible PDF file.</param>
   /// <param name="outputImagePath">The file path where the resulting image will be saved. The file extension determines the image format and must be
   /// supported.</param>
   /// <param name="pageNumber">The zero-based index of the page to save as an image. Defaults to 0, which represents the first page.</param>
   /// <returns>A task that represents the asynchronous operation of saving the specified PDF page as an image.</returns>
   public static async System.Threading.Tasks.Task SavePageAsImageAsync(string pdfPath, string outputImagePath, uint pageNumber = 0)
   {
      Debug.WriteLine($"PDFToImageHelper.SavePageAsImageAsync {pageNumber} {pdfPath} {outputImagePath}");

      var PDFViewer = new PDFViewer();

      await PDFViewer.LoadPDF(pdfPath, false);
      await PDFViewer.SavePageAsImageAsync(outputImagePath, pageNumber);
      PDFViewer.UnloadPDF();
   }


   /// <summary>
   /// !!! Experimental !!!
   /// </summary>
   /// <param name="pdfPath"></param>
   /// <returns></returns>
   public static async System.Threading.Tasks.Task<ImageSource> GetFirstPageAsImageSourceAsync(string pdfPath)
   {
      return await GetPageAsImageSourceAsync(pdfPath, 1);
   }


   /// <summary>
   /// !!! Experimental !!!
   /// </summary>
   /// <param name="pdfPath"></param>
   /// <param name="pageNumber"></param>
   /// <returns></returns>
   public static async System.Threading.Tasks.Task<ImageSource> GetPageAsImageSourceAsync(string pdfPath, int pageNumber = 1)
   {
      Debug.WriteLine($"PDFToImageHelper.GetPageAsImageSourceAsync {pdfPath} {pageNumber}");

      var PDFViewer = new PDFViewer();

      await PDFViewer.LoadPDF(pdfPath, false);
      var imageSource = await PDFViewer.RenderPageToImageSource(pageNumber);
      PDFViewer.UnloadPDF();

      return imageSource;
   }
}
