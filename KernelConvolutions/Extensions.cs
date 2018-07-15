using System.Drawing;
using System.Drawing.Imaging;

namespace KernelConvolutions
{
    public static class Extensions
    {
        public static unsafe Bitmap ToGrayScale(this Bitmap bmOld)
        {
            var bmNew = new Bitmap(bmOld.Width, bmOld.Height);
            var pOld = bmOld.GetPointer(out var bmDataOld);
            var pNew = bmNew.GetPointer(out var bmDataNew);

            for (int x = 0; x < bmOld.Width; x++)
            {
                for (int y = 0; y < bmOld.Height; y++)
                {
                    var pPixelOld = pOld + (y * bmDataOld.Stride) + (x * 3);
                    var pPixelNew = pNew + (y * bmDataNew.Stride) + (x * 3);

                    var grayScale = (byte)(*pPixelOld * 0.3f + *(pPixelOld + 1) * 0.59f + *(pPixelOld + 2) * 0.11f);
                    *pPixelNew = *(pPixelNew + 1) = *(pPixelNew + 2) = grayScale;
                }
            }
            bmOld.UnlockBits(bmDataOld);
            bmNew.UnlockBits(bmDataNew);
            return bmNew;
        }

        public static unsafe byte* GetPointer(this Bitmap bm, out BitmapData bmData)
        {
            bmData = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);
            var ptr = bmData.Scan0;
            return (byte*)ptr.ToPointer();
        }
        
    }
}