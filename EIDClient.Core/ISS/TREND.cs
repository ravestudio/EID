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
            this.type = "simple";

            this._data = data.Skip(data.Count - period).ToList();
        }

        public TREND(IList<MACDItem> data, int period)
        {
            this.type = "macd";

            this._data = data.Skip(data.Count - period).Select(d => d.Histogram).ToList();
        }

        public TRENDResult GetResult()
        {
            this.result = TRENDResult.Flat;

            if (this.type == "simple")
            {
                //decimal d = _data.Last() - _data[0];

                decimal d = GetDiff(_data[0], _data.Last());

                if (d > 0.15m)
                {
                    this.result = TRENDResult.Up;
                }

                if (d < -0.15m)
                {
                    this.result = TRENDResult.Down;
                }

            }
            
            if (this.type == "macd")
            {
                decimal d = GetDiff(_data[0], _data.Last());

                //if (Math.Abs(_data.Last()) < 1m && Math.Abs(_data[0]) < 1m)
                //{
                //    d = 0m;
                //}

                if (d > 10m && d <= 70m)
                {
                    this.result = TRENDResult.Up;
                };

                if (d < -10m && d >= -70m)
                { 
                    this.result = TRENDResult.Down;
                }

                if (d > 70m)
                {
                    this.result = TRENDResult.StrongUp;
                };

                if (d < -70m)
                {
                    this.result = TRENDResult.StrongDown;
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
        Flat,
        Up,
        Down,
        StrongUp,
        StrongDown
    }
}
