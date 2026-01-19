# 19/01/2026 - Work in progress ...
  
# PDFViewer
A simple Maui PDF viewer based on the nativ PDF libraries ...   
based on ideas and code of [vitalii-vov](https://github.com/vitalii-vov) ( https://github.com/vitalii-vov/Maui.PDFView )

_“Powered by native PDF engines. Wrapped in simplicity.”_  

---   

| .NET MAUI | .NET 10   | 
| :-------- | :-------  | 

| Platform  | Android | iOS  | MacOS | Windows |
| :-------- | :-----: | :--: | :---: | :-----: |
| Supported (goal) | ✅ | ✅ | ✅   |   ✅    |
| Progress  |   80 %  | 80 % | 80 %  |  80 %   |

![NuGet Downloads](https://img.shields.io/nuget/dt/ZPF.PDFViewer?style=for-the-badge)
![GitHub License](https://img.shields.io/github/license/ZeProgFactory/PDFViewer?style=for-the-badge)
![last commit](https://img.shields.io/github/last-commit/ZeProgFactory/PDFViewer?style=for-the-badge)

---

&nbsp;<br>
## Installation
```
Install-Package ZPF.PDFViewer
```

&nbsp;<br>
## Usage
**Nothing** to ad to MauiProgram.

&nbsp;<br>
Simply add `PdfViewer` to XAML
```xaml
<ContentPage
    x:Class="Example.Business.UI.Pages.MainPage"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:pdf="clr-namespace:ZPF.PDFViewer.Maui;assembly=PDFViewer.Maui">

    <pdf:PdfViewer />

</ContentPage>
```

<!---
&nbsp;<br>
Set `PdfSource` in ViewModel
```C#
internal partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty] private string? _pdfSource;

    [RelayCommand] private void ChangeUri()
    {
        try 
        {
            //  See the example project to understand how to work with paths.
            PdfSource = "/path/to/file.pdf";

            //  You can set the Uri property to null to clear the component.
            //PdfSource = null;
        }
        catch(Exception ex)
        {
             // handle exceptions
        }
    }
}
```
-->

&nbsp;<br>
## Helper classes implementing `IPdfSource`
The `PdfView` component works **only with file paths**. This is because the native platform components primarily operate with file paths, and handling different PDF data sources directly inside the component would significantly complicate the code.

Therefore, you must always provide a **file path** regardless of the form your PDF data takes—whether it’s a byte array, a stream, an asset, or a URL.

To simplify working with these data sources, the component includes helper classes that implement the `IPdfSource` interface:

- `AssetPdfSource`
- `FilePdfSource`
- `HttpPdfSource`

Example of using PdfSource
```C#
[RelayCommand] private async Task UploadUri()
{      
     await pdfViewer.LoadPDF(new HttpPdfSource(), "https://www.learningcontainer.com/wp-content/uploads/2019/09/sample-pdf-download-10-mb.pdf");
}

[RelayCommand] private async Task UploadAsset()
{
    await pdfViewer.LoadPDF(new AssetPdfSource(),"Example.Resources.PDF.pdf2.pdf");
}
```

You can also create your own implementation of the `IPdfSource` interface to address your specific needs.


