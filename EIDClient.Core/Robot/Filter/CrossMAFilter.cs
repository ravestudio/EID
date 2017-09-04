using EID.Library;
using EIDClient.Core.ISS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Robot.Filter
{
    public class CrossMAFilter : IFilter
    {
        private IDictionary<string, IList<ICandle>> _data = null;

        public CrossMAFilter(IDictionary<string, IList<ICandle>> data)
        {
            _data = data;
        }

        public FilterResult Exec()
        {
            FilterResult res = FilterResult.N;
            //скользящая средняя для пятиминуток
            SimpleMovingAverage ma = new SimpleMovingAverage(_data["5"], 30);

            ICandle prev = _data["5"][_data["5"].Count - 2];

            //свеча вышла за сколящую среднюю вверх
            bool crossUp = prev.close > ma.Last() * 1.003m;

            //свеча вышла за сколящую среднюю вниз
            bool crossDown = prev.close < ma.Last() * 0.997m;

            if (crossUp)
            {
                res = FilterResult.L;
            }

            if (crossDown)
            {
                res = FilterResult.S;
            }

            return res;
        }
    }
}
