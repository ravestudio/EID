using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.Entities
{
    public class OrderOperation : Entity<int>
    {
        public override string Key
        {
            get { return null; }
            protected set => base.Key = value;
        }

        public string Name { get; set; }
    }

    public enum OrderOperationEnum: byte
    {
        Buy = 1,
        Sell = 2
    }
}
