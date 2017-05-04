using EIDClient.Core.Entities;
using EIDClient.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Repository
{
    public class PositionRepository : Repository<Position, int>
    {
        public PositionRepository(WebApiClient apiClient)
            : base(apiClient)
        {
            this.apiPath = "api/position/";
        }
    }
}
