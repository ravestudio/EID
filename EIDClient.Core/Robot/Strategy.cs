using EID.Library;
using EIDClient.Core.Entities;
using EIDClient.Core.ISS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Robot
{
    public class Strategy : IStrategy
    {
        
        public Strategy()
        {

        }

        public IList<string> Need()
        {
            IList<string> res = new List<string>();

            res.Add("5");
            res.Add("60");

            return res;
        }

        public StrategyDecision GetDecision(IDictionary<string, IList<ICandle>> data, string name, string currentPos, Security security, DateTime CurrentDt)
        {
            StrategyDecision dec = new StrategyDecision() { Decision = null };

            ExponentialMovingAverage hours_ema = new ExponentialMovingAverage(data["60"], 9);

            //ExponentialMovingAverage minutes_ema = new ExponentialMovingAverage(data[5], 4);

            MACD macd = new MACD(data["5"], 12, 26, 9);

            TREND hoursTrend = new TREND(hours_ema, 3);
            //TREND minutesTrend = new TREND(minutes_ema, 3);
            TREND macdTrend = new TREND(macd, 2);
            bool valueConfirm = new ValueConfirm(data["5"], 5).GetResult();

            AverageTrueRange atr = new AverageTrueRange(data["5"], 14);

            Extremum extremum = new Extremum(atr, 3, 20);

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

            TRENDResult trend = hoursTrend.GetResult();
            TRENDResult power = macdTrend.GetResult();


            decimal value = data["5"].Last().value; 

            //*if (extremum.Count() > 0 && extremum.Last().ext == "L" && extremum.Last().val < atr.Last())
            if (atr.Last() > 10m && valueConfirm)
            {

                dec.Profit = Math.Round(atr.Last() * 4m, 2);
                dec.StopLoss = Math.Round(atr.Last(), 2);



                if (trend == TRENDResult.Up && (power == TRENDResult.Up || power == TRENDResult.StrongUp) && currentPos == "free")
                {
                    dec.Decision = "open long";
                }

                if (trend == TRENDResult.Flat && power == TRENDResult.StrongUp && currentPos == "free")
                {
                    dec.Decision = "open long";
                }

                if (trend == TRENDResult.Down && (power == TRENDResult.Down || power == TRENDResult.StrongDown) && currentPos == "free")
                {
                    dec.Decision = "open short";
                }

                if (trend == TRENDResult.Flat && power == TRENDResult.StrongDown && currentPos == "free")
                {
                    dec.Decision = "open short";
                }
            }

            if (CurrentDt >= new DateTime(2017, 5, 15, 15, 20, 0))
            {
                //dec = null;
            }

            return dec;
        }
    }
}
