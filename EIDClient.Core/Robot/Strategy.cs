using EIDClient.Core.ISS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot
{
    public class Strategy
    {
        
        public Strategy()
        {

        }

        public IList<int> Need()
        {
            IList<int> res = new List<int>();

            res.Add(5);
            res.Add(60);

            return res;
        }

        public string GetDecision(IDictionary<int, IList<Candle>> data, string name, string currentPos)
        {
            string dec = null;

            ExponentialMovingAverage hours_ema = new ExponentialMovingAverage(data[60], 9);

            ExponentialMovingAverage minutes_ema = new ExponentialMovingAverage(data[5], 4);

            TREND hoursTrend = new TREND(hours_ema, 5);
            TREND minutesTrend = new TREND(minutes_ema, 3);

            if (hoursTrend.GetResult() == TRENDResult.Up && minutesTrend.GetResult() == TRENDResult.Up && currentPos == "free")
            {
                dec = "open long";
            }

            return dec;
        }
    }
}
