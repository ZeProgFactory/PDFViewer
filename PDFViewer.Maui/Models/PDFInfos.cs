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
    public string FileName
    {
        get { return _FileName; }
        set { _FileName = value; }
    }
    string _FileName = string.Empty;


    public int PageCount { get; set; } = -1;
    public int PageIndex { get; set; } = -1;

    public long FileSizeInBytes { get; set; } = 0;


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
        return $"PDFInfos: Title='{Title}', FileName='{FileName}', PageCount={PageCount}, PageIndex={PageIndex}, FileSizeInBytes={FileSizeInBytes}, IsPasswordProtected={IsPasswordProtected}";
    }
}
