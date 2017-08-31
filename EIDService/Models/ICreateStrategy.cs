using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EID.Library;

namespace EIDService.Models
{
    public interface ICreateStrategy
    {
        OrderOperationEnum GetOperation();

        decimal GetDealPrice(IList<Deal> deals);

        decimal GetStopPrice(decimal currentPrice, decimal profit);

        decimal GetStopLimitPrice(decimal currentPrice, decimal limit);

        decimal GetPrice(decimal currentPrice, decimal limit);
    }
}
