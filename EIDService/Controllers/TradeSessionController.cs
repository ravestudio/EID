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
    public class TradeSessionController : ApiController
    {
        // GET api/tradeSession
        public IEnumerable<TradeSession> Get()
        {
            IEnumerable<TradeSession> tradeSessions = null;

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                tradeSessions = unit.TradeSessionRepository.All<TradeSession>(null).ToList();
            }

            return tradeSessions;
        }
    }
}
