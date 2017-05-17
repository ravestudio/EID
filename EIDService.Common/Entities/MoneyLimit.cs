using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.Entities
{
    public class MoneyLimit: Entity<int>
    {
        public override string Key
        {
            get { return null; }
            protected set => base.Key = value;
        }

        public string Firm { get; set; }
        public string Currency { get; set; }
        public string MoneyGroup { get; set; }
        public string Client { get; set; }
        public string Type { get; set; }

        public decimal Available { get; set; }

    }
}
