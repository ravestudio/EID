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
    public class DealController : ApiController
    {
        // GET api/deal
        public IEnumerable<Deal> Get()
        {
            IEnumerable<Deal> deals = null;

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                deals = unit.DealRepository.All<Deal>(null).ToList();
            }

            return deals;
        }
    }
}
