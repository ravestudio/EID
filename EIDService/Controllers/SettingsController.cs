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
    public class SettingsController : ApiController
    {
        // GET api/settings
        public IEnumerable<Settings> Get()
        {
            IEnumerable<Settings> result = null;

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                result = unit.SettingsRepository.All<EIDService.Common.Entities.Settings>(null).ToList();
            }

            return result;
        }
    }
}
