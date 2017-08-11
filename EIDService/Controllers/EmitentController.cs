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
    public class EmitentController : ApiController
    {
        // GET api/emitent
        public IEnumerable<Emitent> Get()
        {
            IEnumerable<Emitent> emitents = null;

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                emitents = unit.EmitentRepository.All<Emitent>(null).ToList();
            }

            return emitents;
        }

        public void Post([FromBody]Emitent emitent)
        {
            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                unit.EmitentRepository.Update(emitent);

                unit.Commit();
            }
        }
    }
}
