﻿using EID.Library;
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
        public SimpleMovingAverage(IList<ICandle> candles, int period)
        {
            this.Clear();

            IList<ICandle> temp = candles.OrderBy(c => c.begin).ToList();

            IEnumerable<int> range = Enumerable.Range(0, candles.Count - period);

            var result = range.Select(n => temp.Skip(n).Take(period).Select(c => c.close).Average());

            this.AddRange(result);
        }
    }
}