namespace ZPF.PDFViewer.DataSources;

/// <summary>
/// Provides a PDF source that retrieves PDF files asynchronously from a specified HTTP URL.
/// </summary>
/// <remarks>This class implements the IPdfSource interface and enables downloading PDF files from the internet.
/// It exposes the LastError property to allow callers to inspect error details after a failed retrieval attempt. Use
/// LoadPDF to initiate a download from a given URL, or instantiate with a URL and call GetFilePathAsync to retrieve the
/// file path of the downloaded PDF. The class is not thread-safe; concurrent calls may overwrite LastError or the
/// internal URL.</remarks>
public class HttpPdfSource : IPdfSource
{
   string _url;


   public HttpPdfSource()
   {
      _url = string.Empty;
   }


   public HttpPdfSource(string url)
   {
      _url = url;
   }


   public string LastError { get; private set; } = "";


   public async Task<string> GetFilePathAsync()
   {
      LastError = "";
      var tempFile = PdfTempFileHelper.CreateTempPdfFilePath();

      try
      {
         using HttpClient client = new HttpClient();
         client.Timeout = TimeSpan.FromSeconds(15);

         // Add a browser-like User-Agent
         client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) " + "AppleWebKit/537.36 (KHTML, like Gecko) " + "Chrome/120.0.0.0 Safari/537.36");

         using HttpResponseMessage response = await client.GetAsync(_url);
         response.EnsureSuccessStatusCode();
         await using FileStream fs = new FileStream(tempFile, FileMode.Create);
         await response.Content.CopyToAsync(fs);
      }
      catch (Exception ex)
      {
         LastError = ex.ToString();
         System.Diagnostics.Debug.WriteLine(ex.ToString());
      }

      System.Diagnostics.Debug.WriteLine($"LoadPDF HttpPdfSource {tempFile}");

      return tempFile;
   }


   public Task<string> LoadPDF(string url )
   {
      _url = url;

      return GetFilePathAsync();
   }
}
