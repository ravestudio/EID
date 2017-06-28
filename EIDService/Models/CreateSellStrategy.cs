using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EIDService.Common.Entities;

namespace EIDService.Models
{
    public class CreateSellStrategy : ICreateStrategy
    {
        public decimal GetDealPrice(IList<Deal> deals)
        {
            return deals.Select(d => d.Price).Max();
        }

        public string GetOperation()
        {
            return "Продажа";
        }

        public decimal GetPrice(decimal currentPrice, decimal limit)
        {
            return currentPrice - limit * 1.1m;
        }

        public decimal GetStopLimitPrice(decimal currentPrice, decimal limit)
        {
            return currentPrice - limit;
        }

        public decimal GetStopPrice(decimal currentPrice, decimal profit)
        {
            return currentPrice + profit;
        }
    }
}