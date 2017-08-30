using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIDService.Models
{
    public class CloseLongStrategy : ICloseStrategy
    {
        public string GetOperation()
        {
            return "Продажа";
        }

        public decimal GetPrice(decimal currentPrice)
        {
            return currentPrice * 0.998m;
        }
    }
}