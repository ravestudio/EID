using EID.Library;
using EIDClient.Core.ISS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Robot.Filter
{
    public class TrendFilter : IFilter
    {

        private IDictionary<string, IList<ICandle>> _data = null;

        public TrendFilter(IDictionary<string, IList<ICandle>> data)
        {
            _data = data;
        }

        public FilterResult Exec()
        {
            FilterResult res = FilterResult.N;

            //скользящая средняя для 60ти минуток
            SimpleMovingAverage hours_ma = new SimpleMovingAverage(_data["60"], 30);
            //определяем тренд
            TRENDResult trend = new TREND(hours_ma, 3).GetResult();

            if (trend == TRENDResult.Up)
            {
                res = FilterResult.L;
            }

            if (trend == TRENDResult.Down)
            {
                res = FilterResult.S;
            }

            return res;
        }
    }
}
