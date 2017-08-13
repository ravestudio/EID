using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EID.Library;
using EIDClient.Core.Entities;
using EIDClient.Core.ISS;

namespace EIDClient.Core.Robot
{
    public class CandleStrategy : IStrategy
    {
        public StrategyDecision GetDecision(IDictionary<string, IList<ICandle>> data, string name, string currentPos, Security security, DateTime CurrentDt)
        {
            StrategyDecision dec = new StrategyDecision() { Decision = null };

            SimpleMovingAverage hours_ma = new SimpleMovingAverage(data["60"], 30);
            SimpleMovingAverage ma = new SimpleMovingAverage(data["5"], 30);

            TRENDResult trend = new TREND(hours_ma, 3).GetResult();

            ICandle last = data["5"].Last();
            ICandle prev = data["5"][data["5"].Count - 2];

            dec.Profit = Math.Round(last.close * 0.02m, 2);
            dec.StopLoss = Math.Round(last.close * 0.01m, 2);

            decimal p = last.open * 1.003m;

            bool currentGreen = last.close > p;

            bool lastgrow = last.close > prev.close;

            bool cross = last.close > ma.Last()*1.01m;

            if ((trend == TRENDResult.Up || trend == TRENDResult.Flat) && currentGreen && lastgrow && cross && currentPos == "free")
            {
                dec.Decision = "open long";
            }

            return dec;
        }

        public IList<string> Need()
        {
            IList<string> res = new List<string>();

            res.Add("5");
            res.Add("60");

            return res;
        }
    }
}
