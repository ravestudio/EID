using EIDClient.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Repository
{
    public class TradeSessionRepository : Repository<TradeSession, int>
    {
        public TradeSessionRepository(EIDClient.Core.WebApiClient apiClient)
            : base(apiClient)
        {
            this.apiPath = "api/tradeSession/";
        }
    }
}
