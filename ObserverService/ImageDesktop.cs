using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace ObserverService
{
    [ServiceContract]
    public interface IImageDesktop
    {
        [OperationContract]
        Bitmap GetImage();
    }

    class ImageDesktop : IImageDesktop
    {
        public Bitmap GetImage()
        {
            Bitmap ImageCamera = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);
            Graphics gfxScreenshot = Graphics.FromImage(ImageCamera);
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            return ImageCamera;
        }
    }
}
