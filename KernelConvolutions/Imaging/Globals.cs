using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace KernelConvolutions.Imaging
{
    
    public struct Matrix3x3
    {
        private int _componentCount;
        public int ComponentCount { get => 9; private set => _componentCount = value; }
        private int[,] _matrix;

        public int this[int i, int k]
        {
            get => _matrix[i, k];
            set => _matrix[i, k] = value;
        }

    }

    public struct Vector2D
    {
        public double X;
        public double Y;
        public double Magnitude => Math.Sqrt(X * X + Y * Y);
    }

    public struct ImageInfo
    {
        public ImageInfo(string name, int width, int height)
        {
            Name = name;
            Width = width;
            Height = height;
        }
        public readonly string Name;
        public readonly int Width;
        public readonly int Height;
    }
    
}
