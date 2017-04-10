using EIDClient.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EIDClient.Views.Controls;
using Windows.UI.Xaml;
using Windows.UI;
using EIDClient.Core.Entities;
using EIDClient.Core.ISS;
using EID.Library;

namespace EIDClient.Managers
{
    public class Chart : IChart
    {
        private ChartControl _control = null;

        private ChartControl GetControl()
        {
            return ChildFinder.FindChild<ChartControl>(Window.Current.Content, "chart_control");
        }

        public void ShowCandles(IList<ICandle> candleList)
        {
            this.GetControl().ShowCandles(candleList);
        }

        public void ShowMA(IList<decimal> ma, Color color)
        {
            this.GetControl().ShowMA(ma, color);
        }

        public void ShowMACD(MACD macd, Color color)
        {
            this.GetControl().ShowMACD(macd, color);
        }
    }
}
