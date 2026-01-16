using System.Reflection;
using System.Xml.Linq;

namespace ZPF.PDFViewer.DataSources;

public class AssetPdfSource : IPdfSource
{
    string _resourceName;

    public AssetPdfSource()
    {
        _resourceName = string.Empty;
    }


    public AssetPdfSource(string resourceName)
    {
        _resourceName = resourceName;
    }

    public string LastError { get; private set; }


    public async Task<string> GetFilePathAsync()
    {
        LastError = "";

        try
        {
            var assembly = Assembly.GetEntryAssembly();
            string resourcePath = assembly
                .GetManifestResourceNames()
                .Single(str => str.EndsWith(_resourceName));

            byte[] bytes;
            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            {
                bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
            }

            var tempFile = PdfTempFileHelper.CreateTempPdfFilePath();
            await File.WriteAllBytesAsync(tempFile, bytes);

            return tempFile;
        }
        catch (Exception ex)
        {
            LastError = ex.ToString();
            System.Diagnostics.Debug.WriteLine(ex.ToString());

            return "";
        }
    }


    public Task<string> LoadPDF(string resourcePath)
    {
        _resourceName = resourcePath;

        return GetFilePathAsync();
    }
}
