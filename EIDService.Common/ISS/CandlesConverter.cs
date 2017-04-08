using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.ISS
{
    public class CandlesConverter
    {
        public IList<Candle> Convert(IList<Candle> candles, int sourceTimeframe, int TargetTimeframe)
        {
            List<Candle> result = new List<Candle>();

            int interval = TargetTimeframe / sourceTimeframe;

            IEnumerable<string> securities = candles.GroupBy(c => c.Code).Select(g => g.Key);

            foreach(string sec in securities)
            {
                var tempdata = candles.Where(c => c.Code == sec).ToList();

                int range = tempdata.Count / interval + 1;

                IList <Candle> res = Enumerable.Range(0, range).Select(n => CreateCandle(tempdata, n, interval, TargetTimeframe)).ToList();

                foreach (Candle candle in res)
                {
                    if (candle != null)
                    {
                        result.Add(candle);
                    }
                }
            }

            return result;
        }

        private Candle CreateCandle(IList<Candle> candles, int n, int interval, int frame)
        {
            Candle candle = null;
            //var temp = candles.Skip(n*interval).Take(interval);
            DateTime dt = candles.First().begin;

            int inc = dt.Minute % frame;
            if (inc > 0)
            {
                dt = dt.AddMinutes(frame - inc);
            }

            DateTime start = dt.AddMinutes(n * frame);
            DateTime stop = dt.AddMinutes(n * frame + frame);

            var temp = candles.Where(c => (c.begin >= start) && (c.begin < stop));


            if (temp.Count() > 0)
            {
                candle = new Candle()
                {
                    Code = temp.First().Code,
                    begin = temp.First().begin,
                    open = temp.First().open,
                    close = temp.Last().close,
                    high = temp.Max(c => c.high)
                };
            }

            return candle;
        }
    }
}
