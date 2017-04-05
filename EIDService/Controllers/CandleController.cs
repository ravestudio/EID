using EIDService.Common.DataAccess;
using EIDService.Common.Entities;
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
        public IEnumerable<Candle> Get()
        {
            IEnumerable<Candle> candles = null;

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                candles = unit.CandleRepository.All<Candle>(null).ToList();
            }

            return candles;
        }
    }
}
