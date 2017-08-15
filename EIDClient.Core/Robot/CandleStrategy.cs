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
            
            //скользящая средняя для 60ти минуток
            SimpleMovingAverage hours_ma = new SimpleMovingAverage(data["60"], 30);
            //скользящая средняя для пятиминуток
            SimpleMovingAverage ma = new SimpleMovingAverage(data["5"], 30);

            //определяем тренд
            TRENDResult trend = new TREND(hours_ma, 3).GetResult();

            ICandle last = data["5"].Last();
            ICandle prev = data["5"][data["5"].Count - 2];

            dec.Code = name;
            dec.LastPrice = last.close;
            dec.Price = last.close;

            dec.Profit = Math.Round(last.close * 0.015m, 2);//профит 1,5%
            dec.StopLoss = Math.Round(last.close * 0.01m, 2);//стоп лосс 2%

            decimal p = last.open * 1.003m;

            //свеча зеленая
            bool currentGreen = last.close > p;

            //выше предыдущей
            bool lastgrow = last.close > prev.close;

            //свеча вышла за сколящую среднюю
            bool cross = last.close > ma.Last()*1.01m;

            if ((trend == TRENDResult.Up || trend == TRENDResult.Flat) && currentGreen && lastgrow && cross && currentPos == "free")
            {
                //покупаем
                dec.Decision = "open long";
                dec.Price = Math.Round(data["5"].Last().close * 1.005m, 2);
            }

            dec.Profit = Optimize(dec.Profit, security.MinStep);
            dec.StopLoss = Optimize(dec.StopLoss, security.MinStep);
            dec.Price = Optimize(dec.Price, security.MinStep);

            return dec;
        }

        public IList<string> Need()
        {
            IList<string> res = new List<string>();

            res.Add("5");
            res.Add("60");

            return res;
        }

        private decimal Optimize(decimal value, decimal step)
        {
            decimal mod = value % step;
            decimal ret = value - mod;


            if (value - ret + mod > step)
            {

                ret = ret + step;
            }

            return ret;
        }

    }
}
