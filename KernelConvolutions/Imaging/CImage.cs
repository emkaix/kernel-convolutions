using System;
using System.Drawing;
using System.IO;
using KernelConvolutions;

namespace KernelConvolutions.Imaging
{
    public class CImage : IConvolutable
    {
        private readonly Bitmap _image;
        private readonly ImageInfo _info;

        public Bitmap Image => _image;
        public ImageInfo Info => _info;

        public IConvolutable Convolute(IConvolutionKernel kernel)
        {
            return kernel.Convolute(this);
        }

        public CImage(Bitmap img, string name)
        {
            this._image = img ?? throw new ArgumentNullException();
            this._info = new ImageInfo(name, img.Width, img.Height);
        }

        public CImage(string path) : this(LoadFromPath(path), Path.GetFileName(path)) { }
        private static Bitmap LoadFromPath(string path) => File.Exists(path) ? new Bitmap(path) : null;
        public IConvolutable ToGrayScale() => new CImage(_image?.ToGrayScale(), _info.Name);
        
        public void Dispose() => _image?.Dispose();
    }
}
