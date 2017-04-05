using EIDClient.Core.ISS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Messages
{
    public class GetCandlesResponseMessage
    {
        public IDictionary<string, IDictionary<int, IList<Candle>>> Сandles { get; set; }
    }
}
