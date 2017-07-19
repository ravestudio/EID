using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.Entities
{
    public class Security : Entity<int>
    {
        public string Code { get; set; }
        public string Name { get; set; } 

        public Int64 IssueSize { get; set; }

        public virtual Emitent Emitent { get; set; }

        public decimal MinStep { get; set; }
        public int LotSize { get; set; }
        public bool AlgoTrade { get; set; }
    }
}
