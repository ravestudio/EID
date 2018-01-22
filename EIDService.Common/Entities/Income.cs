using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.Entities
{
    public class Income : Entity<int>
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }

        public string Comment { get; set; }
    }
}
