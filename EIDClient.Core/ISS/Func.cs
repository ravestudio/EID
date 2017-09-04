using EID.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.ISS
{
    public static class Func
    {
        public static decimal Max(IList<ICandle> candles, int period, int prev)
        {
            IList<ICandle> temp = candles.Skip(candles.Count - period).Take(period - prev).ToList();

            return temp.Select(c => c.close).Max();
        }

        public static decimal Min(IList<ICandle> candles, int period, int prev)
        {
            IList<ICandle> temp = candles.Skip(candles.Count - period).Take(period - prev).ToList();

            return temp.Select(c => c.close).Min();
        }

        public static bool Green(ICandle candle)
        {
            return candle.close > candle.open;
        }

        public static bool Red(ICandle candle)
        {
            return candle.open > candle.close;
        }

        public static bool Big(ICandle candle)
        {
            decimal avg = (candle.open + candle.close) / 2;

            decimal v2 = Math.Abs(candle.open - candle.close);

            decimal p = avg * 0.0015m;

            return v2 > p;
        }

    }
}
