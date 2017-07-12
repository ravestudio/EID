using EID.Library;
using EIDClient.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.ISS
{
    public class MACD : List<MACDItem>
    {
        public MACD(IList<MACDItem> items)
        {
            this.AddRange(items);
        }

        public MACD(IList<ICandle> candles, int shortPeriod, int longPeriod, int signalPeriod)
        {
            this.Clear();

            ExponentialMovingAverage ema_short = new ISS.ExponentialMovingAverage(candles, shortPeriod);
            ExponentialMovingAverage ema_long = new ExponentialMovingAverage(candles, longPeriod);

            int d = ema_short.Count - ema_long.Count;

            int i = 0;
            foreach(decimal p in ema_long)
            {
                this.Add(new ISS.MACDItem() { MACD = ema_short[d + i] - ema_long[i] });
                i++;
            }

            ExponentialMovingAverage signal = new ExponentialMovingAverage(this.Select(m => m.MACD).ToArray(), signalPeriod);

            d = this.Count - signal.Count;

            i = 0;
            foreach(decimal p in signal)
            {
                this[d + i].Signal = p;
                i++;
            }

            this.RemoveRange(0, d);

            this.ForEach(m => m.Histogram = m.MACD - m.Signal);

        }
    }

    public class MACDItem
    {
        public decimal MACD { get; set; }
        public decimal Signal { get; set; }
        public decimal Histogram { get; set; }
    }
}
