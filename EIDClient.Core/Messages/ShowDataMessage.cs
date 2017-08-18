using EID.Library;
using EIDClient.Core.Entities;
using EIDClient.Core.Robot;
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
        public IEnumerable<Position> Positions { get; set; }

        public IDictionary<string, IDictionary<string, IList<ICandle>>> Сandles { get; set; }

        public IList<AnalystData> AnalystDatalist { get; set; }
    }
}
