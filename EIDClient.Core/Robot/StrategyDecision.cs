using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Robot
{
    public class StrategyDecision
    {
        public string Decision { get; set; }
        public decimal Profit { get; set; }
        public decimal StopLoss { get; set; }
    }
}
