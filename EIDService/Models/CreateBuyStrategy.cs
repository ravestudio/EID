using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EID.Library;

namespace EIDService.Models
{
    public class CreateBuyStrategy : ICreateStrategy
    {
        public decimal GetDealPrice(IList<Deal> deals)
        {
            return deals.Select(d => d.Price).Min();
        }

        public OrderOperationEnum GetOperation()
        {
            return OrderOperationEnum.Buy;
        }

        public decimal GetPrice(decimal currentPrice, decimal limit)
        {
            return currentPrice + limit * 1.1m;
        }

        public decimal GetStopLimitPrice(decimal currentPrice, decimal limit)
        {
            return currentPrice + limit;
        }

        public decimal GetStopPrice(decimal currentPrice, decimal profit)
        {
            return currentPrice - profit;
        }
    }
}