using System.Drawing;

namespace KernelConvolutions.Imaging.Filter
{
    public class LaplaceFilter3x3 : IConvolutionKernel
    {
        private readonly double[,] _kernel;

        public LaplaceFilter3x3()
        {
            _kernel = new[,]
            {
                {0d, 1d, 0d},
                {1d, -4d, 1d},
                {0d, 1d, 0d}
            };
        }

        public unsafe IConvolutable Convolute(IConvolutable img)
        {
            var grayScaleImg = img.ToGrayScale();
            var filtered = Algorithms.KernelConvolution(_kernel, grayScaleImg);

            var bm = new Bitmap(img.Info.Width, img.Info.Height);
            byte* pPixels = bm.GetPointer(out var bmData);

            for (int x = 0; x < bm.Width; x++)
            {
                for (int y = 0; y < bm.Height; y++)
                {
                    byte* pPixelAtXY = pPixels + (y * bmData.Stride) + (x * 3);
                    *pPixelAtXY = (byte) ((byte)(filtered[x, y, 0] > byte.MaxValue ? byte.MaxValue : filtered[x, y, 0]) + 127);
                    *(pPixelAtXY + 1) = (byte) ((byte)(filtered[x, y, 1] > byte.MaxValue ? byte.MaxValue : filtered[x, y, 1]) + 127);
                    *(pPixelAtXY + 2) = (byte) ((byte)(filtered[x, y, 2] > byte.MaxValue ? byte.MaxValue : filtered[x, y, 2]) + 127);
                }
            }
            bm.UnlockBits(bmData);
            return new CImage(bm, img.Info.Name);
        }
    }
}