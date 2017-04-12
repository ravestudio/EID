using EID.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Library
{
    public class CandlesConverter
    {
        private Func<ICandle> creator = null;

        public CandlesConverter(Func<ICandle> creator)
        {
            this.creator = creator;
        }

        public IList<ICandle> Convert(IList<ICandle> candles, int sourceTimeframe, int TargetTimeframe)
        {
            List<ICandle> result = new List<ICandle>();

            IEnumerable<string> securities = candles.GroupBy(c => c.Code).Select(g => g.Key);

            foreach(string sec in securities)
            {
                var tempdata = candles.Where(c => c.Code == sec).ToList();

                DateTime start = tempdata.First().begin;
                DateTime stop = tempdata.Last().begin;

                int inc = start.Minute % TargetTimeframe;
                if (inc > 0)
                {
                    start = start.AddMinutes(TargetTimeframe - inc);
                }

                while (start <= stop)
                {
                    ICandle candle = CreateCandle(tempdata, start, TargetTimeframe);
                    if (candle != null)
                    {
                        result.Add(candle);
                    }

                    start = start.AddMinutes(TargetTimeframe);
                }
            }
            return result;
        }

        private ICandle CreateCandle(IList<ICandle> candles, DateTime dt, int frame)
        {
            ICandle candle = null;

            DateTime dt2 = dt.AddMinutes(frame);

            var temp = candles.Where(c => (c.begin >= dt) && (c.begin < dt2));

            if (temp.Count() > 0)
            {
                candle = this.creator.Invoke();

                candle.Code = temp.First().Code;
                candle.begin = dt;
                candle.open = temp.First().open;
                candle.close = temp.Last().close;
                candle.high = temp.Max(c => c.high);
            }

            return candle;
        }

    }
}
