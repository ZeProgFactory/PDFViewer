
<table style="background-color:#444; width: 100%;">
<tr>
<td align='left' >
<img width="441" height="1">
<div style="font-weight: bold; font-size: 26px;"> 📚 Documentation PDFViewer</div>
</td>
<td align='right' >
<img width="441" height="1">   
<a href="../../index.md">Index</a>
</td>
</tr>
</table>

---
# `PDFInfos`

**Namespace** : `ZPF.PDFViewer`

---


&nbsp;   
&nbsp;<br/>
## string `FileName` ( get;  set;) 

**Summary** :

Gets the name of the file associated with this instance.

**Remarks** :

The file name is set internally and cannot be modified directly by external callers. This property is             typically used to identify or reference the file being processed by the instance.


---


&nbsp;<br/>
## Int64 `FileSizeInBytes` ( get;  set;) 

**Summary** :

Gets the size of the file, in bytes.


---


&nbsp;<br/>
## bool `IsPasswordProtected` ( get;  set;) 

**Summary** :

Gets whether the Portable Document Format (PDF) document is password-protected.


---


&nbsp;<br/>
## int `PageCount` ( get;  set;) 

**Summary** :

Gets the total number of pages in the document. A value of -1 indicates that the page count has not been             determined.

**Remarks** :

The page count is set internally by the document processing logic. The value may change if the             document is modified or reloaded.


---


&nbsp;<br/>
## string `Title` ( get; ) 

**Summary** :

Gets the title of the item. If the title is not set, returns the file name without its extension.

**Remarks** :

The title is automatically derived from the file name when it has not been explicitly             specified. This allows for a default, meaningful title even when a custom title is not provided.


---


&nbsp;<br/>
## string `ToString` (  ) 

---


