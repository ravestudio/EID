using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EIDService.Common.Entities;

namespace EIDService.Models
{
    public class SellStrategy : IExecuteStrategy
    {
        public void CalcProfit(Transaction trn, StopOrder order, decimal price)
        {
            if (price >= order.StopPrice && trn.MaxProfit == 0)
            {
                trn.MaxProfit = price;
            }

            if (price > trn.MaxProfit && trn.MaxProfit != 0)
            {
                trn.MaxProfit = price;
            }
        }

        public decimal? GetLossProfit(Transaction trn, decimal price)
        {
            decimal? lossprofit = null;

            if (trn.MaxProfit > 0)
            {
                lossprofit = trn.MaxProfit - price;
            }

            return lossprofit;
        }
    }
}