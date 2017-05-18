using EIDClient.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Messages
{
    public class ShowDataMessage
    {
        public IEnumerable<Deal> Deals { get; set; }
    }
}
