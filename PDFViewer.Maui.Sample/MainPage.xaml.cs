using System.ComponentModel;
using System.Reflection;
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
                Title = (pdfViewer.IsBusy ? "IsBusy" : pdfViewer.GetInfos().Title);
            }
        }


        private void OnToolClicked(object sender, EventArgs e)
        {
            switch ((sender as ToolbarItem)?.CommandParameter.ToString())
            {
                case "PDF1":
                    LoadPDFFromRessource("pdf1.pdf");
                    break;

                case "PDF2":
                    LoadPDFFromRessource("pdf2.pdf");
                    break;

                case "PDF3":
                    LoadPDFFromHTTP();
                    break;

                case "INFO":
                    GetInfo();
                    break;

                default:
                    Title = "PDF Viewer";
                    break;
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


        private async void GetInfo()
        {
            // Display some info about the PDF
            PDFInfos pdfInfos = pdfViewer.GetInfos();

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
    }
}
