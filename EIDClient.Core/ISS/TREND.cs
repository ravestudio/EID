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

        public TREND(IList<decimal> data, int period)
        {
            this.result = TRENDResult.Up;

            this._data = data.Skip(data.Count - period).ToList();
        }

        public TRENDResult GetResult()
        {
            decimal d = _data.Last() - _data[0];

            this.result = d > 0 ? TRENDResult.Up : TRENDResult.Down;

            return result;
        }
    }

    public enum TRENDResult
    {
        Up, Down
    }
}
