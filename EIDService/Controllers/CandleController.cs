using EID.Library;
using EID.Library.ISS;
using EIDClient.Library;
using EIDService.Common.DataAccess;
using EIDService.Common.Entities;
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
        public IEnumerable<ICandle> Get([FromUri] CandleRequestModel request)
        {
            IDictionary<Func<CandleRequestModel, bool>, Action> actions = new Dictionary<Func<CandleRequestModel, bool>, Action>();

            IEnumerable<ICandle> candles = null;

            Settings settings = null;

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                settings = unit.SettingsRepository.All<Settings>(null).Single();
            }

            actions.Add((pr) => { return !pr.from.HasValue; }, () =>
            {
                using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
                {
                    var tempData = unit.CandleRepository.All<EIDService.Common.Entities.Candle>(null).ToList();
                    candles = tempData.Select(c => CandleToISS(c));

                    if (settings.ModeType == ModeType.Test)
                    {
                        candles = candles.Where(c => c.begin < settings.TestDateTime);
                        settings.TestDateTime = settings.TestDateTime.AddMinutes(1);

                        unit.SettingsRepository.Update(settings);
                        unit.Commit();

                        CandlesConverter converter = new CandlesConverter(() => { return new EID.Library.ISS.Candle(); });
                        candles = converter.Convert(candles.ToList(), 1, 5);
                    }
                }
            });

            actions.Add((pr) => { return !string.IsNullOrEmpty(pr.security) && pr.from.HasValue; }, () =>
            {
                DateTime? till = null;
                if (settings.ModeType == ModeType.Test)
                {
                    till = settings.TestDateTime;
                }

                 MicexISSClient client = new MicexISSClient(new WebApiClient());

                candles = client.GetCandles(request.security, request.from.Value, till, request.interval.Value).Result;
            });

            actions.Single(f => f.Key.Invoke(request)).Value.Invoke();

            return candles;
        }

        private EID.Library.ISS.Candle CandleToISS(EIDService.Common.Entities.Candle c)
        {
            EID.Library.ISS.Candle candle = new EID.Library.ISS.Candle();

            candle.Code = c.Code;

            int year = int.Parse(c.CandleDate.Substring(0, 4));
            int moth = int.Parse(c.CandleDate.Substring(4, 2));
            int day = int.Parse(c.CandleDate.Substring(6, 2));

            int hour = int.Parse(c.CandleTime.Substring(0, 2));
            int minute = int.Parse(c.CandleTime.Substring(2, 2));

            candle.begin = new DateTime(year, moth, day, hour, minute, 0);
            candle.open = c.OpenPrice;
            candle.close = c.ClosePrice;
            candle.high = c.MaxPrice;
            candle.low = c.MinPrice;
            candle.value = c.Value;
            candle.volume = c.Volume;

            return candle;
        }
    }
}
