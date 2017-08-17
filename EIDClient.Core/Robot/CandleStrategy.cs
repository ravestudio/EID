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
            dec.LongPrice = Math.Round(data["5"].Last().close * 1.005m, 2);
            dec.ShortPrice = Math.Round(data["5"].Last().close * 0.995m, 2);

            dec.Profit = Math.Round(last.close * 0.015m, 2);//профит 1,5%
            dec.StopLoss = Math.Round(last.close * 0.01m, 2);//стоп лосс 1%

            decimal g = last.open * 1.003m;
            decimal r = last.open * 0.997m;

            //свеча зеленая
            bool currentGreen = last.close > g;
            //свеча зеленая
            bool currentRed = last.close < r;

            //выше предыдущей
            bool lastgrow = last.close > prev.close;
            //ниже предыдущей
            bool lastdecrease = last.close < prev.close;

            //свеча вышла за сколящую среднюю вверх
            bool crossUp = last.close > ma.Last()*1.01m;

            //свеча вышла за сколящую среднюю вниз
            bool crossDown = last.close < ma.Last() * 0.99m;

            if ((trend == TRENDResult.Up || trend == TRENDResult.Flat) && currentGreen && lastgrow && crossUp && currentPos == "free")
            {
                //покупаем
                dec.Decision = "open long";
            }

            if ((trend == TRENDResult.Down || trend == TRENDResult.Flat) && currentRed && lastdecrease && crossDown && currentPos == "free")
            {
                //продаем
                dec.Decision = "open short";
            }

            MathFunctions func = new MathFunctions();

            dec.Profit = func.Optimize(dec.Profit, security.MinStep);
            dec.StopLoss = func.Optimize(dec.StopLoss, security.MinStep);
            dec.LongPrice = func.Optimize(dec.LongPrice, security.MinStep);
            dec.ShortPrice = func.Optimize(dec.ShortPrice, security.MinStep);

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
