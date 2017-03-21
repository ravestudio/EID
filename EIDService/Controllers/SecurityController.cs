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
    public class SecurityController : ApiController
    {

        public Security Get(int Id)
        {
            Security res = null;
            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                res = unit.SecurityRepository.Query<Security>(s => s.Id == Id).SingleOrDefault();
            }

            return res;
        }

        // GET api/security
        public IEnumerable<Security> Get()
        {
            IEnumerable<Security> securities = null;

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                securities = unit.SecurityRepository.All<Security>(null).ToList();
            }

            return securities;
        }
    }
}