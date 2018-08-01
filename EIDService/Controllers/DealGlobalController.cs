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

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                deals = unit.DealGlobalRepository.Query<DealGlobal>(d => d.Code == request.security && d.DateTime > request.from.Value && d.DateTime < request.till.Value && d.Operation == request.operation).ToList();
            }

            IList<DateTime> frames = new List<DateTime>();

            while(request.from.Value < request.till.Value)
            {
                frames.Add(request.from.Value);
                request.from = request.from.Value.AddDays(1);
            }

            //var groups = frames.Select(f => deals.Where(d => d.DateTime > f && d.DateTime < f.AddDays(1))
            //.GroupBy(d => new { d.Code, d.Operation })
            //.Select(gr => new DealGlobalGroup() { Code = gr.Key.Code, Operation = gr.Key.Operation, Volume = gr.Sum(g => g.Volume) })).ToList();

            var groups = frames.Select(f => new DealGlobalGroup()
            {
                Code = request.security,
                Operation = request.operation,
                Volume = deals.Where(d => d.DateTime > f && d.DateTime < f.AddDays(1)).Sum(gr => gr.Volume)
            }).ToList();


            return groups;
        }
    }
}
