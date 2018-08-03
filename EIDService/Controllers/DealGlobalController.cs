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
    public class DealGlobalController : ApiController
    {
        // GET api/deal
        public IEnumerable<DealGlobalGroup> Get([FromUri] DealGlobalRequestModel request)
        {
            IEnumerable<DealGlobal> deals = null;

            IEnumerable<TradeSession> sessions = null;

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                var query = unit.DealGlobalRepository.Query<DealGlobal>(d => d.Code == request.security && d.DateTime > request.from.Value && d.DateTime < request.till.Value && d.Operation == request.operation);

                if (request.ValueFilter.HasValue)
                {
                    query = query.Where(d => d.Volume >= request.ValueFilter);
                }

                deals = query.ToList();

                sessions = unit.TradeSessionRepository.Query<TradeSession>(s => s.Date >= request.from.Value && s.Date < request.till.Value).ToList();
            }

            IList<DateTime> frames = new List<DateTime>();

            int frameMinutes = 0;

            if (request.interval == "60")
            {
                frameMinutes = 60;
            }

            if (request.interval == "D")
            {
                frameMinutes = 60 * 24;
            }

            foreach (TradeSession ts in sessions)
            {
                DateTime start = ts.Date.AddHours(10);
                DateTime stop = ts.Date.AddHours(18).AddMinutes(45);

                while (start < stop)
                {
                    frames.Add(start);

                    start = start.AddMinutes(frameMinutes); 
                }
            }

            //var groups = frames.Select(f => deals.Where(d => d.DateTime > f && d.DateTime < f.AddDays(1))
            //.GroupBy(d => new { d.Code, d.Operation })
            //.Select(gr => new DealGlobalGroup() { Code = gr.Key.Code, Operation = gr.Key.Operation, Volume = gr.Sum(g => g.Volume) })).ToList();

            var groups = frames.Select(f => new DealGlobalGroup()
            {
                dateTime = f,
                volume = deals.Where(d => d.DateTime >= f && d.DateTime < f.AddMinutes(frameMinutes)).Sum(gr => gr.Volume)
            }).ToList();


            return groups;
        }
    }
}
