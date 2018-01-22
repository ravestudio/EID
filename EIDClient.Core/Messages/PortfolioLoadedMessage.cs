using EIDClient.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Messages
{
    public class PortfolioLoadedMessage
    {
        public IEnumerable<PortfolioItem> PortfolioItems { get; set; }

        public decimal PortfolioPrice { get; set; }
        public decimal IncomeTotal { get; set; }
    }
}
