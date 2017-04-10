using EID.Library;
using EIDClient.Core.Entities;
using EIDClient.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Repository
{
    public class CandleRepository : Repository<Candle, int>
    {
        public CandleRepository(WebApiClient apiClient)
            : base(apiClient)
        {
            this.apiPath = "api/candle/";
        }

        public Task<IEnumerable<ICandle>> GetHistory(string sec, DateTime from, int interval)
        {
            TaskCompletionSource<IEnumerable<ICandle>> TCS = new TaskCompletionSource<IEnumerable<ICandle>>();

            string url = string.Format("{0}{1}?security={2}&from={3}&interval={4}", this.ServerURL, "api/candle/", sec, from.ToString(System.Globalization.CultureInfo.InvariantCulture), interval);

            this._apiClient.GetData(url).ContinueWith(t =>
            {
                IList<ICandle> List = new List<ICandle>();

                string data = t.Result;

                var array = Windows.Data.Json.JsonValue.Parse(data).GetArray();

                for (int i = 0; i < array.Count; i++)
                {
                    var value = array[i];
                    ICandle candle = this.GetObject(value);
                    List.Add(candle);
                }

                TCS.SetResult(List);
            });

            return TCS.Task;
        }
    }
}
