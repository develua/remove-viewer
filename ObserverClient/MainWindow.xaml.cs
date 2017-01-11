using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.DirectoryServices;
using System.Net.Sockets;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System;
using ObserverService;
using System.Threading;

namespace ObserverClient
{
    public partial class MainWindow : Window
    {
        bool IsWorked;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbIPAddress.Text = Dns.GetHostAddresses(Environment.MachineName).First(a => a.AddressFamily == AddressFamily.InterNetwork).ToString();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            IsWorked = !IsWorked;
            tbIPAddress.IsEnabled = tbSpeed.IsEnabled = !IsWorked;

            if (IsWorked)
                btnStartStop.Content = "Стоп";
            else
                btnStartStop.Content = "Старт";

            string ipAddress = tbIPAddress.Text;
            int speed = Convert.ToInt32(tbSpeed.Text);

            Task.Factory.StartNew(() =>
            {
                try
                {
                    using (ChannelFactory<IImageDesktop> factory = new ChannelFactory<IImageDesktop>(new NetTcpBinding { MaxReceivedMessageSize = Int32.MaxValue }, new EndpointAddress("net.tcp://" + ipAddress + ":8000/ObserverService/EndPoint")))
                    {
                        IImageDesktop imageDesktop = factory.CreateChannel();

                        while (IsWorked)
                        {
                            Bitmap image = imageDesktop.GetImage();
                            Dispatcher.Invoke(() => imgDesktopServic.Source = BitmapToBitmapImage(image));
                            Thread.Sleep(speed);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Dispatcher.Invoke(() =>
                    {
                        tbIPAddress.IsEnabled = tbSpeed.IsEnabled = !IsWorked;
                        btnStartStop.Content = "Старт";
                    });
                    IsWorked = false;
                }
            });
        }

        private BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Jpeg);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
    }
}
