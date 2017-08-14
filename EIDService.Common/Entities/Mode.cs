using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.Entities
{
    public class Mode : Entity<int>
    {
        public override string Key
        {
            get { return null; }
            protected set => base.Key = value;
        }

        public string Firm { get; set; }
        public string Account { get; set; }
        public string Class { get; set; }
        public string Client { get; set; }

        public bool Active { get; set; }
    }
}
