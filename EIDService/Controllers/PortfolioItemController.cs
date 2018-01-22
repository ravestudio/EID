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
    public class PortfolioItemController : ApiController
    {
        // GET api/deal
        public IEnumerable<PortfolioItem> Get()
        {
            IEnumerable<PortfolioItem> items = null;

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                items = unit.PorfolioItemRepository.All<PortfolioItem>(null).ToList();
            }

            return items;
        }
    }
}
