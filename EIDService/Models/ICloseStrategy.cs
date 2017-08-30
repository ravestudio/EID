using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Models
{
    public interface ICloseStrategy
    {
        decimal GetPrice(decimal currentPrice);
        string GetOperation();
    }
}
