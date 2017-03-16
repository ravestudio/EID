using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EIDService.Common.DataAccess;
using EIDService.Common.Entities;
using System.Data.Entity;

namespace EIDService.Controllers
{
    public class FinancialController : ApiController
    {
        // GET api/emitent
        public IEnumerable<Financial> Get(int emitentId)
        {
            IEnumerable<Financial> financials = null;

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                financials = unit.FinancialRepository.Query<Financial>(f => f.Emitent.Id == emitentId, null).ToList();
            }

            return financials;
        }

        // POST api/Financial
        public void Post([FromBody]Financial financial)
        {
            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                unit.FinancialRepository.Update(financial);

                unit.Commit();
            }
        }

        // POST api/Financial
        public void Put([FromBody]Financial financial)
        {
            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                financial.Emitent = unit.EmitentRepository.Get().Single(e => e.Id == financial.Emitent.Id);
                unit.FinancialRepository.Create(financial);

                unit.Commit();
            }
        }
    }
}
