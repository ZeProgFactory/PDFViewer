using System.Text.Json.Serialization;

namespace ZPF.PDFViewer;

public delegate void OnNeedDataEventHandler(object sender );

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
         if( string.IsNullOrEmpty(_ImageFileName))
         {
            if( OnNeedData != null)
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
         };
      }
   }
   bool _IsCurrentPage = false;

   [JsonIgnore]
   public Color SelectedColor { get => (IsCurrentPage ? Colors.Red : Colors.Transparent); }

   // - - -  - - - 

   public static event OnNeedDataEventHandler OnNeedData;

   // - - -  - - - 

   public override string ToString()
   {
      return $"{(IsCurrentPage ? "¤" : " ")}{PageNumber}";
   }
}
