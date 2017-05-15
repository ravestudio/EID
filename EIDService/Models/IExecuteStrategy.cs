using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIDService.Models
{
    public interface IExecuteStrategy
    {
        void CalcProfit(Transaction trn, StopOrder order, decimal price);

        decimal? GetLossProfit(Transaction trn, decimal price);
    }
}