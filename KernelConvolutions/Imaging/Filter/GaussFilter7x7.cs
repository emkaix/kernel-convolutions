using System.Drawing;

namespace KernelConvolutions.Imaging.Filter
{
    public class GaussFilter7x7 : IConvolutionKernel
    {
        private readonly double[,] _kernel;

        public GaussFilter7x7()
        {
            _kernel = new[,]
            {
                {0.011362d, 0.014962d, 0.017649d, 0.018648d, 0.017649d, 0.014962d, 0.011362d},
                {0.014962d, 0.019703d, 0.02324d, 0.024556d, 0.02324d, 0.019703d, 0.014962d},
                {0.017649d, 0.02324d, 0.027413d, 0.028964d, 0.027413d, 0.02324d, 0.017649d},
                {0.018648d, 0.024556d, 0.028964d, 0.030603d, 0.028964d, 0.024556d, 0.018648d},
                {0.017649d, 0.02324d, 0.027413d, 0.028964d, 0.027413d, 0.02324d, 0.017649d},
                {0.014962d, 0.019703d, 0.02324d, 0.024556d, 0.02324d, 0.019703d, 0.014962d},
                {0.011362d, 0.014962d, 0.017649d, 0.018648d, 0.017649d, 0.014962d, 0.011362d}
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
                    *pPixelAtXY = (byte) (filtered[x, y, 0] > byte.MaxValue ? byte.MaxValue : filtered[x, y, 0]);
                    *(pPixelAtXY + 1) = (byte) (filtered[x, y, 1] > byte.MaxValue ? byte.MaxValue : filtered[x, y, 1]);
                    *(pPixelAtXY + 2) = (byte) (filtered[x, y, 2] > byte.MaxValue ? byte.MaxValue : filtered[x, y, 2]);
                }
            }
            bm.UnlockBits(bmData);
            return new CImage(bm, img.Info.Name);
        }
    }
}