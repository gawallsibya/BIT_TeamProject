using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace UserWallPaper
{
    class PopupChildEnable
    {
        [DllImport("User32.dll")]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        public static IntPtr GetHwnd(System.Windows.Controls.Primitives.Popup popup)
        {
            HwndSource source = (HwndSource)PresentationSource.FromVisual(popup.Child);
            return source.Handle;
        }
    }     
}
