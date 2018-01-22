using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.Entities
{
    public class PortfolioItem : Entity<int>
    {
        public string Code { get; set; }
        public int Count { get; set; }

        public string Account { get; set; }
    }
}
