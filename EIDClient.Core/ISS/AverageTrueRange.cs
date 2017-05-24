using EID.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.ISS
{
    public class AverageTrueRange : List<decimal>
    {

        public AverageTrueRange(IList<ICandle> candles, int period)
        {
            this.Clear();

            IEnumerable<int> range = Enumerable.Range(1, candles.Count - 1);

            var t = range.ToList();

            var TRlist = t.Select(n => TR(candles, n)).ToList();

            SimpleMovingAverage sma = new SimpleMovingAverage(TRlist, period);

            this.AddRange(sma);
        }

        private decimal TR(IList<ICandle> candles, int n)
        {
            decimal res = 0;

            decimal tr1 = candles[n].high - candles[n].low;

            decimal tr2 = Math.Abs(candles[n].high - candles[n - 1].close);

            decimal tr3 = Math.Abs(candles[n].low - candles[n - 1].close);

            res = Math.Max(Math.Max(tr1, tr2), tr3);

            return res;
        }
    }
}
