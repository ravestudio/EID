using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIDService.Models
{
    public class CreateBuyStrategy : ICreateStrategy
    {
        public decimal GetDealPrice(IList<Deal> deals)
        {
            return deals.Select(d => d.Price).Min();
        }

        public string GetOperation()
        {
            return "Покупка";
        }

        public decimal GetPrice(decimal currentPrice, decimal limit)
        {
            return currentPrice + (limit + 0.1m) * currentPrice / 100m;
        }

        public decimal GetStopLimitPrice(decimal currentPrice, decimal limit)
        {
            return currentPrice + limit * currentPrice / 100m;
        }

        public decimal GetStopPrice(decimal currentPrice, decimal profit)
        {
            return currentPrice - profit * currentPrice / 100m;
        }
    }
}