using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows;

namespace UserWallPaper
{
    public static class MyAnimation
    {
        static public void MenuAnimation(DoubleAnimation animation, ListBox lb, DependencyProperty dp, int size)
        {
            animation.To = size;

            lb.BeginAnimation(dp, animation, HandoffBehavior.Compose);
        }

        static public void GridAnimation(DoubleAnimation animation, Grid grid, DependencyProperty dp, int size)
        {
            animation.To = size;

            grid.BeginAnimation(dp, animation, HandoffBehavior.Compose);
        }
    }
}
