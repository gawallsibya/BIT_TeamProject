using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UserWallPaper
{
    /// <summary>
    /// IconOnCanvas.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class IconOnCanvas : UserControl
    {
        public int ThisListIndex
        {
            get;
            set;
        }

        public IconOnCanvas()
        {
            InitializeComponent();
        }

        public void ChangeTextBackgroundColor()
        {
            switch (Icontxtblock.Text)
            {
                case "pdf": Icontxtblock.Background = new BrushConverter().ConvertFrom("#FFFF4343") as Brush; break;
                case "ppt": Icontxtblock.Background = new BrushConverter().ConvertFrom("#FFFF7F43") as Brush; break;
                case "doc":
                case "docx": Icontxtblock.Background = new BrushConverter().ConvertFrom("#FF3973C7") as Brush; break;
                case "hwp": Icontxtblock.Background = new BrushConverter().ConvertFrom("#FF4FB3EC") as Brush; break;
                case "xls": Icontxtblock.Background = new BrushConverter().ConvertFrom("#FF47B44C") as Brush; break;
            }
        }
    }
}
