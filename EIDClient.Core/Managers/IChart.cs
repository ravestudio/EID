using EIDClient.Core.Entities;
using EIDClient.Core.ISS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace EIDClient.Core.Managers
{
    public interface IChart
    {
        void ShowCandles(IList<Candle> candleList);
        void ShowMA(IList<decimal> ma, Color color);
        void ShowMACD(MACD macd, Color color);
    }
}
