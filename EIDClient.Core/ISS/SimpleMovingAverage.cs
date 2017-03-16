using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.ISS
{
    public class SimpleMovingAverage
    {
        public SimpleMovingAverage(IList<Candle> candles, int period)
        {
            IList<decimal> result = new List<decimal>();

            IList<Candle> temp = candles.OrderByDescending(c => c.begin).ToList();

            IEnumerable<int> range = Enumerable.Range(0, candles.Count - period);

            result = range.Select(n => temp.Skip(n).Take(period).Select(c => c.close).Average()).ToList();
        }
    }
}
