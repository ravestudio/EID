using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EID.Library;

namespace EIDService.Common.Entities
{
    public class DealGlobal : Entity<int>
    {
        public override string Key
        {
            get { return null; }
            protected set => base.Key = value;
        }

        public decimal Number { get; set; }
        
        public string Code { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }

        public string Operation { get; set; }
        
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal Volume { get; set; }
        
        public OrderOperationEnum OrderOperation { get; set; }
    }
}
