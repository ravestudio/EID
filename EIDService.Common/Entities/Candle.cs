using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.Entities
{
    public class Candle : Entity<int>
    {
        public override string Key
        {
            get { return null; }
            protected set => base.Key = value;
        }

        public string Code { get; set; }
        public string CandleDate { get; set; }
        public string CandleTime { get; set; }

        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
        public decimal Volume { get; set; }
    }
}
