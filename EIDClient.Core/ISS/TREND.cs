using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.ISS
{
    public class TREND
    {
        private TRENDResult result;

        private IList<decimal> _data = null;

        private string type = string.Empty;

        public TREND(IList<decimal> data, int period)
        {
            this.result = TRENDResult.Up;

            this.type = "simple";

            this._data = data.Skip(data.Count - period).ToList();
        }

        public TREND(IList<MACDItem> data, int period)
        {
            this.result = TRENDResult.Up;

            this.type = "macd";

            this._data = data.Skip(data.Count - period).Select(d => d.Histogram).ToList();
        }

        public TRENDResult GetResult()
        {
            if (this.type == "simple")
            {
                decimal d = _data.Last() - _data[0];

                this.result = d > 0 ? TRENDResult.Up : TRENDResult.Down;
            }

            if (this.type == "macd")
            {
                decimal d = GetDiff(_data[0], _data.Last());

                if (d > 30m)
                {
                    this.result = TRENDResult.Up;
                };

                if (d < 30m)
                { 
                    this.result = TRENDResult.Down;
                }
            }

            return result;
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

    public enum TRENDResult
    {
        Up, Down
    }
}
