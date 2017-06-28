using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Messages
{
    public class CreateOrderMessage
    {
        public string Code { get; set; }
        public string Operation { get; set; }
        public string Account { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public string Class { get; set; }
        public string Client { get; set; }
        public string Comment { get; set; }

        public decimal Profit { get; set; }
        public decimal StopLoss { get; set; }
    }
}
