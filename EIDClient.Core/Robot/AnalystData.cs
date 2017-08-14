using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace EIDClient.Core.Robot
{
    public class AnalystData : ViewModelBase
    {
        public string Sec { get; set; }

        private string _advice = null;
        public string Advice
        {
            get
            {
                return _advice;
            }
            set
            {
                _advice = value;

                this.RaisePropertyChanged("Advice");
                this.RaisePropertyChanged("ForegroundColor");
            }
        }
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
