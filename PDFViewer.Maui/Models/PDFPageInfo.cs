using System.Text.Json.Serialization;
using ZPF.PDFViewer.Events;

namespace ZPF.PDFViewer;

//ToDo: observeableobject

/// <summary>
/// Represents information about a single page in a PDF document, including its dimensions, image source, and display
/// status.
/// </summary>
/// <remarks>This class provides properties for managing the page number, width and height in centimeters, image
/// file name, and image source for rendering. It also tracks whether the page is currently selected and supports
/// updating its values from another instance. The class is designed for use in PDF viewing scenarios where page
/// metadata and visual representation are required.</remarks>
public class PDFPageInfo : BaseViewModel<PDFPageInfo>
{
   public int PageNumber
   {
      get => _PageNumber;
      set => _PageNumber = value;
   }
   int _PageNumber = -1;

   /// <summary>
   /// Width in cm
   /// </summary>
   public double Width { get; set; } = -1;

   /// <summary>
   /// Height in cm
   /// </summary>
   public double Height { get; set; } = -1;

   public string ImageFileName
   {
      get
      {
#if VIA_FILE 
         if (string.IsNullOrEmpty(_ImageFileName))
         {
            if (OnNeedData != null)
            {
               OnNeedData(this);
            }
         }
#endif

         return _ImageFileName;
      }
      set => SetField(ref _ImageFileName, value);
   }
   string _ImageFileName = "";


   /// <summary>
   /// 
   /// </summary>
   [JsonIgnore]
   public ImageSource ImageSource 
   {
      get
      {
#if ! VIA_FILE
         if (_ImageSource == null)
         {
            if (OnNeedData != null)
            {
               OnNeedData(this);
            }
         }
#endif

         return _ImageSource;
      }
      set => SetField(ref _ImageSource, value); 
   }
   ImageSource _ImageSource = null;


   [JsonIgnore]
   public bool IsCurrentPage
   {
      get => _IsCurrentPage;
      set
      {
         if (SetField(ref _IsCurrentPage, value))
         {
            OnPropertyChanged("SelectedColor");
         }
      }
   }
   bool _IsCurrentPage = false;

   [JsonIgnore]
   public Color SelectedColor { get => (IsCurrentPage ? Colors.Red : Colors.Transparent); }


   [JsonIgnore]
   public double WidthRequest { get => _WidthRequest; set => SetField(ref _WidthRequest, value); }
   double _WidthRequest = -1;


   [JsonIgnore]
   public double HeightRequest { get => _HeightRequest; set => SetField(ref _HeightRequest, value); }
   double _HeightRequest = -1;


   public PDFHelper.PDFPageOrientations Rotation { get; internal set; }


   /// <summary>
   /// Gets or sets the scale factor applied to the element. 
   /// 
   /// Remarks:
   ///    Default value is 1.0.
   /// </summary>
   [JsonIgnore]

   public double Scale { get => _Scale; set { _Scale = value; OnPropertyChanged(); } }

   double _Scale = 1.0;

   // - - -  - - - 

   public void SetValues(PDFPageInfo pDFPageInfo)
   {
      this._PageNumber = pDFPageInfo.PageNumber;
      this._IsCurrentPage = pDFPageInfo.IsCurrentPage;

      this.Width = pDFPageInfo.Width;
      this.Height = pDFPageInfo.Height;

      this.HeightRequest = pDFPageInfo.HeightRequest;
      this.WidthRequest = pDFPageInfo.WidthRequest;

      this.Scale = pDFPageInfo.Scale;

      this.ImageFileName = pDFPageInfo.ImageFileName;
      this.ImageSource = pDFPageInfo.ImageSource;  
   }

   // - - -  - - - 

   public static event OnNeedDataEventHandler OnNeedData;

   // - - -  - - - 

   public override string ToString()
   {
      return $"{(IsCurrentPage ? "¤" : " ")}{PageNumber}";
   }

}
