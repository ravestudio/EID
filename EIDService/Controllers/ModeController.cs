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
    public class ModeController : ApiController
    {
        // GET api/deal
        public Mode Get()
        {
            Mode mode = null;

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                mode = unit.ModeRepository.Query<Mode>(m => m.Active, null).Single();
            }

            return mode;
        }
    }
}
