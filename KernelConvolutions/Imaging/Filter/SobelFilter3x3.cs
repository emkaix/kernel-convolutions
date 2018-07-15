using System.Drawing;
using System.Threading;

namespace KernelConvolutions.Imaging.Filter
{
    public class SobelFilter3x3 : IConvolutionKernel
    {
        private readonly double[,] _kernelX;
        private readonly double[,] _kernelY;

        public SobelFilter3x3()
        {
            _kernelX = new double[,]
            {
                {-1, 0, 1},
                {-2, 0, 2},
                {-1, 0, 1}
            };
            _kernelY = new double[,]
            {
                {-1, -2, -1},
                {0, 0, 0},
                {1, 2, 1}
            };
        }


        public unsafe IConvolutable Convolute(IConvolutable img)
        {
            var derivativeX = new double[img.Info.Width, img.Info.Height, 3];
            var derivativeY = new double[img.Info.Width, img.Info.Height, 3];

            var grayScaleImg = img.ToGrayScale();
            var copy = new CImage((Bitmap) grayScaleImg.Image.Clone(), img.Info.Name);

            var t1 = new Thread(() => derivativeX = Algorithms.KernelConvolution(_kernelX, grayScaleImg));
            var t2 = new Thread(() => derivativeY = Algorithms.KernelConvolution(_kernelY, copy));

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            var bm = new Bitmap(img.Info.Width, img.Info.Height);
            byte* pPixels = bm.GetPointer(out var bmData);

            for (int x = 0; x < bm.Width; x++)
            {
                for (int y = 0; y < bm.Height; y++)
                {
                    byte* pPixelAtXY = pPixels + (y * bmData.Stride) + (x * 3);

                    var R = new Vector2D {X = derivativeX[x, y, 0], Y = derivativeY[x, y, 0]}.Magnitude;
                    var G = new Vector2D {X = derivativeX[x, y, 1], Y = derivativeY[x, y, 1]}.Magnitude;
                    var B = new Vector2D {X = derivativeX[x, y, 2], Y = derivativeY[x, y, 2]}.Magnitude;

                    *pPixelAtXY = (byte) (R > byte.MaxValue ? byte.MaxValue : R);
                    *(pPixelAtXY + 1) = (byte) (G > byte.MaxValue ? byte.MaxValue : G);
                    *(pPixelAtXY + 2) = (byte) (B > byte.MaxValue ? byte.MaxValue : B);
                }
            }

            bm.UnlockBits(bmData);
            return new CImage(bm, img.Info.Name);
        }
    }
}