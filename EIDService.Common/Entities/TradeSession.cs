using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.Entities
{
    public class TradeSession : Entity<int>
    {
        public DateTime Date { get; set; }
    }
}
