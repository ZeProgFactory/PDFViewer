namespace ZPF.PDFViewer.Maui;

public partial class PDFViewer
{
   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 

   /// <summary>
   /// Current rotation of the displayed pages in degrees (0, 90, 180 or 270).
   /// </summary>
   public double PageRotation
   {
      get => _PageRotation;
      set => DoRotate(value);
   }
   double _PageRotation = 0;

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 

   /// <summary>
   /// Rotates all pages by 90 degrees clockwise.
   /// </summary>
   public void RotateClockwise() => DoRotate(_PageRotation + 90);

   /// <summary>
   /// Rotates all pages by 90 degrees counter clockwise.
   /// </summary>
   public void RotateCounterClockwise() => DoRotate(_PageRotation - 90);

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 

   /// <summary>
   /// Applies the given rotation (normalized to 0, 90, 180 or 270) to all pages.
   /// </summary>
   public void DoRotate(double rotation)
   {
      _PageRotation = ((rotation % 360) + 360) % 360;

      foreach (var p in Pages)
      {
         p.DisplayRotation = _PageRotation;
      }

      OnPropertyChanged(nameof(PageRotation));
   }

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 

   private void btnRotate_Clicked(object sender, EventArgs e)
   {
      RotateClockwise();
   }

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - 
}
