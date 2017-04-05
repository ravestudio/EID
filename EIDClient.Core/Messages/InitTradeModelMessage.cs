using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Messages
{
    public class InitTradeModelMessage
    {
        public IList<string> securities { get; set; }
        public IList<int> frames { get; set; }
    }
}
