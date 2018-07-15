using System;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace KernelConvolutions.Imaging
{
    public interface IConvolutable : IDisposable
    {
        IConvolutable Convolute(IConvolutionKernel kernel);
        IConvolutable ToGrayScale();
        BitmapImage ConvertToBitmapImage();
        Bitmap Image { get; }
        ImageInfo Info { get; }
    }
}
