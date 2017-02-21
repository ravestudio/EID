using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.Entities
{
    public class Emitent : Entity<int>
    {
        public string Name { get; set; }

        public virtual ICollection<Security> Securities { get; set; }
    }
}
