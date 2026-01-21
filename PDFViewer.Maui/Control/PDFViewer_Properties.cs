namespace ZPF.PDFViewer.Maui;

public partial class PDFViewer
{
   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 

   public int CurrentPageNumber
   {
      get => _CurrentPageNumber;
      internal set
      {
         //ToDo: SetValue
         _CurrentPageNumber = value;

         OnPropertyChanged("CurrentPageNumber");
      }
   }
   int _CurrentPageNumber = 0;
   

   /// <summary>
   /// Gets or sets the collection of pages contained in the PDF document.
   /// </summary>
   /// <remarks>Each element in the collection represents information about a single page, encapsulated in a
   /// PDFPageInfo instance. Modifying this collection updates the set of pages available in the document in memory.</remarks>
   public List<PDFPageInfo> Pages
   {
      get => _Pages; 
      set => _Pages = value;
   }
   List<PDFPageInfo> _Pages = new List<PDFPageInfo>();


   /// <summary>
   /// Gets a value indicating whether the component is currently performing a background operation.
   /// </summary>
   public bool IsBusy
   {
      get => _IsBusy;
      internal set
      {
         //ToDo: SetValue
         _IsBusy = value;

         OnPropertyChanged("IsBusy");
      }
   }
   bool _IsBusy = false;


   /// <summary>
   /// Gets or sets a value indicating whether the toolbar is visible.  
   /// </summary>
   /// <remarks>Changing this property raises the property changed notification, allowing data bindings or
   /// listeners to respond to visibility changes.</remarks>
   public bool IsToolbarVisible
   {
      get => _IsToolbarVisible;
      set
      {
         _IsToolbarVisible = value;

         OnPropertyChanged("IsToolbarVisible");
      }
   }
   bool _IsToolbarVisible = true;

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 
}
