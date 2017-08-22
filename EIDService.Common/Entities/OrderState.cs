using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.Entities
{
    public class OrderState: Entity<int>
    {
        public override string Key
        {
            get { return null; }
            protected set => base.Key = value;
        }

        public string Name { get; set; }
    }

    public enum OrderStateEnum : byte
    {
        IsActive = 1,
        Executed = 2,
        Canceled = 3
    }
}
