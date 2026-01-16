namespace ZPF.PDFViewer.Events;

public class PageChangedEventArgs
{
    public PageChangedEventArgs(int currentPage)
    {
        CurrentPage = currentPage;
    }

    public int CurrentPage { get; }
}
