using System.Text.Json.Serialization;

namespace ZPF.PDFViewer;

public delegate void OnNeedDataEventHandler(object sender);

//ToDo: observeableobject
public class PDFPageInfo : BaseViewModel<PDFPageInfo>
{
   public int PageNumber { get; set; } = -1;

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
         if (string.IsNullOrEmpty(_ImageFileName))
         {
            if (OnNeedData != null)
            {
               OnNeedData(this);
            }

            return $"page_{PageNumber}.png";
         }

         return _ImageFileName;
      }
      set => SetField(ref _ImageFileName, value);
   }
   string _ImageFileName = "";

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

   public double Scale { get => _Scale; set => _Scale = value; }
   double _Scale = 1.0;

   // - - -  - - - 

   public void SetValues(PDFPageInfo pDFPageInfo)
   {
      this.PageNumber = pDFPageInfo.PageNumber;
      this.Width = pDFPageInfo.Width;
      this.Height = pDFPageInfo.Height;
      this.ImageFileName = pDFPageInfo.ImageFileName;
      this.IsCurrentPage = pDFPageInfo.IsCurrentPage;
   }

   // - - -  - - - 

   public static event OnNeedDataEventHandler OnNeedData;

   // - - -  - - - 

   public override string ToString()
   {
      return $"{(IsCurrentPage ? "¤" : " ")}{PageNumber}";
   }

}
