namespace ZPF.PDFViewer;

/// <summary>
/// Represents metadata and basic information about a Portable Document Format (PDF) document, including its file name,
/// page count, file size, title, and password protection status.
/// </summary>
/// <remarks>The PDFInfos class provides properties to access common attributes of a PDF file. The Title property
/// returns the document's title if set; otherwise, it derives the title from the file name without its extension. The
/// IsPasswordProtected property indicates whether the PDF requires a password to open. This class is intended for use
/// in scenarios where PDF file details need to be displayed or processed, such as in document viewers or file
/// management tools.</remarks>
public class PDFInfos
{
   /// <summary>
   /// Gets the name of the file associated with this instance.
   /// </summary>
   /// <remarks>The file name is set internally and cannot be modified directly by external callers. This property is
   /// typically used to identify or reference the file being processed by the instance.</remarks>
   public string FileName
   {
      get { return _FileName; }
      internal set { _FileName = value; }
   }
   string _FileName = string.Empty;

   /// <summary>
   /// Gets the total number of pages in the document. A value of -1 indicates that the page count has not been
   /// determined.
   /// </summary>
   /// <remarks>The page count is set internally by the document processing logic. The value may change if the
   /// document is modified or reloaded.</remarks>
   public int PageCount { get; internal set; } = -1;

   /// <summary>
   /// Gets the size of the file, in bytes.
   /// </summary>
   public long FileSizeInBytes { get; internal set; } = 0;

   /// <summary>
   /// Gets the title of the item. If the title is not set, returns the file name without its extension.
   /// </summary>
   /// <remarks>The title is automatically derived from the file name when it has not been explicitly
   /// specified. This allows for a default, meaningful title even when a custom title is not provided.</remarks>
   public string Title
   {
      get
      {
         return (string.IsNullOrEmpty(_Title) ? System.IO.Path.GetFileNameWithoutExtension(_FileName) : _Title);
      }
   }
   string _Title = string.Empty;


   /// <summary>
   /// Gets whether the Portable Document Format (PDF) document is password-protected.
   /// </summary>
   public bool IsPasswordProtected { get; internal set; } = false;

   // - - -   - - -  

   override public string ToString()
   {
      return $"PDFInfos: Title='{Title}', FileName='{FileName}', PageCount={PageCount}, FileSizeInBytes={FileSizeInBytes}, IsPasswordProtected={IsPasswordProtected}";
   }
}
