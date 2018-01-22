using EID.Library;
using EIDClient.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Repository
{
    public class IncomeRepository : Repository<Income, int>
    {
        public IncomeRepository(WebApiClient apiClient)
            : base(apiClient)
        {
            this.apiPath = "api/income/";
        }
    }
}
