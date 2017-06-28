using EID.Library;
using EIDClient.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.ISS
{
    public class SimpleMovingAverage : List<decimal>
    {
        public SimpleMovingAverage(IList<decimal> values, int period)
        {
            Calculate(values.ToArray(), period);
        }

        public SimpleMovingAverage(IList<ICandle> candles, int period)
        {
            IList<ICandle> temp = candles.OrderBy(c => c.begin).ToList();

            Calculate(temp.Select(c => c.close).ToArray(), period);     
        }

        void Calculate(decimal[] data, int period)
        {
            this.Clear();

            IEnumerable<int> range = Enumerable.Range(0, data.Length - period);

            var result = range.Select(n => data.Skip(n).Take(period).Average());

            this.AddRange(result);
        }
    }
}
