using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EID.Library;

namespace EIDService.Models
{
    public class CloseLongStrategy : ICloseStrategy
    {
        public OrderOperationEnum GetOperation()
        {
            return OrderOperationEnum.Sell;
        }

        public decimal GetPrice(decimal currentPrice)
        {
            return currentPrice * 0.998m;
        }
    }
}