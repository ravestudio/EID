using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace EIDClient.Core.Robot
{
    public class AnalystData
    {
        public string Sec { get; set; }
        public string Advice { get; set; }
        public Brush ForegroundColor
        {
            get
            {
                Brush brush = new SolidColorBrush(Colors.White);

                if (Advice == "open long")
                {
                    brush = new SolidColorBrush(Colors.Green);
                }

                if (Advice == "open short")
                {
                    brush = new SolidColorBrush(Colors.Red);
                }

                return brush;
            }
        }
    }
}
