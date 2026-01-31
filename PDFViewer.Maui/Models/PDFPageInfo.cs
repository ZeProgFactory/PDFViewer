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


   [JsonIgnore]
   public ImageSource ImageSource 
   {
      get
      {
         if (_ImageSource == null)
         {
            if (OnNeedData != null)
            {
               OnNeedData(this);
            }
         }

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
   public int RealWidth { get; internal set; }
   [JsonIgnore]
   public int RealHeight { get; internal set; }

   [JsonIgnore]
   public double WidthRequest => (RealWidth * Scale) + 20;
   [JsonIgnore]
   public double HeightRequest => (RealHeight * Scale) + 20;


   public PDFHelper.PDFPageOrientations Rotation { get; internal set; }


   /// <summary>
   /// Gets or sets the scale factor applied to the element. 
   /// 
   /// Remarks:
   ///    Default value is 1.0.
   /// </summary>
   [JsonIgnore]

   public double Scale
   {
      get => _Scale;
      set
      {
         if (SetField(ref _Scale, value))
         {
            OnPropertyChanged("WidthRequest");
            OnPropertyChanged("HeightRequest");
         }
      }
   }
   double _Scale = 1.0;

   // - - -  - - - 

   public void SetValues(PDFPageInfo pDFPageInfo)
   {
      if( pDFPageInfo == null)
      {
         return;
      }

      this._PageNumber = pDFPageInfo.PageNumber;
      this._IsCurrentPage = pDFPageInfo.IsCurrentPage;

      this.Width = pDFPageInfo.Width;
      this.Height = pDFPageInfo.Height;

      this.RealWidth = pDFPageInfo.RealWidth;
      this.RealHeight = pDFPageInfo.RealHeight;

      this.Scale = pDFPageInfo.Scale;

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
