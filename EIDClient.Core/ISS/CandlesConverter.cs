﻿using EIDClient.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.ISS
{
    public class CandlesConverter
    {
        public IList<Candle> Convert(IEnumerable<Candle> candles, int sourceTimeframe, int TargetTimeframe)
        {
            IList<Candle> result = null;

            int interval = TargetTimeframe / sourceTimeframe;

            result = Enumerable.Range(0, candles.Count() / interval).Select(n => CreateCandle(candles, n, interval)).ToList();

            return result;
        }

        private Candle CreateCandle(IEnumerable<Candle> candles, int n, int interval)
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
