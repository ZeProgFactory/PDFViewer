using System.ComponentModel;
using System.Reflection;
using Microsoft.Maui.Controls.Platform;
using ZPF.PDFViewer;
using ZPF.PDFViewer.DataSources;

namespace PDFViewer.Maui.Sample
{
   public partial class MainPage : ContentPage
   {
      public MainPage()
      {
         InitializeComponent();

         pdfViewer.PropertyChanged += PdfViewer_PropertyChanged;
      }


      private void PdfViewer_PropertyChanged(object? sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName == nameof(pdfViewer.IsBusy))
         {
            Title = (pdfViewer.IsBusy ? "IsBusy" : pdfViewer.Infos.Title);
         }
      }


      private void OnToolClicked(object sender, EventArgs e)
      {
         switch ((sender as ToolbarItem)?.CommandParameter.ToString())
         {
            case "PDF1":
               LoadPDFFromRessource("PDFViewer.Maui.Sample.Resources.PDF.pdf1.pdf");
               break;

            case "PDF2":
               LoadPDFFromRessource("pdf2.pdf");
               break;

            case "PDF3":
               LoadPDFFromHTTP();
               break;

            case "PDF4":
               LoadPDFFromHTTP_PWD();
               break;

            case "PICKER":
               LoadPDFFilePicker();
               break;

            case "INFO":
               GetInfo();
               break;

            default:
               Title = "PDF Viewer";
               break;
         }

      }


      private async void LoadPDFFilePicker()
      {
         var customPdfType = new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
               { DevicePlatform.iOS, new[] { "com.adobe.pdf" } },
               { DevicePlatform.Android, new[] { "application/pdf" } },
               { DevicePlatform.WinUI, new[] { ".pdf" } },
               { DevicePlatform.MacCatalyst, new[] { "com.adobe.pdf" } }
            });

         var options = new PickOptions
         {
            PickerTitle = "Select a PDF",
            FileTypes = customPdfType
         };

         var result = await FilePicker.PickAsync(options);

         if (result != null)
         {
            await pdfViewer.LoadPDF(new FilePdfSource(), result.FullPath);
         }
      }


      private async void LoadPDFFromRessource(string pdfRessourceName)
      {
         await pdfViewer.LoadPDF(new AssetPdfSource(), pdfRessourceName);

         //var tmp = GetPdfFileContent(pdfRessourceName);
         //await pdfViewer.LoadPDF(new FilePdfSource(), tmp);

         Load1rstPage();
      }


      private async void LoadPDFFromHTTP()
      {
         // Sample pdf download 10 MB
         var url = "https://www.learningcontainer.com/wp-content/uploads/2019/09/sample-pdf-download-10-mb.pdf";
         await pdfViewer.LoadPDF(new HttpPdfSource(), url);

         Load1rstPage();
      }


      private async void LoadPDFFromHTTP_PWD()
      {
         // Password Protected PDF (protected.pdf)
         var url = "https://sample-files.com/downloads/documents/pdf/protected.pdf";

         var pwd = await DisplayPromptAsync(
                     "Enter PDF Password", "This PDF is password protected. Please enter the password to open it.",
                     "OK", "Cancel", "password", initialValue: "samplefiles");

         await pdfViewer.LoadPDF(new HttpPdfSource(), url, pwd);

         if (!string.IsNullOrEmpty(pdfViewer.LastMessage))
         {
            await DisplayAlertAsync("Oups ...", pdfViewer.LastMessage, "ok");
         }
      }


      private async void GetInfo()
      {
         // Display some info about the PDF
         PDFInfos pdfInfos = pdfViewer.Infos;

         await DisplayAlertAsync("PDF Infos", ""
            + $"Title: {pdfInfos.Title}\n"
            + $"File: {pdfInfos.FileName}\n"
            + $"PWD: {pdfInfos.IsPasswordProtected}\n"
            + $"Pages: {pdfInfos.PageCount}\n"
            + $"Size: {pdfInfos.FileSizeInBytes / 1024} KB\n"
            //+ $"IsEncrypted: {pdfInfos.IsEncrypted}\n" 
            //+ $"IsLinearized: {pdfInfos.IsLinearized}\n"
            , "OK"
            );
      }


      private async void Load1rstPage()
      {
         var tmpFile = System.IO.Path.GetTempFileName();
         await pdfViewer.SaveFirstPageAsImageAsync(tmpFile);

         imageCover.Source = tmpFile;
      }


      private void pdfViewer_ClickOnPage(object sender, SelectedItemChangedEventArgs e)
      {
         //var pageInfo = e.SelectedItem as PDFPageInfo;

         //if (pageInfo != null)
         //{
         //   imageCover.Source = pageInfo.ImageFileName;
         //}

         //var g = sender as Grid;
         //var i = g.Children.First() as Image;

         //i.Source = pageInfo.ImageFileName;

      }


      private async void pdfViewer_DoubleClickOnPage(object sender, SelectedItemChangedEventArgs e)
      {
         if (pdfViewer.ZoomFactor == 1.0)
         {
            await pdfViewer.DoZoom(2.0);
         }
         else
         {
            await pdfViewer.DoZoom(1.0);

         }
      }
   }
}
