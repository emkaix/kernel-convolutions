using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using KernelConvolutions;

namespace KernelConvolutions.Imaging
{
    public static class Algorithms
    {
        public static unsafe double[,,] KernelConvolution(double[,] kernelMatrix, IConvolutable img)
        {
            var bm = img.Image;
            if (bm == null) return null;
            var halfSize = (int)Math.Sqrt(kernelMatrix.Length) / 2;
            var ret = new double[bm.Width, bm.Height, 3];

            var bmData = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            var ptr = bmData.Scan0;
            var pPixels = (byte*)ptr.ToPointer();

            for (int x = 0; x < bm.Width; x++)
            {
                for (int y = 0; y < bm.Height; y++)
                {
                    double valR = 0;
                    double valG = 0;
                    double valB = 0;
                    

                    for (int k = x - halfSize, a = 0; k <= x + halfSize; k++, a++)
                    {
                        for (int l = y - halfSize, b = 0; l <= y + halfSize; l++, b++)
                        {
                            byte* R;
                            byte* G;
                            byte* B;

                            if (k < 0 || l < 0 || k >= bm.Width || l >= bm.Height)
                            {
                                byte zero = 0;
                                R = G = B = &zero;
                            }
                            else
                            {
                                var pPixelAtXY = pPixels + (l * bmData.Stride) + (k * 3);
                                R = pPixelAtXY;
                                G = pPixelAtXY + 1;
                                B = pPixelAtXY + 2;
                                
                            }
                            valR += *R * kernelMatrix[a, b];
                            valG += *G * kernelMatrix[a, b];
                            valB += *B * kernelMatrix[a, b];
                        }
                    }

                    ret[x, y, 0] = valR;
                    ret[x, y, 1] = valG;
                    ret[x, y, 2] = valB;
                }
            }
            bm.UnlockBits(bmData);
            return ret;
        }
    }
}