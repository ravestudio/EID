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
        private IList<Signal> signals { get; set; }

        public DemoStrategy()
        {
            signals = new List<Signal>();
        }

        public StrategyDecision GetDecision(IDictionary<string, IList<ICandle>> data, string name, string currentPos, Security security, DateTime CurrentDt)
        {

            StrategyDecision dec = new StrategyDecision() { Decision = null };

            SimpleMovingAverage short_ma = new SimpleMovingAverage(data["5"], 9);
            SimpleMovingAverage long_ma = new SimpleMovingAverage(data["5"], 30);

            MACD macd = new MACD(data["5"], 12, 26, 9);
            TREND macdTrend = new TREND(macd, 2);
            TRENDResult power = macdTrend.GetResult();

            TRENDResult trend = new TREND(long_ma, 3).GetResult();

            AverageTrueRange atr = new AverageTrueRange(data["5"], 14);

            //dec.Profit = Math.Round(atr.Last() * 2m, 2);
            //dec.StopLoss = Math.Round(atr.Last(), 2);

            dec.Profit = Math.Round(data["5"].Last().close * 0.006m, 2);
            dec.StopLoss = Math.Round(data["5"].Last().close * 0.002m, 2);


            decimal vollast = data["5"].Last().volume;
            decimal volprev = data["5"][data["5"].Count - 2].volume;

            bool vol = vollast > volprev*2m;

            //if (power == TRENDResult.StrongUp && new Crossover(short_ma, long_ma).GetResult())
            //{
            //    //dec.Decision = "open long";

            //    //dec.Price = Math.Round(data["5"].Last().close * 1.005m, 2);

            //    signals.Add(new Signal() { Security = name, Action = "open long", DateTime = CurrentDt, Price= data["5"].Last().close });
            //}
            //else
            if (power == TRENDResult.StrongUp)
            {
                //dec.Decision = "open long";

                //dec.Price = Math.Round(data["5"].Last().close * 1.005m, 2);
                signals.Add(new Signal() { Security = name, Action = "open long", DateTime = CurrentDt, Price = data["5"].Last().close });
            }

            if (currentPos == "free")
            {
                DateTime minDt = CurrentDt.AddMinutes(-20);
                DateTime maxDt = CurrentDt.AddMinutes(-15);

                var lastSignals = signals.Where(s => s.Security == name && s.Action == "open long" && s.DateTime < maxDt && s.DateTime >= minDt).ToList();

                Signal signal = lastSignals.FirstOrDefault();

                decimal summ = 0;

                if (signal != null)
                {
                    var candlesToCheck = data["5"].Where(c => c.begin >= signal.DateTime).ToList();

                    foreach(Candle c in candlesToCheck)
                    {
                        summ += c.close - c.open;
                    }
                }


                if (summ > atr.Last()*2m && data["5"].Last().close > signal.Price)
                {
                    dec.Decision = "open long";

                    dec.Price = Math.Round(data["5"].Last().close * 1.005m, 2);
                }
            }



            //if ((power == TRENDResult.Down || power == TRENDResult.StrongDown) && new Crossover(long_ma, short_ma).GetResult() && currentPos == "free")
            //{
            //    dec.Decision = "open short";

            //    dec.Price = Math.Round(data[5].Last().close * 0.995m, 2);
            //}


            if (security.MinStep == 1)
            {
                dec.Profit = Math.Round(dec.Profit);
                dec.StopLoss = Math.Round(dec.StopLoss);
                dec.Price = Math.Round(dec.Price);
            }

            //dec.Decision = "open long";

            return dec;
        }

        public IList<string> Need()
        {
            IList<string> res = new List<string>();

            res.Add("5");

            return res;
        }


    }
}
