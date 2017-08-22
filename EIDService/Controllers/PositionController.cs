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
                IList<string> code_list = unit.SecurityRepository.Query<Security>(s => s.AlgoTrade).Select(s => s.Code).ToList();

                positions = unit.PositionRepository.Query<Position>(p => code_list.Contains(p.Code) && p.PosType == PosTypeEnum.T2).ToList();
            }

            return positions;
        }
    }
}
