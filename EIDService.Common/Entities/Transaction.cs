using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.Entities
{
    public class Transaction : Entity<int>
    {
        public override string Key
        {
            get { return null; }
            protected set => base.Key = value;
        }

        public string Description { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public decimal OrderNumber { get; set; }

        public bool Processed { get; set; }

        public decimal Profit { get; set; }
        public decimal StopLoss { get; set; }
        public decimal MaxProfit { get; set; } 
    }
}
