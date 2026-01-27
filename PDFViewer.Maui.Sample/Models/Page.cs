namespace ZPF.PDFViewer;

public class Page : ObservableObject
{
   public ImageSource ImageSource { get; set; }

   public double Scale 
   { 
      get => _Scale; 
      set 
      { 
         if( SetField(ref _Scale, value) )
         {
            OnPropertyChanged("WidthRequest");
            OnPropertyChanged("HeightRequest");
         }
      }
   }
   double _Scale = 1.0;

   public int RealWidth { get; internal set; }
   public int RealHeight { get; internal set; }

   public double WidthRequest => (RealWidth * Scale) + 20;
   public double HeightRequest => (RealHeight * Scale) + 20;

};
