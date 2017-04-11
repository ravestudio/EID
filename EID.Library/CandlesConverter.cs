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

            int interval = TargetTimeframe / sourceTimeframe;

            IEnumerable<string> securities = candles.GroupBy(c => c.Code).Select(g => g.Key);

            foreach(string sec in securities)
            {
                var tempdata = candles.Where(c => c.Code == sec).ToList();

                int range = tempdata.Count / interval + 1;

                IList <ICandle> res = Enumerable.Range(0, range).Select(n => CreateCandle(tempdata, n, interval, TargetTimeframe)).ToList();

                foreach (ICandle candle in res)
                {
                    if (candle != null)
                    {
                        result.Add(candle);
                    }
                }
            }

            return result;
        }

        private ICandle CreateCandle(IList<ICandle> candles, int n, int interval, int frame)
        {
            ICandle candle = null;
            //var temp = candles.Skip(n*interval).Take(interval);
            DateTime dt = candles.First().begin;

            int inc = dt.Minute % frame;
            if (inc > 0)
            {
                dt = dt.AddMinutes(frame - inc);
            }

            int summ = 60 * (dt.Hour - 10) + dt.Minute + n * frame;

            int adddays = summ / (60 * 9);
            int addminutes = summ % (60 * 9);

            DateTime start = dt.AddDays(adddays).AddMinutes(addminutes);
            DateTime stop = dt.AddDays(adddays).AddMinutes(addminutes + frame);

            //DateTime start = dt.AddMinutes(n * frame);
            //DateTime stop = dt.AddMinutes(n * frame + frame);

            var temp = candles.Where(c => (c.begin >= start) && (c.begin < stop));


            if (temp.Count() > 0)
            {
                candle = this.creator.Invoke();

                candle.Code = temp.First().Code;
                candle.begin = start;
                candle.open = temp.First().open;
                candle.close = temp.Last().close;
                candle.high = temp.Max(c => c.high);
            }

            return candle;
        }
    }
}
