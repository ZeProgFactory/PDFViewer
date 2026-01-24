namespace ZPF.PDFViewer.DataSources;


public interface IPdfSource
{
   string LastError { get; }

   Task<string> GetFilePathAsync();

   Task<string> LoadPDF(string url);
}

