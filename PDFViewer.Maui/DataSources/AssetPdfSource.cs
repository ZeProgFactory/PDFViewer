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
#if ANDROID
         // Get the assembly that contains the embedded resource 
         // Assembly assembly = Assembly.GetEntryAssembly(); - Works for Windows App
         Assembly assembly = AppDomain.CurrentDomain.GetAssemblies()
                         .SelectMany(s => s.DefinedTypes
                         .Where(t => t.Name == "App"
                                     && t.BaseType.Equals(typeof(Microsoft.Maui.Controls.Application))
                             )
                         ).FirstOrDefault()?.Assembly
                         ?? throw new Exception("Unable to find the Maui App type. This is required to load embedded resources.");

#else
         var assembly = Assembly.GetEntryAssembly();
#endif

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

         System.Diagnostics.Debug.WriteLine($"LoadPDF AssetPdfSource {tempFile}");

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

   private class ResourceHelper
   {
   }
}
