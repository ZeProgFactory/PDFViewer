
<table style="background-color:#444; width: 100%;">
   <tr>
      <td align='left' >
         <b>PDFViewer</b>
      </td>
      <td align='right' >
         <a href="../../../index.md">Home</a>
      </td>
   </tr>
   <tr>
      <td align='left' valign='center'>
         <font  style="font-weight: bold; font-size: 26px;">PdfTempFileHelper</font>
      </td>
      <td align='right' valign='center' >
         <font style="font-size: 14px;">Namespace :</font>
         <font style="font-weight: bold; font-size: 14px;">ZPF.PDFViewer.DataSources</font>
      </td>
   </tr>
</table>

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


