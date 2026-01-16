using System;

namespace ZPF.PDFViewer.DataSources;

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

   public string LastError { get; private set; }


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


   public Task<string> LoadPDF(string filePath)
   {
      _filePath = filePath;

      return GetFilePathAsync();
   }
}
