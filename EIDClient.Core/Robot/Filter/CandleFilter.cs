using EID.Library;
using EIDClient.Core.ISS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Robot.Filter
{
    public class CandleFilter: IFilter
    {
        private IDictionary<string, IList<ICandle>> _data = null;

        public CandleFilter(IDictionary<string, IList<ICandle>> data)
        {
            _data = data;
        }

        public FilterResult Exec()
        {
            FilterResult res = FilterResult.N;

            ICandle curr = _data["5"].Last();
            ICandle last = _data["5"][_data["5"].Count - 2];
            ICandle prev = _data["5"][_data["5"].Count - 3];

            if (Func.Green(last) && Func.Big(last) && Func.Green(prev) && curr.close > last.close)
            {
                res = FilterResult.L;
            }

            if (Func.Red(last) && Func.Big(last) && Func.Red(prev) && curr.close < last.close)
            {
                res = FilterResult.S;
            }

            return res;
        }
    }
}
