using EID.Library;
using EIDClient.Core.Entities;
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
        public DateTime DateTime { get; set; }
        public IDictionary<string, IDictionary<int, IList<ICandle>>> Сandles { get; set; }
        public IDictionary<string, string> Positions { get; set; }
    }
}
