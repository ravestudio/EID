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
    public class PositionController : ApiController
    {
        // GET api/position
        public IEnumerable<Position> Get()
        {
            IEnumerable<Position> positions = null;

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                positions = unit.PositionRepository.All<Position>(null).ToList();
            }

            return positions;
        }
    }
}
