# 12/02/2026 - Work in progress ...
  
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

| Platform  | Android | iOS  |  Mac  | Windows |
| :-------- | :-----: | :--: | :---: | :-----: |
| Supported (goal) | ✅ | ✅ | ✅   |   ✅    |
| Progress  |  90 %  | 90 %  | 90 %  |  90 %   |

MacCatalyst is not tested yet!

![NuGet Downloads](https://img.shields.io/nuget/v/ZPF.PDFViewer.Maui) &nbsp;
![NuGet Downloads](https://img.shields.io/nuget/dt/ZPF.PDFViewer.Maui) &nbsp;
![GitHub License](https://img.shields.io/github/license/ZeProgFactory/PDFViewer) &nbsp;
![last commit](https://img.shields.io/github/last-commit/ZeProgFactory/PDFViewer) 

---

<div id="image-table">
  <table >
    <tr>
      <td style="padding:10px">
        <img src="https://raw.githubusercontent.com/ZeProgFactory/PDFViewer/refs/heads/main/Doc/ScreenShots/iOS01.png" alt="iOS" height="500" >
      </td>
      <td style="padding:10px">
        <img src="https://raw.githubusercontent.com/ZeProgFactory/PDFViewer/refs/heads/main/Doc/ScreenShots/Android01.png" alt="Android" height="500" >
      </td>
      <td style="padding:10px">
        <img src="https://raw.githubusercontent.com/ZeProgFactory/PDFViewer/refs/heads/main/Doc/ScreenShots/WinUI01.png" alt="WinUI" height="500" >
      </td>
    </tr>
  </table>
</div>

---

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
      BackgroundColor="AntiqueWhite"
      ToolbarColor="LightGray" 
      ClickOnPage="pdfViewer_ClickOnPage"
      DoubleClickOnPage="pdfViewer_DoubleClickOnPage" />

</ContentPage>
```

  
&nbsp;
## Set PDF source in code-behind

### Load PDF from file path
```C#
   await pdfViewer.LoadPDF(fullPath);
```


### Load PDF with password
```C#
   await pdfViewer.LoadPDF(fullPath, password);
```
_For Android, Passwords are only supported on: 'android' 35.0 and later._


### Error handling
```C#
   if (!await pdfViewer.LoadPDF(fullPath, password))
   {
      await DisplayAlertAsync("Oups ...", pdfViewer.LastMessage, "ok");
   }
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

### Load PDF from file path
```C#
   await pdfViewer.LoadPDF(new FilePdfSource(), FullPath);
```

### Load PDF from URL 
```C#
   await pdfViewer.LoadPDF(new HttpPdfSource(), "https://www.learningcontainer.com/wp-content/uploads/2019/09/sample-pdf-download-10-mb.pdf");
```

### Load PDF from resource asset
```C#
   await pdfViewer.LoadPDF(new AssetPdfSource(),"Example.Resources.PDF.pdf2.pdf");
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
**[Experimental doc](Doc/DocGen/index.pdf)...**
 
&nbsp;
  
