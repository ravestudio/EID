using EIDClient.Core.Entities;
using EIDClient.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Repository
{
    public class OrderRepository : Repository<Order, int>
    {
        public OrderRepository(WebApiClient apiClient)
            : base(apiClient)
        {
            this.apiPath = "api/order/";
        }

        public override Task<string> Create(Order model)
        {
            TaskCompletionSource<string> TCS = new TaskCompletionSource<string>();

            string url = string.Format("{0}{1}", this.ServerURL, this.apiPath);

            this._apiClient.PostData(url, model.ConverToKeyValue()).ContinueWith(t =>
            {
                string data = t.Result;

                TCS.SetResult(data);
            });

            return TCS.Task;
        }
    }
}
