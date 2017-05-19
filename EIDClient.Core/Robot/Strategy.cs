using EID.Library;
using EIDClient.Core.Entities;
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

        public string GetDecision(IDictionary<int, IList<ICandle>> data, string name, string currentPos, DateTime CurrentDt)
        {
            string dec = null;

            if (CurrentDt >= new DateTime(2017, 5, 15, 15,25, 0))
            {

            }

            ExponentialMovingAverage hours_ema = new ExponentialMovingAverage(data[60], 9);

            //ExponentialMovingAverage minutes_ema = new ExponentialMovingAverage(data[5], 4);

            MACD macd = new MACD(data[5], 12, 26, 9);

            TREND hoursTrend = new TREND(hours_ema, 5);
            //TREND minutesTrend = new TREND(minutes_ema, 3);
            TREND macdTrend = new TREND(macd, 2);

            //hoursTrend.GetResult();
            //macdTrend.GetResult();

            //if ((hoursTrend.GetResult() == TRENDResult.Up && macdTrend.GetResult() == TRENDResult.Up || macdTrend.GetResult() == TRENDResult.StrongUp) && currentPos == "free")
            //{
            //    dec = "open long";
            //}

            //if ((hoursTrend.GetResult() == TRENDResult.Down && macdTrend.GetResult() == TRENDResult.Down || macdTrend.GetResult() == TRENDResult.StrongDown) && currentPos == "free")
            //{
            //    dec = "open short";
            //}


            if (macdTrend.GetResult() == TRENDResult.Up && currentPos == "free")
            {
                dec = "open long";
            }

            //if (macdTrend.GetResult() == TRENDResult.StrongDown && currentPos == "free")
            //{
            //    dec = "open short";
            //}

            return dec;
        }
    }
}
