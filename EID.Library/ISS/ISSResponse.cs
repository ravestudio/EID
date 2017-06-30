using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EID.Library.ISS
{
    public class ISSResponse
    {
        public IList<SecurityInfo> SecurityInfo { get; set; }
        public IList<MarketData> MarketData { get; set; }
    }
}
