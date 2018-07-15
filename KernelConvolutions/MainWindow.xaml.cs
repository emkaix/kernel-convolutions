using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
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
            if (imagePath.Equals(string.Empty)) return;

            _image = new CImage(imagePath);
            DisplayImageInfo(_image.Info);

            if (File.Exists(imagePath))
                imImageContainer.Source = new BitmapImage(new Uri(imagePath));
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
                    kernel = new GaussFilter7x7();
                    break;
                case "Mittelwert 3x3":
                    kernel = new MittelwertFilter3x3();
                    break;
                case "Laplace 3x3":
                    kernel = new LaplaceFilter3x3();
                    break;
                case "Sharpen 3x3":
                    kernel = new Sharpen3x3();
                    break;
                default:
                    kernel = null;
                    break;
            }

            if (kernel == null) return;

            using (var filteredImage = _image.Convolute(kernel))
            {
                imImageContainer.Source = filteredImage.ConvertToBitmapImage();
            }
        }

        private void DisplayImageInfo(ImageInfo info)
        {
            lbName.Content = info.Name;
            lbSize.Content = $"{info.Height}x{info.Width}";
            lbSize.Foreground = info.Height > 2000 || info.Width > 2000 ? Brushes.IndianRed : Brushes.Black;
        }
    }
}