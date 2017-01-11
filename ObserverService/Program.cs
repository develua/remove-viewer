using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;

namespace ObserverService
{
    class Program
    {
        static void Main(string[] args)
        {
            string ipAdress = Dns.GetHostAddresses(Environment.MachineName).First(a => a.AddressFamily == AddressFamily.InterNetwork).ToString();
            
            ServiceHost sh = new ServiceHost(typeof(ImageDesktop));
            sh.AddServiceEndpoint(typeof(IImageDesktop), new NetTcpBinding(), "net.tcp://" + ipAdress + ":8000/ObserverService/EndPoint");

            sh.Open();
            Console.WriteLine("Служба {0} запущена в {1}\nIP-Адрес сервиса {2}", sh.Description.Name, DateTime.Now.ToString(), ipAdress);
            Console.WriteLine("Для остановки службы нажмите любую клавишу...");
            Console.ReadKey();
            sh.Close();
        }
    }
}
