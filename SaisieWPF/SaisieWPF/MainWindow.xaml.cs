using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SaisieWPF
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>        

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }

        private void MainForm_Loaded(object sender, RoutedEventArgs e)
        {
            IM_Premiere.Source = BitmapToImageSource(SaisieWPF.Properties.Resources.image_a_venir);
        }
    }

}

