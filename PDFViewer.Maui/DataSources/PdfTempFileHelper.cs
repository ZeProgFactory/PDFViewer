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
   /// <summary>
   /// Gets the relative path to the temporary subfolder used for storing PDF files within the application.
   /// </summary>
   /// <remarks>This field is intended for internal use to manage the storage location of temporary PDF files.
   /// The path is relative to the application's working directory.</remarks>
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

   /// <summary>
   /// Deletes all temporary files from the application's temporary folder.
   /// </summary>
   /// <remarks>This method removes all files located in the temporary folder created by the
   /// CreateTempPageFilePath method. It is recommended to ensure that the application has the necessary permissions to
   /// delete files in this directory before calling this method. If any files cannot be deleted, a debugger breakpoint
   /// is triggered to assist with troubleshooting.</remarks>
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
