using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace UserWallPaper.CustomControl
{
    class CustomSpc : StylusPointCollection
    {
        public CustomSpc()
        {
        }

        public Color Color
        {
            get;
            set;
        }

        public int FontSize
        {
            get;
            set;
        }
    }
}
