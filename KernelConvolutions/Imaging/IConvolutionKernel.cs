namespace KernelConvolutions.Imaging
{
    public interface IConvolutionKernel
    {
        IConvolutable Convolute(IConvolutable img);
    }
}
