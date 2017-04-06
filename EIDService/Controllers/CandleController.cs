using EIDClient.Library;
using EIDService.Common.DataAccess;
using EIDService.Common.Entities;
using EIDService.Common.ISS;
using EIDService.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EIDService.Controllers
{
    public class CandleController : ApiController
    {
        // GET api/candle
        public IEnumerable<EIDService.Common.ISS.Candle> Get(CandleRequestModel request)
        {
            IDictionary<Func<CandleRequestModel, bool>, Action> actions = new Dictionary<Func<CandleRequestModel, bool>, Action>();

            IEnumerable<EIDService.Common.ISS.Candle> candles = null;

            actions.Add((pr) => { return pr == null || !pr.from.HasValue; }, () =>
            {
                using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
                {
                    var tempData = unit.CandleRepository.All<EIDService.Common.Entities.Candle>(null).ToList();
                    candles = tempData.Select(c => new EIDService.Common.ISS.Candle(c));
                }
            });

            actions.Add((pr) => { return pr != null && !string.IsNullOrEmpty(pr.security) && pr.from.HasValue; }, () =>
            {
                MicexISSClient client = new MicexISSClient(new WebApiClient());

                candles = client.GetCandles(request.security, request.from.Value, request.interval.Value).Result;
            });

            actions.Single(f => f.Key.Invoke(request)).Value.Invoke();

            return candles;
        }
    }
}
