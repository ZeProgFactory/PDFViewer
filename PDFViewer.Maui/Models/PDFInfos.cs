namespace ZPF.PDFViewer;

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
