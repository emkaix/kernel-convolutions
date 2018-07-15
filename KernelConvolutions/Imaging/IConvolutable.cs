using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KernelConvolutions;

namespace KernelConvolutions.Imaging
{
    public interface IConvolutable : IDisposable
    {
        IConvolutable Convolute(IConvolutionKernel kernel);
        IConvolutable ToGrayScale();
        Bitmap Image { get; }
        ImageInfo Info { get; }
    }
}
