using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using Emgu.CV;
using Emgu.CV.Structure;

namespace CameraPreview
{
    internal class PreviewImage
    {
        public Image Control { get; private set; }
        private readonly WriteableBitmap imageSource;

        int w, h;
        public PreviewImage(int w, int h)
        {
            this.w = w;
            this.h = h;
            Control = new Image();
            imageSource = new WriteableBitmap(w, h, 96, 96, PixelFormats.Bgr24, null);
            Control.Source = imageSource;
        }

        public void Render(Image<Bgr, byte> imageCV)
        {
            try
            {
                if (!Control.Dispatcher.CheckAccess())
                {
                    Control.Dispatcher.Invoke(() =>
                    {
                        Render(imageCV);
                    });

                    return;
                }
                using var newImg = imageCV.Resize(w, h, Emgu.CV.CvEnum.Inter.Linear);
                imageSource.Lock();
                imageSource?.WritePixels(new Int32Rect(0, 0, w, h), newImg.MIplImage.ImageData, newImg.MIplImage.ImageSize, newImg.MIplImage.WidthStep);
                imageSource.Unlock();
            }
            catch
            {
            }
        }

        public Image GetControl()
        {
            return Control;
        }
    }
}
