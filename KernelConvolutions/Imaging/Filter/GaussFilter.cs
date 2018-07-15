using System.Drawing;
using System.Drawing.Imaging;

namespace KernelConvolutions.Imaging.Filter
{
    public class GaussFilter : IConvolutionKernel
    {
        private readonly double[,] _kernel;

        public GaussFilter()
        {
            _kernel = new double[,]
            {
                {0.111108d, 0.111113d, 0.111108d},
                {0.111113d, 0.111118d, 0.111113d},
                {0.111108d, 0.111113d, 0.111108d}
            };
        }

        public unsafe IConvolutable Convolute(IConvolutable img)
        {
            var filtered = Algorithms.KernelConvolution(_kernel, img);

            var bm = new Bitmap(img.Info.Width, img.Info.Height);
            byte* pPixels = bm.GetPointer(out var bmData);

            for (int x = 0; x < bm.Width; x++)
            {
                for (int y = 0; y < bm.Height; y++)
                {
                    byte* pPixelAtXY = pPixels + (y * bmData.Stride) + (x * 3);
                    *pPixelAtXY = (byte)(filtered[x, y, 0] > byte.MaxValue ? byte.MaxValue : filtered[x, y, 0]);
                    *(pPixelAtXY + 1) = (byte)(filtered[x, y, 1] > byte.MaxValue ? byte.MaxValue : filtered[x, y, 1]);
                    *(pPixelAtXY + 2) = (byte)(filtered[x, y, 2] > byte.MaxValue ? byte.MaxValue : filtered[x, y, 2]);
                }
            }
            bm.UnlockBits(bmData);
            return new CImage(bm, img.Info.Name);
        }
    }
}