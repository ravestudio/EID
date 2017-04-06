using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.ISS
{
    public class CandlesConverter
    {
        public IList<Candle> Convert(IList<Candle> candles, int sourceTimeframe, int TargetTimeframe)
        {
            IList<Candle> result = null;

            int interval = TargetTimeframe / sourceTimeframe;

            result = Enumerable.Range(0, candles.Count / interval).Select(n => CreateCandle(candles, n, interval)).ToList();

            return result;
        }

        private Candle CreateCandle(IList<Candle> candles, int n, int interval)
        {
            var temp = candles.Skip(n*interval).Take(interval);

            Candle candle = new Candle()
            {
                begin = temp.First().begin,
                open = temp.First().open,
                close = temp.Last().close,
                high = temp.Max(c => c.high)
            };

            return candle;
        }
    }
}
