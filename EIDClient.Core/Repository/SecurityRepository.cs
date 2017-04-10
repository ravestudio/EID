using EIDClient.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Repository
{
    public class SecurityRepository : Repository<Entities.Security, int>
    {
        public SecurityRepository(WebApiClient apiClient)
            : base(apiClient)
        {
            this.apiPath = "api/security/";

        }
    }
}
