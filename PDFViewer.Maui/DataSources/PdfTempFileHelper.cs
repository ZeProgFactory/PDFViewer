using System.Diagnostics;

namespace ZPF.PDFViewer.DataSources;

/// <summary>
/// Provides utility methods for creating and managing temporary file paths for PDF documents and related page files.
/// </summary>
/// <remarks>This class offers platform-aware methods to generate unique temporary file paths for PDF and page
/// files, ensuring correct directory formatting and creation across supported operating systems. It also includes
/// functionality to delete all temporary files within the designated folder. Use these methods to safely handle
/// temporary PDF-related files in cross-platform applications.</remarks>
public class PdfTempFileHelper
{
   public static string _TmpSubFolder = @"PDFViewer\";

   /// <summary>
   /// Creates a unique temporary file path for a PDF file.
   /// </summary>
   public static string CreateTempPdfFilePath()
   {
      var path = CreateTempPageFilePath(Path.GetRandomFileName() + ".pdf");

#if ANDROID || IOS || MACCATALYST
      path = path.Replace("\\", "/");
      path = path.Replace("//", "/");
#endif

      return path;
   }

   /// <summary>
   /// Creates a unique temporary file path for a page file.
   /// </summary>
   public static string CreateTempPageFilePath(string filename)
   {
#if ANDROID
      var tmpFolder = Path.Combine(FileSystem.AppDataDirectory, _TmpSubFolder);
#elif IOS || MACCATALYST
      var tmpFolder = Path.Combine(FileSystem.AppDataDirectory, _TmpSubFolder);
#else
      var tmpFolder = Path.Combine(Path.GetTempPath(), _TmpSubFolder);
#endif

#if ANDROID || IOS || MACCATALYST
      tmpFolder = tmpFolder.Replace("\\", "/");
      tmpFolder = tmpFolder.Replace("//", "/");
#endif

      if (!Directory.Exists(tmpFolder))
      {
         Directory.CreateDirectory(tmpFolder);
      }

#if ANDROID
      //tmpFolder = Path.GetTempPath();
#endif

      return Path.Combine(tmpFolder, filename);
   }


   public static void DeleteTempFiles()
   {
//#if ANDROID
//#else
      var tmpFolder = CreateTempPageFilePath("");

      // Get all files in the folder (non-recursive)
      string[] files = Directory.GetFiles(tmpFolder);

      foreach (string file in files)
      {
         try
         {
            File.Delete(file);
         }
         catch 
         { 
            Debugger.Break();
         }
      }

      if( Directory.GetFiles(tmpFolder).Count() > 0 )
      { 
         Debugger.Break();
      }

      //#endif
   }
}
