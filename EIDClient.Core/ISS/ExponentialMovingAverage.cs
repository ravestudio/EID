using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.ISS
{
    public class ExponentialMovingAverage
    {
        public ExponentialMovingAverage(IList<Candle> candles, int period)
        {
            IList<decimal> result = new List<decimal>();

            int diff = candles.Count-period;

            decimal[] newdata = candles.Take(period).Select(c => c.close).ToArray();

            decimal factor = CalculateFactor(period);

            decimal sma = Average(newdata);

            result.Add(Math.Round(sma, 2));

            for (int i = 0; i < diff; i++)
            {
                decimal prev = result[result.Count-1];
                decimal price = candles[period + i].close;
                decimal next = factor*(price-prev)+prev;
                result.Add(Math.Round(next, 2));

            }

        }

        decimal Sum(decimal[] data)
        {
            decimal sum = 0;
            foreach (var d in data)
            {
                sum += d;
            }
            return sum;
        }

        decimal Average(decimal[] data)
        {
            if (data.Length == 0)
                return 0;
            return Sum(data) / data.Length;
        }

        private decimal CalculateFactor(int period)
        {
            if (period < 0)
                return 0;
            return 2.0m / (period + 1);
        }
    }
}
