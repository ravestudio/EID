using EIDClient.Core.Entities;
using EID.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Repository
{
    public class SecurityDataRepository : Repository<SecurityData, string>
    {
        public SecurityDataRepository(WebApiClient apiClient)
            : base(apiClient)
        {
            this.apiPath = "api/SecurityData/";
        }
    }
}
