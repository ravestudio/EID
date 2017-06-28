using EID.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.ISS
{
    public class ValueConfirm
    {
        private IList<decimal> _data = null;

        public ValueConfirm(IList<ICandle> candles, int period)
        {
            _data = candles.Skip(candles.Count - period).Select(c => c.value).ToList();
        }

        public bool GetResult()
        {
            decimal v1 = _data[0];
            decimal v2 = _data.Last();

            return v2 > 30000000m && GetDiff(v1, v2) > 0.5m;
        }

        private decimal GetDiff(decimal v1, decimal v2)
        {
            if (v1 == 0m) { v1 = 0.01m; }

            decimal diff = v2 - v1;
            decimal prc = Math.Abs(v1) / 100;
            decimal d = diff / prc;
            return d;
        }
    }
}
