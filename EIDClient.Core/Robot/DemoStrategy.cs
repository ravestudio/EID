using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EID.Library;
using EIDClient.Core.ISS;

namespace EIDClient.Core.Robot
{
    public class DemoStrategy : IStrategy
    {
        public StrategyDecision GetDecision(IDictionary<int, IList<ICandle>> data, string name, string currentPos, DateTime CurrentDt)
        {

            StrategyDecision dec = new StrategyDecision() { Decision = null };

            SimpleMovingAverage short_ma = new SimpleMovingAverage(data[5], 9);
            SimpleMovingAverage long_ma = new SimpleMovingAverage(data[5], 20);

            TRENDResult trend = new TREND(long_ma, 3).GetResult();

            AverageTrueRange atr = new AverageTrueRange(data[5], 14);

            dec.Profit = Math.Round(atr.Last() * 4m, 2);
            dec.StopLoss = Math.Round(atr.Last(), 2);

            dec.Price = Math.Round(data[5].Last().close*1.005m);

            //if (trend == TRENDResult.Up && new Crossover(short_ma, long_ma).GetResult())
            //{
            //    dec.Decision = "open long";
            //}

            //if (trend == TRENDResult.Down && new Crossover(long_ma, short_ma).GetResult())
            //{
            //    dec.Decision = "open short";
            //}

            dec.Decision = "open long";

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
