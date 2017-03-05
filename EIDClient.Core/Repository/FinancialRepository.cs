using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EIDClient.Core.Entities;

namespace EIDClient.Core.Repository
{
    public class FinancialRepository : Repository<Entities.Financial, int>
    {
        public FinancialRepository(EIDClient.Core.WebApiClient apiClient)
            : base(apiClient)
        {

        }

        public Task<IEnumerable<Financial>> GetByEmitentId(int EmitentId)
        {
            TaskCompletionSource<IEnumerable<Entities.Financial>> TCS = new TaskCompletionSource<IEnumerable<Entities.Financial>>();

            IList<Entities.Financial> res_list = new List<Entities.Financial>();

            string url = string.Format("{0}{1}?EmitentId={2}", this.ServerURL, "api/financial/", EmitentId);

            this._apiClient.GetData(url).ContinueWith(t =>
            {
                string data = t.Result;

                var array = Windows.Data.Json.JsonValue.Parse(data).GetArray();

                for (int i = 0; i < array.Count; i++)
                {
                    var value = array[i];
                    Entities.Financial financial = this.GetObject(value);
                    res_list.Add(financial);
                }


                TCS.SetResult(res_list);
            });

            return TCS.Task;
        }
    }
}
