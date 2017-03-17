using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.ISS
{
    public class IISResponse
    {
        public IList<SecurityInfo> SecurityInfo { get; set; }
        public IList<MarketData> MarketData { get; set; }
    }
}
