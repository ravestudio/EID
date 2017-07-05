using EIDClient.Core.Entities;
using EID.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Repository
{
    public class TradeSessionRepository : Repository<TradeSession, int>
    {
        public TradeSessionRepository(WebApiClient apiClient)
            : base(apiClient)
        {
            this.apiPath = "api/tradeSession/";
        }
    }
}
