using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.Entities
{
    public class Security : Entity<int>
    {
        public string Code { get; set; }
        public string Name { get; set; } 

        public int IssueSize { get; set; }

        public virtual Emitent Emitent { get; set; }
    }
}
