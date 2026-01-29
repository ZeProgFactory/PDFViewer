namespace ZPF.PDFViewer;


// All the code in this file is included in all platforms.
public static class PDFHelper
{
   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -

   public static float ToPT(float cm)
   {
#if WINDOWS
      return (float)(cm * 37.79736534);
#else
      // ANDROID || IOS || MACCATALYST
      return cm / 0.03527731092f;
#endif
   }
   public static float ToPT(double cm)
   {
      return ToPT((float)cm);
   }

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -

   public static float ToCM(float pt)
   {
#if WINDOWS
      return (float)(pt / 37.79736534);
#else
      // ANDROID || IOS || MACCATALYST 
      return pt * 0.03527731092f;
#endif
   }
   public static float ToCM(double pt)
   {
      return ToCM((float)pt);
   }

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -

   public enum PDFPageOrientations { Portrait = 0, Landscape = 90, FlippedPortrait = 180, FlippedLandscape = 270 }

   //public static System.Drawing.Rectangle GetPageSizeWithRotation(PDFPageInfo pageInfo)
   //{
   //   //ToDo: calc size based on orientation

   //   var pageSize = new System.Drawing.Rectangle(0, 0, (int)ToPT(pageInfo.Width), (int)ToPT(pageInfo.Height));

   //   return pageSize;
   //}

   // - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -  - -
}
