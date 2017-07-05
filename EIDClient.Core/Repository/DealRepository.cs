using EIDClient.Core.Entities;
using EID.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Repository
{
    public class DealRepository : Repository<Deal, int>
    {
        public DealRepository(WebApiClient apiClient)
            : base(apiClient)
        {
            this.apiPath = "api/deal/";
        }
    }
}
