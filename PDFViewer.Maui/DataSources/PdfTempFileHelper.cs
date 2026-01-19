namespace ZPF.PDFViewer.DataSources;

public class PdfTempFileHelper
{
   public static string _TmpSubFolder = @"PDFViewer\";

   /// <summary>
   /// Creates a unique temporary file path for a PDF file.
   /// </summary>
   public static string CreateTempPdfFilePath()
   {
      var tmpFolder = Path.Combine(Path.GetTempPath(), _TmpSubFolder);

      if (!Directory.Exists(tmpFolder))
      {
         Directory.CreateDirectory(tmpFolder);
      }

      var path = Path.Combine(tmpFolder, Path.GetRandomFileName() + ".pdf");

      return path;
   }

   /// <summary>
   /// Creates a unique temporary file path for a page file.
   /// </summary>
   public static string CreateTempPageFilePath(string filename)
   {
      var tmpFolder = Path.Combine(Path.GetTempPath(), _TmpSubFolder);

      if (!Directory.Exists(tmpFolder))
      {
         Directory.CreateDirectory(tmpFolder);
      }

      return Path.Combine(tmpFolder, filename);
   }


   public static void DeleteTempFiles()
   {
      var tmpFolder = CreateTempPageFilePath("");

      // Get all files in the folder (non-recursive)
      string[] files = Directory.GetFiles(tmpFolder);

      foreach (string file in files)
      {
         try
         {
            File.Delete(file);
         }
         catch { }
      }
   }
}
