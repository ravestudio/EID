using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EID.Library;

namespace EIDService.Models
{
    public interface ICloseStrategy
    {
        decimal GetPrice(decimal currentPrice);
        OrderOperationEnum GetOperation();
    }
}
