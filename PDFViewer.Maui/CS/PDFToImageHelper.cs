
using System.Diagnostics;

namespace ZPF.PDFViewer.Maui;

public static class PDFToImageHelper
{
   public static async System.Threading.Tasks.Task SaveFirstPageAsImageAsync(string pdfPath, string outputImagePath)
   {
      await SavePageAsImageAsync(pdfPath, outputImagePath);
   }

   public static async System.Threading.Tasks.Task SavePageAsImageAsync(string pdfPath, string outputImagePath, uint pageNumber = 0)
   {
      Debug.WriteLine($"PDFToImageHelper.SavePageAsImageAsync {pageNumber} {pdfPath} {outputImagePath}");

      var PDFViewer = new PDFViewer();

      await PDFViewer.LoadPDF(pdfPath);
      await PDFViewer.SavePageAsImageAsync(outputImagePath, pageNumber);
      PDFViewer.UnloadPDF();
   }
}
