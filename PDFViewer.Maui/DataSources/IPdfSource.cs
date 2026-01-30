namespace ZPF.PDFViewer.DataSources;


public interface IPdfSource
{
   string LastMessage { get; }

   Task<string> GetFilePathAsync();

   Task<string> LoadPDF(string url);
}

