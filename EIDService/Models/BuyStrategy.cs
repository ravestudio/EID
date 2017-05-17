using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIDService.Models
{
    public class BuyStrategy : IExecuteStrategy
    {
        public void CalcProfit(Transaction trn, StopOrder order, decimal price)
        {
            if (price <= order.StopPrice && trn.MaxProfit == 0)
            {
                trn.MaxProfit = price;
            }

            if (price < trn.MaxProfit && trn.MaxProfit != 0)
            {
                trn.MaxProfit = price;
            }
        }

        public decimal? GetLossProfit(Transaction trn, decimal price)
        {
            decimal? lossprofit = null;

            if (trn.MaxProfit > 0)
            {
                lossprofit = price - trn.MaxProfit;
            }

            return lossprofit;
        }

        public bool StopLimit(StopOrder order, decimal price)
        {
            return price > order.StopLimitPrice;
        }
    }
}