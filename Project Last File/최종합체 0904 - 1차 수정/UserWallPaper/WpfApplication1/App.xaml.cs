using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.ServiceModel;
using System.Windows.Media.Imaging;
using Microsoft.WindowsAPICodePack.Taskbar;
using System.Windows.Interop;

namespace UserWallPaper
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Service service = new Service();
            GetUsbPath getusbpath = new GetUsbPath();
            Application.Current.Properties["USBDrivePath"] = getusbpath.GetUsbDriverName();

            ////if (e.Args.Length > 0)
            ////{
            ////    string serial = e.Args[0];

            //string serial = "AA010103131909207553";

            //if (service.Find(serial))
            //{
            //    Application.Current.Properties["Members"] = service.Join(serial);
            //    Application.Current.Properties["User"] = serial;

            //    string userinfo = service.FindUserInfo(serial);

            //    WindowsStyle window = new WindowsStyle(service, userinfo);
            //    window.Show();
            //}

            //else
            //{
            //    USB_Join usb_window = new USB_Join(service, serial);
            //    usb_window.Icon = new BitmapImage(new Uri(@"../../아이콘/HP-Flash-Drive.ico", UriKind.Relative));
            //    usb_window.Show();
            //}
            //}

            ////else
            ////{
            ////    USB_Login usb_login = new USB_Login(service);
            ////    usb_login.Icon = new BitmapImage(new Uri(@"../../아이콘/HP-Flash-Drive.ico", UriKind.Relative));
            ////    usb_login.Show();
            ////}

            if (e.Args.Length > 0)
            {
                string serial = e.Args[0];

                //string serial = "AA010103131909207553";

                if (service.Find(serial))
                {
                    Application.Current.Properties["Members"] = service.Join(serial);
                    Application.Current.Properties["User"] = serial;

                    string userinfo = service.FindUserInfo(serial);

                    WindowsStyle window = new WindowsStyle(service, userinfo);
                    window.Show();
                }

                else
                {
                    USB_Join usb_window = new USB_Join(service, serial);
                    //usb_window.Icon = new BitmapImage(new Uri(@"../../아이콘/HP-Flash-Drive.ico", UriKind.Relative));
                    usb_window.Show();
                }
            }

            else
            {
                USB_Login usb_login = new USB_Login(service);
                usb_login.Show();
            }
        }
    }
}
