using System;

namespace ZPF.PDFViewer.DataSources;

public class FilePdfSource : IPdfSource
{
   string _filePath;
   string _password;


   public FilePdfSource()
   {
      _filePath = string.Empty;
      _password = string.Empty;
   }


   public FilePdfSource(string filePath, string password = "")
   {
      _filePath = filePath;
      _password = password;
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


   public Task<string> LoadPDF(string filePath, string password="")
   {
      _filePath = filePath;
      _password = password;

      return GetFilePathAsync();
   }
}
