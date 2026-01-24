# `PDFViewer`

**Namespace :** `ZPF`

---

## `PDFInfos`

**Résumé :**

Represents metadata and basic information about a Portable Document Format (PDF) document, including its file name,             page count, file size, title, and password protection status.

**Remarques :**

The PDFInfos class provides properties to access common attributes of a PDF file. The Title property             returns the document's title if set; otherwise, it derives the title from the file name without its extension. The             IsPasswordProtected property indicates whether the PDF requires a password to open. This class is intended for use             in scenarios where PDF file details need to be displayed or processed, such as in document viewers or file             management tools.

---


## `PDFPageInfo`

**Résumé :**

Represents information about a single page in a PDF document, including its dimensions, image source, and display             status.

**Remarques :**

This class provides properties for managing the page number, width and height in centimeters, image             file name, and image source for rendering. It also tracks whether the page is currently selected and supports             updating its values from another instance. The class is designed for use in PDF viewing scenarios where page             metadata and visual representation are required.

---


