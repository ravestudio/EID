using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EID.Library;
using EIDClient.Core.ISS;
using EIDClient.Core.Entities;

namespace EIDClient.Core.Robot
{
    public class DemoStrategy : IStrategy
    {
        public StrategyDecision GetDecision(IDictionary<int, IList<ICandle>> data, string name, string currentPos, Security security, DateTime CurrentDt)
        {

            StrategyDecision dec = new StrategyDecision() { Decision = null };

            SimpleMovingAverage short_ma = new SimpleMovingAverage(data[5], 9);
            SimpleMovingAverage long_ma = new SimpleMovingAverage(data[5], 20);

            MACD macd = new MACD(data[5], 12, 26, 9);
            TREND macdTrend = new TREND(macd, 2);
            TRENDResult power = macdTrend.GetResult();

            TRENDResult trend = new TREND(long_ma, 3).GetResult();

            AverageTrueRange atr = new AverageTrueRange(data[5], 14);

            dec.Profit = Math.Round(atr.Last() * 4m, 2);
            dec.StopLoss = Math.Round(atr.Last(), 2);

            if ((power == TRENDResult.Up || power == TRENDResult.StrongUp) && new Crossover(short_ma, long_ma).GetResult() && currentPos == "free")
            {
                dec.Decision = "open long";

                dec.Price = Math.Round(data[5].Last().close * 1.005m, 2);
            }

            if ((power == TRENDResult.Down || power == TRENDResult.StrongDown) && new Crossover(long_ma, short_ma).GetResult() && currentPos == "free")
            {
                dec.Decision = "open short";

                dec.Price = Math.Round(data[5].Last().close * 0.995m, 2);
            }
            

            if (security.MinStep == 1)
            {
                dec.Profit = Math.Round(dec.Profit);
                dec.StopLoss = Math.Round(dec.StopLoss);
                dec.Price = Math.Round(dec.Price);
            }

            //dec.Decision = "open long";

            return dec;
        }

        public IList<int> Need()
        {
            IList<int> res = new List<int>();

            res.Add(5);

            return res;
        }
    }
}
