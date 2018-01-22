using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.Entities
{
    public class DealRAW : Entity<int>
    {
        public override string Key
        {
            get { return null; }
            protected set => base.Key = value;
        }

        public decimal Number { get; set; }
        public decimal OrderNumber { get; set; }
        public string Code { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }

        public string Operation { get; set; }
        public string Account { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal Volume { get; set; }
        public string Class { get; set; }
    }
}
