using System;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using KernelConvolutions.Imaging;
using KernelConvolutions.Imaging.Filter;


namespace KernelConvolutions
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IConvolutable _image;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadImage_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                DefaultExt = ".jpg",
                Filter = "Bilddateien (.jpg)|*.png;*.bmp;*.jpg"
            };
            fileDialog.ShowDialog();

            var imagePath = fileDialog.FileName;
            _image = new CImage(imagePath);

            if (File.Exists(imagePath))
                ImageContainer.Source = new BitmapImage(new Uri(imagePath));
            else
                MessageBox.Show("Kein gültiger Pfad gewählt", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            if (_image == null) return;

            IConvolutionKernel kernel;

            switch (FilterMenu.Text)
            {
                case "Sobel 3x3":
                    kernel = new SobelFilter3x3();
                    break;
                case "Gauß 5x5":
                    kernel = new GaussFilter();
                    break;
                case "Mittelwert 3x3":
                    kernel = new MittelwertFilter3x3();
                    break;
                default:
                    kernel = null;
                    break;
            }

            if (kernel == null) return;

            using (var filteredImage = _image.Convolute(kernel))
            {
                var newImg = new BitmapImage();
                using (var ms = new MemoryStream())
                {
                    filteredImage.Image.Save(ms, ImageFormat.Bmp);
                    ms.Position = 0;
                    newImg.BeginInit();
                    newImg.CacheOption = BitmapCacheOption.OnLoad;
                    newImg.StreamSource = ms;
                    newImg.EndInit();
                }

                ImageContainer.Source = newImg;
            }
        }
    }
}