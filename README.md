# 28/01/2026 - Work in progress ...
  
__We were unable to get `CollectionView` to scroll horizontally properly, 
so we had to revert to a `ScrollView`. If anyone knows how to implement horizontal 
scrolling on `CollectionView`, we would appreciate your assistance.
😉__

&nbsp; 
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
| Progress  |  85 %  | 85 %  | 85 %  |  85 %   |

![NuGet Downloads](https://img.shields.io/nuget/v/ZPF.PDFViewer.Maui) &nbsp;
![NuGet Downloads](https://img.shields.io/nuget/dt/ZPF.PDFViewer.Maui) &nbsp;
![GitHub License](https://img.shields.io/github/license/ZeProgFactory/PDFViewer) &nbsp;
![last commit](https://img.shields.io/github/last-commit/ZeProgFactory/PDFViewer) 

---



&nbsp; 
## Download
&nbsp;&nbsp; https://www.nuget.org/packages/ZPF.PDFViewer.Maui

&nbsp; 
## Installation
```
Install-Package ZPF.PDFViewer.Maui
```
or
```
dotnet add package ZPF.PDFViewer.Maui 
```

&nbsp; 
## Installation
```
Install-Package ZPF.PDFViewer.Maui
```

&nbsp;
## Usage
**Nothing** to add to `MauiProgram`.

&nbsp;
Simply add `PdfViewer` to XAML
```xaml
<ContentPage
   x:Class="Example.Business.UI.Pages.MainPage"
   xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
   xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
   xmlns:pdf="clr-namespace:ZPF.PDFViewer.Maui;assembly=PDFViewer.Maui">

   <pdf:PDFViewer 
      IsToolbarVisible="True"
      ClickOnPage="pdfViewer_ClickOnPage"
      DoubleClickOnPage="pdfViewer_DoubleClickOnPage" />

</ContentPage>
```

  
&nbsp;
## Set PDF source in code-behind
```C#
   await pdfViewer.LoadPDF(new FilePdfSource(), FullPath);
```


&nbsp;
## Helper classes implementing `IPdfSource`
The `PDFViewer` component works **only with file paths**. This is because the native platform components primarily operate with file paths, and handling different PDF data sources directly inside the component would significantly complicate the code.

Therefore, you must always provide a **file path** regardless of the form your PDF data takes—whether it’s a file, an asset, or a URL.

To simplify working with these data sources, the component includes helper classes that implement the `IPdfSource` interface:

- `AssetPdfSource`
- `FilePdfSource`
- `HttpPdfSource`

&nbsp;

Example of using LoadPDF with different sources:
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

See the example project to see the different sources in action.


You can also create your own implementation of the `IPdfSource` interface to address your specific needs.


&nbsp;
## Helper classe `PDFToImageHelper`

Methods to save PDF pages as images:
- `SaveFirstPageAsImageAsync(string pdfPath, string outputImagePath)`
- `SavePageAsImageAsync(string pdfPath, string outputImagePath, uint pageNumber = 0)`

&nbsp;
Example of using `PDFToImageHelper`
```C#
   {
      string tnFileName = System.IO.Path.GetTempFileName();

      await PDFToImageHelper.SaveFirstPageAsImageAsync(pdfFilepath, tnFileName);

      return tnFileName;
   }
```

&nbsp;
---
**[Experimental doc](Doc/DocGen/index.md)...**
 
&nbsp;
  
