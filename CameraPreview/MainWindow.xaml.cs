using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CameraPreview
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VideoCapture capture;
        private readonly PreviewImage previewImage;
        public MainWindow()
        {
            InitializeComponent();
            previewImage = new PreviewImage(1920, 1080);
            ImgContainer.Children.Add(previewImage.GetControl());
            InitDevs();
        }
        private void InitDevs()
        {
            capture = new VideoCapture(0,VideoCapture.API.Ffmpeg);
            capture.SetCaptureProperty(CapProp.FrameWidth, 1920);
            capture.SetCaptureProperty(CapProp.FrameHeight, 1080);
            Preview();
            capture.Start();
        }

        /// <summary>
        /// Previews on UI.
        /// </summary>
        private void Preview()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var isGrabed = capture.Grab();
                    if (isGrabed)
                    {
                        var image = new Image<Bgr, byte>(1920, 1080);
                        var isRetrieved = capture.Retrieve(image);
                        if (isRetrieved)
                        {
                            previewImage.Render(image);
                        }
                        else
                        {
                            image.Dispose();
                        }
                    }

                    Thread.Sleep(40);
                }
            });
        }

    }
}
