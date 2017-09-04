using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EID.Library;
using EIDClient.Core.Entities;
using EIDClient.Core.ISS;
using EIDClient.Core.Robot.Filter;

namespace EIDClient.Core.Robot
{
    public class CandleStrategy : IStrategy
    {
        public StrategyDecision GetDecision(IDictionary<string, IList<ICandle>> data, string name, string currentPos, Security security, DateTime CurrentDt)
        {
            StrategyDecision dec = new StrategyDecision() { Decision = null };

            FilterResult local_offset = new LocalOffsetFilter(data).Exec();
            FilterResult trend = new TrendFilter(data).Exec();
            FilterResult candlePattern = new CandleFilter(data).Exec();
            FilterResult cross_ma = new CrossMAFilter(data).Exec();

            ICandle curr = data["5"].Last();

            dec.Code = name;
            dec.LastPrice = curr.close;
            dec.LongPrice = Math.Round(curr.close * 1.005m, 2);
            dec.ShortPrice = Math.Round(curr.close * 0.995m, 2);

            dec.Profit = Math.Round(curr.close * 0.015m, 2);//профит 1,5%
            dec.StopLoss = Math.Round(curr.close * 0.01m, 2);//стоп лосс 1,0%

            IList<FilterResult> filterResult_list = new List<FilterResult>();


            if (local_offset == FilterResult.L && (trend == FilterResult.L || trend == FilterResult.N) && candlePattern == FilterResult.L && cross_ma == FilterResult.L && currentPos == "free")
            {
                //покупаем
                dec.Decision = "open long";
            }

            if (local_offset == FilterResult.S && (trend == FilterResult.S || trend == FilterResult.N) && candlePattern == FilterResult.S && cross_ma == FilterResult.S && currentPos == "free")
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
