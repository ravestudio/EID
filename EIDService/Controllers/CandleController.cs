﻿using EID.Library;
using EID.Library.ISS;
using EIDClient.Library;
using EIDService.Common;
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
        public IEnumerable<ICandle> Get([FromUri] RequestModel request)
        {
            Logger.InitLogger();

            IDictionary<Func<RequestModel, bool>, Action> actions = new Dictionary<Func<RequestModel, bool>, Action>();

            IEnumerable<ICandle> candles = null;

            Settings settings = null;

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                settings = unit.SettingsRepository.All<Settings>(null).Single();
            }

            actions.Add((pr) => { return !pr.from.HasValue; }, () =>
            {
                EIDService.Common.CandleToISS candleToISS = new Common.CandleToISS();
                using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
                {
                    var tempData = unit.CandleRepository.All<EIDService.Common.Entities.Candle>(null).ToList();
                    candles = tempData.Select(c => candleToISS.Convert(c));

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
                MicexISSClient client = new MicexISSClient(new WebApiClient());

                DateTime? till = null;
                if (settings.ModeType == ModeType.Test)
                {
                    till = settings.TestDateTime;
                }

                IDictionary<string, Func<RequestModel, IList<EID.Library.ICandle>>> interval_actions = new Dictionary<string, Func<RequestModel, IList<EID.Library.ICandle>>>();

                interval_actions.Add("1", (req) =>
                {
                    return client.GetCandles(request.security, request.from.Value, till, request.interval).Result;
                });

                interval_actions.Add("60", (req) =>
                {
                    return client.GetCandles(request.security, request.from.Value, till, request.interval).Result;
                });

                interval_actions.Add("D", (req) =>
                {
                    return client.GetHistory(request.security, request.from.Value, till).Result;
                });


                try
                {
                    candles = interval_actions[request.interval].Invoke(request);

                    Logger.Log.InfoFormat("Данные с ММВБ получены. Count {0}", candles.Count());
                }
                catch(Exception ex)
                {
                    Logger.Log.Error("ошибка", ex);
                }

                
            });

            actions.Single(f => f.Key.Invoke(request)).Value.Invoke();

            return candles;
        }
    }
}
