using EID.Library;
using EIDClient.Core.ISS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Robot.Filter
{
    public class LocalOffsetFilter : IFilter
    {
        private IDictionary<string, IList<ICandle>> _data = null;

        public LocalOffsetFilter(IDictionary<string, IList<ICandle>> data)
        {
            _data = data;
        }

        public FilterResult Exec()
        {
            FilterResult res = FilterResult.N;

            ICandle curr = _data["5"].Last();

            decimal day_max = Func.Max(_data["60"], 20, 1);
            decimal day_min = Func.Min(_data["60"], 20, 1);

            decimal max_offset = curr.close / day_max;

            decimal min_offset = day_min / curr.close;

            if (max_offset > 1m)
            {
                res = FilterResult.L;
            }

            if (min_offset > 1m)
            {
                res = FilterResult.S;
            }

            return res;

        }
    }
}
