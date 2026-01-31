
<table style="background-color:#444; width: 100%;">
<tr>
<td align='left' >
<img width="441" height="1">
<div style="font-weight: bold; font-size: 26px;"> 📚 Documentation PDFViewer</div>
</td>
<td align='right' >
<img width="441" height="1">   
<a href="../../../index.md">Index</a>
</td>
</tr>
</table>

---
# `PdfTempFileHelper`

**Namespace** : `ZPF.PDFViewer.DataSources`

---


&nbsp;   
&nbsp;<br/>
## string `CreateTempPageFilePath` ( string filename ) 

---


&nbsp;<br/>
## string `CreateTempPdfFilePath` (  ) 

**Summary** :

Creates a unique temporary file path for a PDF file.


---


&nbsp;<br/>
## void `DeleteTempFiles` (  ) 

**Summary** :

Deletes all temporary files from the application's temporary folder.

**Remarks** :

This method removes all files located in the temporary folder created by the             CreateTempPageFilePath method. It is recommended to ensure that the application has the necessary permissions to             delete files in this directory before calling this method. If any files cannot be deleted, a debugger breakpoint             is triggered to assist with troubleshooting.


---


