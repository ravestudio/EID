using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.ISS
{
    public class ExponentialMovingAverage : List<decimal>
    {
        public ExponentialMovingAverage(IList<decimal> values, int period)
        {
            Calculate(values.ToArray(), period);
        }

        public ExponentialMovingAverage(IList<Candle> candles, int period)
        {
            Calculate(candles.Select(c => c.close).ToArray(), period);
        }
        

        void Calculate(decimal[] data, int period)
        {
            this.Clear();

            int diff = data.Length - period;

            decimal[] newdata = data.Take(period).ToArray();

            decimal factor = CalculateFactor(period);

            decimal sma = Average(newdata);

            this.Add(Math.Round(sma, 2));

            for (int i = 0; i < diff; i++)
            {
                decimal prev = this[this.Count - 1];
                decimal price = data[period + i];
                decimal next = factor * (price - prev) + prev;
                this.Add(Math.Round(next, 2));
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
