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
    public class IncomeController : ApiController
    {
        // GET api/deal
        public IEnumerable<Income> Get()
        {
            IEnumerable<Income> items = null;

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                items = unit.IncomeRepository.All<Income>(null).ToList();
            }

            return items;
        }
    }
}
