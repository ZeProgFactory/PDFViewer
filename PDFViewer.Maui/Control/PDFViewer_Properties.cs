using System.Collections.ObjectModel;

namespace ZPF.PDFViewer.Maui;

public partial class PDFViewer
{
   public string LastMessage { get => _LastMessage; internal set => _LastMessage = value; }
   string _LastMessage = "";

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 

   /// <summary>
   /// Gets the current page number in the pagination system.
   /// </summary>
   /// <remarks>This property is updated internally and raises a property change notification when its value
   /// changes. It is intended for read-only access from outside the class.</remarks>
   public int CurrentPageNumber
   {
      get => _CurrentPageNumber;
      internal set
      {
         //ToDo: SetValue
         //ToDo: set page index to navigate through pages

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
   public ObservableCollection<PDFPageInfo> Pages
   {
      get => _Pages;
      set => _Pages = value;
   }
   ObservableCollection<PDFPageInfo> _Pages = new ObservableCollection<PDFPageInfo>();


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


   /// <summary>
   /// Gets the PDF information associated with the document.
   /// </summary>
   /// <remarks>This property is set internally and is not intended to be modified directly by consumers.
   /// Changes to this property raise the PropertyChanged event, allowing subscribers to be notified when the PDF
   /// information is updated.</remarks>
   public PDFInfos Infos
   {
      get => _PDFInfos;
      internal set
      {
         _PDFInfos = value;

         OnPropertyChanged("Infos");
      }
   }

   private PDFInfos _PDFInfos = new PDFInfos();

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 
}
