using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.Entities
{
    public class DealGlobalRAW : Entity<decimal>
    {
        public override string Key
        {
            get { return null; }
            protected set => base.Key = value;
        }

        public string Code { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }

        public string Operation { get; set; }

        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal Volume { get; set; }
    }
}
