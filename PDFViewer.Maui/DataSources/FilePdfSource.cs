using System;

namespace ZPF.PDFViewer.DataSources;

/// <summary>
/// Represents a PDF source that provides access to PDF files via a specified file path.
/// </summary>
/// <remarks>Use this class to retrieve the file path of a PDF document asynchronously and to load PDF files from
/// disk. If the specified file path does not exist, the LastError property will contain an error message indicating the
/// issue. This class is suitable for scenarios where PDF files are accessed from the local file system.</remarks>
public class FilePdfSource : IPdfSource
{
   string _filePath;


   public FilePdfSource()
   {
      _filePath = string.Empty;
   }


   public FilePdfSource(string filePath)
   {
      _filePath = filePath;
   }


   public string LastError { get; private set; } = "";


   public Task<string> GetFilePathAsync()
   {
      LastError = "";

      if (!File.Exists(_filePath))
      {
         LastError = "File not found: " + _filePath;
         return Task.FromResult("");
      }

      return Task.FromResult(_filePath);
   }


   public Task<string> LoadPDF(string filePath )
   {
      _filePath = filePath;

      return GetFilePathAsync();
   }
}
